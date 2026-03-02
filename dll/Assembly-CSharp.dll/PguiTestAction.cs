using System;
using System.Collections.Generic;
using SGNFW.Common;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020001E3 RID: 483
public class PguiTestAction : MonoBehaviour
{
	// Token: 0x0600204E RID: 8270 RVA: 0x0018AA20 File Offset: 0x00188C20
	private void Start()
	{
		if (this.resuseScroll != null)
		{
			this.resuseScroll.InitForce();
			ReuseScroll reuseScroll = this.resuseScroll;
			reuseScroll.onStartItem = (Action<int, GameObject>)Delegate.Combine(reuseScroll.onStartItem, new Action<int, GameObject>(this.onStartItem));
			ReuseScroll reuseScroll2 = this.resuseScroll;
			reuseScroll2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(reuseScroll2.onUpdateItem, new Action<int, GameObject>(this.onUpdateItem));
			this.resuseScroll.Setup(15, 0);
		}
		if (this.pguiTabGroupCtrl != null)
		{
			this.pguiTabGroupCtrl.Setup(0, new PguiTabGroupCtrl.OnSelectTab(this.onSelectTab));
		}
		if (this.pguiToggleButtonCtrl != null)
		{
			this.pguiToggleButtonCtrl.AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickToggle));
		}
		CanvasManager.Initialize();
		CanvasManager.DisbleCanvasByTestScene();
		Singleton<SoundManager>.Instance.Initialize();
		if (this.connectEnable)
		{
			Singleton<DataManager>.Instance.InitializeByEditor(delegate
			{
				if (this.connectFinishCb != null)
				{
					this.connectFinishCb.Invoke();
				}
			});
		}
	}

	// Token: 0x0600204F RID: 8271 RVA: 0x0018AB1F File Offset: 0x00188D1F
	private bool OnClickToggle(PguiToggleButtonCtrl ptbc, int index)
	{
		return true;
	}

	// Token: 0x06002050 RID: 8272 RVA: 0x0018AB24 File Offset: 0x00188D24
	private void Update()
	{
		if (this.pguiOpenWindowCtrl != null && Input.GetKeyDown(KeyCode.O))
		{
			this.pguiOpenWindowCtrl.Setup("タイトル", "メッセージ\nああああ\n\nおおおおおお", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, new PguiOpenWindowCtrl.Callback(this.OnChoiceOpenWindow), null, false);
			this.pguiOpenWindowCtrl.Open();
		}
		if (this.pguiButtonCtrl != null && Input.GetKeyDown(KeyCode.E))
		{
			this.pguiButtonCtrl.SetActEnable(!this.pguiButtonCtrl.ActEnable, false, false);
		}
		if (this.animator != null && Input.GetKeyDown(KeyCode.T))
		{
			this.animator.SetTrigger("Start");
		}
		if (this.pguiScrollTextCtrl != null && Input.GetKeyDown(KeyCode.S))
		{
			string text = "あいうえお\r\nかきくけこ\r\nさしすせそ\r\nたちつてと\r\nなにぬねの\r\nはひふへほ\r\nまみむめも\r\nやゆよ\r\nわおん\r\n\r\nアイウエオ\r\nカキクケコ\r\nサシスセソ\r\nタチツテト\r\nナニヌネノ\r\nハヒフヘホ\r\nマミムメモ\r\nヤユヨ\r\nワオン";
			this.pguiScrollTextCtrl.SetText(text);
		}
		if (this.simpleAnimation != null && Input.GetKeyDown(KeyCode.Alpha1))
		{
			this.simpleAnimation.Rewind();
			this.simpleAnimation.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
		}
		if (this.simpleAnimation != null && Input.GetKeyDown(KeyCode.Alpha2))
		{
			this.simpleAnimation.ExPlayAnimation(SimpleAnimation.ExPguiStatus.LOOP, null);
		}
		if (this.simpleAnimation != null && Input.GetKeyDown(KeyCode.Alpha3))
		{
			this.simpleAnimation.ExPauseAnimation(SimpleAnimation.ExPguiStatus.END, null);
		}
		if (this.simpleAnimation != null && Input.GetKeyDown(KeyCode.Alpha4))
		{
			this.simpleAnimation.ExPauseAnimation(SimpleAnimation.ExPguiStatus.END, null);
		}
		if (this.simpleAnimation != null && Input.GetKeyDown(KeyCode.Alpha5))
		{
			this.simpleAnimation.ExResumeAnimation(null);
		}
		if (this.pguiAECtrl != null && Input.GetKeyDown(KeyCode.Alpha1))
		{
			this.pguiAECtrl.PlayAnimation(PguiAECtrl.AmimeType.START, null);
		}
		if (this.pguiAECtrl != null && Input.GetKeyDown(KeyCode.Alpha2))
		{
			this.pguiAECtrl.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
		}
		if (this.pguiAECtrl != null && Input.GetKeyDown(KeyCode.Alpha3))
		{
			this.pguiAECtrl.PlayAnimation(PguiAECtrl.AmimeType.END, null);
		}
		if (this.pguiAECtrl != null && Input.GetKeyDown(KeyCode.Alpha4))
		{
			this.pguiAECtrl.PauseAnimation(PguiAECtrl.AmimeType.END, null);
		}
		if (this.pguiAECtrl != null && Input.GetKeyDown(KeyCode.Alpha5))
		{
			this.pguiAECtrl.ResumeAnimation();
		}
		if (Input.GetKeyDown(KeyCode.P))
		{
			foreach (IconPhotoCtrl iconPhotoCtrl in this.iconPhotoCtrl)
			{
				iconPhotoCtrl.Setup(PhotoPackData.MakeDummy(1L, 2001), SortFilterDefine.SortType.LEVEL, true, false, -1, false);
			}
		}
	}

	// Token: 0x06002051 RID: 8273 RVA: 0x0018ADCC File Offset: 0x00188FCC
	private bool OnChoiceOpenWindow(int index)
	{
		return true;
	}

	// Token: 0x06002052 RID: 8274 RVA: 0x0018ADCF File Offset: 0x00188FCF
	public void OnChange(string str)
	{
	}

	// Token: 0x06002053 RID: 8275 RVA: 0x0018ADD1 File Offset: 0x00188FD1
	private void onStartItem(int index, GameObject go)
	{
	}

	// Token: 0x06002054 RID: 8276 RVA: 0x0018ADD3 File Offset: 0x00188FD3
	private void onUpdateItem(int index, GameObject go)
	{
	}

	// Token: 0x06002055 RID: 8277 RVA: 0x0018ADD5 File Offset: 0x00188FD5
	private bool onSelectTab(int index)
	{
		return true;
	}

	// Token: 0x0400176D RID: 5997
	public ReuseScroll resuseScroll;

	// Token: 0x0400176E RID: 5998
	public PguiTabGroupCtrl pguiTabGroupCtrl;

	// Token: 0x0400176F RID: 5999
	public PguiOpenWindowCtrl pguiOpenWindowCtrl;

	// Token: 0x04001770 RID: 6000
	public PguiButtonCtrl pguiButtonCtrl;

	// Token: 0x04001771 RID: 6001
	public Animator animator;

	// Token: 0x04001772 RID: 6002
	public SimpleAnimation simpleAnimation;

	// Token: 0x04001773 RID: 6003
	public PguiAECtrl pguiAECtrl;

	// Token: 0x04001774 RID: 6004
	public PguiToggleButtonCtrl pguiToggleButtonCtrl;

	// Token: 0x04001775 RID: 6005
	public PguiScrollText pguiScrollTextCtrl;

	// Token: 0x04001776 RID: 6006
	public List<IconPhotoCtrl> iconPhotoCtrl;

	// Token: 0x04001777 RID: 6007
	public bool connectEnable;

	// Token: 0x04001778 RID: 6008
	public UnityEvent connectFinishCb;
}
