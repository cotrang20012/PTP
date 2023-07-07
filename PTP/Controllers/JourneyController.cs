using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PTP.Core.Interfaces.Services;
using PTP.Core.Dtos;
using PTP.Core.Domain.Objects;

namespace PTP.Controllers
{
    [ApiController]
    [Route("api/journey")]
    public class JourneyController : ControllerBase
    {
        private readonly IJourneyService _journeyService;
        private readonly IMapper _mapper;
        private readonly IPlaceService _placeService;
        private readonly ICountryService _countryService;
        private readonly ICurrencyService _currencyService;

        public JourneyController(IJourneyService journeyService, IMapper mapper, IPlaceService placeService, ICountryService countryService, ICurrencyService currencyService)
        {
            _journeyService = journeyService;
            _mapper = mapper;
            _placeService = placeService;
            _countryService = countryService;
            _currencyService = currencyService;
        }

        [HttpPost]
        public async Task<ActionResult<JourneyDto>> GetAllPagination([FromBody]SearchJourneyRequestDto searchJourneyRequest,[FromQuery]int pageNumber = 1, int pageSize = 5)
        {
            TotalCountAndPages totalCountAndPages = new TotalCountAndPages { TotalCount = 0,PageCount = 1 };
            var entities = await _journeyService.GetSearchJourneyPagination(searchJourneyRequest: searchJourneyRequest,totalCountAndPages: totalCountAndPages,pageNumber: pageNumber,pageSize: pageSize);
            var response = _journeyService.CreatePaginationResponse(true, "Get journey lít success", entities, "None", StatusCodes.Status200OK, totalCountAndPages);
            return Ok(response);     
        }
        
        [HttpPost]
        public async Task<ActionResult> InsertnewJourney([FromBody] UpsertJourneyRequestDto upsertJourneyRequest)
        {
            await _journeyService.AddNewJourney(upsertJourneyRequest);
            var response = _journeyService.CreateBaseResponse(true, "Insert new journey success", null, "None", StatusCodes.Status200OK);
            return Ok();
        }
        [HttpPut]
        public async Task<ActionResult>UpdateJourney([FromBody] UpsertJourneyRequestDto updatedJourney)
        {
            await _journeyService.UpdateJourney(updatedJourney);
            var response = _journeyService.CreateBaseResponse(true, "Update journey success", null, "None", StatusCodes.Status200OK);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteJourney([FromRoute]int id) 
        {
            await _journeyService.DeleteJourney(id);
            var response = _journeyService.CreateBaseResponse(true, "Delete journey success", null, "None", StatusCodes.Status200OK);
            return Ok(response);
        }
    }
}
