using HR_Project.Domain.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Domain.Entities.Concrete
{
    public class City : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Region> Regions { get; set; }
    }
}
