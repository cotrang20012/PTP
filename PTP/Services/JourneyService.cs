using Microsoft.EntityFrameworkCore;
using PTP.Core.Domain.Entities;
using PTP.Core.Exceptions;
using PTP.Core.Interfaces.Repositories;
using PTP.Core.Interfaces.Services;



namespace PTP.Services
{
    public class JourneyService : IJourneyService
    {
        private readonly IRepository<Journey> _journeyRepository;
        public JourneyService(IRepository<Journey> journeyrepository) 
        {
            _journeyRepository = journeyrepository;
        }       

        public async Task<Journey?> GetAsync(int id, CancellationToken cancellationToken)
        {
            var entity = await _journeyRepository.GetAsync(id, cancellationToken);
            return entity;
        }

        public async Task<IEnumerable<Journey>?> GetAll(CancellationToken cancellationToken)
        {
            var entity = await _journeyRepository.Get().Include(j => j.Currency).ToListAsync();
            return entity;
        }

        public async Task AddNewJourney(Journey newJourney, CancellationToken cancellationToken = default)
        {           
            await _journeyRepository.AddAsync(newJourney);
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

        public async Task UpdateJourney(Journey updatedJourney, CancellationToken cancellationToken = default)
        {
            var entity = await _journeyRepository.GetAsync(updatedJourney.Id);
            if (entity == default(Journey))
            {
               throw new JourneyNotFoundException($"Journey with id: {updatedJourney.Id} doesn't exist");
            }

            MapFromUpdateToEntity(updatedJourney, entity);
            _journeyRepository.Update(entity);
            await _journeyRepository.SaveChangesAsync();
        }

        public async Task TestFunction()
        {
            var entity = await _journeyRepository.GetAsync(2);
            entity.Amount = 5;
        }

        private void MapFromUpdateToEntity(Journey updatedJourney, Journey entity)
        {
            entity.Name = updatedJourney.Name;
            entity.Description = updatedJourney.Description;
            entity.CountryId = updatedJourney.CountryId;
            entity.PlaceId = updatedJourney.PlaceId;
            entity.CurrencyId = updatedJourney.CurrencyId;
            entity.Amount = updatedJourney.Amount;
            entity.Status = updatedJourney.Status;
            entity.StartDate = updatedJourney.StartDate.Date;
            entity.EndDate = updatedJourney.EndDate.Date;
            entity.Version = updatedJourney.Version;
        }
    }
}
