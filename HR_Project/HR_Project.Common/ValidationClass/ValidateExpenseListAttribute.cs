using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HR_Project.Common.Models.DTOs;
using HR_Project.Domain.Enum;

namespace HR_Project.Common.ValidationClass
{
	public class ValidateExpenseListAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			if (value is List<ExpenseDTO> expenses)
			{
				if (expenses.Count == 0)
				{
					return new ValidationResult("Gider listesi boş olamaz.");
				}

				if (IsValidExpense(expenses))
				{
					return ValidationResult.Success;
				}
				else
				{
					return new ValidationResult("Gider listesindeki para birimleri farklı olamaz.");
				}
			}

			return new ValidationResult("Geçersiz veri tipi.");
		}

		private bool IsValidExpense(List<ExpenseDTO> expense)
		{
			Currency currency = expense[0].Currency;

			if (expense.Any(e => e.Currency != currency))
			{
				return false;
			}

			return true;
		}
	}
}
