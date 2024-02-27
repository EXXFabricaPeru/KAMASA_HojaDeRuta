using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.DisposableBO;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.Report;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using Exxis.Addon.HojadeRutaAGuia.Domain.Code;
using Exxis.Addon.HojadeRutaAGuia.Domain.Contracts;
using Z.Core.Extensions;

namespace Exxis.Addon.HojadeRutaAGuia.Domain
{
    public class ReportDomain : BaseDomain, IReportDomain
    {
        Company _company;
        public ReportDomain(Company company) : base(company)
        {
            _company = company;
        }

        public IEnumerable<CertificadoCalidadModel> RetrieveCertificadoCalidad(int docentryRoute, bool isRoute= true)
        {
            var recordset = _company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            string QUERY;
            if (isRoute)
            {
                QUERY = $@"select 
		        OA.""DocEntry"" , --Num Ruta
                OA.""DocNum"" , --Num Ruta
                TR.""U_EXK_DSPR"" as ""CodTransportista"", --Código Transportista
                TR.""U_EXK_NMDC"" as ""RucTransportista"", --Código Transportista
                OA.""U_EXK_CDRF"" as ""RutaReferencia"" , --Num Tura Referencial
                OA.""U_EXK_FEDS"" as ""FechaDespacho"", --Fecha Despacho
                --OT.""DocNum"" as ""NumPedido"", --Num Pedido
                OT.""U_EXK_DSCL"" as ""RazonSocial"",--Razon Social
                OT.""U_EXK_DREN"" as ""Direccion"",--Direccion de Entrega
                IT.""ItemName"" as ""NombreArticulo"",--Descripcion
                IT.""InvntryUom"" as ""UnidadMedida"",--Unidad de Medida
                R3.""U_EXK_DCCA"" as ""CantPedida"",--Cantidad Pedida
                R3.""U_EXK_CADE"" as ""CantDespachada"",--Cantidad Despachada
                R3.""U_EXK_CDLT"" as ""Lote"",--Lote
                A1.""U_EXK_RFDD"" as ""NumPedido"",
                IFNULL(R3.""U_EXK_FCFA"", '') ""FechaFab"",--Fecha Produccion
                IFNULL(R3.""U_EXK_FCEX"", '') ""FechaExp""--Fecha Vencimiento
                from ""@VS_PD_RTR3"" R3
                join ""OITM"" IT on IT.""ItemCode"" = R3.""U_EXK_CDAR""
                join ""@VS_PD_ORTR"" OT on OT.""DocEntry"" = R3.""DocEntry""
                join ""@VS_PD_ARD2"" A1 on A1.""U_EXK_CDTR"" = R3.""DocEntry""  and R3.""U_EXK_DCEN""= A1.""U_EXK_DCEN""
                join ""@VS_PD_OARD"" OA on A1.""DocEntry"" = OA.""DocEntry""
                join ""@VS_PD_OTRD"" TR on TR.""Code"" = OA.""U_EXK_CDPR""
                where A1.""U_EXK_RFDC"" = '{docentryRoute}'; ";
            }
            else
            {
  //              QUERY = $@"
  //select 
		//        OT.""DocEntry"" , --Num Ruta
  //              OT.""DocNum"" , --Num Ruta
  //              '' as ""CodTransportista"", --Código Transportista
  //              '' as ""RucTransportista"", --Código Transportista
  //              'OT-'|| OT.""DocEntry"" as ""RutaReferencia"" , --Num Tura Referencial
  //              OT.""U_EXK_FEDS"" as ""FechaDespacho"", --Fecha Despacho
  //              R1.""U_EXK_DLVC"" as ""NumPedido"", --Num Pedido
  //              OT.""U_EXK_DSCL"" as ""RazonSocial"",--Razon Social
  //              OT.""U_EXK_DREN"" as ""Direccion"",--Direccion de Entrega
  //              IT.""ItemName"" as ""NombreArticulo"",--Descripcion
  //              IT.""InvntryUom"" as ""UnidadMedida"",--Unidad de Medida
  //              R3.""U_EXK_DCCA"" as ""CantPedida"",--Cantidad Pedida
  //              R3.""U_EXK_CADE"" as ""CantDespachada"",--Cantidad Despachada
  //              R3.""U_EXK_CDLT"" as ""Lote"",--Lote
  //              IFNULL(R3.""U_EXK_FCFA"", '') ""FechaFab"",--Fecha Produccion
  //              IFNULL(R3.""U_EXK_FCEX"", '') ""FechaExp""--Fecha Vencimiento
  //              from ""DLN1"" GR1
  //              join ""ODLN"" GR on GR.""DocEntry"" = GR1.""DocEntry""
  //              join ""OSLP"" V on V.""SlpCode"" = GR.""SlpCode"" 
  //              join ""@VS_PD_RTR1"" R1 on 
  //              (CASE WHEN R1.""U_EXK_DLVR""='' THEN 0 ELSE R1.""U_EXK_DLVR"" END )= GR.""DocEntry""          
  //              join ""@VS_PD_ORTR"" OT on OT.""DocEntry"" = R1.""DocEntry"" 
  //              join ""@VS_PD_RTR3"" R3 on GR1.""ItemCode""=R3.""U_EXK_CDAR"" and R1.""U_EXK_DCEN""=R3.""U_EXK_DCEN""
		//		join ""ORDR"" OC on OC.""DocEntry"" = R3.""U_EXK_DCEN""
  //              join ""OITM"" IT on IT.""ItemCode"" = GR1.""ItemCode""
  //                   where GR1.""DocEntry"" =  '{docentryRoute}'; ";

                QUERY = $@"
                select DISTINCT
                OT.""DocEntry"" , --Num Ruta
                OT.""DocNum"" , --Num Ruta
                '' as ""CodTransportista"", --Código Transportista
                '' as ""RucTransportista"", --Código Transportista
                'OT-' || OT.""DocEntry"" as ""RutaReferencia"" , --Num Tura Referencial
                OT.""U_EXK_FEDS"" as ""FechaDespacho"", --Fecha Despacho
                R1.""U_EXK_DLVC"" as ""NumPedido"", --Num Pedido
                OT.""U_EXK_DSCL"" as ""RazonSocial"",--Razon Social
                OT.""U_EXK_DREN"" as ""Direccion"",--Direccion de Entrega
                IT.""ItemName"" as ""NombreArticulo"",--Descripcion
                IT.""InvntryUom"" as ""UnidadMedida"",--Unidad de Medida
                GR1.""Quantity"" as ""CantPedida"",--Cantidad Pedida
                GR1.""Quantity"" as ""CantDespachada"",--Cantidad Despachada
                LT.""DistNumber"" as ""Lote"",--Lote
                LT.""MnfDate"" AS ""FechaFab"",
            	LT.""ExpDate"" AS  ""FechaExp""
                from ""DLN1"" GR1
                join ""ODLN"" GR on GR.""DocEntry"" = GR1.""DocEntry""
                join ""OSLP"" V on V.""SlpCode"" = GR.""SlpCode""
                join ""@VS_PD_RTR1"" R1 ON
                (CASE WHEN R1.""U_EXK_DLVR"" = '' THEN 0 ELSE R1.""U_EXK_DLVR"" END )= GR.""DocEntry""
                join ""@VS_PD_ORTR"" OT on OT.""DocEntry"" = R1.""DocEntry""
                join ""OITM"" IT on IT.""ItemCode"" = GR1.""ItemCode""
                INNER JOIN OITL B ON(GR.""DocNum"" = B.""DocNum"" AND B.""DocType"" = 15)
                INNER JOIN ITL1 C ON(B.""LogEntry"" = C.""LogEntry"" AND B.""DocType"" = 15)
                INNER JOIN OBTN LT ON(C.""SysNumber"" = LT.""SysNumber"" AND B.""DocType"" = 15 AND B.""ItemCode"" = LT.""ItemCode"")           
                     where GR1.""DocEntry"" = '{docentryRoute}'; ";

            }
                

            recordset.DoQuery(QUERY);
            List<CertificadoCalidadModel> _ListPS = new List<CertificadoCalidadModel>();
            CertificadoCalidadModel _ps = new CertificadoCalidadModel();
            while (!recordset.EoF)
            {
                _ps = new CertificadoCalidadModel();

                _ps.DocEntry = recordset.GetColumnValue("DocEntry").ToString();
                _ps.DocNum = recordset.GetColumnValue("DocNum").ToString();
                _ps.CodTransportista = recordset.GetColumnValue("CodTransportista").ToString();
                _ps.RucTransportista = recordset.GetColumnValue("RucTransportista").ToString();
                _ps.RutaReferencia = recordset.GetColumnValue("RutaReferencia").ToString();
                _ps.FechaDespacho = recordset.GetColumnValue("FechaDespacho").ToString().ToDateTime().ToShortDateString();
                _ps.NumPedido = recordset.GetColumnValue("NumPedido").ToString();
                _ps.RazonSocial = recordset.GetColumnValue("RazonSocial").ToString();
                _ps.Direccion = recordset.GetColumnValue("Direccion").ToString();
                _ps.NombreArticulo = recordset.GetColumnValue("NombreArticulo").ToString();
                _ps.UnidadMedida = recordset.GetColumnValue("UnidadMedida").ToString();
                _ps.CantPedida = recordset.GetColumnValue("CantPedida").ToString();
                _ps.CantDespachada = recordset.GetColumnValue("CantDespachada").ToString();
                _ps.Lote = recordset.GetColumnValue("Lote")?.ToString();
                _ps.FechaFab = recordset.GetColumnValue("FechaFab")?.ToString().ToDateTime().ToShortDateString();
                _ps.FechaExp = recordset.GetColumnValue("FechaExp")?.ToString().ToDateTime().ToShortDateString();

                _ListPS.Add(_ps);
                recordset.MoveNext();
            }


            return _ListPS;
        }

