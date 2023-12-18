using HR_Project.Common.Models.DTOs;
using HR_Project.Presentation.APIService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using HR_Project.Domain.Entities.Concrete;
using HR_Project.Application.Services.EmailService;
using Microsoft.AspNetCore.Identity;
using HR_Project.Domain.Enum;

namespace HR_Project.Presentation.Controllers
{
	public class AccountController : BaseController
	{
		private readonly IAPIService _apiService;
		private readonly IEmailService _emailService;
		private readonly UserManager<Personnel> _userManager;
		private readonly IPasswordHasher<Personnel> _passwordHasher;

		public AccountController(IAPIService apiService, IEmailService emailService, UserManager<Personnel> userManager, IPasswordHasher<Personnel> passwordHasher)
		{
			_apiService = apiService;
			_emailService = emailService;
			_userManager = userManager;
			_passwordHasher = passwordHasher;
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
		public IActionResult Register()

		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterDTO model)
		{

			if (ModelState.IsValid)
			{
				// Your registration logic here
				Personnel personnel = new Personnel()
				{
					Email = model.Email,
					PasswordHash = model.Password,
					Name = model.Name,
					Surname = model.Surname,
					Title = model.Title,
					PhoneNumber = model.PhoneNumber,
					Gender = model.Gender,
					Nation = model.Nation,
					AccountStatus = AccountStatus.Inactive
				};
				//TODO: 
				IdentityResult result = await _userManager.CreateAsync(personnel, model.Password);
				if (result.Succeeded)
				{
					var token = await _userManager.GenerateEmailConfirmationTokenAsync(personnel);
					var confirmationLink = Url.Action("Confirmation", "Account", new { id = personnel.Id, token }, Request.Scheme);
					// Send registration confirmation email
					string subject = "Registration Confirmation";
					string body = "Kayıt işlemini gerçekleştirmek için linke tıklayınız: " + confirmationLink;

					await _emailService.SendEmailRegisterAsync(personnel.Email, subject, body);

					ViewBag.ErrorTitle = "Registration successful";
					ViewBag.ErrorMessage = "Before you can Login, please confirm your " +
							"email, by clicking on the confirmation link we have emailed you";
					// Redirect or return a success message
					return RedirectToAction("Login", "Account");
				}
				else
				{
					foreach (IdentityError error in result.Errors)
					{
						ModelState.AddModelError("", error.Description);

					}
					return View();
				}

			}
			return RedirectToAction("Confirmation");



		}
		[HttpGet]
		public async Task<IActionResult> Confirmation(int id, string token)
		{


			if (id == null || token == null)
			{
				return RedirectToAction("login", "account");
			}


			var user = await _userManager.FindByIdAsync(id.ToString());
			RegisterDTO registerDTO = new RegisterDTO()
			{
				Email = user.Email,
				AccountStatus = user.AccountStatus,
				Gender = user.Gender,
				Nation = user.Nation,
				Name = user.Name,
				Password = user.PasswordHash,
				PhoneNumber = user.PhoneNumber,
				Surname = user.Surname,
				Title = user.Title

			};
			if (user == null)
			{
				ViewBag.ErrorMessage = $"The User ID {id} is invalid";
				return View("NotFound");
			}

			var result = await _userManager.ConfirmEmailAsync(user, token);
			if (result.Succeeded)
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
	}
}
