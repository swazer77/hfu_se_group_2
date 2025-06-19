using System.Globalization;
using DBModel;
using HttpModel;

namespace Core.mapper
{
    public static class ProductMapper
    {
        public static List<Product> ToModel(List<DbProduct> entities)
        {
            List<Product> products = new List<Product>();

            foreach (var entity in entities)
            {
                products.Add(new Product
                {
                    Id = entity.ProductId,
                    Type = entity.Type,
                    Attributes = new Attributes
                    {
                        Locale = [new Locale
                        {
                            Language = entity.Attributes.Locale.First().Language,
                            Name = entity.Attributes.Locale.First().Name,
                        }],
                        Price = entity.Attributes.Price,
                        Created = entity.Attributes.Created?.ToString("o"),
                        LastModified = entity.Attributes.LastModified?.ToString("o"),
                        LiveFrom = entity.Attributes.LiveFrom.ToString("o"),
                        LiveUntil = entity.Attributes.LiveUntil.ToString("o"), 
                    },
                    Shop = new Shop
                    {
                        Url = entity.Shop.Url,
                    }
                });
            }
            return products;
        }

        public static List<DbProduct> ToEntity(List<Product>? products)
        {
            List<DbProduct> entities = new List<DbProduct>();

            foreach (var product in products)
            {
                entities.Add(new DbProduct
                {
                    ProductId = product.Id,
                    Type = product.Type,
                    Attributes = new DbAttributes
                    {
                        Locale = [new DbLocale
                        {
                            Name = product.Attributes.Locale.First().Name,
                            Language = product.Attributes.Locale.First().Language,
                        }],
                        Created = DateTime.ParseExact(product.Attributes.Created, "o", CultureInfo.InvariantCulture),
                        LastModified = DateTime.ParseExact(product.Attributes.LastModified, "o", CultureInfo.InvariantCulture),
                        LiveFrom = DateTime.ParseExact(product.Attributes.LiveFrom, "o", CultureInfo.InvariantCulture),
                        LiveUntil = DateTime.ParseExact(product.Attributes.LiveUntil, "o", CultureInfo.InvariantCulture),
                        Price = product.Attributes.Price,
                    }
                });
            }
            return entities;
        }

    }
}
