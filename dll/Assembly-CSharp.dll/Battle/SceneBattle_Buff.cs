using System;

namespace Battle
{
	// Token: 0x0200020D RID: 525
	public class SceneBattle_Buff
	{
		// Token: 0x06002231 RID: 8753 RVA: 0x00192102 File Offset: 0x00190302
		public SceneBattle_Buff clone()
		{
			return (SceneBattle_Buff)base.MemberwiseClone();
		}

		// Token: 0x06002232 RID: 8754 RVA: 0x00192110 File Offset: 0x00190310
		public void SetBuff(CharaBuffParamAbility cbpa, SceneBattle_Chara chara)
		{
			this.buffType = cbpa.buffType;
			this.abnormalType = cbpa.abTyp;
			this.spAttributeMask = cbpa.spAttributeMask;
			this.spHealthMask = cbpa.spHealthMask;
			this.spEnemyMask = cbpa.spEnemyMask;
			this.coefficient = (double)cbpa.coefficient;
			this.increment = cbpa.increment;
			this.rate = 1.0;
			if ((this.turn = cbpa.turn * 2) <= 0)
			{
				this.turn = 99999;
			}
			this.count = 0;
			this.chrTyp = chara.type;
			this.chrIdx = chara.idx;
			this.giveupNum = cbpa.giveupReuseNum;
			this.giveupCnt = 0;
			this.maxSkillCount = cbpa.maxCount;
		}

		// Token: 0x040018AB RID: 6315
		public CharaDef.ActionBuffType buffType;

		// Token: 0x040018AC RID: 6316
		public long abnormalType;

		// Token: 0x040018AD RID: 6317
		public CharaDef.AttributeMask spAttributeMask;

		// Token: 0x040018AE RID: 6318
		public CharaDef.HealthMask spHealthMask;

		// Token: 0x040018AF RID: 6319
		public CharaDef.EnemyMask spEnemyMask;

		// Token: 0x040018B0 RID: 6320
		public double coefficient;

		// Token: 0x040018B1 RID: 6321
		public int increment;

		// Token: 0x040018B2 RID: 6322
		public double rate;

		// Token: 0x040018B3 RID: 6323
		public int turn;

		// Token: 0x040018B4 RID: 6324
		public int count;

		// Token: 0x040018B5 RID: 6325
		public CharaDef.Type chrTyp;

		// Token: 0x040018B6 RID: 6326
		public int chrIdx;

		// Token: 0x040018B7 RID: 6327
		public int giveupNum;

		// Token: 0x040018B8 RID: 6328
		public int giveupCnt;

		// Token: 0x040018B9 RID: 6329
		public int maxSkillCount;

		// Token: 0x040018BA RID: 6330
		public int scheduledTurn;

		// Token: 0x040018BB RID: 6331
		public CharaBuffParam scheduledBuffParam;

		// Token: 0x040018BC RID: 6332
		public CharaDef.ActionBuffType scheduledBuffType;
	}
}
