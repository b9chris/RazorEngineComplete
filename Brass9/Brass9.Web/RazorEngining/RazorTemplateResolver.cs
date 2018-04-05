using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brass9.Web.RazorEngining
{
	// https://stackoverflow.com/a/11798922/176877
	public class RazorTemplateResolver : ITemplateResolver
	{
		protected string appRoot;
		protected string startingViewPath;

		public RazorTemplateResolver(string appRoot, string startingViewPath)
		{
			this.appRoot = appRoot;
			this.startingViewPath = startingViewPath;
		}

		public string Resolve(string name)
		{
			var path = resolvePath(name);
			if (path == null)
				throw new Exception("Could not find Partial or View or Layout " + name);

			return System.IO.File.ReadAllText(path, UTF8Encoding.UTF8);
		}

		protected string resolvePath(string name)
		{
			// Does it contain backslashes? If so we've got the whole path right now just return
			if (name.Contains("\\"))
				return name;

			// Does it contain forward slashes? If so it's an absolute path inside the Project, convert and return
			if (name.Contains("/"))
				return Brass9.Web.IO.WebPathHelper.WebPathToPhysical(name, appRoot);

			// Look in the current folder
			string currFolder = startingViewPath;
			string potentialViewPath = null;
			while (currFolder != appRoot)
			{
				// Work our way up
				currFolder = currFolder.Substring(0, currFolder.LastIndexOf('\\', currFolder.Length - 2) + 1);
				potentialViewPath = currFolder + name + ".cshtml";
				if (System.IO.File.Exists(potentialViewPath))
					return potentialViewPath;

				// Special check - is this folder named Views? If so, check a Shared subfolder
				//if ()	// Ah who cares just check everyone's Shared folder for now
				potentialViewPath = currFolder + @"Shared\" + name + ".cshtml";
				if (System.IO.File.Exists(potentialViewPath))
					return potentialViewPath;
			}

			// Look back down at Views/Shared
			potentialViewPath = appRoot + @"Views\Shared\" + name + ".cshtml";
			if (System.IO.File.Exists(potentialViewPath))
				return potentialViewPath;

			// Give up
			return null;	// Throw for not resolved?
		}
	}
}
