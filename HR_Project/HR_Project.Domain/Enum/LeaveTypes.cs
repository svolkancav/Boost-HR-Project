using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HR_Project.Domain.Enum
{
    public enum LeaveTypes
    {
        [Display(Name = "Kurumun Verdiği İzin")]
        Leave = 1, //Kurum inisiyatifinde
        [Display(Name = "Günlük İzin")]
        DailyLeave = 2,
        [Display(Name = "Yıllık İzin")]
        AnnualLeave = 3, //Çalışma süresine göre değişkenlik göstermektedir.
        [Display(Name = "Evlilik İzni")]
        MarriageLicence = 4, //3 gün
        [Display(Name = "Ölüm İzni")]
        DeathLeave = 5, //3 gün
        [Display(Name = "Hastalık İzni")]
        SickLeave = 6, //Tek seferde 10 güne kadar izin alabilirler. 
        [Display(Name = "Doğum İzni")]
        MaternityLeave = 7, //16 hafta
        [Display(Name = "Ücretsiz İzin")]
        UnpaidLeave = 8, // Ücretsiz izin bir süreliğine iş sözleşmesinin askıya alınması anlamına gelir. 
        [Display(Name = "Ulusal ve Dini Bayram İzinleri")]
        HolidayLeave = 9,
        [Display(Name = "Babalık İzni")]
        PaternityLeave = 10, //5 gün
        [Display(Name = "Refakat İzni")]
        AccompanimentLeave = 11, //Maksimum 3 ay
        [Display(Name = "Gebelik Kontrol İzni")]
        PregnancyCheckPermit =12, //Hamile kalan kadın çalışanlar kontrolleri için bu ücretli izin hakkından yararlanabilmektedir.
        [Display(Name = "Süt İzni")]
        BreastFeedingLeave = 13, //Günde 1.5 saat
        [Display(Name = "Yeni İş Arama İzni")]
        NewJobSearchPermit = 14, //Günde 2 saat

    }
}

