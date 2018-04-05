using Brass9.Web.RazorEngining;
using RazorEngineMvc.ViewModels;
using RazorEngineMvc.WebApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RazorEngineMvc.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		public async Task<ActionResult> SendTestEmail()
		{
			var vm = new TestEmailViewModel
			{
				FirstName = "Chris",
				LastName = "Moschini",
				Email = "chris@brass9.com"
			};

			var razor = RazorHelper.O;
			var html = razor.RenderFromMvc(@"Views\RazorEngine\TestEmail.cshtml", vm);

			var mailResult = await AppMail.O.SendHtmlAsync("Email to", "Subject", html, App.O.AppRoot);

			return View(mailResult);
		}
	}
}
