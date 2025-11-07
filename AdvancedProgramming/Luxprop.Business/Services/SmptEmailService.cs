using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

public interface IEmailService
{
    Task SendAsync(string to, string subject, string htmlBody, CancellationToken ct = default);
}

public class SmtpEmailService : IEmailService
{
    private readonly IConfiguration _cfg;
    public SmtpEmailService(IConfiguration cfg) => _cfg = cfg;

    public async Task SendAsync(string to, string subject, string htmlBody, CancellationToken ct = default)
    {
        using var msg = new MailMessage();
        msg.From = new MailAddress(_cfg["Smtp:From"]!);
        msg.To.Add(to);
        msg.Subject = subject;
        msg.Body = htmlBody;
        msg.IsBodyHtml = true;

        using var client = new SmtpClient(_cfg["Smtp:Host"])
        {
            Port = int.Parse(_cfg["Smtp:Port"]!),
            Credentials = new NetworkCredential(_cfg["Smtp:User"], _cfg["Smtp:Pass"]),
            EnableSsl = true
        };
        await client.SendMailAsync(msg, ct);
    }
}
