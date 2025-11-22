using FreightBKShipping.Models;
using FreightBKShipping.DTOs;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

public class SentWpEmailService
{
    public async Task<bool> SendEmailAsync(SendWpMail mailConfig, SendEmailRequestDto dto)
    {
        try
        {
            // 1️⃣ Create the email message
            var email = new MimeMessage();

            // From
            email.From.Add(MailboxAddress.Parse(mailConfig.SendWpMailEmailFrom));

            // To (multiple allowed, separated by , or ;)
            if (!string.IsNullOrWhiteSpace(dto.ToEmail))
            {
                var toEmails = dto.ToEmail
                    .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(e => e.Trim())
                    .Distinct();

                foreach (var to in toEmails)
                {
                    email.To.Add(MailboxAddress.Parse(to));
                }
            }


            // CC (optional)
            if (!string.IsNullOrEmpty(dto.CcEmails))
            {
                foreach (var cc in dto.CcEmails.Split(';', ','))
                {
                    if (!string.IsNullOrWhiteSpace(cc))
                        email.Cc.Add(MailboxAddress.Parse(cc.Trim()));
                }
            }

            // BCC (optional)
            //if (!string.IsNullOrEmpty(dto.))
            //{
            //    foreach (var bcc in dto.BCC.Split(';', ','))
            //    {
            //        if (!string.IsNullOrWhiteSpace(bcc))
            //            email.Bcc.Add(MailboxAddress.Parse(bcc.Trim()));
            //    }
            //}

            // Subject
            email.Subject = dto.Subject ?? "No Subject";

            // Body
            var builder = new BodyBuilder
            {
                HtmlBody = dto.Body ?? ""
            };

            // Attachment
            if (!string.IsNullOrEmpty(dto.AttachmentBase64) && !string.IsNullOrEmpty(dto.AttachmentFileName))
            {
                var bytes = Convert.FromBase64String(dto.AttachmentBase64);
                builder.Attachments.Add(dto.AttachmentFileName, bytes);
            }

            email.Body = builder.ToMessageBody();

            // 2️⃣ Parse SMTP Port
            if (!int.TryParse(mailConfig.SendWpMailSmtpPort, out int smtpPort))
                throw new Exception("SMTP Port is invalid.");

            // 3️⃣ Send email via SMTP
            SecureSocketOptions sslOption = mailConfig.SendWpMailEnableSsl == true
              ? SecureSocketOptions.StartTls
                : SecureSocketOptions.None;
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(mailConfig.SendWpMailSmtpServer, smtpPort, sslOption);

            // Authenticate
            await smtp.AuthenticateAsync(mailConfig.SendWpMailEmailUser, mailConfig.SendWpMailEmailPass);

            // Send
            await smtp.SendAsync(email);

            // Disconnect
            await smtp.DisconnectAsync(true);

            return true;
        }
        catch (Exception ex)
        {
            // Log error for debugging
            Console.WriteLine($"❌ Email sending failed: {ex.Message}");
            return false;
        }
    }

}
