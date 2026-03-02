using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001E2 RID: 482
public class PguiTabGroupCtrl : PguiBehaviour
{
	// Token: 0x1700044E RID: 1102
	// (get) Token: 0x06002045 RID: 8261 RVA: 0x0018A7DE File Offset: 0x001889DE
	public int SelectIndex
	{
		get
		{
			return this.m_SelectIndex;
		}
	}

	// Token: 0x06002046 RID: 8262 RVA: 0x0018A7E8 File Offset: 0x001889E8
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

	// Token: 0x06002047 RID: 8263 RVA: 0x0018A851 File Offset: 0x00188A51
	private void OnTouchTab(PguiTabCtrl tabObj)
	{
		this.touchObjList.Add(tabObj.gameObject);
	}

	// Token: 0x06002048 RID: 8264 RVA: 0x0018A864 File Offset: 0x00188A64
	private void Update()
	{
		this.UpdateTabsInternal(this.touchObjList);
		this.touchObjList.Clear();
	}

	// Token: 0x06002049 RID: 8265 RVA: 0x0018A880 File Offset: 0x00188A80
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

	// Token: 0x0600204A RID: 8266 RVA: 0x0018A90C File Offset: 0x00188B0C
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

	// Token: 0x0600204B RID: 8267 RVA: 0x0018A96C File Offset: 0x00188B6C
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

	// Token: 0x04001768 RID: 5992
	public List<PguiTabCtrl> m_PguiTabList = new List<PguiTabCtrl>();

	// Token: 0x04001769 RID: 5993
	private int m_SelectIndex;

	// Token: 0x0400176A RID: 5994
	private PguiTabGroupCtrl.OnSelectTab onSelectTab;

	// Token: 0x0400176B RID: 5995
	public static readonly int NonSelectIndex = -1;

	// Token: 0x0400176C RID: 5996
	private List<GameObject> touchObjList = new List<GameObject>();

	// Token: 0x02001024 RID: 4132
	// (Invoke) Token: 0x06005202 RID: 20994
	public delegate bool OnSelectTab(int index);

	// Token: 0x02001025 RID: 4133
	public class Result
	{
		// Token: 0x06005205 RID: 20997 RVA: 0x002483D9 File Offset: 0x002465D9
		public Result()
		{
		}

		// Token: 0x06005206 RID: 20998 RVA: 0x002483E1 File Offset: 0x002465E1
		public Result(bool ischange, int selIndex)
		{
			this.isChangeSelect = ischange;
			this.selectIndex = selIndex;
		}

		// Token: 0x04005AB1 RID: 23217
		public bool isChangeSelect;

		// Token: 0x04005AB2 RID: 23218
		public int selectIndex;
	}
}
