using System;
using System.ComponentModel.DataAnnotations;
using Klika.Dinero.Model.Errors;
using Klika.Dinero.Model.Extensions.Annotations;

namespace Klika.Dinero.Model.DTO.Transaction.Request
{
    public class TransactionRequestDTO
    {
        public string UserId { get; set; }
        
        [Required]
        public int BankId { get; set; }
        
        // [Required]
        // public int TransactionCategoryId { get; set; }
        
        [Required]
        [DateRange("2019/1/1", ErrorMessage = ErrorDescriptions.InvalidDate)]
        public DateTime DateOfTransaction { get; set; }
        
        [Required]
        public string Designation { get; set; }
        
        [Required]
        [Range(0d, double.MaxValue, ErrorMessage = ErrorDescriptions.InvalidAmount)]
        public decimal Amount { get; set; }
   }
}