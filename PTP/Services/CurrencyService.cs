using Microsoft.EntityFrameworkCore;
using PTP.Core.Domain.Entities;
using PTP.Core.Exceptions;
using PTP.Core.Interfaces.Repositories;
using PTP.Core.Interfaces.Services;

namespace PTP.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly IRepository<Currency> _currencyRepository;

        public CurrencyService(IRepository<Currency> currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public async Task AddNewCurrency(Currency newCurrency, CancellationToken cancellationToken = default)
        {
            await _currencyRepository.AddAsync(newCurrency);
            await _currencyRepository.SaveChangesAsync();
        }

        public async Task DeleteCurrency(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _currencyRepository.GetAsync(id);
            if (entity == default(Currency))
            {
                throw new CurrencyNotFoundException($"Currency with id: {id} doesn't exist");
            }
 
            _currencyRepository.Delete(entity);
            await _currencyRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Currency>?> GetAll(CancellationToken cancellationToken = default)
        {
            var entity = await _currencyRepository.Get().ToListAsync();
            return entity;
        }

        public async Task<Currency?> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _currencyRepository.GetAsync(id, cancellationToken);
        }

        public IQueryable<Currency> GetQueryable()
        {
            return _currencyRepository.Get();
        }

        public async Task UpdateCurrency(Currency updatedCurrency, CancellationToken cancellationToken = default)
        {
            var entity = await _currencyRepository.GetAsync(updatedCurrency.Id);
            if (entity == default(Currency))
            {
                throw new CurrencyNotFoundException($"Currency with id: {updatedCurrency.Id} doesn't exist");
            }

            MapFromUpdateToEntity(updatedCurrency, entity);
            await _currencyRepository.SaveChangesAsync();
        }

        private void MapFromUpdateToEntity(Currency updatedCurrency, Currency entity)
        {
            entity.Name = updatedCurrency.Name;
            entity.Version = updatedCurrency.Version;
        }
    }
}
