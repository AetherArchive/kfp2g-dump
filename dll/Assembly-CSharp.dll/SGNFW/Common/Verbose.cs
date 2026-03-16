using System;
using System.Diagnostics;
using UnityEngine;

namespace SGNFW.Common
{
	public class Verbose<T> where T : class
	{
		public static bool Enabled { get; set; }

		[Conditional("SGNFW_DEBUG_LOG")]
		public static void Log(object message, Object context = null)
		{
			if (!Verbose<T>.Enabled)
			{
				return;
			}
			global::UnityEngine.Debug.Log(message, context);
		}

		[Conditional("SGNFW_DEBUG_LOG")]
		public static void LogFormat(string format, params object[] args)
		{
			if (!Verbose<T>.Enabled)
			{
				return;
			}
			global::UnityEngine.Debug.LogFormat(format, args);
		}

		[Conditional("SGNFW_DEBUG_LOG")]
		[Conditional("ENABLE_DMM_RELEASE_ERROR_LOG")]
		public static void LogError(object message, Object context = null)
		{
			global::UnityEngine.Debug.LogError(message, context);
		}

		[Conditional("SGNFW_DEBUG_LOG")]
		public static void LogErrorFormat(string format, params object[] args)
		{
			if (!Verbose<T>.Enabled)
			{
				return;
			}
			global::UnityEngine.Debug.LogErrorFormat(format, args);
		}

		[Conditional("SGNFW_DEBUG_LOG")]
		public static void LogException(Exception exception, Object context = null)
		{
			if (!Verbose<T>.Enabled)
			{
				return;
			}
			global::UnityEngine.Debug.LogException(exception, context);
		}

		[Conditional("SGNFW_DEBUG_LOG")]
		public static void LogWarning(object message, Object context = null)
		{
			if (!Verbose<T>.Enabled)
			{
				return;
			}
			global::UnityEngine.Debug.LogWarning(message, context);
		}

		[Conditional("SGNFW_DEBUG_LOG")]
		public static void LogWarningFormat(string format, params object[] args)
		{
			if (!Verbose<T>.Enabled)
			{
				return;
			}
			global::UnityEngine.Debug.LogWarningFormat(format, args);
		}

		[Conditional("SGNFW_DEBUG_LOG")]
		public static void Assert(bool condition)
		{
		}

		[Conditional("SGNFW_DEBUG_LOG")]
		public static void Assert(bool condition, string message)
		{
		}

		[Conditional("SGNFW_DEBUG_LOG")]
		public static void AssertFormat(bool condition, string format, params object[] args)
		{
		}

		[Conditional("SGNFW_DEBUG_LOG")]
		public static void LogObject<Type>(Type obj, string message = "")
		{
			XmlUtil.ToXmlString<Type>(obj);
		}

		public const string CONDITION_SWITCH = "SGNFW_DEBUG_LOG";

		public const string CONDITION_SWITCH_DMM = "ENABLE_DMM_RELEASE_ERROR_LOG";
	}
}
