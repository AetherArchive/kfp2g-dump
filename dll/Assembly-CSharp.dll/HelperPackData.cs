using System;
using System.Collections.Generic;
using SGNFW.HttpRequest.Protocol;

// Token: 0x0200007E RID: 126
public class HelperPackData
{
	// Token: 0x170000E9 RID: 233
	// (get) Token: 0x060004B7 RID: 1207 RVA: 0x0002214B File Offset: 0x0002034B
	// (set) Token: 0x060004B8 RID: 1208 RVA: 0x00022153 File Offset: 0x00020353
	public List<HelperPackData.HelperCharaSet> HelperCharaSetList { get; set; }

	// Token: 0x170000EA RID: 234
	// (get) Token: 0x060004B9 RID: 1209 RVA: 0x0002215C File Offset: 0x0002035C
	// (set) Token: 0x060004BA RID: 1210 RVA: 0x00022164 File Offset: 0x00020364
	public CharaPackData FavoriteChara { get; private set; }

	// Token: 0x170000EB RID: 235
	// (get) Token: 0x060004BB RID: 1211 RVA: 0x0002216D File Offset: 0x0002036D
	public bool isDummy
	{
		get
		{
			return this.friendId <= 0;
		}
	}

	// Token: 0x060004BC RID: 1212 RVA: 0x0002217B File Offset: 0x0002037B
	public HelperPackData()
	{
	}

	// Token: 0x060004BD RID: 1213 RVA: 0x00022184 File Offset: 0x00020384
	public HelperPackData(Helper srvHelper)
	{
		this.helper = srvHelper;
		this.friendId = srvHelper.friend_id;
		this.userName = srvHelper.user_name;
		this.level = srvHelper.level;
		this.addPoint = srvHelper.add_point;
		this.lastLoginTime = new DateTime(PrjUtil.ConvertTimeToTicks(srvHelper.last_login_time));
		this.HelperCharaSetList = new List<HelperPackData.HelperCharaSet>();
		foreach (Chara chara in srvHelper.help_charas)
		{
			HelperPackData.HelperCharaSet helperCharaSet = new HelperPackData.HelperCharaSet();
			if (chara != null && chara.chara_id != 0)
			{
				CharaDynamicData charaDynamicData = new CharaDynamicData();
				charaDynamicData.OwnerType = CharaDynamicData.CharaOwnerType.Helper;
				charaDynamicData.UpdateByServer(chara);
				helperCharaSet.helpChara = new CharaPackData(charaDynamicData);
				if (this.isDummy)
				{
					int photoFrameTotalStep = helperCharaSet.helpChara.dynamicData.PhotoFrameTotalStep;
					int ppStepMax = DataManager.DmServerMst.StaticCharaPpDataMap[helperCharaSet.helpChara.staticData.baseData.photoFrameTableId].PpStepMax;
					if (photoFrameTotalStep <= 0)
					{
						helperCharaSet.helpChara.dynamicData.PhotoFrameTotalStep = 1;
					}
					else if (ppStepMax < photoFrameTotalStep)
					{
						helperCharaSet.helpChara.dynamicData.PhotoFrameTotalStep = ppStepMax;
					}
				}
			}
			else
			{
				helperCharaSet.helpChara = null;
			}
			helperCharaSet.helpPhotoList = new List<PhotoPackData>();
			foreach (Photo photo in chara.photo_list)
			{
				if (photo.item_id != 0)
				{
					PhotoDynamicData photoDynamicData = new PhotoDynamicData();
					photoDynamicData.OwnerType = PhotoDynamicData.PhotoOwnerType.Helper;
					photoDynamicData.UpdateByServer(photo);
					helperCharaSet.helpPhotoList.Add(new PhotoPackData(photoDynamicData));
				}
				else
				{
					helperCharaSet.helpPhotoList.Add(null);
				}
			}
			helperCharaSet.helpAccessory = ((chara.accessory == null) ? null : new DataManagerCharaAccessory.Accessory(chara.accessory));
			this.HelperCharaSetList.Add(helperCharaSet);
		}
		if (srvHelper.favorite_chara.chara_id != 0)
		{
			CharaDynamicData charaDynamicData2 = new CharaDynamicData();
			charaDynamicData2.OwnerType = CharaDynamicData.CharaOwnerType.Helper;
			charaDynamicData2.UpdateByServer(srvHelper.favorite_chara);
			this.FavoriteChara = new CharaPackData(charaDynamicData2);
		}
		this.isReceiveFollow = srvHelper.is_receive_follow != 0;
		this.isSendFollow = srvHelper.is_send_follw != 0;
		this.receiveFollowTime = new DateTime(PrjUtil.ConvertTimeToTicks(srvHelper.is_receive_follow_datetime));
		this.sendFollowTime = new DateTime(PrjUtil.ConvertTimeToTicks(srvHelper.is_send_follw_datetime));
		this.comment = srvHelper.comment;
		this.achievementId = srvHelper.achievement_id;
	}

