using System;
using UnityEngine;

// Token: 0x020001DB RID: 475
public class PguiRenderTextureCharaCtrl : PguiBehaviour
{
	// Token: 0x17000446 RID: 1094
	// (get) Token: 0x06002013 RID: 8211 RVA: 0x0018A05C File Offset: 0x0018825C
	// (set) Token: 0x06002014 RID: 8212 RVA: 0x0018A064 File Offset: 0x00188264
	public RenderTextureChara renderTextureChara { get; private set; }

	// Token: 0x06002015 RID: 8213 RVA: 0x0018A06D File Offset: 0x0018826D
	private void Awake()
	{
		if (this.disableAwakeCreate)
		{
			return;
		}
		this.Create();
	}

	// Token: 0x06002016 RID: 8214 RVA: 0x0018A080 File Offset: 0x00188280
	public RenderTextureChara Create()
	{
		if (this.isInit)
		{
			return this.renderTextureChara;
		}
		this.isInit = true;
		this.nest = Object.Instantiate<GameObject>((GameObject)Resources.Load("RenderTextureChara/Prefab/RenderTextureCharaCtrl"), this.trans);
		this.renderTextureChara = this.nest.GetComponent<RenderTextureChara>();
		return this.renderTextureChara;
	}

	// Token: 0x06002017 RID: 8215 RVA: 0x0018A0DA File Offset: 0x001882DA
	public Transform GetCharaTransform()
	{
		RenderTextureChara renderTextureChara = this.renderTextureChara;
		if (renderTextureChara == null)
		{
			return null;
		}
		return renderTextureChara.GetChara();
	}

	// Token: 0x06002018 RID: 8216 RVA: 0x0018A0ED File Offset: 0x001882ED
	public void Destroy()
	{
		if (this.isInit)
		{
			Object.Destroy(this.renderTextureChara.gameObject);
			this.renderTextureChara = null;
			this.isInit = false;
		}
	}

	// Token: 0x04001748 RID: 5960
	public PguiRenderTextureCharaCtrl.Param param;

	// Token: 0x04001749 RID: 5961
	[SerializeField]
	private Transform trans;

	// Token: 0x0400174A RID: 5962
	private GameObject nest;

	// Token: 0x0400174B RID: 5963
	public bool requestDebugDisp;

	// Token: 0x0400174C RID: 5964
	private bool isInit;

	// Token: 0x0400174D RID: 5965
	[SerializeField]
	private bool disableAwakeCreate;

	// Token: 0x02001021 RID: 4129
	[Serializable]
	public class Param
	{
		// Token: 0x060051FE RID: 20990 RVA: 0x002483B6 File Offset: 0x002465B6
		public Param()
		{
			this.fieldOfView = 30f;
		}

		// Token: 0x04005AAA RID: 23210
		public Vector2 postion;

		// Token: 0x04005AAB RID: 23211
		public Vector3 rotation;

		// Token: 0x04005AAC RID: 23212
		public float fieldOfView;
	}
}
