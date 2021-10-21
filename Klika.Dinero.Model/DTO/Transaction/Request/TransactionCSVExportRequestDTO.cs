using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Klika.Dinero.Model.DTO.Transaction.Request
{
    public class TransactionCSVExportRequestDTO
    {
        [JsonIgnore]
        public string UserId { get; set; }

        [JsonIgnore]
        public string Email { get; set; }

        [Required]
        public DateTime? Date { get; set; }
        public string AccountNumber { get; set; }
    }
}
