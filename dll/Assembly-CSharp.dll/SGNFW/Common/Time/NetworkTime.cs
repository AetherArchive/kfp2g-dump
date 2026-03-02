using System;
using System.Collections.Generic;

namespace SGNFW.Common.Time
{
	// Token: 0x02000262 RID: 610
	public class NetworkTime
	{
		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x06002604 RID: 9732 RVA: 0x001A0F12 File Offset: 0x0019F112
		public long clientTime
		{
			get
			{
				return this.leadResponseTimeInfo_.GetClientTime(Manager.realtimeSinceStartup);
			}
		}

		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x06002605 RID: 9733 RVA: 0x001A0F24 File Offset: 0x0019F124
		public long clientUnixTime
		{
			get
			{
				return this.clientTime / 1000L;
			}
		}

		// Token: 0x170005A2 RID: 1442
		// (get) Token: 0x06002606 RID: 9734 RVA: 0x001A0F33 File Offset: 0x0019F133
		public bool available
		{
			get
			{
				return this.infos_.Count > 0;
			}
		}

		// Token: 0x06002607 RID: 9735 RVA: 0x001A0F44 File Offset: 0x0019F144
		public void AddResponseTime(long serverTime, float responseInterval, float accesstimeSinceStartup)
		{
			NetworkTime.ResponseTimeInfo responseTimeInfo = new NetworkTime.ResponseTimeInfo
			{
				serverTime = serverTime,
				responseInterval = responseInterval,
				accesstimeSinceStartup = accesstimeSinceStartup
			};
			this.infos_.Add(responseTimeInfo);
			this.updateServerTime_();
		}

		// Token: 0x06002608 RID: 9736 RVA: 0x001A0F80 File Offset: 0x0019F180
		private void updateServerTime_()
		{
			float realtimeSinceStartup = Manager.realtimeSinceStartup;
			this.infos_.Sort((NetworkTime.ResponseTimeInfo a, NetworkTime.ResponseTimeInfo b) => (int)(a.GetClientTime(realtimeSinceStartup) - b.GetClientTime(realtimeSinceStartup)));
			if (this.infos_.Count >= 7)
			{
				this.infos_.RemoveAt(this.infos_.Count - 1);
				this.infos_.RemoveAt(0);
			}
			int num = this.infos_.Count / 2;
			this.leadResponseTimeInfo_ = this.infos_[num];
		}

		// Token: 0x04001BD8 RID: 7128
		private List<NetworkTime.ResponseTimeInfo> infos_ = new List<NetworkTime.ResponseTimeInfo>();

		// Token: 0x04001BD9 RID: 7129
		private NetworkTime.ResponseTimeInfo leadResponseTimeInfo_ = new NetworkTime.ResponseTimeInfo();

		// Token: 0x04001BDA RID: 7130
		private const int NUM_INFOS = 5;

		// Token: 0x0200109F RID: 4255
		private class ResponseTimeInfo
		{
			// Token: 0x06005369 RID: 21353 RVA: 0x00249F0F File Offset: 0x0024810F
			public long GetClientTime(float realtimeSinceStartup)
			{
				return this.serverTime + (long)((this.responseInterval / 2f + (realtimeSinceStartup - this.accesstimeSinceStartup)) * 1000f);
			}

			// Token: 0x04005C40 RID: 23616
			public long serverTime;

			// Token: 0x04005C41 RID: 23617
			public float responseInterval;

			// Token: 0x04005C42 RID: 23618
			public float accesstimeSinceStartup;
		}
	}
}
