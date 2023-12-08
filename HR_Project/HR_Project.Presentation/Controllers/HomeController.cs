using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using HR_Project.Common.Models.DTOs;
using HR_Project.Common.Models.VMs;
using HR_Project.Presentation.APIService;
using HR_Project.Presentation.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace HR_Project.Presentation.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IAPIService _apiService;

		public HomeController(ILogger<HomeController> logger, IAPIService apiService)
		{
			_logger = logger;
			_apiService = apiService;
		}

		public async Task<IActionResult> Index()
		{
			var advance=await _apiService.GetAsync<List<AdvanceVM>>(endpoint: "advance", HttpContext.Request.Cookies["access-token"]);

			CreateAdvanceDTO createAdvanceDTO = new CreateAdvanceDTO();

			await _apiService.PostAsync<CreateAdvanceDTO, CreateAdvanceDTO>("advance", createAdvanceDTO, HttpContext.Request.Cookies["access-token"]);

			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		[HttpPost]
		public async Task<IActionResult> Create(CreateAdvanceDTO model)
		{
			await _apiService.PostAsync<CreateAdvanceDTO, CreateAdvanceDTO>("advance", model, HttpContext.Request.Cookies["access-token"]);

			return RedirectToAction("Index");
		}


		//[HttpPost]
		//public async Task<IActionResult> Login(LoginDTO model)
		//{
		//	try
		//	{
		//		if (ModelState.IsValid)
		//		{
		//			var token = await _apiService.LoginAsync(model);

		//			if (token != null)
		//			{


		//				#region token ı cookie ye atma
		//				Response.Cookies.Append("access_token", token.Token, new CookieOptions
		//				{
		//					HttpOnly = true,
		//					Secure = true,
		//					SameSite = SameSiteMode.None,
		//					Expires = token.expiration
		//				});
		//				#endregion



		//				var handler = new JwtSecurityTokenHandler();
		//				var jsonToken = handler.ReadToken(token.Token) as JwtSecurityToken;

		//				var username = jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

		//				var claims = new List<Claim>
		//			{
		//				new Claim(ClaimTypes.Name, username),
		//				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
		//			};

		//				var identity = new ClaimsIdentity(claims, "login");
		//				ClaimsPrincipal principal = new ClaimsPrincipal(identity);
		//				await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);



		//				return RedirectToAction("Index", "Genre");
		//			}
		//			else
		//			{
		//				ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı");
		//			}
		//		}
		//	}
		//	catch (Exception ex)
		//	{

		//		ModelState.AddModelError("", $"Giriş işlemi başarısız: {ex.Message}");
		//	}
		//	return View();
		//}
	}
}