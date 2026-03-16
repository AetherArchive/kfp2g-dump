using System;

[Serializable]
public class CharaGutsParamAbility
{
	[EnumFlags]
	public CharaDef.AttributeMask attributeMask = (CharaDef.AttributeMask)(-1);

	public bool invertAttributeMask;

	[EnumFlags]
	public CharaDef.AbilityTraits traitsTerrain = (CharaDef.AbilityTraits)(-1);

	public bool invertTraitsTerrainMask;

	[EnumFlags]
	public CharaDef.AbilityTraits2 traitsTimezone = (CharaDef.AbilityTraits2)(-1);

	public bool inverttraitsTimezoneMask;

	public CharaDef.ActionTargetType targetType;

	public bool breakElement;

	public int numOfTimes;
}
