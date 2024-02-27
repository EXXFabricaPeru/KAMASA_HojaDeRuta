using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.Domain.Code;

namespace Exxis.Addon.HojadeRutaAGuia.Domain.Contracts
{
    public interface IInfrastructureDomain: IBaseDomain
    {
        IEnumerable<OMTT> RetrieveApprovalReasons();
        IEnumerable<OMTT> RetrieveDisapprovalReasons();
        IEnumerable<OMTT> RetrieveReasons();

        OMTT RetrieveDisapprovalReasonByCode(string code);
        ODSP RetrieveLoadPriorityByLoadType(string loadType);
        string RetrieveDescriptionOfValidValueCode(string field, string code);
        string RetrieveNextDistributionStatus(string code);
        string RetrieveOppositeDistributionStatus(string code);
        string RetrieveOppositeDistributionTransferOrderStatus(string code);
        IEnumerable<Tuple<string, string>> RetrieveSaleChannels();
        IEnumerable<OTMI> RetrieveSAPDocumentMappingStatus();
        CrossCutting.Model.System.Header.OCRD RetrieveBusinessPartner(string cardCode);
        IEnumerable<OTMT> RetrievePenaltyMotives();
        IEnumerable<OTMT> RetrieveExtraMotives();

        IEnumerable<Tuple<string, string>> RetrieveValidValues<TEntity, TKProperty>(Expression<Func<TEntity, TKProperty>> propertyExpression)
            where TEntity : BaseUDO;

        IEnumerable<OEIT> RetrieveMappingValuesByTemplate(string templateCode);

        string RetrieveLastNumberBySerie(string Serie, string tipo);

        void SendAlertMessageBySaleOrderCashPayment(int documentEntry);

        List<Tuple<string,string>> RetriveMotiveTransferSoraya();

        List<Tuple<string, string>> RetriveMotiveTransferSunat();

        string RetrievePaymentGroupNumDescription(int code);

        void validarTipoCambio(DateTime fchTrn);

        decimal obtenerTipoCambio(DateTime fecha);

        IEnumerable<Tuple<string, string>> RetrieveTipoFlujos();


    }
}
