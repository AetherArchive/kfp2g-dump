using System;
using System.Collections.Generic;
using SGNFW.Common;
using UnityEngine;

// Token: 0x02000108 RID: 264
public class TimeManager : Singleton<TimeManager>
{
	// Token: 0x17000323 RID: 803
	// (get) Token: 0x06000CB9 RID: 3257 RVA: 0x0004DEDC File Offset: 0x0004C0DC
	// (set) Token: 0x06000CBA RID: 3258 RVA: 0x0004DEE3 File Offset: 0x0004C0E3
	public static bool EnableTimeCmd { get; set; } = false;

	// Token: 0x06000CBB RID: 3259 RVA: 0x0004DEEB File Offset: 0x0004C0EB
	protected override void OnSingletonAwake()
	{
		Application.targetFrameRate = 30;
		LocalPushUtil.RestoreUnregistReserveMap();
		TimeManager.pause = false;
		TimeManager.timeScale = 1f;
	}

	// Token: 0x17000324 RID: 804
	// (get) Token: 0x06000CBC RID: 3260 RVA: 0x0004DF09 File Offset: 0x0004C109
	public static float DeltaTime
	{
		get
		{
			return Time.deltaTime;
		}
	}

	// Token: 0x17000325 RID: 805
	// (get) Token: 0x06000CBD RID: 3261 RVA: 0x0004DF10 File Offset: 0x0004C110
	// (set) Token: 0x06000CBE RID: 3262 RVA: 0x0004DF17 File Offset: 0x0004C117
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

	// Token: 0x17000326 RID: 806
	// (get) Token: 0x06000CBF RID: 3263 RVA: 0x0004DF30 File Offset: 0x0004C130
	// (set) Token: 0x06000CC0 RID: 3264 RVA: 0x0004DF37 File Offset: 0x0004C137
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

	// Token: 0x06000CC1 RID: 3265 RVA: 0x0004DF5B File Offset: 0x0004C15B
	public static float NowFPS()
	{
		return 1f / TimeManager.DeltaTime;
	}

	// Token: 0x17000327 RID: 807
	// (get) Token: 0x06000CC2 RID: 3266 RVA: 0x0004DF68 File Offset: 0x0004C168
	public static DateTime Now
	{
		get
		{
			return new DateTime(TimeManager.serverStartTime + (long)(Time.realtimeSinceStartup - TimeManager.startRealtimeSince) * 10000000L);
		}
	}

	// Token: 0x17000328 RID: 808
	// (get) Token: 0x06000CC3 RID: 3267 RVA: 0x0004DF88 File Offset: 0x0004C188
	public static DateTime SystemNow
	{
		get
		{
			return DateTime.Now;
		}
	}

	// Token: 0x06000CC4 RID: 3268 RVA: 0x0004DF8F File Offset: 0x0004C18F
	public static void SetServerStartTime(long startTime)
	{
		TimeManager.serverStartTime = PrjUtil.ConvertTimeToTicks(startTime);
		TimeManager.startRealtimeSince = Time.realtimeSinceStartup;
	}

	// Token: 0x06000CC5 RID: 3269 RVA: 0x0004DFA6 File Offset: 0x0004C1A6
	private void OnApplicationPause(bool pauseStatus)
	{
		if (!pauseStatus && TimeManager.EnableTimeCmd && Singleton<DataManager>.Instance != null && DataManager.DmServerMst != null)
		{
			DataManager.DmServerMst.RequestDownloadServerTime(null);
		}
	}

	// Token: 0x06000CC6 RID: 3270 RVA: 0x0004DFD1 File Offset: 0x0004C1D1
	public static long Second2Tick(long second)
	{
		return second * 10000000L;
	}

	// Token: 0x06000CC7 RID: 3271 RVA: 0x0004DFDC File Offset: 0x0004C1DC
	public static DateTime GetTerminalTimeByDay(DateTime dt)
	{
		DateTime dateTime = new DateTime(dt.Year, dt.Month, dt.Day);
		dateTime = dateTime.AddDays(1.0);
		dateTime = dateTime.AddSeconds(-1.0);
		return dateTime;
	}

