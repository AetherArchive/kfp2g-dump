using System;
using System.Collections.Generic;
using SGNFW.HttpRequest.Protocol;

public class TreeHouseSocialVisitUserData
{
	public List<TreeHouseFurnitureMapping> FurnitureMappingList { get; private set; }

	public List<TreeHousePutCharaData> PutCharaDataList { get; private set; }

	public TreeHouseSocialVisitUserData()
	{
	}

	public TreeHouseSocialVisitUserData(int _friendId, MasterRoomVisitResponse serverData)
	{
		this.friendId = _friendId;
		if (serverData != null)
		{
			this.roomName = serverData.room_name;
			this.userName = serverData.user_name;
			this.roomComment = serverData.room_comment;
			this.achievementId = serverData.achievement_id;
			this.isFinishFollow = serverData.follow_flg != 0;
			this.isFinishSendStamp = serverData.send_stamp_time != 0L;
			this.FurnitureMappingList = serverData.furniture_list.ConvertAll<TreeHouseFurnitureMapping>((MasterRoomFurniture item) => new TreeHouseFurnitureMapping(item));
			this.PutCharaDataList = serverData.chara_list.ConvertAll<TreeHousePutCharaData>((MasterRoomChara item) => new TreeHousePutCharaData(item));
		}
	}

	public int friendId;

	public bool isFinishFollow;

	public string roomName;

	public string userName;

	public string roomComment;

	public int achievementId;

	public bool isFinishSendStamp;
}
