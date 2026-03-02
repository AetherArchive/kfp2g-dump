using System;
using UnityEngine;

// Token: 0x020001C9 RID: 457
public class PguiAEAnimeTrigger : MonoBehaviour
{
	// Token: 0x06001F54 RID: 8020 RVA: 0x00183927 File Offset: 0x00181B27
	private void Update()
	{
		if (this.triggerOn && !this.oldTriggerOn)
		{
			this.m_PguiAECtrl.PlayAnimation(this.m_PlayAnimeType, null);
			this.triggerOn = false;
			return;
		}
		this.oldTriggerOn = this.triggerOn;
	}

	// Token: 0x040016C4 RID: 5828
	public PguiAECtrl m_PguiAECtrl;

	// Token: 0x040016C5 RID: 5829
	public PguiAECtrl.AmimeType m_PlayAnimeType;

	// Token: 0x040016C6 RID: 5830
	private bool oldTriggerOn;

	// Token: 0x040016C7 RID: 5831
	public bool triggerOn;
}
