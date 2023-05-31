namespace ShopZilla.Pedidos.Api.Entidades
{
    public class PedidoEntity
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public IList<ProdutoEntity> Produtos { get; set; }
    }
}
