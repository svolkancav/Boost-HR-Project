using HR_Project.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Common.Models.DTOs
{
    public class RegisterDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Title { get; set; }
        public string PhoneNumber { get; set; }
        public Gender Gender { get; set; }
        public Nation Nation { get; set; }
        public AccountStatus AccountStatus { get; set; }

    }
}
