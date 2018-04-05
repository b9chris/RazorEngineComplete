using RazorEngineMvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RazorEngineMvc.Controllers
{
    public class RazorEngineController : Controller
    {
        public ActionResult TestEmail()
        {
			var vm = new TestEmailViewModel
			{
				FirstName = "Chris",
				LastName = "Moschini",
				Email = "chris@brass9.com"
			};
            return View(vm);
        }
    }
}
