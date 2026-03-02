using System;
using SGNFW.Mst;

// Token: 0x02000074 RID: 116
public class CharaVoiceCombi
{
	// Token: 0x0600040A RID: 1034 RVA: 0x0001BEC8 File Offset: 0x0001A0C8
	public CharaVoiceCombi(MstCharaVoiceComboData m)
	{
		this.mst = m;
	}

	// Token: 0x170000D8 RID: 216
	// (get) Token: 0x0600040B RID: 1035 RVA: 0x0001BED7 File Offset: 0x0001A0D7
	public int firstCharaId
	{
		get
		{
			return this.mst.firstCharaId;
		}
	}

	// Token: 0x170000D9 RID: 217
	// (get) Token: 0x0600040C RID: 1036 RVA: 0x0001BEE4 File Offset: 0x0001A0E4
	public int secondCharaId
	{
		get
		{
			return this.mst.secondCharaId;
		}
	}

	// Token: 0x170000DA RID: 218
	// (get) Token: 0x0600040D RID: 1037 RVA: 0x0001BEF1 File Offset: 0x0001A0F1
	public CharaVoiceCombi.SitType situationType
	{
		get
		{
			return (CharaVoiceCombi.SitType)this.mst.situationType;
		}
	}

	// Token: 0x170000DB RID: 219
	// (get) Token: 0x0600040E RID: 1038 RVA: 0x0001BEFE File Offset: 0x0001A0FE
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

	// Token: 0x170000DC RID: 220
	// (get) Token: 0x0600040F RID: 1039 RVA: 0x0001BF34 File Offset: 0x0001A134
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

	// Token: 0x040004E1 RID: 1249
	private MstCharaVoiceComboData mst;

	// Token: 0x02000658 RID: 1624
	public enum SitType
	{
		// Token: 0x04002E8E RID: 11918
		INVALID,
		// Token: 0x04002E8F RID: 11919
		BATTLE_START,
		// Token: 0x04002E90 RID: 11920
		BATTLE_CLEAR,
		// Token: 0x04002E91 RID: 11921
		WAVE_CLEAR,
		// Token: 0x04002E92 RID: 11922
		CHARA_DEAD,
		// Token: 0x04002E93 RID: 11923
		CHARA_POISON,
		// Token: 0x04002E94 RID: 11924
		CHARA_ATTACK,
		// Token: 0x04002E95 RID: 11925
		CHARA_CHAIN,
		// Token: 0x04002E96 RID: 11926
		CHARA_ARTS_END
	}
}
