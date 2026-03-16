using System;
using System.Collections.Generic;
using System.Linq;
using DMMHelper;
using SGNFW.Common;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;

public class DataManagerServerMst
{
	public DataManagerServerMst(DataManager p)
	{
		this.parentData = p;
	}

	public List<GameLevelInfo> gameLevelInfoList { get; private set; }

	public List<MstCharaArtsData> mstCharaArtsDataList { get; private set; }

	public List<DataManagerServerMst.CharaLevelItem> charaLevelItemDataList { get; private set; }

	public List<MstCharaPromoteData> mstCharaPromoteDataList { get; private set; }

	public List<MstCharaPromotePresetData> mstCharaPromotePresetDataList { get; private set; }

	public Dictionary<int, DataManagerServerMst.StaticPpData> StaticCharaPpDataMap { get; private set; }

	public List<MstCharaRankData> mstCharaRankDataList { get; private set; }

	public List<MstHelpData> mstHelpDataList { get; private set; }

	public List<MstTipsData> mstTipsDataList { get; private set; }

	public List<MstMovieData> mstMovieDataList { get; private set; }

	public List<DataManagerServerMst.ModeReleaseData> ModeReleaseDataList { get; private set; }

	public List<MstPlatformStatusData> mstPlatformStatusDataList { get; private set; }

	public List<AdvertiseBannerData> advertiseBannerDataList { get; private set; }

	public List<MstCharaNanairoAbilityReleaseData> mstNanairoReleaseData { get; private set; }

	public bool IsEnableNoahWeb()
	{
		return false;
	}

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

	public void InitializeAdvertiseBannerData()
	{
		this.advertiseBannerDataList = new List<AdvertiseBannerData>();
		foreach (MstAdvertiseBannerData mstAdvertiseBannerData in Singleton<MstManager>.Instance.GetMst<List<MstAdvertiseBannerData>>(MstType.ADVERTISE_BANNER_DATA))
		{
			this.advertiseBannerDataList.Add(new AdvertiseBannerData(mstAdvertiseBannerData));
		}
	}

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

	public void CbServerConfigCmd(Command cmd)
	{
		TimeManager.SetServerStartTime((cmd.response as ServerConfigResponse).server_time);
	}

	private DataManager parentData;

	public class CharaLevelItem
	{
		public CharaLevelItem(MstCharaLevelItemData mstChLvItem)
		{
			this.itemId = mstChLvItem.itemId;
			this.attribute = (CharaDef.AttributeType)mstChLvItem.attribute;
			this.exp = mstChLvItem.exp;
			this.attributeExp = mstChLvItem.attributeExp;
			this.strengCoinNum = mstChLvItem.strengCoinNum;
			this.isKizuna = mstChLvItem.isKizuna;
		}

		public int itemId;

		public CharaDef.AttributeType attribute;

		public long exp;

		public long attributeExp;

		public int strengCoinNum;

		public int isKizuna;
	}

	public class ModeReleaseData
	{
		public DataManagerServerMst.ModeReleaseData.ModeCategory Category { get; private set; }

		public int QuestId { get; private set; }

		public string WindowTitle { get; private set; }

		public string WindowText { get; private set; }

		public int OriginQuestId { get; private set; }

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

		public enum ModeCategory
		{
			GrowthQuest,
			FriendsStory,
			PvpMode,
			Picnic,
			Picnic2,
			Picnic3,
			Picnic4,
			AraiDiary,
			TrainingMode,
			DholeInactive,
			DholeReturns,
			Cellval,
			TreeHouse,
			CharaCommunication,
			EtceteraQuest,
			MainStory2,
			MainStory3,
			Assistant
		}
	}

	public class StaticPpData
	{
		public int PpId { get; private set; }

		public int PpStepMax { get; set; }

		public List<DataManagerServerMst.StaticPpData.PpDataOne> PpDataList { get; set; }

		public StaticPpData(int ppId)
		{
			this.PpId = ppId;
			this.PpDataList = new List<DataManagerServerMst.StaticPpData.PpDataOne>();
		}

		public void AddPpData(MstCharaPpData mstChPpData)
		{
			if (this.PpStepMax < mstChPpData.ppStep)
			{
				this.PpStepMax = mstChPpData.ppStep + 1;
			}
			this.PpDataList.Add(new DataManagerServerMst.StaticPpData.PpDataOne(mstChPpData));
		}

		public void Sort(Comparison<DataManagerServerMst.StaticPpData.PpDataOne> comparison)
		{
			this.PpDataList.Sort(comparison);
		}

		public class PpDataOne
		{
			public int Index { get; private set; }

			public int Step
			{
				get
				{
					return this.Index + 1;
				}
			}

			public int DispIndex { get; private set; }

			public int CostNum { get; private set; }

			public int HpRate { get; private set; }

			public int AtkRate { get; private set; }

			public int DefRate { get; private set; }

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
