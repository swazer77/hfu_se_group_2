using DBModel;
using Microsoft.EntityFrameworkCore;
using Utility;

namespace DbAccess;

public class Context : DbContext
{
    public DbSet<DbProduct> Product { get; set; }
    public DbSet<DbLocale> Locale { get; set; }
    public DbSet<DbAttributes> Attributes { get; set; }
    public DbSet<DbShop> Shop { get; set; }

    private readonly string connectionString;

    public Context()
    {
        DbConfig dbConfig = new ConfigReader().GetDatabaseConfig();
        if (string.IsNullOrEmpty(dbConfig.MsSqlConnectionString))
        {
            throw new InvalidOperationException("Database connection string is not configured.");
        }
        connectionString = dbConfig.MsSqlConnectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(connectionString,
            builder => builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null));
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Product -> Attributes (one-to-one, cascade delete)
        modelBuilder.Entity<DbProduct>()
            .HasOne(p => p.Attributes)
            .WithOne()
            .HasForeignKey<DbProduct>(p => p.Id)
            .OnDelete(DeleteBehavior.Cascade);

        // Attributes -> Locale (one-to-many, cascade delete)
        modelBuilder.Entity<DbAttributes>()
            .HasMany(a => a.Locale)
            .WithOne()
            .HasForeignKey("DbAttributesId")
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }
}