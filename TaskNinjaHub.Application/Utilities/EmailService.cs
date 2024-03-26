using System.Net.Mail;
using System.Net;
using TaskNinjaHub.Application.Interfaces;
using TaskNinjaHub.Application.Entities.Tasks.Domain;

namespace TaskNinjaHub.Application.Utilities;

public class EmailService : IEmailService
{
    private const string SmtpServer = "smtp.mail.ru";

    private const int SmtpPort = 587;

    private const string SmtpUsername = "zhurnal.kuratora@mail.ru";

    private const string SmtpPassword = "wuxREz6iDKYuegwCQuEm";

    private readonly SmtpClient _smtpClient = new(SmtpServer, SmtpPort)
    {
        UseDefaultCredentials = false,
        Credentials = new NetworkCredential(SmtpUsername, SmtpPassword),
        EnableSsl = true
    };

    public async Task SendCreateEmailAsync(string to, CatalogTask task)
    {
        var htmlBody = ConstructTaskAssignmentEmailBody(task);

        var message = new MailMessage
        {
            From = new MailAddress("zhurnal.kuratora@mail.ru"),
            Subject = "You have been assigned a task",
            Body = htmlBody,
            IsBodyHtml = true
        };
        message.To.Add(to);

        await _smtpClient.SendMailAsync(message);
    }

    public async Task SendUpdateEmailAsync(string to, CatalogTask task)
    {
        var htmlBody = ConstructTaskAssignmentEmailBody(task);

        var message = new MailMessage
        {
            From = new MailAddress("zhurnal.kuratora@mail.ru"),
            Subject = "The task you were assigned to has been updated",
            Body = htmlBody,
            IsBodyHtml = true
        };
        message.To.Add(to);

        await _smtpClient.SendMailAsync(message);
    }

    private string ConstructTaskAssignmentEmailBody(CatalogTask task)
    {
        var htmlBody = @"
    <!DOCTYPE html>
    <html lang=""en"">
    <head>
        <meta charset=""UTF-8"">
        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
        <title>Task</title>
        <style>
            .task-container {
                width: 90%;
                max-width: 600px;
                margin: 0 auto;
                border-radius: 15px;
                background-color: #fff;
                box-shadow: 0px 0px 10px 0px rgba(0,0,0,0.5);
                padding: 20px;
            }
            .task-info {
                text-align: center;
            }
            .task-info img {
                display: block;
                margin: 0 auto 10px;
                max-width: 40%;
            }
            .task-info h2 {
                margin-bottom: 10px;
            }
            .task-info p {
                margin-bottom: 10px;
            }
            .task-info button {
                display: block;
                width: 100%;
                padding: 10px;
                border: none;
                border-radius: 5px;
                background-color: #007bff;
                color: #fff;
                cursor: pointer;
                text-decoration: none;
            }
        </style>
    </head>
    <body>
        <div class=""task-container"">
            <div class=""task-info"">
                <img src=""https://sandme.ru/file-storage/api/File/GetFile?bucketName=task-files&fileName=HubLogoPNG.png"" alt=""Logo"">
                <h2>" + task.Name + @"</h2>
                <p>Task executor: " + task.TaskAuthor?.Name + @"</p>
                <p>Priority: " + task.Priority?.Name + @"</p>
                <p>Information system: " + task.InformationSystem?.Name + @"</p>
                <p>Task status: " + task.TaskStatus?.Name + @"</p>
                <a style=""text-decoration: none;"" href=""https://sandme.ru/task-read/" + task.Id + @"""><button>View task</button></a>
            </div>
        </div>
    </body>
    </html>";

        return htmlBody;
    }
}