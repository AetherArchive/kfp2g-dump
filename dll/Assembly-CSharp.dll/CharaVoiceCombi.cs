using System;
using SGNFW.Mst;

public class CharaVoiceCombi
{
	public CharaVoiceCombi(MstCharaVoiceComboData m)
	{
		this.mst = m;
	}

	public int firstCharaId
	{
		get
		{
			return this.mst.firstCharaId;
		}
	}

	public int secondCharaId
	{
		get
		{
			return this.mst.secondCharaId;
		}
	}

	public CharaVoiceCombi.SitType situationType
	{
		get
		{
			return (CharaVoiceCombi.SitType)this.mst.situationType;
		}
	}

	public string firstVoiceName
	{
		get
		{
			if (!(this.mst.firstVoiceId == string.Empty))
			{
				return SoundManager.CreateVoiceNameByCharaCombi(this.mst.firstCharaId, this.mst.firstVoiceId);
			}
			return null;
		}
	}

	public string secondVoiceName
	{
		get
		{
			if (!(this.mst.secondVoiceId == string.Empty))
			{
				return SoundManager.CreateVoiceNameByCharaCombi(this.mst.secondCharaId, this.mst.secondVoiceId);
			}
			return null;
		}
	}

	private MstCharaVoiceComboData mst;

	public enum SitType
	{
		INVALID,
		BATTLE_START,
		BATTLE_CLEAR,
		WAVE_CLEAR,
		CHARA_DEAD,
		CHARA_POISON,
		CHARA_ATTACK,
		CHARA_CHAIN,
		CHARA_ARTS_END
	}
}