        public IEnumerable<ControlCargaModel> RetrieveControlCarga(int docentryRoute)
        {
            var recordset = _company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            var QUERY = $@"
select  distinct 
            OA.""DocEntry"", 
            OA.""DocNum"",
		    OH.""firstName"" || ' ' || OH.""lastName"" as ""Auxiliar"", 
		    OA.""U_EXK_CDRF"" as ""Apoyo"" , 
		    OA.""U_EXK_FEDS"" as ""Fecha"", 
		    '' as ""CantidadGuia"",
		    SUBSTRING(A2.""U_EXK_RFDD"",3,13) as ""NumGuia"",
            A2.""U_EXK_DCNM"" as ""NumPedido"",
		    OT.""U_EXK_DSCL"" as ""RazonSocial"",          
            OT.""U_EXK_DREN"" as ""DirEntrega"",
		    GR1.""Dscription"" as ""Descripcion"",
		    IT.""InvntryUom"" as ""Unidad"",
		    R3.""U_EXK_CADE"" as ""Cantidad"",
		    CASE WHEN OC.""U_CL_CERTIF"" ='Y' THEN 'SI' ELSE 'NO' END as ""CCalidad"",
		    R3.""U_EXK_CDLT"" as ""NLote"",
            CASE WHEN IFNULL(R3.""U_EXK_FCFA"", '')='' THEN LT.""MnfDate"" ELSE R3.""U_EXK_FCFA"" END ""FecProd"",
            CASE WHEN IFNULL(R3.""U_EXK_FCEX"", '') = '' THEN LT.""ExpDate"" ELSE R3.""U_EXK_FCEX"" END ""FecVenc"",
		    CASE  
		    WHEN OA.""U_EXK_FJOR"" = 'SA' THEN 'Ventas' 
            WHEN OA.""U_EXK_FJOR"" = 'SE' THEN 'Servicio'
            WHEN OA.""U_EXK_FJOR"" = 'TR' THEN 'Traslado'
            WHEN OA.""U_EXK_FJOR"" = 'RE' THEN 'Devolución'
            WHEN OA.""U_EXK_FJOR"" = 'PU' THEN 'Compras'
            END ""PV""
            from ""DLN1"" GR1
			join ""ODLN"" GR on GR.""DocEntry"" = GR1.""DocEntry""
            join ""@VS_PD_ARD2"" A2 on A2.""U_EXK_RFDC"" = GR1.""DocEntry""
            join ""OSLP"" V on V.""SlpCode"" = GR.""SlpCode""
			join ""@VS_PD_ORTR"" OT on OT.""DocEntry"" = A2.""U_EXK_CDTR""
            join ""@VS_PD_RTR3"" R3 on A2.""U_EXK_DCEN"" = R3.""U_EXK_DCEN"" and GR1.""ItemCode""=R3.""U_EXK_CDAR"" and R3.""DocEntry""=OT.""DocEntry""
            join ""OITM"" IT on IT.""ItemCode"" = GR1.""ItemCode""
            join ""OBTN"" LT on IT.""ItemCode"" = LT.""ItemCode"" and LT.""DistNumber""=R3.""U_EXK_CDLT""  
            join ""@VS_PD_OARD"" OA on OA.""DocEntry""=A2.""DocEntry""
            join ""OHEM"" OH on OH.""Code"" = OA.""U_EXK_CDAX"" 
            join ""OCRD"" OC on OC.""CardCode"" = OT.""U_EXK_COCL""   
            where OA.""DocEntry"" = '{docentryRoute}' 
            order by  ""NumGuia"" asc; ";

            recordset.DoQuery(QUERY);
            List<ControlCargaModel> _ListPS = new List<ControlCargaModel>();
            ControlCargaModel _ps = new ControlCargaModel();
            while (!recordset.EoF)
            {
                _ps = new ControlCargaModel();

                _ps.DocEntry = recordset.GetColumnValue("DocEntry").ToString();
                _ps.DocNum = recordset.GetColumnValue("DocNum").ToString();
                _ps.Auxiliar = recordset.GetColumnValue("Auxiliar").ToString();
                _ps.Apoyo = recordset.GetColumnValue("Apoyo").ToString();
                _ps.Fecha = recordset.GetColumnValue("Fecha").ToString().ToDateTime().ToShortDateString();
                _ps.CantidadGuia = recordset.GetColumnValue("CantidadGuia")?.ToString();
                _ps.NumGuia = recordset.GetColumnValue("NumGuia")?.ToString();
                _ps.NumPedido = recordset.GetColumnValue("NumPedido").ToString();
                _ps.RazonSocial = recordset.GetColumnValue("RazonSocial").ToString();
                _ps.DirEntrega = recordset.GetColumnValue("DirEntrega").ToString();
                _ps.Descripcion = recordset.GetColumnValue("Descripcion").ToString();
                _ps.Unidad = recordset.GetColumnValue("Unidad").ToString();
                _ps.Cantidad = recordset.GetColumnValue("Cantidad")?.ToString();
                _ps.CCalidad = recordset.GetColumnValue("CCalidad")?.ToString();
                _ps.NLote = recordset.GetColumnValue("NLote")?.ToString();
                _ps.FecProd = recordset.GetColumnValue("FecProd")?.ToString().ToDateTime().ToShortDateString();
                _ps.FecVenc = recordset.GetColumnValue("FecVenc")?.ToString().ToDateTime().ToShortDateString();
                _ps.PV = recordset.GetColumnValue("PV").ToString();

                _ListPS.Add(_ps);
                recordset.MoveNext();
            }

            var cantGuias = _ListPS.Select(t => t.NumGuia).Distinct().ToList().Count;

            _ListPS.ForEach((item) =>
            {
                item.CantidadGuia = cantGuias.ToString();
                item.DocNum = "RUTA " + int.Parse(item.Apoyo.Split("-")[1]).ToString();
            });

        
            return _ListPS;
        }

