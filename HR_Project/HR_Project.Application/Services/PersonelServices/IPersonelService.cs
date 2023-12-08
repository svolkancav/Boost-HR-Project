using HR_Project.Application.IoC.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Application.Services.PersonelServices
{
    public interface IPersonelService
    {
        // TODO: PersonelDTO geçici eklendi. DTO lar oluştuktan sonra yenileri eklenebilir.
        Task Create(PersonelDTO model);
        Task Update(PersonelDTO model);
        Task Delete(int id);
        Task<PersonelDTO> GetById(int id);
        Task<List<PersonelDTO>> GetPersonels();
    }
}
