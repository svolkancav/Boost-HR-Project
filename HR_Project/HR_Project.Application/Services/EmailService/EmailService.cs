﻿using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Application.Services.EmailService
{
	public class EmailService : IEmailService
	{

		public async Task SendEmailRegisterAsync(string toEmail, string subject, string body)
		{

			var message = new MimeMessage();
			message.From.Add(new MailboxAddress("Admin", "hreasyboost@gmail.com"));
			message.To.Add(new MailboxAddress("User", toEmail));
			message.Subject = subject;

			var builder = new BodyBuilder();
			builder.HtmlBody = body;
			message.Body = builder.ToMessageBody();

			using (var client = new SmtpClient())
			{
				await client.ConnectAsync("smtp.gmail.com", 587, false);
				await client.AuthenticateAsync("hreasyboost@gmail.com", "HS-12Boost");
				await client.SendAsync(message);
				await client.DisconnectAsync(true);
			}
		}
	}
}
