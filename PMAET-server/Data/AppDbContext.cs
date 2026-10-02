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
        public DbSet<Label> Labels  { get; set; }
        public DbSet<LowStockRule> LowStockRules { get; set; }
        public DbSet<ShoppingListItem> ShoppingListItems { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<InventoryItem>()
                .Property(item => item.Quantity)
                .HasPrecision(18, 3);

            modelBuilder.Entity<ShoppingListItem>()
                .Property(item => item.Quantity)
                .HasPrecision(18, 3);

            modelBuilder.Entity<LowStockRule>()
                .Property(rule => rule.MinimumQuantity)
                .HasPrecision(18, 3);

            modelBuilder.Entity<StockTransaction>()
                .Property(rule => rule.QuantityChanged)
                .HasPrecision(18, 3);
        }
    }
}