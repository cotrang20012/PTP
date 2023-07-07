using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTP.Core.Dtos
{
    public class SearchJourneyRequestDto
    {
        public string NameAndDescription { get; set; } = null!;
        public int? CurrencyId { get; set; }
        public int? CountryId { get; set; }
        public int? FromAmount { get; set; }
        public int? ToAmount { get; set; }
        public DateTime? FromStartDate { get; set; }
        public DateTime? ToStartDate { get; set; }
        public DateTime? FromEndDate { get; set; }
        public DateTime? ToEndDate { get; set; }
        public string Status { get; set; } = null!;
    }
}
