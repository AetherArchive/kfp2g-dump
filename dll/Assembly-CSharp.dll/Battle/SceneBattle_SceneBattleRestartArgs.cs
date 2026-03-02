using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;
using UnityEngine;

namespace Battle
{
	// Token: 0x02000223 RID: 547
	public class SceneBattle_SceneBattleRestartArgs
	{
		// Token: 0x0600230F RID: 8975 RVA: 0x0019573C File Offset: 0x0019393C
		public long getHash()
		{
			long num = this.version - this.restartHash;
			MstVersion mstVersion = MstManager.MstVersionList.Find((MstVersion itm) => itm.type == MstType.QUEST_WAVE_DATA.ToString());
			if (mstVersion != null)
			{
				foreach (char c in mstVersion.version)
				{
					if (c >= '0' && c <= '9')
					{
						num -= (long)(c - '0');
					}
				}
			}
			mstVersion = MstManager.MstVersionList.Find((MstVersion itm) => itm.type == MstType.QUEST_ENEMY_DATA.ToString());
			if (mstVersion != null)
			{
				foreach (char c2 in mstVersion.version)
				{
					if (c2 >= '0' && c2 <= '9')
					{
						num -= (long)(c2 - '0');
					}
				}
			}
			num -= (long)this.battleArgs.questOneId;
			num -= (long)this.battleArgs.eventId;
			num -= (long)this.battleArgs.selectDeckId;
			num -= (long)this.battleArgs.pvpSeasonId;
			num -= (long)this.battleArgs.attrIndex;
			if (this.wave > 0)
			{
				num -= (long)this.wave;
				num -= (long)this.turn;
				num -= (long)this.autoOrder;
				num -= (long)this.timeScale;
			}
			if (this.plyCard != null)
			{
				foreach (int num2 in this.plyCard)
				{
					num -= (long)num2;
				}
			}
			if (this.vssCard != null)
			{
				foreach (int num3 in this.vssCard)
				{
					num -= (long)num3;
				}
			}
			if (this.player != null)
			{
				foreach (SceneBattle_SceneBattleRestartArgs.Chara chara in this.player)
				{
					num -= (long)(chara.hp + chara.idx);
				}
			}
			if (this.enemy != null)
			{
				foreach (SceneBattle_SceneBattleRestartArgs.Chara chara2 in this.enemy)
				{
					num -= (long)(chara2.hp + chara2.idx);
				}
			}
			num -= (long)this.errPlayer;
			num -= (long)this.errEnemy;
			num -= (long)this.errVersus;
			num -= (long)this.battleArgs.tutorialSequence;
			return num;
		}

		// Token: 0x06002310 RID: 8976 RVA: 0x00195A14 File Offset: 0x00193C14
		public long getVersion()
		{
			long num = this.battleArgs.hash_id + this.restartHash;
			MstVersion mstVersion = MstManager.MstVersionList.Find((MstVersion itm) => itm.type == MstType.QUEST_WAVE_DATA.ToString());
			if (mstVersion != null)
			{
				foreach (char c in mstVersion.version)
				{
					if (c >= '0' && c <= '9')
					{
						num += (long)(c - '0');
					}
				}
			}
			mstVersion = MstManager.MstVersionList.Find((MstVersion itm) => itm.type == MstType.QUEST_ENEMY_DATA.ToString());
			if (mstVersion != null)
			{
				foreach (char c2 in mstVersion.version)
				{
					if (c2 >= '0' && c2 <= '9')
					{
						num += (long)(c2 - '0');
					}
				}
			}
			num += (long)this.battleArgs.questOneId;
			num += (long)this.battleArgs.eventId;
			num += (long)this.battleArgs.selectDeckId;
			num += (long)this.battleArgs.pvpSeasonId;
			num += (long)this.battleArgs.attrIndex;
			if (this.wave > 0)
			{
				num += (long)this.wave;
				num += (long)this.turn;
				num += (long)this.autoOrder;
				num += (long)this.timeScale;
			}
			if (this.plyCard != null)
			{
				foreach (int num2 in this.plyCard)
				{
					num += (long)num2;
				}
			}
			if (this.vssCard != null)
			{
				foreach (int num3 in this.vssCard)
				{
					num += (long)num3;
				}
			}
			if (this.player != null)
			{
				foreach (SceneBattle_SceneBattleRestartArgs.Chara chara in this.player)
				{
					num += (long)(chara.hp + chara.idx);
				}
			}
			if (this.enemy != null)
			{
				foreach (SceneBattle_SceneBattleRestartArgs.Chara chara2 in this.enemy)
				{
					num += (long)(chara2.hp + chara2.idx);
				}
			}
			num += (long)this.errPlayer;
			num += (long)this.errEnemy;
			num += (long)this.errVersus;
			return num;
		}

		// Token: 0x06002311 RID: 8977 RVA: 0x00195CE0 File Offset: 0x00193EE0
		public SceneBattle_SceneBattleRestartArgs(SceneBattleArgs args)
		{
			if (args == null)
			{
				this.battleArgs = new SceneBattleArgs();
				return;
			}
			this.battleArgs = args;
			this.version = this.getVersion();
			this.battleArgs.hash_id = 0L;
			if (this.battleArgs.tutorialSequence == TutorialUtil.Sequence.INVALID)
			{
				PlayerPrefs.SetString(this.battleRestartKey, PrjJson.ToJson(this));
				PlayerPrefs.Save();
			}
			this.battleArgs.hash_id = this.getHash();
		}

