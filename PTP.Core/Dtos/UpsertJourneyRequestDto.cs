using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTP.Core.Dtos
{
    public class UpsertJourneyRequestDto
    {
        public int? Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int CurrencyId { get; set; }
        public string CurrencyName { get; set; }   
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string PlaceId { get; set; } = null!;
        public string PlaceName { get; set; }
        public int Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = null!;
        public byte[]? Version { get; set; } = null!;
        public int Days { get; set; }
        public int Nights { get; set; }
    }
}
