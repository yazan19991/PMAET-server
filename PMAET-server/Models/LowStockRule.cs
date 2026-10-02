using System.ComponentModel.DataAnnotations;

namespace PMAET_server.Models
{
    public class LowStockRule
    {   
        [Key]
        public int Id { get; set; }
        public InventoryItem? InventoryItem { get; set; }
        public int InventoryItemId { get; set; }
        public decimal  MinimumQuantity { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}