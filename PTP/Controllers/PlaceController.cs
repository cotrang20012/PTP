using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PTP.Core.Exceptions;
using PTP.Core.Interfaces.Services;
using PTP.Services;

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
        public async Task<ActionResult> DeleteController([FromRoute]int id)
        {
            try
            {
                await _placeService.DeletePlace(id);
                return Ok("Delete place success!!!");
            }
            catch (PlaceNotFoundException)
            {
                return BadRequest("Place not found!!!");
            }
        }
    }
}
