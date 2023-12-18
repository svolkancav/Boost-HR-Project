﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Application.Services.EmailService
{
	public interface IEmailService 
	{
		Task SendEmailRegisterAsync(string toEmail, string subject, string body);
	}
}
