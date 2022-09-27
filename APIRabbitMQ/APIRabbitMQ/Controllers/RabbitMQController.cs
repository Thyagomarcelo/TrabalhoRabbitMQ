using APIRabbitMQ.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIRabbitMQ.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class RabbitMQController : ControllerBase
    {

        private readonly ConnectionFactory _connectionFactory;
        private const string QUEUE_NAME = "messages";

        public RabbitMQController()
        {
            _connectionFactory = new ConnectionFactory
            {
                HostName = "localhost"
            };
        }

        [HttpPost]
        public AcceptedResult SendRabbitMQ(MensagemModel mensagem)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: QUEUE_NAME,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    var stringfiedMessage = JsonConvert.SerializeObject(mensagem);
                    var bytesMessage = Encoding.UTF8.GetBytes(stringfiedMessage);

                    channel.BasicPublish(
                        exchange: "",
                        routingKey: QUEUE_NAME,
                        basicProperties: null,
                        body: bytesMessage);
                }
            }

            return Accepted();
        }
    }
}
