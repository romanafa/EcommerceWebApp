using EcommerceWebApp_API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EcommerceWebApp_API.Data
{

    // Using IdentityDbContext instead of DbContext as we agreed to use Identity for authentication and authorization
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Data seeding
            //TODO: Seed product categories when API testing is functional 
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    ProductId = 1,
                    ProductTitle = "Headband - Easter",
                    Description = "Handmade headband",
                    Price = 200.00,
                    Stock = 10,
                    Colour = "Multi",
                    Size = "Onesize",
                    IsActive = true,
                    Image = "https://ecommerceproductsimages.blob.core.windows.net/images/IMG_8882.JPG",
                    CreatedAt = new System.DateTime(2024, 1, 29)
                },
                new Product
                {
                    ProductId = 2,
                    ProductTitle = "Headband - Pinky",
                    Description = "Handmade headband",
                    Price = 200.00,
                    Stock = 10,
                    Colour = "Multi",
                    Size = "Onesize",
                    IsActive = true,
                    Image = "https://ecommerceproductsimages.blob.core.windows.net/images/IMG_8884.JPG",
                    CreatedAt = new System.DateTime(2024, 1, 29)
                },
                new Product
                {
                    ProductId = 3,
                    ProductTitle = "Headband - Summer",
                    Description = "Handmade headband",
                    Price = 200.00,
                    Stock = 10,
                    Colour = "Multi",
                    Size = "Onesize",
                    IsActive = true,
                    Image = "https://ecommerceproductsimages.blob.core.windows.net/images/IMG_8886.JPG",
                    CreatedAt = new System.DateTime(2024, 1, 29)
                });


            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    CategoryId = 1,
                    CategoryTitle = "Accessories",
                    IsActive = true
                },
                new Category
                {
                    CategoryId = 2,
                    CategoryTitle = "Decorations",
                    IsActive = true
                },
                new Category
                {
                    CategoryId = 3,
                    CategoryTitle = "Magnets",
                    IsActive = true
                }
                );

        }
    }
}
