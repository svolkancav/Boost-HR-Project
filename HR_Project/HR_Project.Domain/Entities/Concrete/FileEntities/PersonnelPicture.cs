using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Domain.Entities.Concrete.FileEntities
{
    public class PersonnelPicture : File
    {
        public Guid PersonnelId { get; set; }
        public Personnel Personnel { get; set; }
    }
}
