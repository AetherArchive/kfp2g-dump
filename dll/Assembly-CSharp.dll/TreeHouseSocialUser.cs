using System;
using SGNFW.HttpRequest.Protocol;

public class TreeHouseSocialUser
{
	public TreeHouseSocialUser()
	{
	}

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

	public TreeHouseSocialTabType socialTabType;

	public int friendId;

	public string userName;

	public int userRank;

	public int favoriteCharaId;

	public int favoriteCharaFaceId;

	public int achievementId;

	public string houseName;

	public string houseComment;

	public bool isVisit;

	public bool isFinishSendStamp;

	public long getStampPoint;

	public DateTime updateTime;

	public int rankingNo;

	public TreeHouseSocialUser.StampActionType stampActionType;

	public int actionStampId;

	public DateTime actionTime;

	public bool isReceiveFollow;

	public bool isSendFollow;

	public bool isDispNew;

	public enum StampActionType
	{
		INVALID,
		SEND_BY_MINE,
		RECEIVE_BY_MINE
	}
}
