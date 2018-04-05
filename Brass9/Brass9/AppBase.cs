using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Brass9
{
	/*	
	Implement like
		protected App()
		{
			string rootNs = this.GetType().Namespace;
			rootNs = rootNs.Substring(0, rootNs.IndexOf('.'));
			this.RootNs = rootNs;
#if MIN
			this.IsMin = true;
#endif
		}
	
		public static App O { get; protected set; }

		public static void Init(string appRoot)
		{
			O = new App
			{
				AppRoot = appRoot,
			};

			Brass9.Web.AutoInit.AppAutoInit.Init(this);
		}

		public const int DefaultCacheDuration =
#if DEBUG
			0;
#else
			4*60*60;	// 4 hours
#endif
	*/
	public class AppBase
	{
		public string AppRoot { get; protected set; }
		public string RootNs { get; protected set; }
		public Assembly RootAssembly { get; protected set; }
		public bool IsMin { get; protected set; }

		
		protected AppBase()
		{
		}

		protected AppBase(string appRoot, bool isMin)
		{
			if (!appRoot.EndsWith(@"\"))
				appRoot += @"\";
			AppRoot = appRoot;
			this.IsMin = IsMin;
		}
	}
}
