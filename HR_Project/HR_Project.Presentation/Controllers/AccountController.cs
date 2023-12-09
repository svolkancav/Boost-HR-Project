using HR_Project.Common.Models.DTOs;
using HR_Project.Presentation.APIService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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

                        Response.Cookies.Append("access_token", token.Token, new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            SameSite = SameSiteMode.None,
                            Expires = token.Expiration
                        });


                        var handler = new JwtSecurityTokenHandler();
                        var jsonToken = handler.ReadToken(token.Token) as JwtSecurityToken;

                        var email = jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                        var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

                        var identity = new ClaimsIdentity(claims, "login");
                        ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);



                        return RedirectToAction("Index");
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

            Response.Cookies.Delete("access_token");
            return RedirectToAction("Login");
        }
    }
}
