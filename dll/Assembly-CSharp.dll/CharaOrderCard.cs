using System;

[Serializable]
public class CharaOrderCard
{
	public CharaOrderCard()
	{
	}

	public CharaOrderCard(CharaDef.OrderCardType t, int p, int mp, int plasm)
	{
		this.type = t;
		this.param = p;
		this.spParamMp = ((this.type == CharaDef.OrderCardType.SPECIAL) ? mp : 0);
		this.spParamPlasm = ((this.type == CharaDef.OrderCardType.SPECIAL) ? plasm : 0);
	}

	public CharaDef.OrderCardType type;

	public int param;

	public int spParamMp;

	public int spParamPlasm;
}
