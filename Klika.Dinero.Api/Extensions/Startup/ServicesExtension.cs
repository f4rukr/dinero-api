using Klika.Dinero.Model.Interfaces;
using Klika.Dinero.Service.Accounts;
using Klika.Dinero.Service.Analytics;
using Klika.Dinero.Service.Banks;
using Klika.Dinero.Service.Bulk;
using Klika.Dinero.Service.File;
using Klika.Dinero.Service.Mail;
using Klika.Dinero.Service.Transactions;
using Microsoft.Extensions.DependencyInjection;

namespace Klika.Dinero.Api.Extensions.Startup
{
    public static class ServicesExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {   
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<ICsvTransactionParser, CsvTransactionParser>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IBankService, BankService>();
            services.AddScoped<IAnalyticService, AnalyticService>();
            services.AddScoped<IBulkService, BulkService>();
            services.AddScoped<IFileGeneratorService, FileGeneratorService>();
            services.AddScoped<IMailService, MailService>();

            return services;
        }
    }
}
