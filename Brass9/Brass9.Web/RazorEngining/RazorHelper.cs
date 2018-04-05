using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RazorEngine.Templating;

namespace Brass9.Web.RazorEngining
{
	public class RazorHelper : Brass9.AutoInit.IAppAutoInit
	{
		public static RazorHelper O { get; protected set; }
		public static void Init(AppBase app)
		{
			O = new RazorHelper { AppRoot = app.AppRoot };
		}

		public string AppRoot { get; protected set; }

		public string Render<TViewModel>(string path, TViewModel vm)
		{
			string templateKey = path;	// Just reuse the path as a unique key to the template
			var razor = RazorEngine.Engine.Razor;
			var tvm = typeof(TViewModel);
			if (!razor.IsTemplateCached(templateKey, tvm))
			{
				var template = System.IO.File.ReadAllText(AppRoot + path);
				razor.AddTemplate(templateKey, template);
			}

			string html = RazorEngine.Engine.Razor.RunCompile(templateKey, typeof(TViewModel), vm);
			return html;
		}

		public string RenderFromMvc<TViewModel>(string path, TViewModel vm)
		{
			string templateKey = path;  // Just reuse the path as a unique key to the template
			var viewPath = AppRoot + path;
			//var razor = RazorEngine.Engine.Razor;
			// https://matthid.github.io/RazorEngine/TemplateBasics.html#Extending-the-template-Syntax
			var config = new RazorEngine.Configuration.TemplateServiceConfiguration();
			config.BaseTemplateType = typeof(RazorMvcTemplateBase<>);
			config.Resolver = new RazorTemplateResolver(AppRoot, viewPath);
			//config.TemplateManager
			using (var razor = RazorEngineService.Create(config))
			{
				var tvm = typeof(TViewModel);
				if (!razor.IsTemplateCached(templateKey, tvm))
				{
					var template = System.IO.File.ReadAllText(viewPath);
					//template = FromMvcRazorToRazorEngine(template);
					razor.AddTemplate(templateKey, template);
				}				

				string html = razor.RunCompile(templateKey, typeof(TViewModel), vm);
				return html;
			}
		}

		/*public string FromMvcRazorToRazorEngine(string textInput)
		{
			return textInput
				.Replace("@Html.Raw(", "@Raw(")
				// https://stackoverflow.com/a/27671257/176877
				.Replace("@Html.Partial(", "@Include(");
		}*/
	}
}
