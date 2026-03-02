using System;
using System.Collections.Generic;
using AEAuth3;
using UnityEngine;
using UnityEngine.UI;

namespace Battle
{
	// Token: 0x0200020F RID: 527
	public class SceneBattle_Chara
	{
		// Token: 0x040018C6 RID: 6342
		public int idx;

		// Token: 0x040018C7 RID: 6343
		public int number;

		// Token: 0x040018C8 RID: 6344
		public int wave;

		// Token: 0x040018C9 RID: 6345
		public int charaId;

		// Token: 0x040018CA RID: 6346
		public string charaName;

		// Token: 0x040018CB RID: 6347
		public CharaModelHandle chara;

		// Token: 0x040018CC RID: 6348
		public Vector3 basPos;

		// Token: 0x040018CD RID: 6349
		public Vector3 pos;

		// Token: 0x040018CE RID: 6350
		public float offZ;

		// Token: 0x040018CF RID: 6351
		public CharaDef.Type type;

		// Token: 0x040018D0 RID: 6352
		public bool rare;

		// Token: 0x040018D1 RID: 6353
		public CharaDef.AttributeType attribute;

		// Token: 0x040018D2 RID: 6354
		public CharaDef.AttributeMask attributeMask;

		// Token: 0x040018D3 RID: 6355
		public CharaDef.AttributeType subAttribute;

		// Token: 0x040018D4 RID: 6356
		public CharaDef.AttributeMask subAttributeMask;

		// Token: 0x040018D5 RID: 6357
		public Dictionary<int, int> abilityBuff;

		// Token: 0x040018D6 RID: 6358
		public List<int> abilityBuffKey;

		// Token: 0x040018D7 RID: 6359
		public CharaStaticAction arts;

		// Token: 0x040018D8 RID: 6360
		public List<EnemyAttackData.Param> normalAttack;

		// Token: 0x040018D9 RID: 6361
		public CharaStaticAction specialAttack;

		// Token: 0x040018DA RID: 6362
		public CharaStaticAction specialFlagAttack;

		// Token: 0x040018DB RID: 6363
		public int level;

		// Token: 0x040018DC RID: 6364
		public int total;

		// Token: 0x040018DD RID: 6365
		public int nowHp;

		// Token: 0x040018DE RID: 6366
		public int maxHp;

		// Token: 0x040018DF RID: 6367
		public int dspHp;

		// Token: 0x040018E0 RID: 6368
		public string helHp;

		// Token: 0x040018E1 RID: 6369
		public int nowKp;

		// Token: 0x040018E2 RID: 6370
		public int maxKp;

		// Token: 0x040018E3 RID: 6371
		public int dspKp;

		// Token: 0x040018E4 RID: 6372
		public int atkPwr;

		// Token: 0x040018E5 RID: 6373
		public int defPwr;

		// Token: 0x040018E6 RID: 6374
		public int avoidRatio;

		// Token: 0x040018E7 RID: 6375
		public int actionDamageRatio;

		// Token: 0x040018E8 RID: 6376
		public int tryDamageRatio;

		// Token: 0x040018E9 RID: 6377
		public int beatDamageRatio;

		// Token: 0x040018EA RID: 6378
		public List<SceneBattle_Buff> buff;

		// Token: 0x040018EB RID: 6379
		public int newHp;

		// Token: 0x040018EC RID: 6380
		public int newKp;

		// Token: 0x040018ED RID: 6381
		public List<SceneBattle_Buff> newBuff;

		// Token: 0x040018EE RID: 6382
		public int newGuts;

		// Token: 0x040018EF RID: 6383
		public int guts;

		// Token: 0x040018F0 RID: 6384
		public List<int> recover;

		// Token: 0x040018F1 RID: 6385
		public List<int> newRecover;

		// Token: 0x040018F2 RID: 6386
		public int escapeHp;

		// Token: 0x040018F3 RID: 6387
		public Vector3 hpPos;

		// Token: 0x040018F4 RID: 6388
		public GameObject hpGage;

		// Token: 0x040018F5 RID: 6389
		public Image hpBar;

		// Token: 0x040018F6 RID: 6390
		public Image kpBar;

		// Token: 0x040018F7 RID: 6391
		public AEImage kpAE;

		// Token: 0x040018F8 RID: 6392
		public GameObject kpLock;

		// Token: 0x040018F9 RID: 6393
		public GameObject kpNoMP;

		// Token: 0x040018FA RID: 6394
		public GameObject kpDbl;

		// Token: 0x040018FB RID: 6395
		public Transform touchGage;

		// Token: 0x040018FC RID: 6396
		public GameObject tagMark;

		// Token: 0x040018FD RID: 6397
		public PguiReplaceAECtrl tagMarkAE;

		// Token: 0x040018FE RID: 6398
		public AEImage dmgAE;

		// Token: 0x040018FF RID: 6399
		public Transform statIcon;

		// Token: 0x04001900 RID: 6400
		public List<SceneBattle_InfoBuff> stat;

		// Token: 0x04001901 RID: 6401
		public int statIdx;

		// Token: 0x04001902 RID: 6402
		public float statTim;

		// Token: 0x04001903 RID: 6403
		public List<SceneBattle_InfoBuff> goodStat;

		// Token: 0x04001904 RID: 6404
		public float goodStatTim;

		// Token: 0x04001905 RID: 6405
		public List<SceneBattle_InfoBuff> badStat;

		// Token: 0x04001906 RID: 6406
		public float badStatTim;

		// Token: 0x04001907 RID: 6407
		public EffectData effStun;

		// Token: 0x04001908 RID: 6408
		public EffectData effPoison;

		// Token: 0x04001909 RID: 6409
		public EffectData effSleep;

		// Token: 0x0400190A RID: 6410
		public EffectData effIce;

		// Token: 0x0400190B RID: 6411
		public float width;

		// Token: 0x0400190C RID: 6412
		public float height;

		// Token: 0x0400190D RID: 6413
		public Vector3 abnormalHead;

		// Token: 0x0400190E RID: 6414
		public Vector3 abnormalRoot;

		// Token: 0x0400190F RID: 6415
		public int artsLevel;

		// Token: 0x04001910 RID: 6416
		public bool artsMax;

		// Token: 0x04001911 RID: 6417
		public int increaseKpByAttack;

		// Token: 0x04001912 RID: 6418
		public int increaseKpByDamage;

		// Token: 0x04001913 RID: 6419
		public bool bleed;

		// Token: 0x04001914 RID: 6420
		public bool burned;

		// Token: 0x04001915 RID: 6421
		public SceneBattle_Chara cover;

		// Token: 0x04001916 RID: 6422
		public int coverStep;

		// Token: 0x04001917 RID: 6423
		public Vector3 coverPos;

		// Token: 0x04001918 RID: 6424
		public int wildPoint;
	}
}
