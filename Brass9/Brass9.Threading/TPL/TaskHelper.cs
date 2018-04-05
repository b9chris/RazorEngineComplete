using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brass9.Threading.TPL
{
	public static class TaskHelper
	{
		/// <summary>
		/// Runs a TPL Task fire-and-forget style, the right way - in the background, separate from the current thread,
		/// with no risk of it trying to rejoin the current thread.
		/// 
		/// Usage:
		/// 
		/// TaskHelper.RunBg(() => methodAsync(args));
		/// 
		/// or
		/// 
		/// TaskHelper.RunBg(async () => {
		///		await methodAsync1();
		///		await methodAsync2();
		/// });
		/// </summary>
		/// <param name="fn">Func Task or if you like, an async Action that contains an await (these are technically
		/// Func Tasks underneath the hood).</param>
		public static void RunBg(Func<Task> fn)
		{
			Task.Run(fn).ConfigureAwait(false);
		}

		/// <summary>
		/// Runs a task fire-and-forget style and notifies the TPL that this will not need a Thread to resume on
		/// for a long time, or that there are multiple gaps in thread use that may be long.
		/// Use for example when talking to a slow webservice.
		/// </summary>
		public static void RunBgLong(Func<Task> fn)
		{
			Task.Factory.StartNew(fn, TaskCreationOptions.LongRunning)
				.ConfigureAwait(false);
		}

		/// <summary>
		/// Vary the Task Scheduler based on a flag, in case this can be determined at runtime not compiletime.
		/// </summary>
		public static void RunBg(Func<Task> fn, bool isLong)
		{
			if (isLong)
				RunBgLong(fn);
			else
				RunBg(fn);
		}

		/*public static void RunBg(Action action)
		{
			#pragma warning disable 4014 // Fire and forget.
			Task.Run(action).ConfigureAwait(false);
			#pragma warning restore 4014
		}*/
	}
}
