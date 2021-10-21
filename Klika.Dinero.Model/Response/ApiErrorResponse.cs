using System.Collections.Generic;
using Klika.Dinero.Model.Errors;

namespace Klika.Dinero.Model.Response
{
    public class ApiErrorResponse 
    {
        public List<ApiError> Errors { get; set; }

        public ApiErrorResponse() 
        {
            Errors = new List<ApiError> { };
        }

        public ApiErrorResponse(string code, string description)
        {
            Errors = new List<ApiError>
            {
                new ApiError(code, description)
            };
        }

        public ApiErrorResponse(string type)
        {
            Errors = new List<ApiError>
            {
                new ApiError(type, "")
            };
        }
        
        public ApiErrorResponse(List<ApiError> errors)
        {
            Errors = errors;
        }
    }
}