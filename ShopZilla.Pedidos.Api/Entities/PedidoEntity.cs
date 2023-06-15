using System.ComponentModel.DataAnnotations.Schema;

namespace ShopZilla.Pedidos.Api.Entities
{
    public class PedidoEntity
    {
        public int Id { get; set; }
        public string Status { get; set; }
        [Column("ID_CLIENTE")]
        public int IdCliente { get; set; }
        public IList<ProdutoEntity> Produtos { get; set; }

        public bool FoiAprovado() => Status == "APROVADO";
        public bool FoiRecusado() => Status == "RECUSADO";
        public void CancelarPedido() => Status = "CANCELADO";
        public void EntregarPedido() => Status = "ENTREGAR";
    }
}
