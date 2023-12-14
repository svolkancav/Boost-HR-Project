using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Common.ValidationClass
{
	public class RequiredEnum : ValidationAttribute
	{
		public override bool IsValid(object value)
		{

			if (value != null  )
			{
				return true;
			}

			return false;
		}
	}
}
