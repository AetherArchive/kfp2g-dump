using System;
using System.Collections.Generic;
using UnityEngine;

public class CharaStaticAbility : ScriptableObject
{
	public string abilityName;

	[Multiline(6)]
	public string abilityEffect;

	public List<CharaBuffParamAbility> buffList;

	public List<CharaGutsParamAbility> gutsList;
}
