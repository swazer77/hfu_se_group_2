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

            foreach (DbProduct entity in entities)
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

        public static List<DbProduct> ToEntity(List<Product> products)
        {
            List<DbProduct> entities = new List<DbProduct>();

            foreach (Product product in products)
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
                        Created = ParseStringToDateTime(product.Attributes.Created),
                        LastModified = ParseStringToDateTime(product.Attributes.LastModified),
                        LiveFrom = ParseStringToDateTime(product.Attributes.LiveFrom),
                        LiveUntil = ParseStringToDateTime(product.Attributes.LiveUntil),
                        Price = product.Attributes.Price,
                    },
                    Shop = new DbShop
                    {
                        Url = product.Shop.Url,
                    }
                    
                });
            }
            return entities;
        }

        private static DateTime ParseStringToDateTime(string s)
        {
            DateTime parsedDateTime = new DateTime();
            bool result;
            
            result = DateTime.TryParseExact(
                    s,
                    "o",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.RoundtripKind,
                    out parsedDateTime
                );

            if (result)
            {
                return parsedDateTime;
            }

            return new DateTime();

            //ParseStringToDateTime("1900-01-01 00:00:00");
        }

    }
}
