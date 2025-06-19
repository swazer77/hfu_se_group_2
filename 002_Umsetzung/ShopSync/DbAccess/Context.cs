using DBModel;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
namespace DbAccess;

public class Context : DbContext
{
    public DbSet<DBModel.DbProduct> Product { get; set; }
    public DbSet<DBModel.DbLocale> Locale { get; set; }
    public DbSet<DBModel.DbAttributes> Attributes { get; set; }
    public DbSet<DBModel.DbShop> Shop { get; set; }

    // ToDo: Configuration via appsettings.json or similar
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.UseSqlServer(
        //    @"Server=sql.aplix.ch,14444;Initial Catalog=aplixERP_Shop_G2;User Id=aplixShop_G2;Password=apl!xSHOPHaEfU_G2;TrustServerCertificate=True;",
        //    builder => builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null)
        //);
        optionsBuilder.UseSqlServer(
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=aplixERP_Shop_G2",
            builder => builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null)
        );
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