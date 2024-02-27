using System.Collections.Generic;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.Data.Repository;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Implements
{
    // ReSharper disable once InconsistentNaming
    public class OTDIRepository : BaseOTDIRepository
    {
        public OTDIRepository(Company company) : base(company)
        {
        }

        public override IEnumerable<OTMI> MappingValue(string originName)
        {
            var recordSet = (RecordsetEx) Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx);
            recordSet.DoQuery($"select * from \"@{OTMI.ID}\" where \"U_EXK_SORC\" = '{originName}'");
            IList<OTMI> transfer = new List<OTMI>();
            while (!recordSet.EoF)
            {
                var mapping = new OTMI
                {
                    Code = recordSet.GetColumnValue("Code").ToString(),
                    Name = recordSet.GetColumnValue("Name").ToString(),
                    Source = recordSet.GetColumnValue("U_EXK_SORC").ToString(),
                    OriginValue = recordSet.GetColumnValue("U_EXK_ORVL").ToString(),
                    TargetValue = recordSet.GetColumnValue("U_EXK_DSVL").ToString()
                };
                transfer.Add(mapping);
                recordSet.MoveNext();
            }
            return transfer;
        }
    }
}
