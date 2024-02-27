using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using Exxis.Addon.HojadeRutaAGuia.Data.Repository;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Implements
{
    // ReSharper disable once InconsistentNaming
    public class OIBTRepository : BaseOIBTRepository
    {
        private const string BATCH_QUERY = "select * from \"OIBT\" where \"ItemCode\" = '{0}' ";
        private const string GET_BATCH_QUERY = "select \"NUM_BATCH\".\"AbsEntry\", \"INV_BATCH\".* from \"OIBT\" \"INV_BATCH\", \"OBTN\" \"NUM_BATCH\" " +
                                               "where \"INV_BATCH\".\"ItemCode\" = \"NUM_BATCH\".\"ItemCode\" and \"INV_BATCH\".\"BatchNum\" = \"NUM_BATCH\".\"DistNumber\" " +
                                               "and \"INV_BATCH\".\"BatchNum\" = '{0}' and \"INV_BATCH\".\"WhsCode\" = '{1}' and \"INV_BATCH\".\"ItemCode\" = '{2}'";
        private const string BATCH_QUERY_WO = "select * from \"OIBT\" where \"ItemCode\" = '{0}' ";
        public OIBTRepository(Company company) : base(company)
        {
        }

        public override IEnumerable<OIBT> GetBatches(string itemId)
        {
            var recordSet = (RecordsetEx) Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx);
            recordSet.DoQuery(string.Format(BATCH_QUERY, itemId));
            IList<OIBT> transfer = new List<OIBT>();
            while (!recordSet.EoF)
            {
                var mapping = new OIBT
                {
                    ItemCode = recordSet.GetColumnValue("ItemCode").ToString(),
                    ItemDescription = recordSet.GetColumnValue("ItemName").ToString(),
                    BatchNumber = recordSet.GetColumnValue("BatchNum").ToString(),
                    WarehouseCode = recordSet.GetColumnValue("WhsCode").ToString(),
                    Quantity = recordSet.GetColumnValue("Quantity").ToDecimal(),
                    ReservedAmount = recordSet.GetColumnValue("U_EXK_CNRV").ToDecimal(),
                    ExpireDate = recordSet.GetColumnValue("ExpDate").ToDateTime(),
                    ManufactureDate = recordSet.GetColumnValue("PrdDate").ToDateTime()
                };
                transfer.Add(mapping);
                recordSet.MoveNext();
            }

            return transfer;
        }
        public override IEnumerable<OIBT> GetBatchesWithoutCNRV(string itemId)
        {
            var recordSet = (RecordsetEx)Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx);
            recordSet.DoQuery(string.Format(BATCH_QUERY_WO, itemId));
            IList<OIBT> transfer = new List<OIBT>();
            while (!recordSet.EoF)
            {
                var mapping = new OIBT
                {
                    ItemCode = recordSet.GetColumnValue("ItemCode").ToString(),
                    ItemDescription = recordSet.GetColumnValue("ItemName").ToString(),
                    BatchNumber = recordSet.GetColumnValue("BatchNum").ToString(),
                    WarehouseCode = recordSet.GetColumnValue("WhsCode").ToString(),
                    Quantity = recordSet.GetColumnValue("Quantity").ToDecimal(),
                    ReservedAmount = recordSet.GetColumnValue("U_EXK_CNRV").ToDecimal(),
                    ExpireDate = recordSet.GetColumnValue("ExpDate").ToDateTime(),
                    ManufactureDate = recordSet.GetColumnValue("PrdDate").ToDateTime()
                };
                transfer.Add(mapping);
                recordSet.MoveNext();
            }

            return transfer;
        }


        

        public override OIBT RetrieveBatch(string itemCode, string batchNumber, string warehouseCode)
        {
            var recordSet = (RecordsetEx) Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx);
            recordSet.DoQuery(string.Format(GET_BATCH_QUERY, batchNumber, warehouseCode, itemCode));

            var mapping = new OIBT
            {
                AbsEntry = recordSet.GetColumnValue("AbsEntry").ToInt32(),
                ItemCode = recordSet.GetColumnValue("ItemCode").ToString(),
                ItemDescription = recordSet.GetColumnValue("ItemName").ToString(),
                BatchNumber = recordSet.GetColumnValue("BatchNum").ToString(),
                WarehouseCode = recordSet.GetColumnValue("WhsCode").ToString(),
                Quantity = recordSet.GetColumnValue("Quantity").ToDecimal(),
                ReservedAmount = recordSet.GetColumnValue("U_EXK_CNRV").ToDecimal(),
                ExpireDate = recordSet.GetColumnValue("ExpDate").ToDateTime(),
                ManufactureDate = recordSet.GetColumnValue("PrdDate").ToDateTime()
            };
            return mapping;
        }

        public override void ReserveQuantity(int batchEntry, decimal quantity)
        {
            var companyService = Company.GetCompanyService();
            var batchService = companyService.GetBusinessService(ServiceTypes.BatchNumberDetailsService).To<BatchNumberDetailsService>();
            var batchParams = batchService.GetDataInterface(BatchNumberDetailsServiceDataInterfaces.bndsBatchNumberDetailParams).To<BatchNumberDetailParams>();
            batchParams.DocEntry = batchEntry;
            var batchNumber = batchService.Get(batchParams);
            var reservedQuantity = batchNumber.UserFields.Item("U_EXK_CNRV").Value.ToDouble();
            batchNumber.UserFields.Item("U_EXK_CNRV").Value = reservedQuantity + quantity.ToDouble();
            batchService.Update(batchNumber);
            Marshal.ReleaseComObject(batchNumber);
        }

        public override decimal RetrieveActualStock(string itemCode, string warehouseCode, string batchNumber)
        {
            var query = $@" select sum(BTQT.""Quantity"")
                            from    ""OBTN"" BTCH, 
                                    ""OBTQ"" BTQT 
                            where BTCH.""ItemCode"" = BTQT.""ItemCode""
                            and BTCH.""SysNumber"" = BTQT.""SysNumber""
                            and BTCH.""ItemCode"" = '{itemCode}'
                            and BTCH.""DistNumber"" = '{batchNumber}'
                            and BTQT.""WhsCode"" = '{warehouseCode}'";
            Recordset recordSet = null;
            Fields fields = null;
            Field field = null;
            try
            {
                recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordset).To<Recordset>();
                recordSet.DoQuery(query);
                fields = recordSet.Fields;
                field = fields.Item(0);
                return field.Value.ToDecimal();
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects(recordSet, fields, field);
            }
        }

        public override decimal RetrieveActualStock(string itemCode, string warehouseCode)
        {
            var query = $@" select sum(BTQT.""Quantity"")
                            from    ""OBTN"" BTCH, 
                                    ""OBTQ"" BTQT 
                            where BTCH.""ItemCode"" = BTQT.""ItemCode""
                            and BTCH.""SysNumber"" = BTQT.""SysNumber""
                            and BTCH.""ItemCode"" = '{itemCode}'                            
                            and BTQT.""WhsCode"" = '{warehouseCode}'";
            Recordset recordSet = null;
            Fields fields = null;
            Field field = null;
            try
            {
                recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordset).To<Recordset>();
                recordSet.DoQuery(query);
                fields = recordSet.Fields;
                field = fields.Item(0);
                return field.Value.ToDecimal();
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects(recordSet, fields, field);
            }
        }
    }
}