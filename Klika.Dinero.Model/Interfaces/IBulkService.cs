using Klika.Dinero.Model.Helpers.Bulk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klika.Dinero.Model.Interfaces
{
    public interface IBulkService
    {
        public Task BulkInsertAsync<TEntity, TInsertEntity>(BulkInsertConfig<TEntity, TInsertEntity> config) where TInsertEntity : class;
    }
}
