using System;
using System.Collections;
using SGNFW.Common;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000188 RID: 392
public class SelTutorialFirstCtrl : MonoBehaviour
{
	// Token: 0x06001A3B RID: 6715 RVA: 0x00154D0A File Offset: 0x00152F0A
	public bool TutorialEnd()
	{
		return this.currentMode == SelTutorialFirstCtrl.Mode.END;
	}

	// Token: 0x06001A3C RID: 6716 RVA: 0x00154D18 File Offset: 0x00152F18
	public bool Init()
	{
		if (!AssetManager.IsLoadFinishAssetData(SelTutorialFirstCtrl.introAsset))
		{
			return false;
		}
		GameObject gameObject = AssetManager.InstantiateAssetData(SelTutorialFirstCtrl.introAsset, base.transform);
		GameObject gameObject2 = Object.Instantiate<GameObject>((GameObject)Resources.Load("Cmn/GUI/Prefab/GUI_ScenarioFade"), base.transform);
		this.guiData = new SelTutorialFirstCtrl.GUI(base.transform);
		this.guiData.introObj = new SelTutorialFirstCtrl.GUI_Intro(gameObject.transform);
		this.guiData.fadeObj = new SelTutorialFirstCtrl.GUI_Fade(gameObject2.transform);
		this.guiData.introObj.baseObj.AddComponent<PguiTouchTrigger>().AddListener(delegate
		{
			if (this.currentMode == SelTutorialFirstCtrl.Mode.INTRO && this.guiData.introObj.Txt_Touch.activeSelf)
			{
				this.touch = true;
			}
		}, null, null, null, null);
		this.selectPhoto = 0;
		this.guiData.baseObj.SetActive(false);
		this.guiData.introObj.baseObj.SetActive(false);
		this.guiData.fadeObj.aeCtrl.PauseAnimation(PguiAECtrl.AmimeType.START, null);
		this.currentMode = SelTutorialFirstCtrl.Mode.INVALID;
		this.requestMode = SelTutorialFirstCtrl.Mode.INTRO;
		this.touch = false;
		this.check = false;
		AssetManager.UnloadAssetData(SelTutorialFirstCtrl.introAsset, AssetManager.OWNER.NameEntry);
		return true;
	}

