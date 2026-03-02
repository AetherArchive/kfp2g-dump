using System;
using System.Collections.Generic;
using System.Linq;
using DMMHelper;
using SGNFW.Common;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;

// Token: 0x020000A3 RID: 163
public class DataManagerServerMst
{
	// Token: 0x06000729 RID: 1833 RVA: 0x00031BCC File Offset: 0x0002FDCC
	public DataManagerServerMst(DataManager p)
	{
		this.parentData = p;
	}

	// Token: 0x17000151 RID: 337
	// (get) Token: 0x0600072A RID: 1834 RVA: 0x00031C67 File Offset: 0x0002FE67
	// (set) Token: 0x0600072B RID: 1835 RVA: 0x00031C6F File Offset: 0x0002FE6F
	public List<GameLevelInfo> gameLevelInfoList { get; private set; }

	// Token: 0x17000152 RID: 338
	// (get) Token: 0x0600072C RID: 1836 RVA: 0x00031C78 File Offset: 0x0002FE78
	// (set) Token: 0x0600072D RID: 1837 RVA: 0x00031C80 File Offset: 0x0002FE80
	public List<MstCharaArtsData> mstCharaArtsDataList { get; private set; }

	// Token: 0x17000153 RID: 339
	// (get) Token: 0x0600072E RID: 1838 RVA: 0x00031C89 File Offset: 0x0002FE89
	// (set) Token: 0x0600072F RID: 1839 RVA: 0x00031C91 File Offset: 0x0002FE91
	public List<DataManagerServerMst.CharaLevelItem> charaLevelItemDataList { get; private set; }

	// Token: 0x17000154 RID: 340
	// (get) Token: 0x06000730 RID: 1840 RVA: 0x00031C9A File Offset: 0x0002FE9A
	// (set) Token: 0x06000731 RID: 1841 RVA: 0x00031CA2 File Offset: 0x0002FEA2
	public List<MstCharaPromoteData> mstCharaPromoteDataList { get; private set; }

	// Token: 0x17000155 RID: 341
	// (get) Token: 0x06000732 RID: 1842 RVA: 0x00031CAB File Offset: 0x0002FEAB
	// (set) Token: 0x06000733 RID: 1843 RVA: 0x00031CB3 File Offset: 0x0002FEB3
	public List<MstCharaPromotePresetData> mstCharaPromotePresetDataList { get; private set; }

	// Token: 0x17000156 RID: 342
	// (get) Token: 0x06000734 RID: 1844 RVA: 0x00031CBC File Offset: 0x0002FEBC
	// (set) Token: 0x06000735 RID: 1845 RVA: 0x00031CC4 File Offset: 0x0002FEC4
	public Dictionary<int, DataManagerServerMst.StaticPpData> StaticCharaPpDataMap { get; private set; }

	// Token: 0x17000157 RID: 343
	// (get) Token: 0x06000736 RID: 1846 RVA: 0x00031CCD File Offset: 0x0002FECD
	// (set) Token: 0x06000737 RID: 1847 RVA: 0x00031CD5 File Offset: 0x0002FED5
	public List<MstCharaRankData> mstCharaRankDataList { get; private set; }

	// Token: 0x17000158 RID: 344
	// (get) Token: 0x06000738 RID: 1848 RVA: 0x00031CDE File Offset: 0x0002FEDE
	// (set) Token: 0x06000739 RID: 1849 RVA: 0x00031CE6 File Offset: 0x0002FEE6
	public List<MstHelpData> mstHelpDataList { get; private set; }

	// Token: 0x17000159 RID: 345
	// (get) Token: 0x0600073A RID: 1850 RVA: 0x00031CEF File Offset: 0x0002FEEF
	// (set) Token: 0x0600073B RID: 1851 RVA: 0x00031CF7 File Offset: 0x0002FEF7
	public List<MstTipsData> mstTipsDataList { get; private set; }

	// Token: 0x1700015A RID: 346
	// (get) Token: 0x0600073C RID: 1852 RVA: 0x00031D00 File Offset: 0x0002FF00
	// (set) Token: 0x0600073D RID: 1853 RVA: 0x00031D08 File Offset: 0x0002FF08
	public List<MstMovieData> mstMovieDataList { get; private set; }

