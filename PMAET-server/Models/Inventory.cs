using System.ComponentModel.DataAnnotations;

namespace PMAET_server.Models
{
    public class Inventory
    {
        [Key] // This tells EF Core: "This is the Primary Key"
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int UserId { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}