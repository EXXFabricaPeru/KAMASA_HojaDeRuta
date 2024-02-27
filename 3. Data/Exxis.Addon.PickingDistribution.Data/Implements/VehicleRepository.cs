using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.Localization.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using Exxis.Addon.HojadeRutaAGuia.Data.Repository;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Implements
{
    public class VehicleRepository : BaseVehicleRepository
    {
        private const string DRIVER_FIND_QUERY = "select * from \"@BPP_VEHICU\" {0}";

        public VehicleRepository(Company company) : base(company)
        {
        }

        public override BPP_VEHICU FindByCode(string code)
        {
            var recordset = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            var x = string.Format(DRIVER_FIND_QUERY, $"where \"Name\"='{code}'");
            recordset.DoQuery(string.Format(DRIVER_FIND_QUERY, $"where \"Name\"='{code}'"));

            var result = new BPP_VEHICU();

            IEnumerable<Tuple<PropertyInfo, string>> tuplesProperties = result.GetType()
                .RetrieveSAPProperties()
                .Select(item =>
                {
                    if (item.IsColumnProperty())
                        return Tuple.Create(item, item.RetrieveColumnProperty().ColumnName);

                    if (item.IsFieldNoRelated())
                        return Tuple.Create(item, item.RetrieveFieldNoRelated().sAliasID);

                    return null;
                })
                .Where(item => item != null);

            foreach (var tupleProperty in tuplesProperties)
            {
                var value = recordset.GetColumnValue(tupleProperty.Item2);
                tupleProperty.Item1.SetValue(result, value);
            }

            return result;
        }
    }
}