using Microsoft.EntityFrameworkCore;
using PTP.Core.Domain.Entities;
using PTP.Core.Domain.Objects;
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

        public async Task<IEnumerable<Journey>?> GetPagination(SearchJourneyRequest searchJourneyRequest,TotalCountAndPages totalCountAndPages, int pageNumber,int pageSize ,CancellationToken cancellationToken = default)
        {
            var journeyQuery = GetQueryable();
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
            totalCountAndPages.TotalCount = await journeyQuery.CountAsync();
            var entity = await journeyQuery.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).Include(j => j.Country).Include(j => j.Currency).ToListAsync();
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
            var isJourneyExist = await IsJourneyExist(updatedJourney.Id);
            if (!isJourneyExist)
            {
                throw new JourneyNotFoundException($"Journey with id: {updatedJourney.Id} doesn't exist");
            }

            var entity = await _journeyRepository.GetAsync(updatedJourney.Id);
            MapFromUpdateToEntity(updatedJourney, entity);
            _journeyRepository.Update(entity);
            await _journeyRepository.SaveChangesAsync();
        }
        public async Task RemovePlacesFromJourney(params int[] placeId)
        {
            foreach(var place in placeId)
            {
                var entities = await _journeyRepository.Get().Where(j => j.PlaceId.Contains(place.ToString())).ToListAsync();
                foreach(var entity in entities)
                {
                    var placeString = entity.PlaceId;
                    var placeStringId = placeString.Split(',');
                    var placeIdToRemove = place.ToString();
                    var placeStringAfterRemove = placeStringId.Where(id => id != placeIdToRemove).ToArray();
                    var newPlaceString = string.Join(",", placeStringAfterRemove);
                    entity.PlaceId = newPlaceString;
                }
            }
            await _journeyRepository.SaveChangesAsync();
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

        public IQueryable<Journey> GetQueryable()
        {
            return _journeyRepository.Get();
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
    }
}
