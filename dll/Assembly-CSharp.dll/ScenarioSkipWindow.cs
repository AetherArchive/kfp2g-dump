using System;
using UnityEngine;

public class ScenarioSkipWindow : MonoBehaviour
{
	private void Start()
	{
		this.mBase = base.transform.Find("Window_Skip").transform.Find("Base").gameObject;
		this.mWindow = this.mBase.transform.Find("Window");
		this.mWindow.Find("ButtonL").GetComponent<PguiButtonCtrl>().AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickCancelBtn), PguiButtonCtrl.SoundType.DEFAULT);
		this.mWindow.Find("ButtonC").GetComponent<PguiButtonCtrl>().AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickStoryBtn), PguiButtonCtrl.SoundType.DEFAULT);
		this.mWindow.Find("ButtonR").GetComponent<PguiButtonCtrl>().AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickSkipBtn), PguiButtonCtrl.SoundType.DEFAULT);
		this.mWindow.Find("BtnClose").GetComponent<PguiButtonCtrl>().AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickCancelBtn), PguiButtonCtrl.SoundType.DEFAULT);
		this.mWindow.Find("BtnClose").GetComponent<PguiButtonCtrl>().androidBackKeyTarget = true;
	}

	private void Update()
	{
		if (this.sGuiPlyBtn.bSkipWindowShow && !this.mBase.activeSelf)
		{
			this.mBase.SetActive(true);
			this.mBase.GetComponent<SimpleAnimation>().Play("Open");
		}
		else if (!this.sGuiPlyBtn.bSkipWindowShow && this.mBase.activeSelf)
		{
			this.mBase.GetComponent<SimpleAnimation>().Play("Close");
			this.bAnim = true;
		}
		if (this.bAnim && this.mWindow.GetComponent<RectTransform>().localScale.x <= 0f)
		{
			this.mBase.SetActive(false);
			this.bAnim = false;
		}
	}

	private void OnClickSkipBtn(PguiButtonCtrl button)
	{
		this.mIsSkip = true;
		SoundManager.Play("prd_se_decide", false, false);
	}

	private void OnClickStoryBtn(PguiButtonCtrl button)
	{
		this.mIsStory = true;
		this.sGuiPlyBtn.bSkipWindowShow = false;
	}

	private void OnClickCancelBtn(PguiButtonCtrl button)
	{
		this.sGuiPlyBtn.bSkipWindowShow = false;
		SoundManager.Play("prd_se_cancel", false, false);
	}

	public ScenarioGUIPlyBtns sGuiPlyBtn;

	private GameObject mBase;

	private Transform mWindow;

	private bool bAnim;

	public bool mIsSkip;

	public bool mIsStory;
}
