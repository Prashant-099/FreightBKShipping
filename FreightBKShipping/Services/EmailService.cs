using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

public class EmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendResetPasswordEmailAsync(string toEmail, string token)
    {
        string fromEmail = _config["Email:From"];
        string smtpServer = _config["Email:Smtp"];
        int smtpPort = int.Parse(_config["Email:Port"]);
        string smtpUser = _config["Email:User"];
        string smtpPass = _config["Email:Pass"];

        //string resetLink = $"http://localhost:7226/reset-password?email={toEmail}&token={token}";
        string resetLink = $"http://147.79.68.129/reset-password?email={toEmail}&token={token}";

        var message = new MailMessage(fromEmail, toEmail)
        {
            Subject = "Password Reset Request",
            Body = $"Hi,\n\nClick the link below to reset your password:\n{resetLink}\n\nThis link will expire in 15 minutes.",
            IsBodyHtml = false
        };

        using var smtp = new SmtpClient(smtpServer, smtpPort)
        {
            Credentials = new NetworkCredential(smtpUser, smtpPass),
            EnableSsl = true
        };

        await smtp.SendMailAsync(message);
    }
}
