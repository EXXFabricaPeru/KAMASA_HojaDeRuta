using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Constant;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using VersionDLL.FlagElements.Attributes;
using VersionDLL.FlagElements.FieldsEnums;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Models
{
    [Serializable]
    [SystemTable("ORDR", FormType = "dummy")]
    public abstract class SAPDocument : BaseSAPTable, IComparable
    {
        [SAPColumn("DocEntry")]
        public int DocumentEntry { get; set; }

        [SAPColumn("DocNum")]
        public int DocumentNumber { get; set; }

        [SAPColumn("CardCode")]
        public string CardCode { get; set; }

        [SAPColumn("CardName")]
        public string CardName { get; set; }

        [SAPColumn("ShipToCode")]
        public string ShippingAddressCode { get; set; }

        [SAPColumn("Address2")]
        public string ShippingAddressDescription { get; set; }

        [SAPColumn("NumAtCard")]
        public string NumberAtCard { get; set; }

        [SAPColumn("FolioPref")]
        public string FolioPref { get; set; }

        [SAPColumn("FolioNum")]
        public int FolioNum { get; set; }

        [SAPColumn("DocDate")]
        public DateTime DocumentDate { get; set; }

        [SAPColumn("TaxDate")]
        public DateTime TaxDate { get; set; }

        [SAPColumn("DocDueDate")]
        public DateTime DocumentDeliveryDate { get; set; }

        [SAPColumn("DocStatus")]
        public string DocumentStatus { get; set; }

        [SAPColumn("CANCELED")]
        public string DocumentCancelled { get; set; }

        [SAPColumn("ObjType")]
        public int ObjectType { get; set; }

        [SAPColumn("DocSubType")]
        public string DocSubType { get; set; }

        [SAPColumn("DocTotal")]
        public decimal TotalPrice { get; set; }
        [SAPColumn("VatSum")]
        public decimal Impuesto { get; set; }

        [SAPColumn("Volume")]
        public decimal TotalVolume { get; set; }

        [SAPColumn("Weight")]
        public decimal TotalWeight { get; set; }

        [SAPColumn("JrnlMemo")]
        public string JrnlMemo { get; set; }

        [SAPColumn("Comments")]
        public string Comments { get; set; }

        [SAPColumn("GroupNum")]
        public int CondicionPago { get; set; }


        [SAPColumn("Indicator")]
        public string Indicator { get; set; }


        [SAPColumn("BPLId")]
        public int BranchId { get; set; }



        [SAPColumn(@"U_EXX_FE_GRPESOTOTAL", false)]
        public string Peso { get; set; }

        [SAPColumn(@"U_EXK_CANTBULTO", false)]
        public double CantidadBultos { get; set; }

        [SAPColumn("U_EXK_HRPROG", false)]
        //[FieldNoRelated("U_EXK_HRPROG", "Programado Guía", BoDbTypes.Alpha, Size = 12)]
        public string Programado { get; set; }

        [SAPColumn("U_EXX_FE_Estado", false)]
        //[FieldNoRelated("U_EXK_HRPROG", "Programado Guía", BoDbTypes.Alpha, Size = 12)]
        public string EstadoSUNAT { get; set; }

        [SAPColumn("U_EXX_HOAS_STAD", false)]
        [FieldNoRelated("U_EXX_HOAS_STAD", "Estado Envío Sunat", BoDbTypes.Alpha, Size = 2,Default ="A")]
        [Val("A", "Sin Asignar")]
        [Val("N", "NO")]
        [Val("Y", "SI")]
        public string EstadoEnvioSunat { get; set; }
        public bool isFR { get; set; }

        public int CompareTo(object obj)
        {
            var sapDocument = obj.To<SAPDocument>();
            return DocumentEntry - sapDocument.DocumentEntry;
        }

        public static IComparer<string> SortByDistributionPriority
            => new SortDistributionPriority();


        public static class DistributionValidationStatus
        {
            public const string APPROVED = "AP";
            public const string DISAPPROVED = "DI";
            public const string MANUAL_APPROVED = "MA";
            public const string MANUAL_DISAPPROVED = "MD";
            public const string NON_VALIDATE = "NV";
        }

        public static class Status
        {
            public const string OPEN = "OP";
            public const string CANCELLED = "CL";
            public const string CLOSED = "CA";
        }

        public static class DistributionPriorities
        {
            public const string HIGH = "AL";
            public const string NORMAL = "MD";
            public const string LOW = "BJ";
        }



        [SAPColumn("U_EXX_MOTIVTRA", false)]
        public string MotivoTraslado { get; set; }

        //[SAPColumn("U_VS_BULPAL", false)]
        public int NumeroBultosPallets { get; set; }


        //[SAPColumn("U_VS_GLNPART", false)]
        public string UbigeoPuntoPartida { get; set; }
        //[SAPColumn("U_VS_DIRPART", false)]
        public string DirecciónPuntoPartida { get; set; }
        //[SAPColumn("U_VS_GLNLLEG", false)]
        public string UbigeoPuntoLlegada { get; set; }

        //[SAPColumn("U_VS_DIRLLEG", false)]
        public string DireccionPuntoLlegada { get; set; }


        //[SAPColumn("U_VS_MODTRA", false)]
        public string ModalidadTraslado { get; set; }
        //[SAPColumn("U_VS_CODTCOND", false)]
        public string TipoDocumentoConductor { get; set; }
        //[SAPColumn("U_VS_DOCCOND", false)]
        public string DocumentoConductor { get; set; }

        [SAPColumn("U_EXX_NOMCONDU", false)]
        public string NombreConductor { get; set; }

        [SAPColumn("U_EXX_LICCONDU", false)]
        public string LicenciaConductor { get; set; }

        [SAPColumn("U_EXX_PLACAVEH", false)]
        public string PlacaVehiculo { get; set; }

        //[SAPColumn("U_BPP_MDVN", false)]
        public string MarcaVehiculo { get; set; }

        [SAPColumn("U_EXX_CODTRANS", false)]
        public string CodigoTransportista { get; set; }

        [SAPColumn("U_EXX_RUCTRANS", false)]
        public string RucTransportista { get; set; }

        [SAPColumn("U_EXX_NOMTRANS", false)]
        public string NombreTransportista { get; set; }

        [SAPColumn("U_EXX_DIRTRANS", false)]
        public string DireccionTransportista { get; set; }

        [SAPColumn("U_EXX_TIPOOPER", false)]
        public string TipoOperacion { get; set; }

        [SAPColumn("U_EXX_FE_MODTRA", false)]
        public string FEXModalidadTraslado { get; set; }

        [SAPColumn("U_EXX_FE_GR_FEEntrega", false)]
        public DateTime FechaGuia { get; set; }

        [SAPColumn("U_EXX_FE_GR_FInicio", false)]
        public DateTime FechaInicioTraslado { get; set; }

        [SAPColumn("U_EXK_HOJARUTA", false)]
        public string HojaRuta { get; set; }




        //[SAPColumn("U_VS_NROMTC", false)]
        public string NroRegistroMTC { get; set; }

        //[SAPColumn("U_VS_TARUNI", false)]
        public string TarjetaUnicaCirculacion { get; set; }

        public int ReferencedObjectType { get; set; }
        public int ReferencedDocEntry { get; set; }
        public int ReferencedDocEntryGuia { get; set; }

        public int NroSapCobro { get; set; }
        public int NroSapCobroTransId { get; set; }

        private class SortDistributionPriority : IComparer<string>
        {
            public int Compare(string a, string b)
            {
                if (a == null || b == null)
                    return 0;

                if (b == a)
                    return 0;

                if (b == DistributionPriorities.HIGH)
                    return 1;

                switch (a)
                {
                    case DistributionPriorities.HIGH:
                    case DistributionPriorities.NORMAL:
                        return -1;
                    case DistributionPriorities.LOW:
                        return 1;
                    default:
                        throw new Exception();
                }
            }
        }
    }

    [Serializable]
    public abstract class SAPDocument<T> : SAPDocument where T : SAPDocumentLine
    {
        [SAPColumn(detailLine: true)]
        public IEnumerable<T> DocumentLines { get; set; }
    }
}