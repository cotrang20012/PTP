using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTP.Core.Domain.Entities
{
    public class Journey : Entity
    {
        [MaxLength(255)]
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int CountryId { get; set; }
        public Country? Country { get; set; } = null!;
        public int PlaceId { get; set; }
        public Place? Place { get; set; } = null!;
        public int CurrencyId { get; set; }
        public Currency? Currency { get; set; } = null!;
        public int Amount { get; set; }
        public string Status { get; set; } = null!;
    }
}
