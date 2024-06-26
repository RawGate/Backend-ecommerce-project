﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using backend_teamwork.EntityFramework;

#nullable disable

namespace sda_online_2_csharp_backend_teamwork1.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0-preview.3.24172.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Order", b =>
                {
                    b.Property<Guid>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("order_id");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date");

                    b.Property<string>("OrderStatus")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("status");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(10,2)")
                        .HasColumnName("total_price");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("OrderId");

                    b.HasIndex("UserId");

                    b.ToTable("orders");
                });

            modelBuilder.Entity("backend_teamwork.EntityFramework.OrderProduct", b =>
                {
                    b.Property<Guid>("OrderProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("order_product_id");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uuid")
                        .HasColumnName("order_id");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid")
                        .HasColumnName("product_id");

                    b.Property<int>("ProductQuantity")
                        .HasColumnType("integer")
                        .HasColumnName("product_quantity");

                    b.HasKey("OrderProductId");

                    b.HasIndex("ProductId");

                    b.HasIndex("OrderId", "ProductId")
                        .IsUnique();

                    b.ToTable("order_product");
                });

            modelBuilder.Entity("backend_teamwork.EntityFramework.Product", b =>
                {
                    b.Property<Guid>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("product_id");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uuid")
                        .HasColumnName("category_id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("createdat");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("image");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric")
                        .HasColumnName("price");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("slug");

                    b.Property<int>("SoldQuantity")
                        .HasColumnType("integer")
                        .HasColumnName("sold_quantity");

                    b.Property<int>("Stock")
                        .HasColumnType("integer")
                        .HasColumnName("stock");

                    b.HasKey("ProductId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("product");
                });

            modelBuilder.Entity("backend_teamwork.EntityFramework.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("address");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<bool>("IsBlocked")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.Property<int>("Phone")
                        .HasColumnType("integer")
                        .HasColumnName("phone");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("role");

                    b.HasKey("UserId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("backend_teamwork.Models.Category", b =>
                {
                    b.Property<Guid>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("category_id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("createdat");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<Guid?>("ProductId")
                        .HasColumnType("uuid");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("slug");

                    b.HasKey("CategoryId");

                    b.HasIndex("ProductId");

                    b.ToTable("category");
                });

            modelBuilder.Entity("Order", b =>
                {
                    b.HasOne("backend_teamwork.EntityFramework.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("backend_teamwork.EntityFramework.OrderProduct", b =>
                {
                    b.HasOne("Order", "Order")
                        .WithMany("OrderProducts")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("backend_teamwork.EntityFramework.Product", "Product")
                        .WithMany("OrderProducts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("backend_teamwork.EntityFramework.Product", b =>
                {
                    b.HasOne("backend_teamwork.Models.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("backend_teamwork.Models.Category", b =>
                {
                    b.HasOne("backend_teamwork.EntityFramework.Product", null)
                        .WithMany("Categories")
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("Order", b =>
                {
                    b.Navigation("OrderProducts");
                });

            modelBuilder.Entity("backend_teamwork.EntityFramework.Product", b =>
                {
                    b.Navigation("Categories");

                    b.Navigation("OrderProducts");
                });

            modelBuilder.Entity("backend_teamwork.EntityFramework.User", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("backend_teamwork.Models.Category", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
