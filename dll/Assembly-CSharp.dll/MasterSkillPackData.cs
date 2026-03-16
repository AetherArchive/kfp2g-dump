using System;

public class MasterSkillPackData
{
	public MasterSkillDynamicData dynamicData { get; private set; }

	public MasterStaticSkill staticData { get; private set; }

	public MasterSkillPackData(MasterSkillDynamicData dynamicData)
	{
		this.id = dynamicData.id;
		this.dynamicData = dynamicData;
		this.staticData = DataManager.DmChara.GetMasterSkillStaticData(this.id);
	}

	public static MasterSkillPackData MakeDummy(int id)
	{
		return new MasterSkillPackData(new MasterSkillDynamicData
		{
			id = id,
			level = 1
		});
	}

	public int id;
}