        public IEnumerable<PickingSheet> RetrievePickingExcel(int entryRoute, bool isRoute, bool isSheet)
        {
            string query;
            if (isRoute)
            {
                query = @"
                            select DISTINCT 
                            IFNULL(OA.""U_EXK_PRCG"",0) as ""RutaNum"", 
                            OA.""U_EXK_CDRF"" ,
                            OA.""U_EXK_FEDS"" , 
                            --T1.""U_EXK_DCNM"" as ""DocNum"", 
                            CASE WHEN R3.""U_EXK_DCEN"" is null 
                            then 
                            (Select ""DocNum"" from ORDR where ""DocEntry""=R2.""U_EXK_DCEN"" ) 
                            else 
                            (Select ""DocNum"" from ORDR where ""DocEntry""=R3.""U_EXK_DCEN"" )
                            END as ""DocNum"", 
                            OT.""U_EXK_DSCL"",
                            OT.""U_EXK_DREN"",
                            IT.""ItemName"",
                            IT.""InvntryUom"",
                            CASE WHEN R3.""U_EXK_DCCA"" IS NULL THEN R2.""U_EXK_DCCP"" ELSE R3.""U_EXK_DCCA"" END as ""U_EXK_DCCA"",
                            CASE WHEN R3.""U_EXK_CADE"" IS NULL THEN R2.""U_EXK_DCCA"" ELSE R3.""U_EXK_CADE"" END as ""U_EXK_CADE"" ,
                            R3.""U_EXK_CDLT"",
                            '' ""LOTE_FINAL"",
                            CASE WHEN IFNULL(R3.""U_EXK_FCFA"", '') = ''
                            THEN(SELECT  LT.""MnfDate"" from ""OBTN"" LT where IT.""ItemCode"" = LT.""ItemCode"" and LT.""DistNumber"" = R3.""U_EXK_CDLT"")
                            ELSE R3.""U_EXK_FCFA""
                            END ""FCFA"",
                            CASE WHEN IFNULL(R3.""U_EXK_FCEX"", '') = ''
                            THEN(SELECT  LT.""ExpDate"" from ""OBTN"" LT where IT.""ItemCode"" = LT.""ItemCode"" and LT.""DistNumber"" = R3.""U_EXK_CDLT"")
                            ELSE R3.""U_EXK_FCEX"" END ""FCEX"",
                            CASE WHEN CD.""U_CL_CERTIF""='N' Then 'NO' ELSE 'SI' END as ""Congelado"",
                                case
                            when ifnull(IT.""U_EXK_TPRF"", '') = '' then '--'
                            else
                            (select ""VV"".""Descr""
                            from   ""CUFD"" ""FD"",
                            ""UFD1"" ""VV""
                            where  ""FD"".""TableID"" = ""VV"".""TableID""
                            and    ""FD"".""FieldID"" = ""VV"".""FieldID""
                            and    ""FD"".""TableID"" = 'OITM'
                            and    ""FD"".""AliasID"" = 'VS_PD_TPRF'
                            and    ""VV"".""FldValue"" = IT.""U_EXK_TPRF"")
                                end ""TipoCarga"",
                            OT.""U_EXK_COCL""  as ""CodCliente"" ,
                            OV.""U_VS_OCCLIENTE""      as ""NumOC"", 
                            IT.""ItemCode""  as ""CodArticulo"" ,
                            CASE WHEN CD.""U_CL_FACTF""='N' Then 'NO' ELSE 'SI' END as ""FacturaFisica"" ,
                            CP.""PymntGroup""     as ""CondicionPago"" ,
                            IT.""SWeight1""  as ""PesoxUnidad"" ,
                            IT.""SWeight1""  * CASE WHEN R3.""U_EXK_CADE"" IS NULL THEN R2.""U_EXK_DCCA"" ELSE R3.""U_EXK_CADE"" END  as ""PesoxCantidad"" ,
                            CL.""Name""     as ""Canal"" ,
                            V.""SlpName""   as ""Empleado"" ,
                            OV.""DocDate""   as ""FechaContablizacionOV"" ,
                            OV.""TaxDate""   as ""FechadocumentoOV"" ,
                            OT.""DocNum""   as ""NumeroOrdenTraslado"" 
                            from ""@VS_PD_RTR2"" R2
                            left join ""@VS_PD_RTR3"" R3 on R2.""DocEntry"" = R3.""DocEntry"" and R2.""U_EXK_CDAR"" = R3.""U_EXK_CDAR"" and R3.""U_EXK_DCEN"" = R2.""U_EXK_DCEN""
                            join ""OITM"" IT on IT.""ItemCode"" = R2.""U_EXK_CDAR""
                            join ""@VS_PD_ORTR"" OT on OT.""DocEntry"" = R2.""DocEntry""
                            inner join ""@VS_PD_RTR1"" T1 on OT.""DocEntry"" = T1.""DocEntry""
                            join ""@VS_PD_ARD1"" A1 on A1.""U_EXK_CDTR"" = R2.""DocEntry""
                            join ""@VS_PD_OARD"" OA on A1.""DocEntry"" = OA.""DocEntry""
                            join ""OCRD"" CD on CD.""CardCode"" = OT.""U_EXK_COCL""
                            join ""@VS_LP_CANALVENTA"" CL on CL.""Code"" = OT.""U_EXK_CLVT""
                            join ""ORDR"" OV on OV.""DocEntry"" = R2.""U_EXK_DCEN""
                            join ""OSLP"" V on V.""SlpCode"" = OV.""SlpCode""
                            JOIN ""OCTG"" CP on CP.""GroupNum"" = OV.""GroupNum""
                            where A1.""DocEntry"" = '{0}' 
                            order by  OT.""U_EXK_DSCL"",OT.""U_EXK_DREN"" ,""DocNum""  asc                                                                                                 
";
            }
            else
            {
                query = @"select    ""OT"".""DocNum"" as ""RutaNum"",
                                    '' as ""U_EXK_CDRF"",
                                    ""OT"".""U_EXK_FEDS"",
                                    ""OT"".""DocNum"",
                                    ""OT"".""U_EXK_DSCL"",
                                    ""OT"".""U_EXK_DREN"",
                                    ""IT"".""ItemName"",
                                    ""IT"".""InvntryUom"",
                                    ""R3"".""U_EXK_DCCA"",
                                    ""R3"".""U_EXK_CADE"",
                                    ""R3"".""U_EXK_CDLT"",
                                    '' ""LOTE_FINAL"",
                                    ifnull(""R3.""U_EXK_FCFA"", '') ""FCFA"",
                                    ifnull(""R3.""U_EXK_FCEX"", '') ""FCEX"",
                                    case
                                        when ifnull(IT.""U_EXK_TPRF"", '') = '' then '--'
                                        else
                                            (select ""VV"".""Descr""
                                             from   ""CUFD"" ""FD"",
                                                    ""UFD1"" ""VV""
                                             where  ""FD"".""TableID"" = ""VV"".""TableID""
                                             and    ""FD"".""FieldID"" = ""VV"".""FieldID""
                                             and    ""FD"".""TableID"" = 'OITM'
                                             and    ""FD"".""AliasID"" = 'VS_PD_TPRF'
                                             and    ""VV"".""FldValue"" = IT.""U_EXK_TPRF"")
                                    end ""TipoCarga""
                                    case 
                                        when ifnull(""IT"".""U_EXK_TPRF"", '') = 'FL' then 'SI'
                                        else 'NO' 
                                    end ""Congelado""
                          from      ""@VS_PD_RTR2"" ""R2""
                          left join ""@VS_PD_RTR3"" ""R3"" on R2.""DocEntry"" = R3.""DocEntry"" and
                            ""R2"".""U_EXK_CDAR"" = ""R3"".""U_EXK_CDAR""
                          and       ""R3"".""U_EXK_DCEN"" = ""R2"".""U_EXK_DCEN""
                          join      ""OITM"" ""IT"" on ""IT"".""ItemCode"" = ""R2"".""U_EXK_CDAR""
                          join      ""@VS_PD_ORTR"" ""OT"" on ""OT"".""DocEntry"" = ""R2"".""DocEntry""
                          where     ""OT"".""DocEntry"" = '{0}'";
            }

            query = string.Format(query, entryRoute);
            using (SafeRecordSet safeRecordSet = Company.MakeSafeRecordSet())
            {
                return safeRecordSet.RetrieveListFromQuery(query, populate_picking_excel);
            }
        }

