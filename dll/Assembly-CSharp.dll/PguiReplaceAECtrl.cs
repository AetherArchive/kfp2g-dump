using System;
using System.Collections.Generic;
using AEAuth3;
using UnityEngine;

// Token: 0x020001DC RID: 476
public class PguiReplaceAECtrl : MonoBehaviour
{
	// Token: 0x0600201A RID: 8218 RVA: 0x0018A11D File Offset: 0x0018831D
	public void InitForce()
	{
		this.Awake();
	}

	// Token: 0x0600201B RID: 8219 RVA: 0x0018A128 File Offset: 0x00188328
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

	// Token: 0x0600201C RID: 8220 RVA: 0x0018A196 File Offset: 0x00188396
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

	// Token: 0x0600201D RID: 8221 RVA: 0x0018A1C8 File Offset: 0x001883C8
	public void Replace(ref GameObject go, string id)
	{
		PguiAECtrl component = go.GetComponent<PguiAECtrl>();
		AEImage aeimage = ((component == null) ? go.GetComponent<AEImage>() : component.m_AEImage);
		if (aeimage != null)
		{
			aeimage.sourceClip = this.GetSourceClipById(id);
		}
	}

	// Token: 0x0600201E RID: 8222 RVA: 0x0018A20C File Offset: 0x0018840C
	public void Replace(string id)
	{
		PguiAECtrl component = base.GetComponent<PguiAECtrl>();
		AEImage aeimage = ((component == null) ? base.GetComponent<AEImage>() : component.m_AEImage);
		if (aeimage != null)
		{
			aeimage.sourceClip = this.GetSourceClipById(id);
		}
	}

	// Token: 0x0400174F RID: 5967
	[SerializeField]
	private List<PguiReplaceAECtrl.ExAE> m_AEList = new List<PguiReplaceAECtrl.ExAE>();

	// Token: 0x04001750 RID: 5968
	private Dictionary<string, PguiReplaceAECtrl.ExAE> m_AEMap;

	// Token: 0x04001751 RID: 5969
	private bool isInit;

	// Token: 0x02001022 RID: 4130
	[Serializable]
	public class ExAE
	{
		// Token: 0x04005AAD RID: 23213
		public string id;

		// Token: 0x04005AAE RID: 23214
		public SourceClip m_SourceClip;
	}
}
