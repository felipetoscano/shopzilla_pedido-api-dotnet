using Microsoft.EntityFrameworkCore;
using ShopZilla.Pedidos.Api.Entities;

namespace ShopZilla.Pedidos.Api
{
    public class PedidosDbContext : DbContext
    {
        public DbSet<PedidoEntity> Pedidos { get; set; }
        public DbSet<ProdutoEntity> Produtos { get; set; }
        public DbSet<UsuarioEntity> Usuarios { get; set; }

        public PedidosDbContext(DbContextOptions<PedidosDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PedidoEntity>()
                .HasMany(e => e.Produtos)
                .WithOne(e => e.Pedido)
                .HasForeignKey(e => e.IdPedido)
                .HasPrincipalKey(e => e.Id);
        }  
    }
}
