using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineSchop.OrderService.Models;

namespace OnlineSchop.OrderService.Data
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("orders");
                
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.ClientId)
                    .HasColumnName("client_id")
                    .IsRequired();

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasConversion<string>();

                entity.Property(e => e.TotalAmount)
                    .HasColumnName("total_amount")
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .IsRequired();

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at");
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.ToTable("order_items");
                
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.OrderId)
                    .HasColumnName("order_id")
                    .IsRequired();

                entity.Property(e => e.ProductId)
                    .HasColumnName("product_id")
                    .IsRequired();

                entity.Property(e => e.Quantity)
                    .HasColumnName("quantity")
                    .IsRequired();

                entity.Property(e => e.UnitPrice)
                    .HasColumnName("unit_price")
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                // Связь с заказом
                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}