using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000060 RID: 96
public class CharaStaticAbility : ScriptableObject
{
	// Token: 0x040003B2 RID: 946
	public string abilityName;

	// Token: 0x040003B3 RID: 947
	[Multiline(6)]
	public string abilityEffect;

	// Token: 0x040003B4 RID: 948
	public List<CharaBuffParamAbility> buffList;

	// Token: 0x040003B5 RID: 949
	public List<CharaGutsParamAbility> gutsList;
}
