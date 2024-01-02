using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HR_Project.Common.ValidationClass;
using HR_Project.Domain.Enum;

namespace HR_Project.Common.Models.DTOs
{
	public class CreateAdvanceDTO
	{
        [DateValidationNow(ErrorMessage = "Talep edilen tarih bugünden önce olamaz.")]
        public DateTime LastPaidDate { get; set; }

		[DataType(DataType.Currency)]
		[Display(Name = "Tutar")]
		[Required]
        public decimal Amount { get; set; }

        [NotMapped]
        public string AmountString
        {
            get => Amount.ToString("N2", CultureInfo.InvariantCulture);
            set => Amount = decimal.TryParse(value.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out var parsedValue) ? parsedValue : 0;
        }


        [Display(Name = "Açıklama")]
		[Required]
		public string Reason { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Para Birimi")]
        [RequiredEnum(ErrorMessage = "Lütfen para birimini giriniz.")]
        public Currency Currency { get; set; }
        public ConditionType Condition { get; set; }

		public Guid PersonnelId { get; set; }
	}
}
