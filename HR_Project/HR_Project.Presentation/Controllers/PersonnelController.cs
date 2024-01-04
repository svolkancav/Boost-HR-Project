using HR_Project.Application.IoC.Models.DTOs;
using HR_Project.Common.Models.DTOs;
using HR_Project.Common.Models.VMs;
using HR_Project.Domain.Entities.Concrete;
using HR_Project.Presentation.APIService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using X.PagedList;
using HR_Project.Common;
using NuGet.Common;

namespace HR_Project.Presentation.Controllers
{
    public class PersonnelController : BaseController
    {
        private readonly IAPIService _apiService;
        private readonly IEmailService _emailService;


        public PersonnelController(IAPIService apiService, IEmailService emailService)
        {
            _apiService = apiService;
            _emailService = emailService;
        }

        public async Task<IActionResult> Index(string searchText, int pageNumber = 1, int pageSize = 10, string sortColumn = "", string sortOrder = "")
        {
            if (!string.IsNullOrEmpty(searchText))
            {
                List<PersonelVM> personnels = await _apiService.GetAsync<List<PersonelVM>>("personnel", HttpContext.Request.Cookies["access-token"]);
                List<PersonelVM> selectedPersonnels = personnels.Where(x => x.Name.ToLower().Contains(searchText.ToLower()) || x.Surname.ToString().ToLower().Contains(searchText.ToLower())).ToList();
                // Apply sorting
                selectedPersonnels = ApplySorting(selectedPersonnels.AsQueryable(), sortColumn, sortOrder).ToList();
                return View(selectedPersonnels.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                List<PersonelVM> personnels = await _apiService.GetAsync<List<PersonelVM>>("personnel", HttpContext.Request.Cookies["access-token"]);
                personnels = ApplySorting(personnels.AsQueryable(), sortColumn, sortOrder).ToList();
                return View(personnels.ToPagedList(pageNumber, pageSize));
            }

        }

        private IQueryable<PersonelVM> ApplySorting(IQueryable<PersonelVM> personnelList, string sortColumn, string sortOrder)
        {
            switch (sortColumn)
            {
                case "Name":
                    personnelList = sortOrder == "asc" ? personnelList.OrderBy(p => p.Name) : personnelList.OrderByDescending(p => p.Name);
                    break;
                case "Surname":
                    personnelList = sortOrder == "asc" ? personnelList.OrderBy(p => p.Surname) : personnelList.OrderByDescending(p => p.Surname);
                    break;
                case "Email":
                    personnelList = sortOrder == "asc" ? personnelList.OrderBy(p => p.Email) : personnelList.OrderByDescending(p => p.Email);
                    break;
                case "Adresi":
                    personnelList = sortOrder == "asc" ? personnelList.OrderBy(p => p.Address) : personnelList.OrderByDescending(p => p.Address);
                    break;

                // Add cases for other columns as needed
                default:
                    // Default sorting by Name in ascending order
                    personnelList = personnelList.OrderBy(p => p.Name);
                    break;
            }

            return personnelList;
        }

        public async Task<IActionResult> Create()
        {
            List<CityDTO> cities = await _apiService.GetAsync<List<CityDTO>>("city", HttpContext.Request.Cookies["access-token"]);
            List<RegionDTO> regionList = await _apiService.GetAsync<List<RegionDTO>>("region", HttpContext.Request.Cookies["access-token"]);
            List<CreateProfileDTO> personnelList = await _apiService.GetAsync<List<CreateProfileDTO>>("personnel", HttpContext.Request.Cookies["access-token"]);
            List<DepartmentDTO> departments = await _apiService.GetAsync<List<DepartmentDTO>>("department", HttpContext.Request.Cookies["access-token"]);

            CreateProfileDTO model = new CreateProfileDTO();
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
            model.Departments = departments
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                })
                .ToList();

