using System;
using System.Diagnostics;

namespace SGNFW.Common.Time
{
	// Token: 0x02000261 RID: 609
	public class Manager : Singleton<Manager>
	{
		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x060025FA RID: 9722 RVA: 0x001A0E3C File Offset: 0x0019F03C
		public static float realtimeSinceStartup
		{
			get
			{
				return Singleton<Manager>.Instance.realtimeSinceStartup_;
			}
		}

		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x060025FB RID: 9723 RVA: 0x001A0E48 File Offset: 0x0019F048
		public static long frameCountSinceStartup
		{
			get
			{
				return Singleton<Manager>.Instance.frameCountSinceStartup_;
			}
		}

		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x060025FC RID: 9724 RVA: 0x001A0E54 File Offset: 0x0019F054
		public static long clientTime
		{
			get
			{
				return Singleton<Manager>.Instance.networkTime_.clientTime;
			}
		}

		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x060025FD RID: 9725 RVA: 0x001A0E65 File Offset: 0x0019F065
		public static long clientUnixTime
		{
			get
			{
				return Singleton<Manager>.Instance.networkTime_.clientUnixTime;
			}
		}

		// Token: 0x060025FE RID: 9726 RVA: 0x001A0E76 File Offset: 0x0019F076
		public static DateTime GetDateTime()
		{
			return UnixTime.ConvertDateTime(Manager.clientTime, TimeLocale.Default);
		}

		// Token: 0x060025FF RID: 9727 RVA: 0x001A0E83 File Offset: 0x0019F083
		public static void AddResponseTime(long serverTime, float responceTime)
		{
			Singleton<Manager>.Instance.networkTime_.AddResponseTime(serverTime, responceTime, Manager.realtimeSinceStartup);
		}

		// Token: 0x06002600 RID: 9728 RVA: 0x001A0E9B File Offset: 0x0019F09B
		public static void AddResponseTime(long serverTime, float responceTime, float accesstimeSinceStartup)
		{
			Singleton<Manager>.Instance.networkTime_.AddResponseTime(serverTime, responceTime, accesstimeSinceStartup);
		}

		// Token: 0x06002601 RID: 9729 RVA: 0x001A0EAF File Offset: 0x0019F0AF
		protected override void OnSingletonAwake()
		{
			this.networkTime_ = new NetworkTime();
			this.stopwatch_ = new Stopwatch();
			this.stopwatch_.Start();
		}

		// Token: 0x06002602 RID: 9730 RVA: 0x001A0ED4 File Offset: 0x0019F0D4
		private void Update()
		{
			this.realtimeSinceStartup_ = (float)this.stopwatch_.Elapsed.TotalSeconds;
			this.frameCountSinceStartup_ += 1L;
		}

		// Token: 0x04001BD4 RID: 7124
		private NetworkTime networkTime_;

		// Token: 0x04001BD5 RID: 7125
		private Stopwatch stopwatch_;

		// Token: 0x04001BD6 RID: 7126
		private float realtimeSinceStartup_;

		// Token: 0x04001BD7 RID: 7127
		private long frameCountSinceStartup_;
	}
}
