using System;
using System.Collections.Generic;

public class CharaStaticData : ItemStaticBase
{
	public string cueSheetName
	{
		get
		{
			return "cv_" + this.baseData.assetId.ToString("0000");
		}
	}

	public int maxPromoteNum { get; private set; }

	public Dictionary<int, ItemInput> kizunaLevelBonusItemList { get; private set; } = new Dictionary<int, ItemInput>();

	public ItemInput GetKizunaLevelBonusItem(int tgtKizunaLevel)
	{
		return this.kizunaLevelBonusItemList.TryGetValueEx(tgtKizunaLevel, null);
	}

	public void SetMaxPromoteNumInternal(int mpn)
	{
		this.maxPromoteNum = mpn;
	}

	public string GetMiniIconName()
	{
		string text = "Texture2D/Icon_Chara/Chara_Mini/icon_chara_mini_";
		string text2 = this.baseData.assetId.ToString("0000");
		return text + text2;
	}

	public override int GetId()
	{
		return this.baseData.id;
	}

	public override ItemDef.Kind GetKind()
	{
		return ItemDef.Kind.CHARA;
	}

	public override string GetName()
	{
		return this.baseData.charaName;
	}

	public override string GetInfo()
	{
		return this.baseData.flavorText;
	}

	public override ItemDef.Rarity GetRarity()
	{
		return ItemDef.Rarity.STAR4;
	}

	public override int GetStackMax()
	{
		return 99999;
	}

	public override string GetIconName()
	{
		string text = "Texture2D/Icon_Chara/Chara/icon_chara_";
		string text2 = this.baseData.assetId.ToString("0000");
		return text + text2;
	}

	public override int GetSalePrice()
	{
		return 0;
	}

	public CharaStaticBase baseData;

	public CharaStaticAction artsData;

	public CharaStaticAction normalAttackData;

	public CharaStaticAction specialAttackData;

	public CharaStaticAction specialFlagAttackData;

	public CharaStaticWaitSkill waitActionData;

	public List<CharaStaticAbility> abilityData = new List<CharaStaticAbility>();

	public CharaStaticAbility spAbilityData;

	public CharaStaticAbility nanairoAbilityData;

	public List<CharaOrderCard> orderCardList;

	public List<CharaPromotePreset> promoteList;

	public const int MaxArtsLevelLimit = 5;
}
