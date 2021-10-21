using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Klika.Dinero.Model.Errors;

namespace Klika.Dinero.Model.Response
{
    public class ApiResponse
    {
        [JsonIgnore]
        public List<ApiError> Errors;

        [JsonIgnore]
        public bool Succeeded => Errors.Count == 0;

        [JsonIgnore]
        public string ErrorCode => Errors.FirstOrDefault()?.Code;
        
        public ApiResponse()
        {
            Errors = new List<ApiError>();
        }

        public ApiResponse(string code, string description)
        {
            Errors = new List<ApiError>()
            {
                new ApiError(code, description)
            };
        }

        public ApiErrorResponse GetErrorResponse()
        {
            return new ApiErrorResponse(Errors);
        }
    }
}