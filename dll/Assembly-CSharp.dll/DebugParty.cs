using System;
using UnityEngine;

// Token: 0x02000119 RID: 281
public class DebugParty : ScriptableObject
{
	// Token: 0x04000B3A RID: 2874
	public int masterSkill;

	// Token: 0x04000B3B RID: 2875
	public int masterSkillLevel;

	// Token: 0x04000B3C RID: 2876
	public int quest;

	// Token: 0x04000B3D RID: 2877
	public int waveId;

	// Token: 0x04000B3E RID: 2878
	public CharaDef.AiType aiType;

	// Token: 0x04000B3F RID: 2879
	public TutorialUtil.Sequence tutorial;

	// Token: 0x04000B40 RID: 2880
	public int training;

	// Token: 0x04000B41 RID: 2881
	public int trainingHp;

	// Token: 0x04000B42 RID: 2882
	public int trainingAtk;

	// Token: 0x04000B43 RID: 2883
	public int trainingDef;

	// Token: 0x04000B44 RID: 2884
	public int trainingMission;

	// Token: 0x04000B45 RID: 2885
	public DebugParty.TrainingMission[] trainingMissionList = new DebugParty.TrainingMission[2];

	// Token: 0x04000B46 RID: 2886
	public DebugParty.Friends[] friends = new DebugParty.Friends[5];

	// Token: 0x0200087D RID: 2173
	[Serializable]
	public class Photo
	{
		// Token: 0x04003932 RID: 14642
		public int id;

		// Token: 0x04003933 RID: 14643
		public int limit;

		// Token: 0x04003934 RID: 14644
		public int level;
	}

	// Token: 0x0200087E RID: 2174
	[Serializable]
	public class Accessory
	{
		// Token: 0x04003935 RID: 14645
		public int id;

		// Token: 0x04003936 RID: 14646
		public int level;
	}

	// Token: 0x0200087F RID: 2175
	[Serializable]
	public class Friends
	{
		// Token: 0x04003937 RID: 14647
		public int id;

		// Token: 0x04003938 RID: 14648
		public int cloth;

		// Token: 0x04003939 RID: 14649
		public int rank;

		// Token: 0x0400393A RID: 14650
		public int level;

		// Token: 0x0400393B RID: 14651
		public int kizuna;

		// Token: 0x0400393C RID: 14652
		public int yasei;

		// Token: 0x0400393D RID: 14653
		public int miracleLevel;

		// Token: 0x0400393E RID: 14654
		public bool miracleMax;

		// Token: 0x0400393F RID: 14655
		public DebugParty.Photo[] photo = new DebugParty.Photo[4];

		// Token: 0x04003940 RID: 14656
		public DebugParty.Accessory accessory;

		// Token: 0x04003941 RID: 14657
		public bool nanairoAbilityFlag;
	}

	// Token: 0x02000880 RID: 2176
	[Serializable]
	public class TrainingMission
	{
		// Token: 0x04003942 RID: 14658
		public TrainingStaticData.DayOfWeekData.MissionBonus.Type type;

		// Token: 0x04003943 RID: 14659
		public int val;
	}
}
