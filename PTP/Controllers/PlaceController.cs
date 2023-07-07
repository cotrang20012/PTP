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
        private readonly IMapper _mapper;

        public PlaceController(IPlaceService placeService, IMapper mapper)
        {
            _placeService = placeService;
            _mapper = mapper;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePlace([FromRoute]int id)
        {
            await _placeService.DeletePlace(id);
            var response = _placeService.CreateBaseResponse(true, "Delete place success", null, "None", StatusCodes.Status200OK);
            return Ok(response);
        }
        [HttpPut]
        public async Task<ActionResult> AddPlace([FromBody]UpsertPlaceRequestDto upsertPlaceRequest)
        {
            await _placeService.AddNewPlace(upsertPlaceRequest);
            var response = _placeService.CreateBaseResponse(true, "Insert new place succes", null, "None", StatusCodes.Status200OK);
            return Ok(response);
        }
    }
}
