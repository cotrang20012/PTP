using AutoMapper;
using Azure;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PTP.Core.Domain.Entities;
using PTP.Core.Domain.Objects;
using PTP.Core.Dtos;
using PTP.Core.Exceptions;
using PTP.Core.Interfaces.Repositories;
using PTP.Core.Interfaces.Services;
using PTP.Validator;

namespace PTP.Services
{
    public class PlaceService : IPlaceService
    {
        private readonly IRepository<Place> _placeRepository;
        private readonly IRepository<Journey> _journeyRepository;
        private readonly IMapper _mapper;
        public PlaceService(IRepository<Place> placeRepository, IMapper mapper, IRepository<Journey> journeyRepository)
        {
            _placeRepository = placeRepository;
            _mapper = mapper;
            _journeyRepository = journeyRepository;
        }

        public async Task InsertNewPlace(UpsertPlaceRequestDto newPlace, CancellationToken cancellationToken = default)
        {
            var addValidator = new InsertNewPlaceValidator();
            var addValidationResult = addValidator.Validate(newPlace);
            if (!addValidationResult.IsValid)
            {
                throw new BadUserInputException(addValidationResult.Errors[0].ErrorMessage);
            }
            var entity = _mapper.Map<Place>(newPlace);
            await _placeRepository.AddAsync(entity);
            await _placeRepository.SaveChangesAsync();
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

        public async Task DeletePlace(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _placeRepository.GetAsync(id);
            if (entity == default(Place))
            {
                throw new PlaceNotFoundException($"Place with id: {id} doesn't exist");
            }

            _placeRepository.Delete(entity);
         
            var journeyEntities = await _journeyRepository.Get().Where(j => j.PlaceId.Contains(entity.Id.ToString())).ToListAsync();
            foreach (var journeyEntity in journeyEntities)
            {
                var placeString = journeyEntity.PlaceId;
                var placeArrayString = placeString.Split(',');
                var placeIdToRemove = entity.Id.ToString();
                var placeStringAfterRemove = placeArrayString.Where(id => id != placeIdToRemove).ToArray();
                var newPlaceString = string.Join(",", placeStringAfterRemove);
                journeyEntity.PlaceId = newPlaceString;
            }
 
            await _placeRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Place>?> GetAll(CancellationToken cancellationToken = default)
        {
            var entity = await _placeRepository.Get().ToListAsync();
            return entity;
        }

        public async Task<Place?> GetById(int id, CancellationToken cancellationToken = default)
        {
            return await _placeRepository.GetAsync(id, cancellationToken);
        }

        public IQueryable<Place> GetQueryable()
        {
            return _placeRepository.Get();
        }

        public async Task UpdatePlace(UpsertPlaceRequestDto updatedPlace, CancellationToken cancellationToken = default)
        {
            var updateValidator = new UpdatePlaceValidator();
            var updateValidationResult = updateValidator.Validate(updatedPlace);
            if (!updateValidationResult.IsValid)
            {
                throw new BadUserInputException(updateValidationResult.Errors[0].ErrorMessage);
            }
            var entity = await _placeRepository.GetAsync((int)updatedPlace.Id);
            if (entity == default(Place))
            {
                throw new PlaceNotFoundException($"Place with id: {updatedPlace.Id} doesn't exist");
            }
            entity = _mapper.Map<Place>(entity);
            _placeRepository.Update(entity);
            await _placeRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Place>?> GetAllByCountryId(int countryId, CancellationToken cancellationToken = default)
        {
            var entity = await _placeRepository.Get().Where(p => p.CountryId == countryId).ToListAsync();
            return entity;
        }
    }
}
