using System;
using UnityEngine;

public class DebugParty : ScriptableObject
{
	public int masterSkill;

	public int masterSkillLevel;

	public int quest;

	public int waveId;

	public CharaDef.AiType aiType;

	public TutorialUtil.Sequence tutorial;

	public int training;

	public int trainingHp;

	public int trainingAtk;

	public int trainingDef;

	public int trainingMission;

	public DebugParty.TrainingMission[] trainingMissionList = new DebugParty.TrainingMission[2];

	public DebugParty.Friends[] friends = new DebugParty.Friends[5];

	[Serializable]
	public class Photo
	{
		public int id;

		public int limit;

		public int level;
	}

	[Serializable]
	public class Accessory
	{
		public int id;

		public int level;
	}

	[Serializable]
	public class Friends
	{
		public int id;

		public int cloth;

		public int rank;

		public int level;

		public int kizuna;

		public int yasei;

		public int miracleLevel;

		public bool miracleMax;

		public DebugParty.Photo[] photo = new DebugParty.Photo[4];

		public DebugParty.Accessory accessory;

		public bool nanairoAbilityFlag;
	}

	[Serializable]
	public class TrainingMission
	{
		public TrainingStaticData.DayOfWeekData.MissionBonus.Type type;

		public int val;
	}
}
