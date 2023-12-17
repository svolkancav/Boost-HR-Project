using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HR_Project.Domain.Enum
{
    public enum ExpenseType
    {
        [Display(Name = "Ulaşım")]
        Transportation = 1,
        [Display(Name = "Yiyecek-İçecek")]
        Catering = 2,
        [Display(Name = "Konaklama")]
        Accommodation = 3
    }
}
