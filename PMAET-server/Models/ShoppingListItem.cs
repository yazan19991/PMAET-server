
namespace PMAET_server.Models
{

public class ShoppingListItem
{
    public int Id { get; set; }
    public int UserId { get; set; }

    public string Name { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public string Unit { get; set; } = "pcs";

    public bool IsPurchased { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? PurchasedAt { get; set; }
}
}