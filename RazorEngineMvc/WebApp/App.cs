using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RazorEngineMvc.WebApp
{
	public class App : Brass9.AppBase
	{
		public static App O { get; protected set; }

		public static void Init(string appRoot)
		{
			O = new App
			{
				AppRoot = appRoot,
			};
		}

		protected App()
		{
			string rootNs = this.GetType().Namespace;
			rootNs = rootNs.Substring(0, rootNs.IndexOf('.'));
			this.RootNs = rootNs;
		}

		public void InitModules()
		{
			Brass9.Web.AutoInit.AppAutoInit.Init(this);
		}
	}
}
