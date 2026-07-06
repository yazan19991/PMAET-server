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
        public bool NeverExpires { get; set; } = false;
    }


    public class ItemResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = string.Empty;
        public DateTime? ExpiryDate { get; set; }
        public DateTime PurchaseDate { get; set; }
        public bool NeverExpires { get; set; } = false;
        //Helper to tell frontend if it's expired
        public bool IsExpired => !NeverExpires && ExpiryDate.HasValue && ExpiryDate.Value < DateTime.UtcNow;
        public int DaysUntilExpiry => NeverExpires || !ExpiryDate.HasValue ? 999 : (ExpiryDate.Value - DateTime.UtcNow).Days;
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
        public bool NeverExpires { get; set; } = false;
    }
    public class ConsumeItemDto
    {
        [Required]
        [Range(0.01, 999999)]
        public decimal Amount { get; set; }
    }
    public class PurchaseItemDto
    {
        [Required]
        [Range(0.01, 999999)]
        public decimal Amount { get; set; }
    }
}