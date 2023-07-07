using System.ComponentModel.DataAnnotations;


namespace PTP.Core.Domain.Entities
{
    public class Currency : Entity
    {
        [MaxLength(255)]
        public string Name { get; set; } = null!;
    }
}
