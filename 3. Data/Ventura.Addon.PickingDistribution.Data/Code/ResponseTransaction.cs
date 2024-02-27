namespace Exxis.Addon.HojadeRutaAGuia.Data.Code
{
    public struct ResponseDocumentTransaction
    {
        public int DocumentEntry { get; }
        public int DocumentNumber { get; }
        public bool IsSuccess { get; }
        public string Message { get; }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        private ResponseDocumentTransaction(int documentEntry, int documentNumber, bool isSuccess, string message)
        {
            DocumentEntry = documentEntry;
            DocumentNumber = documentNumber;
            IsSuccess = isSuccess;
            Message = message;
        }

        public static ResponseDocumentTransaction MakeSuccessTransaction(int documentEntry, int documentNumber)
        {
            return new ResponseDocumentTransaction(documentEntry, documentNumber, true, string.Empty);
        }

        public static ResponseDocumentTransaction MakeFailureTransaction(string message)
        {
            return new ResponseDocumentTransaction(default(int), default(int), false, message);
        }
    }

    public struct ResponseTransaction
    {
        public bool IsSuccess { get; }
        public string Message { get; }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        private ResponseTransaction(bool isSuccess, string message)
        {

            IsSuccess = isSuccess;
            Message = message;
        }

        public static ResponseTransaction MakeSuccessTransaction()
        {
            return new ResponseTransaction(true, string.Empty);
        }

        public static ResponseTransaction MakeFailureTransaction(string message)
        {
            return new ResponseTransaction(false, message);
        }
    }
}