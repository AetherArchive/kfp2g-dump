using System;
using System.Collections.Generic;
using SGNFW.HttpRequest.Protocol;

// Token: 0x020000B2 RID: 178
public class TreeHouseSocialVisitUserData
{
	// Token: 0x1700017F RID: 383
	// (get) Token: 0x060007EC RID: 2028 RVA: 0x000356F6 File Offset: 0x000338F6
	// (set) Token: 0x060007ED RID: 2029 RVA: 0x000356FE File Offset: 0x000338FE
	public List<TreeHouseFurnitureMapping> FurnitureMappingList { get; private set; }

	// Token: 0x17000180 RID: 384
	// (get) Token: 0x060007EE RID: 2030 RVA: 0x00035707 File Offset: 0x00033907
	// (set) Token: 0x060007EF RID: 2031 RVA: 0x0003570F File Offset: 0x0003390F
	public List<TreeHousePutCharaData> PutCharaDataList { get; private set; }

	// Token: 0x060007F0 RID: 2032 RVA: 0x00035718 File Offset: 0x00033918
	public TreeHouseSocialVisitUserData()
	{
	}

	// Token: 0x060007F1 RID: 2033 RVA: 0x00035720 File Offset: 0x00033920
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

	// Token: 0x040006C4 RID: 1732
	public int friendId;

	// Token: 0x040006C5 RID: 1733
	public bool isFinishFollow;

	// Token: 0x040006C6 RID: 1734
	public string roomName;

	// Token: 0x040006C7 RID: 1735
	public string userName;

	// Token: 0x040006C8 RID: 1736
	public string roomComment;

	// Token: 0x040006C9 RID: 1737
	public int achievementId;

	// Token: 0x040006CA RID: 1738
	public bool isFinishSendStamp;
}
