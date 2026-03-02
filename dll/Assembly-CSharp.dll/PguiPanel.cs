using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001D9 RID: 473
public class PguiPanel : Image
{
	// Token: 0x06002003 RID: 8195 RVA: 0x00189D36 File Offset: 0x00187F36
	protected override void Awake()
	{
		base.Awake();
		this.color = Color.clear;
	}

	// Token: 0x17000443 RID: 1091
	// (get) Token: 0x06002004 RID: 8196 RVA: 0x00189D49 File Offset: 0x00187F49
	// (set) Token: 0x06002005 RID: 8197 RVA: 0x00189D51 File Offset: 0x00187F51
	public int PguiBehaviourVersion { get; set; }
}
