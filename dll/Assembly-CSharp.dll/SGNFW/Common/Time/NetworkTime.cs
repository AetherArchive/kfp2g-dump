using System;
using System.Collections.Generic;

namespace SGNFW.Common.Time
{
	public class NetworkTime
	{
		public long clientTime
		{
			get
			{
				return this.leadResponseTimeInfo_.GetClientTime(Manager.realtimeSinceStartup);
			}
		}

		public long clientUnixTime
		{
			get
			{
				return this.clientTime / 1000L;
			}
		}

		public bool available
		{
			get
			{
				return this.infos_.Count > 0;
			}
		}

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

		private List<NetworkTime.ResponseTimeInfo> infos_ = new List<NetworkTime.ResponseTimeInfo>();

		private NetworkTime.ResponseTimeInfo leadResponseTimeInfo_ = new NetworkTime.ResponseTimeInfo();

		private const int NUM_INFOS = 5;

		private class ResponseTimeInfo
		{
			public long GetClientTime(float realtimeSinceStartup)
			{
				return this.serverTime + (long)((this.responseInterval / 2f + (realtimeSinceStartup - this.accesstimeSinceStartup)) * 1000f);
			}

			public long serverTime;

			public float responseInterval;

			public float accesstimeSinceStartup;
		}
	}
}
