using System.ComponentModel.DataAnnotations;


namespace PTP.Core.Domain.Entities
{
    public class Country : Entity
    {
        [MaxLength(255)]
        public string Name { get; set; } = null!;
        public ICollection<Place>? Places { get; set; } = null!;
    }
}
