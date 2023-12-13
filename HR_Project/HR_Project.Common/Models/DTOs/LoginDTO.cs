using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Common.Models.DTOs
{
	public class LoginDTO
	{
		[Required]
		public string Email { get; set; }
		[Required]
		public string Password { get; set; }

		//Todo : Add remember me
        //public bool RememberMe { get; set; }
    }
}
