using System;
using System.Diagnostics;

namespace SGNFW.Common.Time
{
	public class Manager : Singleton<Manager>
	{
		public static float realtimeSinceStartup
		{
			get
			{
				return Singleton<Manager>.Instance.realtimeSinceStartup_;
			}
		}

		public static long frameCountSinceStartup
		{
			get
			{
				return Singleton<Manager>.Instance.frameCountSinceStartup_;
			}
		}

		public static long clientTime
		{
			get
			{
				return Singleton<Manager>.Instance.networkTime_.clientTime;
			}
		}

		public static long clientUnixTime
		{
			get
			{
				return Singleton<Manager>.Instance.networkTime_.clientUnixTime;
			}
		}

		public static DateTime GetDateTime()
		{
			return UnixTime.ConvertDateTime(Manager.clientTime, TimeLocale.Default);
		}

		public static void AddResponseTime(long serverTime, float responceTime)
		{
			Singleton<Manager>.Instance.networkTime_.AddResponseTime(serverTime, responceTime, Manager.realtimeSinceStartup);
		}

		public static void AddResponseTime(long serverTime, float responceTime, float accesstimeSinceStartup)
		{
			Singleton<Manager>.Instance.networkTime_.AddResponseTime(serverTime, responceTime, accesstimeSinceStartup);
		}

		protected override void OnSingletonAwake()
		{
			this.networkTime_ = new NetworkTime();
			this.stopwatch_ = new Stopwatch();
			this.stopwatch_.Start();
		}

		private void Update()
		{
			this.realtimeSinceStartup_ = (float)this.stopwatch_.Elapsed.TotalSeconds;
			this.frameCountSinceStartup_ += 1L;
		}

		private NetworkTime networkTime_;

		private Stopwatch stopwatch_;

		private float realtimeSinceStartup_;

		private long frameCountSinceStartup_;
	}
}
