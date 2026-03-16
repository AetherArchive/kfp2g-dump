using System;
using System.Diagnostics;
using UnityEngine;

namespace SegaUnityCEFBrowser
{
	public static class DebugOut
	{
		public static bool isDebugBuild
		{
			get
			{
				return global::UnityEngine.Debug.isDebugBuild;
			}
		}

		[Conditional("DISP_DEBUG_LOG_SUCEFBROWSER")]
		public static void Log(object message)
		{
		}

		[Conditional("DISP_DEBUG_LOG_SUCEFBROWSER")]
		public static void LogFormat(string format, params object[] args)
		{
		}

		[Conditional("DISP_DEBUG_LOG_SUCEFBROWSER")]
		public static void LogError(object message)
		{
		}

		[Conditional("DISP_DEBUG_LOG_SUCEFBROWSER")]
		public static void LogErrorFormat(string format, params object[] args)
		{
		}

		[Conditional("DISP_DEBUG_LOG_SUCEFBROWSER")]
		public static void Assert(bool condition, object message)
		{
		}

		[Conditional("DISP_DEBUG_LOG_SUCEFBROWSER")]
		public static void AssertFormat(bool condition, string format, params object[] args)
		{
		}

		[Conditional("DISP_DEBUG_LOG_SUCEFBROWSER")]
		public static void LogException(Exception exception)
		{
		}
	}
}
