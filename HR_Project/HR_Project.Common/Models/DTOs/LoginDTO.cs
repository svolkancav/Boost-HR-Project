using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Common.Models.DTOs
{
	public class LoginDTO
	{
		public string Email { get; set; }
		public string Password { get; set; }

		//Todo : Add remember me
        //public bool RememberMe { get; set; }
    }
}
