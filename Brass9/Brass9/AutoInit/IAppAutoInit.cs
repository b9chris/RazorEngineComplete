using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brass9.AutoInit
{
	/// <summary>
	/// Apply to classes that offer a static init method like:
	/// 
	/// public static void Init(Brass9.AppBase app)
	/// </summary>
	public interface IAppAutoInit
	{
	}
}
