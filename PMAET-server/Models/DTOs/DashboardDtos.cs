using System.ComponentModel.DataAnnotations;

namespace PMAET_server.Models.DTOs
{

        public class DashboardSummaryDto
        {

            public int TotalInventories { get; set; } 
            public int TotalItems { get; set; }
            public int ExpiredItems { get; set; }
            public int ExpiringSoonItems { get; set; }
            public int LowStockItems { get; set; }
            public int ShoppingListItems { get; set; }

        }



}