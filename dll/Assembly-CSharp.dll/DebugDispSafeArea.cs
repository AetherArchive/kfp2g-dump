using System;
using UnityEngine;

public class DebugDispSafeArea : MonoBehaviour
{
	private void Awake()
	{
		this.ApplySafeArea();
	}

	private void Update()
	{
		if (this.width != Screen.width || this.height != Screen.height)
		{
			this.ApplySafeArea();
		}
	}

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

	private int width;

	private int height;
}
