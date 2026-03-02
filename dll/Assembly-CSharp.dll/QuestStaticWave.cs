using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000E7 RID: 231
public class QuestStaticWave : ScriptableObject
{
	// Token: 0x0400083A RID: 2106
	public List<QuestStaticWave.WaveStatic> waveList = new List<QuestStaticWave.WaveStatic>();

	// Token: 0x020007E5 RID: 2021
	[Serializable]
	public class WaveStatic
	{
		// Token: 0x17000824 RID: 2084
		// (get) Token: 0x0600376C RID: 14188 RVA: 0x001C8283 File Offset: 0x001C6483
		// (set) Token: 0x0600376D RID: 14189 RVA: 0x001C828B File Offset: 0x001C648B
		public int InfoId { get; set; }

		// Token: 0x04003524 RID: 13604
		public int id;

		// Token: 0x04003526 RID: 13606
		public int enemiesId;

		// Token: 0x04003527 RID: 13607
		public List<QuestStaticWave.EnemyData> enemyList;

		// Token: 0x04003528 RID: 13608
		public ScenarioParty vsFriendsList;

		// Token: 0x04003529 RID: 13609
		public string bgmName;

		// Token: 0x0400352A RID: 13610
		public string authName;

		// Token: 0x0400352B RID: 13611
		public string victoryBgmName;
	}

	// Token: 0x020007E6 RID: 2022
	[Serializable]
	public class EnemyData
	{
		// Token: 0x0400352C RID: 13612
		public int id;

		// Token: 0x0400352D RID: 13613
		public int charaId;

		// Token: 0x0400352E RID: 13614
		public int level;

		// Token: 0x0400352F RID: 13615
		public int hpratio;
	}
}
