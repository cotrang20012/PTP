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
    public interface ICountryService
    {
        Task<Country?> GetById(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Country>?> GetAll(CancellationToken cancellationToken = default);
        Task InsertNewCountry(UpsertCountryRequestDto upsertCountryRequest, CancellationToken cancellationToken = default);
        Task DeleteCountry(int id, CancellationToken cancellationToken = default);
        Task UpdateCountry(UpsertCountryRequestDto upsertCountryRequest, CancellationToken cancellationToken = default);
        BaseResponse CreateBaseResponse(bool responseState, string responseMessage, Object responseData, string respsoneErrorMessage, int responseStatusCode);
        IQueryable<Country> GetQueryable();
    }
}
