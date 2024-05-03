
using Ecommerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


namespace Ecommerce.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options)
        {
               
            
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
       
       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasData(
                new Category 
                { 
                    Id = 1, 
                    Name = "Laptop", 
                    CreatedAt = new DateTime(2024, 3, 17), 
                    UpdatedAt = new DateTime(2023, 5, 7) 
                },
                new Category { Id = 2, Name = "Mobile", CreatedAt = new DateTime(2024, 3, 17), UpdatedAt = new DateTime(2023, 5, 7) },
                new Category { Id = 3, Name = "Headphones", CreatedAt = new DateTime(2024, 3, 17), UpdatedAt = new DateTime(2023, 5, 7) },
                new Category { Id = 4, Name = "Accessories", CreatedAt = new DateTime(2024, 3, 17), UpdatedAt = new DateTime(2023, 5, 7) }
                
                
                );

            modelBuilder.Entity<Product>().HasData(
               new Product { Id = 1, Name = "Banana Turning", Description = "Closed [endoscopic] biopsy of small intestine", Price = 483m, StockQuantity = 1, ImageUrl = "http://dummyimage.com/166x100.png/cc0000/ffffff", CreatedAt = DateTime.Parse("7/14/2023"), UpdatedAt = DateTime.Parse("3/19/2024") },
               new Product { Id = 2, Name = "Bay Leaf Fresh", Description = "Narcoanalysis", Price = 1335m, StockQuantity = 2, ImageUrl = "http://dummyimage.com/231x100.png/cc0000/ffffff", CreatedAt = DateTime.Parse("8/14/2023"), UpdatedAt = DateTime.Parse("5/7/2023") },
               new Product { Id = 3, Name = "Sprouts - Brussel", Description = "Arthrotomy for removal of prosthesis without replacement, elbow", Price = 3789m, StockQuantity = 3, ImageUrl = "http://dummyimage.com/218x100.png/5fa2dd/ffffff", CreatedAt = DateTime.Parse("10/31/2023"), UpdatedAt = DateTime.Parse("9/3/2023") }


                );







        }



    }
}
