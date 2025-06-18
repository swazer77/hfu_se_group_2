using DBModel;

namespace DbAccess;

public class DbClient
{
    public DbClient()
    {
        using Context dbContext = new();
        //dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();
    }

    public List<DbProduct> GetAllProducts()
    {
        using Context dbContext = new ();
        return dbContext.Product
            .Where(p => p.ShopChanged != 'D' && p.ErpChanged != 'D').ToList();
    }

    public List<DbProduct> GetAllErpChanged()
    {
        using Context dbContext = new ();
        return dbContext.Product
            .Where(p => p.ShopChanged != 'D' && p.ErpChanged != 'D' && p.ErpChanged != null).ToList();
    }

    public void InsertProduct(DbProduct product)
    {
        using Context dbContext = new ();
        dbContext.Product.Add(product);
        dbContext.SaveChanges();
    }
}