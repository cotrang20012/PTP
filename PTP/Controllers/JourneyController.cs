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
using PTP.Core.Domain.Objects;
using Azure;

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
        public async Task<ActionResult<JourneyDto>> GetAllPagination([FromBody]SearchJourneyRequest searchJourneyRequest,[FromQuery]int pageNumber = 1, int pageSize = 5)
        {
            try
            {
                TotalCountAndPages totalCountAndPages = new TotalCountAndPages { TotalCount = 0,PageCount = 1 };
                var entities = await _journeyService.GetPagination(searchJourneyRequest: searchJourneyRequest,totalCountAndPages: totalCountAndPages,pageNumber: pageNumber,pageSize: pageSize);
               
                if(totalCountAndPages.TotalCount > 0)
                {
                    totalCountAndPages.PageCount = Convert.ToInt32(Math.Ceiling((double)totalCountAndPages.TotalCount / (double)pageSize));
                }

                foreach(var entity in entities)
                {
                    var places = await _placeService.GetQueryable().Where(x => entity.PlaceId.Contains(x.Id.ToString())).ToListAsync();
                    entity.Places = places;
                }

                var response = new PaginationResponse<List<JourneyDto>>
                {
                    Success = true,
                    Message = "Get journey list successfully!!!",
                    Data = _mapper.Map<List<JourneyDto>>(entities),
                    ErrorMessage = "None",
                    StatusCode = StatusCodes.Status200OK,
                    TotalPage = totalCountAndPages.PageCount,
                    TotalCount = totalCountAndPages.TotalCount
                };

                return Ok(response);
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
                        var response = new BaseResponse<Object>
                        {
                            Success = false,
                            Message = "Update journey failed!!!",
                            Data = null,
                            ErrorMessage = updateValidationResult.Errors[0].ErrorMessage,
                            StatusCode = StatusCodes.Status400BadRequest
                        };
                        return BadRequest(response);
                    }

                    var currencyExist = await IsCurrencyExist(upsertJourneyRequest.CurrencyId);
                    if (!currencyExist)
                    {
                        var response = new BaseResponse<Object>
                        {
                            Success = false,
                            Message = "Update journey failed!!!",
                            Data = null,
                            ErrorMessage = "Currency doesn't exist, choose another currency",
                            StatusCode = StatusCodes.Status400BadRequest
                        };
                        return BadRequest(response);
                    }

                    var countryExist = await IsCountryExist(upsertJourneyRequest.CountryId);
                    if (!countryExist)
                    {
                        var response = new BaseResponse<Object>
                        {
                            Success = false,
                            Message = "Update journey failed!!!",
                            Data = null,
                            ErrorMessage = "Country doesn't exist, choose another country",
                            StatusCode = StatusCodes.Status400BadRequest
                        };
                        return BadRequest(response);
                    }

                    var placeExist = await ArePlacesExistInCountry(upsertJourneyRequest.CountryId, upsertJourneyRequest.PlaceId);
                    if (!placeExist) 
                    {
                        var response = new BaseResponse<Object>
                        {
                            Success = false,
                            Message = "Update journey failed!!!",
                            Data = null,
                            ErrorMessage = "Country doesn't contain those places, please choose another places",
                            StatusCode = StatusCodes.Status400BadRequest
                        };
                        return BadRequest(response);
                    }

                    Journey newJourney = _mapper.Map<Journey>(upsertJourneyRequest);
                    await _journeyService.UpdateJourney(newJourney);
                    var successResponse = new BaseResponse<Object>
                    {
                        Success = true,
                        Message = "Update journey successfully!!!",
                        Data = null,
                        ErrorMessage = "Update journey success",
                        StatusCode = StatusCodes.Status200OK
                    };
                    return Ok(successResponse);
                }
                catch (JourneyNotFoundException)
                {
                    var response = new BaseResponse<Object>
                    {
                        Success = false,
                        Message = "Update journey failed!!!",
                        Data = null,
                        ErrorMessage = "Journey not found!!!",
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                    return BadRequest(response);
                }
                catch (DbUpdateConcurrencyException)
                {
                    var response = new BaseResponse<Object>
                    {
                        Success = false,
                        Message = "Update journey failed!!!",
                        Data = null,
                        ErrorMessage = "Another user is currently update the same journey!!",
                        StatusCode = StatusCodes.Status400BadRequest,
                    };
                    return BadRequest(response);
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
                        var response = new BaseResponse<Object>
                        {
                            Success = false,
                            Message = "Insert new journey failed!!!",
                            Data = null,
                            ErrorMessage = insertValidationResult.Errors[0].ErrorMessage,
                            StatusCode = StatusCodes.Status400BadRequest
                        };
                        return BadRequest(response);
                    }

                    var currencyExist = await IsCurrencyExist(upsertJourneyRequest.CurrencyId);
                    if (!currencyExist)
                    {
                        var response = new BaseResponse<Object>
                        {
                            Success = false,
                            Message = "Insert new journey failed!!!",
                            Data = null,
                            ErrorMessage = "Currency doesn't exist, choose another currency",
                            StatusCode = StatusCodes.Status400BadRequest
                        };
                        return BadRequest(response);
                    }

                    var countryExist = await IsCountryExist(upsertJourneyRequest.CountryId); 
                    if (!countryExist)
                    {
                        var response = new BaseResponse<Object>
                        {
                            Success = false,
                            Message = "Insert new journey failed!!!",
                            Data = null,
                            ErrorMessage = "Country doesn't exist, choose another country",
                            StatusCode = StatusCodes.Status400BadRequest
                        };
                        return BadRequest(response);
                    }

                    var placeExist = await ArePlacesExistInCountry(upsertJourneyRequest.CountryId, upsertJourneyRequest.PlaceId);
                    if (!placeExist)
                    {
                        var response = new BaseResponse<Object>
                        {
                            Success = false,
                            Message = "Insert new journey failed!!!",
                            Data = null,
                            ErrorMessage = "Country doesn't contain those places, please choose another places",
                            StatusCode = StatusCodes.Status400BadRequest
                        };
                        return BadRequest(response);
                    }

                    Journey newJourney = _mapper.Map<Journey>(upsertJourneyRequest);
                    await _journeyService.AddNewJourney(newJourney);
                    var successResponse = new BaseResponse<Object>
                    {
                        Success = true,
                        Message = "Insert new journey success!!!",
                        Data = null,
                        ErrorMessage = "None",
                        StatusCode = StatusCodes.Status200OK,
                    };
                    return Ok(successResponse);
                }
                catch (CannotInsertNullException)
                {
                    var response = new BaseResponse<Object>
                    {
                        Success = true,
                        Message = "Insert new journey failed!!!",
                        Data = null,
                        ErrorMessage = "Please fill all the required field!!!",
                        StatusCode = StatusCodes.Status400BadRequest,
                    };
                    return BadRequest(response);
                }
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteJourney([FromRoute]int id) 
        {
            try
            {
                await _journeyService.DeleteJourney(id);
                var successResponse = new BaseResponse<Object>
                {
                    Success = true,
                    Message = "Delete journey successfully!!!",
                    Data = null,
                    ErrorMessage = "None",
                    StatusCode = StatusCodes.Status200OK,
                };
                return Ok(successResponse);
            }
            catch (JourneyNotFoundException)
            {
                var response = new BaseResponse<Object>
                {
                    Success = true,
                    Message = "Delete journey failed!!!",
                    Data = null,
                    ErrorMessage = "Journey not found!!!",
                    StatusCode = StatusCodes.Status400BadRequest,
                };
                return BadRequest(response);
            }
            catch (DbUpdateConcurrencyException)
            {
                var response = new BaseResponse<Object>
                {
                    Success = true,
                    Message = "Delete journey failed!!!",
                    Data = null,
                    ErrorMessage = "The journey has already been deleted",
                    StatusCode = StatusCodes.Status400BadRequest,
                };
                return BadRequest(response);
            }
        }

        private async Task<bool> IsCurrencyExist(int currencyId)
        {
            var currency = await _currencyService.GetQueryable().Where(x => currencyId == x.Id).AsNoTracking().FirstOrDefaultAsync();
            if (currency == default(Currency))
            {
                return false;
            }
            return true;
        }

        private async Task<bool> IsCountryExist(int countryId)
        {
            var country = await _countryService.GetQueryable().Where(x => countryId == x.Id).AsNoTracking().FirstOrDefaultAsync();
            if (country == default(Country))
            {
                return false;
            }
            return true;
        }

        private async Task<bool> ArePlacesExistInCountry(int countryId, string places)
        {
            var country = await _countryService.GetQueryable().Where(x => countryId == x.Id).Include(c => c.Places).AsNoTracking().FirstOrDefaultAsync();
            var placeIds = places.Split(",");
            var placeIdsInCountry = country.Places.Select(c => c.Id).ToList();
            foreach (var placeId in placeIds)
            {
                if (!placeIdsInCountry.Any(x => x.ToString() == placeId))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
