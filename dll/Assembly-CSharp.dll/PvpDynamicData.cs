using System;
using System.Collections.Generic;
using Battle;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;

// Token: 0x0200009E RID: 158
public class PvpDynamicData
{
	// Token: 0x060006B4 RID: 1716 RVA: 0x0002D242 File Offset: 0x0002B442
	public PvpDynamicData()
	{
	}

	// Token: 0x060006B5 RID: 1717 RVA: 0x0002D258 File Offset: 0x0002B458
	public PvpDynamicData(PvPInfo srvPvpInfo, PvpStaticData staticData, DataManagerPvp dataManagerPvp)
	{
		this.seasonId = staticData.seasonId;
		MstAppConfig mstAppConfig = DataManager.DmServerMst.MstAppConfig;
		int num = ((staticData.type == PvpStaticData.Type.SPECIAL) ? mstAppConfig.pvpspecialLimit : mstAppConfig.pvpLimit);
		long num2 = (long)((staticData.type == PvpStaticData.Type.SPECIAL) ? mstAppConfig.pvpspecialRecoveryTime : mstAppConfig.pvpRecoveryTime);
		this.userInfo = new PvpDynamicData.UserInfo
		{
			pvpPoint = srvPvpInfo.pvp_point_now,
			currentDeckId = srvPvpInfo.set_deck_id,
			pvpStaminaInfo = new StaminaInfo(srvPvpInfo.limit_chal_count, PrjUtil.ConvertTimeToTicks(srvPvpInfo.last_chal_datetime), TimeManager.Second2Tick(num2), num, null),
			winningNum = 1
		};
		this.enemyInfoList = new List<PvpDynamicData.EnemyInfo>();
		foreach (OppUser oppUser in srvPvpInfo.opp_user_list)
		{
			PvpDynamicData.EnemyInfo enemyInfo = new PvpDynamicData.EnemyInfo(oppUser);
			foreach (int num3 in oppUser.kemoboard_panel_list)
			{
				DataManager.DmKemoBoard.UpdateKemoboardBonusParam(ref enemyInfo.kemoBoardParamMap, num3);
			}
			this.enemyInfoList.Add(enemyInfo);
		}
		this.SortEnemyInfoList();
		this.defenseResultList = new List<PvpDynamicData.DefenseResult>();
		foreach (PvPDefenseResult pvPDefenseResult in srvPvpInfo.pvp_defense_result)
		{
			DateTime tgtTime = new DateTime(PrjUtil.ConvertTimeToTicks(pvPDefenseResult.battle_datetime));
			PvpDynamicData.DefenseResult defenseResult = this.defenseResultList.Find((PvpDynamicData.DefenseResult item) => item.time.Year == tgtTime.Year && item.time.Year == tgtTime.Year && item.time.Year == tgtTime.Year);
			if (defenseResult == null)
			{
				defenseResult = new PvpDynamicData.DefenseResult();
				this.defenseResultList.Add(defenseResult);
			}
			defenseResult.winNum += ((pvPDefenseResult.decision == 1) ? 1 : 0);
			defenseResult.loseNum += ((pvPDefenseResult.decision == 1) ? 0 : 10);
			defenseResult.time = tgtTime;
		}
		foreach (PvpDynamicData.DefenseResult defenseResult2 in this.defenseResultList)
		{
			for (int i = 0; i < staticData.pvpDefenseList.Count; i++)
			{
				int num4 = staticData.pvpDefenseList.Count - i - 1;
				if (staticData.pvpDefenseList[num4].winNum <= defenseResult2.winNum)
				{
					this.isHappenDefenseBonus = true;
					List<ItemInput> list = new List<ItemInput>();
					list.Add(new ItemInput(staticData.pvpDefenseList[num4].itemId00, staticData.pvpDefenseList[num4].itemNum00));
					list.Add(new ItemInput(staticData.pvpDefenseList[num4].itemId01, staticData.pvpDefenseList[num4].itemNum01));
					list.Add(new ItemInput(staticData.pvpDefenseList[num4].itemId02, staticData.pvpDefenseList[num4].itemNum02));
					list.Add(new ItemInput(staticData.pvpDefenseList[num4].itemId03, staticData.pvpDefenseList[num4].itemNum03));
					list.Add(new ItemInput(staticData.pvpDefenseList[num4].itemId04, staticData.pvpDefenseList[num4].itemNum04));
					defenseResult2.bonusItemList = new List<ItemData>();
					using (List<ItemInput>.Enumerator enumerator5 = list.GetEnumerator())
					{
						while (enumerator5.MoveNext())
						{
							ItemInput itemInput = enumerator5.Current;
							if (itemInput.itemId != 0)
							{
								defenseResult2.bonusItemList.Add(new ItemData(itemInput.itemId, itemInput.num));
							}
						}
						break;
					}
				}
			}
		}
		this.pvpRankBeforReset = srvPvpInfo.pvp_rank_before;
		this.pvpSeasonIdBeforReset = srvPvpInfo.before_season_id;
		this.isHappenReset = srvPvpInfo.before_season_id != 0 && srvPvpInfo.before_season_id != srvPvpInfo.season_id;
	}