        private PickingSheet populate_picking_excel(SafeRecordSet record)
        {
            return new PickingSheet
            {
                RutaNum = record.GetString(@"RutaNum"),
                U_EXK_CDRF = record.GetString(@"U_EXK_CDRF"),
                U_EXK_FEDS = record.GetDateTime(@"U_EXK_FEDS")?.ToString("dd/MM/yyyy"),
                DocNum = record.GetString(@"DocNum"),
                U_EXK_DSCL = record.GetString(@"U_EXK_DSCL"),
                U_EXK_DREN = record.GetString(@"U_EXK_DREN"),
                ItemName = record.GetString(@"ItemName"),
                InvntryUom = record.GetString(@"InvntryUom"),
                U_EXK_DCCA = record.GetString(@"U_EXK_DCCA"),
                U_EXK_CADE = record.GetString(@"U_EXK_CADE"),
                U_EXK_CDLT = record.GetString(@"U_EXK_CDLT"),
                LOTE_FINAL = record.GetString(@"LOTE_FINAL"),
                FCFA = record.GetDateTime(@"FCFA")?.ToString("dd/MM/yyyy"),
                FCEX = record.GetDateTime(@"FCEX")?.ToString("dd/MM/yyyy"),
                Congelado = record.GetString(@"Congelado"),
                ItemType = record.GetString(@"TipoCarga"),
                CodCliente = record.GetString(@"CodCliente"),
                NumOC = record.GetString(@"NumOC"),
                CodArticulo = record.GetString(@"CodArticulo"),
                FacturaFisica = record.GetString(@"FacturaFisica"),
                CondicionPago = record.GetString(@"CondicionPago"),
                PesoxUnidad = record.GetString(@"PesoxUnidad"),
                PesoxCantidad = record.GetString(@"PesoxCantidad"),
                Canal = record.GetString(@"Canal"),
                Empleado = record.GetString(@"Empleado"),
                FechaContablizacionOV = record.GetString(@"FechaContablizacionOV"),
                FechadocumentoOV = record.GetString(@"FechadocumentoOV"),
                NumeroOrdenTraslado = record.GetString(@"NumeroOrdenTraslado"),

            };
        }

