using HttpModel;

namespace Core.testdata
{
    public class TestApiProducts
    {
        private List<Product> createProducts = new List<Product>
        {
            new Product
            {
                Id = "test-product-group2-66",
                Type = "product",
                Attributes = new Attributes
                {
                    Locale = new List<Locale>
                    {
                        new Locale { Language = "de", Name = "Test Create Product Group 2" }
                    },
                    Price = 15,
                    Created = "2025-01-01 00:00:00",
                    LastModified = "",
                    LiveFrom = "2025-01-01 00:00:00",
                    LiveUntil = "2025-12-31 23:59:59"
                },
                Shop = new Shop
                {
                    Url = "https://test.webstores.ch/boreas/shop/api/v2"
                }
            },
            new Product
            {
                Id = "test-product-group2-67",
                Type = "product",
                Attributes = new Attributes
                {
                    Locale = new List<Locale>
                    {
                        new Locale { Language = "de", Name = "Test Create Product Group 2" },
                    },
                    Price = 19.99,
                    Created = "2025-02-01 00:00:00",
                    LastModified = "",
                    LiveFrom = "2025-02-01 00:00:00",
                    LiveUntil = "2025-12-31 23:59:59"
                },
                Shop = new Shop
                {
                    Url = "https://test.webstores.ch/boreas/shop/api/v2"
                }
            }

        };

        private List<Product> deleteProducts = new List<Product>
        {
            new Product
            {
                Id = "test-product-group2-66",
                Type = "product",
                Attributes = new Attributes
                {
                    Locale = new List<Locale>
                    {
                        new Locale { Language = "de", Name = "Test Create Product Group 2" }
                    },
                    Price = 15,
                    Created = "2025-01-01 00:00:00",
                    LastModified = "",
                    LiveFrom = "2025-01-01 00:00:00",
                    LiveUntil = "2025-12-31 23:59:59"
                },
                Shop = new Shop
                {
                    Url = "https://test.webstores.ch/boreas/shop/api/v2"
                }
            },
            new Product
            {
                Id = "test-product-group2-67",
                Type = "product",
                Attributes = new Attributes
                {
                    Locale = new List<Locale>
                    {
                        new Locale { Language = "de", Name = "Test Create Product Group 2" },
                    },
                    Price = 99.99,
                    Created = "2025-02-01 00:00:00",
                    LastModified = "",
                    LiveFrom = "2025-02-01 00:00:00",
                    LiveUntil = "2025-12-31 23:59:59"
                },
                Shop = new Shop
                {
                    Url = "https://test.webstores.ch/boreas/shop/api/v2"
                }
            }

        };

        public List<Product> GetCreateProducts()
        {
            return createProducts;
        }

        public List<Product> GetUpdateProduct()
        {
            return new()
            {
                new Product
                {
                    Id = "test-product-group2-67",
                    Type = "product",
                    Attributes = new Attributes
                    {
                        Locale = new List<Locale>
                        {
                            new Locale { Language = "de", Name = "Test Create Product Group 2" },
                        },
                        Price = 99.99,
                        Created = "2025-02-01 00:00:00",
                        LastModified = "",
                        LiveFrom = "2025-02-01 00:00:00",
                        LiveUntil = "2025-12-31 23:59:59"
                    },
                    Shop = new Shop
                    {
                        Url = "https://test.webstores.ch/boreas/shop/api/v2"
                    }
                }
            };
        }

        public List<Product> GetDeleteProduct()
        {
            return deleteProducts;
        }
    }
}
