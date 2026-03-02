using System;
using System.Collections.Generic;
using SGNFW.HttpRequest.Protocol;

// Token: 0x0200011C RID: 284
public class SceneBattleArgs
{
	// Token: 0x04000CC8 RID: 3272
	public long hash_id;

	// Token: 0x04000CC9 RID: 3273
	public TutorialUtil.Sequence tutorialSequence;

	// Token: 0x04000CCA RID: 3274
	public int questOneId;

	// Token: 0x04000CCB RID: 3275
	public List<int> waveEnemiesIdList;

	// Token: 0x04000CCC RID: 3276
	public List<DrewItem> dropItemList;

	// Token: 0x04000CCD RID: 3277
	public DateTime startTime;

	// Token: 0x04000CCE RID: 3278
	public int eventId;

	// Token: 0x04000CCF RID: 3279
	public int selectDeckId;

	// Token: 0x04000CD0 RID: 3280
	public Helper helper;

	// Token: 0x04000CD1 RID: 3281
	public int attrIndex;

	// Token: 0x04000CD2 RID: 3282
	public OppUser oppUser;

	// Token: 0x04000CD3 RID: 3283
	public PvpDynamicData.EnemyInfo.Difficulty difficulty;

	// Token: 0x04000CD4 RID: 3284
	public List<PrjUtil.ParamPreset> pvpBoard;

	// Token: 0x04000CD5 RID: 3285
	public int kizunaBuffQualified;

	// Token: 0x04000CD6 RID: 3286
	public bool pvp3x;

	// Token: 0x04000CD7 RID: 3287
	public int pvpTraining;

	// Token: 0x04000CD8 RID: 3288
	public bool isQuestNoClear;

	// Token: 0x04000CD9 RID: 3289
	public bool isSeasonReplacement;

	// Token: 0x04000CDA RID: 3290
	public int pvpSeasonId;

	// Token: 0x04000CDB RID: 3291
	public int trainingTurn;

	// Token: 0x04000CDC RID: 3292
	public DayOfWeek trainingDay;

	// Token: 0x04000CDD RID: 3293
	public int trainingSeasonId;

	// Token: 0x04000CDE RID: 3294
	public int trainingMission;

	// Token: 0x04000CDF RID: 3295
	public List<TrainingStaticData.DayOfWeekData.MissionBonus> trainingMissionList;

	// Token: 0x04000CE0 RID: 3296
	public int trainingHp;

	// Token: 0x04000CE1 RID: 3297
	public int trainingAtk;

	// Token: 0x04000CE2 RID: 3298
	public int trainingDef;

	// Token: 0x04000CE3 RID: 3299
	public long trainingScore;

	// Token: 0x04000CE4 RID: 3300
	public bool isPractice;

	// Token: 0x04000CE5 RID: 3301
	public SceneManager.SceneName resultNextSceneName;

	// Token: 0x04000CE6 RID: 3302
	public object resultNextSceneArgs;
}
