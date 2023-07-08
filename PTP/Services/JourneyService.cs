using Microsoft.EntityFrameworkCore;
using PTP.Core.Domain.Entities;
using PTP.Core.Domain.Objects;
using PTP.Core.Exceptions;
using PTP.Core.Interfaces.Repositories;
using PTP.Core.Interfaces.Services;
using PTP.Core.Dtos;
using PTP.Validator;
using AutoMapper;

namespace PTP.Services
{
    public class JourneyService : IJourneyService
    {
        private readonly IRepository<Journey> _journeyRepository;
        private readonly IRepository<Country> _countryRepository;
        private readonly IRepository<Currency> _currencyRepository;
        private readonly IRepository<Place>  _placeRepository;
        private readonly IMapper _mapper;
        public JourneyService(IRepository<Journey> journeyrepository, IRepository<Country> countryRepository, IRepository<Currency> currencyRepository, IRepository<Place> placeRepository, IMapper mapper)
        {
            _journeyRepository = journeyrepository;
            _currencyRepository = currencyRepository;
            _placeRepository = placeRepository;
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        public async Task<Journey?> GetAsync(int id, CancellationToken cancellationToken)
        {
            var entity = await _journeyRepository.GetAsync(id, cancellationToken);
            return entity;
        }

        public async Task<IEnumerable<Journey>?> GetSearchJourneyPagination(SearchJourneyRequestDto searchJourneyRequest,TotalCountAndPages totalCountAndPages, int pageNumber,int pageSize ,CancellationToken cancellationToken = default)
        {
            var journeyQuery = _journeyRepository.Get();
            if (searchJourneyRequest.NameAndDescription != String.Empty)
            {
                journeyQuery = journeyQuery.Where(x => x.Name.Contains(searchJourneyRequest.NameAndDescription) || x.Description.Contains(searchJourneyRequest.NameAndDescription));
            }
            if (searchJourneyRequest.CurrencyId != null && searchJourneyRequest.CurrencyId > 0)
            {
                journeyQuery = journeyQuery.Where(x => x.CurrencyId == searchJourneyRequest.CurrencyId);
            }
            if (searchJourneyRequest.CountryId != null && searchJourneyRequest.CountryId > 0)
            {
                journeyQuery = journeyQuery.Where(x => x.CountryId == searchJourneyRequest.CountryId);
            }
            if (searchJourneyRequest.FromAmount != null && searchJourneyRequest.FromAmount > 0)
            {
                journeyQuery = journeyQuery.Where(x => x.Amount >= searchJourneyRequest.FromAmount);
            }
            if (searchJourneyRequest.PlaceId != String.Empty)
            {
                journeyQuery = journeyQuery.Where(x => x.PlaceId.Contains(searchJourneyRequest.PlaceId));
            }
            if (searchJourneyRequest.ToAmount != null && searchJourneyRequest.ToAmount > 0 && searchJourneyRequest.ToAmount > searchJourneyRequest.FromAmount)
            {
                journeyQuery = journeyQuery.Where(x => x.Amount <= searchJourneyRequest.ToAmount);
            }
            if (searchJourneyRequest.FromStartDate != null)
            {
                journeyQuery = journeyQuery.Where(x => x.StartDate >= searchJourneyRequest.FromStartDate);
            }
            if (searchJourneyRequest.ToStartDate != null && searchJourneyRequest.ToStartDate >= searchJourneyRequest.FromStartDate)
            {
                journeyQuery = journeyQuery.Where(x => x.StartDate <= searchJourneyRequest.ToStartDate);
            }
            if (searchJourneyRequest.FromEndDate != null)
            {
                journeyQuery = journeyQuery.Where(x => x.EndDate >= searchJourneyRequest.FromEndDate);
            }
            if (searchJourneyRequest.ToEndDate != null && searchJourneyRequest.ToEndDate >= searchJourneyRequest.FromEndDate)
            {
                journeyQuery = journeyQuery.Where(x => x.EndDate <= searchJourneyRequest.ToEndDate);
            }
            if (searchJourneyRequest.PlaceId != String.Empty)
            {
                journeyQuery = journeyQuery.Where(x => x.Status.Contains(searchJourneyRequest.Status));
            }

            totalCountAndPages.TotalCount = await journeyQuery.CountAsync();
            if (totalCountAndPages.TotalCount > 0)
            {
                totalCountAndPages.PageCount = Convert.ToInt32(Math.Ceiling((double)totalCountAndPages.TotalCount / (double)pageSize));
            }

            var entities = await journeyQuery.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).Include(j => j.Country).Include(j => j.Currency).ToListAsync();
            foreach (var entity in entities)
            {
                var places = await _placeRepository.Get().Where(x => entity.PlaceId.Contains(x.Id.ToString())).ToListAsync();
                entity.Places = places;
            }
            return entities;
        }

