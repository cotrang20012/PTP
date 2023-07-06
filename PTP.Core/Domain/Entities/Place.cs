using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTP.Core.Domain.Entities
{
    public class Place : Entity
    {
        [MaxLength(255)]
        public string Name { get; set; } = null!;
        public int? CountryId { get; set; }
    }
}
