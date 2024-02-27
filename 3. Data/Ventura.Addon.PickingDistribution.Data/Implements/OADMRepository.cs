using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header;
using Exxis.Addon.HojadeRutaAGuia.Data.Repository;
using static Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header.OADM;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Implements
{
    // ReSharper disable once InconsistentNaming
    public class OADMRepository : BaseOADMRepository
    {
        public OADMRepository(Company company) : base(company)
        {
        }

        public override OADM CurrentCompany
        {
            get
            {
                var recordSet = (RecordsetEx) Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx);
                recordSet.DoQuery("select * from \"OADM\"");
                OADM _oadm = new OADM();
                ADM1 _adm1 = new ADM1();
                while (!recordSet.EoF)
                {
                    _oadm.CompanyName = recordSet.GetColumnValue("CompnyName").ToString();
                    _oadm.CompanyAddress = recordSet.GetColumnValue("CompnyAddr").ToString();
                    recordSet.MoveNext();
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(recordSet);
                recordSet = null;
                recordSet = (RecordsetEx)Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx);
                recordSet.DoQuery("select * from \"ADM1\"");

                while (!recordSet.EoF)
                {
                    _adm1.Ubigeo = recordSet.GetColumnValue("GlblLocNum").ToString();
                    recordSet.MoveNext();

                }
                _oadm.detailCompany = _adm1;

                return _oadm;
            }
        }
    }
}