using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.Domain.Code;

namespace Exxis.Addon.HojadeRutaAGuia.Domain.Contracts
{
    public interface ISettingsDomain : IBaseDomain
    {
        void ChangeClosingState();
        void ChangeClosingStateAddon();
        OPDS ClosingHour { get; }
        OPDS ClosingState { get; }
        OPDS AverageWeight { get; }
        OPDS RouteQuantity { get; }
        OPDS RestAPIPath { get; }
        OPDS RoutingWebPath { get; }
        OPDS DistributionChannelWithoutReference { get; }
        OPDS DistributionTerritoryWithoutReference { get; }        
        OPDS ReturnWarehouse { get; }
        OPDS TaxForSalesOrderCharge { get; }
        OPDS TransporterGeneric { get; }
        OPDS DirSharedCrystalPath { get; }
        OPDS Impresora { get; }
        OPDS ImpresoraMatricial { get; }
        OPDS CorreoExtDesaprobador { get; }
        OPDS PassExtDesprobador { get; }
        OPDS PuertoMail { get; }
        OPDS SMPTMail { get; }
        OPDS WareHousePickUp { get; }
        OPDS DisapprovedSaleOrderEmailFilePath { get; }
        OPDS SerieDefaultDelivery { get; }
        OPDS SerieDefaultInvoice { get; }
        OPDS TemporalWareHouse { get; }
        OPDS PrincipalWareHouse { get; }

        /////// COMISION TARJETAS
        OPDS IPServiceLayer { get; }

        OPDS StoresExcludes { get; }

        OPDS AccountComision { get; }

        OPDS RoundValue { get; }

    }
}