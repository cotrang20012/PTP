using PTP.Core.Domain.Entities;
using PTP.Core.Domain.Objects;
using PTP.Core.Dtos;
 
namespace PTP.Core.Interfaces.Services
{
    public interface IJourneyService
    {
        Task<Journey?> GetAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Journey>?> GetSearchJourneyPagination(SearchJourneyRequestDto searchJourneyRequest,TotalCountAndPages totalCountAndPages,int pageNumber, int pageSize = 5,CancellationToken cancellationToken = default);
        Task AddNewJourney(UpsertJourneyRequestDto newJourney, CancellationToken cancellationToken = default);
        Task DeleteJourney(int id, CancellationToken cancellationToken = default);
        Task UpdateJourney(UpsertJourneyRequestDto updatedJourney, CancellationToken cancellationToken = default);
        Task RemovePlacesFromJourney(params int[] placeId);
        Task<bool> IsJourneyExist(int id);
        BaseResponse CreateBaseResponse(bool responseState, string responseMessage,Object responseData, string respsoneErrorMessage, int responseStatusCode);
        Task<bool> ArePlacesExistInCountry(int countryId, string places, CancellationToken cancellationToken = default);
        Task<bool> IsCurrencyExist(int currencyId, CancellationToken cancellationToken = default);
        Task<bool> IsCountryExist(int countryId, CancellationToken cancellationToken = default);
        PaginationResponse CreatePaginationResponse(bool responseState, string responseMessage, Object responseData, string respsoneErrorMessage, int responseStatusCode, TotalCountAndPages totalCountAndPages);
    }
}