        //TODO : Observación 3 
        public IEnumerable<PickingSheet> RetrievePicking(int docentryRoute, bool isRoute, bool isSheet)
        {
            var recordset = _company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            string QUERY = "";
            if (isRoute)
            {

                QUERY = $@"select 
		            IFNULL(OA.""U_EXK_PRCG"",0) as ""RutaNum"", 
                    OA.""U_EXK_CDRF"" ,
                    OA.""U_EXK_FEDS"" , 
                    --T1.""U_EXK_DCNM"" as ""DocNum"", 
                    CASE WHEN R3.""U_EXK_DCEN"" is null 
                    then
                    (Select ""DocNum"" from ORDR where ""DocEntry"" = R2.""U_EXK_DCEN"")
                    else 
                    (Select ""DocNum"" from ORDR where ""DocEntry"" = R3.""U_EXK_DCEN"" )
                    END as ""DocNum"", 
                    OT.""U_EXK_DSCL"",
                    OT.""U_EXK_DREN"",
                    IT.""ItemName"",
                    IT.""InvntryUom"",
                    CASE WHEN R3.""U_EXK_DCCA"" IS NULL THEN R2.""U_EXK_DCCP"" ELSE R3.""U_EXK_DCCA"" END as ""U_EXK_DCCA"",
                    CASE WHEN R3.""U_EXK_CADE"" IS NULL THEN R2.""U_EXK_DCCA"" ELSE R3.""U_EXK_CADE"" END as ""U_EXK_CADE"" ,
                    R3.""U_EXK_CDLT"",
                    '' ""LOTE_FINAL"",
                    CASE WHEN IFNULL(R3.""U_EXK_FCFA"", '') = ''
                    THEN(SELECT  LT.""MnfDate"" from ""OBTN"" LT where IT.""ItemCode"" = LT.""ItemCode"" and LT.""DistNumber"" = R3.""U_EXK_CDLT"")
                    ELSE R3.""U_EXK_FCFA""
                    END ""FCFA"",
            	    CASE WHEN IFNULL(R3.""U_EXK_FCEX"", '') = ''
                    THEN(SELECT  LT.""ExpDate"" from ""OBTN"" LT where IT.""ItemCode"" = LT.""ItemCode"" and LT.""DistNumber"" = R3.""U_EXK_CDLT"")
                    ELSE R3.""U_EXK_FCEX"" END ""FCEX"",
                    CASE WHEN CD.""U_CL_CERTIF""='N' Then 'NO' ELSE 'SI' END as ""Congelado""
                    from ""@VS_PD_RTR2"" R2
                    left join ""@VS_PD_RTR3"" R3 on R2.""U_EXK_CDAR"" = R3.""U_EXK_CDAR"" and R3.""U_EXK_DCEN"" = R2.""U_EXK_DCEN""
                    join ""OITM"" IT on IT.""ItemCode"" = R2.""U_EXK_CDAR""
                    join ""@VS_PD_ORTR"" OT on OT.""DocEntry"" = R2.""DocEntry""
                    join ""@VS_PD_RTR1"" T1 on OT.""DocEntry"" = T1.""DocEntry""
                    join ""@VS_PD_ARD1"" A1 on A1.""U_EXK_CDTR"" = R2.""DocEntry""
                    join ""@VS_PD_OARD"" OA on A1.""DocEntry"" = OA.""DocEntry""
                    join ""OCRD"" CD on CD.""CardCode"" = OT.""U_EXK_COCL""
                    where A1.""DocEntry"" = '{docentryRoute}' 
                    order by OT.""U_EXK_DSCL"",OT.""U_EXK_DREN"" ,""DocNum"" asc
                    ; "
                                ;


            }
            else
            {
                QUERY = $@"select 
		        OT.""DocNum"" as ""RutaNum"", 
                '' as ""U_EXK_CDRF"" ,
                OT.""U_EXK_FEDS"" , 
                OT.""DocNum"", 
                OT.""U_EXK_DSCL"",
                OT.""U_EXK_DREN"",
                IT.""ItemName"",
                IT.""InvntryUom"",
                 CASE WHEN R3.""U_EXK_DCCA"" IS NULL THEN R2.""U_EXK_DCCP"" ELSE R3.""U_EXK_DCCA"" END as ""U_EXK_DCCA"",
                    CASE WHEN R3.""U_EXK_CADE"" IS NULL THEN R2.""U_EXK_DCCA"" ELSE R3.""U_EXK_CADE"" END as ""U_EXK_CADE"" ,
                R3.""U_EXK_CDLT"",
                '' ""LOTE_FINAL"",
                CASE WHEN IFNULL(R3.""U_EXK_FCFA"", '') = ''
                    THEN(SELECT  LT.""MnfDate"" from ""OBTN"" LT where IT.""ItemCode"" = LT.""ItemCode"" and LT.""DistNumber"" = R3.""U_EXK_CDLT"")
                    ELSE R3.""U_EXK_FCFA""
                    END ""FCFA"",
            	    CASE WHEN IFNULL(R3.""U_EXK_FCEX"", '') = ''
                    THEN(SELECT  LT.""ExpDate"" from ""OBTN"" LT where IT.""ItemCode"" = LT.""ItemCode"" and LT.""DistNumber"" = R3.""U_EXK_CDLT"")
                    ELSE R3.""U_EXK_FCEX"" END ""FCEX"",
                CASE WHEN IFNULL(IT.""U_EXK_TPRF"",'')='FL' THEN 'SI' ELSE 'NO' END ""Congelado""
                from ""@VS_PD_RTR2"" R2
                left join ""@VS_PD_RTR3"" R3 on R2.""U_EXK_CDAR"" = R3.""U_EXK_CDAR"" and R3.""U_EXK_DCEN"" = R2.""U_EXK_DCEN""
                join ""OITM"" IT on IT.""ItemCode"" = R2.""U_EXK_CDAR""
                join ""@VS_PD_ORTR"" OT on OT.""DocEntry"" = R2.""DocEntry"" 
                where OT.""DocEntry"" = '{docentryRoute}'; ";
            }


            recordset.DoQuery(QUERY);
            List<PickingSheet> _ListPS = new List<PickingSheet>();
            PickingSheet _ps = new PickingSheet();
            while (!recordset.EoF)
            {
                _ps = new PickingSheet();

                _ps.RutaNum = recordset.GetColumnValue("RutaNum").ToString();
                _ps.U_EXK_CDRF = recordset.GetColumnValue("U_EXK_CDRF").ToString();
                _ps.U_EXK_FEDS = recordset.GetColumnValue("U_EXK_FEDS").ToString().ToDateTime().ToShortDateString();
                _ps.DocNum = recordset.GetColumnValue("DocNum").ToString();
                _ps.U_EXK_DSCL = recordset.GetColumnValue("U_EXK_DSCL").ToString();
                _ps.U_EXK_DREN = recordset.GetColumnValue("U_EXK_DREN").ToString();
                _ps.ItemName = recordset.GetColumnValue("ItemName").ToString();
                _ps.InvntryUom = recordset.GetColumnValue("InvntryUom").ToString();
                _ps.U_EXK_DCCA = recordset.GetColumnValue("U_EXK_DCCA")?.ToString();
                _ps.U_EXK_CADE = recordset.GetColumnValue("U_EXK_CADE")?.ToString();
                _ps.U_EXK_CDLT = recordset.GetColumnValue("U_EXK_CDLT")?.ToString();
                _ps.LOTE_FINAL = recordset.GetColumnValue("LOTE_FINAL").ToString();
                _ps.FCFA = recordset.GetColumnValue("FCFA")?.ToString().ToDateTime().ToShortDateString();
                _ps.FCEX = recordset.GetColumnValue("FCEX")?.ToString().ToDateTime().ToShortDateString();
                _ps.Congelado = recordset.GetColumnValue("Congelado").ToString();

                _ListPS.Add(_ps);
                recordset.MoveNext();
            }


            return _ListPS;
        }

