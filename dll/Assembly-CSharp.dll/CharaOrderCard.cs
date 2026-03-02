using System;

// Token: 0x0200004E RID: 78
[Serializable]
public class CharaOrderCard
{
	// Token: 0x0600023C RID: 572 RVA: 0x00013F07 File Offset: 0x00012107
	public CharaOrderCard()
	{
	}

	// Token: 0x0600023D RID: 573 RVA: 0x00013F0F File Offset: 0x0001210F
	public CharaOrderCard(CharaDef.OrderCardType t, int p, int mp, int plasm)
	{
		this.type = t;
		this.param = p;
		this.spParamMp = ((this.type == CharaDef.OrderCardType.SPECIAL) ? mp : 0);
		this.spParamPlasm = ((this.type == CharaDef.OrderCardType.SPECIAL) ? plasm : 0);
	}

	// Token: 0x0400029C RID: 668
	public CharaDef.OrderCardType type;

	// Token: 0x0400029D RID: 669
	public int param;

	// Token: 0x0400029E RID: 670
	public int spParamMp;

	// Token: 0x0400029F RID: 671
	public int spParamPlasm;
}
