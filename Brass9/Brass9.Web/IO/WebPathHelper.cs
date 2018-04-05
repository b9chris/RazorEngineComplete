using System;
using System.Collections.Generic;
using System.Linq;


namespace Brass9.Web.IO
{
	/// <summary>
	/// No dependency on HttpContext
	/// </summary>
	public class WebPathHelper
	{
		/// <summary>
		/// Gets the physical path for a web path, but only a partial path (as much as was provided in web path).
		/// 
		/// Tries to be smart about directories - if the path appears to contain a file extension (like .css),
		/// leaves be. If not, it appends a "\" to the end to try to be consistent with other directory paths
		/// in .Net
		/// 
		/// If path begins with /, returned path begins with \
		/// </summary>
		/// <param name="webPath"></param>
		/// <returns></returns>
		public static string WebPathToPhysical(string webPath)
		{
			var physicalPath = webPath.Replace('/', '\\');
			if (!physicalPath.EndsWith(@"\") && !physicalPath.Contains("."))
				physicalPath += @"\";

			return physicalPath;
		}

		/// <summary>
		/// Gets the physical path for a web path, using appRoot to complete the full path.
		/// </summary>
		/// <param name="webPath">A web path like /ui/script/src.js</param>
		/// <param name="appRoot">The physical path appRoot like C:\a\b\c\</param>
		/// <returns>A full path like C:\a\b\c\ui\script\src.js</returns>
		public static string WebPathToPhysical(string webPath, string appRoot)
		{
			string physicalPath = WebPathToPhysical(webPath);

			if (physicalPath.StartsWith(@"~\"))
				return appRoot + physicalPath.Substring(2);

			if (physicalPath.StartsWith(@"\"))
				return appRoot + physicalPath.Substring(1);

			return appRoot + physicalPath;
		}

		public static string PhysicalPathToWeb(string physicalPath, string appRoot)
		{
			return physicalPath.Substring(appRoot.Length - 1).Replace('\\', '/');
		}
	}
}
