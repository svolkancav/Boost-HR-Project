using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Common.ValidationClass
{
	public class DateCompare : ValidationAttribute
	{
		private readonly string _comparisonProperty;

		public DateCompare(string comparisonProperty)
		{
			_comparisonProperty = comparisonProperty;
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			ValidationResult validationResult = ValidationResult.Success;

			DateTime currentValue = (DateTime)value;

			DateTime compareValue = (DateTime)validationContext.ObjectType.GetProperty(_comparisonProperty).GetValue(validationContext.ObjectInstance);

			if (currentValue < compareValue)
			{
				validationResult = new ValidationResult(ErrorMessage);
			}

			return validationResult;
		}
	}
}
