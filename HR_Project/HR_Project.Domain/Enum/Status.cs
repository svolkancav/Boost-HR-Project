using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HR_Project.Domain.Enum
{
    public enum Status
    {
        [Display(Name = "Eklendi")]
        Inserted = 1,
        [Display(Name = "Güncellendi")]
        Updated = 2,
        [Display(Name = "Silindi")]
        Deleted = 3
    }
}
