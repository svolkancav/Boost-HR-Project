using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HR_Project.Domain.Enum
{
    public enum AdvanceConditions
    {
        [Display(Name = "Onaylandı")]
        Approved =1,
        [Display(Name = "Beklemede")]
        Pending =2,
        [Display(Name = "Reddedildi")]
        Refused =3
    }
}
