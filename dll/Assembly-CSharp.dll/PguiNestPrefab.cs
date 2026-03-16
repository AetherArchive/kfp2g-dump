using System;
using SGNFW.uGUI;
using UnityEngine;

public class PguiNestPrefab : PguiBehaviour
{
	private void Awake()
	{
		if (this.isInit)
		{
			return;
		}
		if (this.prefab != null)
		{
			this.nest = Manager.Create(this.prefab, this.trans, null, null, Layer.UI);
			this.prefab = null;
			this.isInit = true;
		}
	}

	public void SetPrefab(GameObject prefab, Transform trans)
	{
		this.prefab = prefab;
		this.trans = trans;
	}

	public void InitForce()
	{
		this.Awake();
	}

	[SerializeField]
	protected GameObject prefab;

	[SerializeField]
	protected Transform trans;

	private GameObject nest;

	private bool isInit;
}
