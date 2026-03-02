using System;
using SGNFW.HttpRequest.Protocol;

// Token: 0x020000B4 RID: 180
public class TreeHouseSocialUser
{
	// Token: 0x060007F2 RID: 2034 RVA: 0x000357EF File Offset: 0x000339EF
	public TreeHouseSocialUser()
	{
	}

	// Token: 0x060007F3 RID: 2035 RVA: 0x000357F8 File Offset: 0x000339F8
	public TreeHouseSocialUser(MasterRoomFollow serverData)
	{
		this.socialTabType = TreeHouseSocialTabType.FOLLOW;
		this.friendId = serverData.friend_id;
		this.userName = serverData.name;
		this.userRank = serverData.user_rank;
		this.favoriteCharaId = serverData.favorite_chara_id;
		this.favoriteCharaFaceId = serverData.favorite_chara_face_id;
		this.achievementId = serverData.achievement_id;
		this.updateTime = new DateTime(PrjUtil.ConvertTimeToTicks(serverData.update_time));
		this.houseName = serverData.public_info.room_name;
		this.houseComment = serverData.public_info.comment;
		this.isVisit = serverData.visit_flg != 0;
		this.isFinishSendStamp = serverData.send_stamp_time != 0L;
		this.isReceiveFollow = serverData.follow_status == 3 || serverData.follow_status == 4;
		this.isSendFollow = serverData.follow_status == 2 || serverData.follow_status == 4;
		this.isDispNew = serverData.update_time > serverData.last_visit_time;
	}

	// Token: 0x060007F4 RID: 2036 RVA: 0x000358FC File Offset: 0x00033AFC
	public TreeHouseSocialUser(MasterRoomPassing serverData)
	{
		this.socialTabType = TreeHouseSocialTabType.PASSING;
		this.friendId = serverData.friend_id;
		this.userName = serverData.name;
		this.userRank = serverData.user_rank;
		this.favoriteCharaId = serverData.favorite_chara_id;
		this.favoriteCharaFaceId = serverData.favorite_chara_face_id;
		this.achievementId = serverData.achievement_id;
		this.updateTime = new DateTime(PrjUtil.ConvertTimeToTicks(serverData.update_time));
		this.houseName = serverData.public_info.room_name;
		this.houseComment = serverData.public_info.comment;
		this.isVisit = serverData.visit_flg != 0;
		this.isFinishSendStamp = serverData.send_stamp_time != 0L;
		this.getStampPoint = serverData.stamp_point;
		this.isReceiveFollow = serverData.follow_status == 3 || serverData.follow_status == 4;
		this.isSendFollow = serverData.follow_status == 2 || serverData.follow_status == 4;
	}

	// Token: 0x060007F5 RID: 2037 RVA: 0x000359F8 File Offset: 0x00033BF8
	public TreeHouseSocialUser(MasterRoomRanking serverData)
	{
		this.socialTabType = TreeHouseSocialTabType.RANKING;
		this.friendId = serverData.friend_id;
		this.userName = serverData.name;
		this.userRank = serverData.user_rank;
		this.favoriteCharaId = serverData.favorite_chara_id;
		this.favoriteCharaFaceId = serverData.favorite_chara_face_id;
		this.achievementId = serverData.achievement_id;
		this.houseName = serverData.public_info.room_name;
		this.houseComment = serverData.public_info.comment;
		this.isVisit = serverData.visit_flg != 0;
		this.isFinishSendStamp = serverData.send_stamp_time != 0L;
		this.getStampPoint = serverData.stamp_point;
		this.rankingNo = serverData.rank;
		this.isReceiveFollow = serverData.follow_status == 3 || serverData.follow_status == 4;
		this.isSendFollow = serverData.follow_status == 2 || serverData.follow_status == 4;
		this.isDispNew = serverData.update_time > serverData.last_visit_time;
	}

	// Token: 0x060007F6 RID: 2038 RVA: 0x00035B00 File Offset: 0x00033D00
	public TreeHouseSocialUser(MasterRoomStampLog serverData)
	{
		this.socialTabType = TreeHouseSocialTabType.STAMP_HISTORY;
		this.friendId = serverData.friend_id;
		this.userName = serverData.name;
		this.userRank = serverData.user_rank;
		this.favoriteCharaId = serverData.favorite_chara_id;
		this.favoriteCharaFaceId = serverData.favorite_chara_face_id;
		this.achievementId = serverData.achievement_id;
		this.houseName = serverData.public_info.room_name;
		this.houseComment = serverData.public_info.comment;
		this.isVisit = serverData.visit_flg != 0;
		this.isFinishSendStamp = serverData.send_stamp_time != 0L;
		this.stampActionType = ((serverData.send_flg == 0) ? TreeHouseSocialUser.StampActionType.RECEIVE_BY_MINE : TreeHouseSocialUser.StampActionType.SEND_BY_MINE);
		this.actionStampId = serverData.stamp_id;
		this.actionTime = new DateTime(PrjUtil.ConvertTimeToTicks(serverData.time));
		this.isReceiveFollow = serverData.follow_status == 3 || serverData.follow_status == 4;
		this.isSendFollow = serverData.follow_status == 2 || serverData.follow_status == 4;
	}

	// Token: 0x040006D5 RID: 1749
	public TreeHouseSocialTabType socialTabType;

	// Token: 0x040006D6 RID: 1750
	public int friendId;

	// Token: 0x040006D7 RID: 1751
	public string userName;

	// Token: 0x040006D8 RID: 1752
	public int userRank;

	// Token: 0x040006D9 RID: 1753
	public int favoriteCharaId;

	// Token: 0x040006DA RID: 1754
	public int favoriteCharaFaceId;

	// Token: 0x040006DB RID: 1755
	public int achievementId;

	// Token: 0x040006DC RID: 1756
	public string houseName;

	// Token: 0x040006DD RID: 1757
	public string houseComment;

	// Token: 0x040006DE RID: 1758
	public bool isVisit;

	// Token: 0x040006DF RID: 1759
	public bool isFinishSendStamp;

	// Token: 0x040006E0 RID: 1760
	public long getStampPoint;

	// Token: 0x040006E1 RID: 1761
	public DateTime updateTime;

	// Token: 0x040006E2 RID: 1762
	public int rankingNo;

	// Token: 0x040006E3 RID: 1763
	public TreeHouseSocialUser.StampActionType stampActionType;

	// Token: 0x040006E4 RID: 1764
	public int actionStampId;

	// Token: 0x040006E5 RID: 1765
	public DateTime actionTime;

	// Token: 0x040006E6 RID: 1766
	public bool isReceiveFollow;

	// Token: 0x040006E7 RID: 1767
	public bool isSendFollow;

	// Token: 0x040006E8 RID: 1768
	public bool isDispNew;

	// Token: 0x020007A9 RID: 1961
	public enum StampActionType
	{
		// Token: 0x040033FD RID: 13309
		INVALID,
		// Token: 0x040033FE RID: 13310
		SEND_BY_MINE,
		// Token: 0x040033FF RID: 13311
		RECEIVE_BY_MINE
	}
}