		// Token: 0x06002312 RID: 8978 RVA: 0x00195D6D File Offset: 0x00193F6D
		public SceneBattle_SceneBattleRestartArgs()
		{
		}

		// Token: 0x04001A37 RID: 6711
		private readonly string battleRestartKey = "battleRestart";

		// Token: 0x04001A38 RID: 6712
		private readonly long restartHash = 20201214L;

		// Token: 0x04001A39 RID: 6713
		public SceneBattleArgs battleArgs;

		// Token: 0x04001A3A RID: 6714
		public DataManagerQuest.BattleEndStatus battleEndStatus;

		// Token: 0x04001A3B RID: 6715
		public long version;

		// Token: 0x04001A3C RID: 6716
		public int autoOrder;

		// Token: 0x04001A3D RID: 6717
		public int timeScale;

		// Token: 0x04001A3E RID: 6718
		public int wave;

		// Token: 0x04001A3F RID: 6719
		public int turn;

		// Token: 0x04001A40 RID: 6720
		public int waveTurn;

		// Token: 0x04001A41 RID: 6721
		public int masterSkill;

		// Token: 0x04001A42 RID: 6722
		public int playerSkillTotalCount;

		// Token: 0x04001A43 RID: 6723
		public int wildPower;

		// Token: 0x04001A44 RID: 6724
		public int wildPowerVersus;

		// Token: 0x04001A45 RID: 6725
		public int addAction;

		// Token: 0x04001A46 RID: 6726
		public int addActionVersus;

		// Token: 0x04001A47 RID: 6727
		public int addActionMax;

		// Token: 0x04001A48 RID: 6728
		public int addActionVersusMax;

		// Token: 0x04001A49 RID: 6729
		public int lastDamageFriends;

		// Token: 0x04001A4A RID: 6730
		public int maxChain;

		// Token: 0x04001A4B RID: 6731
		public int maxChainBeat;

		// Token: 0x04001A4C RID: 6732
		public int maxChainAction;

		// Token: 0x04001A4D RID: 6733
		public int maxChainTry;

		// Token: 0x04001A4E RID: 6734
		public int chainExec;

		// Token: 0x04001A4F RID: 6735
		public int chainSumCnt;

		// Token: 0x04001A50 RID: 6736
		public List<CharaDef.ActionBuffType> statusError;

		// Token: 0x04001A51 RID: 6737
		public int okawari;

		// Token: 0x04001A52 RID: 6738
		public int artsCount;

		// Token: 0x04001A53 RID: 6739
		public int continueCount;

		// Token: 0x04001A54 RID: 6740
		public int maxDmg;

		// Token: 0x04001A55 RID: 6741
		public int trainingRevive;

		// Token: 0x04001A56 RID: 6742
		public long trainingScore;

		// Token: 0x04001A57 RID: 6743
		public List<int> trainingKillMobEnemies;

		// Token: 0x04001A58 RID: 6744
		public long pvpTrainingScorePlayer;

		// Token: 0x04001A59 RID: 6745
		public long pvpTrainingScoreVersus;

		// Token: 0x04001A5A RID: 6746
		public int giveupPlayer;

		// Token: 0x04001A5B RID: 6747
		public int giveupVersus;

		// Token: 0x04001A5C RID: 6748
		public int ticklingPlayer;

		// Token: 0x04001A5D RID: 6749
		public int ticklingVersus;

		// Token: 0x04001A5E RID: 6750
		public int plyTickleSuccessCount;

		// Token: 0x04001A5F RID: 6751
		public List<int> plyCard;

		// Token: 0x04001A60 RID: 6752
		public List<int> vssCard;

		// Token: 0x04001A61 RID: 6753
		public DateTime battleStartTime;

		// Token: 0x04001A62 RID: 6754
		public int target;

		// Token: 0x04001A63 RID: 6755
		public List<SceneBattle_SceneBattleRestartArgs.Chara> player;

		// Token: 0x04001A64 RID: 6756
		public List<SceneBattle_SceneBattleRestartArgs.Chara> enemy;

		// Token: 0x04001A65 RID: 6757
		public int errPlayer;

		// Token: 0x04001A66 RID: 6758
		public int errEnemy;

		// Token: 0x04001A67 RID: 6759
		public int errVersus;

		// Token: 0x0200105F RID: 4191
		public class Chara
		{
			// Token: 0x04005B8A RID: 23434
			public int idx;

			// Token: 0x04005B8B RID: 23435
			public int hp;

			// Token: 0x04005B8C RID: 23436
			public int kp;

			// Token: 0x04005B8D RID: 23437
			public List<int> reuse;

			// Token: 0x04005B8E RID: 23438
			public List<SceneBattle_Buff> buff;

			// Token: 0x04005B8F RID: 23439
			public int guts;

			// Token: 0x04005B90 RID: 23440
			public List<int> recover;

			// Token: 0x04005B91 RID: 23441
			public int actCount;

			// Token: 0x04005B92 RID: 23442
			public int inBack;

			// Token: 0x04005B93 RID: 23443
			public bool wait;

			// Token: 0x04005B94 RID: 23444
			public int waitCount;
		}
	}
}
