using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApplication1.Helper
{
    public class AutorizacijaSwaggerHeader : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "autentifikacija-token",
                In = ParameterLocation.Header,
                Description = "upisati token preuzet iz autentifikacijacontrollera"
            });
        }
    }
}
