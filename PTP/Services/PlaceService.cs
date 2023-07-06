using Microsoft.EntityFrameworkCore;
using PTP.Core.Domain.Entities;
using PTP.Core.Exceptions;
using PTP.Core.Interfaces.Repositories;
using PTP.Core.Interfaces.Services;

namespace PTP.Services
{
    public class PlaceService : IPlaceService
    {
        private readonly IRepository<Place> _placeRepository;
        private readonly IJourneyService _journeyService;

        public PlaceService(IRepository<Place> placeRepository, IJourneyService journeyService)
        {
            _placeRepository = placeRepository;
            _journeyService = journeyService;
        }

        public async Task AddNewPlace(Place newPlace, CancellationToken cancellationToken = default)
        {
            await _placeRepository.AddAsync(newPlace);
            await _placeRepository.SaveChangesAsync();
        }

        public async Task DeletePlace(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _placeRepository.GetAsync(id);
            if (entity == default(Place))
            {
                throw new PlaceNotFoundException($"Place with id: {id} doesn't exist");
            }

            _placeRepository.Delete(entity);
            await _journeyService.RemovePlacesFromJourney(entity.Id);
            await _placeRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Place>?> GetAll(CancellationToken cancellationToken = default)
        {
            var entity = await _placeRepository.Get().ToListAsync();
            return entity;
        }

        public async Task<Place?> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _placeRepository.GetAsync(id, cancellationToken);
        }

        public IQueryable<Place> GetQueryable()
        {
            return _placeRepository.Get();
        }

        public async Task UpdatePlace(Place updatedPlace, CancellationToken cancellationToken = default)
        {
            var entity = await _placeRepository.GetAsync(updatedPlace.Id);
            if (entity == default(Place))
            {
                throw new PlaceNotFoundException($"Place with id: {updatedPlace.Id} doesn't exist");
            }

            MapFromUpdateToEntity(updatedPlace, entity);
            _placeRepository.Update(entity);
            await _placeRepository.SaveChangesAsync();
        }

        private void MapFromUpdateToEntity(Place updatedPlace, Place entity)
        {
            entity.Name = updatedPlace.Name;
            entity.CountryId = updatedPlace.CountryId;
            entity.Version = updatedPlace.Version;
        }
    }
}
