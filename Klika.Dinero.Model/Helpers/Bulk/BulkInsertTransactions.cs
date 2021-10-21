using Klika.Dinero.Model.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klika.Dinero.Model.Helpers.Bulk
{
    public class BulkInsertTransactions : BulkInsertConfig<Transaction, TransactionInsert>
    {
        public BulkInsertTransactions(List<Transaction> entities) : base(entities)
        {

        }

        protected override void MapToDataTable(List<Transaction> entities)
        {
            foreach(var transaction in entities)
            {
                DataRow dr = DataTable.NewRow();
                dr["AccountId"] = transaction.Account.Id;
                dr["DateOfTransaction"] = transaction.DateOfTransaction;
                dr["TransactionCategoryId"] = transaction.TransactionCategoryId;
                dr["Designation"] = transaction.Designation;
                dr["Amount"] = transaction.Amount;
                dr["CreatedAt"] = DateTime.Now;
                dr["UpdatedAt"] = DateTime.Now;
                DataTable.Rows.Add(dr);
            }
        }
    }
}
