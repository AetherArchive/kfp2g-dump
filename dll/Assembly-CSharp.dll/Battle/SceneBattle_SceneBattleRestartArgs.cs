using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;
using UnityEngine;

namespace Battle
{
	public class SceneBattle_SceneBattleRestartArgs
	{
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

		public SceneBattle_SceneBattleRestartArgs()
		{
		}

		private readonly string battleRestartKey = "battleRestart";

		private readonly long restartHash = 20201214L;

		public SceneBattleArgs battleArgs;

		public DataManagerQuest.BattleEndStatus battleEndStatus;

		public long version;

		public int autoOrder;

		public int timeScale;

		public int wave;

		public int turn;

		public int waveTurn;

		public int masterSkill;

		public int playerSkillTotalCount;

		public int wildPower;

		public int wildPowerVersus;

		public int addAction;

		public int addActionVersus;

		public int addActionMax;

		public int addActionVersusMax;

		public int lastDamageFriends;

		public int maxChain;

		public int maxChainBeat;

		public int maxChainAction;

		public int maxChainTry;

		public int chainExec;

		public int chainSumCnt;

		public List<CharaDef.ActionBuffType> statusError;

		public int okawari;

		public int artsCount;

		public int continueCount;

		public int maxDmg;

		public int trainingRevive;

		public long trainingScore;

		public List<int> trainingKillMobEnemies;

		public long pvpTrainingScorePlayer;

		public long pvpTrainingScoreVersus;

		public int giveupPlayer;

		public int giveupVersus;

		public int ticklingPlayer;

		public int ticklingVersus;

		public int plyTickleSuccessCount;

		public List<int> plyCard;

		public List<int> vssCard;

		public DateTime battleStartTime;

		public int target;

		public List<SceneBattle_SceneBattleRestartArgs.Chara> player;

		public List<SceneBattle_SceneBattleRestartArgs.Chara> enemy;

		public int errPlayer;

		public int errEnemy;

		public int errVersus;

		public class Chara
		{
			public int idx;

			public int hp;

			public int kp;

			public List<int> reuse;

			public List<SceneBattle_Buff> buff;

			public int guts;

			public List<int> recover;

			public int actCount;

			public int inBack;

			public bool wait;

			public int waitCount;
		}
	}
}
