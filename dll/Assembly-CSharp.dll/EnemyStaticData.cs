using System;
using System.Collections.Generic;

public class EnemyStaticData : ItemStaticBase
{
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
		return string.Empty;
	}

	public override ItemDef.Rarity GetRarity()
	{
		return ItemDef.Rarity.INVALID;
	}

	public override int GetStackMax()
	{
		return 0;
	}

	public override string GetIconName()
	{
		return "";
	}

	public override int GetSalePrice()
	{
		return 0;
	}

	public EnemyStaticBase baseData;

	public List<EnemyStaticBase> partsData;

	public CharaStaticAction artsData;

	public EnemyAttackData normalAttackData;
}
