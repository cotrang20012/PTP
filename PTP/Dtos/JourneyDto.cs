using PTP.Core.Domain.Entities;

namespace PTP.Dtos
{
    public class JourneyDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set;}
        public int CurrencyId { get; set; }
        public int CountryId { get; set; }
        public int PlaceId { get; set; }
        public Currency Currency { get; set; } = null!;
        public Country Country { get; set; } = null!;
        public Place Place { get; set; } = null!;
        public int Amount { get; set; }
        public string Status { get; set; } = null!;
        public byte[] Version { get; set; } = null!;
    }
}
