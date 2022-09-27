using APIRabbitMQ.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace APIRabbitMQ.Consumers
{
    public class ProcessMessageConsumer : BackgroundService
    {

        private readonly RabbitMqConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public ProcessMessageConsumer(IOptions<RabbitMqConfiguration> option)
        {
            _configuration = option.Value;
            var factory = new ConnectionFactory
            {
                HostName = _configuration.Host
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(
                queue: _configuration.Queue,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (sender, EventArgs) =>
            {
                var contentArray = EventArgs.Body.ToArray();
                var contentString = Encoding.UTF8.GetString(contentArray);
                var message = JsonConvert.DeserializeObject<MensagemModel>(contentString);

                _channel.BasicAck(EventArgs.DeliveryTag, false);
            };

            _channel.BasicConsume(_configuration.Queue, false, consumer);

            return Task.CompletedTask;
        }
    }
}
