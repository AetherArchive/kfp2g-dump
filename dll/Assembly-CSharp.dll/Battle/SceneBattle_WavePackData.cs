using System;
using System.Collections.Generic;

namespace Battle
{
	// Token: 0x02000225 RID: 549
	public class SceneBattle_WavePackData
	{
		// Token: 0x04001A72 RID: 6770
		public string bgmName;

		// Token: 0x04001A73 RID: 6771
		public string victoryBgmName;

		// Token: 0x04001A74 RID: 6772
		public List<EnemyDynamicData> enemyList = new List<EnemyDynamicData>();

		// Token: 0x04001A75 RID: 6773
		public SceneBattle_DeckInfo vsFriends;

		// Token: 0x04001A76 RID: 6774
		public List<ItemData> dropItemList = new List<ItemData>();

		// Token: 0x04001A77 RID: 6775
		public int infoId;
	}
}
