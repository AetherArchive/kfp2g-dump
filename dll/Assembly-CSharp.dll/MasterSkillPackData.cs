using System;

// Token: 0x02000066 RID: 102
public class MasterSkillPackData
{
	// Token: 0x1700007A RID: 122
	// (get) Token: 0x0600029F RID: 671 RVA: 0x00015821 File Offset: 0x00013A21
	// (set) Token: 0x060002A0 RID: 672 RVA: 0x00015829 File Offset: 0x00013A29
	public MasterSkillDynamicData dynamicData { get; private set; }

	// Token: 0x1700007B RID: 123
	// (get) Token: 0x060002A1 RID: 673 RVA: 0x00015832 File Offset: 0x00013A32
	// (set) Token: 0x060002A2 RID: 674 RVA: 0x0001583A File Offset: 0x00013A3A
	public MasterStaticSkill staticData { get; private set; }

	// Token: 0x060002A3 RID: 675 RVA: 0x00015843 File Offset: 0x00013A43
	public MasterSkillPackData(MasterSkillDynamicData dynamicData)
	{
		this.id = dynamicData.id;
		this.dynamicData = dynamicData;
		this.staticData = DataManager.DmChara.GetMasterSkillStaticData(this.id);
	}

	// Token: 0x060002A4 RID: 676 RVA: 0x00015874 File Offset: 0x00013A74
	public static MasterSkillPackData MakeDummy(int id)
	{
		return new MasterSkillPackData(new MasterSkillDynamicData
		{
			id = id,
			level = 1
		});
	}

	// Token: 0x04000435 RID: 1077
	public int id;
}