        public IEnumerable<LiquidationRouteModel> RetrieveLiquidationRoute(int docentryRoute)
        {
            var recordset = _company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            var QUERY = $@"select 
            OA.""DocEntry"", 
            OA.""DocNum"",
		    OH.""firstName"" || ' ' || OH.""lastName"" as ""Auxiliar"", 		    
		    OA.""U_EXK_FEDS"" as ""Fecha"", 
		    OA.""U_EXK_PLVH"" as ""Placa"", 
		    FLOOR(OA.""U_EXK_HRIN"" / 100) || ':' || LPAD(MOD(OA.""U_EXK_HRIN"" , 100),2,'0')as ""HoraInicio"", 
            FLOOR(OA.""U_EXK_HRFN"" / 100) || ':' || LPAD(MOD(OA.""U_EXK_HRFN"", 100), 2, '0') as ""HoraFinal"", 
		    '' as ""CantidadGuia"",
		    ROW_NUMBER() OVER (ORDER BY A2.""U_EXK_RFDD"") as ""Contador"", 
		    SUBSTRING(A2.""U_EXK_RFDD"",3,13)  as ""NumGuia"",
            IFNULL(SUBSTRING(A2.""U_EXK_RFMD"", 3, 13), '-') as ""NumFactura"",
		    OT.""U_EXK_DSCL"" as ""RazonSocial"",          
            OT.""U_EXK_DREN"" as ""DirEntrega"",
            FLOOR(V1.""U_EXK_HRIN"" / 100) || ':' || LPAD(MOD(V1.""U_EXK_HRIN"", 100),2,'0')  as ""VentanaMin"",
            FLOOR(V1.""U_EXK_HRFN"" / 100) || ':' || LPAD(MOD(V1.""U_EXK_HRFN"", 100), 2, '0') as ""VentanaMax""
            from  ""@VS_PD_ORTR"" OT
            join ""@VS_PD_ARD1"" A1 on A1.""U_EXK_CDTR"" = OT.""DocEntry""
            INNER join ""@VS_PD_ARD2"" A2 on A2.""U_EXK_CDTR"" = A1.""U_EXK_CDTR""
            join ""@VS_PD_OARD"" OA on A1.""DocEntry"" = OA.""DocEntry""
            INNER JOIN ""@VS_PD_VHR1"" V1 ON V1.""Code""=OT.""U_EXK_VTHR""
            join ""OHEM"" OH on OH.""Code"" = OA.""U_EXK_CDAX""
            where OA.""DocEntry"" = '{docentryRoute}'; ";

            recordset.DoQuery(QUERY);
            List<LiquidationRouteModel> _ListPS = new List<LiquidationRouteModel>();
            LiquidationRouteModel _ps = new LiquidationRouteModel();
            while (!recordset.EoF)
            {
                _ps = new LiquidationRouteModel();

                _ps.DocEntry = recordset.GetColumnValue("DocEntry").ToString();
                _ps.DocNum = recordset.GetColumnValue("DocNum").ToString();
                _ps.Auxiliar = recordset.GetColumnValue("Auxiliar").ToString();
                _ps.Fecha = recordset.GetColumnValue("Fecha").ToString().ToDateTime().ToShortDateString();
                _ps.Placa = recordset.GetColumnValue("Placa")?.ToString();
                _ps.HoraInicio = recordset.GetColumnValue("HoraInicio")?.ToString();
                _ps.HoraFinal = recordset.GetColumnValue("HoraFinal")?.ToString();
                _ps.CantidadGuia = recordset.GetColumnValue("CantidadGuia")?.ToString();
                _ps.Contador = recordset.GetColumnValue("Contador")?.ToString();
                _ps.NumGuia = recordset.GetColumnValue("NumGuia")?.ToString();
                _ps.NumFactura = recordset.GetColumnValue("NumFactura")?.ToString();
                _ps.RazonSocial = recordset.GetColumnValue("RazonSocial").ToString();
                _ps.DirEntrega = recordset.GetColumnValue("DirEntrega").ToString();
                _ps.VentanaMin = recordset.GetColumnValue("VentanaMin")?.ToString();
                _ps.VentanaMax = recordset.GetColumnValue("VentanaMax")?.ToString();
                _ListPS.Add(_ps);
                recordset.MoveNext();
            }

            _ListPS.ForEach(item =>
            {
                item.CantidadGuia = _ListPS.Count.ToString();
            });
            return _ListPS;
        }

