using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Constant
{
    public static class DocumentManagmentStatus
    {
        public static class GeneracionStatus
        {
            public const string PENDIENTE = "Pendiente";
            public const string GENERADO = "Generado";
            public const string IMPRESO = "Impreso";
            public const string ANULADO = "Anulado";
            public const string REPROGRAMADO = "Reprogramado";
        }

        public static class ComprobanteStatus
        {
            public const string PENDIENTE = "Pendiente";
            public const string GENERADO = "Generado";
            public const string IMPRESO = "Impreso";
            public const string ANULADO = "Anulado";
            public const string NO_SOLICITADO = "No Solicitado";
            public const string FACTURADO_INTERNO = "Factura Manual";
        }
        public static class CertificadoStatus
        {
            public const string PENDIENTE = "Pendiente";
            public const string IMPRESO = "Impreso";
            public const string ANULADO = "Anulado";
        }
    }
}
