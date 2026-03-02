using System;
using AEAuth3;
using UnityEngine;

// Token: 0x02000110 RID: 272
public class ScenarioGUIPlyBtns : MonoBehaviour
{
	// Token: 0x06000D08 RID: 3336 RVA: 0x00051D08 File Offset: 0x0004FF08
	private void Start()
	{
		this.mTimeSwitchBtn = base.transform.Find("TimeBtnsAll/Btn_Yaji").gameObject;
		this.mSimpleAnimTimeSwith = base.transform.Find("TimeBtnsAll").GetComponent<SimpleAnimation>();
		this.mAutoStartBtn = base.transform.Find("TimeBtnsAll/Btn_Auto").gameObject;
		this.bAutoFlag = false;
		this.mAutoStopBtn = base.transform.Find("Btn_Stop").transform.gameObject;
		this.mMarkAuto = base.transform.Find("Mark_Auto").gameObject;
		this.mSkipBtn = base.transform.Find("TimeBtnsAll/Btn_Skip").gameObject;
		this.mLogBtn = base.transform.Find("TimeBtnsAll/Btn_Log").gameObject;
		this.mAutoStopBtn.SetActive(false);
		this.mMarkAuto.SetActive(false);
		this.mTimeSwitchBtn.GetComponent<PguiButtonCtrl>().AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickTimeSwithBtn), PguiButtonCtrl.SoundType.DEFAULT);
		this.mAutoStartBtn.GetComponent<PguiButtonCtrl>().AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickAutoSwithBtn), PguiButtonCtrl.SoundType.DEFAULT);
		this.mAutoStopBtn.GetComponent<PguiButtonCtrl>().AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickAutoSwithBtn), PguiButtonCtrl.SoundType.DEFAULT);
		this.mSkipBtn.GetComponent<PguiButtonCtrl>().AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickSkip), PguiButtonCtrl.SoundType.DEFAULT);
		this.mLogBtn.GetComponent<PguiButtonCtrl>().AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickLog), PguiButtonCtrl.SoundType.DEFAULT);
		this.mFade = base.transform.Find("Eff_AllFade").gameObject;
		this.mNoMark = base.transform.Find("Eff_AllFade_NoMark").GetComponent<AEImage>();
		this.mNoMark.autoPlay = false;
		this.mNoMark.playLoop = false;
		this.mNoMark.playTime = (this.mNoMark.playInTime = 0f);
		this.mNoMark.playOutTime = this.mNoMark.duration;
		this.SetFade(false, ScenarioDefine.FADE_TYPE.WHITE_OUT, true);
	}

	// Token: 0x06000D09 RID: 3337 RVA: 0x00051F14 File Offset: 0x00050114
	private void Update()
	{
		if (this.bAnim)
		{
			if (this.bOpenFlag)
			{
				this.mSimpleAnimTimeSwith.Play("START");
			}
			else
			{
				this.mSimpleAnimTimeSwith.Play("END");
			}
			this.bAnim = !this.bAnim;
		}
		float num = this.mTimeSwitchBtn.transform.localScale.x;
		float num2 = TimeManager.DeltaTime * 9f;
		if (this.bOpenFlag)
		{
			if ((num -= num2) < -1f)
			{
				num = -1f;
			}
		}
		else if ((num += num2) > 1f)
		{
			num = 1f;
		}
		this.mTimeSwitchBtn.transform.localScale = new Vector3(num, 1f, 1f);
		this.mTimeSwitchBtn.GetComponent<PguiButtonCtrl>().androidBackKeyTarget = !this.bLogWindowShow;
	}

	// Token: 0x06000D0A RID: 3338 RVA: 0x00051FED File Offset: 0x000501ED
	private void OnClickTimeSwithBtn(PguiButtonCtrl button)
	{
		this.bOpenFlag = !this.bOpenFlag;
		this.bAnim = true;
	}

	// Token: 0x06000D0B RID: 3339 RVA: 0x00052008 File Offset: 0x00050208
	private void OnClickAutoSwithBtn(PguiButtonCtrl button)
	{
		this.bAutoFlag = !this.bAutoFlag;
		if (this.bAutoFlag)
		{
			this.mAutoStartBtn.transform.Find("Txt").GetComponent<PguiTextCtrl>().text = "停止";
		}
		else
		{
			this.mAutoStartBtn.transform.Find("Txt").GetComponent<PguiTextCtrl>().text = "オート";
		}
		this.mAutoStopBtn.SetActive(this.bAutoFlag);
		this.mMarkAuto.SetActive(this.bAutoFlag);
	}

	// Token: 0x06000D0C RID: 3340 RVA: 0x00052098 File Offset: 0x00050298
	private void OnClickSkip(PguiButtonCtrl button)
	{
		this.bSkipWindowShow = !this.bSkipWindowShow;
	}

	// Token: 0x06000D0D RID: 3341 RVA: 0x000520A9 File Offset: 0x000502A9
	private void OnClickLog(PguiButtonCtrl button)
	{
		this.bLogWindowShow = !this.bLogWindowShow;
	}

	// Token: 0x06000D0E RID: 3342 RVA: 0x000520BC File Offset: 0x000502BC
	public void SetFade(bool isFront, ScenarioDefine.FADE_TYPE type, bool immediate = false)
	{
		if (isFront)
		{
			this.mFade.transform.SetAsFirstSibling();
		}
		else
		{
			this.mFade.transform.SetAsLastSibling();
		}
		this.mNoMark.autoPlay = false;
		if (type == ScenarioDefine.FADE_TYPE.KEMONO_FADE)
		{
			this.mNoMark.autoPlay = true;
			this.mNoMark.playTime = (immediate ? this.mNoMark.duration : 0f);
			return;
		}
		switch (type)
		{
		case ScenarioDefine.FADE_TYPE.BLACK_IN:
		case ScenarioDefine.FADE_TYPE.BLACK_OUT:
			this.mFade.GetComponent<PguiImageCtrl>().m_Image.color = Color.black;
			break;
		case ScenarioDefine.FADE_TYPE.WHITE_IN:
		case ScenarioDefine.FADE_TYPE.WHITE_OUT:
			this.mFade.GetComponent<PguiImageCtrl>().m_Image.color = Color.white;
			break;
		case ScenarioDefine.FADE_TYPE.RED_IN:
		case ScenarioDefine.FADE_TYPE.RED_OUT:
			this.mFade.GetComponent<PguiImageCtrl>().m_Image.color = Color.red;
			break;
		}
		switch (type)
		{
		case ScenarioDefine.FADE_TYPE.BLACK_IN:
		case ScenarioDefine.FADE_TYPE.WHITE_IN:
		case ScenarioDefine.FADE_TYPE.RED_IN:
			this.mFade.GetComponent<SimpleAnimation>().ExPlayAnimation("START", immediate ? 10f : 0f, immediate ? 10f : 1f);
			return;
		case ScenarioDefine.FADE_TYPE.BLACK_OUT:
		case ScenarioDefine.FADE_TYPE.WHITE_OUT:
		case ScenarioDefine.FADE_TYPE.RED_OUT:
			this.mFade.GetComponent<SimpleAnimation>().ExPlayAnimation("END", immediate ? 10f : 0f, immediate ? 10f : 1f);
			return;
		default:
			return;
		}
	}

	// Token: 0x06000D0F RID: 3343 RVA: 0x00052227 File Offset: 0x00050427
	public bool IsFadeEnd()
	{
		return !this.mFade.GetComponent<SimpleAnimation>().ExIsPlaying();
	}

	// Token: 0x06000D10 RID: 3344 RVA: 0x0005223C File Offset: 0x0005043C
	public void DispOff()
	{
		this.mSimpleAnimTimeSwith.gameObject.SetActive(false);
		this.mAutoStopBtn.SetActive(false);
		this.mMarkAuto.SetActive(false);
	}

	// Token: 0x04000A72 RID: 2674
	private GameObject mTimeSwitchBtn;

	// Token: 0x04000A73 RID: 2675
	private SimpleAnimation mSimpleAnimTimeSwith;

	// Token: 0x04000A74 RID: 2676
	private GameObject mAutoStartBtn;

	// Token: 0x04000A75 RID: 2677
	public bool bAutoFlag;

	// Token: 0x04000A76 RID: 2678
	private GameObject mAutoStopBtn;

	// Token: 0x04000A77 RID: 2679
	private GameObject mMarkAuto;

	// Token: 0x04000A78 RID: 2680
	private GameObject mSkipBtn;

	// Token: 0x04000A79 RID: 2681
	public bool bSkipWindowShow;

	// Token: 0x04000A7A RID: 2682
	private GameObject mLogBtn;

	// Token: 0x04000A7B RID: 2683
	public bool bLogWindowShow;

	// Token: 0x04000A7C RID: 2684
	public bool bOpenFlag;

	// Token: 0x04000A7D RID: 2685
	private bool bAnim;

	// Token: 0x04000A7E RID: 2686
	public GameObject mFade;

	// Token: 0x04000A7F RID: 2687
	public AEImage mNoMark;
}
