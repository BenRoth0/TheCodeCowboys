using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;

namespace PromptQuest.Controllers
{
	public class AccountController : Controller
	{
		public IActionResult GoogleLogin()
		{
			var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };
			return Challenge(properties, GoogleDefaults.AuthenticationScheme);
		}

		public async Task<IActionResult> GoogleResponse()
		{
			var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			if (result?.Principal != null)
			{
				// Handle successful log in here

				// Redirect to the home page after logging in
				return RedirectToAction("Index", "Home");
			}
			return RedirectToAction("Login");
		}

		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			// Return to the home page after logging out
			return RedirectToAction("Index", "Home");
		}
	}
}