namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Constant
{
    public static class DistributionFlow
    {
        public const string ITEM_SALE = "SA";
        public const string ITEM_SALE_DESCRIPTION = "Flujo de Venta";
        public const string SERVICE_SALE = "SE";
        public const string SERVICE_SALE_DESCRIPTION = "Flujo de Servicio";
        public const string INVENTORY_TRANSFER = "TR";
        public const string INVENTORY_TRANSFER_DESCRIPTION = "Flujo de Traslado";
        public const string RETURN = "RE";
        public const string RETURN_DESCRIPTION = "Flujo de Devolución";
        public const string PURCHASE = "PU";
        public const string PURCHASE_DESCRIPTION = "Flujo de Compras";
        public const string ITEM_SALE_PARTIAL = "SP";
        public const string ITEM_SALE_PARTIALE_DESCRIPTION = "Flujo de Venta Parcial";

        public static string GetCodeFromDescription(string description)
        {
            switch (description)
            {
                case ITEM_SALE_DESCRIPTION:
                    return ITEM_SALE;
                case SERVICE_SALE_DESCRIPTION:
                    return SERVICE_SALE;
                case INVENTORY_TRANSFER_DESCRIPTION:
                    return INVENTORY_TRANSFER;
                case RETURN_DESCRIPTION:
                    return RETURN;
                case PURCHASE_DESCRIPTION:
                    return PURCHASE;
                default:
                    return string.Empty;
            }
        }
    }
}