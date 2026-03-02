using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200019D RID: 413
public class DressUpWipeCtrl : MonoBehaviour
{
	// Token: 0x06001B7B RID: 7035 RVA: 0x0015F72D File Offset: 0x0015D92D
	public void Init()
	{
		if (this.guiData != null)
		{
			return;
		}
		this.guiData = new DressUpWipeCtrl.GUI(base.transform);
		base.gameObject.SetActive(false);
	}

	// Token: 0x06001B7C RID: 7036 RVA: 0x0015F758 File Offset: 0x0015D958
	public void Play(DressUpWipeCtrl.TriggerCallback cb = null)
	{
		base.gameObject.SetActive(true);
		SoundManager.Play("prd_se_friends_costume_function_shift", false, false);
		this.guiData.wipe.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, delegate
		{
			base.gameObject.SetActive(false);
		});
		this.currentTriggerCallback = cb;
		this.trigger = this.Trigger();
	}

	// Token: 0x06001B7D RID: 7037 RVA: 0x0015F7AE File Offset: 0x0015D9AE
	public bool IsPlaying()
	{
		return this.guiData.wipe.ExIsPlaying();
	}

	// Token: 0x06001B7E RID: 7038 RVA: 0x0015F7C0 File Offset: 0x0015D9C0
	public bool IsPlayingTrigger()
	{
		return this.IsPlaying() && this.trigger == null;
	}

	// Token: 0x06001B7F RID: 7039 RVA: 0x0015F7D5 File Offset: 0x0015D9D5
	public bool IsActive()
	{
		return base.gameObject.activeSelf;
	}

	// Token: 0x06001B80 RID: 7040 RVA: 0x0015F7E2 File Offset: 0x0015D9E2
	private void Start()
	{
	}

	// Token: 0x06001B81 RID: 7041 RVA: 0x0015F7E4 File Offset: 0x0015D9E4
	private void Update()
	{
		if (this.trigger != null && !this.trigger.MoveNext())
		{
			this.trigger = null;
		}
		if (this.currentTriggerCallback != null && this.IsPlayingTrigger())
		{
			DressUpWipeCtrl.TriggerCallback triggerCallback = this.currentTriggerCallback;
			this.currentTriggerCallback = null;
			triggerCallback();
		}
	}

	// Token: 0x06001B82 RID: 7042 RVA: 0x0015F824 File Offset: 0x0015DA24
	private IEnumerator Trigger()
	{
		IEnumerator wait = this.Wait(0.3f);
		while (wait.MoveNext())
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001B83 RID: 7043 RVA: 0x0015F833 File Offset: 0x0015DA33
	private IEnumerator Wait(float second)
	{
		float timeSinceStartup = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup - timeSinceStartup < second)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x040014A8 RID: 5288
	private DressUpWipeCtrl.TriggerCallback currentTriggerCallback;

	// Token: 0x040014A9 RID: 5289
	private DressUpWipeCtrl.GUI guiData;

	// Token: 0x040014AA RID: 5290
	private IEnumerator trigger;

	// Token: 0x02000EC2 RID: 3778
	public class GUI
	{
		// Token: 0x06004DAB RID: 19883 RVA: 0x00233CE0 File Offset: 0x00231EE0
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.wipe = baseTr.GetComponent<SimpleAnimation>();
		}

		// Token: 0x040054A1 RID: 21665
		public GameObject baseObj;

		// Token: 0x040054A2 RID: 21666
		public SimpleAnimation wipe;
	}

	// Token: 0x02000EC3 RID: 3779
	// (Invoke) Token: 0x06004DAD RID: 19885
	public delegate void TriggerCallback();
}
