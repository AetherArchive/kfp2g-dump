using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001D7 RID: 471
public class PguiOptionBoxCtrl : PguiBehaviour
{
	// Token: 0x06001FF3 RID: 8179 RVA: 0x00189968 File Offset: 0x00187B68
	public void Awake()
	{
		this.m_OptionImageMap = new Dictionary<int, PguiOptionBoxCtrl.Option>();
		int num = 0;
		while (this.m_OptionImageList != null && num < this.m_OptionImageList.Count)
		{
			this.m_OptionImageMap[this.m_OptionImageList[num].id] = this.m_OptionImageList[num];
			num++;
		}
	}

	// Token: 0x06001FF4 RID: 8180 RVA: 0x001899C6 File Offset: 0x00187BC6
	public GameObject GetGameObjectById(int id)
	{
		if (this.m_OptionImageMap.ContainsKey(id))
		{
			return this.m_OptionImageMap[id].obj;
		}
		return null;
	}

	// Token: 0x06001FF5 RID: 8181 RVA: 0x001899EC File Offset: 0x00187BEC
	public T GetComponentById<T>(int id) where T : class
	{
		if (this.m_OptionImageMap.ContainsKey(id))
		{
			return this.m_OptionImageMap[id].obj.GetComponent<T>();
		}
		return default(T);
	}

	// Token: 0x06001FF6 RID: 8182 RVA: 0x00189A28 File Offset: 0x00187C28
	public int AddOptionObj(GameObject obj)
	{
		PguiOptionBoxCtrl.Option option = new PguiOptionBoxCtrl.Option();
		int num = 1;
		if (this.m_OptionImageList.Count > 0)
		{
			num = this.m_OptionImageList[this.m_OptionImageList.Count - 1].id + 1;
		}
		option.id = num;
		option.obj = obj;
		this.m_OptionImageList.Add(option);
		return num;
	}

	// Token: 0x0400173C RID: 5948
	[SerializeField]
	private List<PguiOptionBoxCtrl.Option> m_OptionImageList = new List<PguiOptionBoxCtrl.Option>();

	// Token: 0x0400173D RID: 5949
	private Dictionary<int, PguiOptionBoxCtrl.Option> m_OptionImageMap;

	// Token: 0x0200101E RID: 4126
	[Serializable]
	public class Option
	{
		// Token: 0x04005A9A RID: 23194
		public int id;

		// Token: 0x04005A9B RID: 23195
		public GameObject obj;
	}
}
