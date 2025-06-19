using DBModel;
using Microsoft.EntityFrameworkCore;

namespace DbAccess;

public class DbClient
{
    public DbClient()
    {
        using Context dbContext = new();
        //dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();
    }

    // ############################################################################################
    // Public methods
    // ############################################################################################

    // Product ####################################################################################

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
            .Where(p => p.ErpChanged != 'U' && p.ShopChanged != 'C' || p.ErpChanged != 'D' && p.ShopChanged != 'D').ToList();
    }

    public void InsertOrUpdateProducts(List<DbProduct> products)
    {
        foreach (DbProduct product in products)
        {
            InsertOrUpdateProduct(product);
        }
        CleanupDb();
    }
        
    public void SetFlagsForProductById(int id, char erpChanged, char shopChanged)
    {
        using Context dbContext = new();
        DbProduct? product = dbContext.Product.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            throw new ArgumentException($"Product with ID {id} not found.", nameof(id));
        }
        product.ErpChanged = erpChanged;
        product.ShopChanged = shopChanged;
        dbContext.SaveChanges();
    }

    // ############################################################################################
    // Private methods
    // ############################################################################################

    // Product ####################################################################################

    private static void CleanupDb()
    {
        // Remove products that are marked as deleted in both ShopChanged and ErpChanged
        using Context dbContext = new();
        dbContext.Product.RemoveRange(dbContext.Product.Where(p => p.ShopChanged == 'D' && p.ErpChanged == 'D'));
        dbContext.SaveChanges();
    }

    // delete: nur 

    private static void InsertOrUpdateProduct(DbProduct product)
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