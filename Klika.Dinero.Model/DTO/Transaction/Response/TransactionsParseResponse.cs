using System.Collections.Generic;
using Klika.Dinero.Model.Errors;
using Klika.Dinero.Model.FileItem;

namespace Klika.Dinero.Model.DTO.Transaction.Response
{
    public class TransactionsParseResponse
    {
        public List<TransactionFileitem> ParsedTransactions { get; set; }
        public List<ApiError> ParseErrors { get; set; }

        public TransactionsParseResponse(CsvParseError error)
        {
            ParsedTransactions = new List<TransactionFileitem>();
            ParseErrors = new List<ApiError> { error };
        }

        public TransactionsParseResponse(List<TransactionFileitem> parsedTransactions)
        {
            ParsedTransactions = parsedTransactions;
            ParseErrors = new List<ApiError> { };
        }

        public TransactionsParseResponse(List<CsvParseError> parseErrors)
        {
            ParsedTransactions = new List<TransactionFileitem>();
            ParseErrors = new List<ApiError>(parseErrors);
        }
    }
}