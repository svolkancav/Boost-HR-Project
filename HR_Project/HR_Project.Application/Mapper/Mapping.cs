using AutoMapper;
using HR_Project.Application.IoC.Models.DTOs;
using HR_Project.Common.Models.DTOs;
using HR_Project.Common.Models.VMs;
using HR_Project.Domain.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Application.Mapper
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Personnel, PersonelDTO>().ReverseMap();
            CreateMap<Advance, UpdateAdvanceDTO>().ReverseMap();
            CreateMap<Advance, CreateAdvanceDTO>().ReverseMap();
            CreateMap<Advance, AdvanceVM>().ReverseMap();
            CreateMap<Absence, AbsenceDTO>().ReverseMap();
            CreateMap<Absence, AbsenceVM>().ReverseMap();
            CreateMap<AbsenceVM, AbsenceDTO>().ReverseMap();

        }
        
    }
}
