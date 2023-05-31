using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ShopZilla.Pedidos.Api.Entidades
{
    public class ProdutoEntity
    {
        public int Id { get; set; }
        public string Sku { get; set; }
        public int Quantidade { get; set;}
        [JsonIgnore]
        [Column("ID_PEDIDO")]
        public int IdPedido { get; set; }
        [JsonIgnore]
        public PedidoEntity Pedido { get; set; }
    }
}
