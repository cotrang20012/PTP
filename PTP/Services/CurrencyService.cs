using AutoMapper;
using FluentValidation;
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
    public class CurrencyService : ICurrencyService
    {
        private readonly IRepository<Currency> _currencyRepository;
        private readonly IMapper _mapper;

        public CurrencyService(IRepository<Currency> currencyRepository, IMapper mapper)
        {
            _currencyRepository = currencyRepository;
            _mapper = mapper;
        }

        public async Task InsertNewCurrency(UpsertCurrencyRequestDto upsertCurrencyRequest, CancellationToken cancellationToken = default)
        {
            var insertValidator = new InsertNewCurrencyValidator();
            var insertValidationResult = insertValidator.Validate(upsertCurrencyRequest);
            if (!insertValidationResult.IsValid)
            {
                throw new BadUserInputException(insertValidationResult.Errors[0].ErrorMessage);
            }
            var entity = _mapper.Map<Currency>(upsertCurrencyRequest);
            await _currencyRepository.AddAsync(entity);
            await _currencyRepository.SaveChangesAsync();
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

        public async Task<Currency?> GetById(int id, CancellationToken cancellationToken = default)
        {
            return await _currencyRepository.GetAsync(id, cancellationToken);
        }

        public IQueryable<Currency> GetQueryable()
        {
            return _currencyRepository.Get();
        }

        public async Task UpdateCurrency(UpsertCurrencyRequestDto upsertCurrencyRequest, CancellationToken cancellationToken = default)
        {
            var updateValidator = new UpdateCurrencyValidator();
            var updateValidationResult = updateValidator.Validate(upsertCurrencyRequest);
            if (!updateValidationResult.IsValid)
            {
                throw new BadUserInputException(updateValidationResult.Errors[0].ErrorMessage);
            }

            var entity = await _currencyRepository.GetAsync((int)upsertCurrencyRequest.Id);
            if (entity == default(Currency))
            {
                throw new CurrencyNotFoundException($"Currency with id: {upsertCurrencyRequest.Id} doesn't exist");
            }

            entity = _mapper.Map<Currency>(entity);
            _currencyRepository.Update(entity);
            await _currencyRepository.SaveChangesAsync();
        }
    }
}
