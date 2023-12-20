using HR_Project.Domain.Entities.Abstract;
using HR_Project.Domain.Entities.Concrete.FileEntities;
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

        public ExpenseType ExpenseType { get; set; }

        public DateTime ExpenseDate { get; set; }

        public double ExpenseAmount { get; set;}

        public Currency Currency { get; set; }

        public string Reason { get; set; }

        public ConditionType Condition { get; set; }


        //Navigation 
        //Todo: PersonnelId masterexpense taşınacak
        public Guid PersonnelId { get; set; }
        public Personnel Personnel { get; set; }

        public int MasterExpenseId { get; set; }

        public MasterExpense MasterExpense { get; set; }

        public int? ImageId { get; set; }
        public CostPicture CostPicture { get; set; }





    }
}