	// Token: 0x06000CC8 RID: 3272 RVA: 0x0004E028 File Offset: 0x0004C228
	public static DateTime GetTerminalTimeByDay(DateTime dt, int addHours)
	{
		DateTime dateTime = new DateTime(dt.Year, dt.Month, dt.Day);
		dateTime = dateTime.AddDays(1.0);
		dateTime = dateTime.AddHours((double)addHours);
		dateTime = dateTime.AddSeconds(-1.0);
		return dateTime;
	}

	// Token: 0x06000CC9 RID: 3273 RVA: 0x0004E080 File Offset: 0x0004C280
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

	// Token: 0x06000CCA RID: 3274 RVA: 0x0004E0E4 File Offset: 0x0004C2E4
	public static DateTime GetTerminalTimeByMonth(DateTime dt)
	{
		DateTime dateTime = new DateTime(dt.Year, dt.Month, 1);
		dateTime = dateTime.AddMonths(1);
		dateTime = dateTime.AddSeconds(-1.0);
		return dateTime;
	}

	// Token: 0x06000CCB RID: 3275 RVA: 0x0004E124 File Offset: 0x0004C324
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

	// Token: 0x06000CCC RID: 3276 RVA: 0x0004E1FC File Offset: 0x0004C3FC
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

	// Token: 0x06000CCD RID: 3277 RVA: 0x0004E390 File Offset: 0x0004C590
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

	// Token: 0x17000329 RID: 809
	// (get) Token: 0x06000CCE RID: 3278 RVA: 0x0004E419 File Offset: 0x0004C619
	// (set) Token: 0x06000CCF RID: 3279 RVA: 0x0004E420 File Offset: 0x0004C620
	private static float AverageTime { get; set; }

	// Token: 0x06000CD0 RID: 3280 RVA: 0x0004E428 File Offset: 0x0004C628
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

	// Token: 0x06000CD1 RID: 3281 RVA: 0x0004E4DC File Offset: 0x0004C6DC
	public static float AveFPS()
	{
		return 1f / TimeManager.AverageTime;
	}

	// Token: 0x06000CD2 RID: 3282 RVA: 0x0004E4EC File Offset: 0x0004C6EC
	public static DayOfWeek ConvertDayOfWeekByServer2Client(int serverDayOfWeek)
	{
		int num = TimeManager.DayOfWeekConvertTable.FindIndex((KeyValuePair<DayOfWeek, int> item) => item.Value == serverDayOfWeek);
		if (num >= 0)
		{
			return TimeManager.DayOfWeekConvertTable[num].Key;
		}
		return DayOfWeek.Monday;
	}

	// Token: 0x06000CD3 RID: 3283 RVA: 0x0004E538 File Offset: 0x0004C738
	public static int ConvertDayOfWeekByClient2Server(DayOfWeek dow)
	{
		int num = TimeManager.DayOfWeekConvertTable.FindIndex((KeyValuePair<DayOfWeek, int> item) => item.Key == dow);
		if (num >= 0)
		{
			return TimeManager.DayOfWeekConvertTable[num].Value;
		}
		return -1;
	}

	// Token: 0x04000A26 RID: 2598
	public const int DEF_FRAME_RATE = 30;

	// Token: 0x04000A27 RID: 2599
	private static float timeScale;

	// Token: 0x04000A28 RID: 2600
	private static bool pause;

	// Token: 0x04000A29 RID: 2601
	private static long serverStartTime;

	// Token: 0x04000A2A RID: 2602
	private static float startRealtimeSince;

	// Token: 0x04000A2C RID: 2604
	private List<float> fps = new List<float>();

	// Token: 0x04000A2E RID: 2606
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

	// Token: 0x02000842 RID: 2114
	public enum Format
	{
		// Token: 0x0400371E RID: 14110
		yyyyMMdd,
		// Token: 0x0400371F RID: 14111
		yyyyMMdd_hhmm,
		// Token: 0x04003720 RID: 14112
		yyyyMMdd_hhmmss
	}
}
