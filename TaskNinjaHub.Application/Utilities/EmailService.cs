using System.Text;
using TaskNinjaHub.Application.Interfaces;
using TaskNinjaHub.Application.Entities.Tasks.Domain;
using RabbitMQ.Client;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace TaskNinjaHub.Application.Utilities;

public class EmailService : IEmailService
{
    private readonly ConnectionFactory _factory = new()
    {
        HostName = "54.39.207.182",
        Port = 5673
    };

    public async Task SendCreateEmailAsync(CatalogTask task)
    {
        using (var connection = _factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            var senderDataJson = JsonSerializer.Serialize(task);
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
            var senderDataJson = JsonSerializer.Serialize(task);
            var body = Encoding.UTF8.GetBytes(senderDataJson);

            channel.BasicPublish(exchange: "UpdateEmailQueue",
                routingKey: "UpdateEmailQueue",
                basicProperties: null,
                body: body);
        }
    }
}