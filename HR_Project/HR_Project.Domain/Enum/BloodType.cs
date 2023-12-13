using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Domain.Enum
{
	public enum BloodType
	{
		[Display(Name = "0 Rh+")]
		ZeroRhPositive = 1,
		[Display(Name = "0 Rh-")]
		ZeroRhNegative = 2,
		[Display(Name = "AB Rh+")]
		ABRhPositive = 3,
		[Display(Name = "AB Rh-")]
		ABRhNegative = 4,
		[Display(Name = "A Rh+")]
		ARhPositive = 5,
		[Display(Name = "A Rh-")]
		ARhNegative = 6,
		[Display(Name = "B Rh+")]
		BRhPositive = 7,
		[Display(Name = "B Rh-")]
		BRhNegative = 8
	}
}
