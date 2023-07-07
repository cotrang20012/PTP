using Microsoft.EntityFrameworkCore;
using PTP.Core.Domain.Entities;
using PTP.Core.Domain.Objects;
using PTP.Core.Exceptions;
using PTP.Core.Interfaces.Repositories;
using PTP.Core.Interfaces.Services;

namespace PTP.Services
{
    public class CountryService : ICountryService
    {
        private readonly IRepository<Country> _countryRepository;

        public CountryService(IRepository<Country> countryService)
        {
            _countryRepository = countryService;
        }

        public async Task AddNewCountry(Country newCountry, CancellationToken cancellationToken = default)
        {
            await _countryRepository.AddAsync(newCountry);
            await _countryRepository.SaveChangesAsync();
        }

        public BaseResponse CreateBaseResponse(bool responseState, string responseMessage, object responseData, string respsoneErrorMessage, int responseStatusCode)
        {
            var response = new BaseResponse()
            {
                Success = responseState,
                Message = responseMessage,
                Data = responseData,
                ErrorMessage = respsoneErrorMessage,
                StatusCode = responseStatusCode
            };
            return response;
        }

        public async Task DeleteCountry(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _countryRepository.GetAsync(id);
            if (entity == default(Country))
            {
                throw new CountryNotFoundException($"Country with id: {id} doesn't exist");
            }
            _countryRepository.Delete(entity);
            await _countryRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Country>?> GetAll(CancellationToken cancellationToken = default)
        {
            var entity = await _countryRepository.Get().ToListAsync();
            return entity;
        }

        public async Task<Country?> GetAsync(int id, CancellationToken cancellationToken = default)
        {
           return await _countryRepository.GetAsync(id, cancellationToken);
        }

        public IQueryable<Country> GetQueryable()
        {
            return _countryRepository.Get();
        }
        public async Task UpdateCountry(Country updatedCountry, CancellationToken cancellationToken = default)
        {
            var entity = await _countryRepository.GetAsync(updatedCountry.Id);
            if (entity == default(Country))
            {
                throw new CountryNotFoundException($"Journey with id: {updatedCountry.Id} doesn't exist");
            }
            //await Task.Delay(3000);
            MapFromUpdateToEntity(updatedCountry, entity);
            await _countryRepository.SaveChangesAsync();
        }

        private void MapFromUpdateToEntity(Country updatedCountry, Country entity)
        {
            entity.Name = updatedCountry.Name;
            entity.Version = updatedCountry.Version;
        }
    }
}
