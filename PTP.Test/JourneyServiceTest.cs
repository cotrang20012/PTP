using Microsoft.EntityFrameworkCore;
using PTP.Core.Domain.Entities;
using PTP.Core.Domain.Enums;
using PTP.Core.Dtos;
using PTP.Test.Services;
using PTP.Validator;
using System.Text;

namespace PTP.Test
{
    [TestFixture]
    public class JourneyServiceTest : TestService
    {
        [Test]
        public void InsertNewJourneyValidatorTest_ValidRequest()
        {
            UpsertJourneyRequestDto upsertJourneyRequest = new UpsertJourneyRequestDto() { Name = "Company Trip", Description = "A trip with company at ...", CountryId = 2, CountryName = "Viet Nam", PlaceId = "3", PlaceName = "Đà Nẵng", CurrencyId = 3, CurrencyName = "VND", Amount = 5000000, Status = JourneyStatus.Planning.ToString(), StartDate = DateTime.Now.Date, EndDate = DateTime.Now.AddDays(5).Date, Days = 5, Nights = 4 };
            upsertJourneyRequest.EndDate = upsertJourneyRequest.EndDate.Date;
            upsertJourneyRequest.StartDate = upsertJourneyRequest.StartDate.Date;
            var insertValidator = new InsertNewJourneyValidator();

            var insertValidationResult = insertValidator.Validate(upsertJourneyRequest);

            Assert.IsTrue(insertValidationResult.IsValid);
        }
        [Test]
        public void InsertNewJourneyValidatorTest_InvalidRequest()
        {
            UpsertJourneyRequestDto upsertJourneyRequest = new UpsertJourneyRequestDto() { Description = "Short trip", CountryId = 2, PlaceId = "3", Amount = 5000000, Status = JourneyStatus.Planning.ToString(), StartDate = DateTime.Now.Date.AddDays(5).Date, EndDate = DateTime.Now, Days = 5, Nights = 4 };
            upsertJourneyRequest.EndDate = upsertJourneyRequest.EndDate.Date;
            upsertJourneyRequest.StartDate = upsertJourneyRequest.StartDate.Date;
            var insertValidator = new InsertNewJourneyValidator();

            var insertValidationResult = insertValidator.Validate(upsertJourneyRequest);

            Assert.IsFalse(insertValidationResult.IsValid);
        }
        [Test]
        public async Task InsertNewJourneyTest_Success()
        {
            UpsertJourneyRequestDto upsertJourneyRequest = new UpsertJourneyRequestDto() { Name = "Company Trip", Description = "A trip with company at ...", CountryId = 2, PlaceId = "3", CurrencyId = 3, Amount = 5000000, Status = JourneyStatus.Planning.ToString(), StartDate = DateTime.Now.Date, EndDate = DateTime.Now.AddDays(5).Date, Days = 5, Nights = 4 };
            upsertJourneyRequest.EndDate = upsertJourneyRequest.EndDate.Date;
            upsertJourneyRequest.StartDate = upsertJourneyRequest.StartDate.Date;
            var entity = _mapper.Map<Journey>(upsertJourneyRequest);

            await _journeyRepository.AddAsync(entity);
            await _journeyRepository.SaveChangesAsync();
            _context.DbContext.ChangeTracker.Clear();
            var listJourneys = await _journeyRepository.GetAllAsyncNoTracking();

            Assert.IsNotNull(listJourneys);
            Assert.AreEqual(7, listJourneys.Count());
        }

