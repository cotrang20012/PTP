using PTP.Core.Domain.Entities;
using PTP.Core.Domain.Objects;

 
namespace PTP.Core.Interfaces.Services
{
    public interface IJourneyService
    {
        Task<Journey?> GetAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Journey>?> GetPagination(SearchJourneyRequest searchJourneyRequest,int pageNumber, int pageSize = 5,CancellationToken cancellationToken = default);
        Task AddNewJourney(Journey newJourney, CancellationToken cancellationToken = default);
        Task DeleteJourney(int id, CancellationToken cancellationToken = default);
        Task UpdateJourney(Journey updatedJourney, CancellationToken cancellationToken = default);
        Task RemovePlacesFromJourney(params int[] placeId);
        IQueryable<Journey> GetQueryable();
        Task<int> CountAllJourney();
        Task<IEnumerable<Journey>?> Search(SearchJourneyRequest searchJourneyRequest, CancellationToken cancellationToken = default);
    }
}
