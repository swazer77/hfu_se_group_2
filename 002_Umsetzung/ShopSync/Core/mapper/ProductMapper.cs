using Model;
using System.Collections.Generic;

namespace Core.mapper
{
    public static class ProductMapper
    {
        /*
        public static List<Product> ToModel(List<ProductEntity> entities)
        {
            List<Product> products = new List<Product>();

            foreach (var entity in entities)
            {
                products.Add(new Product
                {
                    Id = entity.Id,
                    Type = entity.Type,
                    Attributes = new Attributes
                    {
                        Locale = [new Locale
                        {
                            Language = entity.Attribute.Locale.Language,
                            Name = entity.Attribute.Locale.Name
                        }],
                        Price = entity.Attribute.Price,
                        Created = entity.Attribute.Created,
                        LastModified = entity.Attribute.Modified,
                        LiveFrom = entity.Attribute.LiveFrom,
                        LiveUntil = entity.Attribute.LiveUntil
                    },
                    Shop = new Shop
                    {
                        Url = entity.Attribute.Shop.Url
                    }
                });
            }
            return products;
        }

        public static List<ProductEntity> ToEntity(List<Product> products)
        {
            List<ProductEntity> entities = new List<ProductEntity>();

            foreach (var product in products)
            {
                entities.Add(new ProductEntity
                {

                });
            }
        }


        */

    }
}
