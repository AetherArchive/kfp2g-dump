using System;
using System.Collections.Generic;

// Token: 0x02000052 RID: 82
public class EnemyStaticData : ItemStaticBase
{
	// Token: 0x0600024A RID: 586 RVA: 0x000140C7 File Offset: 0x000122C7
	public override int GetId()
	{
		return this.baseData.id;
	}

	// Token: 0x0600024B RID: 587 RVA: 0x000140D4 File Offset: 0x000122D4
	public override ItemDef.Kind GetKind()
	{
		return ItemDef.Kind.CHARA;
	}

	// Token: 0x0600024C RID: 588 RVA: 0x000140D7 File Offset: 0x000122D7
	public override string GetName()
	{
		return this.baseData.charaName;
	}

	// Token: 0x0600024D RID: 589 RVA: 0x000140E4 File Offset: 0x000122E4
	public override string GetInfo()
	{
		return string.Empty;
	}

	// Token: 0x0600024E RID: 590 RVA: 0x000140EB File Offset: 0x000122EB
	public override ItemDef.Rarity GetRarity()
	{
		return ItemDef.Rarity.INVALID;
	}

	// Token: 0x0600024F RID: 591 RVA: 0x000140EE File Offset: 0x000122EE
	public override int GetStackMax()
	{
		return 0;
	}

	// Token: 0x06000250 RID: 592 RVA: 0x000140F1 File Offset: 0x000122F1
	public override string GetIconName()
	{
		return "";
	}

	// Token: 0x06000251 RID: 593 RVA: 0x000140F8 File Offset: 0x000122F8
	public override int GetSalePrice()
	{
		return 0;
	}

	// Token: 0x040002B0 RID: 688
	public EnemyStaticBase baseData;

	// Token: 0x040002B1 RID: 689
	public List<EnemyStaticBase> partsData;

	// Token: 0x040002B2 RID: 690
	public CharaStaticAction artsData;

	// Token: 0x040002B3 RID: 691
	public EnemyAttackData normalAttackData;
}
