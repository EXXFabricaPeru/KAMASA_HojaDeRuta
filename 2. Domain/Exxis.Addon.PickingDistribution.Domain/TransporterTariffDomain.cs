using System;
using System.Collections.Generic;
using System.Linq;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.Domain.Code;
using Exxis.Addon.HojadeRutaAGuia.Domain.Contracts;

namespace Exxis.Addon.HojadeRutaAGuia.Domain
{
    public class TransporterTariffDomain : BaseDomain, ITransporterTariffDomain
    {
        public TransporterTariffDomain(Company company) : base(company)
        {
        }

        #region Implementation of ITransporterTariffDomain

        public OATP RetrieveByTransporterCode(string code)
        {
            List<OATP> tariff = UnitOfWork.TariffRepository.Retrieve(t => t.SupplierId == code)
                .ToList();

            if (!tariff.Any())
                throw new Exception(@"No existe tarifario configurado para el transportista.");

            if (tariff.Count > 1)
                throw new Exception(@"Existe mas de un tarifario configurado para el transportista.");

            return tariff.Single();
        }

        public bool ExistTransporter(string code)
        {
            return UnitOfWork.TariffRepository.Retrieve(t => t.SupplierId == code)
                .Any();
        }

        #endregion
    }
}