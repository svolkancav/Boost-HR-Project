using MailKit.Net.Smtp;
using MimeKit;
using Org.BouncyCastle.Crypto.Macs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Common
{
    
    public class EmailService : IEmailService
	{
        private readonly ITokenService _tokenService;
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;

        public EmailService( ITokenService tokenService)
        {
            _tokenService = tokenService;
            _smtpServer = "smtp.gmail.com";
            _smtpPort = 587;
            _smtpUsername = "hreasyboost@gmail.com";
            _smtpPassword = "bjas khhb jcud tjrw";
        }

        public async Task SendConfirmationEmailAsync(string toEmail, int companyId)
        {
            //var token = await _tokenService.GenerateTokenAsync(companyId);
            var confirmationLink = $"https://localhost:7034/company/confirm?companyId={companyId}";

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Admin", "hreasyboost@gmail.com"));
            message.To.Add(new MailboxAddress("User", toEmail));
            message.Subject = "Şirket Onayı";

            var builder = new BodyBuilder();
            builder.TextBody = $"Sayın Admin, {companyId} şirketi için onayınız beklenmektedir. Lütfen aşağıdaki linke tıklayarak onaylayın:\n\n{confirmationLink}";

            message.Body = builder.ToMessageBody();

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, false);
                await client.AuthenticateAsync("hreasyboost@gmail.com", "bjas khhb jcud tjrw");
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new System.Net.Mail.SmtpClient(_smtpServer)
            {
                Port = _smtpPort,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                EnableSsl = true,
            };

            var message = new MailMessage
            {
                From = new MailAddress(_smtpUsername, "HS-12-MVC Hamburgerci"),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true,
            };

            message.To.Add(email);

            return client.SendMailAsync(message);
        }



        public async Task SendEmailRegisterAsync(string toEmail, string subject, string body)
		{
			try
			{
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Admin", "hreasyboost@gmail.com"));
                message.To.Add(new MailboxAddress("User", toEmail));
                message.Subject = subject;

                var builder = new BodyBuilder();
                builder.HtmlBody = body;
                message.Body = builder.ToMessageBody();

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 587, false);
                    await client.AuthenticateAsync("hreasyboost@gmail.com", "bjas khhb jcud tjrw");
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
            }
			catch (Exception message)
			{

				throw message;
			}
			


		}
	}
}
