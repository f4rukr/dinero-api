namespace Klika.Dinero.Model.Errors
{
    public class ApiError
    {
        public string Code { get; set; }
        public string Description { get; set; }

        public ApiError(string code, string description)
        {
            Code = code;
            Description = description;
        }
    }
}