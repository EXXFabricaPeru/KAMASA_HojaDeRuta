using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DisposableSAPBO.RuntimeMapper.Attributes;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using Exxis.Addon.HojadeRutaAGuia.Data.Repository;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Implements
{
    public class CertificateAssignmentRepository : BaseCertificateAssignmentRepository
    {
        private static readonly string CERTIFICATE_ASSIGNMENT_QUERY = $"select * from \"@{OACD.ID}\" {{0}}";
        private static readonly string CERTIFICATE_ASSIGNMENT_DETAIL_QUERY = $"select * from \"@{ACD1.ID}\" where \"Code\" = '{{0}}'";

        private IEnumerable<PropertyInfo> _baseProperties;
        private IEnumerable<PropertyInfo> _childProperties;

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
            => _childProperties ?? (_childProperties = typeof(ACD1).RetrieveSAPProperties().ToList());

        public CertificateAssignmentRepository(Company company) : base(company)
        {
        }

        public override IEnumerable<OACD> Retrieve(Expression<Func<OACD, bool>> expression)
        {
            string whereStatement = expression == null ? string.Empty : $"where {QueryHelper.ParseToHANAQuery(expression)}";
            return get_assignments(string.Format(CERTIFICATE_ASSIGNMENT_QUERY, whereStatement));
        }

        private IEnumerable<OACD> get_assignments(string query)
        {
            IList<OACD> result = new List<OACD>();

            var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            recordSet.DoQuery(query);
            
            while (!recordSet.EoF)
            {
                var document = SapHelper.MakeEntityByRecordSet<OACD>(recordSet, BaseProperties);
                document.Certificates = get_assignment_detail(document.Code);
                result.Add(document);
                recordSet.MoveNext();
            }

            return result;
        }

        private List<ACD1> get_assignment_detail(string documentCode)
        {
            var result = new List<ACD1>();
            var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            recordSet.DoQuery(string.Format(CERTIFICATE_ASSIGNMENT_DETAIL_QUERY, documentCode));
            
            while (!recordSet.EoF)
            {
                var document = SapHelper.MakeEntityByRecordSet<ACD1>(recordSet, ChildProperties);
                result.Add(document);
                recordSet.MoveNext();
            }

            return result;
        }
    }
}