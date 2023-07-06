using PTP.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTP.Core.Interfaces.Services
{
    public interface IJourneyService
    {
        Task<Journey?> GetAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Journey>?> GetAll(CancellationToken cancellationToken = default);
        Task AddNewJourney(Journey newJourney, CancellationToken cancellationToken = default);
        Task DeleteJourney(int id, CancellationToken cancellationToken = default);
        Task UpdateJourney(Journey updatedJourney, CancellationToken cancellationToken = default);
        Task TestFunction();
    }
}
