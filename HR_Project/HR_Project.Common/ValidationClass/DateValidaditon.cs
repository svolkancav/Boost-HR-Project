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
            if (date >= DateTime.Now.AddMonths(-1))
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