	// Token: 0x060006B6 RID: 1718 RVA: 0x0002D710 File Offset: 0x0002B910
	public void SortEnemyInfoList()
	{
		this.enemyInfoList.Sort(delegate(PvpDynamicData.EnemyInfo a, PvpDynamicData.EnemyInfo b)
		{
			bool flag = a.difficulty == PvpDynamicData.EnemyInfo.Difficulty.CHAMPION;
			bool flag2 = b.difficulty == PvpDynamicData.EnemyInfo.Difficulty.CHAMPION;
			if (flag && !flag2)
			{
				return -1;
			}
			if (!flag && flag2)
			{
				return 1;
			}
			return a.difficulty - b.difficulty;
		});
	}

	// Token: 0x060006B7 RID: 1719 RVA: 0x0002D73C File Offset: 0x0002B93C
	public SelPvpCtrl.SelectorEffect GetSelectorEffect()
	{
		return new SelPvpCtrl.SelectorEffect
		{
			seasonId = this.seasonId,
			isHappenReset = this.isHappenReset,
			pvpRankBeforReset = this.pvpRankBeforReset,
			pvpSeasonIdBeforReset = this.pvpSeasonIdBeforReset,
			isHappenDefenseBonus = this.isHappenDefenseBonus,
			defenseResultList = this.defenseResultList
		};
	}

	// Token: 0x060006B8 RID: 1720 RVA: 0x0002D798 File Offset: 0x0002B998
	public static PvpDynamicData MakeDummy(int id)
	{
		PvpDynamicData pvpDynamicData = new PvpDynamicData();
		pvpDynamicData.seasonId = id;
		pvpDynamicData.userInfo = new PvpDynamicData.UserInfo
		{
			pvpPoint = 60,
			currentDeckId = 1,
			pvpStaminaInfo = new StaminaInfo(3, TimeManager.SystemNow.Ticks, TimeManager.Second2Tick(300L), 5, null)
		};
		for (int i = 0; i < 5; i++)
		{
			pvpDynamicData.enemyInfoList.Add(PvpDynamicData.EnemyInfo.MakeDummy(i + 1));
		}
		pvpDynamicData.isHappenDefenseBonus = true;
		pvpDynamicData.isHappenReset = true;
		pvpDynamicData.pvpRankBeforReset = 3;
		pvpDynamicData.defenseResultList = new List<PvpDynamicData.DefenseResult>
		{
			new PvpDynamicData.DefenseResult
			{
				winNum = 1,
				loseNum = 2,
				bonusItemList = new List<ItemData>
				{
					new ItemData(30103, 10)
				},
				time = TimeManager.Now
			},
			new PvpDynamicData.DefenseResult
			{
				winNum = 11,
				loseNum = 20,
				bonusItemList = new List<ItemData>
				{
					new ItemData(30103, 50)
				},
				time = TimeManager.Now
			}
		};
		pvpDynamicData.pvpSeasonIdBeforReset = 1;
		return pvpDynamicData;
	}

	// Token: 0x04000623 RID: 1571
	public int seasonId;

	// Token: 0x04000624 RID: 1572
	public PvpDynamicData.UserInfo userInfo;

	// Token: 0x04000625 RID: 1573
	public List<PvpDynamicData.EnemyInfo> enemyInfoList = new List<PvpDynamicData.EnemyInfo>();

	// Token: 0x04000626 RID: 1574
	private bool isHappenReset;

	// Token: 0x04000627 RID: 1575
	private bool isHappenDefenseBonus;

	// Token: 0x04000628 RID: 1576
	private List<PvpDynamicData.DefenseResult> defenseResultList;

	// Token: 0x04000629 RID: 1577
	private int pvpRankBeforReset;

	// Token: 0x0400062A RID: 1578
	private int pvpSeasonIdBeforReset;

	// Token: 0x02000746 RID: 1862
	public class UserInfo
	{
		// Token: 0x040032AD RID: 12973
		public int pvpPoint;

		// Token: 0x040032AE RID: 12974
		public int currentDeckId;

		// Token: 0x040032AF RID: 12975
		public StaminaInfo pvpStaminaInfo;

		// Token: 0x040032B0 RID: 12976
		public int winningNum;
	}

