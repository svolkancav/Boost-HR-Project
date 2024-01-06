using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Common.ValidationClass
{
	public class ExpenseDateValidationAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			ValidationResult validationResult = ValidationResult.Success;
			DateTime date = Convert.ToDateTime(value);
			DateTime now = DateTime.Now.Date.AddDays(1);

			if (date >= now)
			{
				validationResult = new ValidationResult("Masraf tarihi bugünden sonra olamaz.");
			}


			if (date <= now.AddMonths(-3))
			{
				validationResult = new ValidationResult("Masraf tarihini 3 aydan önce giremezsiniz.");
			}

			return validationResult;
		}
	}
}
