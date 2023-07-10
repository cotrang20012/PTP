using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PTP.Core.Domain.Entities;
using PTP.Core.Domain.Objects;
using PTP.Core.Dtos;
using PTP.Core.Exceptions;
using PTP.Core.Interfaces.Repositories;
using PTP.Core.Interfaces.Services;
using PTP.Validator;

namespace PTP.Services
{
    public class CountryService : ICountryService
    {
        private readonly IRepository<Country> _countryRepository;
        private readonly IMapper _mapper;

        public CountryService(IRepository<Country> countryService, IMapper mapper)
        {
            _mapper = mapper;
            _countryRepository = countryService;
        }

        public async Task InsertNewCountry(UpsertCountryRequestDto upsertCountryRequest, CancellationToken cancellationToken = default)
        {
            var insertValidator = new InsertNewCountryValidator();
            var insertValidationResult = insertValidator.Validate(upsertCountryRequest);
            if(!insertValidationResult.IsValid)
            {
                throw new BadUserInputException(insertValidationResult.Errors[0].ErrorMessage);
            }
            var entity = _mapper.Map<Country>(upsertCountryRequest);
            await _countryRepository.AddAsync(entity);
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

        public async Task<Country?> GetById(int id, CancellationToken cancellationToken = default)
        {
           return await _countryRepository.GetAsync(id, cancellationToken);
        }

        public IQueryable<Country> GetQueryable()
        {
            return _countryRepository.Get();
        }
        public async Task UpdateCountry(UpsertCountryRequestDto upsertCountryRequest, CancellationToken cancellationToken = default)
        {
            var updateValidator = new UpdateCountryValidator();
            var updateValidationResult = updateValidator.Validate(upsertCountryRequest);
            if (!updateValidationResult.IsValid)
            {
                throw new BadUserInputException(updateValidationResult.Errors[0].ErrorMessage);
            }

            var entity = await _countryRepository.GetAsync((int)upsertCountryRequest.Id);
            if (entity == default(Country))
            {
                throw new CountryNotFoundException($"Country with id: {upsertCountryRequest.Id} doesn't exist");
            }
            entity = _mapper.Map<Country>(entity);
            _countryRepository.Update(entity);
            await _countryRepository.SaveChangesAsync();
        }

    }
}
