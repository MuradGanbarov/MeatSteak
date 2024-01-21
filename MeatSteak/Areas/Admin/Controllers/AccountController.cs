using MeatSteak.Areas.Admin.Models.Utilities.Enums;
using MeatSteak.Areas.Admin.ViewModels;
using MeatSteak.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MeatSteak.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class AccountController : Controller
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,RoleManager<IdentityRole> roleManager)
        {
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
		}
        public IActionResult Register()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Register(RegisterVM vm)
		{
			if (!ModelState.IsValid) return View();

			AppUser user = new()
			{
				Name = vm.Name,
				Surname = vm.Surname,
				UserName = vm.UserName,
				Email = vm.Email,
			};

			var result = await _userManager.CreateAsync(user,vm.Password);
			if (!result.Succeeded)
			{
				foreach(IdentityError error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
				return View(vm);
			}
			await _userManager.AddToRoleAsync(user,UserRoles.Admin.ToString());
			await _signInManager.SignInAsync(user, isPersistent:false);
			return RedirectToAction("Index", "Home", new { Area = "" });
		}

		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home", new {Area=""});
		}

		public IActionResult Login()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Login(LoginVM vm)
		{
			if(!ModelState.IsValid) return View();

			AppUser user = await _userManager.FindByEmailAsync(vm.UserNameOrEmail);
			if(user is null)
			{
				user = await _userManager.FindByNameAsync(vm.UserNameOrEmail);
				if(user is null)
				{
					ModelState.AddModelError(string.Empty, "Email,username or password is incorrect");
					return View();
				}
			}

			var result = await _signInManager.PasswordSignInAsync(user, vm.Password, vm.IsRemember, true);
			if(result.IsLockedOut)
			{
				ModelState.AddModelError(string.Empty, "We have maintaince at the moment,please try again later");
				return View();
			}
			if(!result.Succeeded)
			{
				ModelState.AddModelError(string.Empty, "Email, username or password is incorrect");
				return View();
			}
			return RedirectToAction("Index", "Home", new { Area = "" });

		}
		
		public async Task<IActionResult> CreateRoles()
		{
			foreach(UserRoles role in Enum.GetValues(typeof(UserRoles)))
			{
				if(!await _roleManager.RoleExistsAsync(role.ToString()))
				{
					await _roleManager.CreateAsync( new IdentityRole
					{
						Name = role.ToString(),
					});
				}
			}
			return RedirectToAction("Index", "Home", new { Area = "" });
		}

	}
}