	// Token: 0x1700015B RID: 347
	// (get) Token: 0x0600073E RID: 1854 RVA: 0x00031D11 File Offset: 0x0002FF11
	// (set) Token: 0x0600073F RID: 1855 RVA: 0x00031D19 File Offset: 0x0002FF19
	public List<DataManagerServerMst.ModeReleaseData> ModeReleaseDataList { get; private set; }

	// Token: 0x1700015C RID: 348
	// (get) Token: 0x06000740 RID: 1856 RVA: 0x00031D22 File Offset: 0x0002FF22
	// (set) Token: 0x06000741 RID: 1857 RVA: 0x00031D2A File Offset: 0x0002FF2A
	public List<MstPlatformStatusData> mstPlatformStatusDataList { get; private set; }

	// Token: 0x1700015D RID: 349
	// (get) Token: 0x06000742 RID: 1858 RVA: 0x00031D33 File Offset: 0x0002FF33
	// (set) Token: 0x06000743 RID: 1859 RVA: 0x00031D3B File Offset: 0x0002FF3B
	public List<AdvertiseBannerData> advertiseBannerDataList { get; private set; }

	// Token: 0x1700015E RID: 350
	// (get) Token: 0x06000744 RID: 1860 RVA: 0x00031D44 File Offset: 0x0002FF44
	// (set) Token: 0x06000745 RID: 1861 RVA: 0x00031D4C File Offset: 0x0002FF4C
	public List<MstCharaNanairoAbilityReleaseData> mstNanairoReleaseData { get; private set; }

	// Token: 0x06000746 RID: 1862 RVA: 0x00031D55 File Offset: 0x0002FF55
	public bool IsEnableNoahWeb()
	{
		return false;
	}

	// Token: 0x1700015F RID: 351
	// (get) Token: 0x06000747 RID: 1863 RVA: 0x00031D58 File Offset: 0x0002FF58
	// (set) Token: 0x06000748 RID: 1864 RVA: 0x00031D60 File Offset: 0x0002FF60
	public MstAppConfig MstAppConfig { get; private set; } = new MstAppConfig
	{
		photoStockLimit = 100,
		accessoryStocKLimit = 1000,
		followLimit = 100,
		followerLimit = 100,
		presentLimit = 1000,
		historyLimit = 1000,
		staminaRecoveryTime = 300,
		staminaLimit = 999,
		pvpRecoveryTime = 3600,
		pvpLimit = 5,
		staminaStone = 5,
		pvpStone = 5,
		continueStone = 3
	};

