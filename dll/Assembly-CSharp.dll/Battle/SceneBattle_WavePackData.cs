using System;
using System.Collections.Generic;

namespace Battle
{
	public class SceneBattle_WavePackData
	{
		public string bgmName;

		public string victoryBgmName;

		public List<EnemyDynamicData> enemyList = new List<EnemyDynamicData>();

		public SceneBattle_DeckInfo vsFriends;

		public List<ItemData> dropItemList = new List<ItemData>();

		public int infoId;
	}
}
