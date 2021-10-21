using Klika.Dinero.Model.Errors;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Klika.Dinero.Model.Extensions.ModelBinders
{
    /// <summary>
    /// Parse comma separated query string parameters to List
    /// </summary>
    public class CommaSeparatedModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException(nameof(bindingContext));

            var values = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).ToString();

            if (values.Length == 0)
                return Task.CompletedTask;

            try
            {
                bindingContext.Result = ModelBindingResult.Success(
                    values.Split(',').Select(int.Parse).ToList());
            }
            catch (Exception ex)
            {
                bindingContext.ModelState.AddModelError(
                    ErrorCodes.InvalidFormat, ErrorDescriptions.RequestParameterInvalidFormat);
            }

            return Task.CompletedTask;
        }
    }
}
