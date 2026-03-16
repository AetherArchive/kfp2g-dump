using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MarkLockCtrl : MonoBehaviour
{
	private MarkLockCtrl.GUI GuiData { get; set; }

	private bool IsInit { get; set; }

	public void Setup(MarkLockCtrl.SetupParam param, bool startAE = true)
	{
		if (!this.IsInit)
		{
			this.InitForce();
		}
		this.setupParam = param;
		this.SetText(this.setupParam.text);
		this.SetActEnable();
		if (startAE)
		{
			this.StartAE();
		}
	}

	public void StartAE()
	{
		this.aeCtrl = this.AECtrl();
	}

	public void StartAEForce()
	{
		this.aeCtrl = this.AEForceCtrl();
	}

	public bool IsActive()
	{
		return this.GuiData.baseObj.activeSelf;
	}

	public void SetActive(bool sw)
	{
		this.GuiData.baseObj.SetActive(sw);
		this.GuiData.ae.PauseAnimation(PguiAECtrl.AmimeType.START, null);
	}

	public void SetText(string text)
	{
		this.GuiData.txt.text = text;
	}

	public string GetText()
	{
		return this.GuiData.txt.text;
	}

	private void InitForce()
	{
		this.GuiData = new MarkLockCtrl.GUI(base.transform);
		this.IsInit = true;
	}

	private void Awake()
	{
		this.InitForce();
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (this.aeCtrl != null && !this.aeCtrl.MoveNext())
		{
			this.aeCtrl = null;
		}
	}

	private void SetActEnable()
	{
		if (this.setupParam.tagetObject == null)
		{
			bool flag = !this.setupParam.updateConditionCallback();
			this.SetActive(flag);
			return;
		}
		PguiButtonCtrl component = this.setupParam.tagetObject.GetComponent<PguiButtonCtrl>();
		if (!(component != null))
		{
			bool flag2 = !this.setupParam.updateConditionCallback();
			this.setupParam.tagetObject.SetActive(flag2);
			this.SetActive(flag2);
			return;
		}
		bool flag3 = !this.setupParam.releaseFlag;
		this.SetActive(flag3);
		if (this.setupParam.ignoreDisableColorChangeList == null)
		{
			component.SetActEnable(!flag3, flag3, false);
			return;
		}
		component.SetActEnable(!flag3, this.setupParam.ignoreDisableColorChangeList, flag3);
	}

	private void ReleaseButton()
	{
		if (!(this.setupParam.tagetObject == null))
		{
			PguiButtonCtrl component = this.setupParam.tagetObject.GetComponent<PguiButtonCtrl>();
			if (component != null)
			{
				if (this.setupParam.ignoreDisableColorChangeList == null)
				{
					component.SetActEnable(!this.IsActive(), this.IsActive(), false);
					return;
				}
				component.SetActEnable(!this.IsActive(), this.setupParam.ignoreDisableColorChangeList, this.IsActive());
				return;
			}
			else
			{
				this.setupParam.tagetObject.SetActive(this.IsActive());
			}
		}
	}

	private IEnumerator AECtrl()
	{
		this.nextSequence = false;
		this.SetActive(true);
		if (!this.setupParam.releaseFlag && this.setupParam.updateConditionCallback())
		{
			SoundManager.Play("prd_se_scenario_unlock", false, false);
			this.GuiData.ae.PlayAnimation(PguiAECtrl.AmimeType.START, delegate
			{
				this.GuiData.ae.PlayAnimation(PguiAECtrl.AmimeType.END, delegate
				{
					this.nextSequence = true;
				});
			});
			while (!this.nextSequence)
			{
				yield return null;
			}
			UnityAction updateUserFlagDataCallback = this.setupParam.updateUserFlagDataCallback;
			if (updateUserFlagDataCallback != null)
			{
				updateUserFlagDataCallback();
			}
			this.SetActive(false);
		}
		if (this.IsActive() && this.setupParam.releaseFlag && this.setupParam.updateConditionCallback())
		{
			this.SetActive(false);
		}
		this.ReleaseButton();
		yield return null;
		yield break;
	}

	private IEnumerator AEForceCtrl()
	{
		CanvasManager.SetEnableCmnTouchMask(true);
		this.nextSequence = false;
		this.SetActive(true);
		SoundManager.Play("prd_se_scenario_unlock", false, false);
		this.GuiData.ae.PlayAnimation(PguiAECtrl.AmimeType.START, delegate
		{
			this.GuiData.ae.PlayAnimation(PguiAECtrl.AmimeType.END, delegate
			{
				this.nextSequence = true;
			});
		});
		while (!this.nextSequence)
		{
			yield return null;
		}
		UnityAction updateUserFlagDataCallback = this.setupParam.updateUserFlagDataCallback;
		if (updateUserFlagDataCallback != null)
		{
			updateUserFlagDataCallback();
		}
		this.SetActive(false);
		this.ReleaseButton();
		CanvasManager.SetEnableCmnTouchMask(false);
		yield break;
	}

	private bool nextSequence;

	private MarkLockCtrl.SetupParam setupParam = new MarkLockCtrl.SetupParam();

	private IEnumerator aeCtrl;

	public delegate bool ConditionCallback();

	public class SetupParam
	{
		public bool releaseFlag;

		public GameObject tagetObject;

		public string text;

		public UnityAction updateUserFlagDataCallback;

		public MarkLockCtrl.ConditionCallback updateConditionCallback;

		public List<string> ignoreDisableColorChangeList;
	}

	private class GUI
	{
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.ae = baseTr.GetComponent<PguiAECtrl>();
			this.txt = baseTr.Find("Txt_LockInfo").GetComponent<PguiTextCtrl>();
			this.ae.PauseAnimation(PguiAECtrl.AmimeType.START, null);
		}

		public GameObject baseObj;

		public PguiAECtrl ae;

		public PguiTextCtrl txt;
	}
}
