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
        public DbSet<ProductCategory> ProductCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductCategory>()
                .HasKey(pc => new { pc.ProductId, pc.CategoryId });

            modelBuilder.Entity<ProductCategory>()
                .HasOne(pc => pc.Product)
                .WithMany(p => p.ProductCategories)
                .HasForeignKey(pc => pc.ProductId);

            modelBuilder.Entity<ProductCategory>()
                .HasOne(pc => pc.Category)
                .WithMany(c => c.ProductCategories)
                .HasForeignKey(pc => pc.CategoryId);

            // Data seeding

            // Seed products
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

            // Seed categories
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

            // Seed product categories
            modelBuilder.Entity<ProductCategory>().HasData(
                new ProductCategory
                {
                    ProductId = 1,
                    CategoryId = 1
                },
                new ProductCategory
                {
                    ProductId = 2,
                    CategoryId = 1
                },
                new ProductCategory
                {
                    ProductId = 3,
                    CategoryId = 1
                }
                );
        }
    }
}
