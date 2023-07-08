using AutoMapper;
using NSubstitute;
using NUnit.Framework;
using PTP.Core.Domain.Entities;
using PTP.Core.Interfaces.Repositories;
using PTP.Core.Interfaces.Services;
using PTP.MappingProfiles;
using PTP.Repositories;
using PTP.Test.Data;
using System.Reflection;


namespace PTP.Test.Services
{
    [SetUpFixture]
    public class JourneyTestService
    {
        protected InMemoryPTPControllerTestContext _context;
        protected IRepository<Journey> _journeyRepository;
        protected IRepository<Country> _countryRepository;
        protected IRepository<Currency> _currencyRepository;
        protected IRepository<Place> _placeRepository;
        protected IMapper _mapper;
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _context = new InMemoryPTPControllerTestContext();
            _journeyRepository = new Repository<Journey>(_context.GetDatabaseContext());
            _countryRepository = new Repository<Country>(_context.GetDatabaseContext());
            _currencyRepository = new Repository<Currency>(_context.GetDatabaseContext());
            _placeRepository = new Repository<Place>(_context.GetDatabaseContext());
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new ProjectAutoMapperProfile()));
            _mapper = configuration.CreateMapper();
        }
        [SetUp]
        public void Setup()
        {
            _context.CreateDataForDatabase();
        }
        [TearDown]
        public void TearDown()
        {
            _context.CleanUp();
        }
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _context.CleanUpDatabaseAndDispose();
        }
    }
}
