using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exxis.Addon.HojadeRutaAGuia.Domain.Code;
using Exxis.Addon.HojadeRutaAGuia.Domain.Contracts;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Models;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.ServiceLayer.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.ServiceLayer.Detail;
using System.Linq.Expressions;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header.Document;

namespace Exxis.Addon.HojadeRutaAGuia.Domain
{
    public class LiquidacionTarjetasDomain : BaseDomain, ILiquidacionTarjetasDomain
    {
        public LiquidacionTarjetasDomain(Company company)
            : base(company)
        {

           
        }

        public Tuple<bool, string> actualizarPagosSap(List<int> list)
        {
            return UnitOfWork.LiquidacionTarjetasRepository.ActualizaPagoSAP( list);
        }

        public Tuple<bool, string> agregarDetalleLiquidacion(string codLiquidacion, List<LiquidationLineSL> detail)
        {
            return UnitOfWork.LiquidacionTarjetasRepository.agregarDetalleLiquidacion(codLiquidacion, detail);
        }

        public Tuple<bool, string> cancelarAsiento(string codAs)
        {
            return UnitOfWork.LiquidacionTarjetasRepository.cancelarAsiento(codAs);
        }

        public Tuple<bool, string, JournalEntrySL> generarAsiento(JournalEntrySL asiento)
        {
            return UnitOfWork.LiquidacionTarjetasRepository.GenerarAsiento(asiento);
        }

        public Tuple<bool, string> generarReconciliacion(ReconciliationSL reconciliation)
        {
            return UnitOfWork.LiquidacionTarjetasRepository.GenerarReconciliacion(reconciliation);
        }

        public Tuple<string, string> RetrieveCuentaTransitoria(string pasarela, string tienda)
        {
            return UnitOfWork.LiquidacionTarjetasRepository.RetrieveCuentaTransitoria(pasarela,tienda);
        }

        public Tuple<bool, string> RetrieveSocioNegocioPasarela(string pasarela)
        {
            return UnitOfWork.LiquidacionTarjetasRepository.RetrieveSocioNegocioPasarela(pasarela);
        }

        public LiquidationSL RetrieveDetalleLiquidacion(string codigo)
        {
            return UnitOfWork.LiquidacionTarjetasRepository.RetrieveDetalleLiquidacion(codigo);
        }

        public List<LiquidationLineSL> RetrieveDetalleLiquidacion(List<Tuple<string, string>> lista)
        {
            return UnitOfWork.LiquidacionTarjetasRepository.RetrieveDetalleLiquidacion(lista);
        }

        public string RetrieveMonedaPasarela(string pasarela)
        {
            return UnitOfWork.LiquidacionTarjetasRepository.RetrieveMonedaPasarela(pasarela);
        }

        public IEnumerable<Tuple<string, string>> RetrieveTiendas()
        {
            return UnitOfWork.LiquidacionTarjetasRepository.RetrieveTiendasData();
        }

        public SAPDocument searchInfoDocument(LTR1 result)
        {
            return UnitOfWork.LiquidacionTarjetasRepository.RetrieveDocumentByPasarela(result);
        }

        public Tuple<bool, string> RetrieveCuentaxMotivo(string codAjuste)
        {
            return UnitOfWork.LiquidacionTarjetasRepository.RetrieveCuentaxMotivo(codAjuste);
        }

        public Tuple<bool, LTR1> RetrieveDataPago(string value)
        {
            return UnitOfWork.LiquidacionTarjetasRepository.RetrieveDataPago(value);
        }

        public Tuple<bool, OHOJ> RetrieveHojaRuta(string value)
        {
            return UnitOfWork.LiquidacionTarjetasRepository.RetrieveHojaRuta(value);
        }

        public Tuple<bool, List<ODLN>> RetrieveGuiasHoja(string desde, string hasta, string programado, string zona)
        {
            return UnitOfWork.LiquidacionTarjetasRepository.RetrieveGuiasHoja(desde,  hasta,  programado,  zona);
        }

        public string RetrieveCodigoGenerado()
        {
            return UnitOfWork.LiquidacionTarjetasRepository.RetrieveCodigoGenerado();
        }

        public void ActualizarProgramado(string numeracion, string estado,ODLN datosGuias)
        {
            UnitOfWork.LiquidacionTarjetasRepository.ActualizarProgramado(numeracion, estado, datosGuias);
        }

        public void ActualizarEnvioSunat(string sunat)
        {
            UnitOfWork.LiquidacionTarjetasRepository.ActualizarEnvioSunat(sunat);
        }

        public void ActualizarEstadoHojaGuia(string estado,string codigo)
        {
            UnitOfWork.LiquidacionTarjetasRepository.ActualizarEstadoHojaGuia(estado, codigo);
        }

        public Tuple<bool, string> GetCargaUtilByPlaca(string placa)
        {
            return UnitOfWork.LiquidacionTarjetasRepository.GetCargaUtilByPlaca(placa);
        }

        public Tuple<bool, byte[]> ObtenerPDF(string numeracion)
        {
            return UnitOfWork.LiquidacionTarjetasRepository.ObtenerPDF(numeracion);
        }

        
    }
}
