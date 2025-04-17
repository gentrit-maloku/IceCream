using IceSync.Enums;
using IceSync.Middlewares.Custom_Exceptions;
using System.Net;
using System.Text.Json;

namespace IceSync.Middlewares
{
    public class ExceptionMiddleware(RequestDelegate _next, ILogger<ExceptionMiddleware> _logger)
    {
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred (Path: {Path}, Method: {Method}): {Message}",
                    httpContext.Request.Path,
                    httpContext.Request.Method,
                    ex.Message);

                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var errorModel = new ErrorModel
            {
                Message = exception.Message
            };

            switch (exception)
            {
                case BadArgumentException e:
                    errorModel.ErrorCode = e.Code.ToString();
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                case ConflictException e:
                    errorModel.ErrorCode = e.Code.ToString();
                    context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                    break;

                case AccessForbiddenException e:
                    errorModel.ErrorCode = e.Code.ToString();
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    break;

                case ItemNotFoundException e:
                    errorModel.ErrorCode = e.Code.ToString();
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                default:
                    errorModel.ErrorCode = ErrorCode.InternalServerError.ToString();
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            return context.Response.WriteAsync(JsonSerializer.Serialize(errorModel));
        }
    }

    public class ErrorModel
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
    }
}
