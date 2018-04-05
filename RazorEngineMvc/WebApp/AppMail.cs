using Brass9.Web.Notify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace RazorEngineMvc.WebApp
{
	public class AppMail : Brass9.Web.Notify.Mail
	{
		#region Singleton
		public static AppMail O { get { return Nested.instance; } }
		class Nested
		{
			static Nested() { }
			internal static readonly AppMail instance = new AppMail();
		}
		#endregion

		public AppMail()
		{
			this.username = "TODO";
			this.password = "TODO";
			this.host = "TODO";
			this.enableSsl = true;
			this.port = 587;//995 POP
			this.setFrom("Name", "Email");
			this.timeout = 2000;
			this.retries = 0;
		}

		public override async Task<MailResult> SendAsync(string to, string subject, string body,
			bool isHtml = false, List<string> attachments = null, Action failWillRetry = null)
		{
			return await base.SendAsync(to, subject, body, isHtml, attachments, failWillRetry);
		}
	}
}