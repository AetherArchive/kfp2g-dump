using System;
using System.Collections.Generic;
using SGNFW.Common;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;

public class PguiTestAction : MonoBehaviour
{
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

	private bool OnClickToggle(PguiToggleButtonCtrl ptbc, int index)
	{
		return true;
	}

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

	private bool OnChoiceOpenWindow(int index)
	{
		return true;
	}

	public void OnChange(string str)
	{
	}

	private void onStartItem(int index, GameObject go)
	{
	}

	private void onUpdateItem(int index, GameObject go)
	{
	}

	private bool onSelectTab(int index)
	{
		return true;
	}

	public ReuseScroll resuseScroll;

	public PguiTabGroupCtrl pguiTabGroupCtrl;

	public PguiOpenWindowCtrl pguiOpenWindowCtrl;

	public PguiButtonCtrl pguiButtonCtrl;

	public Animator animator;

	public SimpleAnimation simpleAnimation;

	public PguiAECtrl pguiAECtrl;

	public PguiToggleButtonCtrl pguiToggleButtonCtrl;

	public PguiScrollText pguiScrollTextCtrl;

	public List<IconPhotoCtrl> iconPhotoCtrl;

	public bool connectEnable;

	public UnityEvent connectFinishCb;
}
