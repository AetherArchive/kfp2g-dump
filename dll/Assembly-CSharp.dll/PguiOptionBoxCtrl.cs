using System;
using System.Collections.Generic;
using UnityEngine;

public class PguiOptionBoxCtrl : PguiBehaviour
{
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

	public GameObject GetGameObjectById(int id)
	{
		if (this.m_OptionImageMap.ContainsKey(id))
		{
			return this.m_OptionImageMap[id].obj;
		}
		return null;
	}

	public T GetComponentById<T>(int id) where T : class
	{
		if (this.m_OptionImageMap.ContainsKey(id))
		{
			return this.m_OptionImageMap[id].obj.GetComponent<T>();
		}
		return default(T);
	}

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

	[SerializeField]
	private List<PguiOptionBoxCtrl.Option> m_OptionImageList = new List<PguiOptionBoxCtrl.Option>();

	private Dictionary<int, PguiOptionBoxCtrl.Option> m_OptionImageMap;

	[Serializable]
	public class Option
	{
		public int id;

		public GameObject obj;
	}
}
