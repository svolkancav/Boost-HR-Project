using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HR_Project.Common.ValidationClass;
using HR_Project.Domain.Enum;

namespace HR_Project.Common.Models.DTOs
{
	public class UpdateAdvanceDTO
	{
		public int Id { get; set; }
		[DateValidaditon(ErrorMessage = "Bitiş tarihi bugünden önce olamaz.")]
		public DateTime LastPaidDate { get; set; }
		[DataType(DataType.Currency)]
		[Display(Name = "Tutar")]
		[Required]
		public decimal Amount { get; set; }
		[Required]
		public string Reason { get; set; }

		[Display(Name = "Para Birimi")]
		[RequiredEnum(ErrorMessage = "Lütfen para birimini giriniz.")]
		public Currency Currency { get; set; }
		public ConditionType Codition { get; set; }

		public Guid PersonnelId { get; set; }
	}
}
