using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTP.Core.Domain.Entities
{
    public class Country : Entity
    {
        [MaxLength(255)]
        public string Name { get; set; } = null!;
        public ICollection<Place> Places { get; set; } = null!;
    }
}
