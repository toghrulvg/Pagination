using EntityFrameworkProject.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace EntityFrameworkProject.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
        }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<SliderDetail> SliderDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Social> Socials { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected  override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>().HasQueryFilter(m => !m.IsDeleted);
            modelBuilder.Entity<Setting>().HasQueryFilter(m => !m.IsDeleted);
            modelBuilder.Entity<Social>().HasQueryFilter(m => !m.IsDeleted);
          
            modelBuilder.Entity<Blog>().HasData(
                    new Blog
                    {
                        Id = 1,
                        IsDeleted = false,
                        Title = "Blog1",
                        Desc = "Desc1",
                        Image = "blog-feature-img-1.jpg",
                        Date = DateTime.Now
                    },
                    new Blog
                    {
                        Id = 2,
                        IsDeleted = false,
                        Title = "Blog2",
                        Desc = "Desc2",
                        Image = "blog-feature-img-3.jpg",
                        Date = DateTime.Now
                    },
                    new Blog
                    {
                        Id = 3,
                        IsDeleted = false,
                        Title = "Blog3",
                        Desc = "Desc3",
                        Image = "blog-feature-img-4.jpg",
                        Date = DateTime.Now
                    }

            );

            modelBuilder.Entity<Setting>().HasData(
                new Setting
                {
                    Id = 1,
                    IsDeleted = false,
                    Key = "HeaderLogo",
                    Value= "logo.png"

                },
                new Setting
                {
                    Id = 2,
                    IsDeleted = false,
                    Key = "Phone",
                    Value = "535462453645"
                },
                new Setting
                {
                    Id = 3,
                    IsDeleted = false,
                    Key = "ProductTake",
                    Value = "4"
                },
                new Setting
                {
                    Id = 4,
                    IsDeleted = false,
                    Key = "Email",
                    Value = "p130@code.edu.az"
                }

          );
            modelBuilder.Entity<Social>().HasData(
               new Social
               {
                   Id = 1,
                   IsDeleted = false,
                   Name = "Facebook",
                   Url = "https://www.facebook.com/"

               },
               new Social
               {
                   Id = 2,
                   IsDeleted = false,
                   Name = "Instagram",
                   Url = "https://www.instagram.com/"
               }

         );
        }



    }
}
