using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020001AD RID: 429
public class MarkLockCtrl : MonoBehaviour
{
	// Token: 0x17000403 RID: 1027
	// (get) Token: 0x06001CE5 RID: 7397 RVA: 0x0016A636 File Offset: 0x00168836
	// (set) Token: 0x06001CE6 RID: 7398 RVA: 0x0016A63E File Offset: 0x0016883E
	private MarkLockCtrl.GUI GuiData { get; set; }

	// Token: 0x17000404 RID: 1028
	// (get) Token: 0x06001CE7 RID: 7399 RVA: 0x0016A647 File Offset: 0x00168847
	// (set) Token: 0x06001CE8 RID: 7400 RVA: 0x0016A64F File Offset: 0x0016884F
	private bool IsInit { get; set; }

	// Token: 0x06001CE9 RID: 7401 RVA: 0x0016A658 File Offset: 0x00168858
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

	// Token: 0x06001CEA RID: 7402 RVA: 0x0016A68F File Offset: 0x0016888F
	public void StartAE()
	{
		this.aeCtrl = this.AECtrl();
	}

	// Token: 0x06001CEB RID: 7403 RVA: 0x0016A69D File Offset: 0x0016889D
	public void StartAEForce()
	{
		this.aeCtrl = this.AEForceCtrl();
	}

	// Token: 0x06001CEC RID: 7404 RVA: 0x0016A6AB File Offset: 0x001688AB
	public bool IsActive()
	{
		return this.GuiData.baseObj.activeSelf;
	}

	// Token: 0x06001CED RID: 7405 RVA: 0x0016A6BD File Offset: 0x001688BD
	public void SetActive(bool sw)
	{
		this.GuiData.baseObj.SetActive(sw);
		this.GuiData.ae.PauseAnimation(PguiAECtrl.AmimeType.START, null);
	}

	// Token: 0x06001CEE RID: 7406 RVA: 0x0016A6E2 File Offset: 0x001688E2
	public void SetText(string text)
	{
		this.GuiData.txt.text = text;
	}

	// Token: 0x06001CEF RID: 7407 RVA: 0x0016A6F5 File Offset: 0x001688F5
	public string GetText()
	{
		return this.GuiData.txt.text;
	}

	// Token: 0x06001CF0 RID: 7408 RVA: 0x0016A707 File Offset: 0x00168907
	private void InitForce()
	{
		this.GuiData = new MarkLockCtrl.GUI(base.transform);
		this.IsInit = true;
	}

	// Token: 0x06001CF1 RID: 7409 RVA: 0x0016A721 File Offset: 0x00168921
	private void Awake()
	{
		this.InitForce();
	}

	// Token: 0x06001CF2 RID: 7410 RVA: 0x0016A729 File Offset: 0x00168929
	private void Start()
	{
	}

	// Token: 0x06001CF3 RID: 7411 RVA: 0x0016A72B File Offset: 0x0016892B
	private void Update()
	{
		if (this.aeCtrl != null && !this.aeCtrl.MoveNext())
		{
			this.aeCtrl = null;
		}
	}

	// Token: 0x06001CF4 RID: 7412 RVA: 0x0016A74C File Offset: 0x0016894C
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

	// Token: 0x06001CF5 RID: 7413 RVA: 0x0016A818 File Offset: 0x00168A18
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

	// Token: 0x06001CF6 RID: 7414 RVA: 0x0016A8AD File Offset: 0x00168AAD
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

	// Token: 0x06001CF7 RID: 7415 RVA: 0x0016A8BC File Offset: 0x00168ABC
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

	// Token: 0x04001579 RID: 5497
	private bool nextSequence;

	// Token: 0x0400157A RID: 5498
	private MarkLockCtrl.SetupParam setupParam = new MarkLockCtrl.SetupParam();

	// Token: 0x0400157B RID: 5499
	private IEnumerator aeCtrl;

	// Token: 0x02000F2F RID: 3887
	// (Invoke) Token: 0x06004EDD RID: 20189
	public delegate bool ConditionCallback();

	// Token: 0x02000F30 RID: 3888
	public class SetupParam
	{
		// Token: 0x04005636 RID: 22070
		public bool releaseFlag;

		// Token: 0x04005637 RID: 22071
		public GameObject tagetObject;

		// Token: 0x04005638 RID: 22072
		public string text;

		// Token: 0x04005639 RID: 22073
		public UnityAction updateUserFlagDataCallback;

		// Token: 0x0400563A RID: 22074
		public MarkLockCtrl.ConditionCallback updateConditionCallback;

		// Token: 0x0400563B RID: 22075
		public List<string> ignoreDisableColorChangeList;
	}

	// Token: 0x02000F31 RID: 3889
	private class GUI
	{
		// Token: 0x06004EE1 RID: 20193 RVA: 0x00238004 File Offset: 0x00236204
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.ae = baseTr.GetComponent<PguiAECtrl>();
			this.txt = baseTr.Find("Txt_LockInfo").GetComponent<PguiTextCtrl>();
			this.ae.PauseAnimation(PguiAECtrl.AmimeType.START, null);
		}

		// Token: 0x0400563C RID: 22076
		public GameObject baseObj;

		// Token: 0x0400563D RID: 22077
		public PguiAECtrl ae;

		// Token: 0x0400563E RID: 22078
		public PguiTextCtrl txt;
	}
}
