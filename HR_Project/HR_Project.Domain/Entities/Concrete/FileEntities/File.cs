using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HR_Project.Domain.Entities.Abstract;

namespace HR_Project.Domain.Entities.Concrete.FileEntities
{
    public class File : BaseEntity, IEntity<int>
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        [NotMapped]
        public override DateTime? ModifiedDate { get => base.ModifiedDate; set => base.ModifiedDate = value; }
    }
}
