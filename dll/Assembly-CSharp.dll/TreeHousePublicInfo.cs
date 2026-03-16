using System;
using System.Collections.Generic;
using SGNFW.HttpRequest.Protocol;

public class TreeHousePublicInfo
{
	public TreeHousePublicInfo()
	{
	}

	public TreeHousePublicInfo(MasterRoomPublicInfo serverDataPublicInfo, List<MasterRoomStampPoint> serverDataStampNum)
	{
		this.houseName = serverDataPublicInfo.room_name;
		this.houseComment = serverDataPublicInfo.comment;
		this.publicType = (TreeHousePublicInfo.PublicType)serverDataPublicInfo.public_type;
		this.receiveStampList = serverDataStampNum.ConvertAll<TreeHousePublicInfo.ReceiveStamp>((MasterRoomStampPoint item) => new TreeHousePublicInfo.ReceiveStamp(item));
	}

	public string houseName;

	public string houseComment;

	public TreeHousePublicInfo.PublicType publicType;

	public List<TreeHousePublicInfo.ReceiveStamp> receiveStampList;

	public enum PublicType
	{
		PRIVATE,
		FOLLOW_ONLY,
		PUBLIC
	}

	public class ReceiveStamp
	{
		public ReceiveStamp()
		{
		}

		public ReceiveStamp(MasterRoomStampPoint serverData)
		{
			this.stampId = serverData.stamp_id;
			this.totalPoint = serverData.total_point;
			this.monthlyPoint = serverData.weekly_point;
			this.dailyPoint = serverData.daily_point;
		}

		public int stampId;

		public long totalPoint;

		public long monthlyPoint;

		public long dailyPoint;
	}
}
