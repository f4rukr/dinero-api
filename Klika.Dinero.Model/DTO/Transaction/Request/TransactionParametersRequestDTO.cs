using Klika.Dinero.Model.Errors;
using Klika.Dinero.Model.Extensions.Annotations;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Klika.Dinero.Model.Constants.RegexConstants;
using Klika.Dinero.Model.Extensions.ModelBinders;

namespace Klika.Dinero.Model.DTO.Transaction.Request
{
    public class TransactionParametersRequestDTO
    {
        [JsonIgnore]
        public string UserId { get; set; }

        [RegularExpression(RegexValidators.AccountNumberPattern, ErrorMessage = RegexMessages.InvalidAccountNumber)]
        public string AccountNumber { get; set; }

        public int? BankId { get; set; }

        [Required(ErrorMessage = ErrorDescriptions.CurrentIndexRequired)]
        [Range(0, int.MaxValue, ErrorMessage = ErrorDescriptions.InvalidRange)]
        public int? CurrentIndex { get; set; }

        [Required(ErrorMessage = ErrorDescriptions.NumberOfElementsRequired)]
        [Range(1, int.MaxValue, ErrorMessage = ErrorDescriptions.InvalidRange)]
        public int? NumberOfElements { get; set; }

        [ModelBinder(BinderType = typeof(CommaSeparatedModelBinder))]
        public List<int> Categories { get; set; }

        [DateRange("2019/1/1", ErrorMessage = ErrorDescriptions.InvalidDate)]
        public DateTime? Date { get; set; }
    }
}
