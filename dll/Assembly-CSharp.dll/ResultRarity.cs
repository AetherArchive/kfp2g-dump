using System;
using System.Collections.Generic;

// Token: 0x02000144 RID: 324
public abstract class ResultRarity
{
	// Token: 0x17000362 RID: 866
	// (get) Token: 0x060011C8 RID: 4552
	public abstract int[,] SKY_PER { get; }

	// Token: 0x17000363 RID: 867
	// (get) Token: 0x060011C9 RID: 4553
	public abstract int[,] POST_PER { get; }

	// Token: 0x17000364 RID: 868
	// (get) Token: 0x060011CA RID: 4554
	public abstract int[,] PUT_PER { get; }

	// Token: 0x17000365 RID: 869
	// (get) Token: 0x060011CB RID: 4555
	public abstract int[,] PUT_PER_V { get; }

	// Token: 0x17000366 RID: 870
	// (get) Token: 0x060011CC RID: 4556
	public abstract int[,] EFF_PER { get; }

	// Token: 0x060011CD RID: 4557
	public abstract int GetHighRarity(List<GachaAuthCtrl.AuthItem> authItemList);

	// Token: 0x060011CE RID: 4558
	public abstract int GetGachaResultRarity(GachaAuthCtrl.AuthItem authItem);

	// Token: 0x060011CF RID: 4559
	public abstract string LotteryPremiumSEName(List<GachaAuthCtrl.AuthItem> authItemList, List<string> seNameList);

	// Token: 0x060011D0 RID: 4560
	public abstract bool LotteryPromotion(GachaAuthCtrl.AuthItem authItem);

	// Token: 0x060011D1 RID: 4561
	public abstract AuthPlayer.GachaParam.EffectType GetEffectTypeForGachaParamAfter(GachaAuthCtrl.AuthItem authItem);
}
