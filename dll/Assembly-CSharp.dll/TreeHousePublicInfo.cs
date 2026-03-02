using System;
using System.Collections.Generic;
using SGNFW.HttpRequest.Protocol;

// Token: 0x020000B5 RID: 181
public class TreeHousePublicInfo
{
	// Token: 0x060007F7 RID: 2039 RVA: 0x00035C0D File Offset: 0x00033E0D
	public TreeHousePublicInfo()
	{
	}

	// Token: 0x060007F8 RID: 2040 RVA: 0x00035C18 File Offset: 0x00033E18
	public TreeHousePublicInfo(MasterRoomPublicInfo serverDataPublicInfo, List<MasterRoomStampPoint> serverDataStampNum)
	{
		this.houseName = serverDataPublicInfo.room_name;
		this.houseComment = serverDataPublicInfo.comment;
		this.publicType = (TreeHousePublicInfo.PublicType)serverDataPublicInfo.public_type;
		this.receiveStampList = serverDataStampNum.ConvertAll<TreeHousePublicInfo.ReceiveStamp>((MasterRoomStampPoint item) => new TreeHousePublicInfo.ReceiveStamp(item));
	}

	// Token: 0x040006E9 RID: 1769
	public string houseName;

	// Token: 0x040006EA RID: 1770
	public string houseComment;

	// Token: 0x040006EB RID: 1771
	public TreeHousePublicInfo.PublicType publicType;

	// Token: 0x040006EC RID: 1772
	public List<TreeHousePublicInfo.ReceiveStamp> receiveStampList;

	// Token: 0x020007AA RID: 1962
	public enum PublicType
	{
		// Token: 0x04003401 RID: 13313
		PRIVATE,
		// Token: 0x04003402 RID: 13314
		FOLLOW_ONLY,
		// Token: 0x04003403 RID: 13315
		PUBLIC
	}

	// Token: 0x020007AB RID: 1963
	public class ReceiveStamp
	{
		// Token: 0x060036FF RID: 14079 RVA: 0x001C7371 File Offset: 0x001C5571
		public ReceiveStamp()
		{
		}

		// Token: 0x06003700 RID: 14080 RVA: 0x001C7379 File Offset: 0x001C5579
		public ReceiveStamp(MasterRoomStampPoint serverData)
		{
			this.stampId = serverData.stamp_id;
			this.totalPoint = serverData.total_point;
			this.monthlyPoint = serverData.weekly_point;
			this.dailyPoint = serverData.daily_point;
		}

		// Token: 0x04003404 RID: 13316
		public int stampId;

		// Token: 0x04003405 RID: 13317
		public long totalPoint;

		// Token: 0x04003406 RID: 13318
		public long monthlyPoint;

		// Token: 0x04003407 RID: 13319
		public long dailyPoint;
	}
}
