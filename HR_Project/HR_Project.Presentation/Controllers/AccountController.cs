using HR_Project.Common.Models.DTOs;
using HR_Project.Presentation.APIService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using HR_Project.Domain.Entities.Concrete;
using HR_Project.Application.Services.EmailService;
using HR_Project.Domain.Enum;
using Microsoft.AspNetCore.Mvc.Rendering;

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
                        var company = jsonToken?.Claims.FirstOrDefault(c => c.Type == "Company")?.Value;
                        var department = jsonToken?.Claims.FirstOrDefault(c => c.Type == "Department")?.Value;


                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Email, email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(ClaimTypes.NameIdentifier, userId),
                            new Claim(ClaimTypes.Name, userName),
                            new Claim(ClaimTypes.Surname, userSurName),
                            //new Claim(ClaimTypes.Thumbprint, imagePath),
                            //new Claim("Company",company),
                            //new Claim("Department",department),

                        };

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
            var registerDTO = new RegisterDTO
            {
                CityList = cities.Select(c => new SelectListItem
                {
                    Value = c.CityId.ToString(),
                    Text = c.Name
                })
                .ToList(),

                Region = new List<SelectListItem>()
            };
            return View(registerDTO);
        }






        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            if (ModelState.IsValid)
            {
                await _apiService.PostAsync<RegisterDTO, RegisterDTO>("personnel", model, HttpContext.Request.Cookies["access-token"]);


                return RedirectToAction("Login", "Account");

            }
            List<CityDTO> cities = await _apiService.GetAsyncWoToken<List<CityDTO>>("city");
            List<RegionDTO> regionList = await _apiService.GetAsyncWoToken<List<RegionDTO>>("region");
            model.CityList = cities.Select(c => new SelectListItem
            {
                Value = c.CityId.ToString(),
                Text = c.Name
            })
                .ToList();
            model.Region = regionList
                .Where(d => d.CityId == model.CityId)
                .Select(d => new SelectListItem
                {
                    Value = d.RegionId.ToString(),
                    Text = d.Name
                })
                .ToList();

            return View(model);
        }







        [HttpGet]
        public async Task<IActionResult> Confirmation(int id, string token)
        {


            if (id == null || token == null)
            {
                return RedirectToAction("login", "account");
            }

            var user = await _apiService.GetByIdAsync<RegisterDTO>($"personnel/getbyid", id.ToString(), HttpContext.Request.Cookies["access-token"]);
            RegisterDTO registerDTO = new RegisterDTO()
            {
                Email = user.Email,
                AccountStatus = user.AccountStatus,
                Gender = user.Gender,
                Nation = user.Nation,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                Surname = user.Surname,
                Title = user.Title

            };
            if (user == null)
            {
                ViewBag.ErrorMessage = $"The User ID {id} is invalid";
                return View("NotFound");
            }



            if (registerDTO is not null)
            {
                try
                {
                    if (!ModelState.IsValid)
                    {
                        return View();
                    }
                    registerDTO.AccountStatus = AccountStatus.Active;
                    await _apiService.UpdateAsync<RegisterDTO>("personnel", registerDTO, HttpContext.Request.Cookies["access-token"]);
                    Toastr("success", "Kayıt başarılı bir şekilde güncellendi.");
                    return RedirectToAction("Login");
                }
                catch (Exception ex)
                {
                    Toastr("error", $"Kayıt güncellenirken hata oluştu : {ex.Message}");

                    return View();
                }

            }
            else
            {
                ViewBag.ErrorTitle = "Email cannot be confirmed";
                return View("Error");
            }




        }
        [HttpGet]
        public async Task<JsonResult> GetRegions(int cityId)
        {
            List<RegionDTO> regionList = await _apiService.GetAsync<List<RegionDTO>>("region", HttpContext.Request.Cookies["access-token"]);
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
    }
}

