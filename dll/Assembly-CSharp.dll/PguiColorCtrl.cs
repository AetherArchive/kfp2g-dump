using System;
using System.Collections.Generic;
using UnityEngine;

public class PguiColorCtrl : PguiBehaviour
{
	public void InitForce()
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
		this.m_ColorMap = new Dictionary<string, PguiColorCtrl.ExColor>();
		int num = 0;
		while (this.m_ColorList != null && num < this.m_ColorList.Count)
		{
			this.m_ColorMap[this.m_ColorList[num].id] = this.m_ColorList[num];
			num++;
		}
	}

	public Color GetGameObjectById(string id)
	{
		this.InitForce();
		if (this.m_ColorMap != null && this.m_ColorMap.ContainsKey(id))
		{
			return this.m_ColorMap[id].color;
		}
		return Color.black;
	}

	[SerializeField]
	private List<PguiColorCtrl.ExColor> m_ColorList = new List<PguiColorCtrl.ExColor>();

	private Dictionary<string, PguiColorCtrl.ExColor> m_ColorMap;

	private bool isInit;

	[Serializable]
	public class ExColor
	{
		public string id;

		public Color color;
	}
}
