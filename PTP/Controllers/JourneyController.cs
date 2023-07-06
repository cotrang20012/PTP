using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PTP.Core.Interfaces.Services;
using PTP.Core.Domain.Entities;
using PTP.Dtos;
using Microsoft.Data.SqlClient;
using EntityFramework.Exceptions.Common;
using PTP.Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using PTP.Validator;

namespace PTP.Controllers
{
    [ApiController]
    [Route("api/journey")]
    public class JourneyController : ControllerBase
    {
        private readonly IJourneyService _journeyService;
        private readonly IMapper _mapper;
        public JourneyController(IJourneyService journeyService, IMapper mapper)
        {
            _journeyService = journeyService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<JourneyDto>> GetAll()
        {
            try
            {
                var entity = await _journeyService.GetAll();
                return Ok(_mapper.Map<List<JourneyDto>>(entity));
            }
            catch (SqlException ex)
            {
                return Problem(detail : ex.Message, statusCode : StatusCodes.Status500InternalServerError);
            }      
        }
        
        [HttpPut]
        public async Task<ActionResult> UpsertJourney([FromBody] UpsertJourneyRequest upsertJourneyRequest)
        {
            if(upsertJourneyRequest.Id != null)
            {
                try
                {
                    upsertJourneyRequest.EndDate = upsertJourneyRequest.EndDate.Date;
                    upsertJourneyRequest.StartDate = upsertJourneyRequest.StartDate.Date;
                    var updateValidator = new UpdateJourneyValidator();
                    var updateValidationResult = updateValidator.Validate(upsertJourneyRequest);

                    if (!updateValidationResult.IsValid)
                    {
                        return BadRequest(updateValidationResult.Errors);
                    }
                    Journey newJourney = _mapper.Map<Journey>(upsertJourneyRequest);

                    await _journeyService.UpdateJourney(newJourney);
                    return Ok("Update journey success");
                }
                catch (JourneyNotFoundException)
                {
                    return BadRequest("Journey not found!!!");
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Problem(detail: "Another user is currently update the same journey!!", statusCode: StatusCodes.Status500InternalServerError);
                }
            }
            else
            {
                try
                {
                    upsertJourneyRequest.EndDate = upsertJourneyRequest.EndDate.Date;
                    upsertJourneyRequest.StartDate = upsertJourneyRequest.StartDate.Date;
                    var insertValidator = new AddJourneyValidator();
                    var insertValidationResult = insertValidator.Validate(upsertJourneyRequest);

                    if (!insertValidationResult.IsValid)
                    {
                        return BadRequest(insertValidationResult.Errors);
                    }
                    Journey newJourney = _mapper.Map<Journey>(upsertJourneyRequest);
                    await _journeyService.AddNewJourney(newJourney);
                    return Ok("Insert new journey success");
                }
                catch (CannotInsertNullException)
                {
                    return BadRequest("Please fill all the field!!!");
                }
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteJourney([FromRoute]int id) 
        {
            try
            {
                await _journeyService.DeleteJourney(id);
                return Ok("Delete journey success!!!");
            }
            catch (JourneyNotFoundException)
            { 
                return BadRequest("Journey not found!!!");
            }
            catch (DbUpdateConcurrencyException)
            {
                return Problem(detail: "The journey has already been deleted", statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}
