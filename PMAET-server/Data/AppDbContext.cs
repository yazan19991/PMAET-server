using Microsoft.EntityFrameworkCore;
using PMAET_server.Models;

namespace PMAET_server.Data
{
    // This class manages the connection to the DB
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // This tells EF Core: "I have a table called Inventories"
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<StockTransaction> StockTransactions { get; set; }
    }
}