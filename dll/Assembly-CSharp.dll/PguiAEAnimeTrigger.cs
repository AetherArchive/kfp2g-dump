using System;
using UnityEngine;

public class PguiAEAnimeTrigger : MonoBehaviour
{
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

	public PguiAECtrl m_PguiAECtrl;

	public PguiAECtrl.AmimeType m_PlayAnimeType;

	private bool oldTriggerOn;

	public bool triggerOn;
}
