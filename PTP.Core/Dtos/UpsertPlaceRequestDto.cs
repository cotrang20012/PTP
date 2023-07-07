using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTP.Core.Dtos
{
    public class UpsertPlaceRequestDto
    {
        public int? Id { get; set; }
        public string Name { get; set; } = null!;
        public int CountryId { get; set; }
        public byte[]? Version { get; set; } = null!;
    }
}
