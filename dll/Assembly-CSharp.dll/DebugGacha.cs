using System;
using UnityEngine;

// Token: 0x0200013F RID: 319
public class DebugGacha : MonoBehaviour
{
	// Token: 0x04000EC5 RID: 3781
	public bool newComer;

	// Token: 0x04000EC6 RID: 3782
	public bool debugAuth;

	// Token: 0x04000EC7 RID: 3783
	public AuthPlayer.GachaParam.SkyType skyType = AuthPlayer.GachaParam.SkyType.NORMAL;

	// Token: 0x04000EC8 RID: 3784
	public AuthPlayer.GachaParam.PutType putType = AuthPlayer.GachaParam.PutType.NORMAL;

	// Token: 0x04000EC9 RID: 3785
	public AuthPlayer.GachaParam.PostActType postType = AuthPlayer.GachaParam.PostActType.NORMAL;

	// Token: 0x04000ECA RID: 3786
	public AuthPlayer.GachaParam.EffectType effType = AuthPlayer.GachaParam.EffectType.BLUE;

	// Token: 0x04000ECB RID: 3787
	public ItemDef.Rarity rarityFor2D = ItemDef.Rarity.STAR1;

	// Token: 0x04000ECC RID: 3788
	public bool promotion;

	// Token: 0x04000ECD RID: 3789
	public bool forceStaging;

	// Token: 0x04000ECE RID: 3790
	public int itemId;

	// Token: 0x04000ECF RID: 3791
	public bool debugLoading;
}
