using Ecommerce.DTO;
using Ecommerce.Models;
using Ecommerce.RepoServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace Ecommerce.Controllers
{
	[Authorize(Roles = "User")]
	public class UserController : Controller
	{
		private readonly IUserRepo UserRepo;


		public UserController(IUserRepo userRepo)
		{
			UserRepo = userRepo;
		}

		[HttpGet]
		public IActionResult Edit()
		{
			return View(UserRepo.GetDetails(UserRepo.GetCurrentLoggedInUserId()));
		}
		public IActionResult Details()
		{
			return View(UserRepo.GetDetails(UserRepo.GetCurrentLoggedInUserId()));
		}
		[HttpPost]
		public IActionResult Edit(string id, [Bind("Fname,Lname,Email,Address")] User user)
		{
			if (ModelState.IsValid)
			{
				UserRepo.Update(id, user);
				return RedirectToAction("Details");
			}
			return View(user);
		}
		[HttpGet]
		public IActionResult ChangePassword()
		{
			ViewBag.Id = UserRepo.GetCurrentLoggedInUserId();
			return View(new ResetPassDTO());
		}
		[HttpPost]
		public async Task<IActionResult> ChangePassword(ResetPassDTO newPass)
		{

			if (ModelState.IsValid)
			{
				try
				{
					await UserRepo.ResetPassword(newPass);
					return RedirectToAction("Details");

				}
				catch
				{
					return View(newPass);
				}
			}
			return View(newPass);
		}

	}
}
