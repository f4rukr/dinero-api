using Klika.Dinero.Model.Errors;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klika.Dinero.Model.Interfaces
{
    public interface IMailService
    {
        Task SendEmailAsync(string to, string subject, BodyBuilder body);
        Task SendEmailAsync(string to, string subject, string body);
    }
}
