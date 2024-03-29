﻿using HR_Project.Common.Models.DTOs;
using HR_Project.Presentation.APIService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using HR_Project.Domain.Entities.Concrete;
using HR_Project.Domain.Enum;
using Microsoft.AspNetCore.Mvc.Rendering;
using HR_Project.Presentation.Models;
using HR_Project.Common;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;

namespace HR_Project.Presentation.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAPIService _apiService;
        private readonly IEmailService _emailService;


        public AccountController(IAPIService apiService, IEmailService emailService)
        {
            _apiService = apiService;
            _emailService = emailService;

        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var token = await _apiService.LoginAsync(model);

                    if (token != null)
                    {

                        Response.Cookies.Append("access-token", token.Token, new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            SameSite = SameSiteMode.None,
                            Expires = token.Expiration
                        });



                        var handler = new JwtSecurityTokenHandler();
                        var jsonToken = handler.ReadToken(token.Token) as JwtSecurityToken;

                        var email = jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                        var userId = jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                        var userName = jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                        var userSurName = jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value;
                        var imagePath = jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Thumbprint)?.Value;
                        var roles = jsonToken?.Claims.Where(c => c.Type == ClaimTypes.Role);
                        var companyId= jsonToken?.Claims.FirstOrDefault(c=>c.Type==ClaimTypes.WindowsSubAuthority)?.Value;
                        var isManager = jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.UserData)?.Value;

                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Email, email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(ClaimTypes.NameIdentifier, userId),
                            new Claim(ClaimTypes.Name, userName),
                            new Claim(ClaimTypes.Surname, userSurName),
                            new Claim(ClaimTypes.Thumbprint, imagePath),
                            new Claim(ClaimTypes.UserData, isManager),
                            new Claim(ClaimTypes.WindowsSubAuthority, companyId),
                            //new Claim("Company",company),
                            //new Claim("Department",department),

                        };
                        claims.AddRange(roles);

                        var identity = new ClaimsIdentity(claims, "login");
                        ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı");
                    }
                }
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", $"Giriş işlemi başarısız: {ex.Message}");
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            Response.Cookies.Delete("access-token");
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {

            List<CityDTO> cities = await _apiService.GetAsyncWoToken<List<CityDTO>>("city");
            List<RegionDTO> regionList = await _apiService.GetAsyncWoToken<List<RegionDTO>>("region");
            RegisterDTO model = new RegisterDTO();
            model.CityList = cities.Select(c => new SelectListItem
            {
                Value = c.CityId.ToString(),
                Text = c.Name
            })
                .ToList();
            model.Regions = regionList
                //.Where(d => d.CityId == personnel.CityId)
                .Select(d => new SelectListItem
                {
                    Value = d.RegionId.ToString(),
                    Text = d.Name
                })
                .ToList();
            return View(model);
        }






        [HttpPost]
        [ValidateAntiForgeryToken]
        //TODO: CompanyID eklenmeli ve Rol ataması yapılmalı Volkan
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            IFormFile uploadedFile = model.UploadImage;
            if (ModelState.IsValid)
            {
                RegisterResponse response = await _apiService.RegisterAsync(model);
                var confirmationLink = Url.Action("Confirmation", "Account", response, Request.Scheme);
                // Send registration confirmation email
                //"https://localhost:7034/Account/Confirmation", new { id = personnel.Id, token }, Request.Scheme
                string subject = "Registration Confirmation";
                string body = $"Lütfen hesabınızı doğrulamak için linke <a href='{confirmationLink}'>tıklayın</a>.";

                //await _emailService.SendEmailRegisterAsync(model.Email, subject, body);

                Toastr("success", "Kayıt başarılı bir şekilde oluşturuldu.");

                //CompanyRegisterDTO companyRegisterDTO = new CompanyRegisterDTO();
                //companyRegisterDTO.CompanyName = model.CompanyName;
                //companyRegisterDTO.PhoneNumber = model.PhoneNumber;
                //companyRegisterDTO.RegionId = model.RegionId;
                //companyRegisterDTO.CityId = model.CityId;
                //companyRegisterDTO.PersonnelCount = model.PersonnelCount;
                //companyRegisterDTO.Email = model.Email;

                //var result = await _apiService.PostAsyncWoToken<CompanyRegisterDTO, CompanyRegisterDTO>("Company", companyRegisterDTO);



                List<CreateCompanyDTO> companies = await _apiService.GetAsyncWoToken<List<CreateCompanyDTO>>("Company");
                var selectedCompany = companies.FirstOrDefault(x => x.Email == model.Email);
                string companyName = selectedCompany.Name;

                await _emailService.SendConfirmationEmailAsync("hreasyboost@gmail.com", companyName);


                return RedirectToAction("Information", "Account");

            }
            return View(model);

        }

        [HttpGet]
        public async Task<IActionResult> Information()
        {
            return View();
        }



        [HttpGet]
        public async Task<IActionResult> Confirmation(string id, string token)
        {

            await _apiService.ConfirmAsync("Account/confirm", new MailConfirmDTO { Id = id, Token = token });
            return RedirectToAction("Login", "Account");

        }
        [HttpGet]
        public async Task<JsonResult> GetRegions(int cityId)
        {
            List<RegionDTO> regionList = await _apiService.GetAsyncWoToken<List<RegionDTO>>("region");
            var regions = regionList
                .Where(d => d.CityId == cityId)
                .Select(d => new SelectListItem
                {
                    Value = d.RegionId.ToString(),
                    Text = d.Name
                })
                .ToList();

            return Json(regions);
        }


        [HttpGet]
        public async Task<IActionResult> ForgotPassword()
        {
            ForgotPasswordDTO forgotPasswordDTO = new ForgotPasswordDTO();
            return View(forgotPasswordDTO);
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    List<UpdateProfileDTO> personnelList = await _apiService.GetAsync<List<UpdateProfileDTO>>("personnel", HttpContext.Request.Cookies["access-token"]);
                    var personnel = personnelList.Find(x => x.Email == model.Email);
                    if (personnel is not null)
                    {
                        string subject = "Şifre Yenile";
                        string body = $"Lütfen şifrenizi yenilemek için linke <a href='{"https://easyhrboost.azurewebsites.net/Account/Changepassword"}'>tıklayın</a>.";

                        await _emailService.SendEmailRegisterAsync(personnel.Email, subject, body);
                        Toastr("success", "Mail gönderildi");
                    }
                }
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                Toastr("error", $"Kayıt sırasında hata oluştu : {ex.Message}");
                return View(model);
            }
            
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            ChangePasswordDTO changePasswordDTO = new ChangePasswordDTO();
            return View(changePasswordDTO);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO model)
        {
            try
            {
                List<UpdateProfileDTO> personnelList = await _apiService.GetAsync<List<UpdateProfileDTO>>("personnel", HttpContext.Request.Cookies["access-token"]);
                var personnel = personnelList.Find(x => x.Email == model.Email);
                personnel.Password = model.NewPassword;
                await _apiService.UpdateAsync<UpdateProfileDTO>("personnel", personnel, HttpContext.Request.Cookies["access-token"]);

                string subject = "Şifre Değiştirildi";
                string body = $"Şifreniz değiştirilmiştir. Yeni şifreniz ile giriş yapmak için linke <a href='{"https://localhost:7034/Account/Login"}'>tıklayın</a>.";

                await _emailService.SendEmailRegisterAsync(personnel.Email, subject, body);

                Toastr("success", "Şifre başarılı bir şekilde güncellendi.");

                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                Toastr("error", $"Kayıt sırasında hata oluştu : {ex.Message}");
                throw;
            }
            
        }
    }
}

