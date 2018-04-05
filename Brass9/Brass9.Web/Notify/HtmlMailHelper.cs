using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Brass9.Text.RegularExpressions;
using Brass9.Web.IO;

namespace Brass9.Web.Notify
{
	public class HtmlMailHelper
	{
		/// <summary>
		/// Parses out all images in an email, as follows:
		/// 
		/// 1) Looks for images with absolute paths like img src="/ui...
		/// 
		/// 2) For each one breaks out its path
		/// 
		/// 3) Replaces the paths with MIME cids like src="cid:0"
		/// 
		/// 4) Emits in index-order the physical paths of these images into the returned List
		/// </summary>
		/// <param name="appRoot"></param>
		/// <param name="html"></param>
		/// <returns></returns>
		public ParseImageResults ParseImages(string appRoot, string html)
		{
			var regex = new Regex(@"(<img[^>]+src="")(/[^""]+)("")");

			var attachments = new Dictionary<string, int>();
			int cid = 0;
			var updatedHtml = regex.Replace(html, new MatchEvaluator(match =>
			{
				var parts = RegexHelper.GetMatchCaptures(match);
				var attachmentWebPath = parts[1];
				var attachmentPhysicalPath = WebPathHelper.WebPathToPhysical(attachmentWebPath, appRoot);

				// Only add this attachment if it's not a repeat
				int currentCid;
				if (!attachments.TryGetValue(attachmentPhysicalPath, out currentCid))
				{
					currentCid = cid;
					attachments.Add(attachmentPhysicalPath, currentCid);
					cid++;
				}

				return parts[0] + "cid:" + currentCid.ToString() + parts[2];
			}));

			return new ParseImageResults
			{
				Html = updatedHtml,
				Attachments = attachments.Keys.ToList()
			};
		}

		public class ParseImageResults
		{
			public string Html { get; set; }
			public List<string> Attachments { get; set; }
		}
	}
}
