using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using DisposableSAPBO.RuntimeMapper.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Detail;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using Exxis.Addon.HojadeRutaAGuia.Data.Code;
using Exxis.Addon.HojadeRutaAGuia.Data.Repository;
using VersionDLL.FlagElements.Attributes;


namespace Exxis.Addon.HojadeRutaAGuia.Data.Implements
{
    public class OFUPRepository : BaseOFUPRepository
    {
        private static readonly string FILE_UPLOAD_QUERY = $"select * from \"@{OFUP.ID}\" {{0}}";
        private static readonly string FILE_UPLOAD_DETAIL_QUERY = $"select * from \"@{FUP1.ID}\" where \"DocEntry\" = {{0}}";

        private IEnumerable<PropertyInfo> _baseProperties;
        private IEnumerable<PropertyInfo> _childProperties;

        bool enviarAlerta = false;
        static string _clientName = "";
        static string _clientAddress = "";
        static string _mensajeDetail = "";
        static decimal _peso = 0.00M;
        static SAPbobsCOM.Company _Company;

        private IEnumerable<PropertyInfo> BaseProperties
        {
            get
            {
                return _baseProperties ?? (_baseProperties = CurrentReferenceType.RetrieveSAPPropertiesWithoutChild()
                    .OrderBy(item => item.GetCustomAttribute<ChildProperty>() != null ? 1 : 0)
                    .ToList());
            }
        }

        private IEnumerable<PropertyInfo> ChildProperties
            => _childProperties ?? (_childProperties = typeof(FUP1).RetrieveSAPProperties().ToList());

        public OFUPRepository(SAPbobsCOM.Company company) : base(company)
        {
            _Company = Company;
        }

        public override IEnumerable<OFUP> Retrieve(Expression<Func<OFUP, bool>> expression = null)
        {
            string whereStatement = expression == null ? string.Empty : $"where {QueryHelper.ParseToHANAQuery(expression)}";
            return get_files(string.Format(FILE_UPLOAD_QUERY, whereStatement));
        }

        public override Tuple<bool, string> UpdateStatusEDIFile(int entry, string status)
        {
            SAPbobsCOM.CompanyService companyService = null;
            SAPbobsCOM.GeneralService generalService = null;
            SAPbobsCOM.GeneralData generalData = null;
            SAPbobsCOM.GeneralDataParams generalDataParams = null;

            try
            {
                Company.StartTransaction();
                companyService = Company.GetCompanyService();
                generalService = companyService.GetGeneralService(OFUP.ID);
                generalDataParams = generalService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams).To<SAPbobsCOM.GeneralDataParams>();
                generalDataParams.SetProperty(@"DocEntry", entry);
                generalData = generalService.GetByParams(generalDataParams);
                generalData.SetProperty(@"U_EXK_STDS", status);
                generalService.Update(generalData);
                Company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);
                return Tuple.Create(true, string.Empty);
            }
            catch (Exception exception)
            {
                if (Company.InTransaction)
                    Company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);

