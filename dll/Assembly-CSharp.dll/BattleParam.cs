using System;
using UnityEngine;

// Token: 0x02000118 RID: 280
public class BattleParam : ScriptableObject
{
	// Token: 0x04000B14 RID: 2836
	public float attributeGood;

	// Token: 0x04000B15 RID: 2837
	public float attributeBad;

	// Token: 0x04000B16 RID: 2838
	public float[] sequenceDamage;

	// Token: 0x04000B17 RID: 2839
	public float[] sequenceHeal;

	// Token: 0x04000B18 RID: 2840
	public float[] chainBeat;

	// Token: 0x04000B19 RID: 2841
	public int[] chainAction;

	// Token: 0x04000B1A RID: 2842
	public int[] chainTry;

	// Token: 0x04000B1B RID: 2843
	public int[] chainBowl;

	// Token: 0x04000B1C RID: 2844
	public float poisonFriends;

	// Token: 0x04000B1D RID: 2845
	public float poisonEnemy;

	// Token: 0x04000B1E RID: 2846
	public float poisonBoss;

	// Token: 0x04000B1F RID: 2847
	public float sleepFriends;

	// Token: 0x04000B20 RID: 2848
	public float sleepEnemy;

	// Token: 0x04000B21 RID: 2849
	public float sleepBoss;

	// Token: 0x04000B22 RID: 2850
	public int defenseBase;

	// Token: 0x04000B23 RID: 2851
	public int iceAvoidFriends;

	// Token: 0x04000B24 RID: 2852
	public int iceAvoidEnemy;

	// Token: 0x04000B25 RID: 2853
	public int iceAvoidBoss;

	// Token: 0x04000B26 RID: 2854
	public float iceRcvFriends;

	// Token: 0x04000B27 RID: 2855
	public float iceRcvEnemy;

	// Token: 0x04000B28 RID: 2856
	public float iceRcvBoss;

	// Token: 0x04000B29 RID: 2857
	public int iceCancelFriends;

	// Token: 0x04000B2A RID: 2858
	public int iceCancelEnemy;

	// Token: 0x04000B2B RID: 2859
	public int iceCancelBoss;

	// Token: 0x04000B2C RID: 2860
	public float bleedFriends;

	// Token: 0x04000B2D RID: 2861
	public float bleedEnemy;

	// Token: 0x04000B2E RID: 2862
	public float bleedBoss;

	// Token: 0x04000B2F RID: 2863
	public float burnedFriends;

	// Token: 0x04000B30 RID: 2864
	public float burnedEnemy;

	// Token: 0x04000B31 RID: 2865
	public float burnedBoss;

	// Token: 0x04000B32 RID: 2866
	public BattleParam.Tickling mySideTickling;

	// Token: 0x04000B33 RID: 2867
	public BattleParam.Tickling enemySideTickling;

	// Token: 0x04000B34 RID: 2868
	public int paralysisFriends;

	// Token: 0x04000B35 RID: 2869
	public int paralysisEnemy;

	// Token: 0x04000B36 RID: 2870
	public int paralysisBoss;

	// Token: 0x04000B37 RID: 2871
	public int focusFriends;

	// Token: 0x04000B38 RID: 2872
	public int focusEnemy;

	// Token: 0x04000B39 RID: 2873
	public int focusBoss;

	// Token: 0x0200087C RID: 2172
	[Serializable]
	public class Tickling
	{
		// Token: 0x0400392E RID: 14638
		public int incidenceRate;

		// Token: 0x0400392F RID: 14639
		public int successRate;

		// Token: 0x04003930 RID: 14640
		public int num;

		// Token: 0x04003931 RID: 14641
		public int mpDecreaseRate;
	}
}
