
using Domain;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Persistence.Contexts
{
    public class StockDbContext : DbContext
    {
        public DbSet<Stock> Stocks { get; set; }
        public static StockDbContext Create(IMongoDatabase database) =>
        new(new DbContextOptionsBuilder<StockDbContext>()
            .UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName)
            .Options);

        public StockDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Stock>().ToCollection("stocks");
            modelBuilder.Entity<Stock>().HasKey(x => x.Id);
            modelBuilder.Entity<Stock>().Property(x => x.ProductId).IsRequired();
            Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;
        }
    }
}