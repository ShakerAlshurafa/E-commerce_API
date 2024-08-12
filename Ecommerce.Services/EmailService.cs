using Ecommerce.Core.IRepositories;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration configuration;

        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Shaker Project", configuration["EmailSettings:FromEmail"]));
            emailMessage.To.Add(new MailboxAddress("", toEmail));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message };

            var client = new SmtpClient();
            await client.ConnectAsync(
                configuration["EmailSettings:SmtpServer"], 
                int.Parse(configuration["EmailSettings:Port"]), 
                bool.Parse(configuration["EmailSettings:UseSSL"])
            );

            await client.AuthenticateAsync(configuration["EmailSettings:FromEmail"], configuration["EmailSettings:Password"]);

            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }
    }
}
