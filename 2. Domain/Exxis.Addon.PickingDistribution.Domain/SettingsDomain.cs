using System;
using System.Linq;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.Domain.Code;
using Exxis.Addon.HojadeRutaAGuia.Domain.Contracts;

namespace Exxis.Addon.HojadeRutaAGuia.Domain
{
    public class SettingsDomain : BaseDomain, ISettingsDomain
    {
        public SettingsDomain(Company company)
            : base(company)
        {
        }

      

        public OPDS RestAPIPath
            => UnitOfWork.SettingsRepository.Setting(OPDS.Codes.REST_API_PATH);

        public OPDS ClosingHour
            => UnitOfWork.SettingsRepository.Setting(OPDS.Codes.CLOSING_HOUR);

        public OPDS ClosingState
            => UnitOfWork.SettingsRepository.Setting(OPDS.Codes.CLOSING_STATUS);

        public OPDS AverageWeight
            => UnitOfWork.SettingsRepository.Setting(OPDS.Codes.ROUTES_AVERAGE_WEIGHT);

        public OPDS RouteQuantity
            => UnitOfWork.SettingsRepository.Setting(OPDS.Codes.ROUTES_DEFAULT_QUANTITY);

        public OPDS RoutingWebPath
            => UnitOfWork.SettingsRepository.Setting(OPDS.Codes.ROUTING_WEB_PATH);

        public OPDS DistributionChannelWithoutReference
            => UnitOfWork.SettingsRepository.Setting(OPDS.Codes.DISTRIBUTION_CHANNEL_WITHOUT_REFERENCE);

        public OPDS DistributionTerritoryWithoutReference
            => UnitOfWork.SettingsRepository.Setting(OPDS.Codes.DISTRIBUTION_TERRITORY_WITHOUT_REFERENCE);

        public OPDS ReturnWarehouse
            => UnitOfWork.SettingsRepository.Setting(OPDS.Codes.RETURN_WAREHOUSE);

        public OPDS TaxForSalesOrderCharge
            => UnitOfWork.SettingsRepository.Setting(OPDS.Codes.CHARGE_TAX);

        public OPDS TransporterGeneric
            => UnitOfWork.SettingsRepository.Setting(OPDS.Codes.TRANSPORTER_GENERIC);

        public OPDS DirSharedCrystalPath
          => UnitOfWork.SettingsRepository.Setting(OPDS.Codes.CRYSTAL_SHARED_PATH);

        public OPDS Impresora
          => UnitOfWork.SettingsRepository.Setting(OPDS.Codes.PRINTER_NAME);

        public OPDS ImpresoraMatricial
          => UnitOfWork.SettingsRepository.Setting(OPDS.Codes.PRINTER_MAT_NAME);

        public OPDS CorreoExtDesaprobador
          => UnitOfWork.SettingsRepository.Setting(OPDS.Codes.EMAIL_EXT);

        public OPDS PassExtDesprobador
          => UnitOfWork.SettingsRepository.Setting(OPDS.Codes.EMAIL_EXT_PASS);

        public OPDS PuertoMail
          => UnitOfWork.SettingsRepository.Setting(OPDS.Codes.EMAIL_PORT);

        public OPDS SMPTMail
          => UnitOfWork.SettingsRepository.Setting(OPDS.Codes.EMAIL_SMPT);

        public OPDS WareHousePickUp
          => UnitOfWork.SettingsRepository.Setting(OPDS.Codes.WHS_PICK);

        public OPDS DisapprovedSaleOrderEmailFilePath
            => UnitOfWork.SettingsRepository.Setting(OPDS.Codes.DISAPPROVED_SALE_ORDER_EMAIL_FILEPATH);

        public OPDS SerieDefaultDelivery
        => UnitOfWork.SettingsRepository.Setting(OPDS.Codes.SERIE_DELIVERY);

        public OPDS SerieDefaultInvoice
        => UnitOfWork.SettingsRepository.Setting(OPDS.Codes.SERIE_INVOICE);

        public OPDS TemporalWareHouse
           => UnitOfWork.SettingsRepository.Setting(OPDS.Codes.TEMPORAL_WAREHOUSE);

        public OPDS PrincipalWareHouse
           => UnitOfWork.SettingsRepository.Setting(OPDS.Codes.PRINCIPAL_WAREHOUSE);

        public OPDS IPServiceLayer
          => UnitOfWork.SettingsRepository.Setting(OPDS.Codes.IP_SERVICE_LAYER);

        public OPDS StoresExcludes
        => UnitOfWork.SettingsRepository.Setting(OPDS.Codes.STORES_EXCLUDES);

        public OPDS AccountComision
        => UnitOfWork.SettingsRepository.Setting(OPDS.Codes.ACCOUNT_COMISION);

        public OPDS RoundValue
        => UnitOfWork.SettingsRepository.Setting(OPDS.Codes.ROUND_VALUE);

        public OPDS RutaCompartida
            => UnitOfWork.SettingsRepository.Setting(OPDS.Codes.RUTA_COMPARTIDA);

        public OPDS UserDB
           => UnitOfWork.SettingsRepository.Setting(OPDS.Codes.DBUSER);

        public OPDS PassDB
           => UnitOfWork.SettingsRepository.Setting(OPDS.Codes.DBPASS);

        public void ValidData()
        {
            UnitOfWork.SettingsRepository.ValidData();
        }
    }
}