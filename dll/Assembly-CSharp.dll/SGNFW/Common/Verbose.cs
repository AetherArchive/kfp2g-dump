using System;
using System.Diagnostics;
using UnityEngine;

namespace SGNFW.Common
{
	// Token: 0x0200025F RID: 607
	public class Verbose<T> where T : class
	{
		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x060025E5 RID: 9701 RVA: 0x001A0BB3 File Offset: 0x0019EDB3
		// (set) Token: 0x060025E4 RID: 9700 RVA: 0x001A0BAB File Offset: 0x0019EDAB
		public static bool Enabled { get; set; }

		// Token: 0x060025E6 RID: 9702 RVA: 0x001A0BBA File Offset: 0x0019EDBA
		[Conditional("SGNFW_DEBUG_LOG")]
		public static void Log(object message, Object context = null)
		{
			if (!Verbose<T>.Enabled)
			{
				return;
			}
			global::UnityEngine.Debug.Log(message, context);
		}

		// Token: 0x060025E7 RID: 9703 RVA: 0x001A0BCB File Offset: 0x0019EDCB
		[Conditional("SGNFW_DEBUG_LOG")]
		public static void LogFormat(string format, params object[] args)
		{
			if (!Verbose<T>.Enabled)
			{
				return;
			}
			global::UnityEngine.Debug.LogFormat(format, args);
		}

		// Token: 0x060025E8 RID: 9704 RVA: 0x001A0BDC File Offset: 0x0019EDDC
		[Conditional("SGNFW_DEBUG_LOG")]
		[Conditional("ENABLE_DMM_RELEASE_ERROR_LOG")]
		public static void LogError(object message, Object context = null)
		{
			global::UnityEngine.Debug.LogError(message, context);
		}

		// Token: 0x060025E9 RID: 9705 RVA: 0x001A0BE5 File Offset: 0x0019EDE5
		[Conditional("SGNFW_DEBUG_LOG")]
		public static void LogErrorFormat(string format, params object[] args)
		{
			if (!Verbose<T>.Enabled)
			{
				return;
			}
			global::UnityEngine.Debug.LogErrorFormat(format, args);
		}

		// Token: 0x060025EA RID: 9706 RVA: 0x001A0BF6 File Offset: 0x0019EDF6
		[Conditional("SGNFW_DEBUG_LOG")]
		public static void LogException(Exception exception, Object context = null)
		{
			if (!Verbose<T>.Enabled)
			{
				return;
			}
			global::UnityEngine.Debug.LogException(exception, context);
		}

		// Token: 0x060025EB RID: 9707 RVA: 0x001A0C07 File Offset: 0x0019EE07
		[Conditional("SGNFW_DEBUG_LOG")]
		public static void LogWarning(object message, Object context = null)
		{
			if (!Verbose<T>.Enabled)
			{
				return;
			}
			global::UnityEngine.Debug.LogWarning(message, context);
		}

		// Token: 0x060025EC RID: 9708 RVA: 0x001A0C18 File Offset: 0x0019EE18
		[Conditional("SGNFW_DEBUG_LOG")]
		public static void LogWarningFormat(string format, params object[] args)
		{
			if (!Verbose<T>.Enabled)
			{
				return;
			}
			global::UnityEngine.Debug.LogWarningFormat(format, args);
		}

		// Token: 0x060025ED RID: 9709 RVA: 0x001A0C29 File Offset: 0x0019EE29
		[Conditional("SGNFW_DEBUG_LOG")]
		public static void Assert(bool condition)
		{
		}

		// Token: 0x060025EE RID: 9710 RVA: 0x001A0C2B File Offset: 0x0019EE2B
		[Conditional("SGNFW_DEBUG_LOG")]
		public static void Assert(bool condition, string message)
		{
		}

		// Token: 0x060025EF RID: 9711 RVA: 0x001A0C2D File Offset: 0x0019EE2D
		[Conditional("SGNFW_DEBUG_LOG")]
		public static void AssertFormat(bool condition, string format, params object[] args)
		{
		}

		// Token: 0x060025F0 RID: 9712 RVA: 0x001A0C2F File Offset: 0x0019EE2F
		[Conditional("SGNFW_DEBUG_LOG")]
		public static void LogObject<Type>(Type obj, string message = "")
		{
			XmlUtil.ToXmlString<Type>(obj);
		}

		// Token: 0x04001BD1 RID: 7121
		public const string CONDITION_SWITCH = "SGNFW_DEBUG_LOG";

		// Token: 0x04001BD2 RID: 7122
		public const string CONDITION_SWITCH_DMM = "ENABLE_DMM_RELEASE_ERROR_LOG";
	}
}
