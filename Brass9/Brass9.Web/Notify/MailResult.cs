using System;
using System.Collections.Generic;
using System.Linq;


namespace Brass9.Web.Notify
{
	public class MailResult
	{
		public bool Success { get; protected set; }
		public Exception[] Exceptions { get; protected set; }

		public MailResult(bool success, List<Exception> exceptions)
		{
			Success = success;
			Exceptions = exceptions.ToArray();
		}
	}
}
