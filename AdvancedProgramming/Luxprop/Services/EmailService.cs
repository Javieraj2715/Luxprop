using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace Luxprop.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        private readonly string Host;
        private readonly int Port;
        private readonly string User;
        private readonly string Pass;
        private readonly string From;

        public EmailService(IConfiguration config)
        {
            _config = config;

            Host = _config["Smtp:Host"]!;
            Port = int.Parse(_config["Smtp:Port"]!);
            User = _config["Smtp:User"]!;
            Pass = _config["Smtp:Pass"]!;
            From = _config["Smtp:From"]!;
        }

        private SmtpClient CreateClient()
        {
            return new SmtpClient(Host, Port)
            {
                Credentials = new NetworkCredential(User, Pass),
                EnableSsl = true
            };
        }

        private MailMessage CreateEmail(string to, string subject, string body)
        {
            var mail = new MailMessage
            {
                From = new MailAddress(From),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mail.To.Add(to);
            return mail;
        }

        // -------------------------------
        // PASSWORD RESET EMAIL
        // -------------------------------
        public async Task SendPasswordResetEmail(string email, string resetUrl)
        {
            string subject = "Reset your password - 2CRRE Docs";

            string body = $@"
                <div style='font-family: Arial, sans-serif; padding:20px;'>
                    <h2 style='color:#0d505a;'>Password Reset Request</h2>

                    <p>Hello,</p>
                    <p>You recently requested to reset your password.</p>
                    <p>Click the button below to reset it:</p>

                    <p style='margin:25px 0;'>
                        <a href='{resetUrl}' 
                           style='background-color:#0d505a; color:white; padding:12px 20px; text-decoration:none; border-radius:6px;'>
                            Reset Password
                        </a>
                    </p>

                    <p>If the button does not work, copy and paste this link:</p>
                    <p><a href='{resetUrl}'>{resetUrl}</a></p>

                    <hr />
                    <p style='color:#777;'>If you did not request this change, you can safely ignore this email.</p>
                </div>
            ";

            using var smtp = CreateClient();
            using var message = CreateEmail(email, subject, body);

            await smtp.SendMailAsync(message);
        }
    }
}
