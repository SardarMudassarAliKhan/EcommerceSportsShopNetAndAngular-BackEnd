﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Infrastracture.Data;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(AppDbContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                if (!context.ProductBrands.Any())
                {
                    context.ProductBrands.AddRange(GetPreconfiguredProductBrands());

                    await context.SaveChangesAsync();
                }

                if (!context.ProductTypes.Any())
                {
                    context.ProductTypes.AddRange(GetPreconfiguredProductTypes());

                    await context.SaveChangesAsync();
                }

                if (!context.Products.Any())
                {
                    context.Products.AddRange(GetPreconfiguredProducts());

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<StoreContextSeed>();
                logger.LogError(ex.Message);
            }
        }

        private static IEnumerable<ProductBrand> GetPreconfiguredProductBrands()
        {
            return new List<ProductBrand>()
            {
                new ProductBrand() {Name = "Angular"},
                new ProductBrand() {Name = "NETCore"},
                new ProductBrand() {Name = "VS Code"},
                new ProductBrand() {Name = "React"},
                new ProductBrand() {Name = "TypeScript"},
                new ProductBrand() {Name = "Redis"},
            };
        }

        private static IEnumerable<ProductType> GetPreconfiguredProductTypes()
        {
            return new List<ProductType>()
            {
                new ProductType() {Name = "Boards"},
                new ProductType() {Name = "Hats"},
                new ProductType() {Name = "Boots"},
                new ProductType() {Name = "Gloves"}
            };
        }

        static IEnumerable<Product> GetPreconfiguredProducts()
        {
            return new List<Product>()
            {
                new Product()
                {
                    ProductTypeId = 1, ProductBrandId = 1, Description = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Maecenas porttitor congue massa. Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.",
                    Name = "Angular Speedster Board 2000", Price = 200.00M,
                    PictureUrl = "images/products/sb-ang1.png"
                },
                new Product()
                {
                    ProductTypeId = 1, ProductBrandId = 1, Description = "Nunc viverra imperdiet enim. Fusce est. Vivamus a tellus.",
                    Name = "Green Angular Board 3000", Price = 150.00M,
                    PictureUrl = "images/products/sb-ang2.png"
                },
                new Product()
                {
                    ProductTypeId = 1, ProductBrandId = 2, Description = "Suspendisse dui purus, scelerisque at, vulputate vitae, pretium mattis, nunc. Mauris eget neque at sem venenatis eleifend. Ut nonummy.",
                    Name = "Core Board Speed Rush 3", Price = 180.00M,
                    PictureUrl = "images/products/sb-core1.png"
                },
                new Product()
                {
                    ProductTypeId = 1, ProductBrandId = 2, Description = "Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Proin pharetra nonummy pede. Mauris et orci.",
                    Name = "Net Core Super Board", Price = 300.00M,
                    PictureUrl = "images/products/sb-core2.png"
                },
                new Product()
                {
                    ProductTypeId = 1, ProductBrandId = 4, Description = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Maecenas porttitor congue massa. Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.",
                    Name = "React Board Supper Whizzy Fast", Price = 250.00M,
                    PictureUrl = "images/products/sb-react1.png"
                },
                new Product()
                {
                    ProductTypeId = 1, ProductBrandId = 5, Description = "Aenean nec lorem. In porttitor. Donec laoreet nonummy augue.",
                    Name = "Typescript Entry Board", Price = 12,
                    PictureUrl = "images/products/sb-ts1.png"
                },
                new Product()
                {
                    ProductTypeId = 2, ProductBrandId = 2, Description = "Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.", Name = "Core Maroon Hat",
                    Price = 10.00M, PictureUrl = "images/products/hat-core1.png"
                },
                new Product()
                {
                    ProductTypeId = 2, ProductBrandId = 4, Description = "Suspendisse dui purus, scelerisque at, vulputate vitae, pretium mattis, nunc. Mauris eget neque at sem venenatis eleifend. Ut nonummy.",
                    Name = "Green React Woolen Hat", Price = 8.00M,
                    PictureUrl = "images/products/hat-react1.png"
                },
                new Product()
                {
                    ProductTypeId = 2, ProductBrandId = 4, Description = "Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.", Name = "Blue React Woolen Hat",
                    Price = 15.00M, PictureUrl = "images/products/hat-react2.png"
                },
                new Product()
                {
                    ProductTypeId = 4, ProductBrandId = 3, Description = "Nunc viverra imperdiet enim. Fusce est. Vivamus a tellus.",
                    Name = "Blue Code Gloves", Price = 18.00M,
                    PictureUrl = "images/products/glove-code1.png"
                },
                new Product()
                {
                    ProductTypeId = 4, ProductBrandId = 3, Description = "Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Proin pharetra nonummy pede. Mauris et orci.",
                    Name = "Green Code Gloves", Price = 15.00M,
                    PictureUrl = "images/products/glove-code2.png"
                },
                new Product()
                {
                    ProductTypeId = 4, ProductBrandId = 4, Description = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Maecenas porttitor congue massa. ",
                    Name = "Purple React Gloves", Price = 16.00M,
                    PictureUrl = "images/products/glove-react1.png"
                },
                new Product()
                {
                    ProductTypeId = 4, ProductBrandId = 4, Description = "Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Proin pharetra nonummy pede. Mauris et orci.",
                    Name = "Green React Gloves", Price = 14.00M,
                    PictureUrl = "images/products/glove-react2.png"
                },
                new Product()
                {
                    ProductTypeId = 3, ProductBrandId = 6, Description = "Suspendisse dui purus, scelerisque at, vulputate vitae, pretium mattis, nunc. Mauris eget neque at sem venenatis eleifend. Ut nonummy.",
                    Name = "Redis Red Boots", Price = 250.00M,
                    PictureUrl = "images/products/boot-redis1.png"
                },
                new Product()
                {
                    ProductTypeId = 4, ProductBrandId = 4, Description = "Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Proin pharetra nonummy pede. Mauris et orci.",
                    Name = "Green React Gloves", Price = 14.00M,
                    PictureUrl = "images/products/glove-react2.png"
                },
                new Product()
                {
                    ProductTypeId = 3, ProductBrandId = 2, Description = "Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Proin pharetra nonummy pede. Mauris et orci.",
                    Name = "Core Red Boots", Price = 189.99M,
                    PictureUrl = "images/products/boot-core2.png"
                },
                new Product()
                {
                    ProductTypeId = 3, ProductBrandId = 2, Description = "Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Proin pharetra nonummy pede. Mauris et orci.",
                    Name = "Core Purple Boots", Price = 199.99M,
                    PictureUrl = "images/products/boot-core1.png"
                },
                new Product()
                {
                    ProductTypeId = 3, ProductBrandId = 1, Description = "Aenean nec lorem. In porttitor. Donec laoreet nonummy augue.",
                    Name = "Angular Purple Boots", Price = 150.00M,
                    PictureUrl = "images/products/boot-ang2.png"
                },
                new Product()
                {
                    ProductTypeId = 3, ProductBrandId = 1, Description = "Suspendisse dui purus, scelerisque at, vulputate vitae, pretium mattis, nunc. Mauris eget neque at sem venenatis eleifend. Ut nonummy.",
                    Name = "Angular Blue Boots", Price = 180.00M,
                    PictureUrl = "images/products/boot-ang1.png"
                },
            };
        }
    }
}