using DBModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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
        using Context dbContext = new();
        return dbContext.Product
            .Where(p => p.ShopChanged != 'D' && p.ErpChanged != 'D')
            .Include(nameof(Context.Attributes))
            .Include(nameof(Context.Shop))
            .ToList();
    }

    public List<DbProduct> GetAllProductsErpChanged()
    {
        using Context dbContext = new();
        return dbContext.Product
            .Include(nameof(Context.Attributes))
            .Include(nameof(Context.Shop))
            .Where(p => p.ShopChanged != 'D' && p.ErpChanged != 'D' && p.ErpChanged != null).ToList();
    }

    public void InsertOrUpdateProducts(List<DbProduct> products)
    {
        foreach (DbProduct product in products)
        {
            InsertOrUpdateProduct(product);
        }
    }

    private void InsertOrUpdateProduct(DbProduct product)
    {
        if (product == null)
        {
            throw new ArgumentNullException(nameof(product), "Product cannot be null.");
        }

        using Context dbContext = new();

        DbShop? existingShop = dbContext.Shop.FirstOrDefault(s => s.Url == product.Shop.Url);
        if (existingShop != null) product.Shop = existingShop;

        DbProduct? existingProduct = dbContext.Product
            .Include(nameof(Context.Attributes))
            .Include(nameof(Context.Shop))
            .FirstOrDefault(p => p.ProductId == product.ProductId && p.Shop.Url == product.Shop.Url);
        if (existingProduct != null)
        {
            dbContext.Product.Remove(existingProduct);
            dbContext.Attributes.Remove(existingProduct.Attributes);
            dbContext.SaveChanges();
        }

        dbContext.Product.Add(product);
        dbContext.SaveChanges();
    }
}