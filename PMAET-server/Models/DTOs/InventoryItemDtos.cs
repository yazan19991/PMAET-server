using System.ComponentModel.DataAnnotations;

namespace PMAET_server.Models.DTOs
{

    public class CreateItemDto
    {
        [Required]
        public int InventoryId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [Range(0.01, 999999)]
        public decimal Quantity { get; set; }
        [Required]
        [MaxLength(30)]
        public string Unit { get; set; } = "pcs";
        public DateTime? ExpiryDate { get; set; }
    }


    public class ItemResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = string.Empty;
        public DateTime? ExpiryDate { get; set; }
        public DateTime PurchaseDate { get; set; }
        
        //Helper to tell frontend if it's expired
        public bool IsExpired => ExpiryDate.HasValue && ExpiryDate.Value < DateTime.UtcNow;
        public int DaysUntilExpiry => ExpiryDate.HasValue ? (ExpiryDate.Value - DateTime.UtcNow).Days : 999;
    }


    public class UpdateItemDto
    {
        [Required]
        public int InventoryId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [Range(0.01, 999999)]
        public decimal Quantity { get; set; }
        [Required]
        [MaxLength(30)]
        public string Unit { get; set; } = "pcs";
        public DateTime? ExpiryDate { get; set; }
    }
}