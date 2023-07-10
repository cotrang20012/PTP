using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PTP.Core.Interfaces.Services;
using PTP.Core.Dtos;

namespace PTP.Controllers
{
    [ApiController]
    [Route("api/place")]
    public class PlaceController : ControllerBase
    {
        private readonly IPlaceService _placeService;

        public PlaceController(IPlaceService placeService)
        {
            _placeService = placeService;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePlace([FromRoute]int id)
        {
            await _placeService.DeletePlace(id);
            var response = _placeService.CreateBaseResponse(true, "Delete place success", null, "None", StatusCodes.Status200OK);
            return Ok(response);
        }
        [HttpPut]
        public async Task<ActionResult> InsertNewPlace([FromBody]UpsertPlaceRequestDto upsertPlaceRequest)
        {
            await _placeService.InsertNewPlace(upsertPlaceRequest);
            var response = _placeService.CreateBaseResponse(true, "Insert new place success", null, "None", StatusCodes.Status200OK);
            return Ok(response);
        }
        [HttpGet("{countryId}")]
        public async Task<ActionResult> GetPlaceByCountryId([FromRoute]int countryId)
        {
            var entites = await _placeService.GetAllByCountryId(countryId);
            var response = _placeService.CreateBaseResponse(true, "Get all place by country id success", entites, "None", StatusCodes.Status200OK);
            return Ok(response);
        }
    }
}
