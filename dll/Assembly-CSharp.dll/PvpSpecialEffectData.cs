using System;
using SGNFW.Mst;

// Token: 0x0200009F RID: 159
public class PvpSpecialEffectData : IComparable<PvpSpecialEffectData>
{
	// Token: 0x17000143 RID: 323
	// (get) Token: 0x060006B9 RID: 1721 RVA: 0x0002D8BD File Offset: 0x0002BABD
	public int Id
	{
		get
		{
			return this.mstData.id;
		}
	}

	// Token: 0x17000144 RID: 324
	// (get) Token: 0x060006BA RID: 1722 RVA: 0x0002D8CA File Offset: 0x0002BACA
	public string ScenarioName
	{
		get
		{
			return this.mstData.scenarioName;
		}
	}

	// Token: 0x17000145 RID: 325
	// (get) Token: 0x060006BB RID: 1723 RVA: 0x0002D8D7 File Offset: 0x0002BAD7
	public int ScenarioQuestOneId
	{
		get
		{
			return this.mstData.scenarioId;
		}
	}

	// Token: 0x17000146 RID: 326
	// (get) Token: 0x060006BC RID: 1724 RVA: 0x0002D8E4 File Offset: 0x0002BAE4
	public string VoiceType
	{
		get
		{
			return this.mstData.voiceName;
		}
	}

	// Token: 0x060006BD RID: 1725 RVA: 0x0002D8F4 File Offset: 0x0002BAF4
	public int CompareTo(PvpSpecialEffectData data)
	{
		return this.Id.CompareTo(data.Id);
	}

	// Token: 0x060006BE RID: 1726 RVA: 0x0002D915 File Offset: 0x0002BB15
	public PvpSpecialEffectData()
	{
	}

	// Token: 0x060006BF RID: 1727 RVA: 0x0002D91D File Offset: 0x0002BB1D
	public PvpSpecialEffectData(MstPvpspecialData mstData)
	{
		this.mstData = mstData;
	}

	// Token: 0x0400062B RID: 1579
	private MstPvpspecialData mstData;
}
