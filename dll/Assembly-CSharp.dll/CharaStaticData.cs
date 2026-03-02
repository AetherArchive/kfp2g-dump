using System;
using System.Collections.Generic;

// Token: 0x0200004D RID: 77
public class CharaStaticData : ItemStaticBase
{
	// Token: 0x17000056 RID: 86
	// (get) Token: 0x0600022B RID: 555 RVA: 0x00013DF8 File Offset: 0x00011FF8
	public string cueSheetName
	{
		get
		{
			return "cv_" + this.baseData.assetId.ToString("0000");
		}
	}

	// Token: 0x17000057 RID: 87
	// (get) Token: 0x0600022C RID: 556 RVA: 0x00013E19 File Offset: 0x00012019
	// (set) Token: 0x0600022D RID: 557 RVA: 0x00013E21 File Offset: 0x00012021
	public int maxPromoteNum { get; private set; }

	// Token: 0x17000058 RID: 88
	// (get) Token: 0x0600022E RID: 558 RVA: 0x00013E2A File Offset: 0x0001202A
	// (set) Token: 0x0600022F RID: 559 RVA: 0x00013E32 File Offset: 0x00012032
	public Dictionary<int, ItemInput> kizunaLevelBonusItemList { get; private set; } = new Dictionary<int, ItemInput>();

	// Token: 0x06000230 RID: 560 RVA: 0x00013E3B File Offset: 0x0001203B
	public ItemInput GetKizunaLevelBonusItem(int tgtKizunaLevel)
	{
		return this.kizunaLevelBonusItemList.TryGetValueEx(tgtKizunaLevel, null);
	}

	// Token: 0x06000231 RID: 561 RVA: 0x00013E4A File Offset: 0x0001204A
	public void SetMaxPromoteNumInternal(int mpn)
	{
		this.maxPromoteNum = mpn;
	}

	// Token: 0x06000232 RID: 562 RVA: 0x00013E54 File Offset: 0x00012054
	public string GetMiniIconName()
	{
		string text = "Texture2D/Icon_Chara/Chara_Mini/icon_chara_mini_";
		string text2 = this.baseData.assetId.ToString("0000");
		return text + text2;
	}

	// Token: 0x06000233 RID: 563 RVA: 0x00013E82 File Offset: 0x00012082
	public override int GetId()
	{
		return this.baseData.id;
	}

	// Token: 0x06000234 RID: 564 RVA: 0x00013E8F File Offset: 0x0001208F
	public override ItemDef.Kind GetKind()
	{
		return ItemDef.Kind.CHARA;
	}

	// Token: 0x06000235 RID: 565 RVA: 0x00013E92 File Offset: 0x00012092
	public override string GetName()
	{
		return this.baseData.charaName;
	}

	// Token: 0x06000236 RID: 566 RVA: 0x00013E9F File Offset: 0x0001209F
	public override string GetInfo()
	{
		return this.baseData.flavorText;
	}

	// Token: 0x06000237 RID: 567 RVA: 0x00013EAC File Offset: 0x000120AC
	public override ItemDef.Rarity GetRarity()
	{
		return ItemDef.Rarity.STAR4;
	}

	// Token: 0x06000238 RID: 568 RVA: 0x00013EAF File Offset: 0x000120AF
	public override int GetStackMax()
	{
		return 99999;
	}

	// Token: 0x06000239 RID: 569 RVA: 0x00013EB8 File Offset: 0x000120B8
	public override string GetIconName()
	{
		string text = "Texture2D/Icon_Chara/Chara/icon_chara_";
		string text2 = this.baseData.assetId.ToString("0000");
		return text + text2;
	}

	// Token: 0x0600023A RID: 570 RVA: 0x00013EE6 File Offset: 0x000120E6
	public override int GetSalePrice()
	{
		return 0;
	}

	// Token: 0x0400028E RID: 654
	public CharaStaticBase baseData;

	// Token: 0x0400028F RID: 655
	public CharaStaticAction artsData;

	// Token: 0x04000290 RID: 656
	public CharaStaticAction normalAttackData;

	// Token: 0x04000291 RID: 657
	public CharaStaticAction specialAttackData;

	// Token: 0x04000292 RID: 658
	public CharaStaticAction specialFlagAttackData;

	// Token: 0x04000293 RID: 659
	public CharaStaticWaitSkill waitActionData;

	// Token: 0x04000294 RID: 660
	public List<CharaStaticAbility> abilityData = new List<CharaStaticAbility>();

	// Token: 0x04000295 RID: 661
	public CharaStaticAbility spAbilityData;

	// Token: 0x04000296 RID: 662
	public CharaStaticAbility nanairoAbilityData;

	// Token: 0x04000297 RID: 663
	public List<CharaOrderCard> orderCardList;

	// Token: 0x04000298 RID: 664
	public List<CharaPromotePreset> promoteList;

	// Token: 0x0400029B RID: 667
	public const int MaxArtsLevelLimit = 5;
}
