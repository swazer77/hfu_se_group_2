using DBModel;

namespace DbAccess;

public class DbClient
{
    public DbClient()
    {
        //using (Context dbContext = new())
        //{
        //    dbContext.Database.EnsureDeleted();
        //    dbContext.Database.EnsureCreated();
        //}
    }

    public List<DbProduct> GetAllProducts()
    {
        using Context dbContext = new ();
        return dbContext.Product.ToList();
    }

    public void InsertProduct(DbProduct product)
    {
        using Context dbContext = new ();
        dbContext.Product.Add(product);
        dbContext.SaveChanges();
    }
}