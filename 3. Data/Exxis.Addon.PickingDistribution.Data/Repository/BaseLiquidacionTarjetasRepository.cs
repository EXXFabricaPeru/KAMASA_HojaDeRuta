using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Models;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.ServiceLayer.Detail;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.ServiceLayer.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header.Document;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Repository
{
    public abstract  class BaseLiquidacionTarjetasRepository : Code.Repository
    {
        protected BaseLiquidacionTarjetasRepository(Company company) : base(company)
        {

        }

        public abstract IEnumerable<Tuple<string, string>> RetrieveTiendasData();

        public abstract Tuple<string, string> RetrieveCuentaTransitoria(string Pasarela, string Tienda);

        public abstract string RetrieveMonedaPasarela(string Pasarela);
        public abstract Tuple<bool, string> ActualizaPagoSAP(List<int> list);
        public abstract Tuple<bool, string> cancelarAsiento(string codAs);
        public abstract Tuple<bool, string> agregarDetalleLiquidacion(string codLiquidacion, List<LiquidationLineSL> detail);
        public abstract SAPDocument RetrieveDocumentByPasarela(LTR1 result);
        public abstract Tuple<bool, string, JournalEntrySL> GenerarAsiento(JournalEntrySL asiento);
        public abstract Tuple<bool, string> GenerarReconciliacion(ReconciliationSL reconciliation);
        public abstract LiquidationSL RetrieveDetalleLiquidacion(string codigo);
        public abstract List<LiquidationLineSL> RetrieveDetalleLiquidacion(List<Tuple<string, string>> lista);
        public abstract Tuple<bool, string> RetrieveSocioNegocioPasarela(string pasarela);
        public abstract Tuple<bool, string> RetrieveCuentaxMotivo(string codAjuste);
        public abstract Tuple<bool, LTR1> RetrieveDataPago(string value);
        public abstract Tuple<bool, OHOJ> RetrieveHojaRuta(string value);
        public abstract Tuple<bool, List<ODLN>> RetrieveGuiasHoja(string desde, string hasta, string programado, string zona);
        public abstract string RetrieveCodigoGenerado();
        public abstract void ActualizarProgramado(string numeracion, string estado);
        public abstract void ActualizarEnvioSunat(string sunat);
        public abstract void ActualizarEstadoHojaGuia(string estado,string codigo);
        public abstract Tuple<bool, string> GetCargaUtilByPlaca(string placa);
        public abstract Tuple<bool, string> ObtenerPDF(string numeracion);
    }
}
