using ShopZilla.Pedidos.Api.Entities;
using ShopZilla.Pedidos.Api.Pedidos;

namespace ShopZilla.Pedidos.Api
{
    public class ProcessadorPedidos
    {
        public PedidoEntity Processar(PedidoEntity pedido)
        {
            var cadeia = new PedidoAprovado(new PedidoRecusado(new PedidoPadrao()));

            return cadeia.Processar(pedido);
        }
    }
}
