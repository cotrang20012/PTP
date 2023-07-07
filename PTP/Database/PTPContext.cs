using EntityFramework.Exceptions.SqlServer;
using Microsoft.EntityFrameworkCore;
using PTP.Core.Domain.Entities;
using PTP.Core.Domain.Enums;

namespace PTP.Database
{
    public class PTPContext : DbContext
    {
        public DbSet<Journey> Journeys { get; set; } = null!;
        public DbSet<Currency> Currencies { get; set; } = null!;
        public DbSet<Country> Countries { get; set; } = null!;
        public DbSet<Place> Places { get; set; } = null!;
        public PTPContext(DbContextOptions<PTPContext> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder
 //               .LogTo(Console.WriteLine)
                .UseSqlServer(@"Data Source=localhost; Initial Catalog=PTP;TrustServerCertificate=true; Integrated Security=True;")
                .UseExceptionProcessor();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Set delete behavior

            //model seeding for country
            modelBuilder.Entity<Country>().HasData(new Country() {Id = 1, Name = "USA"});
            modelBuilder.Entity<Country>().HasData(new Country() {Id = 2, Name = "Viet Nam" });
            modelBuilder.Entity<Country>().HasData(new Country() {Id = 3, Name = "Switzerland" });

            //model seeding for Places
            modelBuilder.Entity<Place>().HasData(new Place() { Id = 1, Name = "Arizona", CountryId = 1 });
            modelBuilder.Entity<Place>().HasData(new Place() { Id = 2, Name = "California", CountryId = 1 });
            modelBuilder.Entity<Place>().HasData(new Place() { Id = 3, Name = "Đà Nẵng", CountryId = 2 });
            modelBuilder.Entity<Place>().HasData(new Place() { Id = 4, Name = "Hà Nội", CountryId = 2 });
            modelBuilder.Entity<Place>().HasData(new Place() { Id = 5, Name = "Berne", CountryId = 3 });
            modelBuilder.Entity<Place>().HasData(new Place() { Id = 6, Name = "Aargau", CountryId = 3 });

            //model seeding for Currencies
            modelBuilder.Entity<Currency>().HasData(new Currency() { Id = 1, Name = "CHF" });
            modelBuilder.Entity<Currency>().HasData(new Currency() { Id = 2, Name = "USD" });
            modelBuilder.Entity<Currency>().HasData(new Currency() { Id = 3, Name = "VND" });

            //model seeding for journey 
            modelBuilder.Entity<Journey>().HasData(new Journey() { Id = 1, Name = "Company Trip", Description = "A trip with company at ...", CountryId = 2, PlaceId ="3", CurrencyId = 3, Amount = 5000000, Status = JourneyStatus.Planning.ToString(), StartDate = DateTime.Now.Date, EndDate = DateTime.Now.AddDays(5).Date, Days = 5,Nights = 4 });
            modelBuilder.Entity<Journey>().HasData(new Journey() { Id = 2, Name = "Đà Lạt Trip", Description = "Đi chill cùng ae ...", CountryId = 2, PlaceId = "3,4", CurrencyId = 3, Amount = 4000000, Status = JourneyStatus.Planning.ToString(), StartDate = DateTime.Now.Date, EndDate = DateTime.Now.AddDays(5).Date, Days = 5, Nights = 4 });
            modelBuilder.Entity<Journey>().HasData(new Journey() { Id = 3, Name = "Đà Lạt Trip2", Description = "Đi chill cùng ae ...", CountryId = 2, PlaceId = "3,4", CurrencyId = 3, Amount = 4000000, Status = JourneyStatus.Planning.ToString(), StartDate = DateTime.Now.Date, EndDate = DateTime.Now.AddDays(5).Date, Days = 5, Nights = 4 });
            modelBuilder.Entity<Journey>().HasData(new Journey() {Id = 4, Name = "Đà Lạt Trip3", Description = "Đi chill cùng ae ...", CountryId = 2, PlaceId = "3", CurrencyId = 3, Amount = 4000000, Status = JourneyStatus.Planning.ToString(), StartDate = DateTime.Now.Date, EndDate = DateTime.Now.AddDays(5).Date, Days = 5, Nights = 4 });
            modelBuilder.Entity<Journey>().HasData(new Journey() {Id = 5, Name = "Đà Lạt Trip4", Description = "Đi chill cùng ae ...", CountryId = 2, PlaceId = "3,4", CurrencyId = 3, Amount = 4000000, Status = JourneyStatus.Planning.ToString(), StartDate = DateTime.Now.Date, EndDate = DateTime.Now.AddDays(5).Date, Days = 5, Nights = 4 });
            modelBuilder.Entity<Journey>().HasData(new Journey() {Id = 6, Name = "Đà Lạt Trip5", Description = "Đi chill cùng ae ...", CountryId = 2, PlaceId = "4", CurrencyId = 3, Amount = 4000000, Status = JourneyStatus.Planning.ToString(), StartDate = DateTime.Now.Date, EndDate = DateTime.Now.AddDays(5).Date, Days = 5, Nights = 4 });
        }
    }
}
