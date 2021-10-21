using Klika.Dinero.Model.Constants.Csv;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace Klika.Dinero.Model.DTO.Transaction.Response
{
    public class TransactionCSVExport : FileResult
    {
        private readonly List<TransactionCSVExportItemDTO> _transactions;
        public TransactionCSVExport(List<TransactionCSVExportItemDTO> transactions, string fileDownloadName) : base("text/csv")
        {
            _transactions = transactions;
            FileDownloadName = fileDownloadName;
        }
        public async override Task ExecuteResultAsync(ActionContext context)
        {
            string d = CsvExportConstants.delimiter;
            var response = context.HttpContext.Response;
            context.HttpContext.Response.Headers.Add("Content-Disposition", new[] { "attachment; filename=" + FileDownloadName });
            
            using (var streamWriter = new StreamWriter(response.Body))
            {
                await streamWriter.WriteLineAsync(CsvExportConstants.csvColumnHeader).ConfigureAwait(false);

                foreach (var t in _transactions)
                {
                    await streamWriter.WriteLineAsync(
                      $"{t.DateOfTransaction.ToString(CsvExportConstants.dateTimeFormat, CultureInfo.InvariantCulture)}{d}{t.Designation}{d}{t.Amount}{d}{t.Bank}{d}{t.AccountNumber}{d}{t.IBAN}"
                    ).ConfigureAwait(false);
                    await streamWriter.FlushAsync().ConfigureAwait(false);
                }
                await streamWriter.FlushAsync().ConfigureAwait(false);
            }
        }
    }
}