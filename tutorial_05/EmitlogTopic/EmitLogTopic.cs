﻿using System;
using System.Text;
using System.Linq;
using RabbitMQ.Client;

namespace EmitlogTopic
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            using (var connetion = factory.CreateConnection())
            using (var channel = connetion.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "topic_logs", type: "topic");
                
                var routingKey = (args.Length > 0) ? args[0] : "anonymous.info";
                var message = (args.Length > 1) ? string.Join(" ",args.Skip( 1 ).ToArray()) : "Hello World!";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "topic_logs", routingKey: routingKey, basicProperties: null, body: body);

                Console.WriteLine($" [x] Sent {routingKey} : {message}");
            }
        }
    }
}
