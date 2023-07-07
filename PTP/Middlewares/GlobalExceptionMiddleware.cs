using PTP.Core.Exceptions;

namespace PTP.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected exception occure");
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 500;

                await context.Response.WriteAsync(new
                {
                    Code = 500,
                    Message = ex.Message
                }.ToString() ?? string.Empty);
            }
        }
        private async Task LogAndCreateContext(Exception ex, HttpContext context)
        {
            context.Response.StatusCode = 500;
            if (ex is JourneyNotFoundException || ex is CountryNotFoundException 
                || ex is PlaceNotFoundException || ex is CurrencyNotFoundException || ex is BadUserInputException)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode = 400;
            }
            else
            {
                _logger.LogError(ex, "Unexpected exception occure");
            }
       
            await context.Response.WriteAsync(new
            {
                Code = context.Response.StatusCode,
                Message = ex.Message
            }.ToString() ?? string.Empty);
        }
    }
}
