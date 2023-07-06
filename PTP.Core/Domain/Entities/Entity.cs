using System.ComponentModel.DataAnnotations;

namespace PTP.Core.Domain.Entities
{
    public class Entity
    {
        public int Id { get; set; }
        [Timestamp]
        public byte[] Version { get; set; } = null!;
    }
}