            model.Managers = personnelList
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                })
                .ToList();


            return View(model);


        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProfileDTO model)
        {
            if (ModelState.IsValid)
            {
                var id = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var personnel = await _apiService.GetByIdAsync<CreateProfileDTO>("personnel", id, HttpContext.Request.Cookies["access-token"]);
                model.CompanyId = personnel.CompanyId;
                await _apiService.PostAsync<CreateProfileDTO, CreateProfileDTO>("personnel", model, HttpContext.Request.Cookies["access-token"]);


                var confirmationLink = Url.Action("Changepassword", "Account", new ChangePasswordDTO { Email = model.Email }, Request.Scheme);


                _emailService.SendEmailAsync(model.Email, "HR Project - Şifre Oluşturma", $"Şifrenizi Oluşturmak için <a href='{confirmationLink}'>tıklayınız</a>.");
                
                return RedirectToAction("Index");
            }
            else return RedirectToAction("Index");

        }

        public async Task<IActionResult> Profil()
        {
            var id = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var personnel = await _apiService.GetByIdAsync<UpdateProfileDTO>("personnel", id, HttpContext.Request.Cookies["access-token"]);

            List<CityDTO> cities = await _apiService.GetAsyncWoToken<List<CityDTO>>("city");
            List<RegionDTO> regionList = await _apiService.GetAsyncWoToken<List<RegionDTO>>("region");

            personnel.CityList = cities.Select(c => new SelectListItem
            {
                Value = c.CityId.ToString(),
                Text = c.Name
            })
                .ToList();
            personnel.Regions = regionList
                //.Where(d => d.CityId == personnel.CityId)
                .Select(d => new SelectListItem
                {
                    Value = d.RegionId.ToString(),
                    Text = d.Name
                })
                .ToList();


            return View(personnel);
        }

        public async Task<IActionResult> Update(string id)
        {

            var personnel = await _apiService.GetByIdAsync<UpdateProfileDTO>("personnel", id, HttpContext.Request.Cookies["access-token"]);

            List<CityDTO> cities = await _apiService.GetAsyncWoToken<List<CityDTO>>("city");
            List<RegionDTO> regionList = await _apiService.GetAsyncWoToken<List<RegionDTO>>("region");

            personnel.CityList = cities.Select(c => new SelectListItem
            {
                Value = c.CityId.ToString(),
                Text = c.Name
            })
                .ToList();
            personnel.Regions = regionList
                //.Where(d => d.CityId == personnel.CityId)
                .Select(d => new SelectListItem
                {
                    Value = d.RegionId.ToString(),
                    Text = d.Name
                })
                .ToList();


            return View(personnel);
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            try
            {
                await _apiService.PostFileAsync("personnel/UploadImage", file, HttpContext.Request.Cookies["access-token"]);

                var token = await _apiService.RefreshToken(HttpContext.Request.Cookies["access-token"]);

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
                    var roles = jsonToken?.Claims.Where(c => c.Type == ClaimTypes.Role);

                    var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Email, email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(ClaimTypes.NameIdentifier, userId),
                            new Claim(ClaimTypes.Name, userName),
                            new Claim(ClaimTypes.Surname, userSurName),
                            new Claim(ClaimTypes.Thumbprint, imagePath),
                            //new Claim("Company",company),
                            //new Claim("Department",department),

                        };
                    claims.AddRange(roles);

                    var identity = new ClaimsIdentity(claims, "login");
                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                }
                Toastr("success", "Profil resmi başarılı bir şekilde güncellendi.");

                return Ok();
            }
            catch (Exception ex)
            {
                Toastr("error", $"Kayıt sırasında hata oluştu : {ex.Message}");

                throw ex;
            }
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateProfileDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                await _apiService.UpdateAsync("personnel", model, HttpContext.Request.Cookies["access-token"]);
                Toastr("success", "Kayıt başarılı bir şekilde güncellendi.");
                return RedirectToAction("Profil");

            }

            catch (Exception ex)
            {
                Toastr("error", $"Kayıt sırasında hata oluştu : {ex.Message}");
                return View(model);
            }

        }

        // silme için
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _apiService.DeleteAsync<PersonelDTO>($"personnel", id, HttpContext.Request.Cookies["access-token"]);
                Toastr("success", "Kayıt başarılı bir şekilde silindi.");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Toastr("error", $"Kayıt silinirken hata oluştu : {ex.Message}");
                return RedirectToAction("Index");
            }
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
        public async Task<JsonResult> GetManagersByDepartment(int departmentId)
        {
            List<UpdateProfileDTO> personnelList = await _apiService.GetAsync<List<UpdateProfileDTO>>("personnel", HttpContext.Request.Cookies["access-token"]);
            var managers = personnelList
                .Where(d => d.DepartmentId == departmentId)
                .Select(d => new SelectListItem
                {
                    Value = d.ManagerId.ToString(),
                    Text = d.ManagerName
                })
                .ToList();

            return Json(managers);
        }
    }
}