	// Token: 0x02000747 RID: 1863
	public class DefenseResult
	{
		// Token: 0x040032B1 RID: 12977
		public DateTime time;

		// Token: 0x040032B2 RID: 12978
		public int winNum;

		// Token: 0x040032B3 RID: 12979
		public int loseNum;

		// Token: 0x040032B4 RID: 12980
		public List<ItemData> bonusItemList;
	}

	// Token: 0x02000748 RID: 1864
	public class EnemyInfo
	{
		// Token: 0x060035B1 RID: 13745 RVA: 0x001C5341 File Offset: 0x001C3541
		public EnemyInfo()
		{
		}

		// Token: 0x060035B2 RID: 13746 RVA: 0x001C534C File Offset: 0x001C354C
		public EnemyInfo(OppUser server)
		{
			this.oppUser = server;
			this.difficulty = (PvpDynamicData.EnemyInfo.Difficulty)server.difficulty;
			this.friendId = server.friend_id;
			this.userName = server.user_name;
			this.userLevel = server.player_rank;
			this.achievementId = server.achievement_id;
			this.deckInfo = new SceneBattle_DeckInfo(server);
			this.kemoBoardParamMap = new Dictionary<CharaDef.AttributeType, DataManagerKemoBoard.KemoBoardBonusParam>
			{
				{
					CharaDef.AttributeType.RED,
					new DataManagerKemoBoard.KemoBoardBonusParam(CharaDef.AttributeType.RED)
				},
				{
					CharaDef.AttributeType.GREEN,
					new DataManagerKemoBoard.KemoBoardBonusParam(CharaDef.AttributeType.GREEN)
				},
				{
					CharaDef.AttributeType.BLUE,
					new DataManagerKemoBoard.KemoBoardBonusParam(CharaDef.AttributeType.BLUE)
				},
				{
					CharaDef.AttributeType.PINK,
					new DataManagerKemoBoard.KemoBoardBonusParam(CharaDef.AttributeType.PINK)
				},
				{
					CharaDef.AttributeType.LIME,
					new DataManagerKemoBoard.KemoBoardBonusParam(CharaDef.AttributeType.LIME)
				},
				{
					CharaDef.AttributeType.AQUA,
					new DataManagerKemoBoard.KemoBoardBonusParam(CharaDef.AttributeType.AQUA)
				}
			};
			this.kizunaBuffQualified = server.kizuna_buff_qualified;
		}

		// Token: 0x060035B3 RID: 13747 RVA: 0x001C5414 File Offset: 0x001C3614
		public static PvpDynamicData.EnemyInfo MakeDummy(int id)
		{
			PvpDynamicData.EnemyInfo enemyInfo = new PvpDynamicData.EnemyInfo();
			enemyInfo.userName = "ダミー" + id.ToString();
			enemyInfo.userLevel = id + 1;
			enemyInfo.difficulty = id % 3 + PvpDynamicData.EnemyInfo.Difficulty.HARD;
			DebugParty debugParty = AssetManager.GetAssetData("Charas/Parameter/DebugParty/PvpData/DebugParty_pvp_enemy_" + (id % 3).ToString() + "_1") as DebugParty;
			enemyInfo.deckInfo = new SceneBattle_DeckInfo(debugParty);
			enemyInfo.oppUser = new OppUser();
			enemyInfo.oppUser.friend_id = id;
			return enemyInfo;
		}

		// Token: 0x040032B5 RID: 12981
		public OppUser oppUser;

		// Token: 0x040032B6 RID: 12982
		public PvpDynamicData.EnemyInfo.Difficulty difficulty;

		// Token: 0x040032B7 RID: 12983
		public int friendId;

		// Token: 0x040032B8 RID: 12984
		public string userName;

		// Token: 0x040032B9 RID: 12985
		public int userLevel;

		// Token: 0x040032BA RID: 12986
		public int achievementId;

		// Token: 0x040032BB RID: 12987
		public int kizunaBuffQualified;

		// Token: 0x040032BC RID: 12988
		public SceneBattle_DeckInfo deckInfo;

		// Token: 0x040032BD RID: 12989
		public Dictionary<CharaDef.AttributeType, DataManagerKemoBoard.KemoBoardBonusParam> kemoBoardParamMap;

		// Token: 0x02001141 RID: 4417
		public enum Difficulty
		{
			// Token: 0x04005EC6 RID: 24262
			INVALID,
			// Token: 0x04005EC7 RID: 24263
			HARD,
			// Token: 0x04005EC8 RID: 24264
			NORMAL,
			// Token: 0x04005EC9 RID: 24265
			EASY,
			// Token: 0x04005ECA RID: 24266
			CHAMPION
		}
	}
}
