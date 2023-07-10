using Microsoft.AspNetCore.Mvc;
using PTP.Core.Interfaces.Services;

namespace PTP.Controllers
{
    [ApiController]
    [Route("api/currency")]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;

        public CurrencyController(ICurrencyService currencyService) 
        {
            _currencyService = currencyService;
        }
        [HttpGet]
        public async Task<ActionResult> GetAllCurrency()
        {
            var entities = await _currencyService.GetAll();
            var response = _currencyService.CreateBaseResponse(true, "Get all currency success", entities, "None", StatusCodes.Status200OK);
            return Ok(response);
        }
    }
}
