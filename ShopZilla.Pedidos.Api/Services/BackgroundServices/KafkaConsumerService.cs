using Confluent.Kafka;
using ShopZilla.Pedidos.Api.Dal;
using ShopZilla.Pedidos.Api.Entities;
using ShopZilla.Pedidos.Api.Models;
using System.Text.Json;

namespace ShopZilla.Pedidos.Api.Services.BackgroundServices
{
    public class KafkaConsumerService : IHostedService
    {
        private readonly ConnectionStrings _connectionStrings;
        private readonly IServiceProvider _serviceProvider;

        public KafkaConsumerService(ConnectionStrings connectionStrings, IServiceProvider serviceProvider)
        {
            _connectionStrings = connectionStrings;
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _connectionStrings.Kafka,
                GroupId = "PEDIDOS"
            };

            var consumidor = new ConsumerBuilder<Ignore, string>(config).Build();
            consumidor.Subscribe("NOVO_PEDIDO");

            try
            {
                Task.Run(() =>
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        var mensagem = consumidor.Consume(cancellationToken);
                        var pedido = JsonSerializer.Deserialize<PedidoEntity>(mensagem.Message.Value);

                        pedido.Status = "APROVADO";

                        using var scope = _serviceProvider.CreateScope();
                        var pedidosDal = scope.ServiceProvider.GetRequiredService<PedidosDal>();
                        pedidosDal.AlterarPedido(pedido.Id, pedido);
                    }
                });
            }
            catch (OperationCanceledException)
            {
                consumidor.Close();
            }
            
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Fila parou");

            return Task.CompletedTask;
        }
    }
}
