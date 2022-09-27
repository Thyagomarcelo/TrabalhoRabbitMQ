using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIRabbitMQ.Models
{
    public class MensagemModel
    {

        public int FromId { get; set; }
        public int ToId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
