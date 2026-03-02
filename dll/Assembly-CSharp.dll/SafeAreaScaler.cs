using System;
using SGNFW.Common;
using UnityEngine;

// Token: 0x02000100 RID: 256
[RequireComponent(typeof(RectTransform))]
public class SafeAreaScaler : MonoBehaviour
{
	// Token: 0x17000313 RID: 787
	// (get) Token: 0x06000C5B RID: 3163 RVA: 0x0004BA50 File Offset: 0x00049C50
	public static int ScreenWidth
	{
		get
		{
			return SceneManager.screenSize.width;
		}
	}

	// Token: 0x17000314 RID: 788
	// (get) Token: 0x06000C5C RID: 3164 RVA: 0x0004BA5C File Offset: 0x00049C5C
	public static int ScreenHeight
	{
		get
		{
			return SceneManager.screenSize.height;
		}
	}

	// Token: 0x06000C5D RID: 3165 RVA: 0x0004BA68 File Offset: 0x00049C68
	public static Rect GetSafeArea()
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = (float)SafeAreaScaler.ScreenWidth;
		float num4 = (float)SafeAreaScaler.ScreenHeight;
		SafeAreaScaler.SafeArea.x = num;
		SafeAreaScaler.SafeArea.y = num2;
		SafeAreaScaler.SafeArea.width = num3;
		SafeAreaScaler.SafeArea.height = num4;
		return SafeAreaScaler.SafeArea;
	}

	// Token: 0x06000C5E RID: 3166 RVA: 0x0004BAC0 File Offset: 0x00049CC0
	public static bool IsLongDevice()
	{
		if (!SceneHome.nowVertView)
		{
			return SafeAreaScaler.CompareToFloat((float)SafeAreaScaler.ScreenWidth / (float)SafeAreaScaler.ScreenHeight, 2.1666667f) >= 0;
		}
		return SafeAreaScaler.CompareToFloat((float)SafeAreaScaler.ScreenHeight / (float)SafeAreaScaler.ScreenWidth, 2.1666667f) >= 0;
	}

	// Token: 0x06000C5F RID: 3167 RVA: 0x0004BB10 File Offset: 0x00049D10
	public static bool IsMapFixDevice()
	{
		if (!SceneHome.nowVertView)
		{
			float num = (float)SafeAreaScaler.ScreenHeight / (QuestUtil.DEFAULT_SCREEN_HEIGHT + QuestUtil.MAP_MASK_IMAGE_HEIGHT);
			float num2 = (float)SafeAreaScaler.ScreenWidth - QuestUtil.MAP_MASK_IMAGE_WIDTH * 2f * num;
			float num3 = (float)SafeAreaScaler.ScreenHeight - QuestUtil.MAP_MASK_IMAGE_HEIGHT * num;
			return SafeAreaScaler.CompareToFloat(num2 / num3, 1.7777778f) >= 0;
		}
		return false;
	}

	// Token: 0x06000C60 RID: 3168 RVA: 0x0004BB70 File Offset: 0x00049D70
	public void ApplySafeArea()
	{
		if (SafeAreaScaler.ScreenWidth == 0 || SafeAreaScaler.ScreenHeight == 0)
		{
			return;
		}
		this.width = SafeAreaScaler.ScreenWidth;
		this.height = SafeAreaScaler.ScreenHeight;
		Rect safeArea = SafeAreaScaler.GetSafeArea();
		Rect safeArea2 = SafeAreaScaler.GetSafeArea();
		float num = (float)this.width;
		float num2 = (float)this.height;
		float num3 = (SceneHome.nowVertView ? safeArea2.height : safeArea2.width);
		float num4 = (SceneHome.nowVertView ? safeArea2.width : safeArea2.height);
		bool flag = Singleton<CanvasManager>.Instance != null && base.gameObject == Singleton<CanvasManager>.Instance.outFrame && num3 / 16f > num4 / 9f;
		bool flag2 = base.gameObject == SceneQuest.mapBoxObject && safeArea2.height == (float)this.height && SafeAreaScaler.IsMapFixDevice();
		float num5 = 0f;
		if (flag2)
		{
			num5 = (float)this.height / (QuestUtil.DEFAULT_SCREEN_HEIGHT + QuestUtil.MAP_MASK_IMAGE_HEIGHT) * QuestUtil.MAP_MASK_IMAGE_HEIGHT;
		}
		float num6 = (flag ? 19.5f : 16f);
		float num7 = 9f;
		if (SceneHome.nowVertView)
		{
			num6 = 9f;
			num7 = (flag ? 19.5f : 16f);
		}
		float num8 = (flag ? num : safeArea2.size.x);
		float num9 = (flag ? num2 : (safeArea2.size.y - num5));
		float num10 = num8 / num6;
		float num11 = num9 / num7;
		float num12 = ((num10 > num11) ? num11 : num10);
		float num13 = num12 * num6;
		float num14 = num12 * num7;
		float num15 = num13 / num;
		if (SceneHome.nowVertView)
		{
			num15 *= num7 / num6;
		}
		float num16 = safeArea2.position.x + (safeArea2.size.x - num13) * 0.5f;
		float num17 = safeArea2.position.y + num5 + (safeArea2.size.y - num5 - num14) * 0.5f;
		float num18 = (num13 / num15 - num13) * 0.5f;
		float num19 = (num14 / num15 - num14) * 0.5f;
		this.rectTransform.localScale = new Vector3(num15, num15, 1f);
		this.rectTransform.anchorMin = new Vector2((num16 - num18) / num, (num17 - num19) / num2);
		this.rectTransform.anchorMax = new Vector2((num16 + num13 + num18) / num, (num17 + num14 + num19) / num2);
		float num20 = (SceneHome.nowVertView ? 9f : 19.5f);
		float num21 = (SceneHome.nowVertView ? 19.5f : 9f);
		float num22 = num / num20;
		float num23 = num2 / num21;
		float num24 = ((num22 > num23) ? num23 : num22);
		float num25 = num24 * num20;
		float num26 = num24 * num21;
		num18 = ((!flag && !SceneHome.nowVertView && SafeAreaScaler.CompareToFloat(num, num25) > 0 && SafeAreaScaler.CompareToFloat(safeArea2.size.x, num25) > 0) ? ((num25 - num13) * 360f / num9) : ((!flag && !SceneHome.nowVertView && SafeAreaScaler.CompareToFloat(num, num25) > 0 && SafeAreaScaler.CompareToFloat(safeArea2.size.x, num25) < 0) ? ((safeArea2.size.x - num13) * 360f / num9) : ((flag && !SceneHome.nowVertView && SafeAreaScaler.CompareToFloat(num, num25) > 0 && SafeAreaScaler.CompareToFloat(safeArea2.size.x, num25) < 0) ? ((num25 - num13) * 360f / num9) : ((!flag && !SceneHome.nowVertView && SafeAreaScaler.CompareToFloat(safeArea2.size.x, num13) > 0) ? ((safeArea2.size.x - num13) * 360f / num9) : ((!flag && SceneHome.nowVertView) ? ((safeArea2.size.x - num13) * 640f / num9) : 0f)))));
		num19 = ((!flag && SceneHome.nowVertView && SafeAreaScaler.CompareToFloat(num2, num26) > 0 && SafeAreaScaler.CompareToFloat(safeArea2.size.y, num26) > 0) ? ((num26 - num14) * 360f / num8) : ((!flag && SceneHome.nowVertView && SafeAreaScaler.CompareToFloat(num2, num26) > 0 && SafeAreaScaler.CompareToFloat(safeArea2.size.y, num26) < 0) ? ((safeArea2.size.y - num14) * 360f / num8) : ((flag && SceneHome.nowVertView && SafeAreaScaler.CompareToFloat(num2, num26) > 0 && SafeAreaScaler.CompareToFloat(safeArea2.size.y, num26) < 0) ? ((num26 - num14) * 360f / num8) : ((!flag && SceneHome.nowVertView && SafeAreaScaler.CompareToFloat(safeArea2.size.y, num14) > 0) ? ((safeArea2.size.y - num14) * 360f / num8) : 0f))));
		this.rectTransform.offsetMin = new Vector2(this.offmin.x - num18, this.offmin.y - num19);
		this.rectTransform.offsetMax = new Vector2(this.offmax.x + num18, this.offmax.y + num19);
		if (flag && SafeAreaScaler.CompareToFloat(num, num25) > 0 && SafeAreaScaler.CompareToFloat(safeArea2.size.x, num25) < 0)
		{
			float x = safeArea2.x;
			float num27 = num - safeArea2.x - safeArea2.size.x;
			if (x != num27)
			{
				this.rectTransform.offsetMin -= new Vector2((x - num27) * 360f * 0.5f / num9, 0f);
				this.rectTransform.offsetMax -= new Vector2((x - num27) * 360f * 0.5f / num9, 0f);
			}
		}
		else if (flag && SceneHome.nowVertView && SafeAreaScaler.CompareToFloat(num2, num26) > 0 && SafeAreaScaler.CompareToFloat(safeArea2.size.y, num26) < 0)
		{
			float num28 = num2 - safeArea2.y - safeArea2.size.y;
			float y = safeArea2.y;
			if (num28 != y)
			{
				this.rectTransform.offsetMin -= new Vector2(0f, (y - num28) * 360f * 0.5f / num8);
				this.rectTransform.offsetMax -= new Vector2(0f, (y - num28) * 360f * 0.5f / num8);
			}
		}
		this.scrW = Screen.width;
		this.scrH = Screen.height;
		this.applySafeArea = safeArea;
	}

	// Token: 0x06000C61 RID: 3169 RVA: 0x0004C260 File Offset: 0x0004A460
	private void Awake()
	{
		this.rectTransform = base.GetComponent<RectTransform>();
		this.offmin = this.rectTransform.offsetMin;
		this.offmax = this.rectTransform.offsetMax;
		this.ApplySafeArea();
	}

	// Token: 0x06000C62 RID: 3170 RVA: 0x0004C298 File Offset: 0x0004A498
	private void Update()
	{
		if (this.width != SafeAreaScaler.ScreenWidth || this.scrW != Screen.width || this.height != SafeAreaScaler.ScreenHeight || this.scrH != Screen.height || this.applySafeArea != SafeAreaScaler.GetSafeArea())
		{
			this.ApplySafeArea();
		}
	}

	// Token: 0x06000C63 RID: 3171 RVA: 0x0004C2F4 File Offset: 0x0004A4F4
	private void OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus)
		{
			return;
		}
		this.width = (this.height = 0);
		this.scrW = (this.scrH = 0);
	}

	// Token: 0x06000C64 RID: 3172 RVA: 0x0004C325 File Offset: 0x0004A525
	private void OnEnable()
	{
		this.ApplySafeArea();
	}

	// Token: 0x06000C65 RID: 3173 RVA: 0x0004C330 File Offset: 0x0004A530
	public static int CompareToFloat(float a, float b)
	{
		float num = 0.01f;
		if (Math.Abs(a - b) < num)
		{
			return 0;
		}
		if (a > b)
		{
			return 1;
		}
		return -1;
	}

	// Token: 0x0400099A RID: 2458
	public static readonly float FIX_SAFEAREA_PERCENT = 0.03f;

	// Token: 0x0400099B RID: 2459
	private static Rect SafeArea = new Rect(0f, 0f, 0f, 0f);

	// Token: 0x0400099C RID: 2460
	private RectTransform rectTransform;

	// Token: 0x0400099D RID: 2461
	private int width;

	// Token: 0x0400099E RID: 2462
	private int height;

	// Token: 0x0400099F RID: 2463
	private Vector2 offmin = Vector2.zero;

	// Token: 0x040009A0 RID: 2464
	private Vector2 offmax = Vector2.zero;

	// Token: 0x040009A1 RID: 2465
	private int scrW;

	// Token: 0x040009A2 RID: 2466
	private int scrH;

	// Token: 0x040009A3 RID: 2467
	public Rect applySafeArea;
}
