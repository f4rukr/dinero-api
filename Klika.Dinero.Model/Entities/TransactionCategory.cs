using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Klika.Dinero.Model.Entities
{
    public class TransactionCategory : IEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(64)]
        public string Name { get; set; }
        [MaxLength(128)]
        public string Description { get; set; }

        [MaxLength(500)]
        public string Keywords { get; set; }
        public List<Transaction> Transactions { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
