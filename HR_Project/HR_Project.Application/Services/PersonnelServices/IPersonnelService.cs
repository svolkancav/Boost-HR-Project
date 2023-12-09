using HR_Project.Application.IoC.Models.DTOs;
using HR_Project.Common.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Application.Services.PersonelServices
{
    public interface IPersonnelService
    {
        // TODO: PersonelDTO geçici eklendi. DTO lar oluştuktan sonra yenileri eklenebilir.
        Task Create(PersonelDTO model);
        Task Update(PersonelDTO model);
        Task Delete(string id);
        Task<PersonelDTO> GetById(string id);
        Task<List<PersonelDTO>> GetPersonels();
        Task<SignInResult> Login(LoginDTO model);
        Task<string[]> GetRoles(string email);
        void Logout(string token);
    }
}
