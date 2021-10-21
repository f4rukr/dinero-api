using ClosedXML.Excel;
using Klika.Dinero.Model.Constants.Csv;
using Klika.Dinero.Model.DTO.Transaction.Response;
using Klika.Dinero.Model.Errors;
using Klika.Dinero.Model.Interfaces;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Klika.Dinero.Service.File
{
    public class FileGeneratorService : IFileGeneratorService
    {
        public byte[] GenerateErrorsExcelDocument(List<ApiError> apiErrors)
        {

            byte[] workbookBytes = new byte[0];
            var numberOfErrors = apiErrors.Count < 20000 ? apiErrors.Count : 20000;
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Error_Sheet");
                

                worksheet.Cell(1, 1).Value = "Error Code";
                worksheet.Cell(1, 1).Style.Font.Bold = true;

                worksheet.Cell(1, 2).Value = "Error Description";
                worksheet.Cell(1, 2).Style.Font.Bold = true;

                for (int index = 1; index <= numberOfErrors ; index++)
                {
                    worksheet.Cell(index + 1, 1).Value = apiErrors[index - 1].Code;
                    worksheet.Cell(index + 1, 1).RichText.SetFontColor(XLColor.Red);
                    worksheet.Cell(index + 1, 2).Value = apiErrors[index - 1].Description;
                }

                worksheet.Columns().AdjustToContents();

                using (var ms = new MemoryStream())
                {
                    workbook.SaveAs(ms);
                    workbookBytes = ms.ToArray();
                }
            }
            return workbookBytes;
        }

        public byte[] GenerateTransactionsExcelDocument(List<TransactionCSVExportItemDTO> transactions)
        {

            byte[] workbookBytes = new byte[0];
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Transactions_Sheet");

                worksheet.Cell(1, 1).Value = "date_of_transaction";
                worksheet.Cell(1, 1).Style.Font.Bold = true;
                worksheet.Cell(1, 2).Value = "designation";
                worksheet.Cell(1, 2).Style.Font.Bold = true;
                worksheet.Cell(1, 3).Value = "amount";
                worksheet.Cell(1, 3).Style.Font.Bold = true;
                worksheet.Cell(1, 4).Value = "bank";
                worksheet.Cell(1, 4).Style.Font.Bold = true;
                worksheet.Cell(1, 5).Value = "account_no";
                worksheet.Cell(1, 5).Style.Font.Bold = true;
                worksheet.Cell(1, 6).Value = "iban";
                worksheet.Cell(1, 6).Style.Font.Bold = true;

                for (int index = 1; index <= transactions.Count; index++)
                {
                    worksheet.Cell(index + 1, 1).Value = transactions[index - 1].DateOfTransaction
                        .ToString(CsvExportConstants.dateTimeFormat, CultureInfo.InvariantCulture);
                    worksheet.Cell(index + 1, 2).Value = transactions[index - 1].Designation;
                    worksheet.Cell(index + 1, 3).Value = transactions[index - 1].Amount
                        .ToString(CsvExportConstants.amountFormat);
                    worksheet.Cell(index + 1, 4).Value = transactions[index - 1].Bank;
                    worksheet.Cell(index + 1, 5).Value = transactions[index - 1].AccountNumber;
                    worksheet.Cell(index + 1, 6).Value = transactions[index - 1].IBAN;
                }

                worksheet.Columns().AdjustToContents();

                using (var ms = new MemoryStream())
                {
                    workbook.SaveAs(ms);
                    workbookBytes = ms.ToArray();
                }
            }
            return workbookBytes;
        }
    }
}