        public async Task InsertNewJourney(UpsertJourneyRequestDto newJourney, CancellationToken cancellationToken = default)
        {
            newJourney.EndDate = newJourney.EndDate.Date;
            newJourney.StartDate = newJourney.StartDate.Date;
            var insertValidator = new AddJourneyValidator();
            var insertValidationResult = insertValidator.Validate(newJourney);
            if (!insertValidationResult.IsValid)
            {
                throw new BadUserInputException(insertValidationResult.Errors[0].ErrorMessage);
            }
            await ValidateUpsertJourneyRequest(newJourney);
            var entity = _mapper.Map<Journey>(newJourney);
            await _journeyRepository.AddAsync(entity);
            await _journeyRepository.SaveChangesAsync();
        }

        public async Task DeleteJourney(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _journeyRepository.GetAsync(id);
            if (entity == default(Journey))
            {
                throw new JourneyNotFoundException($"Journey with id: {id} doesn't exist");
            }

            _journeyRepository.Delete(entity);
            await _journeyRepository.SaveChangesAsync();
        }

        public async Task UpdateJourney(UpsertJourneyRequestDto updatedJourney, CancellationToken cancellationToken = default)
        {
            updatedJourney.EndDate = updatedJourney.EndDate.Date;
            updatedJourney.StartDate = updatedJourney.StartDate.Date;
            var updateValidator = new UpdateJourneyValidator();
            var updateValidationResult = updateValidator.Validate(updatedJourney);          
            if (!updateValidationResult.IsValid)
            {
                throw new BadUserInputException(updateValidationResult.Errors[0].ErrorMessage);
            }
            var isJourneyExist = await IsJourneyExist((int)updatedJourney.Id);
            if (!isJourneyExist)
            {
                throw new JourneyNotFoundException($"Journey with name: {updatedJourney.Name} doesn't exist");
            }
            await ValidateUpsertJourneyRequest(updatedJourney);
            var entity = await _journeyRepository.GetAsync((int)updatedJourney.Id);
            entity = _mapper.Map<Journey>(updatedJourney);
            _journeyRepository.Update(entity);
            await _journeyRepository.SaveChangesAsync();
        }

        public async Task<bool> IsJourneyExist(int id)
        {
            var entity = await _journeyRepository.GetAsyncNoTracking(id);
            if (entity == default(Journey))
            {
                return false;
            }
            return true;
        }

        public async Task ValidateUpsertJourneyRequest(UpsertJourneyRequestDto upsertJourneyRequestDto)
        {
            var isCurrencyExist = await IsCurrencyExist(upsertJourneyRequestDto.CurrencyId);
            if (!isCurrencyExist)
            {
                throw new CurrencyNotFoundException($"Currency with name: {upsertJourneyRequestDto.CurrencyName} doesn't exist");
            }
            var isCountryExist = await IsCountryExist(upsertJourneyRequestDto.CountryId);
            if (!isCountryExist)
            {
                throw new CountryNotFoundException($"Country with name: {upsertJourneyRequestDto.CountryName} doesn't exist");
            }
            var placeExist = await ArePlacesExistInCountry(upsertJourneyRequestDto.CountryId, upsertJourneyRequestDto.PlaceId);
            if (!placeExist)
            {
                throw new PlaceNotFoundException($"Place with name: {upsertJourneyRequestDto.PlaceName} doesn't exist with Country {upsertJourneyRequestDto.CountryName}");
            }
        }
        public async Task<bool> IsCurrencyExist(int currencyId, CancellationToken cancellationToken = default)
        {
            var currency = await _currencyRepository.Get().Where(x => currencyId == x.Id).AsNoTracking().FirstOrDefaultAsync();
            if (currency == default(Currency))
            {
                return false;
            }
            return true;
        }

        public async Task<bool> ArePlacesExistInCountry(int countryId, string places, CancellationToken cancellationToken = default)
        {
            var country = await _countryRepository.Get().Where(x => countryId == x.Id).Include(c => c.Places).AsNoTracking().FirstOrDefaultAsync();
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

        public async Task<bool> IsCountryExist(int countryId, CancellationToken cancellationToken = default)
        {
            var country = await _countryRepository.Get().Where(x => countryId == x.Id).AsNoTracking().FirstOrDefaultAsync();
            if (country == default(Country))
            {
                return false;
            }
            return true;
        }

        public BaseResponse CreateBaseResponse(bool responseState, string responseMessage, object responseData, string respsoneErrorMessage, int responseStatusCode)
        {
            var response = new BaseResponse()
            {
                Success = responseState,
                Message = responseMessage,
                Data = responseData,
                ErrorMessage = respsoneErrorMessage,
                StatusCode = responseStatusCode
            };
            return response;
        }

        public PaginationResponse CreatePaginationResponse(bool responseState, string responseMessage, object responseData, string respsoneErrorMessage, int responseStatusCode, TotalCountAndPages totalCountAndPages)
        {
            var response = new PaginationResponse()
            {
                Success = responseState,
                Message = responseMessage,
                Data = responseData,
                ErrorMessage = respsoneErrorMessage,
                StatusCode = responseStatusCode,
                TotalPage = totalCountAndPages.PageCount,
                TotalCount = totalCountAndPages.TotalCount               
            };
            return response;
        }
    }
}
