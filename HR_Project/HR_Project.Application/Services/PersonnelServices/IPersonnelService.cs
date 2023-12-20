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
        Task Update(UpdateProfileDTO model);
        Task Delete(string id);
        Task<PersonelDTO> GetById(string id);
        Task<PersonelDTO> GetByEmail(string email);
        Task<List<PersonelDTO>> GetPersonels();
        Task<SignInResult> Login(LoginDTO model);
        Task<string[]> GetRoles(string email);

        //RegisterDTO
        Task<IdentityResult> Register(RegisterDTO model);
        void Logout(string token);
        Task<UpdateProfileDTO> FillDTO(string id);
    }
}
