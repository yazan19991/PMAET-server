using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMAET_server.Models
{
    public class Label
    {
        [Key] 
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public List<InventoryItem> InventoryItems { get; set; } = new();
    }
}
