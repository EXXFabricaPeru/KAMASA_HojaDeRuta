using B1SLayer;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Models;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.Local;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.ServiceLayer.Detail;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.ServiceLayer.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Detail.DocumentLine;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header.Document;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using Exxis.Addon.HojadeRutaAGuia.Data.Code;
using Exxis.Addon.HojadeRutaAGuia.Data.Repository;
using Sap.Data.Hana;
using System.IO;
using System.Diagnostics;
using System.Data;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Implements
{
    public class LiquidacionTarjetasRepository : BaseLiquidacionTarjetasRepository
    {
        public SLConnection Login;
        public LiquidacionTarjetasRepository(Company company) : base(company)
        {

            ServiceLayerHelper serviceLayerHelper = new ServiceLayerHelper();

            //var _company = company.CompanyDB;
            //var user = company.UserName;
            //var pass = "B1Admin$$";
            //Login = serviceLayerHelper.Login(company, _company, user, pass);

        }

        public override IEnumerable<Tuple<string, string>> RetrieveTiendasData()
        {
            try
            {
                List<Tuple<string, string>> result = new List<Tuple<string, string>>();

                //var list= TiendasList(Login);

                var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
                var query = "Select * from \"@CL_CODEMP\"";
                recordSet.DoQuery(string.Format(query));

                while (!recordSet.EoF)
                {
                    var Code = recordSet.GetColumnValue("Code").ToString();
                    var Name = recordSet.GetColumnValue("Name").ToString();
                    result.Add(Tuple.Create(Code, Name));
                    recordSet.MoveNext();
                }

                //    foreach (var item in list.Result)
                //{
                //    result.Add(Tuple.Create(item.Code, item.Name));
                //}

                return result;
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects();
            }
        }


        private async Task<List<OCEM>> TiendasList(SLConnection login)
        {


            var bpList = await login.Request(OCEM.ID)
               //.Filter("startswith(CardCode, 'c')")
               .Select("Code, Name")
               //.OrderBy("CardName")
               .WithPageSize(150)
               .WithCaseInsensitive()
               .GetAsync<List<OCEM>>();

            return bpList;
        }

        public override Tuple<string, string> RetrieveCuentaTransitoria(string Pasarela, string Tienda)
        {
            try
            {

                //var list= TiendasList(Login);

                var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
                var query = "Select * from \"OACT\" where \"U_VS_LT_PASP\"='{0}' and \"U_VS_LT_COTI\"='{1}'";
                recordSet.DoQuery(string.Format(query, Pasarela, Tienda));

                while (!recordSet.EoF)
                {
                    var Code = recordSet.GetColumnValue("AcctCode").ToString();
                    var Name = recordSet.GetColumnValue("AcctName").ToString();
                    return Tuple.Create(Code, Name);
                    recordSet.MoveNext();
                }

                return Tuple.Create("", "");
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects();
            }
        }

        public override string RetrieveMonedaPasarela(string Pasarela)
        {
            try
            {

                //var list= TiendasList(Login);

                var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
                var query = "Select * from \"@VS_LT_OPAP\" where \"Code\"='{0}' ";
                recordSet.DoQuery(string.Format(query, Pasarela));

                while (!recordSet.EoF)
                {
                    var moneda = recordSet.GetColumnValue("U_VS_LT_MONE").ToString();
                    return moneda;
                    recordSet.MoveNext();
                }

                return "";
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects();
            }
        }

        public override SAPDocument RetrieveDocumentByPasarela(LTR1 result)
        {
            try
            {

                //var list= TiendasList(Login);
                var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
                var query = "Select R1.* from \"ORCT\" R  JOIN \"RCT2\" R1 on R.\"DocEntry\"=R1.\"DocNum\" where R.\"U_VS_LT_STAD\"<>'Y' AND RIGHT(\"U_NDP_NROTAR\",4)='{0}'  AND \"DocDate\"='{1}' and \"DocTotal\" ={2}";


                var _date = result.FechaCobro.ToString("yyyyMMdd");
                var _query = string.Format(query,
                    ObtenerUltimosCuatroDigitos(result.NroTarjeta),
                    result.FechaCobro.ToString("yyyyMMdd"),
                    result.ImporteCobradoTarjeta);

                recordSet.DoQuery(_query);

                int DocEntry = 0;
                int DocNum = 0;
                int TypeDoc = 0;

                while (!recordSet.EoF)
                {
                    DocEntry = recordSet.GetColumnValue("DocEntry").ToInt32();
                    TypeDoc = recordSet.GetColumnValue("InvType").ToInt32();
                    DocNum = recordSet.GetColumnValue("DocNum").ToInt32();
                    recordSet.MoveNext();
                }

                if (TypeDoc == 13)
                {
                    BaseSAPDocumentRepository<OINV, INV1> documentRepository = new UnitOfWork(Company).InvoiceRepository;
                    var document = documentRepository.RetrieveDocuments(t => t.DocumentEntry == DocEntry).FirstOrDefault();
                    document.NroSapCobro = DocNum;

                    var respuesta = documentRepository.RetriveCobroTransId(DocNum.ToString());

                    document.NroSapCobroTransId = respuesta.Item1 ? respuesta.Item2.ToInt32() : 0;
                    return document;

                }


                return null;
            }
            catch (Exception ex)
            {

                throw new Exception("RetrieveDocumentByPasarela: " + ex.Message);
            }
        }

        static string ObtenerUltimosCuatroDigitos(string texto)
        {
            if (texto.Length >= 4)
            {
                // Utilizar Substring para obtener los últimos 4 caracteres
                return texto.Substring(texto.Length - 4);
            }
            else
            {
                // El texto es demasiado corto para obtener 4 dígitos
                return "Texto demasiado corto";
            }
        }

        public override Tuple<bool, string, JournalEntrySL> GenerarAsiento(JournalEntrySL asiento)
        {

            JournalEntrySL receive = new JournalEntrySL();

            var list = CreateJournalEntrySLAsync(Login, asiento);


            while (list.Status == TaskStatus.WaitingForActivation)
            {

            }
            if (list.Status == TaskStatus.RanToCompletion)
            {
                return Tuple.Create(true, list.Result.JdtNum.ToString(), list.Result);
            }
            if (list.Status == TaskStatus.Faulted)
            {
                return Tuple.Create(false, list.Exception.InnerException.Message, receive);
            }

            return Tuple.Create(true, "", receive);
        }


        private async Task<JournalEntrySL> CreateJournalEntrySLAsync(SLConnection login, JournalEntrySL asiento)
        {


            var _journal = await login.Request("JournalEntries")
                .PostAsync<JournalEntrySL>(asiento);

            return _journal;
        }

        public override Tuple<bool, string> cancelarAsiento(string codAs)
        {
            var list = CancelournalEntrySLAsync(Login, codAs);


            while (list.Status == TaskStatus.WaitingForActivation)
            {

            }
            if (list.Status == TaskStatus.RanToCompletion)
            {
                return Tuple.Create(true, "");
            }
            if (list.Status == TaskStatus.Faulted)
            {
                return Tuple.Create(false, list.Exception.InnerException.Message);
            }

            return Tuple.Create(true, "");
        }

        private async Task CancelournalEntrySLAsync(SLConnection login, string asiento)
        {
            await login.Request("JournalEntries(" + asiento + ")/Cancel")
                .WithReturnNoContent()
                .PostAsync()
                ;


        }

        public override Tuple<bool, string> GenerarReconciliacion(ReconciliationSL reconciliation)
        {

            var list = GenerarReconciliacionSLAsync(Login, reconciliation);

            while (list.Status == TaskStatus.WaitingForActivation)
            {

            }
            if (list.Status == TaskStatus.RanToCompletion)
            {
                return Tuple.Create(true, "Correcto");
            }
            if (list.Status == TaskStatus.Faulted)
            {
                return Tuple.Create(false, list.Exception.InnerException.Message);
            }

            return Tuple.Create(true, "");


        }

        private async Task<ReconciliationSL> GenerarReconciliacionSLAsync(SLConnection login, ReconciliationSL reconciliation)
        {
            var _reconciliation = await login.Request("InternalReconciliations")
            .PostAsync<ReconciliationSL>(reconciliation);

            return _reconciliation;


        }

        public override Tuple<bool, string> agregarDetalleLiquidacion(string codLiquidacion, List<LiquidationLineSL> detail)
        {


            LiquidationSL liquidation = new LiquidationSL();
            liquidation.VsLtLtr1Collection = detail;
            var list = AddDetailLiquidationAsync(Login, liquidation, codLiquidacion);

            while (list.Status == TaskStatus.WaitingForActivation)
            {

            }
            if (list.Status == TaskStatus.RanToCompletion)
            {
                return Tuple.Create(true, "Correcto");
            }
            if (list.Status == TaskStatus.Faulted)
            {
                return Tuple.Create(false, list.Exception.InnerException.Message);
            }

            return Tuple.Create(true, "");
        }

        private async Task AddDetailLiquidationAsync(SLConnection login, LiquidationSL liquidation, string codLiquidacion)
        {
            await login.Request("VS_LT_OLTR(" + codLiquidacion + ")")
                .WithReplaceCollectionsOnPatch()
                .WithReturnNoContent()
                .PatchAsync(liquidation);

            //return _data;
        }


        public override Tuple<bool, string> ActualizaPagoSAP(List<int> listCod)
        {

            foreach (var item in listCod)
            {
                var list = ActualizaPagoSAPAsync(Login, item);
                while (list.Status == TaskStatus.WaitingForActivation)
                {

                }
            }
            return Tuple.Create(true, "");
        }

        private async Task ActualizaPagoSAPAsync(SLConnection login, int cod)
        {
            var estado = new { U_VS_LT_STAD = "Y" };

            await login.Request("IncomingPayments(" + cod + ")")
              .WithReturnNoContent()
              .PatchAsync(estado);

        }

        public override LiquidationSL RetrieveDetalleLiquidacion(string codigo)
        {
            //var list = AddDetailLiquidationAsync(Login, codigo);

            var list = Login.Request("VS_LT_OLTR(" + codigo + ")").GetAsync<LiquidationSL>().GetAwaiter().GetResult();
            return list;
            //while (list.Status == TaskStatus.WaitingForActivation)
            //{

            //}
            //if (list.Status == TaskStatus.RanToCompletion)
            //{
            //    return list.Result;
            //}
            //if (list.Status == TaskStatus.Faulted)
            //{
            //    return null;
            //}

            return null;
        }


        private async Task<LiquidationSL> AddDetailLiquidationAsync(SLConnection login, string codLiquidacion)
        {
            var data = await login.Request("VS_LT_OLTR(" + codLiquidacion + ")")
                .GetAsync<LiquidationSL>();

            return data;
        }

        public override List<LiquidationLineSL> RetrieveDetalleLiquidacion(List<Tuple<string, string>> lista)
        {
            //string whereStatement = QueryHelper.ParseToHANAQuery(expression);

            var list = RetrieveLiquidationAsync(Login, lista);

            while (list.Status == TaskStatus.WaitingForActivation)
            {

            }
            if (list.Status == TaskStatus.RanToCompletion)
            {
                List<LiquidationLineSL> details = new List<LiquidationLineSL>();
                foreach (var item in list.Result)
                {
                    details.AddRange(item.VsLtLtr1Collection);
                }
                return details;//list.Result;
            }
            if (list.Status == TaskStatus.Faulted)
            {
                return null;
            }

            return null;//documents_from_query_draft(string.Format(DOCUMENT_QUERY_DRAFT, $"where {whereStatement}"));
        }

        private async Task<List<LiquidationSL>> RetrieveLiquidationAsync(SLConnection login, List<Tuple<string, string>> lista)
        {
            var filter = "";
            bool isFirst = true;
            foreach (var item in lista)
            {
                if (isFirst)
                {
                    filter = item.Item1 + " eq '" + item.Item2 + "'";
                    isFirst = false;
                }
                else
                {
                    filter = filter + " and " + item.Item1 + " eq '" + item.Item2 + "'";
                }


            }

            var data = await login.Request("VS_LT_OLTR")
                //.WithHeader("Prefer", "odata.maxpagesize = 500")
                .Filter(filter)
                .GetAllAsync<LiquidationSL>();

            return data.ToList();
        }

        public override Tuple<bool, string> RetrieveSocioNegocioPasarela(string pasarela)
        {
            try
            {

                //var list= TiendasList(Login);

                var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
                var query = "Select * from \"@VS_LT_OPAP\" where \"Code\"='{0}' ";
                recordSet.DoQuery(string.Format(query, pasarela));

                while (!recordSet.EoF)
                {
                    var Code = recordSet.GetColumnValue("U_VS_LT_CDSN").ToString();
                    return Tuple.Create(true, Code);
                    recordSet.MoveNext();
                }

                return Tuple.Create(false, "");
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects();
            }
        }

        public override Tuple<bool, string> RetrieveCuentaxMotivo(string codAjuste)
        {
            try
            {

                //var list= TiendasList(Login);

                var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
                var query = "Select * from \"@VS_LT_OMAJ\" where \"Code\"='{0}' ";
                recordSet.DoQuery(string.Format(query, codAjuste));

                while (!recordSet.EoF)
                {
                    var Code = recordSet.GetColumnValue("U_VS_LT_CCAS").ToString();
                    return Tuple.Create(true, Code);
                    recordSet.MoveNext();
                }

                return Tuple.Create(false, "");
            }
            catch (Exception ex)
            {
                return Tuple.Create(false, ex.Message);
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects();
            }
        }

        public override Tuple<bool, LTR1> RetrieveDataPago(string value)
        {
            LTR1 line = new LTR1();
            try
            {


                var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
                var query = "select OV.\"CardCode\",OV.\"CardName\",R2.\"InvType\", OV.\"NumAtCard\",OV.\"DocDate\", " +
                    "OV.\"DocTotal\", RC.\"DocEntry\" as \"SapCobro\", RC.\"TransId\" from \"OINV\"  OV JOIN  \"RCT2\"" +
                    " R2 ON OV.\"DocEntry\"=R2.\"DocEntry\" JOIN ORCT RC ON R2.\"DocNum\"=RC.\"DocEntry\" where OV.\"DocEntry\"='{0}'  and RC.\"Canceled\"='N' and RC.\"U_VS_LT_STAD\"='N' ";
                recordSet.DoQuery(string.Format(query, value));

                while (!recordSet.EoF)
                {


                    line.CodigoCliente = recordSet.GetColumnValue("CardCode").ToString();
                    line.TipoDocumento = "01";
                    line.NombreCliente = recordSet.GetColumnValue("CardName").ToString();
                    line.NroTicket = recordSet.GetColumnValue("NumAtCard").ToString();
                    line.FechaDocumento = recordSet.GetColumnValue("DocDate").ToDateTime();
                    line.ImporteDocumento = recordSet.GetColumnValue("DocTotal").ToDouble();
                    line.Moneda = "SOL";
                    line.NroSAPCobro = recordSet.GetColumnValue("SapCobro").ToInt32();
                    line.CodigoDocumento = recordSet.GetColumnValue("TransId").ToString();

                    return Tuple.Create(true, line);
                    recordSet.MoveNext();
                }

                var recordSet2 = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
                var query2 = "select OV.\"CardCode\",OV.\"CardName\",R2.\"InvType\", OV.\"NumAtCard\",OV.\"DocDate\", " +
                    "OV.\"DocTotal\", RC.\"DocEntry\" as \"SapCobro\", RC.\"TransId\" from \"ORIN\"  OV JOIN  \"VPM2\"" +
                    " R2 ON OV.\"DocEntry\"=R2.\"DocEntry\" JOIN OVPM RC ON R2.\"DocNum\"=RC.\"DocEntry\" where OV.\"DocEntry\"='{0}' and RC.\"Canceled\"='N' ";
                recordSet.DoQuery(string.Format(query2, value));

                while (!recordSet.EoF)
                {


                    line.CodigoCliente = recordSet.GetColumnValue("CardCode").ToString();
                    line.TipoDocumento = "07";
                    line.NombreCliente = recordSet.GetColumnValue("CardName").ToString();
                    line.NroTicket = recordSet.GetColumnValue("NumAtCard").ToString();
                    line.FechaDocumento = recordSet.GetColumnValue("DocDate").ToDateTime();
                    line.ImporteDocumento = recordSet.GetColumnValue("DocTotal").ToDouble();
                    line.Moneda = "SOL";
                    line.NroSAPCobro = recordSet.GetColumnValue("SapCobro").ToInt32();
                    line.CodigoDocumento = recordSet.GetColumnValue("TransId").ToString();

                    return Tuple.Create(true, line);
                    recordSet.MoveNext();
                }

                return Tuple.Create(false, line);
            }
            catch (Exception ex)
            {
                return Tuple.Create(false, line);
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects();
            }
        }

        public override Tuple<bool, OHOJ> RetrieveHojaRuta(string value)
        {
            OHOJ hoja = new OHOJ();
            try
            {
                var r1 = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
                var val = "select* from \"@EX_HR_OHGR\"  where \"U_EXK_COD\" = '{0}'   and \"U_EXK_EST\" in ('O','T') ";
                r1.DoQuery(string.Format(val, value));
                while (!r1.EoF)
                {

                    return Tuple.Create(false, hoja);
                    r1.MoveNext();
                }





                var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
                var query = "select * from \"@EXK_HOJARUTA\" where \"Code\"='{0}'   ";
                recordSet.DoQuery(string.Format(query, value));

                while (!recordSet.EoF)
                {


                    hoja.Transportista = recordSet.GetColumnValue("U_EXK_TRANSP").ToString();
                    hoja.Chofer = recordSet.GetColumnValue("U_EXK_CHOFER").ToString();
                    hoja.Auxiliar1 = recordSet.GetColumnValue("U_EXK_AUX1")?.ToString();
                    hoja.Auxiliar2 = recordSet.GetColumnValue("U_EXK_AUX2")?.ToString();
                    hoja.Auxiliar3 = recordSet.GetColumnValue("U_EXK_AUX3")?.ToString();
                    hoja.InicioTraslado = recordSet.GetColumnValue("U_EXK_FEINTRAS").ToDateTime();
                    hoja.FinTraslado = recordSet.GetColumnValue("U_EXK_FEFITRAS").ToDateTime();
                    hoja.Placa = recordSet.GetColumnValue("U_EXK_PLACA")?.ToString();

                    recordSet.MoveNext();
                }
                var recordSetDet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
                var queryDet = "select * from \"@EXK_DHOJARUTA\" where \"Code\"='{0}'   ";
                recordSetDet.DoQuery(string.Format(queryDet, value));

                List<HOJ1> list = new List<HOJ1>();
                while (!recordSetDet.EoF)
                {
                    HOJ1 line = new HOJ1();

                    line.ZonaDespacho = recordSetDet.GetColumnValue("U_EXK_ZONDESP").ToString();

                    list.Add(line);
                    recordSetDet.MoveNext();
                }

                hoja.DetalleZonas = list;

                return Tuple.Create(true, hoja);
            }
            catch (Exception)
            {
                return Tuple.Create(false, hoja);
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects();
            }
        }

        public override Tuple<bool, List<ODLN>> RetrieveGuiasHoja(string desde, string hasta, string programado, string zona)
        {
            List<ODLN> listaGuias = new List<ODLN>();
            try
            {
                var queryprogram = "";
                if (!string.IsNullOrEmpty(programado))
                {
                    if (programado != "B")
                        queryprogram = " and \"U_EXK_HRPROG\"='" + programado + "'";

                }
                var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
                var query = "select (select \"Name\" from \"@EXK_ZONAVENTA\" where \"Code\"=D.\"U_EXK_AGENZONA\") as \"ZonaAgencia\", " +
                    " ZN.\"Name\", ST.\"Name\" as  \"DepAgencia\", CR.\"County\" as \"ProvAgencia\", CR.\"ZipCode\" as \"DisAgencia\", " +
                    " D.*  from \"ODLN\" D join  \"DLN12\" D12 on D12.\"DocEntry\"=D.\"DocEntry\" " +
                    " LEFT JOIN \"@EXK_ZONAVENTA\" ZN on ZN.\"Code\"=D12.\"U_EXX_TPED_ZONAS\" " +
                    " LEFT JOIN \"OSHP\" SH on SH.\"TrnspCode\"=D.\"TrnspCode\" OR D.\"TrnspCode\"=-1 " +
                    " LEFT JOIN \"CRD1\" CR on CR.\"Address\" = D.\"U_EXK_AGENIDDIREC\" and CR.\"CardCode\" = D.\"U_EXK_AGENCOD\" "+
                    " LEFT JOIN \"OCST\" ST on CR.\"State\" = ST.\"Code\" and ST.\"Country\" = CR.\"Country\" "+
                    " where \"FolioPref\" is not null and \"DocDate\">= TO_DATE('{0}', 'YYYYMMDD') and \"DocDate\"<= TO_DATE('{1}', 'YYYYMMDD') {2} " +
                    " AND SH.\"TrnspName\"<>'Recojo'";
                recordSet.DoQuery(string.Format(query, desde, hasta, queryprogram));

                var ex = string.Format(query, desde, hasta, queryprogram);
                while (!recordSet.EoF)
                {
                    ODLN Guias = new ODLN();

                    Guias.DocumentEntry = recordSet.GetColumnValue("DocEntry").ToString().ToInt32();
                    Guias.NumberAtCard = recordSet.GetColumnValue("FolioPref").ToString() + "-" + recordSet.GetColumnValue("FolioNum").ToString();
                    Guias.Peso = recordSet.GetColumnValue("U_EXX_FE_GRPESOTOTAL").ToString();
                    Guias.CantidadBultos = (recordSet.GetColumnValue("U_EXK_CANTBULTO") == null) ? 0 : recordSet.GetColumnValue("U_EXK_CANTBULTO").ToInt32();
                    Guias.Programado = recordSet.GetColumnValue("U_EXK_HRPROG").ToString();
                    Guias.CardName = recordSet.GetColumnValue("CardName").ToString();
                    var valAgencia = recordSet.GetColumnValue("U_EXK_AGENCOD") != null ? recordSet.GetColumnValue("U_EXK_AGENCOD").ToString() : "";

                    var dept = "";
                    var proc = "";
                    var dist = "";
                    if (string.IsNullOrEmpty(valAgencia))
                    {
                        Guias.Zona = recordSet.GetColumnValue("Name") != null ? recordSet.GetColumnValue("Name").ToString() : "";
                        Guias.DireccionDespacho = recordSet.GetColumnValue("Address2")?.ToString();
                        dept = recordSet.GetColumnValue("U_EXK_DPTO") != null ? recordSet.GetColumnValue("U_EXK_DPTO").ToString() : "";
                        proc = recordSet.GetColumnValue("U_EXK_PROVINCIA") != null ? recordSet.GetColumnValue("U_EXK_PROVINCIA").ToString() : "";
                        dist = recordSet.GetColumnValue("U_EXK_DISTRITO") != null ? recordSet.GetColumnValue("U_EXK_DISTRITO").ToString() : "";
                    }
                    else
                    {
                        Guias.Zona = recordSet.GetColumnValue("ZonaAgencia") != null ? recordSet.GetColumnValue("ZonaAgencia").ToString() : "";
                        Guias.DireccionDespacho = recordSet.GetColumnValue("U_EXK_AGENDIREC") != null ? recordSet.GetColumnValue("U_EXK_AGENDIREC").ToString() : "";
                         dept = recordSet.GetColumnValue("DepAgencia") != null ? recordSet.GetColumnValue("DepAgencia").ToString() : "";
                         proc = recordSet.GetColumnValue("ProvAgencia") != null ? recordSet.GetColumnValue("ProvAgencia").ToString() : "";
                         dist = recordSet.GetColumnValue("DisAgencia") != null ? recordSet.GetColumnValue("DisAgencia").ToString() : "";
                    }

                    Guias.DepProvZona = dept + "-" + proc + "-" + dist;



                    listaGuias.Add(Guias);
                    recordSet.MoveNext();
                }
                listaGuias = listaGuias.OrderBy(t => t.NumberAtCard).ToList();
                return Tuple.Create(true, listaGuias);
            }

            catch (Exception ex)
            {
                return Tuple.Create(false, listaGuias);
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects();
            }
        }

        public override string RetrieveCodigoGenerado()
        {
            try
            {

                //var list= TiendasList(Login);

                var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
                var query = " select LPAD(Count(*) + 1, 3, '0') as \"count\" from \"@EX_HR_OHGR\" ";
                recordSet.DoQuery(string.Format(query));

                while (!recordSet.EoF)
                {
                    var Code = DateTime.Now.Year + "-" + recordSet.GetColumnValue("count").ToString();
                    return Code;
                    recordSet.MoveNext();
                }

                return "";
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects();
            }
        }

        public override void ActualizarProgramado(string numeracion, string estado, ODLN datosGuia)
        {
            try
            {

                //var list= TiendasList(Login);
                var list = numeracion.Split("-");

                BaseSAPDocumentRepository<ODLN, DLN1> documentRepository = new UnitOfWork(Company).DeliveryRepository;
                string foliopref = list[0];
                int folionum = list[1].ToInt32();
                var document = documentRepository.RetrieveDocuments(t => t.FolioPref == foliopref && t.FolioNum == folionum).FirstOrDefault();

                document.Programado = estado;
                document.FechaInicioTraslado = datosGuia.FechaInicioTraslado;
                document.CodigoTransportista = datosGuia.CodigoTransportista;
                document.NombreTransportista = datosGuia.NombreTransportista;
                document.NombreConductor = datosGuia.NombreConductor;
                document.LicenciaConductor = datosGuia.LicenciaConductor;
                document.CantidadBultos = datosGuia.CantidadBultos;
                document.FechaGuia = document.DocumentDeliveryDate;
                document.TipoOperacion = datosGuia.TipoOperacion;
                document.MotivoTraslado = datosGuia.MotivoTraslado;
                document.FEXModalidadTraslado = datosGuia.FEXModalidadTraslado;
                document.HojaRuta = datosGuia.HojaRuta;
                document.EstadoEnvioSunat = datosGuia.EstadoEnvioSunat;
                var val = documentRepository.UpdateCustomFieldsFromDocument(document);
                //var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
                //var query = " update ODLN where \"FolioPref\"='{0}' and \"FolioNum\"={1}";
                //recordSet.DoQuery(string.Format(query,list[0],list[1]));
                if (!val.Item1)
                {
                    throw new Exception(val.Item2);
                }
                //using (HanaCommand updateCommand = new HanaCommand(query, connection))
                //{
                //    // Ejecutar la sentencia de actualización
                //    int rowsAffected = updateCommand.ExecuteNonQuery();
                //    Console.WriteLine($"Filas actualizadas: {rowsAffected}");
                //}


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects();
            }
        }

        public override void ActualizarEnvioSunat(string sunat)
        {
            try
            {

                //var list= TiendasList(Login);

                var list = sunat.Split("-");
                BaseSAPDocumentRepository<ODLN, DLN1> documentRepository = new UnitOfWork(Company).DeliveryRepository;
                string foliopref = list[0];
                int folionum = list[1].ToInt32();
                var document = documentRepository.RetrieveDocuments(t => t.FolioPref == foliopref && t.FolioNum == folionum).FirstOrDefault();

                document.EstadoEnvioSunat = "Y";
                //document.EstadoSUNAT = "AUT";
                documentRepository.UpdateCustomFieldsFromDocument(document);
                //var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
                //var query = " update ODLN where \"FolioPref\"='{0}' and \"FolioNum\"={1}";
                //recordSet.DoQuery(string.Format(query,list[0],list[1]));

                //using (HanaCommand updateCommand = new HanaCommand(query, connection))
                //{
                //    // Ejecutar la sentencia de actualización
                //    int rowsAffected = updateCommand.ExecuteNonQuery();
                //    Console.WriteLine($"Filas actualizadas: {rowsAffected}");
                //}


                //var list= TiendasList(Login);

                var recordSetIDCompany = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
                var queryIDCompany = "SELECT \"IdFexCompany\" FROM \"FEX_PE\".FEX_COMPANY WHERE \"dbName\"='{0}' AND IFNULL(\"BranchID\",-1) = {1}";
                recordSetIDCompany.DoQuery(string.Format(queryIDCompany, Company.CompanyDB, document.BranchId));

                var IdFexCompany = "";
                while (!recordSetIDCompany.EoF)
                {
                    IdFexCompany = recordSetIDCompany.GetColumnValue("IdFexCompany").ToString();
                    recordSetIDCompany.MoveNext();
                }


                var rvalidar = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
                var queryvalidar = "SELECT count(*) as \"existe\" FROM \"FEX_PE\".\"FEX_DOCUMENTOS\" where \"IdFexCompany\" = {0} and \"Docentry\" = {1} and \"ObjectType\" = {2}";
                rvalidar.DoQuery(string.Format(queryvalidar, IdFexCompany, document.DocumentEntry,15));

                int  existe = 0;
                while (!rvalidar.EoF)
                {
                    existe = rvalidar.GetColumnValue("existe").ToInt32 ();
                    rvalidar.MoveNext();
                }

                if (existe == 0)
                {
                    var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
                    var query = "Call \"FEX_PE\".\"FEX_ACCION_Guardar\"( " +
                        "  0," +//  --IN IdAccion bigint.  0 Para Insert.  Valor de IdAccion para actualizar registro existente
                        IdFexCompany + ", " +//    --IN IdFexCompany int.Valor de Company para documento actual
                          "'" + document.Indicator + "'," + //--IN CodigoEntidad varchar(50).
                         +document.DocumentEntry + "," +// --IN DocEntry int.
                        "'" + document.ObjectType + "'," +//   --IN ObjectType varchar(50).
                        "'" + document.DocSubType + "', " + //   --IN DocSubType varchar(50).
                     " '100',  " +//  --IN IdTipoAccion int.
                                    "  'VIG', " +//   --IN Estado varchar(50).
                     "'" + DateTime.Now.ToString() + "'" + //--IN FechaCreacion varchar(70)
                     "); ";
                    recordSet.DoQuery(string.Format(query));
                }

             

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects();
            }
        }

        public override void ActualizarEstadoHojaGuia(string estado, string codigo)
        {
            try
            {
                Company.StartTransaction();
                CompanyService companyService = Company.GetCompanyService();
                GeneralService generalService = companyService.GetGeneralService(OHRG.ID);
                var generalDataParams = (GeneralDataParams)generalService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                generalDataParams.SetProperty("Code", codigo);
                GeneralData generalData = generalService.GetByParams(generalDataParams);

                generalData.SetProperty("U_EXK_EST", estado);


                generalService.Update(generalData);
                Company.EndTransaction(BoWfTransOpt.wf_Commit);
                //return Tuple.Create(true, string.Empty);
            }
            catch (Exception exception)
            {
                if (Company.InTransaction)
                    Company.EndTransaction(BoWfTransOpt.wf_RollBack);

                //return Tuple.Create(false, exception.Message);
            }
        }

        public override Tuple<bool, string> GetCargaUtilByPlaca(string placa)
        {
            try
            {

                //var list= TiendasList(Login);

                var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
                var query = "Select * from \"@EXX_VEHICU\" where \"Code\"='{0}' ";
                recordSet.DoQuery(string.Format(query, placa));

                while (!recordSet.EoF)
                {
                    var Code = recordSet.GetColumnValue("U_EXK_CARGAUTIL").ToString();
                    return Tuple.Create(true, Code);
                    recordSet.MoveNext();
                }

                return Tuple.Create(false, "");
            }
            catch (Exception ex)
            {
                return Tuple.Create(false, ex.Message);
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects();
            }
        }
        static byte[] DecodePdfText(string pdfText)
        {
            // Convertir el texto PDF a bytes
            byte[] pdfBytes = System.Text.Encoding.UTF8.GetBytes(pdfText);

            // Devolver los bytes del PDF decodificado
            return pdfBytes;
        }
        public override Tuple<bool, byte[]> ObtenerPDF(string numeracion)
        {
            byte[] res = new byte[0];
            try
            {

                var list = numeracion.Split("-");
                BaseSAPDocumentRepository<ODLN, DLN1> documentRepository = new UnitOfWork(Company).DeliveryRepository;
                string foliopref = list[0];
                int folionum = list[1].ToInt32();
                var document = documentRepository.RetrieveDocuments(t => t.FolioPref == foliopref && t.FolioNum == folionum).FirstOrDefault();



                BaseOPDSRepository configRepository = new UnitOfWork(Company).SettingsRepository;
                var user=configRepository.Setting(OPDS.Codes.DBUSER);
                var pass = configRepository.Setting(OPDS.Codes.DBPASS);
                var server = configRepository.Setting(OPDS.Codes.SERVER);

                var recordSetIDCompany = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
                var queryIDCompany = "SELECT \"IdFexCompany\" FROM \"FEX_PE\".FEX_COMPANY WHERE \"dbName\"='{0}' AND IFNULL(\"BranchID\",-1) = {1}";
                recordSetIDCompany.DoQuery(string.Format(queryIDCompany, Company.CompanyDB, document.BranchId));

                var IdFexCompany = "";
                while (!recordSetIDCompany.EoF)
                {
                    IdFexCompany = recordSetIDCompany.GetColumnValue("IdFexCompany").ToString();
                    recordSetIDCompany.MoveNext();
                }



                var connectionString = "Server="+ server.Value + ";CS="+Company.CompanyDB+";UserID="+user.Value+";Password="+pass.Value;

                HanaConnectionStringBuilder connBuilder = new HanaConnectionStringBuilder();
                using (HanaConnection connection = new HanaConnection(connectionString))
                {
                    try
                    {
                        // Abrir la conexión
                        connection.Open();

                        // Crear un comando SQL para ejecutar la consulta
                        string query2 = "SELECT * FROM \"FEX_PE\".\"FEX_DOCUMENTOS\" WHERE \"IdFexCompany\" = "+IdFexCompany+" AND \"ObjectType\" = 15 AND \"Docentry\"="+document.DocumentEntry;

                        using (HanaCommand command = new HanaCommand(query2, connection))
                        {
                            // Ejecutar la consulta y obtener un lector de datos
                            using (HanaDataReader reader = command.ExecuteReader())
                            {
                                //// Crear un DataTable para almacenar los resultados
                                //DataTable dataTable = new DataTable();
                                //dataTable.Load(reader); // Cargar los datos del DataReader en el DataTable
                                if (reader.Read())
                                {
                                    byte[] pdfBytes = (byte[])reader["PDF"];
                                    return Tuple.Create(true, pdfBytes); ;
                                    //string tempFilePath = Path.GetTempFileName() + ".pdf";
                                    //File.WriteAllBytes(tempFilePath, pdfBytes);

                                    //// Abrir el archivo PDF en el navegador predeterminado
                                    //Process.Start(tempFilePath);
                                }

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }



                return Tuple.Create(false, res);
            }
            catch (Exception ex)
            {
                return Tuple.Create(false, res);
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects();
            }
        }

        public override Tuple<bool, string> ValidarSunat(string sunat)
        {
            try
            {
                var list = sunat.Split("-");
                BaseSAPDocumentRepository<ODLN, DLN1> documentRepository = new UnitOfWork(Company).DeliveryRepository;
                string foliopref = list[0];
                int folionum = list[1].ToInt32();
                var document = documentRepository.RetrieveDocuments(t => t.FolioPref == foliopref && t.FolioNum == folionum).FirstOrDefault();

                if (document.EstadoSUNAT == "AUT")
                {
                    return Tuple.Create(true, "validado");
                }
                else{
                    return Tuple.Create(false, "sin validar");
                }
            


            }
            catch (Exception ex)
            {
                return Tuple.Create(false, ex.Message);
            }
            finally
            {
                GenericHelper.ReleaseCOMObjects();
            }
        }
    }
}