	// Token: 0x06000749 RID: 1865 RVA: 0x00031D6C File Offset: 0x0002FF6C
	public void InitializeMstData(MstManager mstManager)
	{
		List<MstPhotoLevelData> mst8 = mstManager.GetMst<List<MstPhotoLevelData>>(MstType.PHOTO_LEVEL_DATA);
		List<MstCharaLevelData> mst2 = mstManager.GetMst<List<MstCharaLevelData>>(MstType.CHARA_LEVEL_DATA);
		this.mstCharaArtsDataList = mstManager.GetMst<List<MstCharaArtsData>>(MstType.CHARA_ARTS_DATA);
		List<MstCharaLevelItemData> mst3 = mstManager.GetMst<List<MstCharaLevelItemData>>(MstType.CHARA_LEVEL_ITEM_DATA);
		this.mstCharaPromoteDataList = mstManager.GetMst<List<MstCharaPromoteData>>(MstType.CHARA_PROMOTE_DATA);
		this.mstCharaPromotePresetDataList = mstManager.GetMst<List<MstCharaPromotePresetData>>(MstType.CHARA_PROMOTE_PRESET_DATA);
		this.StaticCharaPpDataMap = new Dictionary<int, DataManagerServerMst.StaticPpData>();
		this.mstNanairoReleaseData = mstManager.GetMst<List<MstCharaNanairoAbilityReleaseData>>(MstType.CHARA_NANAIRO_ABILITY_RELEASE_DATA);
		foreach (MstCharaPpData mstCharaPpData in mstManager.GetMst<List<MstCharaPpData>>(MstType.CHARA_PP_DATA))
		{
			if (!this.StaticCharaPpDataMap.ContainsKey(mstCharaPpData.ppId))
			{
				this.StaticCharaPpDataMap.Add(mstCharaPpData.ppId, new DataManagerServerMst.StaticPpData(mstCharaPpData.ppId));
			}
			this.StaticCharaPpDataMap[mstCharaPpData.ppId].AddPpData(mstCharaPpData);
		}
		foreach (KeyValuePair<int, DataManagerServerMst.StaticPpData> keyValuePair in this.StaticCharaPpDataMap)
		{
			keyValuePair.Value.Sort((DataManagerServerMst.StaticPpData.PpDataOne a, DataManagerServerMst.StaticPpData.PpDataOne b) => a.Step - b.Step);
		}
		this.mstCharaRankDataList = mstManager.GetMst<List<MstCharaRankData>>(MstType.CHARA_RANK_DATA);
		this.mstHelpDataList = mstManager.GetMst<List<MstHelpData>>(MstType.HELP_DATA);
		this.mstTipsDataList = mstManager.GetMst<List<MstTipsData>>(MstType.TIPS_DATA);
		List<MstLevelData> mst4 = mstManager.GetMst<List<MstLevelData>>(MstType.LEVEL_DATA);
		List<MstCharaKizunaLevelData> mst5 = mstManager.GetMst<List<MstCharaKizunaLevelData>>(MstType.CHARA_KIZUNA_LEVEL_DATA);
		this.mstMovieDataList = mstManager.GetMst<List<MstMovieData>>(MstType.MOVIE_DATA);
		List<MstQuestModeOpenData> mst6 = mstManager.GetMst<List<MstQuestModeOpenData>>(MstType.QUEST_MODE_OPEN_DATA);
		List<MstAppConfig> mst7 = Singleton<MstManager>.Instance.GetMst<List<MstAppConfig>>(MstType.APP_CONFIG);
		this.mstPlatformStatusDataList = Singleton<MstManager>.Instance.GetMst<List<MstPlatformStatusData>>(MstType.PLATFORM_STATUS_DATA);
		long ticks = TimeManager.Now.Ticks;
		PrjUtil.ConvertTicksToTime(ticks);
		this.gameLevelInfoList = new List<GameLevelInfo>();
		MstAppConfig mstAppConfig = mst7.Find((MstAppConfig item) => item.id == 1);
		if (mstAppConfig != null)
		{
			this.MstAppConfig = mstAppConfig;
		}
		int num = mst8.Max<MstPhotoLevelData>((MstPhotoLevelData item) => item.level);
		num = Math.Max(num, mst2.Max<MstCharaLevelData>((MstCharaLevelData item) => item.level));
		num = Math.Max(num, mst4.Max<MstLevelData>((MstLevelData item) => item.level));
		for (int i = 0; i < num; i++)
		{
			int num2 = i + 1;
			this.gameLevelInfoList.Add(new GameLevelInfo
			{
				level = num2
			});
		}
		using (List<MstLevelData>.Enumerator enumerator3 = mst4.GetEnumerator())
		{
			while (enumerator3.MoveNext())
			{
				MstLevelData mst9 = enumerator3.Current;
				if (PrjUtil.ConvertTimeToTicks(mst9.playerReleaseDatetime) <= ticks)
				{
					int num3 = mst9.level - 1;
					long num4 = mst9.playerExp;
					if (num3 > 0)
					{
						num4 -= mst4.Find((MstLevelData item) => item.level == mst9.level - 1).playerExp;
					}
					this.gameLevelInfoList[num3].userLevelExp = num4;
					this.gameLevelInfoList[num3].staminaLimit = mst9.staminaLimit;
				}
			}
		}
		using (List<MstCharaLevelData>.Enumerator enumerator4 = mst2.GetEnumerator())
		{
			while (enumerator4.MoveNext())
			{
				MstCharaLevelData mst10 = enumerator4.Current;
				if (PrjUtil.ConvertTimeToTicks(mst10.charaReleaseDatetime) <= ticks)
				{
					int num5 = mst10.level - 1;
					long num6 = mst10.charaExp;
					if (num5 > 0)
					{
						num6 -= mst2.Find((MstCharaLevelData item) => item.levelId == mst10.levelId && item.level == mst10.level - 1).charaExp;
					}
					this.gameLevelInfoList[num5].charaLevelExp.Add(mst10.levelId, num6);
				}
			}
		}
		using (List<MstCharaKizunaLevelData>.Enumerator enumerator5 = mst5.GetEnumerator())
		{
			while (enumerator5.MoveNext())
			{
				MstCharaKizunaLevelData mstKizuna = enumerator5.Current;
				if (PrjUtil.ConvertTimeToTicks(mstKizuna.levelReleaseDatetime) <= ticks)
				{
					int num7 = mstKizuna.level - 1;
					long num8 = mstKizuna.kizunaExp;
					if (num7 > 0)
					{
						num8 -= mst5.Find((MstCharaKizunaLevelData item) => item.levelId == mstKizuna.levelId && item.level == mstKizuna.level - 1).kizunaExp;
					}
					GameLevelInfo.KizunaLevelData kizunaLevelData = new GameLevelInfo.KizunaLevelData(num8, mstKizuna);
					this.gameLevelInfoList[num7].kizunaLevelExp.Add(mstKizuna.levelId, kizunaLevelData);
				}
			}
		}
		using (List<MstPhotoLevelData>.Enumerator enumerator6 = mst8.GetEnumerator())
		{
			while (enumerator6.MoveNext())
			{
				MstPhotoLevelData mst = enumerator6.Current;
				if (PrjUtil.ConvertTimeToTicks(mst.photoReleaseDatetime) <= ticks)
				{
					int num9 = mst.level - 1;
					long num10 = mst.photoExp;
					if (num9 > 0)
					{
						num10 -= mst8.Find((MstPhotoLevelData item) => item.levelId == mst.levelId && item.level == mst.level - 1).photoExp;
					}
					this.gameLevelInfoList[num9].photoLevelExp.Add(mst.levelId, num10);
				}
			}
		}
		this.charaLevelItemDataList = new List<DataManagerServerMst.CharaLevelItem>();
		foreach (MstCharaLevelItemData mstCharaLevelItemData in mst3)
		{
			DataManagerServerMst.CharaLevelItem charaLevelItem = new DataManagerServerMst.CharaLevelItem(mstCharaLevelItemData);
			this.charaLevelItemDataList.Add(charaLevelItem);
		}
		this.ModeReleaseDataList = new List<DataManagerServerMst.ModeReleaseData>();
		foreach (MstQuestModeOpenData mstQuestModeOpenData in mst6)
		{
			DataManagerServerMst.ModeReleaseData modeReleaseData = new DataManagerServerMst.ModeReleaseData(mstQuestModeOpenData);
			this.ModeReleaseDataList.Add(modeReleaseData);
		}
	}

