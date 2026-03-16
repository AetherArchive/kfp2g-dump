using System;
using System.Collections.Generic;
using SGNFW.HttpRequest.Protocol;

public class TreeHouseReceiveStampLog
{
	public TreeHouseReceiveStampLog()
	{
	}

	public TreeHouseReceiveStampLog(MasterRoomGetDataResponse resp)
	{
		this.receiveStampNum = resp.receive_stamp_num;
		if (resp.receive_stamp_log_list != null)
		{
			this.logList = resp.receive_stamp_log_list.ConvertAll<TreeHouseReceiveStampLog.Log>((MasterRoomReceiveStamplog item) => new TreeHouseReceiveStampLog.Log(item));
		}
	}

	public long receiveStampNum;

	public List<TreeHouseReceiveStampLog.Log> logList = new List<TreeHouseReceiveStampLog.Log>();

	public class Log
	{
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

		public int stampId;

		public string userName;

		public int userRank;

		public DateTime receiveTime;

		public long getStampPoint;

		public bool isReceiveFollow;

		public bool isSendFollow;
	}
}
