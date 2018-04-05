using RazorEngine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brass9.Web.RazorEngining
{
	public class RazorHtml<T>
	{
		protected RazorMvcTemplateBase<T> templateBase;

		public RazorHtml(RazorMvcTemplateBase<T> _templateBase)
		{
			this.templateBase = _templateBase;
		}

		public IEncodedString Raw(string rawHtml)
		{
			return new RawString(rawHtml);
		}

		// TODO: WTH is TemplateWriter? MVC users are gonna expect an MvcHtmlString
		public RazorEngine.Templating.TemplateWriter Partial(string viewName)
		{
			return templateBase.Include(viewName);
		}
	}
}
