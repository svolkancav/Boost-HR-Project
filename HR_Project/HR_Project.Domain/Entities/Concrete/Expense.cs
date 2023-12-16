using HR_Project.Domain.Entities.Abstract;
using HR_Project.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Domain.Entities.Concrete
{
    public class Expense : BaseEntity, IEntity<int>
    {

        public int Id { get; set; }
        public string ExpenseType { get; set; }

        public DateTime ExpenseDate { get; set; }

        public double ExpenseAmount { get; set;}

        public Currency Currency { get; set; }

        public string Explanation { get; set; }

        public ConditionType Condition { get; set; }


        //Navigation 

        public Guid PersonnelId { get; set; }
        public Personnel Personnel { get; set; }






    }
}
