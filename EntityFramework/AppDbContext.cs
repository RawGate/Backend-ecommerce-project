using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_teamwork.Models;
using Microsoft.EntityFrameworkCore;
namespace backend_teamwork.EntityFramework
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { set; get; }
        public DbSet<OrderProduct> OrderProducts { set; get; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //Category-Product-Relation-Start
            modelBuilder.Entity<Category>()
            .HasMany(category => category.Products)
            .WithOne(c => c.Category)
            .HasForeignKey(c => c.CategoryId);

            //Category-Product-Relation-End


            // ProductEntityRoles - Start
            modelBuilder.Entity<Product>()
            .HasIndex(p => p.Name)
            .IsUnique();
            // ProductEntityRoles - End


            // -----Configure the User entity for database mapping-----
            modelBuilder.Entity<User>()
            .ToTable("users")
            .HasKey(user => user.UserId);


            modelBuilder.Entity<User>()
           .HasKey(_ => _.UserId);


            modelBuilder.Entity<User>()
            .Property(user => user.UserId)
            .HasColumnName("user_id")
            .IsRequired()
            .ValueGeneratedOnAdd();


            modelBuilder.Entity<User>()
            .Property(user => user.Role)
            .HasColumnName("role")
            .IsRequired();

            modelBuilder.Entity<User>()
            .Property(user => user.Name)
            .HasColumnName("name")
            .IsRequired();

            modelBuilder.Entity<User>()
            .Property(user => user.Address)
            .HasColumnName("address");

            modelBuilder.Entity<User>()
            .Property(user => user.Email)
            .HasColumnName("email")
            .IsRequired();

            modelBuilder.Entity<User>()
            .HasIndex(user => user.Email)
            .IsUnique();

            modelBuilder.Entity<User>()
            .Property(user => user.Password)
            .HasColumnName("password")
            .IsRequired();

            modelBuilder.Entity<User>()
            .Property(user => user.Phone)
            .HasColumnName("phone")
           .IsRequired();


            // --------------- Orders Constraints ------------------

            modelBuilder.Entity<Order>()
            .HasKey(_ => _.OrderId);

            modelBuilder.Entity<Order>()
            .Property(_ => _.OrderId)
            .ValueGeneratedOnAdd();

            modelBuilder.Entity<Order>()
            .Property(_ => _.TotalPrice).HasColumnType("decimal(10,2)")
            .IsRequired();

            modelBuilder.Entity<Order>()
            .Property(_ => _.OrderStatus)
            .IsRequired();


            //----------- Orders and Users Relation 1-M -------------

            modelBuilder.Entity<Order>()
            .HasOne(_ => _.User)
            .WithMany(_ => _.Orders)
            .HasForeignKey(_ => _.UserId);

            modelBuilder.Entity<Order>()
            .Property(_ => _.UserId)
            .IsRequired();


            //---------- Orders and Products Relation M-M -----------

            modelBuilder.Entity<OrderProduct>()
            .HasKey(_ => _.OrderProductId);

            modelBuilder.Entity<OrderProduct>()
            .HasOne(_ => _.Order)
            .WithMany(_ => _.OrderProducts)
            .HasForeignKey(_ => _.OrderId);

            modelBuilder.Entity<OrderProduct>()
            .HasOne(_ => _.Product)
            .WithMany(_ => _.OrderProducts)
            .HasForeignKey(_ => _.ProductId);

            modelBuilder.Entity<OrderProduct>()
           .Property(_ => _.ProductQuantity)
           .IsRequired();

            modelBuilder.Entity<OrderProduct>()
            .HasIndex(_ => new { _.OrderId, _.ProductId })
            .IsUnique();
        }
    }
}