                return Tuple.Create(false, exception.Message);
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects(companyService, generalService, generalData, generalDataParams);
            }
        }

        public override Tuple<bool, string, int, int> Register(OFUP entity)
        {
            SAPbobsCOM.CompanyService companyService = null;
            SAPbobsCOM.GeneralService generalService = null;
            SAPbobsCOM.GeneralData generalData = null;
            SAPbobsCOM.GeneralDataParams generalDataParams = null;

            try
            {
                Company.StartTransaction();
                companyService = Company.GetCompanyService();
                generalService = companyService.GetGeneralService(OFUP.ID);
                generalData = generalService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData).To<SAPbobsCOM.GeneralData>();
                IEnumerable<PropertyInfo> properties = entity.GetType()
                    .RetrieveSAPProperties()
                    .Where(t => t.IsChildProperty() || t.IsFieldNoRelated());
                
                foreach (PropertyInfo property in properties)
                {
                    if (property.IsFieldNoRelated())
                    {
                        FieldNoRelated noRelatedProperty = property.RetrieveFieldNoRelated();
                        object value = property.GetValue(entity);
                        if (value != null && property.PropertyType == typeof(decimal))
                            value = Convert.ToDouble(value);

                        generalData.SetValueIfNotDefault(noRelatedProperty.sAliasID, property, value);
                    }
                    else if (property.IsChildProperty())
                        register_child(ref generalData, property, property.GetValue(entity));
                }
                
                generalDataParams = generalService.Add(generalData);
                Company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);
                generalData = generalService.GetByParams(generalDataParams);
                object documentEntry = generalData.GetProperty("DocEntry");
                object documentNumber = generalData.GetProperty("DocNum");
                return Tuple.Create(true, string.Empty, documentEntry.ToInt32(), documentNumber.ToInt32());
            }
            catch (Exception exception)
            {
                if (Company.InTransaction)
                    Company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);

                return Tuple.Create(false, exception.Message, default(int), default(int));
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects(companyService, generalService, generalData, generalDataParams);
            }
        }

        public override Tuple<bool, string> InsertLineEDIFile(int entry, IEnumerable<FUP1> lines)
        {
            SAPbobsCOM.CompanyService companyService = null;
            SAPbobsCOM.GeneralService generalService = null;
            SAPbobsCOM.GeneralDataParams generalDataParams = null;
            SAPbobsCOM.GeneralData generalData = null;

            try
            {
                Company.StartTransaction();
                companyService = Company.GetCompanyService();
                generalService = companyService.GetGeneralService(OFUP.ID);
                generalDataParams = generalService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
                    .To<SAPbobsCOM.GeneralDataParams>();
                generalDataParams.SetProperty("DocEntry", entry);
                generalData = generalService.GetByParams(generalDataParams);
                register_child(ref generalData, typeof(FUP1), FUP1.ID, lines);

                generalService.Update(generalData);
                Company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);
                return Tuple.Create(true, string.Empty);
            }
            catch (Exception exception)
            {
                if (Company.InTransaction)
                    Company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);

                return Tuple.Create(false, exception.Message);
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects(companyService, generalService, generalData, generalDataParams);
            }
        }

        private void register_child(ref SAPbobsCOM.GeneralData generalData, PropertyInfo listPropertyChild, object children)
        {
            Type childType = listPropertyChild.PropertyType.GetGenericArguments()[0];
            ChildProperty childPropertyAttribute = listPropertyChild.RetrieveChildProperty();
            register_child(ref generalData, childType, childPropertyAttribute.ChildName, children.To<IList>());
        }

        private void register_child(ref SAPbobsCOM.GeneralData generalData, Type lineType, string lineTableName, IEnumerable lines)
        {
            SAPbobsCOM.GeneralDataCollection childDataCollection = null;
            SAPbobsCOM.GeneralData childGeneralData = null;

            try
            {
                IEnumerable<PropertyInfo> childProperties = lineType
                    .RetrieveSAPFieldNoRelatedProperties()
                    .ToList();
                
                childDataCollection = generalData.Child(lineTableName);

                foreach (object item in lines)
                {
                    childGeneralData = childDataCollection.Add();
                    foreach (PropertyInfo childProperty in childProperties)
                    {
                        FieldNoRelated noRelatedProperty = childProperty.RetrieveFieldNoRelated();
                        object value = childProperty.GetValue(item);
                        if (value != null && childProperty.PropertyType == typeof(decimal))
                            value = Convert.ToDouble(value);

                        childGeneralData.SetValueIfNotDefault(noRelatedProperty.sAliasID, childProperty, value);
                    }
                }
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects(childDataCollection, childGeneralData);
            }
        }
        
        private IEnumerable<OFUP> get_files(string query)
        {
            IList<OFUP> result = new List<OFUP>();

            var recordSet = Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordsetEx).To<SAPbobsCOM.RecordsetEx>();
            recordSet.DoQuery(query);

            while (!recordSet.EoF)
            {
                var document = SapHelper.MakeEntityByRecordSet<OFUP>(recordSet, BaseProperties);
                document.ColumnDetail = get_column_detail(document.DocumentEntry);
                result.Add(document);
                recordSet.MoveNext();
            }

            GenericHelper.ReleaseCOMObjects(recordSet);

            return result;
        }

        private List<FUP1> get_column_detail(int documentEntry)
        {
            var result = new List<FUP1>();
            var recordSet = Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordsetEx).To<SAPbobsCOM.RecordsetEx>();
            recordSet.DoQuery(string.Format(FILE_UPLOAD_DETAIL_QUERY, documentEntry));

            while (!recordSet.EoF)
            {
                var document = SapHelper.MakeEntityByRecordSet<FUP1>(recordSet, ChildProperties);
                result.Add(document);
                recordSet.MoveNext();
            }

            GenericHelper.ReleaseCOMObjects(recordSet);

            return result;
        }

        public override void LoadFile(int docentry, string archivo)
        {
            try
            {
                SAPbobsCOM.GeneralService oGeneralService;
                SAPbobsCOM.GeneralData oGeneralData;
                SAPbobsCOM.GeneralDataParams oGeneralParams;

                SAPbobsCOM.CompanyService sCmp;
                sCmp = Company.GetCompanyService();

                oGeneralService = sCmp.GetGeneralService("VS_PD_OFUP");

                oGeneralParams = (SAPbobsCOM.GeneralDataParams) oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams);
                oGeneralParams.SetProperty("DocEntry", docentry);
                oGeneralData = oGeneralService.GetByParams(oGeneralParams);

                oGeneralData.SetProperty("U_EXK_FUPL", archivo);

                oGeneralService.Update(oGeneralData);
            }
            catch
            {
            }
        }

        public override void CreaterOrder(ref ORDR_ OrdenVenta, out int DocEntry, out string mensaje)
        {
            DocEntry = 0;
            mensaje = string.Empty;
            try
            {
                string CardCode = ObtenerCardCode(OrdenVenta.TipoSN, OrdenVenta);
                if (string.IsNullOrEmpty(CardCode))
                {
                    throw new Exception("No se encontró el código SN");
                }

                string ShipToCode = ObtenerAddrCod(CardCode, OrdenVenta.AddressFormat, OrdenVenta.ShipToAdr);
                if (string.IsNullOrEmpty(ShipToCode))
                {
                    throw new Exception($"No se encontró la dirección {OrdenVenta.ShipToAdr} del SN {CardCode}");
                }

                OrdenVenta.CardCode = CardCode;
                OrdenVenta.CardName = ObtenerCardName(CardCode);
                SAPbobsCOM.Documents document = (SAPbobsCOM.Documents) Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
                document.CardCode = CardCode;
                document.DocDate = OrdenVenta.DocDate;
                document.DocDueDate = OrdenVenta.DueDate;
                document.Comments = OrdenVenta.Comments + "[Creado por carga]";
                document.ShipToCode = ShipToCode;
                document.JournalMemo = OrdenVenta.CardName;


                var recordSet = Company
                    .GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordsetEx)
                    .To<SAPbobsCOM.RecordsetEx>();

                recordSet.DoQuery($@"select ""U_EXK_CVAL"" from ""@VS_PD_OPDS"" where ""Code"" = 'CLS_HR' ");
                var _closeHour = 0;


                while (!recordSet.EoF)
                {
                    _closeHour = int.Parse(recordSet.GetColumnValue("U_EXK_CVAL").ToString().Substring(0, 2));
                    recordSet.MoveNext();
                }

                var _dateDeliveryDay = OrdenVenta.DueDate.Day;

                if (_dateDeliveryDay != DateTime.Now.AddDays(1).Day)
                {
                    document.UserFields.Fields.Item("U_EXK_VAEX").Value = "N";
                    enviarAlerta = false;
                }
                else if (DateTime.Now.Hour < _closeHour)
                {
                    document.UserFields.Fields.Item("U_EXK_VAEX").Value = "N";
                    enviarAlerta = false;
                }
                else
                {
                    document.UserFields.Fields.Item("U_EXK_VAEX").Value = "Y";
                    _mensajeDetail = "";
                    int i = 1;
                    foreach (RDR1_ linea in OrdenVenta.Detalle)
                    {
                        SAPbobsCOM.Items s = (SAPbobsCOM.Items) Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems);
                        s.GetByKey(linea.ItemCode);
                        var itemCodeEditText = linea.ItemCode;
                        if (string.IsNullOrEmpty(itemCodeEditText))
                            continue;


                        var itemDescriptionEditText = s.ItemName;
                        var itemQuantityEditText = linea.Quantity;
                        var itemUnitEditText = s.SalesUnit;
                        //var itemWeightEditText = _saleOrderLinesMatrix.GetSpecificCell<EditText>("3", i);


                        var totalWeightEditText = s.InventoryWeight * linea.Quantity;
                        _peso = _peso + totalWeightEditText.ToDecimal();
                        _mensajeDetail = _mensajeDetail +
                                         $"\t {i}. {itemCodeEditText} - {itemDescriptionEditText} - Cantidad: {itemQuantityEditText}  Unidad:{itemUnitEditText}  Peso:{totalWeightEditText}   \n";
                        i++;
                    }
                }

                foreach (KeyValuePair<string, string> udf in OrdenVenta.UserFields)
                {
                    document.UserFields.Fields.Item(udf.Key).Value = udf.Value;
                }

                // //JFR: Se adiciona esta seccion de codigo para obtener precio * * * * * * * * * * * * * * * * * * * * * * * * * * * * 
                // var recSet = (SAPbobsCOM.Recordset) Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                // var territorio = default(string);
                // var canalVenta = default(string);
                // var qry = default(string);
                //
                // territorio = OrdenVenta.UserFields["U_CL_TERRIT"];
                // if (string.IsNullOrWhiteSpace(territorio))
                // {
                //     qry = $"CALL VS_SP_LP_OBTENERTERRITORIO('{CardCode}')";
                //     recSet.DoQuery(qry);
                //     if (!recSet.EoF) territorio = recSet.Fields.Item(0).Value.ToString();
                //     else throw new InvalidOperationException("No se obtuvo territorio");
                // }
                //
                // canalVenta = OrdenVenta.UserFields["U_CL_CANAL"];
                // if (string.IsNullOrWhiteSpace(canalVenta))
                // {
                //     recSet = (SAPbobsCOM.Recordset) Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                //     qry = $"select \"U_CL_CANAL\" from OCRD where \"CardCode\" = '{CardCode}' and ifnull(\"U_CL_CANAL\",'') <> ''";
                //     recSet.DoQuery(qry);
                //     if (!recSet.EoF) canalVenta = recSet.Fields.Item(0).Value.ToString();
                //     else throw new InvalidOperationException("No se obtuvo canal de venta");
                // }
                // //* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * 

                int lineas = 0;
                foreach (RDR1_ linea in OrdenVenta.Detalle)
                {
                    if (lineas > 0) document.Lines.Add();
                    string itemcode = ObtenerItemCode(OrdenVenta.TipoItem, linea);
                    if (string.IsNullOrEmpty(itemcode))
                    {
                        string item = !string.IsNullOrEmpty(linea.ItemCode) ? linea.ItemCode : linea.U_EXK_CFIL;
                        throw new Exception($"No se encontró el item {item}");
                    }

                    validate_item(itemcode);

                    // Dictionary<string, string> valoresPrecioUnitario =
                    //     ObtenerValoresPrecioUnitario(CardCode, canalVenta, territorio, itemcode, linea.Quantity);
                    
                    document.Lines.ItemCode = itemcode;
                    document.Lines.Quantity = linea.Quantity;
                    document.Lines.PriceAfterVAT = linea.PriceAfVat;
                    document.Lines.UnitPrice = linea.UnitPrice;
                    if (!string.IsNullOrWhiteSpace(linea.OcrCode)) document.Lines.CostingCode = linea.OcrCode;
                    if (!string.IsNullOrWhiteSpace(linea.OcrCode2)) document.Lines.CostingCode2 = linea.OcrCode2;
                    if (!string.IsNullOrWhiteSpace(linea.OcrCode3)) document.Lines.CostingCode3 = linea.OcrCode3;
                    if (!string.IsNullOrEmpty(linea.TaxCode)) document.Lines.TaxCode = linea.TaxCode;

                    // if (!string.IsNullOrEmpty(valoresPrecioUnitario["TPLP"]))
                    //     document.Lines.UserFields.Fields.Item("U_VS_TIPOLP").Value = valoresPrecioUnitario["TPLP"];
                    //
                    // if (!string.IsNullOrEmpty(valoresPrecioUnitario["CDLP"]))
                    //     document.Lines.UserFields.Fields.Item("U_VS_CODIGOLP").Value = valoresPrecioUnitario["CDLP"];

                    lineas++;
                    foreach (KeyValuePair<string, string> udf in linea.UserFields)
                    {
                        if (udf.Key == "U_EXK_CFIL") continue;
                        document.Lines.UserFields.Fields.Item(udf.Key).Value = udf.Value;
                    }
                }

                if (document.Add() == 0)
                {
                    DocEntry = int.Parse(Company.GetNewObjectKey());
                    if (enviarAlerta)
                        SendMessage_AlertAsync(DocEntry.ToString());
                }
                else
                {
                    throw new Exception(Company.GetLastErrorDescription());
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
        }

        private void ProcesarAddress(string formato, ref string address)
        {
            try
            {
                if (formato == "0")
                {
                    return;
                }

                int index = formato.IndexOf("[");
                string separador = formato.Substring(index + 1, 1);
                string[] addr = address.Split(separador);
                index = formato.IndexOf("]");
                int position = int.Parse(formato.Substring(index + 1, 1)) + 1;
                address = addr[position];
            }
            catch
            {
            }
        }

        private string ObtenerCardCode(string tipo, ORDR_ OrdenVenta)
        {
            string cardcode = string.Empty;

            var recordSet = Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordsetEx).To<SAPbobsCOM.RecordsetEx>();

            string valor;

            switch (tipo)
            {
                case "LicTradNum":
                    valor = OrdenVenta.LicTradNum;
                    break;
                case "CardName":
                    valor = OrdenVenta.CardName;
                    break;
                case "CardCode":
                default:
                    return OrdenVenta.CardCode;
            }

            recordSet.DoQuery($@"select ""CardCode"" from OCRD where ""CardType""='C' and ""{tipo}""='{valor}'");
            if (!recordSet.EoF)
            {
                cardcode = recordSet.GetColumnValue("CardCode").ToString();
            }

            return cardcode;
        }

        private string ObtenerCardName(string cardcode)
        {
            string addrcode = string.Empty;

            var recordSet = Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordsetEx).To<SAPbobsCOM.RecordsetEx>();
            recordSet.DoQuery($@"select ""CardName"" from OCRD where ""CardCode""='{cardcode}'");
            if (!recordSet.EoF)
            {
                addrcode = recordSet.GetColumnValue("CardName").ToString();
            }

            return addrcode;
        }

        private void validate_item(string itemCode)
        {
            var items = Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems).To<SAPbobsCOM.Items>();
            if (!items.GetByKey(itemCode))
                throw new Exception($"[Error] No existe el código de artículo relacionado '{itemCode}'.");

            if (items.SalesUnitWeight <= 0)
                throw new Exception($"[Error] No artículo '{itemCode}' no tiene un peso de venta relacionado.");
        }

        private string ObtenerAddrCod(string cardcode, string formato, string valor)
        {
            string addrcode = string.Empty;

            var recordSet = Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordsetEx).To<SAPbobsCOM.RecordsetEx>();
            ProcesarAddress(formato, ref valor);
            recordSet.DoQuery($@"select ""Address"" from CRD1 where ""AdresType""='S' and ""CardCode""= '{cardcode}' and ""U_EXK_DLCD""='{valor}'");
            if (!recordSet.EoF)
            {
                addrcode = recordSet.GetColumnValue("Address").ToString();
            }

            return addrcode;
        }

        private string ObtenerItemCode(string tipo, RDR1_ Detalle)
        {
            string itemcode = string.Empty;

            var recordSet = Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordsetEx).To<SAPbobsCOM.RecordsetEx>();

            string valor;

            switch (tipo)
            {
                case "U_EXK_CFIL":
                    valor = Detalle.U_EXK_CFIL;
                    break;
                case "ItemCode":
                default:
                    return Detalle.ItemCode;
            }

            recordSet.DoQuery($@"select ""ItemCode"" from OITM where ""{tipo}""='{valor}'");
            if (!recordSet.EoF)
            {
                itemcode = recordSet.GetColumnValue("ItemCode").ToString();
            }

            return itemcode;
        }

        public static async void SendMessage_AlertAsync(string oMessage)
        {
            await Task.Run(() => SendMessage_Alert(oMessage));
        }


        private static void SendMessage_Alert(string order)
        {
            SAPbobsCOM.Messages msg = (SAPbobsCOM.Messages) _Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages);
            var oCmpSrv = _Company.GetCompanyService();
            var oMessageService = (SAPbobsCOM.MessagesService) oCmpSrv.GetBusinessService(SAPbobsCOM.ServiceTypes.MessagesService);


            var recordSet = _Company
                .GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordsetEx)
                .To<SAPbobsCOM.RecordsetEx>();
            recordSet.DoQuery($@"select ""U_EXK_CVAL"" from ""@VS_PD_OPDS"" where ""Code"" = 'USR_AL' ");
            var userCode = "";
            List<string> listUser = new List<string>();

            while (!recordSet.EoF)
            {
                userCode = recordSet.GetColumnValue("U_EXK_CVAL").ToString();
                recordSet.MoveNext();
            }

            foreach (var item in userCode.Split(","))
            {
                listUser.Add(item);
            }

            //if (oMessage.Priority.HasValue)
            msg.Priority = SAPbobsCOM.BoMsgPriorities.pr_High;

            msg.Subject = ": Alerta de Creación de Orden de Venta Extraordinaria";


            var mensaje = $"Se ha creado la orden de venta extraordinaria N° {order} (peso total: {_peso}kg) " +
                          $"para el cliente:{_clientName} MAKRO SUPERMAYORISTA SA con entrega a la dirección: {_clientAddress} con el siguiente detalle: " +
                          $"\n \n";

            mensaje = mensaje + _mensajeDetail;
            mensaje = mensaje + $"\n \n Peso total de la Orden: {_peso} kg";

            msg.MessageText = mensaje;

            for (int i = 0; i < listUser.Count; i++)
            {
                if (i > 0)
                    msg.Recipients.Add();
                msg.Recipients.SetCurrentLine(i);
                msg.Recipients.UserCode = listUser[i];
                msg.Recipients.SendInternal = SAPbobsCOM.BoYesNoEnum.tYES;
            }

            //msg.Recipients.Add();


            var result = msg.Add();


            if (result != 0) // Check the result
            {
                string error;
                string vm_GetLastErrorDescription_string = _Company.GetLastErrorDescription();

                //Company.GetLastError(out res, out error);                 
            }
        }

        private Dictionary<string, string> ObtenerValoresPrecioUnitario(string cardCode, string canalVenta, string territorio
            , string itemCode, double quantity)
        {
            var categoriaLP = default(string);
            var qry = $"select \"U_VS_CATEGORIALP\" from OITM where \"ItemCode\" = '{itemCode}' and ifnull(\"U_VS_CATEGORIALP\",'') <> ''";
            var recSet = (SAPbobsCOM.Recordset) Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            recSet.DoQuery(qry);
            if (!recSet.EoF) categoriaLP = recSet.Fields.Item(0).Value.ToString();

            //Determino si el cliente tiene precio por escala 

            recSet = (SAPbobsCOM.Recordset) Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            qry = $"select 'E' from \"@VS_LP_ACDOCOMER\" where \"U_CODCLIENTE\" = '{cardCode}' and \"U_TIPOLP\"= 'PE'";
            recSet = (SAPbobsCOM.Recordset) Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            recSet.DoQuery(qry);
            if (!recSet.EoF && string.IsNullOrWhiteSpace(categoriaLP))
            {
                throw new Exception($"No se ha definido la categoria del producto: {itemCode}");

                /*
                qry = $"CALL VS_SP_ObtenerCodigoEscala('{itemCode}','{categoriaLP}','{quantity}')";
                recSet = (SAPbobsCOM.Recordset)sboCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                recSet.DoQuery(qry);
                if (!recSet.EoF) codEscala = recSet.Fields.Item(0).Value;
                else sboApplication.StatusBarWarningMsg("No se obtuvo escala del producto");
                */
            }

            qry = $"CALL VS_SP_ObtenerPrecioUnitario('{cardCode}','{canalVenta}','{territorio}'" +
                  $",'{categoriaLP}','{itemCode}')";
            recSet = (SAPbobsCOM.Recordset) Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            recSet.DoQuery(qry);

            //devuelve el precio unitario
            var preUni = recSet.Fields.Item(0).Value is double ? (double?) Convert.ToDouble(recSet.Fields.Item(0).Value) : null;

            if (!recSet.EoF && preUni != null && preUni > 0)
                // if (!recSet.EoF && recSet.Fields.Item(0).Value is double preUni && preUni > 0)
                return new Dictionary<string, string>
                {
                    {"PUNI", preUni.ToString()},
                    {"TPLP", recSet.Fields.Item(1).Value.ToString()}, //tipo de lista de precios (estandar, fijo, por escala)
                    {"CDLP", recSet.Fields.Item(2).Value.ToString()} //codigo de lista de precios
                };
            else
                throw new Exception("No se ha definido ningún precio para este producto");
        }


        private double ObtenerPrecioUnitario(string cardCode, string canalVenta, string territorio
            , string itemCode, double quantity)
        {
            var categoriaLP = default(string);
            var qry = $"select \"U_VS_CATEGORIALP\" from OITM where \"ItemCode\" = '{itemCode}' and ifnull(\"U_VS_CATEGORIALP\",'') <> ''";
            var recSet = (SAPbobsCOM.Recordset) Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            recSet.DoQuery(qry);
            if (!recSet.EoF) categoriaLP = recSet.Fields.Item(0).Value.ToString();

            //Determino si el cliente tiene precio por escala 

            recSet = (SAPbobsCOM.Recordset) Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            qry = $"select 'E' from \"@VS_LP_ACDOCOMER\" where \"U_CODCLIENTE\" = '{cardCode}' and \"U_TIPOLP\"= 'PE'";
            recSet = (SAPbobsCOM.Recordset) Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            recSet.DoQuery(qry);
            if (!recSet.EoF && string.IsNullOrWhiteSpace(categoriaLP))
            {
                throw new Exception($"No se ha definido la categoria del producto: {itemCode}");

                /*
                qry = $"CALL VS_SP_ObtenerCodigoEscala('{itemCode}','{categoriaLP}','{quantity}')";
                recSet = (SAPbobsCOM.Recordset)sboCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                recSet.DoQuery(qry);
                if (!recSet.EoF) codEscala = recSet.Fields.Item(0).Value;
                else sboApplication.StatusBarWarningMsg("No se obtuvo escala del producto");
                */
            }

            qry = $"CALL VS_SP_ObtenerPrecioUnitario('{cardCode}','{canalVenta}','{territorio}'" +
                  $",'{categoriaLP}','{itemCode}')";
            recSet = (SAPbobsCOM.Recordset) Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            recSet.DoQuery(qry);
            if (!recSet.EoF)
            {
                var preUni = Convert.ToDouble(recSet.Fields.Item(0).Value);
                if (preUni > 0)
                    return preUni;
                else
                    throw new Exception("No se ha definido ningún precio para este producto");
            }
            else
                throw new Exception("No se ha definido ningún precio para este producto");
        }
    }
}