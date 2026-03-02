using System;
using System.Collections.Generic;
using SGNFW.HttpRequest.Protocol;

// Token: 0x020000B7 RID: 183
public class TreeHouseReceiveStampLog
{
	// Token: 0x060007FB RID: 2043 RVA: 0x00035CE9 File Offset: 0x00033EE9
	public TreeHouseReceiveStampLog()
	{
	}

	// Token: 0x060007FC RID: 2044 RVA: 0x00035CFC File Offset: 0x00033EFC
	public TreeHouseReceiveStampLog(MasterRoomGetDataResponse resp)
	{
		this.receiveStampNum = resp.receive_stamp_num;
		if (resp.receive_stamp_log_list != null)
		{
			this.logList = resp.receive_stamp_log_list.ConvertAll<TreeHouseReceiveStampLog.Log>((MasterRoomReceiveStamplog item) => new TreeHouseReceiveStampLog.Log(item));
		}
	}

	// Token: 0x040006F1 RID: 1777
	public long receiveStampNum;

	// Token: 0x040006F2 RID: 1778
	public List<TreeHouseReceiveStampLog.Log> logList = new List<TreeHouseReceiveStampLog.Log>();

	// Token: 0x020007AD RID: 1965
	public class Log
	{
		// Token: 0x06003704 RID: 14084 RVA: 0x001C73D0 File Offset: 0x001C55D0
		public Log(MasterRoomReceiveStamplog serverData)
		{
			this.stampId = serverData.stamp_id;
			this.userName = serverData.user_name;
			this.userRank = serverData.user_rank;
			this.receiveTime = new DateTime(PrjUtil.ConvertTimeToTicks(serverData.receive_time));
			this.getStampPoint = serverData.stamp_point;
			this.isReceiveFollow = serverData.follow_status == 3 || serverData.follow_status == 4;
			this.isSendFollow = serverData.follow_status == 2 || serverData.follow_status == 4;
		}

		// Token: 0x0400340A RID: 13322
		public int stampId;

		// Token: 0x0400340B RID: 13323
		public string userName;

		// Token: 0x0400340C RID: 13324
		public int userRank;

		// Token: 0x0400340D RID: 13325
		public DateTime receiveTime;

		// Token: 0x0400340E RID: 13326
		public long getStampPoint;

		// Token: 0x0400340F RID: 13327
		public bool isReceiveFollow;

		// Token: 0x04003410 RID: 13328
		public bool isSendFollow;
	}
}
