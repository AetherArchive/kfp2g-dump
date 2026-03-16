using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestStaticWave : ScriptableObject
{
	public List<QuestStaticWave.WaveStatic> waveList = new List<QuestStaticWave.WaveStatic>();

	[Serializable]
	public class WaveStatic
	{
		public int InfoId { get; set; }

		public int id;

		public int enemiesId;

		public List<QuestStaticWave.EnemyData> enemyList;

		public ScenarioParty vsFriendsList;

		public string bgmName;

		public string authName;

		public string victoryBgmName;
	}

	[Serializable]
	public class EnemyData
	{
		public int id;

		public int charaId;

		public int level;

		public int hpratio;
	}
}
