using System;
using UnityEngine;

namespace Battle
{
	// Token: 0x02000216 RID: 534
	public class SceneBattle_Friends : SceneBattle_Chara
	{
		// Token: 0x04001950 RID: 6480
		public string iconName;

		// Token: 0x04001951 RID: 6481
		public string iconNameInf;

		// Token: 0x04001952 RID: 6482
		public Texture2D icon;

		// Token: 0x04001953 RID: 6483
		public Vector3 commandCameraPos;

		// Token: 0x04001954 RID: 6484
		public Vector3 commandCameraRot;

		// Token: 0x04001955 RID: 6485
		public Vector3 chainCameraPos;

		// Token: 0x04001956 RID: 6486
		public Vector3 chainCameraRot;

		// Token: 0x04001957 RID: 6487
		public CharaStaticWaitSkill waitAction;

		// Token: 0x04001958 RID: 6488
		public bool isWaitAct;

		// Token: 0x04001959 RID: 6489
		public SceneBattle_CardInfo card;

		// Token: 0x0400195A RID: 6490
		public int inBack;

		// Token: 0x0400195B RID: 6491
		public int nowBack;

		// Token: 0x0400195C RID: 6492
		public int waitCount;

		// Token: 0x0400195D RID: 6493
		public int entNum;

		// Token: 0x0400195E RID: 6494
		public float entEn;

		// Token: 0x0400195F RID: 6495
		public float entLp;

		// Token: 0x04001960 RID: 6496
		public string entEff;

		// Token: 0x04001961 RID: 6497
		public float dspAtr;

		// Token: 0x04001962 RID: 6498
		public EffectData effBarrier;

		// Token: 0x04001963 RID: 6499
		public EffectData effWait;

		// Token: 0x04001964 RID: 6500
		public float effWaitTime;

		// Token: 0x04001965 RID: 6501
		public PrjUtil.ParamPreset boardParam;

		// Token: 0x04001966 RID: 6502
		public int wildPower;

		// Token: 0x04001967 RID: 6503
		public int tfBeforeId;
	}
}
