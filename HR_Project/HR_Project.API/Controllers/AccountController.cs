﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using HR_Project.Application.IoC.Models.DTOs;
using HR_Project.Application.Services.FileService;
using HR_Project.Application.Services.PersonelServices;
using HR_Project.Common;
using HR_Project.Common.Models.DTOs;
using HR_Project.Domain.Entities.Concrete;

using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace HR_Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IPersonnelService _personnelService;
        private readonly IConfiguration _configuration;
        private readonly UserManager<Personnel> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProfileImageService _profileImageService;

		public AccountController(IPersonnelService personnelService, IConfiguration configuration, UserManager<Personnel> userManager, IHttpContextAccessor httpContextAccessor, IProfileImageService profileImageService)
		{
			_personnelService = personnelService;
			_configuration = configuration;
			_userManager = userManager;
			_httpContextAccessor = httpContextAccessor;
			_profileImageService = profileImageService;
		}

		[HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            await _personnelService.Register(new RegisterDTO
            {
                Name = "admin",
                Surname = "admin",
                Title = "admin",
                Email = "admin",
                PhoneNumber = "admin",
                UserName = "admin",
                Password = "admin123",
                DepartmentId = 1,
                CompanyId = 2,

            });


            var user = await _personnelService.Login(model);

            if (user.Succeeded)
            {
                Personnel personnel = await _userManager.FindByEmailAsync(model.Email);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, model.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, personnel.Id.ToString()),
                    new Claim(ClaimTypes.Name, personnel.Name),
                    new Claim(ClaimTypes.Surname, personnel.Surname),
                    new Claim(ClaimTypes.Thumbprint, await _profileImageService.GetFileById(personnel.Id.ToString()))
                    //new Claim(ClaimTypes.Thumbprint, personnel.ImagePath),
                    //new Claim("Company",personnel.CompanyId.ToString()),
                    //new Claim("Department",personnel.DepartmentId.ToString()),
                };


                //var userRoles = await _personnelService.GetRoles(model.Email);

                //foreach (var userRole in userRoles)
                //{
                //    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                //}

                var token = GetToken(authClaims);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Kullanıcının token'ını iptal etme işlemi
            var accessToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            _personnelService.Logout(accessToken);

            return Ok(new { message = "Logout successful" });
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:secretKey"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["JwtSettings:validIssuer"],
                _configuration["JwtSettings:validAudience"],
                authClaims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: signIn);

            return token;
        }


        [HttpPost("Confirm")]
        public async Task<IActionResult> Confirm(MailConfirmDTO model)
        {
            Personnel personnel =  await _userManager.FindByIdAsync(model.Id);
            var result = await _userManager.ConfirmEmailAsync(personnel, model.Token);

            return Ok(result.Succeeded);
        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
			//Personnel personnel = new Personnel()
			//{
   //             UserName = model.UserName,
			//	Email = model.Email,
			//	PasswordHash = model.Password,
			//	Name = model.Name,
			//	Surname = model.Surname,
			//	Title = model.Title,
			//	PhoneNumber = model.PhoneNumber,
			//	Gender = model.Gender,
			//	Nation = model.Nation,
			//	AccountStatus = AccountStatus.Inactive,
   //             CityId = model.CityId,
   //             RegionId = model.RegionId,
   //             BirthDate = model.BirthDate,
   //             BloodType = model.BloodType,

			//};

			IdentityResult result = await _personnelService.Register(model);
			if (result.Succeeded)
            {
                Personnel personnel = await _userManager.FindByEmailAsync(model.Email);
				var token = await _userManager.GenerateEmailConfirmationTokenAsync(personnel);
                


                return Ok(new { id = personnel.Id, token });
                //await _emailService.SendEmailRegisterAsync(personnel.Email, subject, body);
            }


			return BadRequest(result);
        }



    }
}
