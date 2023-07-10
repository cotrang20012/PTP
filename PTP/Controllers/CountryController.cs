using Microsoft.AspNetCore.Mvc;
using PTP.Core.Interfaces.Services;

namespace PTP.Controllers
{
    [ApiController]
    [Route("api/country")]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _countryService;
        public CountryController(ICountryService countryService) 
        {
            _countryService = countryService;
        }
        [HttpGet]
        public async Task<ActionResult> GetAllCountry()
        {
            var entities = await _countryService.GetAll();
            var response = _countryService.CreateBaseResponse(true, "Get all country success", entities, "None", StatusCodes.Status200OK);
            return Ok(response);
        }
    }
}
