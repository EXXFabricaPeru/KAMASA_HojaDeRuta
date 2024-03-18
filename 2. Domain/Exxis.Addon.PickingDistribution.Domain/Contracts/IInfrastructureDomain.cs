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

        string RetrieveDescriptionOfValidValueCode(string field, string code);

        IEnumerable<Tuple<string, string>> RetrieveSaleChannels();

        CrossCutting.Model.System.Header.OCRD RetrieveBusinessPartner(string cardCode);


        IEnumerable<Tuple<string, string>> RetrieveValidValues<TEntity, TKProperty>(Expression<Func<TEntity, TKProperty>> propertyExpression)
            where TEntity : BaseUDO;


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
