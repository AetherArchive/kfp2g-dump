using System;
using System.Collections.Generic;

public abstract class ResultRarity
{
	public abstract int[,] SKY_PER { get; }

	public abstract int[,] POST_PER { get; }

	public abstract int[,] PUT_PER { get; }

	public abstract int[,] PUT_PER_V { get; }

	public abstract int[,] EFF_PER { get; }

	public abstract int GetHighRarity(List<GachaAuthCtrl.AuthItem> authItemList);

	public abstract int GetGachaResultRarity(GachaAuthCtrl.AuthItem authItem);

	public abstract string LotteryPremiumSEName(List<GachaAuthCtrl.AuthItem> authItemList, List<string> seNameList);

	public abstract bool LotteryPromotion(GachaAuthCtrl.AuthItem authItem);

	public abstract AuthPlayer.GachaParam.EffectType GetEffectTypeForGachaParamAfter(GachaAuthCtrl.AuthItem authItem);
}
