using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using bill.Models;

namespace bill.Context
{
    public partial class BillDbContext : DbContext
    {
        public BillDbContext()
        {
        }

        public BillDbContext(DbContextOptions<BillDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<item> items { get; set; } = null!;
        public virtual DbSet<list> lists { get; set; } = null!;
        public virtual DbSet<receipt> receipts { get; set; } = null!;
        public virtual DbSet<unit> units { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=localhost;port=10000;database=bill;uid=root;password=@Ae15032544", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.29-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<item>(entity =>
            {
                entity.HasKey(e => e.item_id)
                    .HasName("PRIMARY");

                entity.ToTable("item");

                entity.HasIndex(e => e.item_code, "item_code_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.item_name, "item_name_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.item_unit_id, "item_unit_id_idx");

                entity.Property(e => e.item_code).HasMaxLength(45);

                entity.Property(e => e.item_name).HasMaxLength(45);

                entity.Property(e => e.item_price).HasPrecision(10, 2);

                entity.HasOne(d => d.item_unit)
                    .WithMany(p => p.items)
                    .HasForeignKey(d => d.item_unit_id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("item_unit_id");
            });

            modelBuilder.Entity<list>(entity =>
            {
                entity.HasKey(e => e.list_id)
                    .HasName("PRIMARY");

                entity.ToTable("list");

                entity.HasIndex(e => e.list_bill_id, "list_bill_id_idx");

                entity.HasIndex(e => e.list_item_id, "list_item_id_idx");

                entity.Property(e => e.list_discount).HasPrecision(10, 2);

                entity.Property(e => e.list_discount_bath).HasPrecision(10, 2);

                entity.Property(e => e.list_price).HasPrecision(10, 2);

                entity.Property(e => e.list_quantity).HasPrecision(10, 2);

                entity.Property(e => e.list_total_price).HasPrecision(10, 2);

                entity.HasOne(d => d.list_bill)
                    .WithMany(p => p.lists)
                    .HasForeignKey(d => d.list_bill_id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("list_bill_id");

                entity.HasOne(d => d.list_item)
                    .WithMany(p => p.lists)
                    .HasForeignKey(d => d.list_item_id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("list_item_id");
            });

            modelBuilder.Entity<receipt>(entity =>
            {
                entity.HasKey(e => e.receipt_id)
                    .HasName("PRIMARY");

                entity.ToTable("receipt");

                entity.HasIndex(e => e.receipt_code, "bill_code_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.receipt_code).HasMaxLength(45);

                entity.Property(e => e.receipt_discount).HasPrecision(10, 2);

                entity.Property(e => e.receipt_product_discount).HasPrecision(10, 2);

                entity.Property(e => e.receipt_product_price).HasPrecision(10, 2);

                entity.Property(e => e.receipt_total_price).HasPrecision(10, 2);
            });

            modelBuilder.Entity<unit>(entity =>
            {
                entity.HasKey(e => e.unit_id)
                    .HasName("PRIMARY");

                entity.ToTable("unit");

                entity.HasIndex(e => e.unit_name, "unit_name_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.unit_name).HasMaxLength(45);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
