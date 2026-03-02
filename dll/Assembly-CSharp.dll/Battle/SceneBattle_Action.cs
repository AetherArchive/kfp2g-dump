using System;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
	// Token: 0x0200020C RID: 524
	public class SceneBattle_Action
	{
		// Token: 0x04001884 RID: 6276
		public SceneBattle_Chara chara;

		// Token: 0x04001885 RID: 6277
		public SceneBattle_CardInfo card;

		// Token: 0x04001886 RID: 6278
		public CharaStaticAction actParam;

		// Token: 0x04001887 RID: 6279
		public int arts;

		// Token: 0x04001888 RID: 6280
		public bool synergy;

		// Token: 0x04001889 RID: 6281
		public int chain;

		// Token: 0x0400188A RID: 6282
		public SceneBattle_Chara target;

		// Token: 0x0400188B RID: 6283
		public List<SceneBattle_Act> act;

		// Token: 0x0400188C RID: 6284
		public List<SceneBattle_Eff> eff;

		// Token: 0x0400188D RID: 6285
		public List<SceneBattle_Tag> total;

		// Token: 0x0400188E RID: 6286
		public float blwTim;

		// Token: 0x0400188F RID: 6287
		public Vector3 blwSpd;

		// Token: 0x04001890 RID: 6288
		public Vector3 blwPos;

		// Token: 0x04001891 RID: 6289
		public List<SceneBattle_Chara> blwChr;

		// Token: 0x04001892 RID: 6290
		public Vector3 pos;

		// Token: 0x04001893 RID: 6291
		public Vector3 tagPos;

		// Token: 0x04001894 RID: 6292
		public float tagSiz;

		// Token: 0x04001895 RID: 6293
		public SceneBattle.CMDSTEP step;

		// Token: 0x04001896 RID: 6294
		public float actTim;

		// Token: 0x04001897 RID: 6295
		public float lokTim;

		// Token: 0x04001898 RID: 6296
		public float dedTim;

		// Token: 0x04001899 RID: 6297
		public List<SceneBattle_Chara> dedChr;

		// Token: 0x0400189A RID: 6298
		public float chrTim;

		// Token: 0x0400189B RID: 6299
		public float quakeTim;

		// Token: 0x0400189C RID: 6300
		public SceneBattle_Missile camMssl;

		// Token: 0x0400189D RID: 6301
		public VOICE_TYPE voice;

		// Token: 0x0400189E RID: 6302
		public float voiceDelay;

		// Token: 0x0400189F RID: 6303
		public float retTim;

		// Token: 0x040018A0 RID: 6304
		public bool dmgkp;

		// Token: 0x040018A1 RID: 6305
		public Dictionary<SceneBattle_Chara, int> reflect;

		// Token: 0x040018A2 RID: 6306
		public int refFlg;

		// Token: 0x040018A3 RID: 6307
		public List<SceneBattle_Tag> refAbility;

		// Token: 0x040018A4 RID: 6308
		public List<SceneBattle_Tag> refTactics;

		// Token: 0x040018A5 RID: 6309
		public Dictionary<SceneBattle_Chara, int> perDamage;

		// Token: 0x040018A6 RID: 6310
		public List<SceneBattle_Tag> perAbility;

		// Token: 0x040018A7 RID: 6311
		public List<SceneBattle_Tag> perTactics;

		// Token: 0x040018A8 RID: 6312
		public List<KeyValuePair<SceneBattle_Tag, SceneBattle_Buff>> givBuf;

		// Token: 0x040018A9 RID: 6313
		public bool paralysis;

		// Token: 0x040018AA RID: 6314
		public bool silence;
	}
}
