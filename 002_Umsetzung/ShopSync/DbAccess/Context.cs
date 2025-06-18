using Microsoft.EntityFrameworkCore;
namespace DbAccess;

public class Context : DbContext
{
    public DbSet<DBModel.DbProduct> Product { get; set; }
    public DbSet<DBModel.DbLocale> Product { get; set; }
    public DbSet<DBModel.DbProduct> Product { get; set; }
    public DbSet<DBModel.DbProduct> Product { get; set; }

}