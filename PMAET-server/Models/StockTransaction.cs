using System.ComponentModel.DataAnnotations;

namespace PMAET_server.Models
{
    public class StockTransaction
    {   
        [Key]
        public int Id { get; set; }
        public int InventoryItemId { get; set; }
   
        public InventoryItem? InventoryItem { get; set; }
        public string Type { get; set; } = string.Empty; // "Purchase" or "Consume"
        public decimal QuantityChanged { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}