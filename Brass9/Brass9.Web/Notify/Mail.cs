using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

// ParallelTaskExtensions
using System.Net.NetworkInformation;
using Brass9.Web.RazorEngining;


namespace Brass9.Web.Notify
{
	public class Mail
	{
		protected string username;
		protected string password;
		protected string host;
		protected int port;
		protected bool enableSsl;
		protected int timeout;
		protected int retries;
		protected string from;

		public Mail()
		{
		}
		public Mail(string username, string password, string host, int port, bool enableSsl, int timeout, int retries, string from)
		{
			this.username = username;
			this.password = password;
			this.host = host;
			this.port = port;
			this.enableSsl = enableSsl;
			this.timeout = timeout;
			this.retries = retries;
			this.from = from;
		}


		/// <summary>
		/// Sets the from property to the standard name-email format used in email - just easy to forget, may as well
		/// put it in a method.
		/// </summary>
		/// <param name="name">User-friendly name for their Address Books.</param>
		/// <param name="email">Valid email address this is from.</param>
		protected void setFrom(string name, string email)
		{
			this.from = name + " <" + email + ">";
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="to"></param>
		/// <param name="subject"></param>
		/// <param name="body"></param>
		/// <param name="isHtml"></param>
		/// <param name="failWillRetry">An Action to call when a failure occurs and there are still retries left</param>
		/// <returns></returns>
		public virtual async Task<MailResult> SendAsync(string to, string subject, string body, bool isHtml = false, List<string> attachments = null, Action failWillRetry = null)
		{
			var context = new MailContext
			{
				To = to,
				Subject = subject,
				Body = body,
				IsHtml = isHtml,
				Attachments = attachments
			};

			int delay = 10000;
			while (!context.Success && context.RetriesTried <= retries)
			{
				// TODO: Offer various retry/wait strategies?

				// TODO: make email all go into a MessageQueue we can inspect, and make exponential falloff etc be governed by a
				// Dequeue loop, not each individual email Task, so we don't hit Gmail with any more than one attempt each try
				// after a failure.
				// 1) Retry immediately in case the connection/GFE flaked,
				// 2) Notify a central scheduler the SMTP service is likely down, hold up all emails,
				// 3) Central scheduler takes over from there and tries exactly one email in the queue at random
				// at an exponential interval until the SMTP service recovers
				// Might also be better to just deposit emails into a Message Queue and let the worker that manages that handle that kind of
				// coordination.
				await sendAsync(context);
				if (!context.Success)
				{
					if (failWillRetry != null && context.RetriesTried < retries)
						failWillRetry();

					// Exponential falloff
					await Task.Delay(delay);
					delay *= 3;
				}
			}

			return new MailResult(context.Success, context.Exceptions);
		}

		/// <summary>
		/// Sends an HTML email with images parsed out and attached as MIME-attachments
		/// </summary>
		/// <param name="to"></param>
		/// <param name="subject"></param>
		/// <param name="html"></param>
		/// <returns></returns>
		public async Task<MailResult> SendHtmlAsync(string to, string subject, string html, string appRoot)
		{
			var helper = new HtmlMailHelper();
			var parseResult = helper.ParseImages(appRoot, html);
			return await SendAsync(to, subject, parseResult.Html, true, parseResult.Attachments);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TViewModel"></typeparam>
		/// <param name="to"></param>
		/// <param name="subject"></param>
		/// <param name="razorPath">A system path like @"Areas\Home\Views\RazorEngine\Invite.cshtml"</param>
		/// <param name="viewModel"></param>
		/// <returns></returns>
		public async Task<MailResult> SendRazorAsync<TViewModel>(string to, string subject, string razorPath, TViewModel viewModel)
		{
			var razor = RazorHelper.O;
			string html = razor.Render(razorPath, viewModel);

			string appRoot = razor.AppRoot;
			var helper = new HtmlMailHelper();
			var parseResult = helper.ParseImages(appRoot, html);
			html = parseResult.Html;

			return await SendAsync(to, subject, html, true, parseResult.Attachments);
		}

		protected async Task<MailContext> sendAsync(MailContext context)
		{
			MailMessage mail = new MailMessage(from, context.To, context.Subject, context.Body);
			mail.IsBodyHtml = context.IsHtml;

			if (context.Attachments != null)
			{
				int i = 0;
				foreach(var attach in context.Attachments)
				{
					var attachment = new Attachment(attach);
					attachment.ContentId = i.ToString();
					mail.Attachments.Add(attachment);
					i++;
				}
			}

			SmtpClient smtp = new SmtpClient(host, port);
			smtp.Credentials = new NetworkCredential(username, password);
			smtp.EnableSsl = enableSsl;
			smtp.Timeout = timeout;

			try
			{
				await smtp.SendTask(mail, context);
			}
			catch (SmtpException ex)
			{
				context.Exceptions.Add(ex);
				context.RetriesTried++;
				return context;
			}

			context.Success = true;
			return context;
		}

		protected internal class MailContext
		{
			public string To;
			public string Subject;
			public string Body;
			public bool IsHtml;
			public List<string> Attachments = new List<string>();

			public int RetriesTried;
			public bool Success;
			public List<Exception> Exceptions = new List<Exception>();
		}
	}
}