        [Test]
        public async Task DeleteJourneyTest_JourneyExist()
        {
            var journeyId = 1;
            var journeyEntity = await _journeyRepository.GetAsync(journeyId);
            _journeyRepository.Delete(journeyEntity);
            await _journeyRepository.SaveChangesAsync();
            _context.DbContext.ChangeTracker.Clear();
            var listJourneys = await _journeyRepository.GetAllAsyncNoTracking();

            Assert.IsNotNull(listJourneys);
            Assert.AreEqual(5, listJourneys.Count());
        }
        [Test]
        public async Task DeleteJourneyTest_JourneyNotExist()
        {
            var journeyId = 8;
            var journeyEntity = await _journeyRepository.GetAsync(journeyId);
            
            Assert.AreEqual(default(Journey), journeyEntity);  
        }
        [Test]
        public void UpdateJourneyValidatorTest_ValidRequest()
        {
            UpsertJourneyRequestDto upsertJourneyRequest = new UpsertJourneyRequestDto() {Id = 1 ,Name = "Company Trip", Description = "A trip with company at ...", CountryId = 2, CountryName = "Viet Nam", PlaceId = "3", PlaceName = "Đà Nẵng", CurrencyId = 3, CurrencyName = "VND", Amount = 5000000, Status = JourneyStatus.Planning.ToString(), StartDate = DateTime.Now.Date, EndDate = DateTime.Now.AddDays(5).Date, Days = 5, Nights = 4, Version = Encoding.ASCII.GetBytes("Version")};
            upsertJourneyRequest.EndDate = upsertJourneyRequest.EndDate.Date;
            upsertJourneyRequest.StartDate = upsertJourneyRequest.StartDate.Date;
            var updateValidator = new UpdateJourneyValidator();

            var updateValidationResult = updateValidator.Validate(upsertJourneyRequest);

            Assert.IsTrue(updateValidationResult.IsValid);
        }
        [Test]
        public void UpdateJourneyValidatorTest_InvalidRequest()
        {
            UpsertJourneyRequestDto upsertJourneyRequest = new UpsertJourneyRequestDto() { Description = "Short trip", CountryId = 2, PlaceId = "3", Amount = 5000000, Status = JourneyStatus.Planning.ToString(), StartDate = DateTime.Now.Date.AddDays(5).Date, EndDate = DateTime.Now, Days = 5, Nights = 4 };
            upsertJourneyRequest.EndDate = upsertJourneyRequest.EndDate.Date;
            upsertJourneyRequest.StartDate = upsertJourneyRequest.StartDate.Date;
            var updateValidator = new UpdateJourneyValidator();

            var updateValidationResult = updateValidator.Validate(upsertJourneyRequest);

            Assert.IsFalse(updateValidationResult.IsValid);
        }
        [Test]
        public async Task UpdateJourneyTest_Success()
        {
            UpsertJourneyRequestDto upsertJourneyRequest = new UpsertJourneyRequestDto() { Id = 1, Name = "Unit Test Update", Description = "A trip with company at ...", CountryId = 2, CountryName = "Viet Nam", PlaceId = "3", PlaceName = "Đà Nẵng", CurrencyId = 3, CurrencyName = "VND", Amount = 5000000, Status = JourneyStatus.Planning.ToString(), StartDate = DateTime.Now.Date, EndDate = DateTime.Now.AddDays(5).Date, Days = 5, Nights = 4, Version = Array.Empty<Byte>() };
            var entity = _mapper.Map<Journey>(upsertJourneyRequest);
            _journeyRepository.Update(entity);
            await _journeyRepository.SaveChangesAsync();

            var updatedJourney = await _journeyRepository.GetAsyncNoTracking((int)upsertJourneyRequest.Id);
            Assert.AreEqual("Unit Test Update", updatedJourney.Name);
        }
        [Test]
        public async Task UpdateJourneyTest_JourneyNotExist()
        {
            UpsertJourneyRequestDto upsertJourneyRequest = new UpsertJourneyRequestDto() { Id = 9, Name = "Unit Test Update", Description = "A trip with company at ...", CountryId = 2, CountryName = "Viet Nam", PlaceId = "3", PlaceName = "Đà Nẵng", CurrencyId = 3, CurrencyName = "VND", Amount = 5000000, Status = JourneyStatus.Planning.ToString(), StartDate = DateTime.Now.Date, EndDate = DateTime.Now.AddDays(5).Date, Days = 5, Nights = 4, Version = Array.Empty<Byte>() };
            var entity = _mapper.Map<Journey>(upsertJourneyRequest);
            _journeyRepository.Update(entity);
            Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await _journeyRepository.SaveChangesAsync());
        }
        [Test]
        public async Task RemovePlacesFromJourneyTest()
        {
            Assert.Pass();
        }
        [Test]
        public async Task IsJourneyExistTest_JourneyNotExist()
        {
            var journeyList = await _journeyRepository.GetAllAsyncNoTracking();
            var lastJourney = journeyList.Last();
            var entity = await _journeyRepository.GetAsyncNoTracking(lastJourney.Id + 1);
            Assert.AreEqual(default(Journey), entity);
        }
        public async Task IsJourneyExistTest_JourneyExist()
        {
            var journeyIdExist = 1;
            var entity = await _journeyRepository.GetAsyncNoTracking(journeyIdExist);
            Assert.AreNotEqual(default(Journey), entity);
        }
        [Test]
        public async Task IsCurrencyExistTest()
        {
            Assert.Pass();
        }
    }
}