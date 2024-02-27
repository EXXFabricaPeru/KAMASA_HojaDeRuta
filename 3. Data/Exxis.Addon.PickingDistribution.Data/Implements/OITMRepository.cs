using System;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.DisposableBO;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using Exxis.Addon.HojadeRutaAGuia.Data.Repository;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Implements
{
    // ReSharper disable once InconsistentNaming
    public class OITMRepository : BaseOITMRepository
    {
        private const string ITEM_QUERY = "select UOM.\"Name\" as \"UoMDesc\", ITM.* " +
                                          "from	\"OITM\" ITM left join \"@VS_UNID_MEDIDA\" UOM " +
                                          "on ITM.\"InvntryUom\" = UOM.\"U_VS_UM_ABREV\" where ITM.\"ItemCode\" = '{0}'";

        private const string ITEM_AVAILABLE_QUERY = "select sum(\"Quantity\" - ifnull(\"U_EXK_CNRV\", 0)) " +
                                                    "from \"OIBT\" " +
                                                    "where \"ExpDate\" > TO_DATE('{0}', 'YYYY-MM-DD') " +
                                                    "and \"ItemCode\" = '{1}' " +
                                                    "and \"WhsCode\" = '{2}' ";


        private const string ITEM_AVAILABLE_QUERY_WO = "select sum(\"Quantity\") " +
                                                    "from \"OIBT\" " +
                                                    "where \"ExpDate\" > TO_DATE('{0}', 'YYYY-MM-DD') " +
                                                    "and \"ItemCode\" = '{1}' " +
                                                    "and \"WhsCode\" = '{2}' ";

        private const string ITEM_AVAILABLE_QUERY_WO_WOD = "select sum(\"Quantity\") " +
                                                    "from \"OIBT\" " +
                                                    "where \"ItemCode\" = '{0}' " +
                                                    "and \"WhsCode\" = '{1}' ";

        private const string ITEM_AVAILABLE_QUERY_WO_BY_BATCH = "select sum(\"Quantity\") " +
                                                  "from \"OIBT\" " +
                                                  "where \"ExpDate\" > TO_DATE('{0}', 'YYYY-MM-DD') " +
                                                  "and \"ItemCode\" = '{1}' " +
                                                  "and \"WhsCode\" = '{2}' " +
                                                  "and \"BatchNum\" = '{3}' ";

        private const string ITEM_AVAILABLE_QUERY_WO_BY_BATCH_WO_DATE = "select sum(\"Quantity\") " +
                                                "from \"OIBT\" " +
                                                "where " +
                                                " \"ItemCode\" = '{0}' " +
                                                "and \"WhsCode\" = '{1}' " +
                                                "and \"BatchNum\" = '{2}' ";

        private const string STOCK_QUERY = "select sum(\"OnHand\" - \"IsCommited\") from \"OITW\" " +
                                           " where \"ItemCode\" = '{0}'";

        public OITMRepository(Company company) : base(company)
        {
        }

        public override OITM GetItem(string id)
        {
            var recordSet = (RecordsetEx)Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx);
            recordSet.DoQuery(string.Format(ITEM_QUERY, id));
            var item = new OITM();
            item.ItemCode = recordSet.GetColumnValue("ItemCode").ToString();
            item.ItemDescription = recordSet.GetColumnValue("ItemName").ToString();
            item.InventoryMeasureUnitCode = recordSet.GetColumnValue("InvntryUom")?.ToString() ?? string.Empty;
            item.SaleUnitMeasureDescription = recordSet.GetColumnValue("UoMDesc")?.ToString() ?? string.Empty;
            item.SaleMeasureUnitCode = recordSet.GetColumnValue("SalUnitMsr")?.ToString() ?? string.Empty;
            item.InventoryUnitPerSale = recordSet.GetColumnValue("NumInSale").ToDecimal();
            item.LoadType = recordSet.GetColumnValue("U_EXK_TPRF")?.ToString() ?? string.Empty;
            item.SaleVolume = recordSet.GetColumnValue("SVolume").ToDecimal();
            item.SaleWeight = recordSet.GetColumnValue("SWeight1").ToDecimal();
            item.IsInventory = recordSet.GetColumnValue("InvntItem").ToString() == "Y";
            item.IsSale = recordSet.GetColumnValue("SellItem").ToString() == "Y";
            item.BoxPercent = recordSet.GetColumnValue("U_EXK_PRCJ").ToDecimal();
            item.SaleUnit = recordSet.GetColumnValue("NumInSale").ToDecimal();
            item.InventoryWeight = recordSet.GetColumnValue("IWeight1")?.ToDecimal() ?? decimal.Zero;
            item.TipoItem = recordSet.GetColumnValue("U_VS_TIPITM")?.ToString() ?? string.Empty;
            item.BuyUnit = recordSet.GetColumnValue("NumInBuy").ToDecimal();
            item.IsInventory = recordSet.GetColumnValue("InvntItem").ToString() == "Y" ? true : false;


            return item;
        }

        public override OITM FindByEDICode(string ediCode)
        {
            try
            {
                const string query = @"select UOM.""Name"" as ""UoMDesc"", ITM.* from ""OITM"" ITM left join ""@VS_UNID_MEDIDA"" UOM on ITM.""InvntryUom"" = UOM.""U_VS_UM_ABREV"" where ITM.""U_EXK_CFIL"" = '{0}'";
                OITM result = null;

                using (SafeRecordSet safeRecordSet = Company.MakeSafeRecordSet())
                {
                    safeRecordSet.ExecuteQuery(string.Format(query, ediCode));
                    safeRecordSet.ReadSingleRecord(t => result = populate_item(t));
                }

                return result;
            }
            catch (Exception)
            {
                throw new Exception($"El código EDI '{ediCode}' no esta configurado en el Maestro de Artículo (OITM).");
            }
        }

        private OITM populate_item(SafeRecordSet safeRecordSet)
        {
            var item = new OITM();
            item.ItemCode = safeRecordSet.GetString(@"ItemCode");
            item.ItemDescription = safeRecordSet.GetString(@"ItemName");
            item.InventoryMeasureUnitCode = safeRecordSet.GetString(@"InvntryUom");
            item.SaleUnitMeasureDescription = safeRecordSet.GetString("UoMDesc");
            item.SaleMeasureUnitCode = safeRecordSet.GetString("SalUnitMsr");
            item.InventoryUnitPerSale = safeRecordSet.TryGetDecimal("NumInSale");
            item.LoadType = safeRecordSet.GetString(@"U_EXK_TPRF");
            item.SaleVolume = safeRecordSet.TryGetDecimal(@"SVolume");
            item.SaleWeight = safeRecordSet.TryGetDecimal(@"SWeight1");
            item.IsInventory = safeRecordSet.GetString(@"InvntItem") == "Y";
            item.IsSale = safeRecordSet.GetString(@"SellItem") == "Y";
            item.BoxPercent = safeRecordSet.TryGetDecimal(@"U_EXK_PRCJ");
            item.SaleUnit = safeRecordSet.TryGetDecimal(@"NumInSale");
            item.InventoryWeight = safeRecordSet.TryGetDecimal(@"IWeight1");
            item.TipoItem = safeRecordSet.GetString(@"U_VS_TIPITM");
            item.BuyUnit = safeRecordSet.TryGetDecimal(@"NumInBuy");
            item.IsInventory = safeRecordSet.GetString(@"InvntItem") == "Y";
            //item.PriceListCategory = safeRecordSet.GetString(@"InvntItem");
            return item;
        }

        public override decimal AvaliableStock(string itemCode)
        {
            var recordSet = (RecordsetEx)Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx);
            recordSet.DoQuery(string.Format(STOCK_QUERY, itemCode));
            return recordSet.GetColumnValue(0).ToDecimal();
        }

        public override decimal AvaliableStockPerExpirationDate(string itemCode, DateTime expiration, string whscode)
        {
            var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            recordSet.DoQuery(string.Format(ITEM_AVAILABLE_QUERY, expiration.ToString("yyyy-MM-dd"), itemCode, whscode));
            return recordSet.GetColumnValue(0).ToDecimal();
        }

        public override decimal AvaliableStockPerExpirationDateWithoutReservation(string itemCode, DateTime expiration, string whscode)
        {
            var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            recordSet.DoQuery(string.Format(ITEM_AVAILABLE_QUERY_WO, expiration.ToString("yyyy-MM-dd"), itemCode, whscode));
            return recordSet.GetColumnValue(0).ToDecimal();
        }

      

        public override decimal AvaliableStockPerExpirationDateWithoutReservationWithoutDate(string itemCode, string whscode)
        {
            var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            recordSet.DoQuery(string.Format(ITEM_AVAILABLE_QUERY_WO_WOD, itemCode, whscode));
            return recordSet.GetColumnValue(0).ToDecimal();
        }

        public override decimal AvaliableStockPerExpirationDateWithoutReservationByBatch(string itemCode, DateTime expiration, string whscode, string batch)
        {
            var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            //var s = string.Format(ITEM_AVAILABLE_QUERY_WO_BY_BATCH, expiration.ToString("yyyy-MM-dd"), itemCode, whscode);
            recordSet.DoQuery(string.Format(ITEM_AVAILABLE_QUERY_WO_BY_BATCH, expiration.ToString("yyyy-MM-dd"), itemCode, whscode, batch));
            return recordSet.GetColumnValue(0).ToDecimal();
        }

        public override decimal AvaliableStockPerExpirationDateWithoutReservationByBatchWODate(string itemCode,  string whscode, string batch)
        {
            var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            //var s = string.Format(ITEM_AVAILABLE_QUERY_WO_BY_BATCH, expiration.ToString("yyyy-MM-dd"), itemCode, whscode);
            recordSet.DoQuery(string.Format(ITEM_AVAILABLE_QUERY_WO_BY_BATCH_WO_DATE, itemCode, whscode, batch));
            return recordSet.GetColumnValue(0).ToDecimal();
        }
    }
}