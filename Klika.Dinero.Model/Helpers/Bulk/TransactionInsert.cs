using Klika.Dinero.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klika.Dinero.Model.Helpers.Bulk
{
    public class TransactionInsert
    {
        public int AccountId { get; set; }
        public DateTime DateOfTransaction { get; set; }
        public int TransactionCategoryId { get; set; }
        public string Designation { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
