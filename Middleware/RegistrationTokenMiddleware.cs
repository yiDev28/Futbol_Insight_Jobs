using System.Text.Json;

namespace Futbol_Insight_Jobs.Middleware
{

    public class RegistrationTokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _expectedToken;

        public RegistrationTokenMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _expectedToken = configuration["Security:RegisToken"];
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Verificar si la ruta es para el registro de usuarios
            if (context.Request.Path.StartsWithSegments("/api/v1/Access/Register") || context.Request.Path.StartsWithSegments("/api/v1/Access/Signin"))
            {
                // Obtener el token del header
                if (!context.Request.Headers.TryGetValue("X-Api-Token-Users", out var providedToken))
                {
                    var errorResponse = new
                    {
                        code = 401,
                        message = "El encabezado 'X-Api-Token-Users' no fue proporcionado.",
                        details = "Missing user token.",
                        data = false
                    };

                    var jsonResponse = JsonSerializer.Serialize(errorResponse);

                    context.Response.ContentType = "application/json";

                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync(jsonResponse);
                    return;
                }

                // Validar el token
                if (!providedToken.Equals(_expectedToken))
                {
                    var errorResponse = new
                    {
                        code = 401,
                        message = "Api Token incorrecta.",
                        details = "Invalid user token.",
                        data = false
                    };

                    var jsonResponse = JsonSerializer.Serialize(errorResponse);

                    context.Response.ContentType = "application/json";

                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync(jsonResponse);
                    return;
                }
            }

            // Si el token es válido o no es la ruta de registro, continuar al siguiente middleware
            await _next(context);
        }
    }

}
