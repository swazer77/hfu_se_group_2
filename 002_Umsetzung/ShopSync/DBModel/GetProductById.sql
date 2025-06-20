select * from Product
join Attributes on Product.id = Attributes.id
join Locale on Product.id = Locale.DbAttributesId
join Shop on Product.ShopID = Shop.id
where product.ProductID = '111111111111113';