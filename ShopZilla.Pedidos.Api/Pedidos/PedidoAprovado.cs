﻿using ShopZilla.Pedidos.Api.Entities;
using ShopZilla.Pedidos.Api.Models.Consts;

namespace ShopZilla.Pedidos.Api.Pedidos
{
    public class PedidoAprovado : IPedido
    {
        public IPedido Proximo { get; set; }

        public PedidoAprovado(IPedido proximo)
        {
            Proximo = proximo;
        }

        public PedidoEntity Processar(PedidoEntity pedido)
        {
            if (pedido.Status == StatusPedido.APROVADO)
            {
                pedido.Status = StatusPedido.ENTREGAR;
                return pedido;
            }

            return Proximo.Processar(pedido);
        }
    }
}
