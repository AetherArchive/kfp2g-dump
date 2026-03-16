using System;

namespace Battle
{
	public class SceneBattle_Buff
	{
		public SceneBattle_Buff clone()
		{
			return (SceneBattle_Buff)base.MemberwiseClone();
		}

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

		public CharaDef.ActionBuffType buffType;

		public long abnormalType;

		public CharaDef.AttributeMask spAttributeMask;

		public CharaDef.HealthMask spHealthMask;

		public CharaDef.EnemyMask spEnemyMask;

		public double coefficient;

		public int increment;

		public double rate;

		public int turn;

		public int count;

		public CharaDef.Type chrTyp;

		public int chrIdx;

		public int giveupNum;

		public int giveupCnt;

		public int maxSkillCount;

		public int scheduledTurn;

		public CharaBuffParam scheduledBuffParam;

		public CharaDef.ActionBuffType scheduledBuffType;
	}
}
