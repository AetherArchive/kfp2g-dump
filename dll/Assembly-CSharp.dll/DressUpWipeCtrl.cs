using System;
using System.Collections;
using UnityEngine;

public class DressUpWipeCtrl : MonoBehaviour
{
	public void Init()
	{
		if (this.guiData != null)
		{
			return;
		}
		this.guiData = new DressUpWipeCtrl.GUI(base.transform);
		base.gameObject.SetActive(false);
	}

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

	public bool IsPlaying()
	{
		return this.guiData.wipe.ExIsPlaying();
	}

	public bool IsPlayingTrigger()
	{
		return this.IsPlaying() && this.trigger == null;
	}

	public bool IsActive()
	{
		return base.gameObject.activeSelf;
	}

	private void Start()
	{
	}

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

	private IEnumerator Trigger()
	{
		IEnumerator wait = this.Wait(0.3f);
		while (wait.MoveNext())
		{
			yield return null;
		}
		yield break;
	}

	private IEnumerator Wait(float second)
	{
		float timeSinceStartup = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup - timeSinceStartup < second)
		{
			yield return null;
		}
		yield break;
	}

	private DressUpWipeCtrl.TriggerCallback currentTriggerCallback;

	private DressUpWipeCtrl.GUI guiData;

	private IEnumerator trigger;

	public class GUI
	{
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.wipe = baseTr.GetComponent<SimpleAnimation>();
		}

		public GameObject baseObj;

		public SimpleAnimation wipe;
	}

	public delegate void TriggerCallback();
}
