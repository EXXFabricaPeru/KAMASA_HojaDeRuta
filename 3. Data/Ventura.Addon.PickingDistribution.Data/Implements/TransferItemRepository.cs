using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Models;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Constant;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Detail;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using Exxis.Addon.HojadeRutaAGuia.Data.Repository;
using VSVersionControl.FlagElements.Attributes;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Implements
{
    public class TransferItemRepository : BaseTransferItemRepository
    {
        private const string BASE_QUERY = "select * from \"OWTR\" {0}";
        private const string BASE_LINE_QUERY = "select * from \"WTR1\" where \"DocEntry\" = {0} order by \"LineNum\"";

        private const string SELECTED_BATCHES_QUERY =
            "select * from \"IBT1\" where \"BaseLinNum\" = {1} and \"BaseType\" = {2} and \"BaseEntry\" = {0} and \"WhsCode\" = '{3}'";

        private const string OTHERS_DEVOLUTION_TYPE = "99";

        private static readonly string DOCUMENT_QUERY_REQUEST = $"select * from \"OWTQ\" {{0}}";

        private static readonly string DOCUMENT_LINE_QUERY_REQUEST = $"select * from \"WTQ1\" where \"DocEntry\" = {{0}} order by \"LineNum\"";

        private static readonly string DOCUMENT_QUERY = $"select * from \"OWTR\" {{0}}";

        private static readonly string DOCUMENT_LINE_QUERY = $"select * from \"WTR1\" where \"DocEntry\" = {{0}} order by \"LineNum\"";

        public TransferItemRepository(Company company)
            : base(company)
        {
        }

        public override Tuple<bool, string> Register(OWTR entity)
        {
            try
            {
                Company.StartTransaction();
                var stockTransfer = Company.GetBusinessObject(BoObjectTypes.oStockTransfer).To<StockTransfer>();
                stockTransfer.DocDate = entity.DocumentDate;
                stockTransfer.DueDate = entity.DueDate;
                if (!string.IsNullOrEmpty(entity.FromWarehouse)) stockTransfer.FromWarehouse = entity.FromWarehouse;
                stockTransfer.ToWarehouse = entity.ToWarehouse;
                stockTransfer.Comments = entity.Comments;
                Fields userFields = stockTransfer.UserFields.Fields;
                userFields.Item(entity.GetSAPFieldName(t => t.ReferenceTransferOrderEntry)).Value = entity.ReferenceTransferOrderEntry;
                userFields.Item(entity.GetSAPFieldName(t => t.ReferenceTransferOrderNumber)).Value = entity.ReferenceTransferOrderNumber;

                Document_DocumentReferences documentReferences = stockTransfer.DocumentReferences;
                if (entity.RelatedDocuments != null)
                {
                    entity.RelatedDocuments.ForEach((document, index, lastIteration) =>
                    {
                        documentReferences.ReferencedDocEntry = document.DocumentEntry;
                        documentReferences.ReferencedObjectType = (ReferencedObjectTypeEnum)Enum.ToObject(typeof(ReferencedObjectTypeEnum), document.SAPType);
                        lastIteration.IfFalse(() => documentReferences.Add());
                    });
                }
               

                register_lines(stockTransfer, entity);

                int operationResult = stockTransfer.Add();
                if (operationResult.IsDefault())
                {
                    string key;
                    Company.GetNewObjectCode(out key);
                    if (Company.InTransaction)
                        Company.EndTransaction(BoWfTransOpt.wf_Commit);
                    return Tuple.Create(true, key);
                }

                if (Company.InTransaction)
                    Company.EndTransaction(BoWfTransOpt.wf_RollBack);
                return Tuple.Create(false, Company.GetLastErrorDescription());
            }
            catch (Exception exception)
            {
                if (Company.InTransaction)
                    Company.EndTransaction(BoWfTransOpt.wf_RollBack);

                return Tuple.Create(false, exception.Message);
            }
        }

        private void register_lines(StockTransfer stockTransfer, OWTR entity)
        {
            StockTransfer_Lines stockTransferLines = stockTransfer.Lines;
            entity.Lines.ForEach((item, index, lastIteration) =>
            {
                stockTransferLines.ItemCode = item.ItemCode;
                stockTransferLines.Quantity = item.Quantity.ToDouble();
                stockTransferLines.FromWarehouseCode = string.IsNullOrEmpty(item.FromWarehouse) ? entity.FromWarehouse: item.FromWarehouse;
               
                try
                {
                    stockTransferLines.WarehouseCode = item.ToWarehouse;
                }
                catch (Exception)
                {
                    stockTransferLines.WarehouseCode = entity.ToWarehouse;
                }
               
                Fields lineUserFields = stockTransferLines.UserFields.Fields;
                lineUserFields.Item(item.GetSAPFieldName(t => t.SUNATOperation)).Value = OTHERS_DEVOLUTION_TYPE;
                register_batches(stockTransferLines, item);

                if (!lastIteration) stockTransferLines.Add();
            });
        }

        private void register_batches(StockTransfer_Lines stockTransferLines, WTR1 line)
        {
            BatchNumbers batchNumbers = stockTransferLines.BatchNumbers;
            line.SelectedBatches.ForEach((batch, index, lastIteration) =>
            {
                batchNumbers.ItemCode = batch.ItemCode;
                batchNumbers.BatchNumber = batch.BatchNumber;
                batchNumbers.Quantity = batch.Quantity.ToDouble();
                if (!lastIteration) batchNumbers.Add();
            });
        }

        public override IEnumerable<OWTR> Retrieve(Expression<Func<OWTR, bool>> expression = null)
        {
            string whereStatement = expression == null ? string.Empty : $"where {QueryHelper.ParseToHANAQuery(expression)}";
            return element_from_query(string.Format(BASE_QUERY, whereStatement));
        }

        private IEnumerable<OWTR> element_from_query(string query)
        {
            IList<OWTR> documents = new List<OWTR>();
            Type entityType = typeof(OWTR);

            IEnumerable<PropertyInfo> entityProperties = entityType.GetProperties()
                .Where(item => item.GetCustomAttribute<SAPColumnAttribute>() != null || item.GetCustomAttribute<FieldNoRelated>() != null)
                .OrderBy(order_attributes)
                .ToList();

            if (!entityProperties.Any())
                throw new Exception($"[ERROR] The '{nameof(TransferItemRepository)}' class is not setter correctly");

            var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            recordSet.DoQuery(query);

            while (!recordSet.EoF)
            {
                var element = new OWTR();
                foreach (PropertyInfo property in entityProperties)
                {
                    if (property.Name == nameof(element.Lines))
                    {
                        element.Lines = retrieve_lines(element.DocumentEntry);
                        continue;
                    }

                    if (property.Name == nameof(element.RelatedDocuments))
                    {
                        continue;
                    }

                    string sapColumnName = get_sap_column_name(property);
                    object parsedValue = try_get_sap_value(recordSet, sapColumnName, property);
                    property.SetValue(element, parsedValue);
                }

                documents.Add(element);
                recordSet.MoveNext();
            }

            return documents;
        }

        private int order_attributes(PropertyInfo propertyInfo)
        {
            var sapColumnAttribute = propertyInfo.GetCustomAttribute<SAPColumnAttribute>();
            var fieldNoRelatedAttribute = propertyInfo.GetCustomAttribute<FieldNoRelated>();

            if (sapColumnAttribute == null && fieldNoRelatedAttribute == null)
                throw new Exception($"[ERROR] The property '{propertyInfo.Name}' don't have set a 'SAPColumnAttribute'");

            if (sapColumnAttribute == null)
                return 4;
            if (sapColumnAttribute.SystemField && !sapColumnAttribute.DetailLine)
                return 1;
            if (sapColumnAttribute.SystemField && sapColumnAttribute.DetailLine)
                return 2;
            if (!sapColumnAttribute.SystemField)
                return 3;
            throw new Exception($"[ERROR] The property '{propertyInfo.Name}' don't have set a 'SAPColumnAttribute'");
        }

        private IList<WTR1> retrieve_lines(int documentEntry)
        {
            IList<WTR1> lines = new List<WTR1>();
            var recordSet = (RecordsetEx)Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx);
            recordSet.DoQuery(string.Format(BASE_LINE_QUERY, documentEntry));

            IEnumerable<PropertyInfo> lineProperties = typeof(WTR1).GetProperties()
                .Where(item => item.GetCustomAttribute<SAPColumnAttribute>() != null || item.GetCustomAttribute<FieldNoRelated>() != null)
                .OrderBy(order_attributes)
                .ToList();

            while (!recordSet.EoF)
            {
                var line = new WTR1();

                foreach (PropertyInfo property in lineProperties)
                {
                    if (property.Name == nameof(line.SelectedBatches))
                    {
                        line.SelectedBatches = retrieve_batches(documentEntry, line.LineNumber, line.ToWarehouse);
                        continue;
                    }

                    string sapColumnName = get_sap_column_name(property);
                    object parsedValue = try_get_sap_value(recordSet, sapColumnName, property);
                    property.SetValue(line, parsedValue);
                }

                lines.Add(line);
                recordSet.MoveNext();
            }

            return lines;
        }

        private IList<SAPSelectedBatch> retrieve_batches(int documentEntry, int lineNumber, string warehouseCode)
        {
            IList<SAPSelectedBatch> result = new List<SAPSelectedBatch>();
            var recordSet = (RecordsetEx)Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx);
            recordSet.DoQuery(string.Format(SELECTED_BATCHES_QUERY, documentEntry, lineNumber, SAPCodeType.INVENTORY_TRANSFER, warehouseCode));
            while (!recordSet.EoF)
            {
                var batch = new SAPSelectedBatch();
                batch.ItemCode = recordSet.GetColumnValue("ItemCode").ToString();
                batch.BatchNumber = recordSet.GetColumnValue("BatchNum").ToString();
                batch.WarehouseCode = recordSet.GetColumnValue("WhsCode").ToString();
                batch.Quantity = recordSet.GetColumnValue("Quantity").ToInt32();
                result.Add(batch);
                recordSet.MoveNext();
            }

            return result;
        }

        private string get_sap_column_name(MemberInfo propertyInfo)
        {
            var sapColumnAttribute = propertyInfo.GetCustomAttribute<SAPColumnAttribute>();
            var fieldNoRelatedAttribute = propertyInfo.GetCustomAttribute<FieldNoRelated>();
            return fieldNoRelatedAttribute == null ? sapColumnAttribute.Name : fieldNoRelatedAttribute.ColumnName;
        }

        private object try_get_sap_value(IRecordsetEx recordSet, string sapColumnName, PropertyInfo propertyInfo)
        {
            try
            {
                object originalValue = recordSet.GetColumnValue(sapColumnName) ??
                                       propertyInfo.PropertyType.GetDefaultValue();
                return Convert.ChangeType(originalValue, propertyInfo.PropertyType);
            }
            catch
            {
                return propertyInfo.PropertyType.IsValueType ? Activator.CreateInstance(propertyInfo.PropertyType) : null;
            }
        }

        public override Tuple<bool, string> UpdateBaseEntity(OWTR document)
        {
            try
            {
                var documents = Company.GetBusinessObject(BoObjectTypes.oStockTransfer).To<Documents>();
                documents.GetByKey(document.DocumentEntry);
                foreach (var documentLine in document.Lines)
                {
                    documents.Lines.SetCurrentLine(documentLine.LineNumber);
                    documents.Lines.Quantity = documentLine.Quantity.ToDouble();
                }

                documents.Update();
                if (Company.InTransaction)
                    Company.EndTransaction(BoWfTransOpt.wf_Commit);
                return Tuple.Create(true, "");
            }
            catch (Exception exception)
            {
                if (Company.InTransaction)
                    Company.EndTransaction(BoWfTransOpt.wf_RollBack);

                return Tuple.Create(false, exception.Message);
            }

        }

        public override Tuple<bool, string> UpdateBaseEntityRequest(OWTR document)
        {
            try
            {
                var documents = Company.GetBusinessObject(BoObjectTypes.oInventoryTransferRequest).To<StockTransfer>();
                documents.GetByKey(document.DocumentEntry);

                documents.UserFields.Fields.Item("U_EXK_ROTE").Value = document.ReferenceTransferOrderEntry;
                documents.UserFields.Fields.Item("U_EXK_ROTN").Value = document.ReferenceTransferOrderNumber;
                foreach (var documentLine in document.Lines)
                {
                    documents.Lines.SetCurrentLine(documentLine.LineNumber);
                    documents.Lines.Quantity = documentLine.Quantity.ToDouble();
                }

                documents.Update();
                if (Company.InTransaction)
                    Company.EndTransaction(BoWfTransOpt.wf_Commit);
                return Tuple.Create(true, "");
            }
            catch (Exception exception)
            {
                if (Company.InTransaction)
                    Company.EndTransaction(BoWfTransOpt.wf_RollBack);

                return Tuple.Create(false, exception.Message);
            }

        }

        public override Tuple<bool, string> UpdateFolio(OWTR document)
        {
            try
            {
                var documents = Company.GetBusinessObject(BoObjectTypes.oStockTransfer).To<StockTransfer>();
                documents.GetByKey(document.DocumentEntry);
                string numeracion = "";
                var recordSet = ((SAPbobsCOM.Recordset)(Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)));
                string query = $"CALL \"bpvs_LC_BF_OP_INVENTARIO\"('11','10','N','G001-09','P','','','')";
                recordSet.DoQuery(query);

                documents.UserFields.Fields.Item("U_BPV_SERI").Value = "G001-09";
                string correlativo = "";
                while (!recordSet.EoF)
                {
                    correlativo = recordSet.Fields.Item("U_BP_NROACT").Value.ToString();
                    documents.UserFields.Fields.Item("U_BPV_NCON2").Value = correlativo;
                    //documents.Add(document);
                    recordSet.MoveNext();
                }

                //documents. = numeracion = "09G001-" + correlativo;

                documents.Update();
                if (Company.InTransaction)
                    Company.EndTransaction(BoWfTransOpt.wf_Commit);
                return Tuple.Create(true, "");
            }
            catch (Exception exception)
            {
                if (Company.InTransaction)
                    Company.EndTransaction(BoWfTransOpt.wf_RollBack);

                return Tuple.Create(false, exception.Message);
            }

        }

        public override Tuple<bool, string> UpdateFolioRequest(OWTR document)
        {
            try
            {
                var documents = Company.GetBusinessObject(BoObjectTypes.oInventoryTransferRequest).To<StockTransfer>();
                //documents.DocObjectCode = BoObjectTypes.oInventoryTransferRequest;
                documents.GetByKey(document.DocumentEntry);
                string numeracion = "";
                var recordSet = ((SAPbobsCOM.Recordset)(Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)));
                string query = $"CALL \"bpvs_LC_BF_OP_INVENTARIO\"('11','10','N','G001-09','P','','','')";
                recordSet.DoQuery(query);

                documents.UserFields.Fields.Item("U_BPV_SERI").Value = "G001-09";
                string correlativo = "";
                while (!recordSet.EoF)
                {
                    correlativo = recordSet.Fields.Item("U_BP_NROACT").Value.ToString();
                    documents.UserFields.Fields.Item("U_BPV_NCON2").Value = correlativo;
                    //documents.Add(document);
                    recordSet.MoveNext();
                }

                //documents. = numeracion = "09G001-" + correlativo;

                documents.Update();
                if (Company.InTransaction)
                    Company.EndTransaction(BoWfTransOpt.wf_Commit);
                return Tuple.Create(true, "");
            }
            catch (Exception exception)
            {
                if (Company.InTransaction)
                    Company.EndTransaction(BoWfTransOpt.wf_RollBack);

                return Tuple.Create(false, exception.Message);
            }

        }

        public override OWTR RetrieveTransfeStockByOT(Expression<Func<OWTR, bool>> expression)
        {
            var recordSet = ((SAPbobsCOM.Recordset)(Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)));
            //string query = $"CALL \"bpvs_LC_BF_OP_INVENTARIO\"('11','10','N','G001-09','P','','','')";
            //recordSet.DoQuery(query);


            IEnumerable<OWTR> documents = RetrieveDocuments(expression)
              .ToList();
            var unitOfWork = new UnitOfWork(Company);
            foreach (OWTR document in documents)
            {
                //WTR1 firstLine = document.Lines.First();
                //OITM item = unitOfWork.ItemsRepository.GetItem(firstLine.ItemCode);
                //document.IsService = item.IsInventory == false;
                //document.IsItem = item.IsInventory;
            }

            return documents.FirstOrDefault();
        }

        public override OWTR RetrieveRequestTransfeStockByOT(Expression<Func<OWTR, bool>> expression)
        {
            var recordSet = ((SAPbobsCOM.Recordset)(Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)));
            //string query = $"CALL \"bpvs_LC_BF_OP_INVENTARIO\"('11','10','N','G001-09','P','','','')";
            //recordSet.DoQuery(query);


            IEnumerable<OWTR> documents = RetrieveRequestDocuments(expression)
              .ToList();
            var unitOfWork = new UnitOfWork(Company);
            foreach (OWTR document in documents)
            {
                //WTR1 firstLine = document.Lines.First();
                //OITM item = unitOfWork.ItemsRepository.GetItem(firstLine.ItemCode);
                //document.IsService = item.IsInventory == false;
                //document.IsItem = item.IsInventory;
            }

            return documents.FirstOrDefault();
        }

        public virtual IEnumerable<OWTR> RetrieveDocuments(Expression<Func<OWTR, bool>> expression)
        {
            string whereStatement = QueryHelper.ParseToHANAQuery(expression);
            return documents_from_query(string.Format(DOCUMENT_QUERY, $"where {whereStatement}"));
        }

        public virtual IEnumerable<OWTR> RetrieveRequestDocuments(Expression<Func<OWTR, bool>> expression)
        {
            string whereStatement = QueryHelper.ParseToHANAQuery(expression);
            return documents_from_query(string.Format(DOCUMENT_QUERY_REQUEST, $"where {whereStatement}"), 1);
        }

        private IEnumerable<OWTR> documents_from_query(string query, int _type = 0)
        {
            IList<OWTR> documents = new List<OWTR>();
            Type entityType = typeof(OWTR);

            IEnumerable<PropertyInfo> entityProperties = entityType.GetProperties()
                .Where(item => item.GetCustomAttribute<SAPColumnAttribute>() != null)
                .OrderBy(order_attributes)
                .ToList();

            if (!entityProperties.Any())
                throw new Exception("[ERROR] The SAP document is not setter correctly");

            var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            recordSet.DoQuery(query);

            while (!recordSet.EoF)
            {
                var document = (OWTR)Activator.CreateInstance(entityType);
                foreach (PropertyInfo property in entityProperties)
                {
                    if (property.Name == nameof(document.Lines))
                    {

                        document.Lines = retrieve_lines(document.DocumentEntry, document.DocumentNumber, _type).ToList();
                        continue;
                    }

                    string sapColumnName = get_sap_column_name(property);
                    object parsedValue = try_get_sap_value(recordSet, sapColumnName, property);
                    property.SetValue(document, parsedValue);
                }

                documents.Add(document);
                recordSet.MoveNext();
            }

            return documents;
        }
        private IEnumerable<WTR1> retrieve_lines(int id, int number, int type)
        {
            var recordSet = (RecordsetEx)Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx);
            if (type == 0)
                recordSet.DoQuery(string.Format(DOCUMENT_LINE_QUERY, id));
            else
                recordSet.DoQuery(string.Format(DOCUMENT_LINE_QUERY_REQUEST, id));
            IList<WTR1> lines = new List<WTR1>();
            while (!recordSet.EoF)
            {
                var sapDocument = new WTR1();
                sapDocument.LineNumber = recordSet.GetColumnValue("LineNum").ToInt32();
                sapDocument.ItemCode = recordSet.GetStringValue("ItemCode");
                //sapDocument.ItemDescription = recordSet.GetStringValue("Dscription");
                sapDocument.Quantity = recordSet.GetColumnValue("Quantity").ToDecimal();
                //sapDocument.UsefulLifeTime = recordSet.GetColumnValue("U_EXK_TVUM")?.ToInt32() ?? 0;
                //sapDocument.UnitPrice = recordSet.GetColumnValue("Price").ToDecimal();
                //sapDocument.TotalAmount = recordSet.GetColumnValue("LineTotal").ToDecimal();
                //sapDocument.TotalPrice = recordSet.GetColumnValue("GTotal").ToDecimal();
                //sapDocument.TotalSaleVolume = recordSet.GetColumnValue("Volume").ToDecimal();
                //sapDocument.TotalSaleWeight = recordSet.GetColumnValue("Weight1").ToDecimal();
                //sapDocument.TaxCode = recordSet.GetStringValue("TaxCode");
                //sapDocument.WarehouseCode = recordSet.GetStringValue("WhsCode");
                //sapDocument.BaseEntry = recordSet.GetColumnValue("BaseEntry").ToInt32();
                //sapDocument.BaseLine = recordSet.GetColumnValue("BaseLine").ToInt32();
                //sapDocument.BaseType = recordSet.GetColumnValue("BaseType").ToInt32();
                //sapDocument.SelectedBatches = RetrieveBatchesFromDocument(id, sapDocument.LineNumber, type);
                lines.Add(sapDocument);
                recordSet.MoveNext();
            }

            return lines;
        }


    }
}