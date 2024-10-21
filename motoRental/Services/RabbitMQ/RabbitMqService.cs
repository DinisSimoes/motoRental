using motoRental.Interfaces;
using motoRental.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace motoRental.Services.RabbitMQ
{
    public class RabbitMqService : IMotoEventPublisher
    {
        private readonly IConnection _rabbitConnection;
        private const string ExchangeName = "moto_cadastrada";

        public RabbitMqService(IConnection rabbitConnection)
        {
            _rabbitConnection = rabbitConnection;
        }

        public void PublishMotoAddedEvent(Moto moto)
        {
            using var channel = _rabbitConnection.CreateModel();
            channel.ExchangeDeclare(exchange: ExchangeName, type: ExchangeType.Fanout);

            var message = JsonConvert.SerializeObject(moto);
            var body = Encoding.UTF8.GetBytes(message);


        }
    }
}
