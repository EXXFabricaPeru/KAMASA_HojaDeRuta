namespace Exxis.Addon.HojadeRutaAGuia.Domain.EDIStrategy.Structures
{
    public struct EDIRecord
    {
        public string EDIIdentifier { get; set; }
        public string EDIAddressCode { get; set; }
        public string SAPAddressCode { get; set; }
        public string SAPAddressName { get; set; }
        public string SAPAddressStreet { get; set; }
        public string EDIItemCode { get; set; }
        public string SAPItemCode { get; set; }
        public string SAPItemName { get; set; }
        public decimal EDIItemPrice { get; set; }
        public decimal SAPItemPrice { get; set; }
        public decimal EDIItemQuantity { get; set; }
        public string ErrorMessage { get; set; }
        public string AvailableStatus { get; set; }

        public static class AvailableState
        {
            public const string CORRECT = "C";
            public const string ERROR = "E";
            public const string PENDING = "P";
        }
    }

    
}