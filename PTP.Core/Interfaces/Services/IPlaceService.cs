using PTP.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTP.Core.Interfaces.Services
{
    public interface IPlaceService
    {
        Task<Place?> GetAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Place>?> GetAll(CancellationToken cancellationToken = default);
        Task AddNewPlace(Place newPlace, CancellationToken cancellationToken = default);
        Task DeletePlace(int id, CancellationToken cancellationToken = default);
        Task UpdatePlace(Place updatedPlace, CancellationToken cancellationToken = default);
    }
}
