using ShopZilla.Pedidos.Api.Entities;
using ShopZilla.Pedidos.Api.Models.Consts;

namespace ShopZilla.Pedidos.Api.Pedidos
{
    public class PedidoRecusado : IPedido
    {
        public IPedido Proximo { get; set; }

        public PedidoRecusado(IPedido proximo)
        {
            Proximo = proximo;
        }

        public PedidoEntity Processar(PedidoEntity pedido)
        {
            if (pedido.Status == StatusPedido.RECUSADO)
            {
                pedido.Status = StatusPedido.CANCELADO;
                return pedido;
            }

            return Proximo.Processar(pedido);
        }
    }
}
