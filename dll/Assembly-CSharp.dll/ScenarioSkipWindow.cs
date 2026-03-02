using System;
using UnityEngine;

// Token: 0x02000115 RID: 277
public class ScenarioSkipWindow : MonoBehaviour
{
	// Token: 0x06000D60 RID: 3424 RVA: 0x00058AF0 File Offset: 0x00056CF0
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

	// Token: 0x06000D61 RID: 3425 RVA: 0x00058BFC File Offset: 0x00056DFC
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

	// Token: 0x06000D62 RID: 3426 RVA: 0x00058CB5 File Offset: 0x00056EB5
	private void OnClickSkipBtn(PguiButtonCtrl button)
	{
		this.mIsSkip = true;
		SoundManager.Play("prd_se_decide", false, false);
	}

	// Token: 0x06000D63 RID: 3427 RVA: 0x00058CCB File Offset: 0x00056ECB
	private void OnClickStoryBtn(PguiButtonCtrl button)
	{
		this.mIsStory = true;
		this.sGuiPlyBtn.bSkipWindowShow = false;
	}

	// Token: 0x06000D64 RID: 3428 RVA: 0x00058CE0 File Offset: 0x00056EE0
	private void OnClickCancelBtn(PguiButtonCtrl button)
	{
		this.sGuiPlyBtn.bSkipWindowShow = false;
		SoundManager.Play("prd_se_cancel", false, false);
	}

	// Token: 0x04000AF5 RID: 2805
	public ScenarioGUIPlyBtns sGuiPlyBtn;

	// Token: 0x04000AF6 RID: 2806
	private GameObject mBase;

	// Token: 0x04000AF7 RID: 2807
	private Transform mWindow;

	// Token: 0x04000AF8 RID: 2808
	private bool bAnim;

	// Token: 0x04000AF9 RID: 2809
	public bool mIsSkip;

	// Token: 0x04000AFA RID: 2810
	public bool mIsStory;
}
