using System.ComponentModel.DataAnnotations;


namespace PTP.Core.Domain.Entities
{
    public class Place : Entity
    {
        [MaxLength(255)]
        public string Name { get; set; } = null!;
        public int? CountryId { get; set; }
    }
}
