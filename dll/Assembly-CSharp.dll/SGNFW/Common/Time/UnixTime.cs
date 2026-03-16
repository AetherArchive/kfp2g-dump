using System;
using System.Globalization;

namespace SGNFW.Common.Time
{
	public class UnixTime
	{
		public static TimeLocale defaultLocale { get; set; } = TimeLocale.JST;

		public static long ConvertUnixTime(DateTime dateTime)
		{
			if (dateTime.Kind != DateTimeKind.Utc)
			{
				dateTime = dateTime.ToUniversalTime();
			}
			return (long)dateTime.Subtract(UnixTime.UnixEpoch).TotalMilliseconds;
		}

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

		public static string ConvertDateTimeString(long unixTime, string format = "g", TimeLocale locale = TimeLocale.Default)
		{
			if (unixTime <= 0L)
			{
				return "";
			}
			CultureInfo cultureInfo_ = UnixTime.getCultureInfo_();
			return UnixTime.ConvertDateTime(unixTime, locale).ToString(format, cultureInfo_);
		}

		private static CultureInfo getCultureInfo_()
		{
			return new CultureInfo("ja-JP", false);
		}

		public static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		public static readonly TimeSpan JST = new TimeSpan(9, 0, 0);
	}
}
