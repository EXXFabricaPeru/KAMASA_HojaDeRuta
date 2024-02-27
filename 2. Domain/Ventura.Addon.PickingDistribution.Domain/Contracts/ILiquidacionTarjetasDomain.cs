using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Models;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.ServiceLayer.Detail;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.ServiceLayer.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header.Document;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.Domain.Code;

namespace Exxis.Addon.HojadeRutaAGuia.Domain.Contracts
{
    public interface ILiquidacionTarjetasDomain : IBaseDomain
    {
        IEnumerable<Tuple<string, string>> RetrieveTiendas();

        Tuple<string, string> RetrieveCuentaTransitoria(string pasarela,string tienda);

        string RetrieveMonedaPasarela(string pasarela);
        SAPDocument searchInfoDocument(LTR1 result);
        Tuple<bool, string, JournalEntrySL> generarAsiento(JournalEntrySL asiento);
        Tuple<bool, string> cancelarAsiento(string codAs);
        Tuple<bool, string> generarReconciliacion(ReconciliationSL reconciliation);

        Tuple<bool, string> agregarDetalleLiquidacion(string codLiquidacion, List<LiquidationLineSL> listDetailLiquidation);
        Tuple<bool, string> actualizarPagosSap(List<int> list);
        LiquidationSL RetrieveDetalleLiquidacion(string codigo);

        List<LiquidationLineSL> RetrieveDetalleLiquidacion(List<Tuple<string, string>> lista);

        Tuple<bool, string> RetrieveSocioNegocioPasarela(string pasarela);
        Tuple<bool, string> RetrieveCuentaxMotivo(string v);
        Tuple<bool, LTR1> RetrieveDataPago(string value);
        Tuple<bool, OHOJ> RetrieveHojaRuta(string value);
        Tuple<bool, List<ODLN>> RetrieveGuiasHoja(string value1, string value2, string value3, string v);
        string RetrieveCodigoGenerado();
        void ActualizarProgramado(string numeracion, string estado);
        void ActualizarEnvioSunat(string sunat);
        void ActualizarEstadoHojaGuia(string estado, string codigo);
    }
}
