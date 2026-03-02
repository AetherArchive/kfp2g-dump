using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200011A RID: 282
public class ScenarioParty : ScriptableObject
{
	// Token: 0x06000D75 RID: 3445 RVA: 0x00059F40 File Offset: 0x00058140
	public ScenarioParty(List<ScenarioParty.Friends> friendsList)
	{
		this.friends = friendsList.ToArray();
	}

	// Token: 0x04000B47 RID: 2887
	public ScenarioParty.Friends[] friends = new ScenarioParty.Friends[1];

	// Token: 0x02000881 RID: 2177
	[Serializable]
	public class Photo
	{
		// Token: 0x04003944 RID: 14660
		public int id;

		// Token: 0x04003945 RID: 14661
		public int level;

		// Token: 0x04003946 RID: 14662
		public int limit;
	}

	// Token: 0x02000882 RID: 2178
	[Serializable]
	public class Friends
	{
		// Token: 0x04003947 RID: 14663
		public int id;

		// Token: 0x04003948 RID: 14664
		public int clothItem;

		// Token: 0x04003949 RID: 14665
		public int rank;

		// Token: 0x0400394A RID: 14666
		public int level;

		// Token: 0x0400394B RID: 14667
		public int kizunaLevel;

		// Token: 0x0400394C RID: 14668
		public int yasei;

		// Token: 0x0400394D RID: 14669
		public int miracleLevel;

		// Token: 0x0400394E RID: 14670
		public bool miracleMax;

		// Token: 0x0400394F RID: 14671
		public int dropItemId;

		// Token: 0x04003950 RID: 14672
		public ScenarioParty.Photo[] photo = new ScenarioParty.Photo[4];

		// Token: 0x04003951 RID: 14673
		public bool nanairoAbilityReleaseFlag;
	}
}
