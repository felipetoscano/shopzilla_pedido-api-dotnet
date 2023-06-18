using ShopZilla.Pedidos.Api.Entities;

namespace ShopZilla.Pedidos.Api.Pedidos
{
    public interface IPedido
    {
        public IPedido Proximo { get; set; }
        public PedidoEntity Processar(PedidoEntity pedido);
    }
}
