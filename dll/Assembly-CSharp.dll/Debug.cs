using System;
using System.Diagnostics;
using UnityEngine;

public static class Debug
{
	[Conditional("DUMMY")]
	public static void Log(object message)
	{
	}

	[Conditional("DUMMY")]
	public static void Log(object message, Object context)
	{
	}

	[Conditional("DUMMY")]
	public static void LogError(object message)
	{
	}

	[Conditional("DUMMY")]
	public static void LogError(object message, Object context)
	{
	}

	[Conditional("DUMMY")]
	public static void LogWarning(object message)
	{
	}

	[Conditional("DUMMY")]
	public static void LogWarning(object message, Object context)
	{
	}

	[Conditional("DUMMY")]
	public static void LogErrorFormat(string fromats, params object[] args)
	{
	}

	[Conditional("DUMMY")]
	public static void LogErrorFormat(Object context, string fromats, params object[] args)
	{
	}

	[Conditional("DUMMY")]
	public static void Assert(bool condition)
	{
	}

	[Conditional("DUMMY")]
	public static void Assert(bool condition, object message)
	{
	}

	[Conditional("DUMMY")]
	public static void Assert(bool condition, string message)
	{
	}

	[Conditional("DUMMY")]
	public static void Assert(bool condition, Object context)
	{
	}

	[Conditional("DUMMY")]
	public static void Assert(bool condition, object message, Object context)
	{
	}

	[Conditional("DUMMY")]
	public static void Assert(bool condition, string message, Object context)
	{
	}
}
