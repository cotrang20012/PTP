using PTP.Core.Domain.Entities;
using PTP.Core.Domain.Enums;
using PTP.Core.Dtos;
using PTP.Test.Services;
using PTP.Validator;

namespace PTP.Test
{
    [TestFixture]
    public class JourneyServiceTest : JourneyTestService
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
        public void UpdateJourneyValidator_ValidRequest()
        {
            Assert.Pass();
        }
        [Test]
        public void UpdateJourneyValidator_InvalidRequest()
        {
            Assert.Pass();
        }
        [Test]
        public async Task UpdateJourney_JourneyExist()
        {
            Assert.Pass();
        }
        [Test]
        public async Task UpdateJourney_JourneyNotExist()
        {
            Assert.Pass();
        }
        [Test]
        public async Task RemovePlacesFromJourneyTest()
        {
            Assert.Pass();
        }
        [Test]
        public async Task IsJourneyExistTest_J()
        {
            Assert.Pass(); 
        }
        [Test]
        public async Task IsCurrencyExist()
        {
            Assert.Pass();
        }
    }
}