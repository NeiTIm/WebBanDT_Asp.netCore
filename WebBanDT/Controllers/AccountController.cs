using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebBanDT.Models;
using WebBanDT.Models.ViewModels;

namespace WebBanDT.Controllers
{
	public class AccountController : Controller
	{
		private UserManager<AppUserModel> _userManager;
		private SignInManager<AppUserModel> _signInManager;

		public AccountController(SignInManager<AppUserModel> signInManager, UserManager<AppUserModel> userManager)
		{
			_signInManager = signInManager;
			_userManager = userManager;
		}

		// Hiển thị trang login - GET
		[HttpGet]
		public IActionResult Login(string returnUrl)
		{
			return View(new LoginViewModel { ReturnUrl = returnUrl });
		}

		// Xử lý dữ liệu đăng nhập - POST
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, isPersistent: false, lockoutOnFailure: false);

				if (result.Succeeded)
				{
					return Redirect(model.ReturnUrl ?? "/");
				}
				ModelState.AddModelError(string.Empty, "Đăng nhập không thành công.");
			}
			return View(model);
		}

		// Hiển thị trang tạo user - GET
		public IActionResult Create()
		{
			return View();
		}

		// Xử lý việc tạo user - POST
		[HttpPost]
	
		public async Task<IActionResult> Create(UserModel user)
		{
			if (ModelState.IsValid)
			{
				AppUserModel newUser = new AppUserModel { UserName = user.Username, Email = user.Email };
				IdentityResult result = await _userManager.CreateAsync(newUser, user.Password);

				if (result.Succeeded)
				{
					TempData["success"] = "Tạo user thành công";
					return Redirect("/account/login");
				}

				foreach (IdentityError error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
			}
			return View(user);
		}
	}
}
