using System;
using System.Collections.Generic;
using SGNFW.Common;
using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
	public static bool EnableTimeCmd { get; set; } = false;

	protected override void OnSingletonAwake()
	{
		Application.targetFrameRate = 30;
		LocalPushUtil.RestoreUnregistReserveMap();
		TimeManager.pause = false;
		TimeManager.timeScale = 1f;
	}

	public static float DeltaTime
	{
		get
		{
			return Time.deltaTime;
		}
	}

	public static float TimeScale
	{
		get
		{
			return TimeManager.timeScale;
		}
		set
		{
			TimeManager.timeScale = value;
			if (!TimeManager.pause)
			{
				Time.timeScale = TimeManager.timeScale;
			}
		}
	}

	public static bool Pause
	{
		get
		{
			return TimeManager.pause;
		}
		set
		{
			TimeManager.pause = value;
			if (TimeManager.pause)
			{
				Time.timeScale = 0f;
				return;
			}
			Time.timeScale = TimeManager.timeScale;
		}
	}

	public static float NowFPS()
	{
		return 1f / TimeManager.DeltaTime;
	}

	public static DateTime Now
	{
		get
		{
			return new DateTime(TimeManager.serverStartTime + (long)(Time.realtimeSinceStartup - TimeManager.startRealtimeSince) * 10000000L);
		}
	}

	public static DateTime SystemNow
	{
		get
		{
			return DateTime.Now;
		}
	}

	public static void SetServerStartTime(long startTime)
	{
		TimeManager.serverStartTime = PrjUtil.ConvertTimeToTicks(startTime);
		TimeManager.startRealtimeSince = Time.realtimeSinceStartup;
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (!pauseStatus && TimeManager.EnableTimeCmd && Singleton<DataManager>.Instance != null && DataManager.DmServerMst != null)
		{
			DataManager.DmServerMst.RequestDownloadServerTime(null);
		}
	}

	public static long Second2Tick(long second)
	{
		return second * 10000000L;
	}

	public static DateTime GetTerminalTimeByDay(DateTime dt)
	{
		DateTime dateTime = new DateTime(dt.Year, dt.Month, dt.Day);
		dateTime = dateTime.AddDays(1.0);
		dateTime = dateTime.AddSeconds(-1.0);
		return dateTime;
	}

	public static DateTime GetTerminalTimeByDay(DateTime dt, int addHours)
	{
		DateTime dateTime = new DateTime(dt.Year, dt.Month, dt.Day);
		dateTime = dateTime.AddDays(1.0);
		dateTime = dateTime.AddHours((double)addHours);
		dateTime = dateTime.AddSeconds(-1.0);
		return dateTime;
	}

	public static DateTime GetTerminalTimeByWeek(DateTime dt)
	{
		DateTime dateTime = new DateTime(dt.Year, dt.Month, dt.Day);
		for (int i = 0; i < 7; i++)
		{
			dateTime = dateTime.AddDays(1.0);
			if (dateTime.DayOfWeek == DayOfWeek.Monday)
			{
				break;
			}
		}
		dateTime = dateTime.AddSeconds(-1.0);
		return dateTime;
	}

	public static DateTime GetTerminalTimeByMonth(DateTime dt)
	{
		DateTime dateTime = new DateTime(dt.Year, dt.Month, 1);
		dateTime = dateTime.AddMonths(1);
		dateTime = dateTime.AddSeconds(-1.0);
		return dateTime;
	}

	public static string MakeTimeSpanText(DateTime from, DateTime to)
	{
		TimeSpan timeSpan = to - from;
		string text;
		if (timeSpan.Days > 0)
		{
			text = timeSpan.Days.ToString() + PrjUtil.MakeMessage("日");
		}
		else if (timeSpan.Hours > 0)
		{
			text = timeSpan.Hours.ToString() + PrjUtil.MakeMessage("時間");
		}
		else if (timeSpan.Minutes > 0)
		{
			text = timeSpan.Minutes.ToString() + PrjUtil.MakeMessage("分");
		}
		else if (timeSpan.Seconds > 0)
		{
			text = timeSpan.Seconds.ToString() + PrjUtil.MakeMessage("秒");
		}
		else
		{
			text = PrjUtil.MakeMessage("0秒");
		}
		return text;
	}

	public static string MakeTimeResidueText(DateTime from, DateTime to, bool dispSecond = false, bool dispDayDetail = true)
	{
		TimeSpan timeSpan = to - from;
		string text;
		if (timeSpan.Days > 0)
		{
			if (dispDayDetail)
			{
				text = string.Concat(new string[]
				{
					PrjUtil.MakeMessage("あと"),
					timeSpan.Days.ToString(),
					PrjUtil.MakeMessage("日と"),
					timeSpan.Hours.ToString(),
					PrjUtil.MakeMessage("時間")
				});
			}
			else
			{
				text = PrjUtil.MakeMessage("あと") + timeSpan.Days.ToString() + PrjUtil.MakeMessage("日");
			}
		}
		else if (timeSpan.Hours > 0)
		{
			text = PrjUtil.MakeMessage("あと") + timeSpan.Hours.ToString() + PrjUtil.MakeMessage("時間");
		}
		else if (timeSpan.Minutes > 0)
		{
			text = PrjUtil.MakeMessage("あと") + timeSpan.Minutes.ToString() + PrjUtil.MakeMessage("分");
		}
		else if (timeSpan.Seconds > 0)
		{
			if (dispSecond)
			{
				text = PrjUtil.MakeMessage("あと") + timeSpan.Seconds.ToString() + PrjUtil.MakeMessage("秒");
			}
			else
			{
				text = PrjUtil.MakeMessage("あと") + timeSpan.Minutes.ToString() + PrjUtil.MakeMessage("分");
			}
		}
		else
		{
			text = PrjUtil.MakeMessage("終了しました");
		}
		return text;
	}

	public static string FormattedTime(DateTime dateTime, TimeManager.Format format)
	{
		string text = string.Empty;
		string text2 = "\u00a0";
		switch (format)
		{
		case TimeManager.Format.yyyyMMdd:
			text = dateTime.ToString("yyyy/MM/dd");
			break;
		case TimeManager.Format.yyyyMMdd_hhmm:
			text = dateTime.ToString("yyyy/MM/dd" + text2 + "HH:mm");
			break;
		case TimeManager.Format.yyyyMMdd_hhmmss:
			text = dateTime.ToString("yyyy/MM/dd" + text2 + "HH:mm:ss");
			break;
		default:
			text = dateTime.ToString("yyyy/MM/dd" + text2 + "HH:mm:ss");
			break;
		}
		return text;
	}

	private static float AverageTime { get; set; }

	private void Update()
	{
		LocalPushUtil.ResolveUnregistReserve();
		if (TimeManager.pause)
		{
			return;
		}
		this.fps.Add(TimeManager.DeltaTime / Time.timeScale);
		float num = 0f;
		foreach (float num2 in this.fps)
		{
			num += num2;
		}
		TimeManager.AverageTime = num / (float)this.fps.Count;
		while (num > 1f)
		{
			num -= this.fps[0];
			this.fps.RemoveAt(0);
		}
	}

	public static float AveFPS()
	{
		return 1f / TimeManager.AverageTime;
	}

	public static DayOfWeek ConvertDayOfWeekByServer2Client(int serverDayOfWeek)
	{
		int num = TimeManager.DayOfWeekConvertTable.FindIndex((KeyValuePair<DayOfWeek, int> item) => item.Value == serverDayOfWeek);
		if (num >= 0)
		{
			return TimeManager.DayOfWeekConvertTable[num].Key;
		}
		return DayOfWeek.Monday;
	}

	public static int ConvertDayOfWeekByClient2Server(DayOfWeek dow)
	{
		int num = TimeManager.DayOfWeekConvertTable.FindIndex((KeyValuePair<DayOfWeek, int> item) => item.Key == dow);
		if (num >= 0)
		{
			return TimeManager.DayOfWeekConvertTable[num].Value;
		}
		return -1;
	}

	public const int DEF_FRAME_RATE = 30;

	private static float timeScale;

	private static bool pause;

	private static long serverStartTime;

	private static float startRealtimeSince;

	private List<float> fps = new List<float>();

	private static readonly List<KeyValuePair<DayOfWeek, int>> DayOfWeekConvertTable = new List<KeyValuePair<DayOfWeek, int>>
	{
		new KeyValuePair<DayOfWeek, int>(DayOfWeek.Monday, 1),
		new KeyValuePair<DayOfWeek, int>(DayOfWeek.Tuesday, 2),
		new KeyValuePair<DayOfWeek, int>(DayOfWeek.Wednesday, 3),
		new KeyValuePair<DayOfWeek, int>(DayOfWeek.Thursday, 4),
		new KeyValuePair<DayOfWeek, int>(DayOfWeek.Friday, 5),
		new KeyValuePair<DayOfWeek, int>(DayOfWeek.Saturday, 6),
		new KeyValuePair<DayOfWeek, int>(DayOfWeek.Sunday, 7)
	};

	public enum Format
	{
		yyyyMMdd,
		yyyyMMdd_hhmm,
		yyyyMMdd_hhmmss
	}
}
