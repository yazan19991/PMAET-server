using System.ComponentModel.DataAnnotations;

namespace PMAET_server.Models.DTOs
{


    public class CreateShoppingListItemDto
    {

        [Range(1, int.MaxValue)]
        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Range(0.01, 999999)]
        public decimal Quantity { get; set; }

        [Required]
        [MaxLength(30)]
        public string Unit { get; set; } = "pcs";
    }
    public class UpdateShoppingListItemDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Range(0.01, 999999)]
        public decimal Quantity { get; set; }

        [Required]
        [MaxLength(30)]
        public string Unit { get; set; } = "pcs";
    }
    public class ShoppingListItemResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = "pcs";
        public bool IsPurchased { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? PurchasedAt { get; set; }
    }

}