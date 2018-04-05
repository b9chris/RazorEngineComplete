using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brass9.Web.RazorEngining
{
	/// <summary>
	/// Provides the base class for RazorEngine templates to inherit from.
	/// Allows RazorEngine to better mimic MVC, esp providing the Html property
	/// </summary>
	public abstract class RazorMvcTemplateBase<T> : TemplateBase<T>
	{
		public RazorHtml<T> Html { get; protected set; }
		//public string ExecutingPath { get; protected set; }

		public RazorMvcTemplateBase()
		{
			Html = new RazorHtml<T>(this);
		}
		/*public RazorMvcTemplateBase(string razorFile)
			: this()
		{
			ExecutingPath = razorFile;
		}*/
	}
}
