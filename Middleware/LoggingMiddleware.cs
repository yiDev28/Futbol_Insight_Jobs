using LogServiceYiDev.Data;
using LogServiceYiDev.Models;
using System.Reflection;
using System.Text.Json;
using System.Text;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _applicationName;
    private readonly bool _statusLog;
    private readonly IDictionary<string, bool> _logTypes;

    public LoggingMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _applicationName = Assembly.GetExecutingAssembly().GetName().Name;
        _statusLog = configuration.GetValue<bool>("Logs:Status");
        _logTypes = configuration.GetSection("Logs:Type").Get<Dictionary<string, bool>>();
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!_statusLog)
        {
            // Si el logging está desactivado, simplemente pasa al siguiente middleware
            await _next(context);
            return;
        }

        // Obtener el contexto de datos del log
        var logDataContext = context.RequestServices.GetService<LogDataContext>();

        // Crear un nuevo objeto LogModel para almacenar la información del log
        var log = new LogModel
        {
            log_uuid = Guid.NewGuid(),
            log_aplicacion = _applicationName,
            log_servicio = context.Request.Path,
            log_endpoint = context.Request.Host + context.Request.Path,
            log_fec_req = DateTime.Now,
            log_request = await FormatRequest(context.Request),
            log_origen = context.Connection.RemoteIpAddress?.ToString() ?? "",
            log_usuario = 9999,
            log_estado = 0,
            log_fec_reg = DateTime.Now
        };

        var originalBodyStream = context.Response.Body;
        using (var responseBody = new MemoryStream())
        {
            context.Response.Body = responseBody;
            try
            {
                // Continuar con la solicitud
                await _next(context);
                // Obtener y procesar la respuesta
                var res = await FormatResponse(context.Response);
                using (JsonDocument document = JsonDocument.Parse(res))
                {
                    JsonElement root = document.RootElement;

                    log.log_cod_resp = root.GetProperty("code").GetInt32();
                    log.log_desc_resp = root.GetProperty("details").GetString();
                }

                // Actualizar el log con la información de la respuesta
                log.log_fec_response = DateTime.Now;
                log.log_cod_http = context.Response.StatusCode;
                log.log_response = res;

                // Modificar la respuesta para el cliente
                var modifiedResponse = ModifyResponseForClient(res);

                // Restaurar el cuerpo original de la respuesta y escribir la respuesta modificada
                context.Response.Body = originalBodyStream;
                context.Response.ContentType = "application/json";
                context.Response.ContentLength = modifiedResponse.Length;
                await context.Response.WriteAsync(modifiedResponse);
            }
            catch (Exception ex)
            {
                // Manejar excepciones, registrar el error y modificar la respuesta para el cliente
                log.log_fec_response = DateTime.Now;
                log.log_cod_resp = 500;
                log.log_desc_resp = ex.Message;
                log.log_estado = 0;

                var errorDetails = new
                {
                    code = 500,
                    message = "Ocurrió un error interno en el servidor al generar el log",
                    details = ex.Message
                };

                log.log_response = JsonSerializer.Serialize(errorDetails);

                var modifiedResponse = ModifyResponseForClient(JsonSerializer.Serialize(errorDetails));

                context.Response.Body = originalBodyStream;
                context.Response.ContentType = "application/json";
                context.Response.ContentLength = modifiedResponse.Length;
                await context.Response.WriteAsync(modifiedResponse);
            }
            finally
            {
                // Guardar el log si corresponde
                if (ShouldLog(log.log_cod_resp))
                {
                    SaveLog(logDataContext, log);
                }

                // Restablecer el stream y copiar la respuesta modificada
                responseBody.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }
    }

    // Modifica la respuesta para el cliente eliminando el campo 'details'
    private string ModifyResponseForClient(string responseText)
    {
        using (JsonDocument document = JsonDocument.Parse(responseText))
        {
            var root = document.RootElement;
            var code = root.GetProperty("code").GetInt32();
            var message = root.GetProperty("message").GetString();
            var data = root.GetProperty("data");

            // Crear un nuevo objeto de respuesta sin el campo 'details'
            var modifiedResponse = new
            {
                
                code = code,
                message = message,
                details = "",
                data = data,
            };

            return JsonSerializer.Serialize(modifiedResponse);
        }
    }

    // Determina si se debe registrar el log basándose en el código de error
    private bool ShouldLog(int errorCode)
    {
        var error = ErrorDictionary.Errors.FirstOrDefault(e => e.ErrorCode == errorCode);
        if (error != null && _logTypes.TryGetValue(error.Type, out bool shouldLog))
        {
            return shouldLog || _logTypes["All"];
        }

        return _logTypes["Info"] || _logTypes["All"];
    }

    // Formatea la solicitud para registrar el cuerpo
    private async Task<string> FormatRequest(HttpRequest request)
    {
        request.EnableBuffering();
        using (var reader = new StreamReader(
                request.Body,
                encoding: Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                bufferSize: 1024,
                leaveOpen: true))
        {
            var bodyAsText = await reader.ReadToEndAsync();
            request.Body.Position = 0;
            return bodyAsText;
        }
    }

    // Formatea la respuesta para registrar el cuerpo
    private async Task<string> FormatResponse(HttpResponse response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);
        string text = await new StreamReader(response.Body).ReadToEndAsync();
        response.Body.Seek(0, SeekOrigin.Begin);
        return text;
    }

    // Guarda el log en la base de datos
    private void SaveLog(LogDataContext logDataContext, LogModel log)
    {
        try
        {
            logDataContext.admlogs.Add(log);
            logDataContext.SaveChanges();
        }
        catch (Exception ex)
        {

            throw;
        }
        
    }
}
