using HR_Project.Common.Models.DTOs;
using HR_Project.Presentation.APIService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using HR_Project.Domain.Entities.Concrete;

namespace HR_Project.Presentation.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAPIService _apiService;

        public AccountController(IAPIService apiService)
        {
            _apiService = apiService;
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
            return RedirectToAction("Login","Account");
        }
    }
}
