using MimeKit;
using System;

namespace Klika.Dinero.Model.Constants.Email
{
    public  class RawEmailTemplates
    {
        public static BodyBuilder GetErrorListMessageBody(byte[] attachment)
        {
            var body = new BodyBuilder()
            {
                HtmlBody = @"<p>Hi,</p><p> Your CSV document was not successfully imported.</p> <p>These transactions are invalid or missing information. Please re - check the data fields in CSV document and try again.</p>",
                TextBody = "Hi, Your CSV document was not successfully imported. These transactions are invalid or missing information.Please re - check the data fields in CSV document and try again.",
            };

            body.Attachments.Add("CsvErrors.xlsx", attachment);
            return body;
        }

        public static BodyBuilder GetTransactionsMessageBody(byte[] attachment, DateTime date)
        {
            var body = new BodyBuilder()
            {
                HtmlBody = @"<p>Hi,</p><p> Your CSV document was successfully exported.</p>",
                TextBody = "Hi, Your CSV document was successfully exported.",
            };

            body.Attachments.Add($"DineroTransactions-{date.Year.ToString()}-{date.Month.ToString()}.xlsx", attachment);
            return body;
        }

        public static BodyBuilder GetNoTransactionsMessageBody()
        {
            var body = new BodyBuilder()
            {
                HtmlBody = @"<p>Hi,</p><p> There are no transactions for the given month.</p>",
                TextBody = "Hi, There are no transactions for the given month.",
            };

            return body;
        }
    }
}
