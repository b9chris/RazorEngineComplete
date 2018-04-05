using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace Brass9.Text.RegularExpressions
{
	public class RegexHelper
	{
		public static IEnumerable<Match> MatchesGeneric(string regex, string s)
		{
			return MatchesGeneric(regex, s, 0);
		}
		public static IEnumerable<Match> MatchesGeneric(string regex, string s, int startIndex)
		{
			var r = new Regex(regex);
			return MatchesGeneric(r, s, startIndex);
		}
		public static IEnumerable<Match> MatchesGeneric(Regex regex, string s)
		{
			return MatchesGeneric(regex, s, 0);
		}
		public static IEnumerable<Match> MatchesGeneric(Regex regex, string s, int startIndex)
		{
			var matches = regex.Matches(s, startIndex).Cast<Match>();
			return matches;
		}

		/// <summary>
		/// Returns all the first groups in regex matches
		/// 
		/// For example, /(\w+ain)/ would return ["rain", "spain"], but /(\w+ain).*(a)/ wouldn't return anything more - assumes only
		/// one capturing group in the source regex.
		/// 
		/// Assumes only one grouped capture in the regex - doesn't work with multiple (ignores all but first)
		/// </summary>
		/// <param name="regex">A regex like "a(b)" (and not like "a(b)(c)")</param>
		/// <param name="s">A string like "ab cab blab stab"</param>
		/// <returns>An array of captures in groups only, like new string[] { "b", "b", "b", "b" }</returns>
		public static string[] ListAllFirstGroups(Regex regex, string s)
		{
			return ListAllFirstGroups(regex, s, 0);
		}
		public static string[] ListAllFirstGroups(string regex, string s)
		{
			return ListAllFirstGroups(new Regex(regex), s);
		}
		public static string[] ListAllFirstGroups(Regex regex, string s, int startIndex)
		{
			var matches = MatchesGeneric(regex, s, startIndex).Select(m => m.Groups[1].Value).ToArray();
			return matches;
		}
		public static string[] ListAllFirstGroups(string regex, string s, int startIndex)
		{
			return ListAllFirstGroups(new Regex(regex), s, startIndex);
		}



		/// <summary>
		/// Returns all the simple matches for a Regex. Useful when you have no () Captures.
		/// For example, a Regex "\w\w?ain" would return "rain", "plain", "Spain"
		/// </summary>
		/// <param name="regex"></param>
		/// <param name="s"></param>
		/// <returns></returns>
		public static string[] ListAllMatches(Regex regex, string s)
		{
			var matches = MatchesGeneric(regex, s);
			var result = matches.Select(m => m.Value).ToArray();
			return result;
		}
		public static string[] ListAllMatches(string regex, string s)
		{
			return ListAllMatches(new Regex(regex), s);
		}



		/// <summary>
		/// Gets the first match in a string and returns all grouped captures inside it
		/// For example, /(rain) in (spain)/ would return ["rain", "spain"]
		/// 
		/// Uses Match, not Matches - processes just the one match
		/// </summary>
		/// <param name="regex"></param>
		/// <param name="s"></param>
		/// <returns></returns>
		public static string[] ListAllGroups(Regex regex, string s)
		{
			var match = regex.Match(s);
			string[] groupCaptures = GetMatchCaptures(match);
			return groupCaptures;
		}
		public static string[] ListAllGroups(string regex, string s)
		{
			return ListAllGroups(new Regex(regex), s);
		}

		/// <summary>
		/// Returns the first capture in the match as a string. For example, regex @"(\d+)" on "12 34" returns "12".
		/// </summary>
		/// <param name="regex">A Regex object with a capture () in it</param>
		/// <param name="s">A string to run the Regex on</param>
		/// <returns>The first matched string, or null.</returns>
		public static string FirstGroup(string regex, string s)
		{
			return FirstGroup(new Regex(regex), s, 0);
		}
		public static string FirstGroup(string regex, string s, int startIndex)
		{
			return FirstGroup(new Regex(regex), s, startIndex);
		}
		public static string FirstGroup(Regex regex, string s)
		{
			return FirstGroup(regex, s, 0);
		}
		public static string FirstGroup(Regex regex, string s, int startIndex)
		{
			var first = MatchesGeneric(regex, s, startIndex).Select(m => m.Groups[1].Value).FirstOrDefault();
			return first;
		}

		/// <summary>
		/// Gets all captures from a Match. For example:
		/// 
		/// var match = (new Regex(@"(img )(src=)("")")).Match(html);
		/// var captures = GetMatchCaptures(match);
		/// 
		/// Captures would now contain an array of 3 strings, corresponding to the 3 captures (parentheses) in the regex.
		/// 
		/// NOTE: If nothing was found, match will be null and this method will throw a NullReferenceException.
		/// Consider checking for null before passing into this method.
		/// </summary>
		/// <param name="match"></param>
		/// <returns></returns>
		public static string[] GetMatchCaptures(Match match)
		{
			var groups = match.Groups.Cast<Group>();
			var values = groups.Select(g => g.Value);
			string[] groupCaptures = values.Skip(1).ToArray();
			return groupCaptures;
		}
	}
}
