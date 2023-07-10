using Microsoft.EntityFrameworkCore;
using PTP.Core.Domain.Objects;
using PTP.Core.Exceptions;
using System.Text.Json;

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
                if (ex is DbUpdateConcurrencyException)
                {
                    _logger.LogError(ex, ex.Message);
                    context.Response.StatusCode = StatusCodes.Status409Conflict;
                }
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

                var newResponse = new BaseResponse() 
                {
                    StatusCode = context.Response.StatusCode,
                    Message = String.Empty,
                    ErrorMessage = ex.Message,
                    Data = null,
                    Success = false
                };

                var responseJson = JsonSerializer.Serialize(newResponse);

                await context.Response.WriteAsync(responseJson);
            }
        }
    }
}
