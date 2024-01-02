using HR_Project.Presentation.APIService;
using Microsoft.AspNetCore.Mvc;
using HR_Project.Application.IoC.Models.DTOs;
using HR_Project.Common.Models.DTOs;
using HR_Project.Common.Models.VMs;
using X.PagedList;
using HR_Project.Domain.Entities.Concrete;
using Microsoft.AspNetCore.Mvc.Rendering;
using Azure;
using HR_Project.Common;
using System.Security.Claims;

namespace HR_Project.Presentation.Controllers
{
	public class CompanyController : BaseController
	{
		private readonly IAPIService _apiService;
        private readonly IEmailService _emailService;
        private readonly ITokenService _tokenService;

        public CompanyController(IAPIService apiService, IEmailService emailService, ITokenService tokenService)
        {
            _apiService = apiService;
            _emailService = emailService;
            _tokenService = tokenService;
        }

        public async Task<IActionResult> Index()
        {
            var personnelId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<CompanyDTO> companies = await _apiService.GetAsync<List<CompanyDTO>>("company", HttpContext.Request.Cookies["access-token"]);

            var personnel = await _apiService.GetByIdAsync<UpdateProfileDTO>("personnel", personnelId, HttpContext.Request.Cookies["access-token"]);

            var selectedCompany = companies.Find(x => x.Id == personnel.CompanyId);

            return View(selectedCompany);
        }
        public async Task<IActionResult> Create()
        {
            List<CityDTO> cities = await _apiService.GetAsync<List<CityDTO>>("city", HttpContext.Request.Cookies["access-token"]);
            List<RegionDTO> regionList = await _apiService.GetAsync<List<RegionDTO>>("region", HttpContext.Request.Cookies["access-token"]); 
            
            CreateCompanyDTO model = new CreateCompanyDTO();
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
        public async Task<IActionResult> Create(CreateCompanyDTO model)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                await _apiService.PostAsync<CreateCompanyDTO, CreateCompanyDTO>("Company", model, HttpContext.Request.Cookies["access-token"]); 

                Toastr("success", "Kayıt başarılı bir şekilde oluşturuldu.");

                List<CreateCompanyDTO> companies = await _apiService.GetAsyncWoToken<List<CreateCompanyDTO>>("Company");
                var selectedCompany = companies.FirstOrDefault(x => x.Name == model.Name);

                await _emailService.SendConfirmationEmailAsync("hreasyboost@gmail.com", selectedCompany.Id); ;


                return RedirectToAction("Information","Account");

            }
            catch (Exception ex)
            {
                Toastr("error", $"Kayıt sırasında hata oluştu : {ex.Message}");

                return View(model);
            }

        }


        public async Task<IActionResult> Update(string id)
        {
            UpdateCompanyDTO company = await _apiService.GetByIdAsync<UpdateCompanyDTO>($"company/getbyid", id, HttpContext.Request.Cookies["access-token"]);
            return View(company);
        }

        [HttpPost]

        //TODO: Update hatalı
        public async Task<IActionResult> Update(UpdateCompanyDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                await _apiService.UpdateAsync<UpdateCompanyDTO>("company", model, HttpContext.Request.Cookies["access-token"]);

                Toastr("success", "Kayıt başarılı bir şekilde güncellendi.");

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Toastr("error", $"Kayıt güncellenirken hata oluştu : {ex.Message}");

                return View(model);
            }
        }

        // silme için
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _apiService.DeleteAsync<UpdateCompanyDTO>($"company", id, HttpContext.Request.Cookies["access-token"]);
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
        public async Task<IActionResult> Confirm([FromQuery] int companyId)
        {

                var company = await _apiService.GetByIdAsync<UpdateCompanyDTO>("company", companyId.ToString(), HttpContext.Request.Cookies["access-token"]);
                // Token doğrulandı, şirketi aktif hale getir
                // companyId parametresini kullanarak şirketi bulup aktifleştirme işlemlerini gerçekleştirin
                string body = $"Şirketiniz onaylandı. Lütfen giriş yapınız.<a href='https://localhost:7034/'>tıklayın</a>. ";
                string subject = "Registration Confirmation";
                await _emailService.SendEmailRegisterAsync(company.Email, subject, body);
                return RedirectToAction("Login", "Account");
      
        }
    }

}

