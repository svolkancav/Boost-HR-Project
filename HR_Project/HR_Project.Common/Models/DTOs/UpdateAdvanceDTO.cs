using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HR_Project.Domain.Enum;

namespace HR_Project.Common.Models.DTOs
{
	public class UpdateAdvanceDTO
	{
		public int Id { get; set; }
		public DateTime LastPaidDate { get; set; }
		public decimal Amount { get; set; }
		public string Reason { get; set; }
		public ConditionType Codition { get; set; }

		public Guid PersonnelId { get; set; }
	}
}
