using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Klika.Dinero.Model.Constants.Email;
using Klika.Dinero.Model.Email;
using Klika.Dinero.Model.Errors;
using Klika.Dinero.Model.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klika.Dinero.Service.Mail
{
    public class MailService : IMailService
    {
        private readonly IOptions<EmailSettings> _emailSettings;
        private readonly ILogger<MailService> _logger;

        public MailService(IOptions<EmailSettings> emailSettings,
            ILogger<MailService> logger)
        {
            _emailSettings = emailSettings;
            _logger = logger;
        }     

        private MimeMessage GetMessage(BodyBuilder body, string subject, string toEmail)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailSettings.Value.FromName, _emailSettings.Value.From));
            message.To.Add(new MailboxAddress(string.Empty, toEmail));
            message.Subject = subject;
            message.Body = body.ToMessageBody();
            return message;
        }


        public async Task SendEmailAsync(string to, string subject, BodyBuilder body)
        {
            try
            {
                using (var stream = new MemoryStream())
                {
                     using (var client = new AmazonSimpleEmailServiceClient(RegionEndpoint.EUCentral1))
                     {
                        GetMessage(body, subject, to).WriteTo(stream);
                        var sendRequest = new SendRawEmailRequest { RawMessage = new RawMessage(stream) };
                    
                        await client.SendRawEmailAsync(sendRequest).ConfigureAwait(false);
                   
                     }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(SendEmailAsync));
                throw;
            }

        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                using (var client = new AmazonSimpleEmailServiceClient(RegionEndpoint.EUCentral1))
                {
                    var sendRequest = new SendEmailRequest
                    {
                        Source = _emailSettings.Value.From,
                        Destination = new Destination
                        {
                            ToAddresses =
                            new List<string> { to }
                        },
                        Message = new Message
                        {
                            Subject = new Content(subject),
                            Body = new Body
                            {
                                Html = new Content
                                {
                                    Charset = "UTF-8",
                                    Data = body
                                }
                            }
                        },
                    };
                
                   await client.SendEmailAsync(sendRequest).ConfigureAwait(false);               
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(SendEmailAsync));
                throw;
            }
        }
    }
}
