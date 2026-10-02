using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMAET_server.Models
{
    public class InventoryItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty; 
        public decimal Quantity { get; set; } 
        public string Unit { get; set; } = "pcs"; 
        public DateTime? ExpiryDate { get; set; }
        public bool NeverExpires { get; set; } = false;
        public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;
        public int InventoryId { get; set; }
        [ForeignKey("InventoryId")]
        public Inventory? Inventory { get; set; }
        
        public List<Label> Labels  { get; set; } = new();
       
        public LowStockRule? StockRule  { get; set; } = null;
    }
}


