using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json.Serialization;

namespace Klika.Dinero.Model.DTO.Transaction.Request
{

    public class TransactionUploadRequestDTO
    {
        [JsonIgnore]
        public string Email { get; set; }

        [JsonIgnore]
        public string UserId { get; set; }
       
        public IFormFile File { get; set; }
    }
}