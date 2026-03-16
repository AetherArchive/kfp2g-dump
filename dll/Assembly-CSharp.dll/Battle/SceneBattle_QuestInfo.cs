using System;
using System.Collections.Generic;
using SGNFW.HttpRequest.Protocol;

namespace Battle
{
	public class SceneBattle_QuestInfo
	{
		public List<SceneBattle_WavePackData> wavePackDataList
		{
			get
			{
				return this._wavePackDataList;
			}
		}

		public QuestOnePackData staticOneData
		{
			get
			{
				return this._staticOneData;
			}
		}

		public BattleMissionPack battleMissionPack
		{
			get
			{
				return this._battleMissionPack;
			}
		}

		public SceneBattle_QuestInfo()
		{
		}

		public SceneBattle_QuestInfo(int questOneId, List<int> waveEnemiesIdList, List<DrewItem> dropItemList)
		{
			List<DrewItem> list = null;
			if (dropItemList != null)
			{
				list = new List<DrewItem>(dropItemList);
			}
			this._staticOneData = DataManager.DmQuest.GetQuestOnePackData(questOneId);
			int num;
			if (waveEnemiesIdList == null || waveEnemiesIdList.Count <= 0)
			{
				waveEnemiesIdList = new List<int>();
				int j = 1;
				for (;;)
				{
					QuestStaticWave.WaveStatic waveStatic = this._staticOneData.questOne.waveData.waveList.Find((QuestStaticWave.WaveStatic itm) => itm.id == j);
					if (waveStatic == null)
					{
						break;
					}
					waveEnemiesIdList.Add(waveStatic.enemiesId);
					num = j;
					j = num + 1;
				}
			}
			this._wavePackDataList = new List<SceneBattle_WavePackData>();
			int i;
			Predicate<QuestStaticWave.WaveStatic> <>9__1;
			Predicate<QuestStaticWave.WaveStatic> <>9__2;
			Predicate<QuestStaticWave.WaveStatic> <>9__3;
			for (i = 0; i < waveEnemiesIdList.Count; i = num + 1)
			{
				List<QuestStaticWave.WaveStatic> waveList = this._staticOneData.questOne.waveData.waveList;
				Predicate<QuestStaticWave.WaveStatic> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = (QuestStaticWave.WaveStatic itm) => itm.id == i + 1 && itm.enemiesId == waveEnemiesIdList[i]);
				}
				QuestStaticWave.WaveStatic waveStatic2 = waveList.Find(predicate);
				if (waveStatic2 == null)
				{
					List<QuestStaticWave.WaveStatic> waveList2 = this._staticOneData.questOne.waveData.waveList;
					Predicate<QuestStaticWave.WaveStatic> predicate2;
					if ((predicate2 = <>9__2) == null)
					{
						predicate2 = (<>9__2 = (QuestStaticWave.WaveStatic itm) => itm.enemiesId == waveEnemiesIdList[i]);
					}
					waveStatic2 = waveList2.Find(predicate2);
				}
				if (waveStatic2 == null)
				{
					List<QuestStaticWave.WaveStatic> waveList3 = this._staticOneData.questOne.waveData.waveList;
					Predicate<QuestStaticWave.WaveStatic> predicate3;
					if ((predicate3 = <>9__3) == null)
					{
						predicate3 = (<>9__3 = (QuestStaticWave.WaveStatic itm) => itm.id == i + 1);
					}
					waveStatic2 = waveList3.Find(predicate3);
				}
				if (waveStatic2 == null)
				{
					if (this._staticOneData.questOne.waveData.waveList.Count <= 0)
					{
						break;
					}
					waveStatic2 = this._staticOneData.questOne.waveData.waveList[0];
				}
				SceneBattle_WavePackData sceneBattle_WavePackData = new SceneBattle_WavePackData();
				sceneBattle_WavePackData.bgmName = waveStatic2.bgmName;
				sceneBattle_WavePackData.victoryBgmName = waveStatic2.victoryBgmName;
				using (List<QuestStaticWave.EnemyData>.Enumerator enumerator = waveStatic2.enemyList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						QuestStaticWave.EnemyData enemy = enumerator.Current;
						sceneBattle_WavePackData.enemyList.Add(new EnemyDynamicData(enemy.charaId, enemy.level, enemy.hpratio));
						if (list != null)
						{
							DrewItem drewItem = list.Find((DrewItem item) => item.enemy_id == enemy.id);
							sceneBattle_WavePackData.dropItemList.Add((drewItem != null) ? new ItemData(drewItem.item_id, drewItem.item_num) : null);
							list.Remove(drewItem);
						}
						else
						{
							sceneBattle_WavePackData.dropItemList.Add(null);
						}
					}
				}
				sceneBattle_WavePackData.vsFriends = ((waveStatic2.vsFriendsList == null) ? null : new SceneBattle_DeckInfo(waveStatic2.vsFriendsList));
				sceneBattle_WavePackData.infoId = waveStatic2.InfoId;
				this._wavePackDataList.Add(sceneBattle_WavePackData);
				num = i;
			}
			this._battleMissionPack = DataManager.DmQuest.GetBattleMissionPack(questOneId);
		}

		public void MakeQuestSkipInfo(int questOneId, List<DrewItem> dropItemList)
		{
			if (dropItemList != null)
			{
				new List<DrewItem>(dropItemList);
			}
			this._staticOneData = DataManager.DmQuest.GetQuestOnePackData(questOneId);
			this._wavePackDataList = new List<SceneBattle_WavePackData>();
			SceneBattle_WavePackData sceneBattle_WavePackData = new SceneBattle_WavePackData();
			using (List<DrewItem>.Enumerator enumerator = dropItemList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DrewItem dropItem = enumerator.Current;
					if (dropItem != null)
					{
						ItemData itemData = sceneBattle_WavePackData.dropItemList.Find((ItemData item) => item.id == dropItem.item_id);
						if (itemData == null)
						{
							sceneBattle_WavePackData.dropItemList.Add(new ItemData(dropItem.item_id, dropItem.item_num));
						}
						else
						{
							itemData.skipNumSet(itemData.num + dropItem.item_num);
						}
					}
				}
			}
			this._wavePackDataList.Add(sceneBattle_WavePackData);
			this._battleMissionPack = DataManager.DmQuest.GetBattleMissionPack(questOneId);
		}

		public SceneBattle_QuestInfo MakeDebugQuestInfo(int questOneId, int debugWaveId)
		{
			SceneBattle_QuestInfo sceneBattle_QuestInfo = new SceneBattle_QuestInfo();
			sceneBattle_QuestInfo.SetStaticOneData(questOneId);
			if (debugWaveId == 0)
			{
				SceneBattle_WavePackData sceneBattle_WavePackData = new SceneBattle_WavePackData();
				sceneBattle_WavePackData.bgmName = "prd_bgm0011";
				sceneBattle_WavePackData.enemyList = new List<EnemyDynamicData>();
				sceneBattle_WavePackData.vsFriends = new SceneBattle_DeckInfo(AssetManager.GetAssetData("Quest/Parameter/QuestWave/ScenarioParty_Friends_9999") as ScenarioParty);
				sceneBattle_QuestInfo.wavePackDataList.Add(sceneBattle_WavePackData);
			}
			else
			{
				foreach (QuestStaticWave.WaveStatic waveStatic in (AssetManager.GetAssetData("Charas/Parameter/DebugQuestWave/ParamQuestWave_" + debugWaveId.ToString("00000")) as QuestStaticWave).waveList)
				{
					SceneBattle_WavePackData sceneBattle_WavePackData2 = new SceneBattle_WavePackData();
					sceneBattle_WavePackData2.bgmName = waveStatic.bgmName;
					sceneBattle_WavePackData2.victoryBgmName = waveStatic.victoryBgmName;
					foreach (QuestStaticWave.EnemyData enemyData in waveStatic.enemyList)
					{
						sceneBattle_WavePackData2.enemyList.Add(new EnemyDynamicData(enemyData.charaId, enemyData.level, enemyData.hpratio));
						sceneBattle_WavePackData2.dropItemList.Add(new ItemData(10001, 2));
					}
					sceneBattle_WavePackData2.vsFriends = null;
					sceneBattle_QuestInfo.wavePackDataList.Add(sceneBattle_WavePackData2);
				}
			}
			sceneBattle_QuestInfo.SetBattleMissionPack(questOneId);
			return sceneBattle_QuestInfo;
		}

		private void SetStaticOneData(int questOneId)
		{
			this._staticOneData = DataManager.DmQuest.GetQuestOnePackData(questOneId);
		}

		private void SetBattleMissionPack(int questOneId)
		{
			this._battleMissionPack = DataManager.DmQuest.GetBattleMissionPack(questOneId);
		}

		private List<SceneBattle_WavePackData> _wavePackDataList = new List<SceneBattle_WavePackData>();

		private QuestOnePackData _staticOneData = new QuestOnePackData();

		private BattleMissionPack _battleMissionPack = new BattleMissionPack();
	}
}
