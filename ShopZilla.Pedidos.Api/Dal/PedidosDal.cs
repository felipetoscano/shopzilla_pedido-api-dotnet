using Microsoft.EntityFrameworkCore;
using ShopZilla.Pedidos.Api.Entidades;

namespace ShopZilla.Pedidos.Api.Dal
{
    public class PedidosDal
    {
        private readonly PedidosDbContext _dbContext;

        public PedidosDal(PedidosDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public PedidoEntity[] BuscarPedidos() => _dbContext.Pedidos.ToArray();
        public PedidoEntity BuscarPedidoCompletoPorId(int id) => _dbContext.Pedidos.Include(p => p.Produtos).First(p => p.Id == id);
        public void CriarPedido(PedidoEntity pedidoEntity) 
        {
            _dbContext.Pedidos.Add(pedidoEntity);
            _dbContext.SaveChanges();
        } 
        public void AlterarPedido(int id, PedidoEntity pedidoEntity) 
        {
            pedidoEntity.Id = id;

            _dbContext.Pedidos.Update(pedidoEntity);
            _dbContext.SaveChanges();
        }
        public void DeletarPedido(int id) 
        {
            var pedidoEntity = new PedidoEntity { Id = id };

            _dbContext.Pedidos.Remove(pedidoEntity);
            _dbContext.SaveChanges();
        } 
    }
}
