using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIRabbitMQ.Models
{
    public class RabbitMqConfiguration
    {
        public string Host { get; set; }
        public string Queue { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
