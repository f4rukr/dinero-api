using Microsoft.AspNetCore.Builder;

namespace Klika.Dinero.Api.Extensions.Api
{
    public static class ApiEndpointsExtension
    {
        public static void UseApiEndpoints(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
