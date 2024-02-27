using System.Collections.Generic;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.Data.Repository;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Implements
{
    // ReSharper disable once InconsistentNaming
    public class ODSPRepository : BaseODSPRepository
    {
        private static readonly string LOAD_TYPE_PRIORITY_QUERY = $"select * from \"@{ODSP.ID}\"";

        public ODSPRepository(Company company) : base(company)
        {
        }

        public override IEnumerable<ODSP> RetrieveAll()
        {
            IList<ODSP> result = new List<ODSP>();
            var recordSet = (RecordsetEx) Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx);
            recordSet.DoQuery(LOAD_TYPE_PRIORITY_QUERY);
            while (!recordSet.EoF)
            {
                var item = new ODSP
                {
                    Code = recordSet.GetColumnValue("Code").ToInt32(),
                    Name = recordSet.GetColumnValue("Name").ToString(),
                    ItemLoadType = recordSet.GetColumnValue("U_EXK_TPCR").ToString(),
                    LoadTypePriority = recordSet.GetColumnValue("U_EXK_TPPR").ToInt32(),
                };
                result.Add(item);
                recordSet.MoveNext();
            }
            return result;
        }
    }
}