	// Token: 0x0600074A RID: 1866 RVA: 0x00032420 File Offset: 0x00030620
	public void InitializeAdvertiseBannerData()
	{
		this.advertiseBannerDataList = new List<AdvertiseBannerData>();
		foreach (MstAdvertiseBannerData mstAdvertiseBannerData in Singleton<MstManager>.Instance.GetMst<List<MstAdvertiseBannerData>>(MstType.ADVERTISE_BANNER_DATA))
		{
			this.advertiseBannerDataList.Add(new AdvertiseBannerData(mstAdvertiseBannerData));
		}
	}

	// Token: 0x0600074B RID: 1867 RVA: 0x00032490 File Offset: 0x00030690
	public void RequestDownloadServerTime(Action callback = null)
	{
		this.parentData.ServerRequest(ServerConfigCmd.Create(Singleton<DMMHelpManager>.Instance.VewerID), delegate(Command cmd)
		{
			this.CbServerConfigCmd(cmd);
			Action callback2 = callback;
			if (callback2 == null)
			{
				return;
			}
			callback2();
		});
	}

	// Token: 0x0600074C RID: 1868 RVA: 0x000324D7 File Offset: 0x000306D7
	public void CbServerConfigCmd(Command cmd)
	{
		TimeManager.SetServerStartTime((cmd.response as ServerConfigResponse).server_time);
	}

