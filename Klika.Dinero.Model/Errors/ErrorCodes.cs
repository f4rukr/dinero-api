namespace Klika.Dinero.Model.Errors
{
    public static class ErrorCodes {
        public const string NotFound = "NotFound";
        public const string AlreadyExist = "AlreadyExist";
        public const string InvalidFormat = "BadRequest";
        public const string CsvInsertError = "CsvInsertError";
        public const string UnsupportedContentType = "UnsupportedContentType";
        public const string UnsupportedExtension = "UnsupportedExtension";
        public const string MissingParameter = "MissingParameter";
        public const string EmptyPayload = "EmptyPayload";
        public const string PayloadTooLarge = "PayloadTooLarge";
        public const string InvalidConfiguration = "InvalidConfiguration";
        public const string InvalidCsvHeaders = "InvalidCsvHeaders";
        public const string MissingCsvHeaders = "MissingCsvHeaders";
        public const string SentOnEmail = "SentOnEmail";
    }
}