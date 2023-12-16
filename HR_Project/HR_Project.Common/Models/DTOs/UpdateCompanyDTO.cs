using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Common.Models.DTOs
{
	public class UpdateCompanyDTO
	{
		public int Id { get; set; }

		public string Name { get; set; }
		public string Address { get; set; }
		public string TaxNumber { get; set; }
		public string TaxOffice { get; set; }
		public string PhoneNumber { get; set; }


		//public string Logo { get; set; }
	}
}
