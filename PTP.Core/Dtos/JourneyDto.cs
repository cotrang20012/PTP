using PTP.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTP.Core.Dtos
{
    public class JourneyDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int CurrencyId { get; set; }
        public int CountryId { get; set; }
        public string PlaceId { get; set; } = null!;
        public Currency Currency { get; set; } = null!;
        public Country Country { get; set; } = null!;
        public ICollection<Place>? Places { get; set; }
        public int Amount { get; set; }
        public string Status { get; set; } = null!;
        public byte[] Version { get; set; } = null!;
        public int Days { get; set; }
        public int Nights { get; set; }
    }
}
