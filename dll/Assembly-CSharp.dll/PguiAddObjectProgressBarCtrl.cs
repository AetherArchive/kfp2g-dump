using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001C8 RID: 456
public class PguiAddObjectProgressBarCtrl : PguiBehaviour
{
	// Token: 0x06001F4D RID: 8013 RVA: 0x001837F8 File Offset: 0x001819F8
	private void InitForce()
	{
		this.Awake();
	}

	// Token: 0x06001F4E RID: 8014 RVA: 0x00183800 File Offset: 0x00181A00
	public void Awake()
	{
		if (this.isInit)
		{
			return;
		}
		this.isInit = true;
	}

	// Token: 0x06001F4F RID: 8015 RVA: 0x00183812 File Offset: 0x00181A12
	public void SetBarImage(Image image)
	{
		this.m_BarImage = image;
	}

	// Token: 0x06001F50 RID: 8016 RVA: 0x0018381B File Offset: 0x00181A1B
	public void SetGameObjectList(List<GameObject> gameObjects)
	{
		this.m_GameObjectList = gameObjects;
	}

	// Token: 0x06001F51 RID: 8017 RVA: 0x00183824 File Offset: 0x00181A24
	public void SetAnchorObject(RectTransform anchor)
	{
		this.m_AnchorRT = anchor;
	}

	// Token: 0x06001F52 RID: 8018 RVA: 0x00183830 File Offset: 0x00181A30
	private void Update()
	{
		if (!this.isInit)
		{
			this.InitForce();
		}
		if (this.m_BarImage != null && this.m_AnchorRT != null)
		{
			foreach (GameObject gameObject in this.m_GameObjectList)
			{
			}
			this.m_AnchorRT.anchorMin = new Vector2(this.m_BarImage.fillAmount, this.m_AnchorRT.anchorMin.y);
			this.m_AnchorRT.anchorMax = new Vector2(this.m_BarImage.fillAmount, this.m_AnchorRT.anchorMax.y);
			this.m_AnchorRT.anchoredPosition = Vector2.zero;
		}
	}

	// Token: 0x040016C0 RID: 5824
	[SerializeField]
	private Image m_BarImage;

	// Token: 0x040016C1 RID: 5825
	private List<GameObject> m_GameObjectList = new List<GameObject>();

	// Token: 0x040016C2 RID: 5826
	[SerializeField]
	private RectTransform m_AnchorRT;

	// Token: 0x040016C3 RID: 5827
	private bool isInit;
}
