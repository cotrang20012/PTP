namespace PTP.Dtos
{
    public class UpsertJourneyRequest
    {
        public int? Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int CurrencyId { get; set; }
        public int CountryId { get; set; }
        public string PlaceId { get; set; } = null!;
        public int Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = null!;
        public byte[]? Version { get; set; } = null!;
        public int Days { get; set; }
        public int Nights { get; set; }
    }
}
