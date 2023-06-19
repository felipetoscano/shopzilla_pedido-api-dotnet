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
        private readonly KafkaSettings _kafkaSettings;
        private readonly IServiceProvider _serviceProvider;

        public KafkaConsumerService(ConnectionStrings connectionStrings, KafkaSettings kafkaSettings, IServiceProvider serviceProvider)
        {
            _connectionStrings = connectionStrings;
            _kafkaSettings = kafkaSettings;
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            ConsumirTopicoConfirmacaoPedido(cancellationToken);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Fila parou");

            return Task.CompletedTask;
        }

        private void ConsumirTopicoConfirmacaoPedido(CancellationToken cancellationToken)
        {
            var config = ObterConfiguracaoConsumidor();
            var consumidor = ObterConsumidorTopicoConfirmacaoPedido(config);

            try
            {
                Console.WriteLine("Consumo iniciado");
                CriarTarefaDeConsumoTopico(consumidor, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                consumidor.Close();
            }
        }

        private ConsumerConfig ObterConfiguracaoConsumidor()
        {
            return new ConsumerConfig
            {
                BootstrapServers = _connectionStrings.Kafka,
                GroupId = _kafkaSettings.GroupId
            };
        }

        private IConsumer<Ignore, string> ObterConsumidorTopicoConfirmacaoPedido(ConsumerConfig config)
        {
            var consumidor = new ConsumerBuilder<Ignore, string>(config).Build();
            consumidor.Subscribe(_kafkaSettings.Topics.ConfirmacaoPedido);

            return consumidor;
        }

        private void CriarTarefaDeConsumoTopico(IConsumer<Ignore, string> consumidor, CancellationToken cancellationToken)
        {
            Task.Run(() =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var mensagem = consumidor.Consume(cancellationToken);
                    var pedido = JsonSerializer.Deserialize<PedidoEntity>(mensagem.Message.Value);
                    var pedidoProcessado = new ProcessadorPedidos().Processar(pedido);

                    AlterarPedido(pedidoProcessado);

                    Console.WriteLine("Registro da fila consumido com sucesso");
                }
            }, CancellationToken.None);
        }

        private void AlterarPedido(PedidoEntity pedido)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var pedidosDal = scope.ServiceProvider.GetRequiredService<PedidosDal>();
                pedidosDal.AlterarPedido(pedido.Id, pedido);
                pedidosDal.SalvarAlteracoes();
            }
        }
    }
}
