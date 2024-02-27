using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DisposableSAPBO.RuntimeMapper.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using Exxis.Addon.HojadeRutaAGuia.Data.Repository;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Implements
{
    public class TariffMotiveRepository : BaseTariffMotiveRepository
    {
        private static readonly string MOTIVE_QUERY = $"select * from \"@{OTMT.ID}\" {{0}}";

        public TariffMotiveRepository(SAPbobsCOM.Company company) : base(company)
        {
        }

        #region Overrides of BaseTariffMotiveRepository

        public override IEnumerable<OTMT> Retrieve(Expression<Func<OTMT, bool>> expression)
        {
            string whereStatement = expression == null ? string.Empty : $"where {QueryHelper.ParseToHANAQuery(expression)}";
            return get_motives(string.Format(MOTIVE_QUERY, whereStatement));
        }

        private IEnumerable<OTMT> get_motives(string query)
        {
            IList<OTMT> motives = new List<OTMT>();
            
            var recordSet = Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordsetEx).To<SAPbobsCOM.RecordsetEx>();
            recordSet.DoQuery(query);
            
            IEnumerable<PropertyInfo> entityProperties = CurrentReferenceType.RetrieveSAPProperties()
                .OrderBy(item => item.GetCustomAttribute<ChildProperty>() != null ? 1 : 0)
                .ToList();

            while (!recordSet.EoF)
            {
                var motive = recordSet.MakeBasicSAPEntity<OTMT>(entityProperties);
                motives.Add(motive);
                recordSet.MoveNext();
            }

            GenericHelper.ReleaseCOMObjects(recordSet);

            return motives;
        }

        #endregion
    }
}