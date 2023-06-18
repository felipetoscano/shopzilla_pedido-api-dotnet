using ShopZilla.Pedidos.Api.Entities;

namespace ShopZilla.Pedidos.Api.Pedidos
{
    public class PedidoPadrao : IPedido
    {
        public IPedido Proximo { get; set; }

        public PedidoEntity Processar(PedidoEntity pedido)
        {
            return pedido;
        }
    }
}
