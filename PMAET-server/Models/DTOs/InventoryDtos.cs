using System.ComponentModel.DataAnnotations;

namespace PMAET_server.Models.DTOs
{
        public class CreateInventoryDto
        {
            [Required] 
            [MaxLength(100)]
            public string Name { get; set; } = string.Empty;
            [MaxLength(500)]
            public string Description { get; set; } = string.Empty;

            // For now, we accept UserId manually. 
            // Later, we will get this automatically from the Login Token (JWT).
            [Required]
            public int UserId { get; set; }
        }

        public class InventoryResponseDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public int ItemCount { get; set; } 
            public DateTime CreatedAt { get; set; }
        }
            public class InventoryUpdateDto
        {   
            [Required]
            [MaxLength(100)]
            public string Name { get; set; } = string.Empty;
            [MaxLength(500)]
            public string Description { get; set; } = string.Empty;

        }
}
