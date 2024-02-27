using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Detail.DocumentLine;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header.Document;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using Exxis.Addon.HojadeRutaAGuia.Data.Repository;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Implements.DocumentRepository
{
    // ReSharper disable once InconsistentNaming
    public class ORDRDocumentRepository : BaseSAPDocumentRepository<ORDR, RDR1>
    {
        public ORDRDocumentRepository(SAPbobsCOM.Company company) : base(company)
        {
        }

        public override IEnumerable<ORDR> RetrieveDocuments(Expression<Func<ORDR, bool>> expression)
        {
            IEnumerable<ORDR> documents = base.RetrieveDocuments(expression)
                .ToList();
            var unitOfWork = new UnitOfWork(Company);
            foreach (ORDR document in documents)
            {
                RDR1 firstLine = document.DocumentLines.First();
                OITM item = unitOfWork.ItemsRepository.GetItem(firstLine.ItemCode);
                document.IsService = item.IsInventory == false;
                document.IsItem = item.IsInventory;
            }

            return documents;
        }

        #region Overrides of BaseSAPDocumentRepository<ORDR,RDR1>
        
        //Comments: Es correcto que en carga de Archivos EDI exista el registro extemporaneo?
        public override Tuple<bool, string> RegisterDocument(ORDR x)
        {
            SAPbobsCOM.Recordset recordSet = null;
            SAPbobsCOM.Documents saleOrder = null;
            SAPbobsCOM.Document_Lines saleOrderLines = null;
            SAPbobsCOM.UserFields userFields = null;
            SAPbobsCOM.Fields fields = null;
            SAPbobsCOM.Field field = null;

            try
            {
                recordSet = Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset).To<SAPbobsCOM.Recordset>();
                saleOrder = Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders).To<SAPbobsCOM.Documents>();


                saleOrder.CardCode = x.CardCode;
                saleOrder.DocDate = x.DocumentDate;
                saleOrder.DocDueDate = x.DocumentDeliveryDate;
                saleOrder.Comments = @"[Addon Distribución] Creado por la Carga de Archivos EDI";
                saleOrder.ShipToCode = x.ShippingAddressCode;

                //La validacion de territorio es sobre el punto de entrega x socio de negocio, mas no por el socio de negocio solamente
                if (string.IsNullOrWhiteSpace(x.Region))
                {
                    var query = $"CALL VS_SP_LP_OBTENERTERRITORIO('{x.CardCode}')";
                    recordSet.DoQuery(query);
                    if (!recordSet.EoF)
                    {
                        fields = recordSet.Fields;
                        field = fields.Item(0);
                        x.Region = field.Value?.ToString() ?? string.Empty;
                    }
                    else throw new InvalidOperationException($"No se obtuvo Territorio para el socio de negocio '{x.CardCode}'");
                }

                if (string.IsNullOrWhiteSpace(x.SaleChannel))
                {
                    var query = $"select \"U_CL_CANAL\" from OCRD where \"CardCode\" = '{x.CardCode}' and ifnull(\"U_CL_CANAL\",'') <> ''";
                    recordSet.DoQuery(query);
                    if (!recordSet.EoF)
                    {
                        fields = recordSet.Fields;
                        field = fields.Item(0);
                        x.SaleChannel = field.Value?.ToString() ?? string.Empty;
                    }
                    else throw new InvalidOperationException($"No se obtuvo Canal de Venta para el socio de negocio '{x.CardCode}'");
                }


                userFields = saleOrder.UserFields;
                fields = userFields.Fields;

                IEnumerable<PropertyInfo> customSAPProperties = x.GetProperties()
                    .Where(t => !GenericHelper.IsDefaultValue(t.PropertyType, t.GetValue(x)) &&
                                t.GetCustomAttribute<SAPColumnAttribute>()?.SystemField == false);

                foreach (PropertyInfo customSAPProperty in customSAPProperties)
                {
                    string customFieldSAPName = customSAPProperty.GetCustomAttribute<SAPColumnAttribute>().Name;
                    object customFieldSAPValue = customSAPProperty.GetValue(x);
                    field = fields.Item(customFieldSAPName);
                    if (customFieldSAPValue is decimal)
                        customFieldSAPValue = customFieldSAPValue.ToDouble();
                    field.Value = customFieldSAPValue;
                }

                saleOrderLines = saleOrder.Lines;
                x.DocumentLines.ForEach((item, index, lastIteration) =>
                {
                    var priceListType = string.Empty;
                    var priceListCode = string.Empty;
                    var priceListCategory = string.Empty;
                    var unitPrice = decimal.Zero;
                    var query = string.Empty;

                    query = $"select \"U_VS_CATEGORIALP\" from OITM where \"ItemCode\" = '{item.ItemCode}' and ifnull(\"U_VS_CATEGORIALP\",'') <> ''";
                    recordSet.DoQuery(query);
                    if (!recordSet.EoF)
                    {
                        fields = recordSet.Fields;
                        field = fields.Item(0);
                        priceListCategory = field.Value?.ToString() ?? string.Empty;
                    }

                    query = $"select 'E' from \"@VS_LP_ACDOCOMER\" where \"U_CODCLIENTE\" = '{x.CardCode}' and \"U_TIPOLP\"= 'PE'";
                    recordSet.DoQuery(query);
                    if (!recordSet.EoF && string.IsNullOrWhiteSpace(priceListCategory))
                    {
                        throw new Exception($"No se ha definido la categoria del producto: {item.ItemCode}");
                    }

                    query =
                        $"CALL VS_SP_ObtenerPrecioUnitario('{x.CardCode}','{x.SaleChannel}','{x.Region}','{priceListCategory}','{item.ItemCode}')";
                    recordSet.DoQuery(query);
                    if (!recordSet.EoF)
                    {
                        fields = recordSet.Fields;
                        field = fields.Item(0);
                        if (field.Value == null)
                            throw new Exception($"No se ha definido la Precio Unitario del producto: {item.ItemCode}");

                        unitPrice = field.Value.ToDecimal();

                        field = fields.Item(1);
                        if (field.Value == null)
                            throw new Exception($"No se ha definido Tipo Lista Precio del producto: {item.ItemCode}");

                        priceListType = field.Value.ToString();

                        field = fields.Item(2);
                        if (field.Value == null)
                            throw new Exception($"No se ha definido Código Lista Precio del producto: {item.ItemCode}");

                        priceListCode = field.Value.ToString();
                    }

                    if (!item.UnitPrice.IsDefault())
                    {
                        item.UnitPrice = unitPrice;
                    }

                    saleOrderLines.ItemCode = item.ItemCode;
                    saleOrderLines.Quantity = item.Quantity.ToDouble();
                    saleOrderLines.UnitPrice = item.UnitPrice.ToDouble();
                    saleOrderLines.TaxCode = item.TaxCode;

                    //TODO: agregar el centro costo
                    // if (!string.IsNullOrWhiteSpace(linea.OcrCode)) document.Lines.CostingCode = linea.OcrCode;
                    // if (!string.IsNullOrWhiteSpace(linea.OcrCode2)) document.Lines.CostingCode2 = linea.OcrCode2;
                    // if (!string.IsNullOrWhiteSpace(linea.OcrCode3)) document.Lines.CostingCode3 = linea.OcrCode3;

                    userFields = saleOrderLines.UserFields;
                    fields = userFields.Fields;

                    if (!string.IsNullOrEmpty(priceListType))
                    {
                        field = fields.Item("U_VS_TIPOLP");
                        field.Value = priceListType;
                    }

                    if (!string.IsNullOrEmpty(priceListCode))
                    {
                        field = fields.Item("U_VS_CODIGOLP");
                        field.Value = priceListCode;
                    }

                    lastIteration.IfFalse(() => { saleOrderLines.Add(); });
                });


                return saleOrder.Add() == 0 ? Tuple.Create(true, Company.GetNewObjectKey()) : Tuple.Create(false, Company.GetLastErrorDescription());
            }
            catch (Exception exception)
            {
                return Tuple.Create(false, exception.Message);
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects(recordSet, saleOrder, saleOrderLines, userFields, fields, field);
            }
        }

        #endregion

        public override void UpdateSystemFieldsFromDocument(ORDR document)
        {
            var documents = Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders).To<SAPbobsCOM.Documents>();
            documents.GetByKey(document.DocumentEntry);
            foreach (var documentLine in document.DocumentLines)
            {
                documents.Lines.SetCurrentLine(documentLine.LineNumber);
                documents.Lines.Quantity = documentLine.Quantity.ToDouble();
            }

            documents.Update();
        }

        //public override IEnumerable<RDR1> RetrieveDocumentsDetailByRoute(int codeRoute)
        //{

        //    IEnumerable<RDR1> documents = base.RetrieveDocuments("")
        //        .ToList();
        //    var unitOfWork = new UnitOfWork(Company);
        //    foreach (ORDR document in documents)
        //    {
        //        RDR1 firstLine = document.DocumentLines.First();
        //        OITM item = unitOfWork.ItemsRepository.GetItem(firstLine.ItemCode);
        //        document.IsService = item.IsInventory == false;
        //        document.IsItem = item.IsInventory;
        //    }

        //    return documents;
        //}
    }
}