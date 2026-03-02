using System;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000101 RID: 257
public class ScrollAdditionalCtrl : MonoBehaviour
{
	// Token: 0x06000C68 RID: 3176 RVA: 0x0004C39F File Offset: 0x0004A59F
	private void Awake()
	{
		this.SensitivityCtrl();
	}

	// Token: 0x06000C69 RID: 3177 RVA: 0x0004C3A8 File Offset: 0x0004A5A8
	private void SensitivityCtrl()
	{
		FixedScrollRect component = base.GetComponent<FixedScrollRect>();
		if (null != component)
		{
			ReuseScroll component2 = base.GetComponent<ReuseScroll>();
			if (null != component2)
			{
				return;
			}
		}
		ScrollRect component3 = base.GetComponent<ScrollRect>();
		if (null != component3)
		{
			CustomScrollRect component4 = base.GetComponent<CustomScrollRect>();
			if (null != component4)
			{
				return;
			}
			if (component3.vertical)
			{
				component3.scrollSensitivity = ScrollParamDefine.BaseSensivility;
				if (component3.verticalScrollbar != null)
				{
					RectTransform component5 = component3.verticalScrollbar.transform.GetChild(0).Find("Handle").GetComponent<RectTransform>();
					if (component5.sizeDelta.x == ScrollParamDefine.BaseHandleRange)
					{
						component5.sizeDelta = new Vector2(component5.sizeDelta.x * (float)ScrollParamDefine.HandleAdditionalFactor, component5.sizeDelta.y);
					}
				}
				return;
			}
			if (component3.horizontal)
			{
				component3.scrollSensitivity = ScrollParamDefine.BaseSensivility * -1f;
				if (component3.horizontalScrollbar != null)
				{
					RectTransform component6 = component3.horizontalScrollbar.transform.GetChild(0).Find("Handle").GetComponent<RectTransform>();
					if (component6.sizeDelta.y == ScrollParamDefine.BaseHandleRange)
					{
						component6.sizeDelta = new Vector2(component6.sizeDelta.x, component6.sizeDelta.y * (float)ScrollParamDefine.HandleAdditionalFactor);
					}
				}
				return;
			}
		}
	}
}
