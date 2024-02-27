using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using Exxis.Addon.HojadeRutaAGuia.Data.Repository;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Implements
{
    public class OTRDRepository : BaseOTRDRepository
    {
        private const string TRANSPORTER_QUERY = "select * from \"@VS_PD_OTRD\" {0}";

        public OTRDRepository(Company company) : base(company)
        {
        }

        public override IEnumerable<OTRD> RetrieveTransporters(Expression<Func<OTRD, bool>> expression = null)
        {
            IList<OTRD> result = new List<OTRD>();
            var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            var whereStatement = string.Empty;
            if (expression != null)
                whereStatement = $"where {QueryHelper.ParseToHANAQuery(expression)}";

            recordSet.DoQuery(string.Format(TRANSPORTER_QUERY, whereStatement));
            while (!recordSet.EoF)
            {
                var item = new OTRD();
                item.SupplierName = recordSet.GetColumnValue("U_EXK_DSPR").ToString();
                item.SupplierDocumentId = recordSet.GetColumnValue("U_EXK_NMDC").ToString();
                item.SupplierId = recordSet.GetColumnValue("Code").ToString();
                item.RateType = recordSet.GetColumnValue(@"U_EXK_TPTR")?.ToString()?? string.Empty;
                item.CodeType = recordSet.GetColumnValue(@"U_EXK_CDTR")?.ToString() ?? string.Empty;
                item.SupplierTransferModality = recordSet.GetColumnValue(@"U_EXK_MDTR")?.ToString() ?? string.Empty;
                item.SupplierMTC = recordSet.GetColumnValue(@"U_EXK_MTTR")?.ToString() ?? string.Empty;
                item.Drivers = retrieve_conductors(item.SupplierId);
                item.Vehicles = retrieve_vehicles(item.SupplierId);
                result.Add(item);
                recordSet.MoveNext();
            }

            return result;
        }

        private List<TRD1> retrieve_conductors(string code)
        {
            List<TRD1> result = new List<TRD1>();
            string query = $"select * from \"@VS_PD_TRD1\" where \"Code\" = '{code}'";
            var recordSet = (RecordsetEx) Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx);
            recordSet.DoQuery(query);
            while (!recordSet.EoF)
            {
                var item = new TRD1();
                item.Code = recordSet.GetColumnValue("Code").ToString();
                item.LineId = recordSet.GetColumnValue("LineId").ToInt32();
                item.DriverId = recordSet.GetColumnValue("U_EXK_CDCD")?.ToString() ?? string.Empty;
                item.DriveLicense = recordSet.GetColumnValue("U_EXK_LCCD")?.ToString() ?? string.Empty;
                item.DriverName = recordSet.GetColumnValue("U_EXK_NMCD")?.ToString() ?? string.Empty;
                result.Add(item);
                recordSet.MoveNext();
            }

            return result;
        }

        private List<TRD2> retrieve_vehicles(string code)
        {
            List<TRD2> result = new List<TRD2>();
            string query = $"select * from \"@VS_PD_TRD2\" where \"Code\" = '{code}'";
            var recordSet = (RecordsetEx) Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx);
            recordSet.DoQuery(query);
            while (!recordSet.EoF)
            {
                var item = new TRD2();
                item.Code = recordSet.GetColumnValue("Code").ToString();
                item.LineId = recordSet.GetColumnValue("LineId").ToInt32();
                item.LisencePlate = recordSet.GetColumnValue("U_EXK_VEPL")?.ToString() ?? string.Empty;
                item.Identifier = recordSet.GetColumnValue("U_EXK_CDVH").ToString();
                item.VolumeCapacity = recordSet.GetColumnValue("U_EXK_VLDC").ToDecimal();
                item.LoadCapacity = recordSet.GetColumnValue("U_EXK_CPDC").ToDecimal();
                item.DistributionType = recordSet.GetColumnValue("U_EXK_TPDS")?.ToString() ?? string.Empty;
                item.HasRefrigeration = recordSet.GetColumnValue("U_EXK_SRVH")?.ToString() ?? string.Empty;
                item.IsAvailable = recordSet.GetColumnValue("U_EXK_ACVH")?.ToString() ?? string.Empty;
                result.Add(item);
                recordSet.MoveNext();
            }

            return result;
        }
    }
}