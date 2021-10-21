using Klika.Dinero.Model.Helpers.Bulk;
using Klika.Dinero.Model.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Klika.Dinero.Service.Bulk
{
    public class BulkService : IBulkService
    {
        private readonly ILogger<BulkService> _logger;

        public BulkService(ILogger<BulkService> logger)
        {
            _logger = logger;
        }

        public async Task BulkInsertAsync<TEntity, TInsertEntity>(BulkInsertConfig<TEntity, TInsertEntity> config) where TInsertEntity : class
        {
            try
            {
                SqlTransaction transaction = null;
                using (var connection = new SqlConnection(config.ConnectionString))
                {
                    connection.Open();
                    transaction = connection.BeginTransaction();

                    using (var sqlCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
                    {
                        sqlCopy.DestinationTableName = config.DestinationTable;
                        sqlCopy.BatchSize = config.BatchSize;

                        foreach (var mapping in config.ColumnMappings)
                        {
                            sqlCopy.ColumnMappings.Add(mapping.Key, mapping.Value);
                        }

                        await sqlCopy.WriteToServerAsync(config.DataTable).ConfigureAwait(false);
                        transaction.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(BulkInsertAsync));
                throw;
            }
        }
    }
}
