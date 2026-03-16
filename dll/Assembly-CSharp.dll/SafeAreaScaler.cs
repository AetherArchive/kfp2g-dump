using System;
using SGNFW.Common;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SafeAreaScaler : MonoBehaviour
{
	public static int ScreenWidth
	{
		get
		{
			return SceneManager.screenSize.width;
		}
	}

	public static int ScreenHeight
	{
		get
		{
			return SceneManager.screenSize.height;
		}
	}

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

	public static bool IsLongDevice()
	{
		if (!SceneHome.nowVertView)
		{
			return SafeAreaScaler.CompareToFloat((float)SafeAreaScaler.ScreenWidth / (float)SafeAreaScaler.ScreenHeight, 2.1666667f) >= 0;
		}
		return SafeAreaScaler.CompareToFloat((float)SafeAreaScaler.ScreenHeight / (float)SafeAreaScaler.ScreenWidth, 2.1666667f) >= 0;
	}

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

	private void Awake()
	{
		this.rectTransform = base.GetComponent<RectTransform>();
		this.offmin = this.rectTransform.offsetMin;
		this.offmax = this.rectTransform.offsetMax;
		this.ApplySafeArea();
	}

	private void Update()
	{
		if (this.width != SafeAreaScaler.ScreenWidth || this.scrW != Screen.width || this.height != SafeAreaScaler.ScreenHeight || this.scrH != Screen.height || this.applySafeArea != SafeAreaScaler.GetSafeArea())
		{
			this.ApplySafeArea();
		}
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus)
		{
			return;
		}
		this.width = (this.height = 0);
		this.scrW = (this.scrH = 0);
	}

	private void OnEnable()
	{
		this.ApplySafeArea();
	}

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

	public static readonly float FIX_SAFEAREA_PERCENT = 0.03f;

	private static Rect SafeArea = new Rect(0f, 0f, 0f, 0f);

	private RectTransform rectTransform;

	private int width;

	private int height;

	private Vector2 offmin = Vector2.zero;

	private Vector2 offmax = Vector2.zero;

	private int scrW;

	private int scrH;

	public Rect applySafeArea;
}