	// Token: 0x060004BE RID: 1214 RVA: 0x0002244C File Offset: 0x0002064C
	public HelperPackData(FollowsUser serverData)
	{
		this.friendId = serverData.friend_id;
		this.userName = serverData.user_name;
		this.level = serverData.level;
		this.lastLoginTime = new DateTime(PrjUtil.ConvertTimeToTicks(serverData.last_login_time));
		this.HelperCharaSetList = new List<HelperPackData.HelperCharaSet>();
		foreach (FollowsChara followsChara in serverData.help_charas)
		{
			HelperPackData.HelperCharaSet helperCharaSet = new HelperPackData.HelperCharaSet();
			if (followsChara != null && followsChara.chara_id != 0)
			{
				CharaDynamicData charaDynamicData = new CharaDynamicData();
				charaDynamicData.OwnerType = CharaDynamicData.CharaOwnerType.Helper;
				charaDynamicData.UpdateByServer(followsChara);
				helperCharaSet.helpChara = new CharaPackData(charaDynamicData);
				helperCharaSet.equipAccessory = 1 == followsChara.acessory_equip;
			}
			else
			{
				helperCharaSet.helpChara = null;
			}
			helperCharaSet.helpPhotoList = new List<PhotoPackData>();
			this.HelperCharaSetList.Add(helperCharaSet);
		}
		this.isReceiveFollow = serverData.is_receive_follow != 0;
		this.isSendFollow = serverData.is_send_follw != 0;
		this.receiveFollowTime = new DateTime(PrjUtil.ConvertTimeToTicks(serverData.is_receive_follow_datetime));
		this.sendFollowTime = new DateTime(PrjUtil.ConvertTimeToTicks(serverData.is_send_follw_datetime));
		this.comment = serverData.comment;
		this.achievementId = serverData.achievement_id;
	}

	// Token: 0x060004BF RID: 1215 RVA: 0x000225A8 File Offset: 0x000207A8
	public static HelperPackData MakeTutorialDummy(int id, string nm = "")
	{
		HelperPackData helperPackData = new HelperPackData();
		helperPackData.friendId = -id;
		helperPackData.userName = PrjUtil.MakeMessage(string.IsNullOrEmpty(nm) ? "隊長" : nm);
		helperPackData.comment = PrjUtil.MakeMessage("よろしくお願いします");
		helperPackData.level = 1;
		helperPackData.addPoint = 0;
		helperPackData.lastLoginTime = new DateTime(TimeManager.Now.Ticks);
		helperPackData.HelperCharaSetList = new List<HelperPackData.HelperCharaSet>();
		for (int i = 0; i < 7; i++)
		{
			HelperPackData.HelperCharaSet helperCharaSet = new HelperPackData.HelperCharaSet();
			helperCharaSet.helpChara = CharaPackData.MakeDummy(id);
			helperCharaSet.helpChara.dynamicData.OwnerType = CharaDynamicData.CharaOwnerType.Helper;
			helperCharaSet.helpPhotoList = new List<PhotoPackData> { null, null, null, null };
			helperPackData.HelperCharaSetList.Add(helperCharaSet);
		}
		helperPackData.isReceiveFollow = false;
		helperPackData.isSendFollow = false;
		return helperPackData;
	}

	// Token: 0x04000525 RID: 1317
	public Helper helper;

	// Token: 0x04000526 RID: 1318
	public int friendId;

	// Token: 0x04000527 RID: 1319
	public string userName;

	// Token: 0x04000528 RID: 1320
	public int level;

	// Token: 0x04000529 RID: 1321
	public int addPoint;

	// Token: 0x0400052A RID: 1322
	public DateTime lastLoginTime;

	// Token: 0x0400052B RID: 1323
	public static readonly int HELP_ARRAY_SIZE = 7;

	// Token: 0x0400052D RID: 1325
	public bool isReceiveFollow;

	// Token: 0x0400052E RID: 1326
	public bool isSendFollow;

	// Token: 0x0400052F RID: 1327
	public DateTime receiveFollowTime;

	// Token: 0x04000530 RID: 1328
	public DateTime sendFollowTime;

	// Token: 0x04000531 RID: 1329
	public string comment;

	// Token: 0x04000532 RID: 1330
	public int achievementId;

	// Token: 0x020006BD RID: 1725
	public class HelperCharaSet
	{
		// Token: 0x0400304A RID: 12362
		public CharaPackData helpChara;

		// Token: 0x0400304B RID: 12363
		public List<PhotoPackData> helpPhotoList;

		// Token: 0x0400304C RID: 12364
		public bool equipAccessory;

		// Token: 0x0400304D RID: 12365
		public DataManagerCharaAccessory.Accessory helpAccessory;
	}
}
