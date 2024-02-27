using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using Exxis.Addon.HojadeRutaAGuia.Data.Repository;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Implements
{
    public class EmployeeRepository : BaseEmployeeRepository
    {
        private const string EMPLOYEE_FIND_QUERY = "select * from \"OHEM\" {0}";

        public EmployeeRepository(Company company) : base(company)
        {
        }

        public override OHEM FindByCode(string cardCode)
        {
            var recordset = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            recordset.DoQuery(string.Format(EMPLOYEE_FIND_QUERY, $"where \"Code\" = '{cardCode}'"));
            return recordset.RetrieveBasicSAPEntity<OHEM>();
        }

        public override IEnumerable<OHEM> Retrieve(Expression<Func<OHEM, bool>> expression = null)
        {
            string whereStatement = expression == null ? string.Empty : $"where {QueryHelper.ParseToHANAQuery(expression)}";
            return get_employees(string.Format(EMPLOYEE_FIND_QUERY, whereStatement));
        }

        private IEnumerable<OHEM> get_employees(string query)
        {
            IEnumerable<PropertyInfo> entityProperties = CurrentReferenceType
                .RetrieveSAPPropertiesWithoutChild()
                .ToList();

            IList<OHEM> motives = new List<OHEM>();

            var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx)
                .To<RecordsetEx>();
            recordSet.DoQuery(query);
            while (!recordSet.EoF)
            {
                var motive = recordSet.MakeBasicSAPEntity<OHEM>(entityProperties);
                motives.Add(motive);
                recordSet.MoveNext();
            }

            GenericHelper.ReleaseCOMObjects(recordSet);

            return motives;
        }
    }
}