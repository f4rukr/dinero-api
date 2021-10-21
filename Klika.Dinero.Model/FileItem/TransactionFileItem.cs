using System;
using Klika.Dinero.Model.Entities;

namespace Klika.Dinero.Model.FileItem
{
    public class TransactionFileitem
    {
        public long LineIndex { get; set; }
        public Transaction Transaction { get; set; }
        public bool IsExpense { get; set; }
        public string BankName { get; set; }
        public int BankId { get; set; }
        public string AccountNumber { get; set; }

        private string _iban;
        public string IBAN
        {
            get
            {
                return _iban;
            }
            set
            {
                _iban = value != String.Empty ? value : null;
            }
        }

        public override bool Equals(object? param)
        {
            if (param is TransactionFileitem tfi)
            {
                return this.LineIndex == tfi.LineIndex &&
                       this.Transaction.DateOfTransaction == tfi.Transaction.DateOfTransaction &&
                       this.Transaction.Designation == tfi.Transaction.Designation &&
                       this.Transaction.Amount == tfi.Transaction.Amount &&
                       this.IsExpense == tfi.IsExpense &&
                       this.BankName == tfi.BankName &&
                       this.BankId == tfi.BankId &&
                       this.AccountNumber == tfi.AccountNumber &&
                       this.IBAN == tfi.IBAN;
            }
            
            return false;
        }
    }
}