using System;
using SGNFW.Mst;

public class PvpSpecialEffectData : IComparable<PvpSpecialEffectData>
{
	public int Id
	{
		get
		{
			return this.mstData.id;
		}
	}

	public string ScenarioName
	{
		get
		{
			return this.mstData.scenarioName;
		}
	}

	public int ScenarioQuestOneId
	{
		get
		{
			return this.mstData.scenarioId;
		}
	}

	public string VoiceType
	{
		get
		{
			return this.mstData.voiceName;
		}
	}

	public int CompareTo(PvpSpecialEffectData data)
	{
		return this.Id.CompareTo(data.Id);
	}

	public PvpSpecialEffectData()
	{
	}

	public PvpSpecialEffectData(MstPvpspecialData mstData)
	{
		this.mstData = mstData;
	}

	private MstPvpspecialData mstData;
}
