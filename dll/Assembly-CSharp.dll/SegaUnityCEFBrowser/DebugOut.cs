using System;
using System.Diagnostics;
using UnityEngine;

namespace SegaUnityCEFBrowser
{
	// Token: 0x0200020A RID: 522
	public static class DebugOut
	{
		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x06002227 RID: 8743 RVA: 0x001920A8 File Offset: 0x001902A8
		public static bool isDebugBuild
		{
			get
			{
				return global::UnityEngine.Debug.isDebugBuild;
			}
		}

		// Token: 0x06002228 RID: 8744 RVA: 0x001920AF File Offset: 0x001902AF
		[Conditional("DISP_DEBUG_LOG_SUCEFBROWSER")]
		public static void Log(object message)
		{
		}

		// Token: 0x06002229 RID: 8745 RVA: 0x001920B1 File Offset: 0x001902B1
		[Conditional("DISP_DEBUG_LOG_SUCEFBROWSER")]
		public static void LogFormat(string format, params object[] args)
		{
		}

		// Token: 0x0600222A RID: 8746 RVA: 0x001920B3 File Offset: 0x001902B3
		[Conditional("DISP_DEBUG_LOG_SUCEFBROWSER")]
		public static void LogError(object message)
		{
		}

		// Token: 0x0600222B RID: 8747 RVA: 0x001920B5 File Offset: 0x001902B5
		[Conditional("DISP_DEBUG_LOG_SUCEFBROWSER")]
		public static void LogErrorFormat(string format, params object[] args)
		{
		}

		// Token: 0x0600222C RID: 8748 RVA: 0x001920B7 File Offset: 0x001902B7
		[Conditional("DISP_DEBUG_LOG_SUCEFBROWSER")]
		public static void Assert(bool condition, object message)
		{
		}

		// Token: 0x0600222D RID: 8749 RVA: 0x001920B9 File Offset: 0x001902B9
		[Conditional("DISP_DEBUG_LOG_SUCEFBROWSER")]
		public static void AssertFormat(bool condition, string format, params object[] args)
		{
		}

		// Token: 0x0600222E RID: 8750 RVA: 0x001920BB File Offset: 0x001902BB
		[Conditional("DISP_DEBUG_LOG_SUCEFBROWSER")]
		public static void LogException(Exception exception)
		{
		}
	}
}
