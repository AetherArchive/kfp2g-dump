using System;
using System.Collections.Generic;
using SGNFW.HttpRequest.Protocol;

public class HelperPackData
{
	public List<HelperPackData.HelperCharaSet> HelperCharaSetList { get; set; }

	public CharaPackData FavoriteChara { get; private set; }

	public bool isDummy
	{
		get
		{
			return this.friendId <= 0;
		}
	}

	public HelperPackData()
	{
	}

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

	public Helper helper;

	public int friendId;

	public string userName;

	public int level;

	public int addPoint;

	public DateTime lastLoginTime;

	public static readonly int HELP_ARRAY_SIZE = 7;

	public bool isReceiveFollow;

	public bool isSendFollow;

	public DateTime receiveFollowTime;

	public DateTime sendFollowTime;

	public string comment;

	public int achievementId;

	public class HelperCharaSet
	{
		public CharaPackData helpChara;

		public List<PhotoPackData> helpPhotoList;

		public bool equipAccessory;

		public DataManagerCharaAccessory.Accessory helpAccessory;
	}
}
