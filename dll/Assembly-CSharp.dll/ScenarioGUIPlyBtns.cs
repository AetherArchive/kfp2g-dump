using System;
using AEAuth3;
using UnityEngine;

public class ScenarioGUIPlyBtns : MonoBehaviour
{
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

	private void OnClickTimeSwithBtn(PguiButtonCtrl button)
	{
		this.bOpenFlag = !this.bOpenFlag;
		this.bAnim = true;
	}

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

	private void OnClickSkip(PguiButtonCtrl button)
	{
		this.bSkipWindowShow = !this.bSkipWindowShow;
	}

	private void OnClickLog(PguiButtonCtrl button)
	{
		this.bLogWindowShow = !this.bLogWindowShow;
	}

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

	public bool IsFadeEnd()
	{
		return !this.mFade.GetComponent<SimpleAnimation>().ExIsPlaying();
	}

	public void DispOff()
	{
		this.mSimpleAnimTimeSwith.gameObject.SetActive(false);
		this.mAutoStopBtn.SetActive(false);
		this.mMarkAuto.SetActive(false);
	}

	private GameObject mTimeSwitchBtn;

	private SimpleAnimation mSimpleAnimTimeSwith;

	private GameObject mAutoStartBtn;

	public bool bAutoFlag;

	private GameObject mAutoStopBtn;

	private GameObject mMarkAuto;

	private GameObject mSkipBtn;

	public bool bSkipWindowShow;

	private GameObject mLogBtn;

	public bool bLogWindowShow;

	public bool bOpenFlag;

	private bool bAnim;

	public GameObject mFade;

	public AEImage mNoMark;
}
