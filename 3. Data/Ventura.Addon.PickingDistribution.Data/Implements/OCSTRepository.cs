using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header;
using Exxis.Addon.HojadeRutaAGuia.Data.Repository;
using System.Collections.Generic;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Implements
{
    // ReSharper disable once InconsistentNaming
    public class OCSTRepository : BaseOCSTRepository
    {
        public OCSTRepository(Company company) : base(company)
        {
        }

        public override List<OCST> ListDepartments
        {
            get
            {
                var recordSet = (RecordsetEx) Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx);
                recordSet.DoQuery("select * from \"OCST\" where \"Country\"='PE'");
                List<OCST> deps = new List<OCST>();
                while (!recordSet.EoF)
                {

                    deps.Add(new OCST
                    {
                        Code= recordSet.GetColumnValue("Code").ToString(),
                        Name = recordSet.GetColumnValue("Name").ToString()
                    });


                    recordSet.MoveNext();
                }
                return deps;
                
            }
        }
    }
}