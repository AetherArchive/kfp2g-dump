using System;
using SGNFW.uGUI;
using UnityEngine;

// Token: 0x020001D5 RID: 469
public class PguiNestPrefab : PguiBehaviour
{
	// Token: 0x06001FC1 RID: 8129 RVA: 0x00187718 File Offset: 0x00185918
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

	// Token: 0x06001FC2 RID: 8130 RVA: 0x00187764 File Offset: 0x00185964
	public void SetPrefab(GameObject prefab, Transform trans)
	{
		this.prefab = prefab;
		this.trans = trans;
	}

	// Token: 0x06001FC3 RID: 8131 RVA: 0x00187774 File Offset: 0x00185974
	public void InitForce()
	{
		this.Awake();
	}

	// Token: 0x0400171E RID: 5918
	[SerializeField]
	protected GameObject prefab;

	// Token: 0x0400171F RID: 5919
	[SerializeField]
	protected Transform trans;

	// Token: 0x04001720 RID: 5920
	private GameObject nest;

	// Token: 0x04001721 RID: 5921
	private bool isInit;
}
