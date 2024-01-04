using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Common.ValidationClass
{
    public class DateValidaditon : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime date = Convert.ToDateTime(value);
            DateTime now = DateTime.Now;
            DateTime lastMonthStart = new DateTime(now.Year, now.Month, 1).AddMonths(-1);

            if (date >= lastMonthStart)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
