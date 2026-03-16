using System;
using System.Collections.Generic;
using UnityEngine;

public class PguiTabGroupCtrl : PguiBehaviour
{
	public int SelectIndex
	{
		get
		{
			return this.m_SelectIndex;
		}
	}

	public void Awake()
	{
		for (int i = 0; i < this.m_PguiTabList.Count; i++)
		{
			PguiTabCtrl obj = this.m_PguiTabList[i];
			this.m_PguiTabList[i].m_Button.onClick.AddListener(delegate
			{
				this.OnTouchTab(obj);
			});
		}
	}

	private void OnTouchTab(PguiTabCtrl tabObj)
	{
		this.touchObjList.Add(tabObj.gameObject);
	}

	private void Update()
	{
		this.UpdateTabsInternal(this.touchObjList);
		this.touchObjList.Clear();
	}

	private PguiTabGroupCtrl.Result UpdateTabsInternal(List<GameObject> touchObjList)
	{
		int i;
		Predicate<PguiTabCtrl> <>9__0;
		int j;
		for (i = 0; i < touchObjList.Count; i = j + 1)
		{
			List<PguiTabCtrl> pguiTabList = this.m_PguiTabList;
			Predicate<PguiTabCtrl> predicate;
			if ((predicate = <>9__0) == null)
			{
				predicate = (<>9__0 = (PguiTabCtrl item) => item.gameObject == touchObjList[i]);
			}
			int num = pguiTabList.FindIndex(predicate);
			if (num >= 0)
			{
				return this.SelectTab(num);
			}
			j = i;
		}
		return new PguiTabGroupCtrl.Result(false, this.m_SelectIndex);
	}

	public void Setup(int firstSelectIndex, PguiTabGroupCtrl.OnSelectTab callback)
	{
		this.m_SelectIndex = firstSelectIndex;
		for (int i = 0; i < this.m_PguiTabList.Count; i++)
		{
			if (i == firstSelectIndex)
			{
				this.m_PguiTabList[i].TabAction(true, true);
			}
			else
			{
				this.m_PguiTabList[i].TabAction(false, true);
			}
		}
		this.onSelectTab = callback;
	}

	public PguiTabGroupCtrl.Result SelectTab(int index)
	{
		bool flag = false;
		if (this.m_SelectIndex != index && this.onSelectTab != null && this.onSelectTab(index))
		{
			flag = true;
			if (this.m_SelectIndex != PguiTabGroupCtrl.NonSelectIndex)
			{
				this.m_PguiTabList[this.m_SelectIndex].TabAction(false, true);
			}
			this.m_SelectIndex = index;
			this.m_PguiTabList[this.m_SelectIndex].TabAction(true, true);
			SoundManager.Play("prd_se_click", false, false);
		}
		return new PguiTabGroupCtrl.Result(flag, this.m_SelectIndex);
	}

	public List<PguiTabCtrl> m_PguiTabList = new List<PguiTabCtrl>();

	private int m_SelectIndex;

	private PguiTabGroupCtrl.OnSelectTab onSelectTab;

	public static readonly int NonSelectIndex = -1;

	private List<GameObject> touchObjList = new List<GameObject>();

	public delegate bool OnSelectTab(int index);

	public class Result
	{
		public Result()
		{
		}

		public Result(bool ischange, int selIndex)
		{
			this.isChangeSelect = ischange;
			this.selectIndex = selIndex;
		}

		public bool isChangeSelect;

		public int selectIndex;
	}
}
