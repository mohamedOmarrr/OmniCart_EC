using System.Net;
using System.Net.Mail;
using E_commerce_application.Interfaces;
using Microsoft.Extensions.Options;

namespace E_commerce_infrastructure.Email;

public class EmailService(IOptions<EmailSettings> settings) :IEmailService
{
    private readonly EmailSettings _settings = settings.Value;
    
    public async Task SendEmailAsync(
        string to,
        string subject,
        string body)
    {
        using var message = new MailMessage();

        message.From = new MailAddress(
            _settings.FromEmail,
            _settings.FromName);

        message.To.Add(to);
        message.Subject = subject;
        message.Body = body;
        message.IsBodyHtml = true;

        using var smtp = new SmtpClient(
            _settings.Host,
            _settings.Port);

        smtp.EnableSsl = true;

        smtp.Credentials = new NetworkCredential(
            _settings.Username,
            _settings.Password);

        await smtp.SendMailAsync(message);
    }
}