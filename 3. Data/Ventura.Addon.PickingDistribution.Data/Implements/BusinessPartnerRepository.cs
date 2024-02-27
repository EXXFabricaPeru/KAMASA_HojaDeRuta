using System.Collections.Generic;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Detail;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using Exxis.Addon.HojadeRutaAGuia.Data.Code;
using Exxis.Addon.HojadeRutaAGuia.Data.Repository;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Implements
{
    public class BusinessPartnerRepository : BaseBusinessPartnerRepository
    {
        private const string BUSINESS_PARTNER_FIND_QUERY = "select * from \"OCRD\" where \"CardCode\" = '{0}'";
        private const string ADDRESS_FIND_QUERY = "select * from \"CRD1\" where \"CardCode\" = '{0}'";
        private const string CONTACT_FIND_QUERY = "select * from \"OCPR\" where \"CardCode\" = '{0}'";

        public BusinessPartnerRepository(Company company) : base(company)
        {
        }

        public override OCRD FindByCode(string cardCode)
        {
            var recordset = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            recordset.DoQuery(string.Format(BUSINESS_PARTNER_FIND_QUERY, cardCode));
            var result = recordset.RetrieveBasicSAPEntity<OCRD>();

            if (RetrieveFormat != RetrieveFormat.Complete)
                return result;

            recordset.DoQuery(string.Format(ADDRESS_FIND_QUERY, cardCode));
            IList<CRD1> addresses = new List<CRD1>();
            while (!recordset.EoF)
            {
                var item = recordset.RetrieveBasicSAPEntity<CRD1>();
                addresses.Add(item);
                recordset.MoveNext();
            }
            IList<ContactPerson> contacts = new List<ContactPerson>();
            try
            {
                recordset.DoQuery(string.Format(CONTACT_FIND_QUERY, cardCode));
            
                while (!recordset.EoF)
                {
                    var item = recordset.RetrieveBasicSAPEntity<ContactPerson>();
                    contacts.Add(item);
                    recordset.MoveNext();
                }

            }
            catch (System.Exception)
            {
            }
            result.Contact = contacts;
            result.Addresses = addresses;
            
            return result;
        }
    }
}