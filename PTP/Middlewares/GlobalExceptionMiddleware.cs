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
            catch (PlaceNotFoundException ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 500;

                await context.Response.WriteAsync(new
                {
                    Code = 500,
                    Message = ex.Message
                }.ToString() ?? string.Empty);
            }
            catch (CountryNotFoundException ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 500;

                await context.Response.WriteAsync(new
                {
                    Code = 500,
                    Message = ex.Message
                }.ToString() ?? string.Empty);
            }
            catch (CurrencyNotFoundException ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 500;

                await context.Response.WriteAsync(new
                {
                    Code = 500,
                    Message = ex.Message
                }.ToString() ?? string.Empty);
            }
            catch (JourneyNotFoundException ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 500;

                await context.Response.WriteAsync(new
                {
                    Code = 500,
                    Message = ex.Message
                }.ToString() ?? string.Empty);
            }
            catch (BadUserInputException ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 500;

                await context.Response.WriteAsync(new
                {
                    Code = 500,
                    Message = ex.Message
                }.ToString() ?? string.Empty);
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
        private void LogAndCreateContext(Exception ex, HttpContext context)
        {

        }
    }
}
