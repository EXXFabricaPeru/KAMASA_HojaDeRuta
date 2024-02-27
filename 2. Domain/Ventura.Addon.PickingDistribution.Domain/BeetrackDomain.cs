using System;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.Domain.Code;
using Exxis.Addon.HojadeRutaAGuia.Domain.Contracts;

namespace Exxis.Addon.HojadeRutaAGuia.Domain
{
    public class BeetrackDomain : BaseDomain, IBeetrackDomain
    {
        public BeetrackDomain(Company company) : base(company)
        {
        }

        public Tuple<string, string> GetCredentials()
        {
            return  Tuple.Create ( UnitOfWork.SettingsRepository.Setting(OPDS.Codes.KEY_API_BEETRACK).Value, UnitOfWork.SettingsRepository.Setting(OPDS.Codes.URL_API_BEETRACK).Value);
        }

        public bool IsEnable()
        {
            OPDS enabled = UnitOfWork.SettingsRepository.Setting(OPDS.Codes.ENABLE_API_BEETRACK);
            return enabled.Value == "1";
        }
    }
}
