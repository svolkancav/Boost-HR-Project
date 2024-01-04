using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Common.ValidationClass
{
	public class AbsenceDurationValidation : ValidationAttribute
	{
		private readonly string _startDate;
		private readonly string _endDate;

		public AbsenceDurationValidation(string startDate, string endDate)
		{
			_startDate = startDate;
			_endDate = endDate;
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			ValidationResult validationResult = ValidationResult.Success;

			var absenceduration = (double)value;

			DateTime startDate = (DateTime)validationContext.ObjectType.GetProperty(_startDate).GetValue(validationContext.ObjectInstance);
			DateTime endDate = (DateTime)validationContext.ObjectType.GetProperty(_endDate).GetValue(validationContext.ObjectInstance);

			var difference = endDate - startDate;
			var days = ((int)difference.TotalDays) + 1;

			if (absenceduration > days)
			{
				validationResult = new ValidationResult("İzin süresi seçilen tarihlerden fazla olamaz.");
			}

			if (Math.Abs(absenceduration % 0.5) > double.Epsilon)
			{
				validationResult = new ValidationResult("İzin süresi tam gün veya yarım gün girebilirsiniz.");
			}

			return validationResult;
		}
	}
}
