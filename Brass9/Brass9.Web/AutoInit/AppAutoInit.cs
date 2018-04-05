using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brass9.AutoInit;
using Brass9.Reflection;

namespace Brass9.Web.AutoInit
{
	public static class AppAutoInit
	{
		public static void Init(AppBase app)
		{
			var types = ReflectionHelper.GetAllPublicClasses();

			var iAppAutoInit = typeof(IAppAutoInit);

			var needInit = types.Where(t =>
				iAppAutoInit.IsAssignableFrom(t) && !t.IsInterface
			).ToArray();

			foreach(var type in needInit)
			{
				var initMethod = ReflectionHelper.GetPublicStaticMethod(type, "Init");
				initMethod.Invoke(null, new object[] { app });
			}
		}
	}
}