	// Token: 0x0400064F RID: 1615
	private DataManager parentData;

	// Token: 0x0200077A RID: 1914
	public class CharaLevelItem
	{
		// Token: 0x0600364C RID: 13900 RVA: 0x001C6168 File Offset: 0x001C4368
		public CharaLevelItem(MstCharaLevelItemData mstChLvItem)
		{
			this.itemId = mstChLvItem.itemId;
			this.attribute = (CharaDef.AttributeType)mstChLvItem.attribute;
			this.exp = mstChLvItem.exp;
			this.attributeExp = mstChLvItem.attributeExp;
			this.strengCoinNum = mstChLvItem.strengCoinNum;
			this.isKizuna = mstChLvItem.isKizuna;
		}

		// Token: 0x0400333A RID: 13114
		public int itemId;

		// Token: 0x0400333B RID: 13115
		public CharaDef.AttributeType attribute;

		// Token: 0x0400333C RID: 13116
		public long exp;

		// Token: 0x0400333D RID: 13117
		public long attributeExp;

		// Token: 0x0400333E RID: 13118
		public int strengCoinNum;

		// Token: 0x0400333F RID: 13119
		public int isKizuna;
	}

	// Token: 0x0200077B RID: 1915
	public class ModeReleaseData
	{
		// Token: 0x17000800 RID: 2048
		// (get) Token: 0x0600364D RID: 13901 RVA: 0x001C61C3 File Offset: 0x001C43C3
		// (set) Token: 0x0600364E RID: 13902 RVA: 0x001C61CB File Offset: 0x001C43CB
		public DataManagerServerMst.ModeReleaseData.ModeCategory Category { get; private set; }

		// Token: 0x17000801 RID: 2049
		// (get) Token: 0x0600364F RID: 13903 RVA: 0x001C61D4 File Offset: 0x001C43D4
		// (set) Token: 0x06003650 RID: 13904 RVA: 0x001C61DC File Offset: 0x001C43DC
		public int QuestId { get; private set; }

		// Token: 0x17000802 RID: 2050
		// (get) Token: 0x06003651 RID: 13905 RVA: 0x001C61E5 File Offset: 0x001C43E5
		// (set) Token: 0x06003652 RID: 13906 RVA: 0x001C61ED File Offset: 0x001C43ED
		public string WindowTitle { get; private set; }

		// Token: 0x17000803 RID: 2051
		// (get) Token: 0x06003653 RID: 13907 RVA: 0x001C61F6 File Offset: 0x001C43F6
		// (set) Token: 0x06003654 RID: 13908 RVA: 0x001C61FE File Offset: 0x001C43FE
		public string WindowText { get; private set; }

		// Token: 0x17000804 RID: 2052
		// (get) Token: 0x06003655 RID: 13909 RVA: 0x001C6207 File Offset: 0x001C4407
		// (set) Token: 0x06003656 RID: 13910 RVA: 0x001C620F File Offset: 0x001C440F
		public int OriginQuestId { get; private set; }

