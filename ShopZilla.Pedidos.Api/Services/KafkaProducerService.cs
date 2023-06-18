using Confluent.Kafka;
using ShopZilla.Pedidos.Api.Entities;
using ShopZilla.Pedidos.Api.Models;
using System.Text.Json;

namespace ShopZilla.Pedidos.Api.Services
{
    public class KafkaProducerService
    {
        private readonly ConnectionStrings _connectionStrings;

        public KafkaProducerService(ConnectionStrings connectionStrings)
        {
            _connectionStrings = connectionStrings;
        }

        public async void AdicionarTopicoNovoPedido(PedidoEntity pedido)
        {
            var config = ObterConfiguracaoConsumidor();

            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                    var pedidoSerializado = JsonSerializer.Serialize(pedido);
                    var mensagem = new Message<Null, string>() { Value = pedidoSerializado };
                    var response = await producer.ProduceAsync("NOVO_PEDIDO", mensagem);

                    Console.WriteLine("Registro da fila adicionado com sucesso");
                }
                catch (ProduceException<Null, string> e)
                {
                    Console.WriteLine($"Erro no envio: {e.Error.Reason}");
                }
            }
        }

        private ConsumerConfig ObterConfiguracaoConsumidor()
        {
            return new ConsumerConfig
            {
                BootstrapServers = _connectionStrings.Kafka
            };
        }
    }
}
