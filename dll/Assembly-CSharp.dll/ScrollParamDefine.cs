using System;
using SGNFW.Common;

public class ScrollParamDefine : Singleton<ScrollParamDefine>
{
	public static float BaseSensivility { get; set; } = 42f;

	public static float ShopTop { get; set; } = 42f;

	public static float GachaInfo { get; set; } = 42f;

	public static float GachaBoxInfo { get; set; } = 42f;

	public static float Quest { get; set; } = 42f;

	public static float BattleCharaInfo { get; set; } = 42f;

	public static float CharaWindow { get; set; } = 42f;

	public static float HelpWindow { get; set; } = 42f;

	public static float PhotoFlavor { get; set; } = 42f;

	public static float MonthlyPackWindow { get; set; } = -42f;

	public static float BaseHandleRange = -2f;

	public static int HandleAdditionalFactor = -8;
}
