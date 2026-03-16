using System;
using System.Collections.Generic;
using SGNFW.HttpRequest.Protocol;

public class SceneBattleArgs
{
	public long hash_id;

	public TutorialUtil.Sequence tutorialSequence;

	public int questOneId;

	public List<int> waveEnemiesIdList;

	public List<DrewItem> dropItemList;

	public DateTime startTime;

	public int eventId;

	public int selectDeckId;

	public Helper helper;

	public int attrIndex;

	public OppUser oppUser;

	public PvpDynamicData.EnemyInfo.Difficulty difficulty;

	public List<PrjUtil.ParamPreset> pvpBoard;

	public int kizunaBuffQualified;

	public bool pvp3x;

	public int pvpTraining;

	public bool isQuestNoClear;

	public bool isSeasonReplacement;

	public int pvpSeasonId;

	public int trainingTurn;

	public DayOfWeek trainingDay;

	public int trainingSeasonId;

	public int trainingMission;

	public List<TrainingStaticData.DayOfWeekData.MissionBonus> trainingMissionList;

	public int trainingHp;

	public int trainingAtk;

	public int trainingDef;

	public long trainingScore;

	public bool isPractice;

	public SceneManager.SceneName resultNextSceneName;

	public object resultNextSceneArgs;
}
