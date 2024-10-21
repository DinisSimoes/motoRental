using Microsoft.EntityFrameworkCore;
using motoRental.Data;
using motoRental.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace motoRental.Consumers
{
    public class MotoCadastradaConsumer
    {
        private readonly ApplicationDbContext _context;
        private readonly IConnection _rabbitConnection;

        public MotoCadastradaConsumer(ApplicationDbContext context, IConnection rabbitConnection)
        {
            _context = context;
            _rabbitConnection = rabbitConnection;
        }

        public void StartConsumer()
        {
            using var channel = _rabbitConnection.CreateModel();
            channel.ExchangeDeclare(exchange: "moto_cadastrada", type: ExchangeType.Fanout);

            var queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queue: queueName, exchange: "moto_cadastrada", routingKey: "");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var moto = JsonConvert.DeserializeObject<Moto>(message);

                if (moto.Ano == 2024)
                {
                    // Armazena a moto no banco de dados para consulta futura
                    _context.Motos.Add(moto);
                    await _context.SaveChangesAsync();
                }
            };

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }
    }
}
