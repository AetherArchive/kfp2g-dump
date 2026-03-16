using System;
using UnityEngine;

public class DebugGacha : MonoBehaviour
{
	public bool newComer;

	public bool debugAuth;

	public AuthPlayer.GachaParam.SkyType skyType = AuthPlayer.GachaParam.SkyType.NORMAL;

	public AuthPlayer.GachaParam.PutType putType = AuthPlayer.GachaParam.PutType.NORMAL;

	public AuthPlayer.GachaParam.PostActType postType = AuthPlayer.GachaParam.PostActType.NORMAL;

	public AuthPlayer.GachaParam.EffectType effType = AuthPlayer.GachaParam.EffectType.BLUE;

	public ItemDef.Rarity rarityFor2D = ItemDef.Rarity.STAR1;

	public bool promotion;

	public bool forceStaging;

	public int itemId;

	public bool debugLoading;
}
