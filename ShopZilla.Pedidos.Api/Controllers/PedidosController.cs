using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopZilla.Pedidos.Api.Dal;
using ShopZilla.Pedidos.Api.Entities;
using ShopZilla.Pedidos.Api.Services;

namespace ShopZilla.Pedidos.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly PedidosDal _pedidosDal;
        private readonly KafkaProducerService _kafkaService;

        public PedidosController(PedidosDal pedidosDal, KafkaProducerService kafkaService)
        {
            _pedidosDal = pedidosDal;
            _kafkaService = kafkaService;
        }

        /// <summary>
        /// Busca todos os pedidos de forma simplificada
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<PedidoEntity[]> BuscarPedidos()
        {
            var pedidos = _pedidosDal.BuscarPedidos();

            return Ok(pedidos);
        }

        /// <summary>
        /// Busca um pedido completo por Id
        /// </summary>
        /// <param name="id">Id do pedido</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<PedidoEntity> BuscarPedidoCompletoPorId([FromRoute] int id)
        {
            var pedido = _pedidosDal.BuscarPedidoCompletoPorId(id);

            return Ok(pedido);
        }

        /// <summary>
        /// Cria um novo pedido com seus produtos
        /// </summary>
        /// <param name="pedido">Objeto representando o pedido e seus produtos</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CriarPedido([FromBody] PedidoEntity pedido)
        {
            _pedidosDal.CriarPedido(pedido);
            _pedidosDal.SalvarAlteracoes();

            _kafkaService.AdicionarTopicoNovoPedido(pedido);

            return Ok();
        }

        /// <summary>
        /// Altera um pedido e seus produtos pelo seu Id
        /// </summary>
        /// <param name="id">Id do pedido</param>
        /// <param name="pedido">Objeto representando o pedido e seus produtos</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public ActionResult AlterarPedido([FromRoute] int id, [FromBody] PedidoEntity pedido)
        {
            _pedidosDal.AlterarPedido(id, pedido);
            _pedidosDal.SalvarAlteracoes();

            return Ok();
        }

        /// <summary>
        /// Deleta um pedido pelo seu Id
        /// </summary>
        /// <param name="id">Id do pedido</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public ActionResult DeletarPedido([FromRoute] int id)
        {
            _pedidosDal.DeletarPedido(id);
            _pedidosDal.SalvarAlteracoes();

            return Ok();
        }
    }
}