		// Token: 0x06003657 RID: 13911 RVA: 0x001C6218 File Offset: 0x001C4418
		public ModeReleaseData(MstQuestModeOpenData mstQuestModeOpenData)
		{
			switch (mstQuestModeOpenData.releaseMode)
			{
			case 1:
				this.Category = DataManagerServerMst.ModeReleaseData.ModeCategory.GrowthQuest;
				break;
			case 2:
				this.Category = DataManagerServerMst.ModeReleaseData.ModeCategory.Picnic;
				break;
			case 3:
				this.Category = DataManagerServerMst.ModeReleaseData.ModeCategory.FriendsStory;
				break;
			case 4:
				this.Category = DataManagerServerMst.ModeReleaseData.ModeCategory.Picnic2;
				break;
			case 5:
				this.Category = DataManagerServerMst.ModeReleaseData.ModeCategory.Picnic3;
				break;
			case 6:
				this.Category = DataManagerServerMst.ModeReleaseData.ModeCategory.PvpMode;
				break;
			case 7:
				this.Category = DataManagerServerMst.ModeReleaseData.ModeCategory.Picnic4;
				break;
			case 8:
				this.Category = DataManagerServerMst.ModeReleaseData.ModeCategory.AraiDiary;
				break;
			case 9:
				this.Category = DataManagerServerMst.ModeReleaseData.ModeCategory.TrainingMode;
				break;
			case 10:
				this.Category = DataManagerServerMst.ModeReleaseData.ModeCategory.DholeInactive;
				break;
			case 11:
				this.Category = DataManagerServerMst.ModeReleaseData.ModeCategory.DholeReturns;
				break;
			case 12:
				this.Category = DataManagerServerMst.ModeReleaseData.ModeCategory.Cellval;
				break;
			case 13:
				this.Category = DataManagerServerMst.ModeReleaseData.ModeCategory.TreeHouse;
				break;
			case 14:
				this.Category = DataManagerServerMst.ModeReleaseData.ModeCategory.CharaCommunication;
				break;
			case 15:
				this.Category = DataManagerServerMst.ModeReleaseData.ModeCategory.EtceteraQuest;
				break;
			case 16:
				this.Category = DataManagerServerMst.ModeReleaseData.ModeCategory.MainStory2;
				break;
			case 17:
				this.Category = DataManagerServerMst.ModeReleaseData.ModeCategory.MainStory3;
				break;
			case 18:
				this.Category = DataManagerServerMst.ModeReleaseData.ModeCategory.Assistant;
				break;
			}
			this.QuestId = mstQuestModeOpenData.questoneId;
			this.WindowTitle = mstQuestModeOpenData.title;
			this.WindowText = mstQuestModeOpenData.text;
			this.OriginQuestId = mstQuestModeOpenData.originQuestoneId;
		}

		// Token: 0x02001144 RID: 4420
		public enum ModeCategory
		{
			// Token: 0x04005ED7 RID: 24279
			GrowthQuest,
			// Token: 0x04005ED8 RID: 24280
			FriendsStory,
			// Token: 0x04005ED9 RID: 24281
			PvpMode,
			// Token: 0x04005EDA RID: 24282
			Picnic,
			// Token: 0x04005EDB RID: 24283
			Picnic2,
			// Token: 0x04005EDC RID: 24284
			Picnic3,
			// Token: 0x04005EDD RID: 24285
			Picnic4,
			// Token: 0x04005EDE RID: 24286
			AraiDiary,
			// Token: 0x04005EDF RID: 24287
			TrainingMode,
			// Token: 0x04005EE0 RID: 24288
			DholeInactive,
			// Token: 0x04005EE1 RID: 24289
			DholeReturns,
			// Token: 0x04005EE2 RID: 24290
			Cellval,
			// Token: 0x04005EE3 RID: 24291
			TreeHouse,
			// Token: 0x04005EE4 RID: 24292
			CharaCommunication,
			// Token: 0x04005EE5 RID: 24293
			EtceteraQuest,
			// Token: 0x04005EE6 RID: 24294
			MainStory2,
			// Token: 0x04005EE7 RID: 24295
			MainStory3,
			// Token: 0x04005EE8 RID: 24296
			Assistant
		}
	}

	// Token: 0x0200077C RID: 1916
	public class StaticPpData
	{
		// Token: 0x17000805 RID: 2053
		// (get) Token: 0x06003658 RID: 13912 RVA: 0x001C636C File Offset: 0x001C456C
		// (set) Token: 0x06003659 RID: 13913 RVA: 0x001C6374 File Offset: 0x001C4574
		public int PpId { get; private set; }

		// Token: 0x17000806 RID: 2054
		// (get) Token: 0x0600365A RID: 13914 RVA: 0x001C637D File Offset: 0x001C457D
		// (set) Token: 0x0600365B RID: 13915 RVA: 0x001C6385 File Offset: 0x001C4585
		public int PpStepMax { get; set; }

