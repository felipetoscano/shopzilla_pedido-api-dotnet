using Microsoft.EntityFrameworkCore;
using ShopZilla.Pedidos.Api.Entities;

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

        public PedidoEntity BuscarPedidoCompletoPorId(int id) => _dbContext.Pedidos.Include(p => p.Produtos).FirstOrDefault(p => p.Id == id);

        public void CriarPedido(PedidoEntity pedidoEntity) => _dbContext.Pedidos.Add(pedidoEntity);

        public void AlterarPedido(int id, PedidoEntity pedidoEntity) 
        {
            pedidoEntity.Id = id;

            _dbContext.Pedidos.Update(pedidoEntity);
        }

        public void DeletarPedido(int id) 
        {
            var pedidoEntity = new PedidoEntity { Id = id };

            _dbContext.Pedidos.Remove(pedidoEntity);
        } 

        public void SalvarAlteracoes() => _dbContext.SaveChanges();
    }
}