	// Token: 0x06001A3D RID: 6717 RVA: 0x00154E34 File Offset: 0x00153034
	private IEnumerator Init2()
	{
		AssetManager.LoadAssetData(SelTutorialFirstCtrl.nameAsset, AssetManager.OWNER.NameEntry, 0, null);
		while (!AssetManager.IsLoadFinishAssetData(SelTutorialFirstCtrl.nameAsset))
		{
			yield return null;
		}
		while (!this.guiData.introObj.Txt_Touch.activeSelf && !this.check)
		{
			yield return null;
		}
		AssetManager.LoadAssetData(SelTutorialFirstCtrl.selchrAsset, AssetManager.OWNER.NameEntry, 0, null);
		while (!AssetManager.IsLoadFinishAssetData(SelTutorialFirstCtrl.selchrAsset))
		{
			yield return null;
		}
		while (!this.guiData.introObj.Txt_Touch.activeSelf && !this.check)
		{
			yield return null;
		}
		AssetManager.LoadAssetData(SelTutorialFirstCtrl.concluAsset, AssetManager.OWNER.NameEntry, 0, null);
		while (!AssetManager.IsLoadFinishAssetData(SelTutorialFirstCtrl.concluAsset))
		{
			yield return null;
		}
		while (!this.check)
		{
			yield return null;
		}
		GameObject gameObject = AssetManager.InstantiateAssetData(SelTutorialFirstCtrl.nameAsset, base.transform);
		this.guiData.nameObj = new SelTutorialFirstCtrl.GUI_Name(gameObject.transform);
		this.guiData.nameObj.Btn_OK.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickNameSelectButton), PguiButtonCtrl.SoundType.DECIDE);
		this.guiData.nameObj.Btn_Yes.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickDecideSelectButton), PguiButtonCtrl.SoundType.DECIDE);
		this.guiData.nameObj.Btn_No.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickDecideCancelButton), PguiButtonCtrl.SoundType.CANCEL);
		this.guiData.nameObj.Btn_No.androidBackKeyTarget = true;
		this.guiData.nameObj.inputName.onEndEdit.AddListener(delegate(string str)
		{
			this.guiData.nameObj.inputName.text = PrjUtil.ModifiedName(str);
			this.guiData.nameObj.Txt_Caution.text = "全角10文字以内で入力してください";
			this.guiData.nameObj.Txt_Caution.gameObject.SetActive(true);
		});
		this.guiData.nameObj.baseObj.SetActive(false);
		this.guiData.fadeObj.baseObj.transform.SetAsLastSibling();
		this.guiData.nameObj.Btn_OK.SetActEnable(false, false, false);
		this.guiData.nameObj.Btn_Yes.SetActEnable(false, false, false);
		this.guiData.nameObj.Btn_No.SetActEnable(false, false, false);
		yield return null;
		GameObject gameObject2 = AssetManager.InstantiateAssetData(SelTutorialFirstCtrl.selchrAsset, base.transform);
		this.guiData.selchrObj = new SelTutorialFirstCtrl.GUI_SelChara(gameObject2.transform);
		this.guiData.selchrObj.Btn_Back.AddOnClickListener(delegate(PguiButtonCtrl btn)
		{
			if (this.currentMode == SelTutorialFirstCtrl.Mode.PHOTO && this.selectPhoto >= 0)
			{
				this.selectPhoto = -3;
			}
		}, PguiButtonCtrl.SoundType.CANCEL);
		this.guiData.selchrObj.Btn_Ok.AddOnClickListener(delegate(PguiButtonCtrl btn)
		{
			if (this.currentMode == SelTutorialFirstCtrl.Mode.PHOTO && this.selectPhoto > 0)
			{
				this.selectPhoto = -this.selectPhoto;
			}
		}, PguiButtonCtrl.SoundType.DECIDE);
		this.guiData.selchrObj.Btn_Left.AddOnClickListener(delegate(PguiButtonCtrl btn)
		{
			if (this.currentMode == SelTutorialFirstCtrl.Mode.PHOTO && this.selectPhoto >= 0)
			{
				this.selectPhoto = SelTutorialFirstCtrl.photo2;
			}
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.selchrObj.Btn_Right.AddOnClickListener(delegate(PguiButtonCtrl btn)
		{
			if (this.currentMode == SelTutorialFirstCtrl.Mode.PHOTO && this.selectPhoto >= 0)
			{
				this.selectPhoto = SelTutorialFirstCtrl.photo1;
			}
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.selchrObj.Btn_Back.androidBackKeyTarget = true;
		GameObject gameObject3 = new GameObject("Touch1", new Type[] { typeof(RectTransform) });
		RectTransform component = gameObject3.GetComponent<RectTransform>();
		component.SetParent(this.guiData.selchrObj.baseObj.transform, false);
		component.SetAsFirstSibling();
		component.sizeDelta = new Vector2(827f, 764f);
		component.anchoredPosition = new Vector2(413.5f, 0f);
		gameObject3.AddComponent<PguiCollider>();
		gameObject3.AddComponent<PguiTouchTrigger>().AddListener(delegate
		{
			if (this.currentMode == SelTutorialFirstCtrl.Mode.PHOTO && this.selectPhoto >= 0)
			{
				this.selectPhoto = SelTutorialFirstCtrl.photo1;
				SoundManager.Play("prd_se_click", false, false);
			}
		}, null, null, null, null);
		GameObject gameObject4 = new GameObject("Touch2", new Type[] { typeof(RectTransform) });
		RectTransform component2 = gameObject4.GetComponent<RectTransform>();
		component2.SetParent(this.guiData.selchrObj.baseObj.transform, false);
		component2.SetAsFirstSibling();
		component2.sizeDelta = new Vector2(827f, 764f);
		component2.anchoredPosition = new Vector2(-413.5f, 0f);
		gameObject4.AddComponent<PguiCollider>();
		gameObject4.AddComponent<PguiTouchTrigger>().AddListener(delegate
		{
			if (this.currentMode == SelTutorialFirstCtrl.Mode.PHOTO && this.selectPhoto >= 0)
			{
				this.selectPhoto = SelTutorialFirstCtrl.photo2;
				SoundManager.Play("prd_se_click", false, false);
			}
		}, null, null, null, null);
		this.guiData.selchrObj.baseObj.SetActive(false);
		this.guiData.fadeObj.baseObj.transform.SetAsLastSibling();
		this.guiData.selchrObj.Btn_Ok.SetActEnable(false, false, false);
		this.guiData.selchrObj.Btn_Back.SetActEnable(false, false, false);
		this.guiData.selchrObj.Btn_Left.SetActEnable(false, false, false);
		this.guiData.selchrObj.Btn_Right.SetActEnable(false, false, false);
		yield return null;
		GameObject gameObject5 = AssetManager.InstantiateAssetData(SelTutorialFirstCtrl.concluAsset, base.transform);
		this.guiData.concluObj = new SelTutorialFirstCtrl.GUI_Conclu(gameObject5.transform);
		this.guiData.concluObj.baseObj.AddComponent<PguiTouchTrigger>().AddListener(delegate
		{
			if (this.currentMode == SelTutorialFirstCtrl.Mode.LETTER && this.guiData.concluObj.Txt_Touch.activeSelf)
			{
				this.touch = true;
			}
		}, null, null, null, null);
		this.guiData.concluObj.baseObj.SetActive(false);
		this.guiData.fadeObj.baseObj.transform.SetAsLastSibling();
		yield return null;
		string userName = DataManager.DmUserInfo.userName;
		if (string.IsNullOrEmpty(userName))
		{
			userName = SelTutorialFirstCtrl.defName;
		}
		this.guiData.nameObj.inputName.text = userName;
		this.guiData.nameObj.inputName.textComponent.text = userName;
		this.guiData.nameObj.Txt_Caution.text = "全角10文字以内で入力してください";
		this.guiData.nameObj.Txt_Caution.gameObject.SetActive(true);
		yield return null;
		AssetManager.UnloadAssetData(SelTutorialFirstCtrl.nameAsset, AssetManager.OWNER.NameEntry);
		AssetManager.UnloadAssetData(SelTutorialFirstCtrl.selchrAsset, AssetManager.OWNER.NameEntry);
		AssetManager.UnloadAssetData(SelTutorialFirstCtrl.concluAsset, AssetManager.OWNER.NameEntry);
		yield return null;
		yield break;
	}

	// Token: 0x06001A3E RID: 6718 RVA: 0x00154E44 File Offset: 0x00153044
	public void Term()
	{
		if (this.guiData != null)
		{
			this.guiData.baseObj = null;
			if (this.guiData.introObj != null)
			{
				Object.Destroy(this.guiData.introObj.baseObj);
				this.guiData.introObj = null;
			}
			if (this.guiData.nameObj != null)
			{
				Object.Destroy(this.guiData.nameObj.baseObj);
				this.guiData.nameObj = null;
			}
			if (this.guiData.selchrObj != null)
			{
				Object.Destroy(this.guiData.selchrObj.baseObj);
				this.guiData.selchrObj = null;
			}
			if (this.guiData.concluObj != null)
			{
				Object.Destroy(this.guiData.concluObj.baseObj);
				this.guiData.concluObj = null;
			}
			if (this.guiData.fadeObj != null)
			{
				Object.Destroy(this.guiData.fadeObj.baseObj);
				this.guiData.fadeObj = null;
			}
			this.guiData = null;
		}
	}

	// Token: 0x06001A3F RID: 6719 RVA: 0x00154F58 File Offset: 0x00153158
	private void Update()
	{
		if (this.requestMode != this.currentMode)
		{
			this.currentMode = this.requestMode;
			if (this.currentMode == SelTutorialFirstCtrl.Mode.INTRO)
			{
				this.guiData.introObj.baseObj.SetActive(true);
				this.guiData.introObj.Txt_Touch.SetActive(false);
				this.guiData.introObj.AEImage.PlayAnimation(this.guiData.introObj.AEImage.m_AnimeParam[this.anmIdx = 0].type, null);
				this.ienum = this.Init2();
			}
			else if (this.currentMode == SelTutorialFirstCtrl.Mode.INTRO_FADE)
			{
				this.guiData.fadeObj.aeCtrl.PlayAnimation(PguiAECtrl.AmimeType.START, delegate
				{
					this.check = true;
					this.guiData.fadeObj.aeCtrl.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
				});
				this.guiData.introObj.Txt_Touch.SetActive(false);
				SoundManager.Play("prd_se_scenario_scene_change", false, false);
			}
			else if (this.currentMode == SelTutorialFirstCtrl.Mode.NAME)
			{
				this.guiData.nameObj.inputName.enabled = true;
				this.guiData.nameObj.Paper01.PauseAnimation(PguiAECtrl.AmimeType.START, null);
				this.guiData.nameObj.Paper02.PauseAnimation(PguiAECtrl.AmimeType.START, null);
				this.guiData.nameObj.Btn_OK.SetActEnable(true, false, false);
			}
			else if (this.currentMode == SelTutorialFirstCtrl.Mode.PHOTO)
			{
				this.guiData.nameObj.Btn_OK.SetActEnable(false, false, false);
				this.guiData.selchrObj.baseObj.SetActive(true);
				this.guiData.selchrObj.AEImage.PlayAnimation(PguiAECtrl.AmimeType.START, delegate
				{
					this.guiData.selchrObj.AEImage.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
				});
				this.guiData.selchrObj.AEImage1.GetComponent<PguiReplaceAECtrl>().Replace("IN");
				this.guiData.selchrObj.AEImage1.PlayAnimation(PguiAECtrl.AmimeType.MAX, null);
				this.guiData.selchrObj.AEImage2.GetComponent<PguiReplaceAECtrl>().Replace("IN");
				this.guiData.selchrObj.AEImage2.PlayAnimation(PguiAECtrl.AmimeType.MAX, null);
				this.guiData.selchrObj.AEImageL.PauseAnimation(PguiAECtrl.AmimeType.START, null);
				this.guiData.selchrObj.Btn_Left.transform.Find("BaseImage/AEImage_Eff_Brink").GetComponent<PguiAECtrl>().m_AEImage.playTime = 0f;
				this.guiData.selchrObj.Btn_Right.transform.Find("BaseImage/AEImage_Eff_Brink").GetComponent<PguiAECtrl>().m_AEImage.playTime = 0f;
				this.selectPhoto = 0;
				SoundManager.Play("prd_se_tutorial_appearance_select", false, false);
			}
			else if (this.currentMode == SelTutorialFirstCtrl.Mode.CHECK)
			{
				this.guiData.nameObj.Btn_Yes.SetActEnable(true, false, false);
				this.guiData.nameObj.Btn_No.SetActEnable(true, false, false);
				this.guiData.nameObj.Paper01.PlayAnimation(PguiAECtrl.AmimeType.START, null);
				this.guiData.nameObj.Paper02.PlayAnimation(PguiAECtrl.AmimeType.START, null);
				this.guiData.nameObj.Img_Photo.Replace((this.selectPhoto == SelTutorialFirstCtrl.photo1) ? 2 : 1);
				SoundManager.Play("prd_se_tutorial_document_slide_b", false, false);
			}
			else if (this.currentMode == SelTutorialFirstCtrl.Mode.CONCLU)
			{
				this.guiData.nameObj.Btn_Yes.SetActEnable(false, false, false);
				this.guiData.nameObj.Btn_No.SetActEnable(false, false, false);
				this.guiData.concluObj.baseObj.SetActive(true);
				this.guiData.concluObj.Txt_Touch.SetActive(false);
				this.guiData.concluObj.AEImage1.PlayAnimation(PguiAECtrl.AmimeType.MAX, null);
				this.guiData.concluObj.AEImage2.PauseAnimation(PguiAECtrl.AmimeType.START, null);
			}
			else if (this.currentMode == SelTutorialFirstCtrl.Mode.LETTER)
			{
				this.guiData.concluObj.AEImage2.ResumeAnimation();
				SoundManager.Play("prd_se_tutorial_letter_receive", false, false);
			}
			this.check = false;
		}
		else if (this.currentMode == SelTutorialFirstCtrl.Mode.INTRO)
		{
			if (this.ienum != null && !this.ienum.MoveNext())
			{
				this.ienum = null;
			}
			if (!this.guiData.introObj.AEImage.IsPlaying())
			{
				if (this.anmIdx >= this.guiData.introObj.AEImage.m_AnimeParam.Count)
				{
					this.requestMode = SelTutorialFirstCtrl.Mode.INTRO_FADE;
				}
				else if (!this.guiData.introObj.Txt_Touch.activeSelf)
				{
					this.guiData.introObj.Txt_Touch.SetActive(true);
				}
				else if (this.touch)
				{
					SoundManager.Play("prd_se_click", false, false);
					this.guiData.introObj.Txt_Touch.SetActive(false);
					int num = this.anmIdx + 1;
					this.anmIdx = num;
					if (num < this.guiData.introObj.AEImage.m_AnimeParam.Count)
					{
						this.guiData.introObj.AEImage.PlayAnimation(this.guiData.introObj.AEImage.m_AnimeParam[this.anmIdx].type, null);
					}
					else
					{
						this.guiData.introObj.AEImage.m_AEImage.playOutTime = this.guiData.introObj.AEImage.m_AEImage.duration;
					}
				}
			}
		}
		else if (this.currentMode == SelTutorialFirstCtrl.Mode.INTRO_FADE)
		{
			if (this.ienum != null && !this.ienum.MoveNext())
			{
				this.ienum = null;
			}
			if (this.ienum == null)
			{
				if (this.check)
				{
					this.check = false;
					this.guiData.fadeObj.aeCtrl.PlayAnimation(PguiAECtrl.AmimeType.END, null);
					this.guiData.introObj.baseObj.SetActive(false);
					this.guiData.nameObj.baseObj.SetActive(true);
					this.guiData.nameObj.Txt_Touch.SetActive(false);
					this.guiData.nameObj.AEImage.PauseAnimation(PguiAECtrl.AmimeType.START, null);
					this.guiData.nameObj.Paper01.PauseAnimation(PguiAECtrl.AmimeType.START, null);
				}
				else if (!this.guiData.fadeObj.aeCtrl.IsPlaying())
				{
					SoundManager.Play("prd_se_tutorial_document_slide_a", false, false);
					this.guiData.nameObj.AEImage.ResumeAnimation();
					this.requestMode = SelTutorialFirstCtrl.Mode.NAME;
				}
			}
			else if (this.guiData.fadeObj.aeCtrl.GetAnimeType() == PguiAECtrl.AmimeType.LOOP)
			{
				this.check = true;
			}
		}
		else if (this.currentMode != SelTutorialFirstCtrl.Mode.NAME)
		{
			if (this.currentMode == SelTutorialFirstCtrl.Mode.PHOTO)
			{
				this.guiData.selchrObj.Btn_Ok.SetActEnable(this.selectPhoto > 0, true, false);
				this.guiData.selchrObj.Btn_Ok.m_Button.enabled = this.selectPhoto > 0;
				this.guiData.selchrObj.Btn_Back.SetActEnable(this.selectPhoto >= 0, false, false);
				this.guiData.selchrObj.Btn_Left.SetActEnable(this.selectPhoto >= 0, false, false);
				this.guiData.selchrObj.Btn_Right.SetActEnable(this.selectPhoto >= 0, false, false);
				if (this.selectPhoto < 0)
				{
					if (this.guiData.selchrObj.AEImage.GetAnimeType() == PguiAECtrl.AmimeType.END)
					{
						if (!this.guiData.selchrObj.AEImage.IsPlaying())
						{
							this.selectPhoto = -this.selectPhoto;
							this.guiData.selchrObj.baseObj.SetActive(false);
							this.requestMode = ((this.selectPhoto < 3) ? SelTutorialFirstCtrl.Mode.CHECK : SelTutorialFirstCtrl.Mode.NAME);
						}
					}
					else
					{
						PguiAECtrl pguiAECtrl = null;
						if (-this.selectPhoto == SelTutorialFirstCtrl.photo1)
						{
							pguiAECtrl = this.guiData.selchrObj.AEImage1;
						}
						else if (-this.selectPhoto == SelTutorialFirstCtrl.photo2)
						{
							pguiAECtrl = this.guiData.selchrObj.AEImage2;
						}
						if (pguiAECtrl != null)
						{
							PguiReplaceAECtrl component = pguiAECtrl.GetComponent<PguiReplaceAECtrl>();
							if (pguiAECtrl.m_AEImage.sourceClip == component.GetSourceClipById("SEL_END"))
							{
								if (!pguiAECtrl.IsPlaying())
								{
									this.guiData.selchrObj.AEImageL.PlayAnimation(PguiAECtrl.AmimeType.START, null);
									pguiAECtrl = null;
								}
							}
							else
							{
								component.Replace("SEL_END");
								pguiAECtrl.PlayAnimation(PguiAECtrl.AmimeType.MAX, null);
							}
						}
						if (pguiAECtrl == null)
						{
							this.guiData.selchrObj.AEImage.PlayAnimation(PguiAECtrl.AmimeType.END, null);
							this.guiData.nameObj.Paper01.PauseAnimation(PguiAECtrl.AmimeType.START, null);
							this.guiData.nameObj.Paper02.PauseAnimation(PguiAECtrl.AmimeType.START, null);
						}
					}
				}
				else
				{
					PguiReplaceAECtrl pguiReplaceAECtrl = this.guiData.selchrObj.AEImage1.GetComponent<PguiReplaceAECtrl>();
					if (this.selectPhoto == SelTutorialFirstCtrl.photo1)
					{
						if (!(this.guiData.selchrObj.AEImage1.m_AEImage.sourceClip == pguiReplaceAECtrl.GetSourceClipById("SEL_LOOP")))
						{
							if (this.guiData.selchrObj.AEImage1.m_AEImage.sourceClip == pguiReplaceAECtrl.GetSourceClipById("SEL"))
							{
								if (!this.guiData.selchrObj.AEImage1.IsPlaying())
								{
									this.guiData.selchrObj.AEImage1.GetComponent<PguiReplaceAECtrl>().Replace("SEL_LOOP");
									this.guiData.selchrObj.AEImage1.PlayAnimation(PguiAECtrl.AmimeType.MAX, null);
									this.guiData.selchrObj.AEImage1.m_AEImage.playLoop = true;
								}
							}
							else
							{
								this.guiData.selchrObj.AEImage1.GetComponent<PguiReplaceAECtrl>().Replace("SEL");
								this.guiData.selchrObj.AEImage1.PlayAnimation(PguiAECtrl.AmimeType.MAX, null);
							}
						}
					}
					else if (!(this.guiData.selchrObj.AEImage1.m_AEImage.sourceClip == pguiReplaceAECtrl.GetSourceClipById("LOOP")) && (this.guiData.selchrObj.AEImage1.m_AEImage.sourceClip != pguiReplaceAECtrl.GetSourceClipById("IN") || !this.guiData.selchrObj.AEImage1.IsPlaying()))
					{
						this.guiData.selchrObj.AEImage1.GetComponent<PguiReplaceAECtrl>().Replace("LOOP");
						this.guiData.selchrObj.AEImage1.PlayAnimation(PguiAECtrl.AmimeType.MAX, null);
						this.guiData.selchrObj.AEImage1.m_AEImage.playLoop = true;
					}
					pguiReplaceAECtrl = this.guiData.selchrObj.AEImage2.GetComponent<PguiReplaceAECtrl>();
					if (this.selectPhoto == SelTutorialFirstCtrl.photo2)
					{
						if (!(this.guiData.selchrObj.AEImage2.m_AEImage.sourceClip == pguiReplaceAECtrl.GetSourceClipById("SEL_LOOP")))
						{
							if (this.guiData.selchrObj.AEImage2.m_AEImage.sourceClip == pguiReplaceAECtrl.GetSourceClipById("SEL"))
							{
								if (!this.guiData.selchrObj.AEImage2.IsPlaying())
								{
									this.guiData.selchrObj.AEImage2.GetComponent<PguiReplaceAECtrl>().Replace("SEL_LOOP");
									this.guiData.selchrObj.AEImage2.PlayAnimation(PguiAECtrl.AmimeType.MAX, null);
									this.guiData.selchrObj.AEImage2.m_AEImage.playLoop = true;
								}
							}
							else
							{
								this.guiData.selchrObj.AEImage2.GetComponent<PguiReplaceAECtrl>().Replace("SEL");
								this.guiData.selchrObj.AEImage2.PlayAnimation(PguiAECtrl.AmimeType.MAX, null);
							}
						}
					}
					else if (!(this.guiData.selchrObj.AEImage2.m_AEImage.sourceClip == pguiReplaceAECtrl.GetSourceClipById("LOOP")) && (this.guiData.selchrObj.AEImage2.m_AEImage.sourceClip != pguiReplaceAECtrl.GetSourceClipById("IN") || !this.guiData.selchrObj.AEImage2.IsPlaying()))
					{
						this.guiData.selchrObj.AEImage2.GetComponent<PguiReplaceAECtrl>().Replace("LOOP");
						this.guiData.selchrObj.AEImage2.PlayAnimation(PguiAECtrl.AmimeType.MAX, null);
						this.guiData.selchrObj.AEImage2.m_AEImage.playLoop = true;
					}
				}
			}
			else if (this.currentMode != SelTutorialFirstCtrl.Mode.CHECK)
			{
				if (this.currentMode == SelTutorialFirstCtrl.Mode.CONCLU)
				{
					if (!this.guiData.concluObj.AEImage1.IsPlaying())
					{
						this.requestMode = SelTutorialFirstCtrl.Mode.LETTER;
					}
				}
				else if (this.currentMode == SelTutorialFirstCtrl.Mode.LETTER && !this.guiData.concluObj.AEImage2.IsPlaying())
				{
					if (this.guiData.concluObj.AEImage2.GetAnimeType() == PguiAECtrl.AmimeType.START)
					{
						if (!this.guiData.concluObj.Txt_Touch.activeSelf)
						{
							this.guiData.concluObj.Txt_Touch.SetActive(true);
						}
						else if (this.touch)
						{
							SoundManager.Play("prd_se_click", false, false);
							this.guiData.concluObj.Txt_Touch.SetActive(false);
							this.guiData.concluObj.AEImage2.PlayAnimation(PguiAECtrl.AmimeType.END, null);
							SoundManager.Play("prd_se_tutorial_photo_slide", false, false);
						}
					}
					else if (!DataManager.IsServerRequesting())
					{
						if (!this.guiData.concluObj.Txt_Touch.activeSelf)
						{
							this.guiData.concluObj.Txt_Touch.SetActive(true);
						}
						else if (this.touch)
						{
							SoundManager.Play("prd_se_click", false, false);
							this.guiData.concluObj.Txt_Touch.SetActive(false);
							this.requestMode = SelTutorialFirstCtrl.Mode.END;
						}
					}
				}
			}
		}
		this.touch = false;
	}

	// Token: 0x06001A40 RID: 6720 RVA: 0x00155DD7 File Offset: 0x00153FD7
	private IEnumerator NameCheckWait()
	{
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		if (DataManager.DmUserInfo.GetUpdateNameResult().isSuccess)
		{
			this.guiData.nameObj.Txt_UserName.text = this.guiData.nameObj.inputName.text;
			this.guiData.nameObj.Txt_Caution.text = "全角10文字以内で入力してください";
			this.guiData.nameObj.Txt_Caution.gameObject.SetActive(true);
			this.requestMode = SelTutorialFirstCtrl.Mode.PHOTO;
		}
		else
		{
			this.guiData.nameObj.inputName.enabled = true;
			this.guiData.nameObj.inputName.textComponent.text = this.guiData.nameObj.inputName.text;
			this.guiData.nameObj.Txt_Caution.gameObject.SetActive(true);
			this.guiData.nameObj.Txt_Caution.text = "この名前は利用できません";
			this.guiData.nameObj.Btn_OK.SetActEnable(true, false, false);
		}
		this.check = false;
		yield break;
	}

	// Token: 0x06001A41 RID: 6721 RVA: 0x00155DE8 File Offset: 0x00153FE8
	private void OnClickNameSelectButton(PguiButtonCtrl button)
	{
		if (this.currentMode != SelTutorialFirstCtrl.Mode.NAME || this.check)
		{
			return;
		}
		if (string.IsNullOrEmpty(this.guiData.nameObj.inputName.text))
		{
			this.guiData.nameObj.inputName.textComponent.text = "";
			this.guiData.nameObj.Txt_Caution.gameObject.SetActive(true);
			this.guiData.nameObj.Txt_Caution.text = "名前を入力してください";
			return;
		}
		bool disableServerRequestByTutorial = Singleton<DataManager>.Instance.DisableServerRequestByTutorial;
		Singleton<DataManager>.Instance.DisableServerRequestByTutorial = false;
		this.guiData.nameObj.inputName.text = DataManager.DmUserInfo.RequestActionUpdateUserName(this.guiData.nameObj.inputName.text);
		this.guiData.nameObj.inputName.enabled = false;
		Singleton<DataManager>.Instance.DisableServerRequestByTutorial = disableServerRequestByTutorial;
		this.guiData.nameObj.Btn_OK.SetActEnable(false, false, false);
		base.StartCoroutine(this.NameCheckWait());
		this.check = true;
	}

	// Token: 0x06001A42 RID: 6722 RVA: 0x00155F10 File Offset: 0x00154110
	private void OnClickDecideSelectButton(PguiButtonCtrl button)
	{
		if (this.currentMode != SelTutorialFirstCtrl.Mode.CHECK || this.check)
		{
			return;
		}
		DataManagerUserInfo.AvatarType avatarType = (DataManagerUserInfo.AvatarType)this.selectPhoto;
		bool disableServerRequestByTutorial = Singleton<DataManager>.Instance.DisableServerRequestByTutorial;
		Singleton<DataManager>.Instance.DisableServerRequestByTutorial = false;
		DataManager.DmUserInfo.RequestActionUpdateUserAvatar(avatarType);
		Singleton<DataManager>.Instance.DisableServerRequestByTutorial = disableServerRequestByTutorial;
		this.guiData.nameObj.AEImage.PlayAnimation(PguiAECtrl.AmimeType.END, new PguiAECtrl.FinishCallback(this.OnSendLetter));
		this.guiData.nameObj.Paper01.PlayAnimation(PguiAECtrl.AmimeType.END, null);
		this.check = true;
		SoundManager.Play("prd_se_tutorial_document_submit", false, false);
		this.guiData.nameObj.Btn_Yes.SetActEnable(false, false, false);
		this.guiData.nameObj.Btn_No.SetActEnable(false, false, false);
	}

	// Token: 0x06001A43 RID: 6723 RVA: 0x00155FDF File Offset: 0x001541DF
	private void OnSendLetter()
	{
		SoundManager.Play("prd_se_tutorial_letter_send", false, false);
		SoundManager.Play("prd_se_tutorial_letter_return_and_open", false, false);
		this.requestMode = SelTutorialFirstCtrl.Mode.CONCLU;
	}

	// Token: 0x06001A44 RID: 6724 RVA: 0x00156004 File Offset: 0x00154204
	private void OnClickDecideCancelButton(PguiButtonCtrl button)
	{
		if (this.currentMode != SelTutorialFirstCtrl.Mode.CHECK || this.check)
		{
			return;
		}
		this.requestMode = SelTutorialFirstCtrl.Mode.PHOTO;
		this.guiData.nameObj.Btn_Yes.SetActEnable(false, false, false);
		this.guiData.nameObj.Btn_No.SetActEnable(false, false, false);
	}

	// Token: 0x04001417 RID: 5143
	private SelTutorialFirstCtrl.GUI guiData;

	// Token: 0x04001418 RID: 5144
	private int selectPhoto;

	// Token: 0x04001419 RID: 5145
	private SelTutorialFirstCtrl.Mode requestMode;

	// Token: 0x0400141A RID: 5146
	private SelTutorialFirstCtrl.Mode currentMode;

	// Token: 0x0400141B RID: 5147
	private int anmIdx;

	// Token: 0x0400141C RID: 5148
	private bool touch;

	// Token: 0x0400141D RID: 5149
	private bool check;

	// Token: 0x0400141E RID: 5150
	private IEnumerator ienum;

	// Token: 0x0400141F RID: 5151
	private static readonly int photo1 = 2;

	// Token: 0x04001420 RID: 5152
	private static readonly int photo2 = 1;

	// Token: 0x04001421 RID: 5153
	private static readonly string defName = "しんまい";

	// Token: 0x04001422 RID: 5154
	public static readonly string introAsset = "Gui/NameEntry/Tutorial_NameEntry_Apart";

	// Token: 0x04001423 RID: 5155
	private static readonly string nameAsset = "Gui/NameEntry/Tutorial_NameEntry_Bpart";

	// Token: 0x04001424 RID: 5156
	private static readonly string selchrAsset = "Gui/NameEntry/Tutorial_NameEntry_SelChara";

	// Token: 0x04001425 RID: 5157
	private static readonly string concluAsset = "Gui/NameEntry/Tutorial_NameEntry_Cpart";

	// Token: 0x02000E5E RID: 3678
	public enum Mode
	{
		// Token: 0x040052A8 RID: 21160
		INVALID,
		// Token: 0x040052A9 RID: 21161
		INTRO,
		// Token: 0x040052AA RID: 21162
		INTRO_FADE,
		// Token: 0x040052AB RID: 21163
		NAME,
		// Token: 0x040052AC RID: 21164
		PHOTO,
		// Token: 0x040052AD RID: 21165
		CHECK,
		// Token: 0x040052AE RID: 21166
		CONCLU,
		// Token: 0x040052AF RID: 21167
		LETTER,
		// Token: 0x040052B0 RID: 21168
		END
	}

	// Token: 0x02000E5F RID: 3679
	public class GUI_Intro
	{
		// Token: 0x06004C50 RID: 19536 RVA: 0x0022C121 File Offset: 0x0022A321
		public GUI_Intro(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.AEImage = baseTr.Find("AEImage").GetComponent<PguiAECtrl>();
			this.Txt_Touch = baseTr.Find("Txt_Touch").gameObject;
		}

		// Token: 0x040052B1 RID: 21169
		public GameObject baseObj;

		// Token: 0x040052B2 RID: 21170
		public PguiAECtrl AEImage;

		// Token: 0x040052B3 RID: 21171
		public GameObject Txt_Touch;
	}

	// Token: 0x02000E60 RID: 3680
	public class GUI_Name
	{
		// Token: 0x06004C51 RID: 19537 RVA: 0x0022C164 File Offset: 0x0022A364
		public GUI_Name(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.AEImage = baseTr.Find("AEImage").GetComponent<PguiAECtrl>();
			this.inputName = baseTr.Find("Page_All/Paper01/PaperAll/InputField").GetComponent<InputField>();
			this.Paper01 = baseTr.Find("Page_All/Paper01").GetComponent<PguiAECtrl>();
			this.Txt_Caution = baseTr.Find("Page_All/Paper01/PaperAll/Txt_Caution").GetComponent<PguiTextCtrl>();
			this.Btn_OK = baseTr.Find("Page_All/Paper01/PaperAll/Btn_OK").GetComponent<PguiButtonCtrl>();
			this.Txt_UserName = baseTr.Find("Page_All/Paper02/PaperAll/Txt_UserName").GetComponent<PguiTextCtrl>();
			this.Paper02 = baseTr.Find("Page_All/Paper02").GetComponent<PguiAECtrl>();
			this.Btn_Yes = baseTr.Find("Page_All/Paper02/PaperAll/Btn_Yes").GetComponent<PguiButtonCtrl>();
			this.Btn_No = baseTr.Find("Page_All/Paper02/PaperAll/Btn_No").GetComponent<PguiButtonCtrl>();
			this.Img_Photo = baseTr.Find("Page_All/Paper02/Chara_Photo").GetComponent<PguiReplaceSpriteCtrl>();
			this.Txt_Touch = baseTr.Find("Txt_Touch").gameObject;
		}

		// Token: 0x040052B4 RID: 21172
		public GameObject baseObj;

		// Token: 0x040052B5 RID: 21173
		public PguiAECtrl AEImage;

		// Token: 0x040052B6 RID: 21174
		public InputField inputName;

		// Token: 0x040052B7 RID: 21175
		public PguiAECtrl Paper01;

		// Token: 0x040052B8 RID: 21176
		public PguiAECtrl Paper02;

		// Token: 0x040052B9 RID: 21177
		public PguiTextCtrl Txt_Caution;

		// Token: 0x040052BA RID: 21178
		public PguiButtonCtrl Btn_OK;

		// Token: 0x040052BB RID: 21179
		public PguiButtonCtrl Btn_Yes;

		// Token: 0x040052BC RID: 21180
		public PguiButtonCtrl Btn_No;

		// Token: 0x040052BD RID: 21181
		public PguiTextCtrl Txt_UserName;

		// Token: 0x040052BE RID: 21182
		public PguiReplaceSpriteCtrl Img_Photo;

		// Token: 0x040052BF RID: 21183
		public GameObject Txt_Touch;
	}

	// Token: 0x02000E61 RID: 3681
	public class GUI_SelChara
	{
		// Token: 0x06004C52 RID: 19538 RVA: 0x0022C278 File Offset: 0x0022A478
		public GUI_SelChara(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.AEImage = baseTr.Find("AEImage").GetComponent<PguiAECtrl>();
			this.AEImage1 = baseTr.Find("AEImage_Human01").GetComponent<PguiAECtrl>();
			this.AEImage2 = baseTr.Find("AEImage_Human02").GetComponent<PguiAECtrl>();
			this.AEImageL = baseTr.Find("AEImage_Light").GetComponent<PguiAECtrl>();
			this.Btn_Back = baseTr.Find("Btn_All/Btn_Back").GetComponent<PguiButtonCtrl>();
			this.Btn_Ok = baseTr.Find("Btn_All/Btn_Ok").GetComponent<PguiButtonCtrl>();
			this.Btn_Left = baseTr.Find("Btn_All/Btn_Left").GetComponent<PguiButtonCtrl>();
			this.Btn_Right = baseTr.Find("Btn_All/Btn_Right").GetComponent<PguiButtonCtrl>();
		}

		// Token: 0x040052C0 RID: 21184
		public GameObject baseObj;

		// Token: 0x040052C1 RID: 21185
		public PguiAECtrl AEImage;

		// Token: 0x040052C2 RID: 21186
		public PguiAECtrl AEImage1;

		// Token: 0x040052C3 RID: 21187
		public PguiAECtrl AEImage2;

		// Token: 0x040052C4 RID: 21188
		public PguiAECtrl AEImageL;

		// Token: 0x040052C5 RID: 21189
		public PguiButtonCtrl Btn_Back;

		// Token: 0x040052C6 RID: 21190
		public PguiButtonCtrl Btn_Ok;

		// Token: 0x040052C7 RID: 21191
		public PguiButtonCtrl Btn_Left;

		// Token: 0x040052C8 RID: 21192
		public PguiButtonCtrl Btn_Right;
	}

	// Token: 0x02000E62 RID: 3682
	public class GUI_Conclu
	{
		// Token: 0x06004C53 RID: 19539 RVA: 0x0022C348 File Offset: 0x0022A548
		public GUI_Conclu(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.AEImage1 = baseTr.Find("01/AEImage").GetComponent<PguiAECtrl>();
			this.AEImage2 = baseTr.Find("02/AEImage_Letter").GetComponent<PguiAECtrl>();
			this.Txt_Touch = baseTr.Find("Txt_Touch").gameObject;
		}

		// Token: 0x040052C9 RID: 21193
		public GameObject baseObj;

		// Token: 0x040052CA RID: 21194
		public PguiAECtrl AEImage1;

		// Token: 0x040052CB RID: 21195
		public PguiAECtrl AEImage2;

		// Token: 0x040052CC RID: 21196
		public GameObject Txt_Touch;
	}

	// Token: 0x02000E63 RID: 3683
	public class GUI_Fade
	{
		// Token: 0x06004C54 RID: 19540 RVA: 0x0022C3A9 File Offset: 0x0022A5A9
		public GUI_Fade(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.aeCtrl = baseTr.GetComponent<PguiAECtrl>();
		}

		// Token: 0x040052CD RID: 21197
		public GameObject baseObj;

		// Token: 0x040052CE RID: 21198
		public PguiAECtrl aeCtrl;
	}

	// Token: 0x02000E64 RID: 3684
	public class GUI
	{
		// Token: 0x06004C55 RID: 19541 RVA: 0x0022C3C9 File Offset: 0x0022A5C9
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
		}

		// Token: 0x040052CF RID: 21199
		public GameObject baseObj;

		// Token: 0x040052D0 RID: 21200
		public SelTutorialFirstCtrl.GUI_Intro introObj;

		// Token: 0x040052D1 RID: 21201
		public SelTutorialFirstCtrl.GUI_Name nameObj;

		// Token: 0x040052D2 RID: 21202
		public SelTutorialFirstCtrl.GUI_SelChara selchrObj;

		// Token: 0x040052D3 RID: 21203
		public SelTutorialFirstCtrl.GUI_Conclu concluObj;

		// Token: 0x040052D4 RID: 21204
		public SelTutorialFirstCtrl.GUI_Fade fadeObj;
	}
}
