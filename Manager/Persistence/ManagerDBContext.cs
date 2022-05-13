using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manager.Core.Entities;
using Microsoft.EntityFrameworkCore;


namespace Manager.Persistence
{
    public class ManagerDBContext : DbContext
    {
        
        public ManagerDBContext(DbContextOptions<ManagerDBContext> options) : base(options)
        {
        }
        //public DbSet<Vendedor> Users { get; set; }
        public DbSet<Vendedor> Vendedores { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<User>(e =>
            //{
            //    e.HasKey(u => u.Id);

            //});

            modelBuilder.Entity<Vendedor>(e =>
            {
                e.HasKey(v => v.Id);

                e.HasMany(v => v.Produtos)
                .WithOne()
                .HasForeignKey(p => p.VendedorForeignKey)
                .OnDelete(DeleteBehavior.Restrict);

                e.HasMany(v => v.Pedidos)
                .WithOne()
                .HasForeignKey(p => p.VendedorForeignKey)
                .OnDelete(DeleteBehavior.Restrict);

            });

            modelBuilder.Entity<Produto>(e =>
            {
                e.HasKey(p => p.Id);

                e.HasMany(p => p.Pedidos)
                .WithOne()
                .HasForeignKey(p => p.ProdutoForeignKey);

            });

            modelBuilder.Entity<Pedido>(e =>
            {
                e.HasKey(p => p.Id);

            });

        }

    }
}
