using Klika.Dinero.Model.Extensions.ModelBinders;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Klika.Dinero.Model.DTO.Analytic.Request
{
    public class AnalyticRequestDTO
    {
        [JsonIgnore]
        public string UserId { get; set; }

        [Required]
        public DateTime? Date { get; set; }
        public string AccountNumber { get; set; }

        [ModelBinder(BinderType = typeof(CommaSeparatedModelBinder))]
        public List<int> BankIds { get; set; }
    }
}
