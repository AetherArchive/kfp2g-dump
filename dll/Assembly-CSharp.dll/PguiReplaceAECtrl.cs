using System;
using System.Collections.Generic;
using AEAuth3;
using UnityEngine;

public class PguiReplaceAECtrl : MonoBehaviour
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
		this.m_AEMap = new Dictionary<string, PguiReplaceAECtrl.ExAE>();
		int num = 0;
		while (this.m_AEList != null && num < this.m_AEList.Count)
		{
			this.m_AEMap[this.m_AEList[num].id] = this.m_AEList[num];
			num++;
		}
	}

	public SourceClip GetSourceClipById(string id)
	{
		if (this.m_AEMap == null)
		{
			this.InitForce();
		}
		if (this.m_AEMap.ContainsKey(id))
		{
			return this.m_AEMap[id].m_SourceClip;
		}
		return null;
	}

	public void Replace(ref GameObject go, string id)
	{
		PguiAECtrl component = go.GetComponent<PguiAECtrl>();
		AEImage aeimage = ((component == null) ? go.GetComponent<AEImage>() : component.m_AEImage);
		if (aeimage != null)
		{
			aeimage.sourceClip = this.GetSourceClipById(id);
		}
	}

	public void Replace(string id)
	{
		PguiAECtrl component = base.GetComponent<PguiAECtrl>();
		AEImage aeimage = ((component == null) ? base.GetComponent<AEImage>() : component.m_AEImage);
		if (aeimage != null)
		{
			aeimage.sourceClip = this.GetSourceClipById(id);
		}
	}

	[SerializeField]
	private List<PguiReplaceAECtrl.ExAE> m_AEList = new List<PguiReplaceAECtrl.ExAE>();

	private Dictionary<string, PguiReplaceAECtrl.ExAE> m_AEMap;

	private bool isInit;

	[Serializable]
	public class ExAE
	{
		public string id;

		public SourceClip m_SourceClip;
	}
}
