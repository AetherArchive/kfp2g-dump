using System;

// Token: 0x0200005A RID: 90
[Serializable]
public class CharaGutsParamAbility
{
	// Token: 0x0400037C RID: 892
	[EnumFlags]
	public CharaDef.AttributeMask attributeMask = (CharaDef.AttributeMask)(-1);

	// Token: 0x0400037D RID: 893
	public bool invertAttributeMask;

	// Token: 0x0400037E RID: 894
	[EnumFlags]
	public CharaDef.AbilityTraits traitsTerrain = (CharaDef.AbilityTraits)(-1);

	// Token: 0x0400037F RID: 895
	public bool invertTraitsTerrainMask;

	// Token: 0x04000380 RID: 896
	[EnumFlags]
	public CharaDef.AbilityTraits2 traitsTimezone = (CharaDef.AbilityTraits2)(-1);

	// Token: 0x04000381 RID: 897
	public bool inverttraitsTimezoneMask;

	// Token: 0x04000382 RID: 898
	public CharaDef.ActionTargetType targetType;

	// Token: 0x04000383 RID: 899
	public bool breakElement;

	// Token: 0x04000384 RID: 900
	public int numOfTimes;
}
