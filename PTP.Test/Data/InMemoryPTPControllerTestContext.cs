using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NSubstitute;
using PTP.Core.Domain.Entities;
using PTP.Core.Domain.Enums;
using PTP.Database;
using System.Text;

namespace PTP.Test.Data
{
    public class InMemoryPTPControllerTestContext
    {
        private readonly DbContextOptions<PTPContext> _contextOptions;
        public readonly PTPContext DbContext;
        public InMemoryPTPControllerTestContext()
        {
            _contextOptions = new DbContextOptionsBuilder<PTPContext>()
                .UseInMemoryDatabase("PTPControllerTest")
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            DbContext = new PTPContext(_contextOptions);
            DbContext.Database.EnsureDeleted();
        }

        public PTPContext GetDatabaseContext()  
        {
            return DbContext;
        }

        public void CleanUp()
        {
            DbContext.ChangeTracker.Clear();   
        }
        public void CleanUpDatabaseAndDispose()
        {
            DbContext.Dispose();
        }
        public void CreateDataForDatabase()
        {
            DbContext.Database.EnsureDeleted();
            DbContext.Countries.AddRange
              (
              new Country() { Id = 1, Name = "USA", Version = Array.Empty<byte>() },
              new Country() { Id = 2, Name = "Viet Nam", Version = Array.Empty<byte>() },
              new Country() { Id = 3, Name = "Switzerland", Version = Array.Empty<byte>() }
              );
            DbContext.Places.AddRange
                (
                new Place() { Id = 1, Name = "Arizona", CountryId = 1, Version = Array.Empty<byte>() },
                new Place() { Id = 2, Name = "California", CountryId = 1, Version = Array.Empty<byte>() },
                new Place() { Id = 3, Name = "Đà Nẵng", CountryId = 2, Version = Array.Empty<byte>() },
                new Place() { Id = 4, Name = "Hà Nội", CountryId = 2, Version = Array.Empty<byte>() },
                new Place() { Id = 5, Name = "Berne", CountryId = 3, Version = Array.Empty<byte>() },
                new Place() { Id = 6, Name = "Aargau", CountryId = 3, Version = Array.Empty<byte>() }
                );
            DbContext.Currencies.AddRange
                (
                new Currency() { Id = 1, Name = "CHF", Version = Array.Empty<byte>() },
                new Currency() { Id = 2, Name = "USD", Version = Array.Empty<byte>() },
                new Currency() { Id = 3, Name = "VND", Version = Array.Empty<byte>() }
                );
            DbContext.Journeys.AddRange
                (
                new Journey() { Id = 1, Name = "Company Trip", Description = "A trip with company at ...", CountryId = 2, PlaceId = "3", CurrencyId = 3, Amount = 5000000, Status = JourneyStatus.Planning.ToString(), StartDate = DateTime.Now.Date, EndDate = DateTime.Now.AddDays(5).Date, Days = 5, Nights = 4, Version = Array.Empty<byte>() },
                new Journey() { Id = 2, Name = "Đà Lạt Trip", Description = "Đi chill cùng ae ...", CountryId = 2, PlaceId = "3,4", CurrencyId = 3, Amount = 4000000, Status = JourneyStatus.Planning.ToString(), StartDate = DateTime.Now.Date, EndDate = DateTime.Now.AddDays(5).Date, Days = 5, Nights = 4, Version = Array.Empty<byte>() },
                new Journey() { Id = 3, Name = "Đà Lạt Trip2", Description = "Đi chill cùng ae ...", CountryId = 2, PlaceId = "3,4", CurrencyId = 3, Amount = 4000000, Status = JourneyStatus.Planning.ToString(), StartDate = DateTime.Now.Date, EndDate = DateTime.Now.AddDays(5).Date, Days = 5, Nights = 4, Version = Array.Empty<byte>() },
                new Journey() { Id = 4, Name = "Đà Lạt Trip3", Description = "Đi chill cùng ae ...", CountryId = 2, PlaceId = "3", CurrencyId = 3, Amount = 4000000, Status = JourneyStatus.Planning.ToString(), StartDate = DateTime.Now.Date, EndDate = DateTime.Now.AddDays(5).Date, Days = 5, Nights = 4, Version = Array.Empty<byte>() },
                new Journey() { Id = 5, Name = "Đà Lạt Trip4", Description = "Đi chill cùng ae ...", CountryId = 2, PlaceId = "3,4", CurrencyId = 3, Amount = 4000000, Status = JourneyStatus.Planning.ToString(), StartDate = DateTime.Now.Date, EndDate = DateTime.Now.AddDays(5).Date, Days = 5, Nights = 4, Version = Array.Empty<byte>() },
                new Journey() { Id = 6, Name = "Đà Lạt Trip5", Description = "Đi chill cùng ae ...", CountryId = 2, PlaceId = "4", CurrencyId = 3, Amount = 4000000, Status = JourneyStatus.Planning.ToString(), StartDate = DateTime.Now.Date, EndDate = DateTime.Now.AddDays(5).Date, Days = 5, Nights = 4, Version = Array.Empty<byte>() }
                );

            DbContext.SaveChanges();
            DbContext.ChangeTracker.Clear();
        }

    }
}
