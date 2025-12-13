
using LocationSystem.Application.Exceptions;
using LocationSystem.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace LocationSystem.Api.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;
        public ExceptionHandlerMiddleware(RequestDelegate requestDelegate,ILogger<ExceptionHandlerMiddleware> logger) 
        {
            _next = requestDelegate;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e) 
            {
                _logger.LogError(e, "Unhandled exception occurred");
                await HandleExceptionAsync(context,e);
            }
        }
        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            object responseData;

            // Default response
            responseData = new
            {
                error = exception.Message,
                detail = exception.StackTrace
            };

            switch (exception)
            {
                case NotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    responseData = new { error = exception.Message };
                    break;

                case BussinessRuleException businessRuleException:
                    statusCode = HttpStatusCode.BadRequest;
                    responseData = JsonSerializer.Serialize(businessRuleException.Message);
                    break;
                case CustomVallidatorException validationException:
                    statusCode = HttpStatusCode.BadRequest;
                    responseData = JsonSerializer.Serialize(validationException.ValidationError,new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping, IncludeFields = true });
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(JsonSerializer.Serialize(responseData,new JsonSerializerOptions() {}));
        }
    }
    public static class ErrorHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
