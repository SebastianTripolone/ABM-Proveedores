using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ProveedorEntity.Models
{
    public partial class ProveedorDatosContext : DbContext
    {
        public ProveedorDatosContext()
        {
        }

        public ProveedorDatosContext(DbContextOptions<ProveedorDatosContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Proveedor> Proveedors { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("server=DESKTOP-K7EBAL8\\SQLEXPRESS; Database=ProveedorDatos; integrated security=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Proveedor>(entity =>
            {
                entity.ToTable("Proveedor");

                entity.Property(e => e.Telefono).HasMaxLength(20);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
