using System;
using System.Collections.Generic;
using SGNFW.Mst;

public class CharaContactStatic : ItemStaticBase
{
	public int CharaId
	{
		get
		{
			return this.mstCharaMotionItemData.charaId;
		}
	}

	public bool IsNotHaveDisp
	{
		get
		{
			return this.mstCharaMotionItemData.notHaveDisp != 0;
		}
	}

	public bool IsDefaultItem
	{
		get
		{
			return this.mstCharaMotionItemData.defaultFlag != 0;
		}
	}

	public int SortPriority
	{
		get
		{
			return this.mstCharaMotionItemData.priority;
		}
	}

	public CharaContactStatic.Situation SituationType
	{
		get
		{
			return (CharaContactStatic.Situation)this.mstCharaMotionItemData.situationType;
		}
	}

	public string MotionKey
	{
		get
		{
			return this.mstCharaMotionItemData.motionKey;
		}
	}

	public string FacePackId
	{
		get
		{
			return this.mstCharaMotionItemData.facePackId;
		}
	}

	public List<string> VoiceKeyList { get; private set; }

	public override int GetId()
	{
		return this.mstCharaMotionItemData.id;
	}

	public override ItemDef.Kind GetKind()
	{
		return ItemDef.Kind.CHARA_CONTACT;
	}

	public override string GetName()
	{
		return this.mstCharaMotionItemData.name;
	}

	public override string GetInfo()
	{
		return this.mstCharaMotionItemData.flavorText;
	}

	public override ItemDef.Rarity GetRarity()
	{
		return (ItemDef.Rarity)this.mstCharaMotionItemData.rarity;
	}

	public override int GetStackMax()
	{
		return this.mstCharaMotionItemData.stackMax;
	}

	public override string GetIconName()
	{
		return "Texture2D/Icon_Fureai/" + this.mstCharaMotionItemData.iconName;
	}

	public override int GetSalePrice()
	{
		return 0;
	}

	public CharaContactStatic(MstCharaMotionItemData mst)
	{
		this.mstCharaMotionItemData = mst;
		this.VoiceKeyList = new List<string> { mst.voiceKey01, mst.voiceKey02, mst.voiceKey03, mst.voiceKey04, mst.voiceKey05 };
		this.VoiceKeyList.RemoveAll((string item) => string.IsNullOrEmpty(item));
	}

	private MstCharaMotionItemData mstCharaMotionItemData;

	public enum Situation
	{
		INVALID,
		STAND_NEGLECT,
		STAND_TAP,
		STAND_STROKING,
		SLEEP_TAP,
		SITDOWN_TAP,
		STAND_OUT_TAP
	}
}
