using System.Text;
using Newtonsoft.Json;
using TaskNinjaHub.Application.Interfaces;
using TaskNinjaHub.Application.Entities.Tasks.Domain;
using RabbitMQ.Client;

namespace TaskNinjaHub.Application.Utilities;

public class EmailService : IEmailService
{
    readonly ConnectionFactory _factory = new ConnectionFactory()
    {
        HostName = "54.39.207.182",
        Port = 5673
    };
    public async Task SendCreateEmailAsync(CatalogTask task)
    {
        using (var connection = _factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            var senderDataJson = JsonConvert.SerializeObject(task);
            var body = Encoding.UTF8.GetBytes(senderDataJson);

            channel.BasicPublish(exchange: "CreateEmailQueue",
                routingKey: "CreateEmailQueue",
                basicProperties: null,
                body: body);
        }
    }

    public async Task SendUpdateEmailAsync(CatalogTask task)
    {
        using (var connection = _factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            var senderDataJson = JsonConvert.SerializeObject(task);
            var body = Encoding.UTF8.GetBytes(senderDataJson);

            channel.BasicPublish(exchange: "UpdateEmailQueue",
                routingKey: "UpdateEmailQueue",
                basicProperties: null,
                body: body);
        }
    }
}