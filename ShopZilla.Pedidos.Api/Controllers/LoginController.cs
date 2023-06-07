using Microsoft.AspNetCore.Mvc;
using ShopZilla.Pedidos.Api.Dal;
using ShopZilla.Pedidos.Api.Entities;
using ShopZilla.Pedidos.Api.Services;

namespace ShopZilla.Pedidos.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly UsuariosDal _usuariosDal;
        private readonly TokenService _tokenService;

        public LoginController(UsuariosDal usuariosDal, TokenService tokenService)
        {
            _usuariosDal = usuariosDal;
            _tokenService = tokenService;
        }

        [HttpPost]
        public ActionResult Autenticar([FromBody] UsuarioEntity usuario)
        {
            var usuarioVerificado = _usuariosDal.BuscarPorLoginESenha(usuario.Login, usuario.Senha);

            if(usuarioVerificado == null)
            {
                return NotFound("Usuário não encontrado");
            }

            var token = _tokenService.GerarTokenDoUsuario(usuarioVerificado);

            return Ok(new { TokenJwt = token });
        }
    }
}
