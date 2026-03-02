using System;
using SGNFW.Common;

// Token: 0x02000102 RID: 258
public class ScrollParamDefine : Singleton<ScrollParamDefine>
{
	// Token: 0x17000315 RID: 789
	// (get) Token: 0x06000C6B RID: 3179 RVA: 0x0004C50F File Offset: 0x0004A70F
	// (set) Token: 0x06000C6C RID: 3180 RVA: 0x0004C516 File Offset: 0x0004A716
	public static float BaseSensivility { get; set; } = 42f;

	// Token: 0x17000316 RID: 790
	// (get) Token: 0x06000C6D RID: 3181 RVA: 0x0004C51E File Offset: 0x0004A71E
	// (set) Token: 0x06000C6E RID: 3182 RVA: 0x0004C525 File Offset: 0x0004A725
	public static float ShopTop { get; set; } = 42f;

	// Token: 0x17000317 RID: 791
	// (get) Token: 0x06000C6F RID: 3183 RVA: 0x0004C52D File Offset: 0x0004A72D
	// (set) Token: 0x06000C70 RID: 3184 RVA: 0x0004C534 File Offset: 0x0004A734
	public static float GachaInfo { get; set; } = 42f;

	// Token: 0x17000318 RID: 792
	// (get) Token: 0x06000C71 RID: 3185 RVA: 0x0004C53C File Offset: 0x0004A73C
	// (set) Token: 0x06000C72 RID: 3186 RVA: 0x0004C543 File Offset: 0x0004A743
	public static float GachaBoxInfo { get; set; } = 42f;

	// Token: 0x17000319 RID: 793
	// (get) Token: 0x06000C73 RID: 3187 RVA: 0x0004C54B File Offset: 0x0004A74B
	// (set) Token: 0x06000C74 RID: 3188 RVA: 0x0004C552 File Offset: 0x0004A752
	public static float Quest { get; set; } = 42f;

	// Token: 0x1700031A RID: 794
	// (get) Token: 0x06000C75 RID: 3189 RVA: 0x0004C55A File Offset: 0x0004A75A
	// (set) Token: 0x06000C76 RID: 3190 RVA: 0x0004C561 File Offset: 0x0004A761
	public static float BattleCharaInfo { get; set; } = 42f;

	// Token: 0x1700031B RID: 795
	// (get) Token: 0x06000C77 RID: 3191 RVA: 0x0004C569 File Offset: 0x0004A769
	// (set) Token: 0x06000C78 RID: 3192 RVA: 0x0004C570 File Offset: 0x0004A770
	public static float CharaWindow { get; set; } = 42f;

	// Token: 0x1700031C RID: 796
	// (get) Token: 0x06000C79 RID: 3193 RVA: 0x0004C578 File Offset: 0x0004A778
	// (set) Token: 0x06000C7A RID: 3194 RVA: 0x0004C57F File Offset: 0x0004A77F
	public static float HelpWindow { get; set; } = 42f;

	// Token: 0x1700031D RID: 797
	// (get) Token: 0x06000C7B RID: 3195 RVA: 0x0004C587 File Offset: 0x0004A787
	// (set) Token: 0x06000C7C RID: 3196 RVA: 0x0004C58E File Offset: 0x0004A78E
	public static float PhotoFlavor { get; set; } = 42f;

	// Token: 0x1700031E RID: 798
	// (get) Token: 0x06000C7D RID: 3197 RVA: 0x0004C596 File Offset: 0x0004A796
	// (set) Token: 0x06000C7E RID: 3198 RVA: 0x0004C59D File Offset: 0x0004A79D
	public static float MonthlyPackWindow { get; set; } = -42f;

	// Token: 0x040009AE RID: 2478
	public static float BaseHandleRange = -2f;

	// Token: 0x040009AF RID: 2479
	public static int HandleAdditionalFactor = -8;
}
