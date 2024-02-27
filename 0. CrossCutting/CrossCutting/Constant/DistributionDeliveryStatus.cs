namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Constant
{
    public static class DistributionDeliveryStatus
    {
        public const string DELIVERED = "DE";
        public const string DELIVERED_DESCRIPTION = "Entregado";
        public const string NOT_DELIVERED = "ND";
        public const string NOT_DELIVERED_DESCRIPTION = "No Entregado";
        public const string REJECTED = "RE";
        public const string REJECTED_DESCRIPTION = "Rechazado";
        public const string RESCHEDULED = "RS";
        public const string RESCHEDULED_DESCRIPTION = "Reprogramado";
        public const string IN_PROGRESS = "IP";
        public const string IN_PROGRESS_DESCRIPTION = "En Ruta";
        public const string PROGRAMMED = "PR";
        public const string PROGRAMMED_DESCRIPTION = "Programado";
        public const string RESCHEDULED_TRANS = "TR";
        public const string RESCHEDULED_TRANS_DESCRIPTION = "Reprogramado Transportista";

        public const string RETURNED = "RT";
        public const string RETURNED_DESCRIPTION = "Devuelto Correctamente";
        public const string RETURNED_OBSERVATION = "RO";
        public const string RETURNED_OBSERVATION_DESCRIPTION = "Devuelto Observado";

        public const string TRANSFERRED = "TF";
        public const string TRANSFERRED_DESCRIPTION = "Transferido Correctamente";
        public const string TRANSFERRED_OBSERVATION = "TO";
        public const string TRANSFERRED_OBSERVATION_DESCRIPTION = "Transferido Observado";

        public const string ASSIGN = "AS";
        public const string ASSIGN_DESCRIPTION = "Lote Asignado";

        public const string TRANSSHIPPING = "TS";
        public const string TRANSSHIPPING_DESCRIPTION = "Transbordo";

        public const string PENDING = "PE";
        public const string PENDING_DESCRIPTION = "Pendiente";

        public const string ANULADO = "AN";
        public const string ANULADO_DESCRIPTION = "Anulado";
    }
}