using System;
using System.Globalization;

namespace SGNFW.Common.Time
{
	// Token: 0x02000264 RID: 612
	public class UnixTime
	{
		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x0600260A RID: 9738 RVA: 0x001A1025 File Offset: 0x0019F225
		// (set) Token: 0x0600260B RID: 9739 RVA: 0x001A102C File Offset: 0x0019F22C
		public static TimeLocale defaultLocale { get; set; } = TimeLocale.JST;

		// Token: 0x0600260C RID: 9740 RVA: 0x001A1034 File Offset: 0x0019F234
		public static long ConvertUnixTime(DateTime dateTime)
		{
			if (dateTime.Kind != DateTimeKind.Utc)
			{
				dateTime = dateTime.ToUniversalTime();
			}
			return (long)dateTime.Subtract(UnixTime.UnixEpoch).TotalMilliseconds;
		}

		// Token: 0x0600260D RID: 9741 RVA: 0x001A106C File Offset: 0x0019F26C
		public static DateTime ConvertDateTime(long unixTime, TimeLocale locale = TimeLocale.Default)
		{
			if (locale == TimeLocale.Default)
			{
				locale = UnixTime.defaultLocale;
			}
			switch (locale)
			{
			default:
				return UnixTime.UnixEpoch.AddMilliseconds((double)unixTime + UnixTime.JST.TotalMilliseconds);
			case TimeLocale.GMT:
				return UnixTime.UnixEpoch.AddMilliseconds((double)unixTime);
			case TimeLocale.Local:
				return UnixTime.UnixEpoch.AddMilliseconds((double)unixTime).ToLocalTime();
			}
		}

		// Token: 0x0600260E RID: 9742 RVA: 0x001A10D0 File Offset: 0x0019F2D0
		public static string ConvertDateTimeString(long unixTime, string format = "g", TimeLocale locale = TimeLocale.Default)
		{
			if (unixTime <= 0L)
			{
				return "";
			}
			CultureInfo cultureInfo_ = UnixTime.getCultureInfo_();
			return UnixTime.ConvertDateTime(unixTime, locale).ToString(format, cultureInfo_);
		}

		// Token: 0x0600260F RID: 9743 RVA: 0x001A10FF File Offset: 0x0019F2FF
		private static CultureInfo getCultureInfo_()
		{
			return new CultureInfo("ja-JP", false);
		}

		// Token: 0x04001BE1 RID: 7137
		public static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		// Token: 0x04001BE2 RID: 7138
		public static readonly TimeSpan JST = new TimeSpan(9, 0, 0);
	}
}
