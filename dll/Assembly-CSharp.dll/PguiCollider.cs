using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001CE RID: 462
public class PguiCollider : Graphic
{
	// Token: 0x06001FA0 RID: 8096 RVA: 0x001870F4 File Offset: 0x001852F4
	protected override void Awake()
	{
		this.color = Color.clear;
		if (base.gameObject.GetComponent<CanvasRenderer>() == null)
		{
			base.gameObject.AddComponent<CanvasRenderer>();
		}
	}
}
