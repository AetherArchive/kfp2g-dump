using System;
using UnityEngine;

public class PguiRenderTextureCharaCtrl : PguiBehaviour
{
	public RenderTextureChara renderTextureChara { get; private set; }

	private void Awake()
	{
		if (this.disableAwakeCreate)
		{
			return;
		}
		this.Create();
	}

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

	public Transform GetCharaTransform()
	{
		RenderTextureChara renderTextureChara = this.renderTextureChara;
		if (renderTextureChara == null)
		{
			return null;
		}
		return renderTextureChara.GetChara();
	}

	public void Destroy()
	{
		if (this.isInit)
		{
			Object.Destroy(this.renderTextureChara.gameObject);
			this.renderTextureChara = null;
			this.isInit = false;
		}
	}

	public PguiRenderTextureCharaCtrl.Param param;

	[SerializeField]
	private Transform trans;

	private GameObject nest;

	public bool requestDebugDisp;

	private bool isInit;

	[SerializeField]
	private bool disableAwakeCreate;

	[Serializable]
	public class Param
	{
		public Param()
		{
			this.fieldOfView = 30f;
		}

		public Vector2 postion;

		public Vector3 rotation;

		public float fieldOfView;
	}
}
