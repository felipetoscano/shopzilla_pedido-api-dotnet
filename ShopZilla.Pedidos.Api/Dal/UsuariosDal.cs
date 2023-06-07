using ShopZilla.Pedidos.Api.Entities;

namespace ShopZilla.Pedidos.Api.Dal
{
    public class UsuariosDal
    {
        private readonly PedidosDbContext _dbContext;

        public UsuariosDal(PedidosDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public UsuarioEntity BuscarPorLoginESenha(string login, string senha)
        {
            return _dbContext.Usuarios.FirstOrDefault(usuario => usuario.Login == login && usuario.Senha == senha);
        }
    }
}
