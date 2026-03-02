using System;
using UnityEngine;

// Token: 0x0200002C RID: 44
public class DebugDispSafeArea : MonoBehaviour
{
	// Token: 0x06000099 RID: 153 RVA: 0x00005A48 File Offset: 0x00003C48
	private void Awake()
	{
		this.ApplySafeArea();
	}

	// Token: 0x0600009A RID: 154 RVA: 0x00005A50 File Offset: 0x00003C50
	private void Update()
	{
		if (this.width != Screen.width || this.height != Screen.height)
		{
			this.ApplySafeArea();
		}
	}

	// Token: 0x0600009B RID: 155 RVA: 0x00005A74 File Offset: 0x00003C74
	private void ApplySafeArea()
	{
		this.width = Screen.width;
		this.height = Screen.height;
		RectTransform rectTransform = base.GetComponent<RectTransform>();
		if (rectTransform != null)
		{
			Rect safeArea = SafeAreaScaler.GetSafeArea();
			Vector2 vector = new Vector2(1f / (float)this.width, 1f / (float)this.height);
			rectTransform.anchorMin = Vector2.Scale(safeArea.position, vector);
			rectTransform.anchorMax = Vector2.Scale(safeArea.position + safeArea.size, vector);
			if (base.transform.childCount == 1)
			{
				rectTransform = base.transform.GetChild(0).GetComponent<RectTransform>();
				if (rectTransform != null)
				{
					float num = 16f;
					float num2 = 9f;
					if (this.width < this.height)
					{
						num = 9f;
						num2 = 16f;
					}
					float num3 = safeArea.size.x / num;
					float num4 = safeArea.size.y / num2;
					float num5 = ((num3 > num4) ? num4 : num3);
					rectTransform.anchorMax = new Vector2(num5 * num, num5 * num2);
					rectTransform.anchorMin = (safeArea.size - rectTransform.anchorMax) * 0.5f;
					rectTransform.anchorMax += rectTransform.anchorMin;
					vector = new Vector2(1f / safeArea.size.x, 1f / safeArea.size.y);
					rectTransform.anchorMin = Vector2.Scale(rectTransform.anchorMin, vector);
					rectTransform.anchorMax = Vector2.Scale(rectTransform.anchorMax, vector);
				}
			}
		}
	}

	// Token: 0x04000118 RID: 280
	private int width;

	// Token: 0x04000119 RID: 281
	private int height;
}
