using System;
using System.Collections.Generic;
using SGNFW.Mst;

// Token: 0x02000073 RID: 115
public class CharaContactStatic : ItemStaticBase
{
	// Token: 0x170000D0 RID: 208
	// (get) Token: 0x060003F8 RID: 1016 RVA: 0x0001BD69 File Offset: 0x00019F69
	public int CharaId
	{
		get
		{
			return this.mstCharaMotionItemData.charaId;
		}
	}

	// Token: 0x170000D1 RID: 209
	// (get) Token: 0x060003F9 RID: 1017 RVA: 0x0001BD76 File Offset: 0x00019F76
	public bool IsNotHaveDisp
	{
		get
		{
			return this.mstCharaMotionItemData.notHaveDisp != 0;
		}
	}

	// Token: 0x170000D2 RID: 210
	// (get) Token: 0x060003FA RID: 1018 RVA: 0x0001BD86 File Offset: 0x00019F86
	public bool IsDefaultItem
	{
		get
		{
			return this.mstCharaMotionItemData.defaultFlag != 0;
		}
	}

	// Token: 0x170000D3 RID: 211
	// (get) Token: 0x060003FB RID: 1019 RVA: 0x0001BD96 File Offset: 0x00019F96
	public int SortPriority
	{
		get
		{
			return this.mstCharaMotionItemData.priority;
		}
	}

	// Token: 0x170000D4 RID: 212
	// (get) Token: 0x060003FC RID: 1020 RVA: 0x0001BDA3 File Offset: 0x00019FA3
	public CharaContactStatic.Situation SituationType
	{
		get
		{
			return (CharaContactStatic.Situation)this.mstCharaMotionItemData.situationType;
		}
	}

	// Token: 0x170000D5 RID: 213
	// (get) Token: 0x060003FD RID: 1021 RVA: 0x0001BDB0 File Offset: 0x00019FB0
	public string MotionKey
	{
		get
		{
			return this.mstCharaMotionItemData.motionKey;
		}
	}

	// Token: 0x170000D6 RID: 214
	// (get) Token: 0x060003FE RID: 1022 RVA: 0x0001BDBD File Offset: 0x00019FBD
	public string FacePackId
	{
		get
		{
			return this.mstCharaMotionItemData.facePackId;
		}
	}

	// Token: 0x170000D7 RID: 215
	// (get) Token: 0x060003FF RID: 1023 RVA: 0x0001BDCA File Offset: 0x00019FCA
	// (set) Token: 0x06000400 RID: 1024 RVA: 0x0001BDD2 File Offset: 0x00019FD2
	public List<string> VoiceKeyList { get; private set; }

	// Token: 0x06000401 RID: 1025 RVA: 0x0001BDDB File Offset: 0x00019FDB
	public override int GetId()
	{
		return this.mstCharaMotionItemData.id;
	}

	// Token: 0x06000402 RID: 1026 RVA: 0x0001BDE8 File Offset: 0x00019FE8
	public override ItemDef.Kind GetKind()
	{
		return ItemDef.Kind.CHARA_CONTACT;
	}

	// Token: 0x06000403 RID: 1027 RVA: 0x0001BDEC File Offset: 0x00019FEC
	public override string GetName()
	{
		return this.mstCharaMotionItemData.name;
	}

	// Token: 0x06000404 RID: 1028 RVA: 0x0001BDF9 File Offset: 0x00019FF9
	public override string GetInfo()
	{
		return this.mstCharaMotionItemData.flavorText;
	}

	// Token: 0x06000405 RID: 1029 RVA: 0x0001BE06 File Offset: 0x0001A006
	public override ItemDef.Rarity GetRarity()
	{
		return (ItemDef.Rarity)this.mstCharaMotionItemData.rarity;
	}

	// Token: 0x06000406 RID: 1030 RVA: 0x0001BE13 File Offset: 0x0001A013
	public override int GetStackMax()
	{
		return this.mstCharaMotionItemData.stackMax;
	}

	// Token: 0x06000407 RID: 1031 RVA: 0x0001BE20 File Offset: 0x0001A020
	public override string GetIconName()
	{
		return "Texture2D/Icon_Fureai/" + this.mstCharaMotionItemData.iconName;
	}

	// Token: 0x06000408 RID: 1032 RVA: 0x0001BE37 File Offset: 0x0001A037
	public override int GetSalePrice()
	{
		return 0;
	}

	// Token: 0x06000409 RID: 1033 RVA: 0x0001BE3C File Offset: 0x0001A03C
	public CharaContactStatic(MstCharaMotionItemData mst)
	{
		this.mstCharaMotionItemData = mst;
		this.VoiceKeyList = new List<string> { mst.voiceKey01, mst.voiceKey02, mst.voiceKey03, mst.voiceKey04, mst.voiceKey05 };
		this.VoiceKeyList.RemoveAll((string item) => string.IsNullOrEmpty(item));
	}

	// Token: 0x040004DF RID: 1247
	private MstCharaMotionItemData mstCharaMotionItemData;

	// Token: 0x02000656 RID: 1622
	public enum Situation
	{
		// Token: 0x04002E84 RID: 11908
		INVALID,
		// Token: 0x04002E85 RID: 11909
		STAND_NEGLECT,
		// Token: 0x04002E86 RID: 11910
		STAND_TAP,
		// Token: 0x04002E87 RID: 11911
		STAND_STROKING,
		// Token: 0x04002E88 RID: 11912
		SLEEP_TAP,
		// Token: 0x04002E89 RID: 11913
		SITDOWN_TAP,
		// Token: 0x04002E8A RID: 11914
		STAND_OUT_TAP
	}
}
