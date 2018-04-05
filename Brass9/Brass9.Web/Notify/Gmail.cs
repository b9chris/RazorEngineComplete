using System;
using System.Collections.Generic;
using System.Linq;


namespace Brass9.Web.Notify
{
	/// <summary>
	/// Typical usage:
	/// 
	/// Subclass for app-specific version and add a standard Singleton.
	/// 
	/// In subclass set the 5 args for protected default constructor, with 30000, 3 for timeout, retries.
	/// </summary>
	public class Gmail : Mail
	{
		public Gmail()
		{
			host = "smtp.gmail.com";
			port = 587;
			enableSsl = true;
		}

		public Gmail(string username, string password, int timeout, int retries, string from)
			: this()
		{
			this.username = username;
			this.password = password;
			this.timeout = timeout;
			this.retries = retries;
			this.from = from;
		}
	}
}
