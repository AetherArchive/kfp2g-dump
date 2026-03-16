using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PguiAddObjectProgressBarCtrl : PguiBehaviour
{
	private void InitForce()
	{
		this.Awake();
	}

	public void Awake()
	{
		if (this.isInit)
		{
			return;
		}
		this.isInit = true;
	}

	public void SetBarImage(Image image)
	{
		this.m_BarImage = image;
	}

	public void SetGameObjectList(List<GameObject> gameObjects)
	{
		this.m_GameObjectList = gameObjects;
	}

	public void SetAnchorObject(RectTransform anchor)
	{
		this.m_AnchorRT = anchor;
	}

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

	[SerializeField]
	private Image m_BarImage;

	private List<GameObject> m_GameObjectList = new List<GameObject>();

	[SerializeField]
	private RectTransform m_AnchorRT;

	private bool isInit;
}
