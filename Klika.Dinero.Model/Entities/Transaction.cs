using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Klika.Dinero.Model.Entities
{
    public class Transaction : IEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Account Account { get; set; }
        public int AccountId { get; set; }
        public TransactionCategory TransactionCategory { get; set; }
        public int TransactionCategoryId { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Amount { get; set; }

        [MaxLength(500)]
        public string Designation { get; set; }
        public DateTime DateOfTransaction { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
