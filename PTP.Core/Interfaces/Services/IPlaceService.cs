using PTP.Core.Domain.Entities;
using PTP.Core.Domain.Objects;
using PTP.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTP.Core.Interfaces.Services
{
    public interface IPlaceService
    {
        Task<Place?> GetById(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Place>?> GetAll(CancellationToken cancellationToken = default);
        Task InsertNewPlace(UpsertPlaceRequestDto newPlace, CancellationToken cancellationToken = default);
        Task DeletePlace(int id, CancellationToken cancellationToken = default);
        Task UpdatePlace(UpsertPlaceRequestDto updatedPlace, CancellationToken cancellationToken = default);
        BaseResponse CreateBaseResponse(bool responseState, string responseMessage, Object responseData, string respsoneErrorMessage, int responseStatusCode);
        IQueryable<Place> GetQueryable();
    }
}
