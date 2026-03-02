using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x0200002F RID: 47
public static class Debug
{
	// Token: 0x060000A6 RID: 166 RVA: 0x000062CA File Offset: 0x000044CA
	[Conditional("DUMMY")]
	public static void Log(object message)
	{
	}

	// Token: 0x060000A7 RID: 167 RVA: 0x000062CC File Offset: 0x000044CC
	[Conditional("DUMMY")]
	public static void Log(object message, Object context)
	{
	}

	// Token: 0x060000A8 RID: 168 RVA: 0x000062CE File Offset: 0x000044CE
	[Conditional("DUMMY")]
	public static void LogError(object message)
	{
	}

	// Token: 0x060000A9 RID: 169 RVA: 0x000062D0 File Offset: 0x000044D0
	[Conditional("DUMMY")]
	public static void LogError(object message, Object context)
	{
	}

	// Token: 0x060000AA RID: 170 RVA: 0x000062D2 File Offset: 0x000044D2
	[Conditional("DUMMY")]
	public static void LogWarning(object message)
	{
	}

	// Token: 0x060000AB RID: 171 RVA: 0x000062D4 File Offset: 0x000044D4
	[Conditional("DUMMY")]
	public static void LogWarning(object message, Object context)
	{
	}

	// Token: 0x060000AC RID: 172 RVA: 0x000062D6 File Offset: 0x000044D6
	[Conditional("DUMMY")]
	public static void LogErrorFormat(string fromats, params object[] args)
	{
	}

	// Token: 0x060000AD RID: 173 RVA: 0x000062D8 File Offset: 0x000044D8
	[Conditional("DUMMY")]
	public static void LogErrorFormat(Object context, string fromats, params object[] args)
	{
	}

	// Token: 0x060000AE RID: 174 RVA: 0x000062DA File Offset: 0x000044DA
	[Conditional("DUMMY")]
	public static void Assert(bool condition)
	{
	}

	// Token: 0x060000AF RID: 175 RVA: 0x000062DC File Offset: 0x000044DC
	[Conditional("DUMMY")]
	public static void Assert(bool condition, object message)
	{
	}

	// Token: 0x060000B0 RID: 176 RVA: 0x000062DE File Offset: 0x000044DE
	[Conditional("DUMMY")]
	public static void Assert(bool condition, string message)
	{
	}

	// Token: 0x060000B1 RID: 177 RVA: 0x000062E0 File Offset: 0x000044E0
	[Conditional("DUMMY")]
	public static void Assert(bool condition, Object context)
	{
	}

	// Token: 0x060000B2 RID: 178 RVA: 0x000062E2 File Offset: 0x000044E2
	[Conditional("DUMMY")]
	public static void Assert(bool condition, object message, Object context)
	{
	}

	// Token: 0x060000B3 RID: 179 RVA: 0x000062E4 File Offset: 0x000044E4
	[Conditional("DUMMY")]
	public static void Assert(bool condition, string message, Object context)
	{
	}
}
