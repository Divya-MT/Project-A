using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DeliveryManagementSystemApi.Models
{
    public partial class DeliveryManagementSystemContext : DbContext
    {
        public DeliveryManagementSystemContext()
        {
        }

        public DeliveryManagementSystemContext(DbContextOptions<DeliveryManagementSystemContext> options)
            : base(options)
        {
        }

        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<Registration> Registrations { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=YKG-PC;Database=DeliveryManagementSystem;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => e.OrderId)
                    .HasName("PK__OrderDet__F1E4607BF8A99CE5");

                entity.Property(e => e.OrderId).HasColumnName("Order_Id");

                entity.Property(e => e.DeliveryDate).HasColumnType("datetime");

                entity.Property(e => e.Price).HasColumnType("decimal(9, 2)");

                entity.Property(e => e.TimeOfPickup).HasColumnType("datetime");

                entity.Property(e => e.WeightOfPackage).HasColumnType("decimal(9, 3)");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.OrderDetailCustomers)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderDeta__Custo__2CF2ADDF");

                entity.HasOne(d => d.Executive)
                    .WithMany(p => p.OrderDetailExecutives)
                    .HasForeignKey(d => d.ExecutiveId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderDeta__Execu__2DE6D218");
            });

            modelBuilder.Entity<Registration>(entity =>
            {
                entity.ToTable("Registration");

                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNo)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.RegistrationType)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
