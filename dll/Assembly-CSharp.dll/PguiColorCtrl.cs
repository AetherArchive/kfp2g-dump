using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001D0 RID: 464
public class PguiColorCtrl : PguiBehaviour
{
	// Token: 0x06001FA8 RID: 8104 RVA: 0x001871D9 File Offset: 0x001853D9
	public void InitForce()
	{
		this.Awake();
	}

	// Token: 0x06001FA9 RID: 8105 RVA: 0x001871E4 File Offset: 0x001853E4
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

	// Token: 0x06001FAA RID: 8106 RVA: 0x00187252 File Offset: 0x00185452
	public Color GetGameObjectById(string id)
	{
		this.InitForce();
		if (this.m_ColorMap != null && this.m_ColorMap.ContainsKey(id))
		{
			return this.m_ColorMap[id].color;
		}
		return Color.black;
	}

	// Token: 0x04001705 RID: 5893
	[SerializeField]
	private List<PguiColorCtrl.ExColor> m_ColorList = new List<PguiColorCtrl.ExColor>();

	// Token: 0x04001706 RID: 5894
	private Dictionary<string, PguiColorCtrl.ExColor> m_ColorMap;

	// Token: 0x04001707 RID: 5895
	private bool isInit;

	// Token: 0x02001014 RID: 4116
	[Serializable]
	public class ExColor
	{
		// Token: 0x04005A5F RID: 23135
		public string id;

		// Token: 0x04005A60 RID: 23136
		public Color color;
	}
}
