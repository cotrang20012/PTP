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
    public interface ICurrencyService
    {
        Task<Currency?> GetById(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Currency>?> GetAll(CancellationToken cancellationToken = default);
        Task InsertNewCurrency(UpsertCurrencyRequestDto upsertCurrencyRequest, CancellationToken cancellationToken = default);
        Task DeleteCurrency(int id, CancellationToken cancellationToken = default);
        Task UpdateCurrency(UpsertCurrencyRequestDto upsertCurrencyRequest, CancellationToken cancellationToken = default);
        BaseResponse CreateBaseResponse(bool responseState, string responseMessage, Object responseData, string respsoneErrorMessage, int responseStatusCode);
        IQueryable<Currency> GetQueryable();
    }
}