        public IEnumerable<GuiaRemisionModel> RetrieveGuiaRoute(int docentryRoute, int codOT, bool isRoute =true)
        {
            var recordset = _company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();

            var QUERY = "";

            if (isRoute)
            {
                QUERY = $@"
                    select DISTINCT
                    GR.""DocEntry"", 
                    GR.""DocNum"", 
                    GR.""NumAtCard"" as ""NumGuia"" , 
                    GR.""CardName"" as ""NomCliente"" , 
                    GR.""Address"" as ""DirCliente"" , 
                    GR.""LicTradNum"" as ""RucCliente"" , 
                    GR.""DocDate"" as ""FechaEmision"" , 
                    GR.""DocDueDate"" as ""FechaDespacho"" , 
                    A2.""U_EXK_DCNM"" as ""NumPedido"" ,
                    V.""SlpName"" as ""Vendedor"" ,
                    IFNULL(OC.""U_VS_OCCLIENTE"",'') as ""TipoNumDoc"" ,
                    OT.""U_EXK_DRSA"" as ""PtPartida"" ,
                    GR.""U_VS_DIRLLEG"" as ""PtLlegada"",
                    ROW_NUMBER() OVER(order by R3.""LineId"" asc) as ""NumItem"",
                    GR1.""ItemCode"" as ""ItemCode"",
                    GR1.""Dscription"" as ""ItemName"",
                    IT.""InvntryUom"" as ""UnidadMedida"",
                    R3.""U_EXK_CADE"" as ""Cantidad"",
                    R3.""U_EXK_CDLT"" as ""Lote"",
                    R3.""U_EXK_CADE"" * IT.""SWeight1""  as ""Peso""
                    from ""DLN1"" GR1
                    join ""ODLN"" GR on GR.""DocEntry"" = GR1.""DocEntry""
                    join ""@VS_PD_ARD2"" A2 on A2.""U_EXK_RFDC"" = GR1.""DocEntry""
                    join ""OSLP"" V on V.""SlpCode"" = GR.""SlpCode""
                    join ""ORDR"" OC on OC.""DocEntry"" = A2.""U_EXK_DCEN""
                    join ""@VS_PD_ORTR"" OT on OT.""DocEntry"" = A2.""U_EXK_CDTR""
                    join ""@VS_PD_RTR3"" R3 on A2.""U_EXK_DCEN"" = R3.""U_EXK_DCEN"" and GR1.""ItemCode""=R3.""U_EXK_CDAR"" and R3.""DocEntry""=OT.""DocEntry""
                     join ""OITM"" IT on IT.""ItemCode"" = GR1.""ItemCode""
                    where GR1.""DocEntry"" =  '{docentryRoute}' 
                     order by ""NumItem""
                ; ";

            }
            else
            {
                QUERY = $@" 
          select DISTINCT
                    GR.""DocEntry"", 
                    GR.""DocNum"", 
                    GR.""NumAtCard"" as ""NumGuia"" , 
                    GR.""CardName"" as ""NomCliente"" , 
                    GR.""Address"" as ""DirCliente"" , 
                    GR.""LicTradNum"" as ""RucCliente"" , 
                    GR.""DocDate"" as ""FechaEmision"" , 
                    GR.""DocDueDate"" as ""FechaDespacho"" , 
                    R3.""U_EXK_DCEN"" as ""NumPedido"" ,
                    V.""SlpName"" as ""Vendedor"" ,
                    IFNULL(OC.""U_VS_OCCLIENTE"",'') as ""TipoNumDoc"" ,
                    OT.""U_EXK_DRSA"" as ""PtPartida"" ,
                    GR.""U_VS_DIRLLEG"" as ""PtLlegada"",
                    ROW_NUMBER() OVER(order by R3.""LineId"" asc) as ""NumItem"",
                    GR1.""ItemCode"" as ""ItemCode"",
                    GR1.""Dscription"" as ""ItemName"",
                    IT.""InvntryUom"" as ""UnidadMedida"",
                    R3.""U_EXK_CADE"" as ""Cantidad"",
                    R3.""U_EXK_CDLT"" as ""Lote"",
                    R3.""U_EXK_CADE"" * IT.""SWeight1""  as ""Peso""
                    from ""DLN1"" GR1
                    join ""ODLN"" GR on GR.""DocEntry"" = GR1.""DocEntry""
                    join ""OSLP"" V on V.""SlpCode"" = GR.""SlpCode"" 
                    join ""@VS_PD_RTR1"" R1 on 
                    (CASE WHEN R1.""U_EXK_DLVR""='' THEN 0 ELSE R1.""U_EXK_DLVR"" END )= GR.""DocEntry""          
                    join ""@VS_PD_ORTR"" OT on OT.""DocEntry"" = R1.""DocEntry"" 
                    join ""@VS_PD_RTR3"" R3 on GR1.""ItemCode""=R3.""U_EXK_CDAR"" and R1.""U_EXK_DCEN""=R3.""U_EXK_DCEN"" and R3.""DocEntry""=OT.""DocEntry""
                     join ""ORDR"" OC on OC.""DocEntry"" = R3.""U_EXK_DCEN""
                     join ""OITM"" IT on IT.""ItemCode"" = GR1.""ItemCode""
                     where GR1.""DocEntry"" =  '{docentryRoute}'  order by ""NumItem"" ;";
                
            }


            recordset.DoQuery(QUERY);
            List<GuiaRemisionModel> _ListPS = new List<GuiaRemisionModel>();
            GuiaRemisionModel _ps = new GuiaRemisionModel();
            while (!recordset.EoF)
            {
                _ps = new GuiaRemisionModel();

                _ps.NumGuia = recordset.GetColumnValue("NumGuia").ToString();
                _ps.NomCliente = recordset.GetColumnValue("NomCliente").ToString();
                _ps.DirCliente = recordset.GetColumnValue("DirCliente").ToString();
                _ps.RucCliente = recordset.GetColumnValue("RucCliente").ToString();
                _ps.FechaEmision = recordset.GetColumnValue("FechaEmision").ToString().ToDateTime().ToShortDateString();
                _ps.FechaDespacho = recordset.GetColumnValue("FechaDespacho").ToString().ToDateTime().ToShortDateString();
                _ps.NumPedido = recordset.GetColumnValue("NumPedido").ToString();
                _ps.Vendedor = recordset.GetColumnValue("Vendedor").ToString();
                _ps.TipoNumDoc = recordset.GetColumnValue("TipoNumDoc").ToString();
                _ps.PtPartida = recordset.GetColumnValue("PtPartida").ToString();
                _ps.PtLlegada = recordset.GetColumnValue("PtLlegada").ToString();
                _ps.NumItem = recordset.GetColumnValue("NumItem").ToString();
                _ps.ItemCode = recordset.GetColumnValue("ItemCode")?.ToString();
                _ps.ItemName = recordset.GetColumnValue("ItemName")?.ToString();
                _ps.Cantidad = recordset.GetColumnValue("Cantidad")?.ToString();
                _ps.UnidadMedida = recordset.GetColumnValue("UnidadMedida")?.ToString();
                _ps.Lote = recordset.GetColumnValue("Lote").ToString();
                _ps.Peso = recordset.GetColumnValue("Peso").ToString();
                _ps.TipoNumDoc = recordset.GetColumnValue("TipoNumDoc").ToString();

                _ListPS.Add(_ps);
                recordset.MoveNext();
            }

            var pesoTotal = _ListPS.Sum(t => decimal.Parse(t.Peso));

            _ListPS.ForEach(item =>
            {
                item.Peso = pesoTotal.ToString();
            });


            return _ListPS;
        }
    }
}
