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

        public JourneyController(IJourneyService journeyService)
        {
            _journeyService = journeyService;
        }

        [HttpPost]
        public async Task<ActionResult<JourneyDto>> GetAllPagination([FromBody]SearchJourneyRequestDto searchJourneyRequest,[FromQuery]int pageNumber = 1, int pageSize = 5)
        {
            TotalCountAndPages totalCountAndPages = new TotalCountAndPages { TotalCount = 0,PageCount = 1 };
            var entities = await _journeyService.GetSearchJourneyPagination(searchJourneyRequest: searchJourneyRequest,totalCountAndPages: totalCountAndPages,pageNumber: pageNumber,pageSize: pageSize);
            var response = _journeyService.CreatePaginationResponse(true, "Get journey list success", entities, "None", StatusCodes.Status200OK, totalCountAndPages);
            return Ok(response);     
        }
        
        [HttpPost]
        [Route("upsert")]
        public async Task<ActionResult> InsertNewJourney([FromBody] UpsertJourneyRequestDto upsertJourneyRequest)
        {
            await _journeyService.InsertNewJourney(upsertJourneyRequest);
            var response = _journeyService.CreateBaseResponse(true, "Insert new journey success", null, "None", StatusCodes.Status200OK);
            return Ok();
        }
        [HttpPut]
        [Route("upsert")]
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
