using HR_Project.Domain.Enum;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HR_Project.Common.ValidationClass;

namespace HR_Project.Common.Models.DTOs
{
    public class UpdateAbsenceDTO
    {
        public int Id { get; set; }
        public Guid PersonnelId { get; set; }
        [Display(Name = "Açıklama")]
        [Required(ErrorMessage = "Açıklama alanı boş bırakılamaz.")]
        public string Reason { get; set; }
        [Display(Name = "İzin Türü")]
        public LeaveTypes LeaveTypes { get; set; }
        [Display(Name = "Başlangıç Tarihi")]
		[DataType(DataType.Date)]
		[DateValidaditon(ErrorMessage = "Başlangıç tarihi bugünden önce olamaz.")]
		public DateTime StartDate { get; set; }
        [Display(Name = "Bitiş Tarihi")]
		[DateValidaditon(ErrorMessage = "Bitiş tarihi bugünden önce olamaz.")]
		[DateCompare("StartDate", ErrorMessage = "Bitiş tarihi başlangıç tarihinden önce olamaz.")]
		[DataType(DataType.Date)]
		public DateTime EndDate { get; set; }
        [Display(Name = "İzin Süresi")]
        [AbsenceDurationValidation("StartDate", "EndDate")]
        public double AbsenceDuration { get; set; }
        [Display(Name = "İzin Durumu")]
        public ConditionType Condition { get; set; }
    }
}