		// Token: 0x17000807 RID: 2055
		// (get) Token: 0x0600365C RID: 13916 RVA: 0x001C638E File Offset: 0x001C458E
		// (set) Token: 0x0600365D RID: 13917 RVA: 0x001C6396 File Offset: 0x001C4596
		public List<DataManagerServerMst.StaticPpData.PpDataOne> PpDataList { get; set; }

		// Token: 0x0600365E RID: 13918 RVA: 0x001C639F File Offset: 0x001C459F
		public StaticPpData(int ppId)
		{
			this.PpId = ppId;
			this.PpDataList = new List<DataManagerServerMst.StaticPpData.PpDataOne>();
		}

		// Token: 0x0600365F RID: 13919 RVA: 0x001C63B9 File Offset: 0x001C45B9
		public void AddPpData(MstCharaPpData mstChPpData)
		{
			if (this.PpStepMax < mstChPpData.ppStep)
			{
				this.PpStepMax = mstChPpData.ppStep + 1;
			}
			this.PpDataList.Add(new DataManagerServerMst.StaticPpData.PpDataOne(mstChPpData));
		}

		// Token: 0x06003660 RID: 13920 RVA: 0x001C63E8 File Offset: 0x001C45E8
		public void Sort(Comparison<DataManagerServerMst.StaticPpData.PpDataOne> comparison)
		{
			this.PpDataList.Sort(comparison);
		}

		// Token: 0x02001145 RID: 4421
		public class PpDataOne
		{
			// Token: 0x17000C8D RID: 3213
			// (get) Token: 0x0600557F RID: 21887 RVA: 0x0024F20B File Offset: 0x0024D40B
			// (set) Token: 0x06005580 RID: 21888 RVA: 0x0024F213 File Offset: 0x0024D413
			public int Index { get; private set; }

			// Token: 0x17000C8E RID: 3214
			// (get) Token: 0x06005581 RID: 21889 RVA: 0x0024F21C File Offset: 0x0024D41C
			public int Step
			{
				get
				{
					return this.Index + 1;
				}
			}

			// Token: 0x17000C8F RID: 3215
			// (get) Token: 0x06005582 RID: 21890 RVA: 0x0024F226 File Offset: 0x0024D426
			// (set) Token: 0x06005583 RID: 21891 RVA: 0x0024F22E File Offset: 0x0024D42E
			public int DispIndex { get; private set; }

			// Token: 0x17000C90 RID: 3216
			// (get) Token: 0x06005584 RID: 21892 RVA: 0x0024F237 File Offset: 0x0024D437
			// (set) Token: 0x06005585 RID: 21893 RVA: 0x0024F23F File Offset: 0x0024D43F
			public int CostNum { get; private set; }

			// Token: 0x17000C91 RID: 3217
			// (get) Token: 0x06005586 RID: 21894 RVA: 0x0024F248 File Offset: 0x0024D448
			// (set) Token: 0x06005587 RID: 21895 RVA: 0x0024F250 File Offset: 0x0024D450
			public int HpRate { get; private set; }

			// Token: 0x17000C92 RID: 3218
			// (get) Token: 0x06005588 RID: 21896 RVA: 0x0024F259 File Offset: 0x0024D459
			// (set) Token: 0x06005589 RID: 21897 RVA: 0x0024F261 File Offset: 0x0024D461
			public int AtkRate { get; private set; }

			// Token: 0x17000C93 RID: 3219
			// (get) Token: 0x0600558A RID: 21898 RVA: 0x0024F26A File Offset: 0x0024D46A
			// (set) Token: 0x0600558B RID: 21899 RVA: 0x0024F272 File Offset: 0x0024D472
			public int DefRate { get; private set; }

			// Token: 0x0600558C RID: 21900 RVA: 0x0024F27C File Offset: 0x0024D47C
			public PpDataOne(MstCharaPpData mstChPpData)
			{
				this.Index = mstChPpData.ppStep;
				this.DispIndex = mstChPpData.ppStep % 4;
				this.CostNum = mstChPpData.useCostFragmentNum;
				this.HpRate = mstChPpData.hpRate;
				this.AtkRate = mstChPpData.attackRate;
				this.DefRate = mstChPpData.defenseRate;
			}
		}
	}
}
