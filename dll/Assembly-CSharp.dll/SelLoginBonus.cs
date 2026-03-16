using System;
using System.Collections;
using System.Collections.Generic;
using CriWare;
using SGNFW.Common;
using SGNFW.Mst;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelLoginBonus : MonoBehaviour
{
	private void Start()
	{
		Singleton<DataManager>.Instance.InitializeByEditor(null);
	}

	private void Update()
	{
	}

	public static IEnumerator ExeLoginBonus(Transform parentTr)
	{
		SelLoginBonus.rcvDate = TimeManager.Now;
		SelLoginBonus.allSkip = false;
		HomeCheckResult homeCheckResult = DataManager.DmHome.GetHomeCheckResult();
		if (homeCheckResult == null || homeCheckResult.loginBonusList == null)
		{
			yield break;
		}
		if (DataManager.DmHome.GetMstLoginBonusPresetList().FindAll((MstBonusPresetData item) => item.GetDispCategory() == MstBonusPresetData.DispCategory.CAMPAIGN).Find((MstBonusPresetData item) => homeCheckResult.loginBonusList.Find((HomeCheckResult.LoginBonus item2) => item2.isReceive && item2.id == item.bonusId) != null) != null)
		{
			IEnumerator campaign = SelLoginBonus.ExeLoginBonusCampaign(parentTr, homeCheckResult);
			while (campaign.MoveNext())
			{
				yield return null;
			}
			campaign = null;
		}
		IEnumerator normal = SelLoginBonus.ExeLoginBonusNormal(parentTr, homeCheckResult);
		while (normal.MoveNext())
		{
			yield return null;
		}
		yield break;
	}

	private static IEnumerator ExeLoginBonusCampaign(Transform parentTr, HomeCheckResult homeCheckResult)
	{
		SelLoginBonus.<>c__DisplayClass12_0 CS$<>8__locals1 = new SelLoginBonus.<>c__DisplayClass12_0();
		if (SelLoginBonus.allSkip)
		{
			yield break;
		}
		SelLoginBonus.GuiCampaign guiData = null;
		CS$<>8__locals1.isRequestSkip = false;
		List<HomeCheckResult.LoginBonus> enableBonusList = new List<HomeCheckResult.LoginBonus>();
		for (int i = 0; i < homeCheckResult.loginBonusList.Count; i++)
		{
			HomeCheckResult.LoginBonus lgbData = homeCheckResult.loginBonusList[i];
			if (lgbData.isReceive)
			{
				MstBonusPresetData mstBonusPresetData = DataManager.DmHome.GetMstLoginBonusPresetList().Find((MstBonusPresetData item) => item.bonusId == lgbData.id);
				if (mstBonusPresetData != null && mstBonusPresetData.GetDispCategory() == MstBonusPresetData.DispCategory.CAMPAIGN)
				{
					enableBonusList.Add(lgbData);
				}
			}
		}
		if (enableBonusList.Count > 0)
		{
			SceneHome.StartNotice();
		}
		CS$<>8__locals1.fadeGui = AssetManager.InstantiateAssetData("Cmn/GUI/Prefab/GUI_LeafFade", parentTr).GetComponent<PguiAECtrl>();
		int num;
		for (int lcnt = 0; lcnt < enableBonusList.Count; lcnt = num + 1)
		{
			SelLoginBonus.<>c__DisplayClass12_2 CS$<>8__locals3 = new SelLoginBonus.<>c__DisplayClass12_2();
			CS$<>8__locals3.CS$<>8__locals1 = CS$<>8__locals1;
			CS$<>8__locals3.lgbData = enableBonusList[lcnt];
			CS$<>8__locals3.presetMstCampaign = DataManager.DmHome.GetMstLoginBonusPresetList().Find((MstBonusPresetData item) => item.bonusId == CS$<>8__locals3.lgbData.id);
			List<MstBonusData> list = DataManager.DmHome.GetMstLoginBonusList().FindAll((MstBonusData item) => item.bonusId == CS$<>8__locals3.presetMstCampaign.bonusId);
			list.Sort((MstBonusData a, MstBonusData b) => a.days - b.days);
			CS$<>8__locals3.CS$<>8__locals1.isRequestSkip = false;
			if (guiData == null)
			{
				guiData = new SelLoginBonus.GuiCampaign(AssetManager.InstantiateAssetData("SceneLoginBonus/GUI/Prefab/GUI_LoginCanpaign", parentTr).transform);
				guiData.baseObj.transform.SetAsFirstSibling();
				guiData.Btn_Skip.AddOnClickListener(delegate(PguiButtonCtrl btn)
				{
					SelLoginBonus.allSkip = true;
				}, PguiButtonCtrl.SoundType.DEFAULT);
				PguiTouchTrigger pguiTouchTrigger = guiData.FrontBG.gameObject.AddComponent<PguiTouchTrigger>();
				UnityAction unityAction;
				if ((unityAction = CS$<>8__locals3.CS$<>8__locals1.<>9__5) == null)
				{
					unityAction = (CS$<>8__locals3.CS$<>8__locals1.<>9__5 = delegate
					{
						CS$<>8__locals3.CS$<>8__locals1.isRequestSkip = true;
					});
				}
				pguiTouchTrigger.AddListener(unityAction, null, null, null, null);
				guiData.Touch.SetActive(false);
			}
			guiData.MakeList(list.Count);
			if (CS$<>8__locals3.presetMstCampaign.bonusType == 4 || CS$<>8__locals3.presetMstCampaign.bonusType == 5)
			{
				guiData.Tex.transform.parent.gameObject.SetActive(false);
			}
			else
			{
				DateTime dateTime = new DateTime(PrjUtil.ConvertTimeToTicks(CS$<>8__locals3.presetMstCampaign.startDatetime));
				DateTime dateTime2 = new DateTime(PrjUtil.ConvertTimeToTicks(CS$<>8__locals3.presetMstCampaign.endDatetime));
				TimeSpan timeSpan = dateTime2 - TimeManager.Now;
				guiData.Tex.transform.parent.gameObject.SetActive(timeSpan.Days < 3650);
				guiData.Tex.ReplaceTextByDefault("Param01", dateTime.ToString("yyyy年M月d日～") + dateTime2.ToString("yyyy年M月d日"));
			}
			guiData.FrontBG.SetRawImage(CS$<>8__locals3.presetMstCampaign.bannerName, true, false, null);
			for (int j = 0; j < list.Count; j++)
			{
				ItemStaticBase itemStaticBase = DataManager.DmItem.GetItemStaticBase(list[j].itemId);
				guiData.oneList[j].BonusItem.Setup(itemStaticBase);
				IconItemCtrl bonusItem = guiData.oneList[j].BonusItem;
				IconItemCtrl.OnClick onClick;
				if ((onClick = CS$<>8__locals3.CS$<>8__locals1.<>9__7) == null)
				{
					onClick = (CS$<>8__locals3.CS$<>8__locals1.<>9__7 = delegate(IconItemCtrl iic)
					{
						CS$<>8__locals3.CS$<>8__locals1.isRequestSkip = true;
					});
				}
				bonusItem.AddOnClickListener(onClick);
				guiData.oneList[j].Num_Txt.text = "×" + list[j].itemNum.ToString();
				guiData.oneList[j].Txt_Day.text = (j + 1).ToString() + "日目";
				guiData.oneList[j].Txt_Item.text = itemStaticBase.GetName();
				if (j + 1 < CS$<>8__locals3.lgbData.day)
				{
					guiData.oneList[j].Anim.ExPauseAnimationLastFrame(SimpleAnimation.ExPguiStatus.START);
				}
				else
				{
					guiData.oneList[j].Anim.ExPauseAnimation(SimpleAnimation.ExPguiStatus.START, null);
				}
			}
			SelLoginBonus.<>c__DisplayClass12_3 CS$<>8__locals4 = new SelLoginBonus.<>c__DisplayClass12_3();
			CS$<>8__locals4.animEnd = false;
			CS$<>8__locals3.CS$<>8__locals1.fadeGui.PlayAnimation(PguiAECtrl.AmimeType.END, delegate
			{
				CS$<>8__locals4.animEnd = true;
			});
			while (!CS$<>8__locals4.animEnd && !SelLoginBonus.allSkip)
			{
				yield return null;
			}
			if (SelLoginBonus.allSkip)
			{
				break;
			}
			CS$<>8__locals4 = null;
			if (!SelLoginBonus.allSkip)
			{
				SoundManager.Play("prd_se_login_bonus_stamp", false, false);
			}
			CS$<>8__locals3.anmEnd = false;
			SimpleAnimation crntAnm = guiData.oneList[CS$<>8__locals3.lgbData.day - 1].Anim;
			CS$<>8__locals3.crntInfo = crntAnm.transform.Find("Login_ItemInfo").GetComponent<SimpleAnimation>();
			CS$<>8__locals3.crntInfo.transform.SetParent(guiData.Btn_Skip.transform.parent, true);
			CS$<>8__locals3.crntInfo.transform.SetSiblingIndex(guiData.Btn_Skip.transform.GetSiblingIndex());
			crntAnm.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, delegate
			{
				CS$<>8__locals3.crntInfo.gameObject.SetActive(true);
				SimpleAnimation crntInfo = CS$<>8__locals3.crntInfo;
				SimpleAnimation.ExPguiStatus exPguiStatus = SimpleAnimation.ExPguiStatus.START;
				SimpleAnimation.ExFinishCallback exFinishCallback;
				if ((exFinishCallback = CS$<>8__locals3.<>9__9) == null)
				{
					exFinishCallback = (CS$<>8__locals3.<>9__9 = delegate
					{
						CS$<>8__locals3.anmEnd = true;
					});
				}
				crntInfo.ExPlayAnimation(exPguiStatus, exFinishCallback);
			});
			while (!CS$<>8__locals3.anmEnd && !CS$<>8__locals3.CS$<>8__locals1.isRequestSkip && !SelLoginBonus.allSkip)
			{
				yield return null;
			}
			if (SelLoginBonus.allSkip)
			{
				break;
			}
			guiData.Touch.SetActive(true);
			CS$<>8__locals3.CS$<>8__locals1.isRequestSkip = false;
			while (!CS$<>8__locals3.CS$<>8__locals1.isRequestSkip && !SelLoginBonus.allSkip)
			{
				yield return null;
			}
			if (SelLoginBonus.allSkip)
			{
				break;
			}
			guiData.Touch.SetActive(false);
			SelLoginBonus.<>c__DisplayClass12_4 CS$<>8__locals5 = new SelLoginBonus.<>c__DisplayClass12_4();
			CS$<>8__locals5.CS$<>8__locals2 = CS$<>8__locals3;
			CS$<>8__locals5.animEnd = false;
			if (!SelLoginBonus.allSkip)
			{
				SoundManager.Play("prd_se_login_bonus_leaf", false, false);
			}
			CS$<>8__locals5.CS$<>8__locals2.CS$<>8__locals1.fadeGui.PlayAnimation(PguiAECtrl.AmimeType.START, delegate
			{
				CS$<>8__locals5.animEnd = true;
				CS$<>8__locals5.CS$<>8__locals2.CS$<>8__locals1.fadeGui.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
			});
			while (!CS$<>8__locals5.animEnd && !SelLoginBonus.allSkip)
			{
				yield return null;
			}
			if (SelLoginBonus.allSkip)
			{
				break;
			}
			CS$<>8__locals5 = null;
			CS$<>8__locals3.crntInfo.transform.SetParent(crntAnm.transform, true);
			CS$<>8__locals3 = null;
			crntAnm = null;
			num = lcnt;
		}
		Object.Destroy(guiData.baseObj);
		Object.Destroy(CS$<>8__locals1.fadeGui.gameObject);
		yield break;
	}

	private static IEnumerator ExeLoginBonusNormal(Transform parentTr, HomeCheckResult homeCheckResult)
	{
		if (SelLoginBonus.allSkip)
		{
			yield break;
		}
		MstBonusPresetData presetMstDaily = DataManager.DmHome.GetMstLoginBonusPresetList().Find((MstBonusPresetData item) => item.GetDispCategory() == MstBonusPresetData.DispCategory.DYAS);
		List<MstBonusData> list = DataManager.DmHome.GetMstLoginBonusList().FindAll((MstBonusData item) => item.bonusId == presetMstDaily.bonusId);
		list.Sort((MstBonusData a, MstBonusData b) => a.days - b.days);
		MstBonusPresetData presetMstTotal = DataManager.DmHome.GetMstLoginBonusPresetList().Find((MstBonusPresetData item) => item.GetDispCategory() == MstBonusPresetData.DispCategory.TOTAL);
		List<MstBonusData> list2 = DataManager.DmHome.GetMstLoginBonusList().FindAll((MstBonusData item) => item.bonusId == presetMstTotal.bonusId);
		list2.Sort((MstBonusData a, MstBonusData b) => a.days - b.days);
		MstBonusPresetData presetMstSpecial = DataManager.DmHome.GetMstLoginBonusPresetList().Find((MstBonusPresetData item) => item.GetDispCategory() == MstBonusPresetData.DispCategory.SPECIAL_TOTAL);
		List<MstBonusData> list3 = DataManager.DmHome.GetMstLoginBonusList().FindAll((MstBonusData item) => item.bonusId == presetMstSpecial.bonusId);
		list3.Sort((MstBonusData a, MstBonusData b) => a.days - b.days);
		HomeCheckResult.LoginBonus loginBonus = homeCheckResult.loginBonusList.Find((HomeCheckResult.LoginBonus item) => item.id == presetMstDaily.bonusId);
		HomeCheckResult.LoginBonus loginBonusTotal = homeCheckResult.loginBonusList.Find((HomeCheckResult.LoginBonus item) => item.id == presetMstTotal.bonusId);
		HomeCheckResult.LoginBonus loginBonusSpecial = homeCheckResult.loginBonusList.Find((HomeCheckResult.LoginBonus item) => item.id == presetMstSpecial.bonusId);
		if (loginBonus == null)
		{
			yield break;
		}
		if (loginBonusTotal == null)
		{
			yield break;
		}
		if (loginBonusSpecial == null)
		{
			yield break;
		}
		MstBonusData firstMstTotal = list2.Find((MstBonusData item) => item.days >= loginBonusTotal.day);
		MstBonusData secondMstTotal = list2.Find((MstBonusData item) => item.days > loginBonusTotal.day);
		MstBonusData firstMstSpecial = list3.Find((MstBonusData item) => item.days >= loginBonusSpecial.day);
		MstBonusData secondMstSpecial = list3.Find((MstBonusData item) => item.days > loginBonusSpecial.day);
		if (firstMstTotal == null)
		{
			yield break;
		}
		if (secondMstTotal == null)
		{
			yield break;
		}
		if (firstMstSpecial == null)
		{
			yield break;
		}
		if (secondMstSpecial == null)
		{
			yield break;
		}
		SceneHome.StartNotice();
		SelLoginBonus.GUI guiData = new SelLoginBonus.GUI(AssetManager.InstantiateAssetData("SceneLoginBonus/GUI/Prefab/GUI_LoginBonus", parentTr).transform);
		guiData.baseObj.transform.SetAsFirstSibling();
		guiData.Touch.SetActive(false);
		SelLoginBonus.GuiTotalGet guiNormal = new SelLoginBonus.GuiTotalGet(AssetManager.InstantiateAssetData("SceneLoginBonus/GUI/Prefab/LoginBonus_TotalBonus_Normal", guiData.TotalGetNull).transform);
		SelLoginBonus.GuiTotalGet guiSpecial = new SelLoginBonus.GuiTotalGet(AssetManager.InstantiateAssetData("SceneLoginBonus/GUI/Prefab/LoginBonus_TotalBonus_Special", guiData.TotalGetNull).transform);
		int charaId = DataManager.DmUserInfo.favoriteCharaId;
		if (DataManager.DmUserInfo.optionData.LoginBonusFriends)
		{
			List<int> list4 = new List<int>(DataManager.DmChara.GetUserCharaMap().Keys);
			if (list4.Count > 0)
			{
				charaId = list4[Random.Range(0, list4.Count)];
			}
		}
		RenderTextureChara renderTextureChara = AssetManager.InstantiateAssetData("RenderTextureChara/Prefab/RenderTextureCharaCtrl", guiData.baseObj.transform.Find("RenderChara").transform).GetComponent<RenderTextureChara>();
		renderTextureChara.postion = new Vector2(-2480f, -150f);
		renderTextureChara.fieldOfView = 15f;
		renderTextureChara.Setup(DataManager.DmChara.GetUserCharaData(charaId), 0, CharaMotionDefine.ActKey.SCENARIO_STAND_BY, true, null, false, null, 0f, null, false);
		bool isNextEffect = false;
		bool isRequestSkip = false;
		bool isAnimeEnd = false;
		guiData.FrontBG.gameObject.AddComponent<PguiTouchTrigger>().AddListener(delegate
		{
			isRequestSkip = true;
		}, null, null, null, null);
		guiData.Window01.ExPauseAnimation(SimpleAnimation.ExPguiStatus.START, null);
		guiData.Window02.ExPauseAnimation(SimpleAnimation.ExPguiStatus.START, null);
		guiData.AEImage_NextEff.PauseAnimation(PguiAECtrl.AmimeType.LOOP, null);
		guiData.AEImage_NextEffBack.PauseAnimation(PguiAECtrl.AmimeType.LOOP, null);
		guiData.AEImage_Stamp.PauseAnimation(PguiAECtrl.AmimeType.START, null);
		guiData.AEImage_Txt.PauseAnimation(PguiAECtrl.AmimeType.START, null);
		guiData.FrontBG.GetComponent<PguiRawImageCtrl>().SetRawImage("Texture2D/Bg_Scene/selbg_home_out", true, false, null);
		guiData.FrontBG.localScale = Vector3.one;
		guiData.CharSerif.gameObject.SetActive(false);
		guiData.BackObj.gameObject.SetActive(false);
		guiData.TopObj.gameObject.SetActive(false);
		guiData.Btn_Skip.AddOnClickListener(delegate(PguiButtonCtrl btn)
		{
			SelLoginBonus.allSkip = true;
		}, PguiButtonCtrl.SoundType.DEFAULT);
		guiData.Btn_Skip.gameObject.SetActive(false);
		guiData.TotalGet.PauseAnimation(PguiAECtrl.AmimeType.START, null);
		guiData.TotalGetStamp.PauseAnimation(PguiAECtrl.AmimeType.START, null);
		Transform nextTopObj = null;
		if (loginBonus.day == guiData.BonusObj01.Count)
		{
			isNextEffect = true;
			Transform transform = guiData.Window02.transform.Find("LoginBonus_List/LoginBonus_day01").transform;
			guiData.AEImage_NextEff.transform.SetParent(transform, false);
			guiData.AEImage_NextEffBack.transform.SetParent(transform, false);
			guiData.AEImage_NextEffBack.transform.SetAsFirstSibling();
			transform.SetAsLastSibling();
			nextTopObj = guiData.Window02.transform.Find("Tex_Top");
			nextTopObj.SetParent(guiData.Window02.transform.parent, false);
			nextTopObj.SetAsLastSibling();
			nextTopObj.gameObject.SetActive(false);
			PguiAECtrl component = guiData.Window02.transform.Find("Total_Normal/AEImage").GetComponent<PguiAECtrl>();
			component.GetComponent<PguiReplaceAECtrl>().Replace("OUT");
			component.PauseAnimation(PguiAECtrl.AmimeType.START, null);
			PguiAECtrl component2 = guiData.Window02.transform.Find("Total_Special/AEImage").GetComponent<PguiAECtrl>();
			component2.GetComponent<PguiReplaceAECtrl>().Replace("OUT");
			component2.PauseAnimation(PguiAECtrl.AmimeType.START, null);
		}
		guiData.Daily_NowDay.text = loginBonusTotal.day.ToString() + PrjUtil.MakeMessage("日目");
		ItemData normID = new ItemData(firstMstTotal.itemId, firstMstTotal.itemNum);
		guiData.Normal_ItemIcon.Setup(normID);
		guiData.Normal_ItemIcon.AddOnClickListener(delegate(IconItemCtrl iic)
		{
			isRequestSkip = true;
		});
		guiNormal.ItemIcon.Setup(normID);
		guiNormal.ItemIcon.AddOnClickListener(delegate(IconItemCtrl iic)
		{
			isRequestSkip = true;
		});
		guiNormal.Num_NowDayTxt.text = (guiData.Normal_Num_NowDayTxt.text = firstMstTotal.days.ToString() + PrjUtil.MakeMessage("日"));
		guiNormal.Num_InfoTxt.text = (guiData.Normal_Num_InfoTxt.text = PrjUtil.MakeMessage("<size=18>あと</size>") + (firstMstTotal.days - loginBonusTotal.day).ToString() + PrjUtil.MakeMessage("日"));
		ItemData specID = new ItemData(firstMstSpecial.itemId, firstMstSpecial.itemNum);
		guiData.Special_ItemIcon.Setup(specID);
		guiData.Special_ItemIcon.AddOnClickListener(delegate(IconItemCtrl iic)
		{
			isRequestSkip = true;
		});
		guiSpecial.ItemIcon.Setup(specID);
		guiSpecial.ItemIcon.AddOnClickListener(delegate(IconItemCtrl iic)
		{
			isRequestSkip = true;
		});
		guiSpecial.Num_NowDayTxt.text = (guiData.Special_Num_NowDayTxt.text = firstMstSpecial.days.ToString() + PrjUtil.MakeMessage("日"));
		guiSpecial.Num_InfoTxt.text = (guiData.Special_Num_InfoTxt.text = PrjUtil.MakeMessage("<size=18>あと</size>") + (firstMstSpecial.days - loginBonusSpecial.day).ToString() + PrjUtil.MakeMessage("日"));
		IconItemCtrl.OnClick <>9__40;
		for (int i = 0; i < guiData.BonusObj01.Count; i++)
		{
			ItemStaticBase itemStaticBase = DataManager.DmItem.GetItemStaticBase(list[i].itemId);
			guiData.BonusObj01[i].BonusItem.Setup(itemStaticBase);
			IconItemCtrl bonusItem = guiData.BonusObj01[i].BonusItem;
			IconItemCtrl.OnClick onClick;
			if ((onClick = <>9__40) == null)
			{
				onClick = (<>9__40 = delegate(IconItemCtrl iic)
				{
					isRequestSkip = true;
				});
			}
			bonusItem.AddOnClickListener(onClick);
			guiData.BonusObj01[i].Num_Txt.text = "×" + list[i].itemNum.ToString();
			guiData.BonusObj01[i].Stamp.gameObject.SetActive(false);
			guiData.BonusObj01[i].Txt_Day.text = (i + 1).ToString() + "日目";
			guiData.BonusObj01[i].Txt_Item.text = itemStaticBase.GetName();
		}
		if (nextTopObj != null)
		{
			guiData.Window02.transform.Find("DayTotal/Num_DayTxt").GetComponent<PguiTextCtrl>().text = loginBonusTotal.day.ToString() + PrjUtil.MakeMessage("日目");
			guiData.Window02.transform.Find("Total_Normal/LoginBonus_TotalBonus_Normal/BonusItem/Icon_Item").GetComponent<IconItemCtrl>().Setup(normID);
			guiData.Window02.transform.Find("Total_Normal/LoginBonus_TotalBonus_Normal/BonusItem/Icon_Item").GetComponent<IconItemCtrl>().AddOnClickListener(delegate(IconItemCtrl iic)
			{
				isRequestSkip = true;
			});
			guiData.Window02.transform.Find("Total_Normal/LoginBonus_TotalBonus_Normal/DayInfo/Num_DayTxt").GetComponent<PguiTextCtrl>().text = firstMstTotal.days.ToString() + PrjUtil.MakeMessage("日");
			guiData.Window02.transform.Find("Total_Normal/LoginBonus_TotalBonus_Normal/Num_DayCount").GetComponent<PguiTextCtrl>().text = PrjUtil.MakeMessage("<size=18>あと</size>") + (firstMstTotal.days - loginBonusTotal.day).ToString() + PrjUtil.MakeMessage("日");
			guiData.Window02.transform.Find("Total_Special/LoginBonus_TotalBonus_Special/BonusItem/Icon_Item").GetComponent<IconItemCtrl>().Setup(specID);
			guiData.Window02.transform.Find("Total_Special/LoginBonus_TotalBonus_Special/BonusItem/Icon_Item").GetComponent<IconItemCtrl>().AddOnClickListener(delegate(IconItemCtrl iic)
			{
				isRequestSkip = true;
			});
			guiData.Window02.transform.Find("Total_Special/LoginBonus_TotalBonus_Special/DayInfo/Num_DayTxt").GetComponent<PguiTextCtrl>().text = firstMstSpecial.days.ToString() + PrjUtil.MakeMessage("日");
			guiData.Window02.transform.Find("Total_Special/LoginBonus_TotalBonus_Special/Num_DayCount").GetComponent<PguiTextCtrl>().text = PrjUtil.MakeMessage("<size=18>あと</size>") + (firstMstSpecial.days - loginBonusSpecial.day).ToString() + PrjUtil.MakeMessage("日");
			IconItemCtrl.OnClick <>9__41;
			for (int j = 0; j < guiData.BonusObj01.Count; j++)
			{
				ItemStaticBase itemStaticBase2 = DataManager.DmItem.GetItemStaticBase(list[j].itemId);
				SelLoginBonus.GuiFrameOne guiFrameOne = new SelLoginBonus.GuiFrameOne(guiData.Window02.transform.Find("LoginBonus_List/LoginBonus_day" + (j + 1).ToString("D2")));
				guiFrameOne.BonusItem.Setup(itemStaticBase2);
				IconItemCtrl bonusItem2 = guiFrameOne.BonusItem;
				IconItemCtrl.OnClick onClick2;
				if ((onClick2 = <>9__41) == null)
				{
					onClick2 = (<>9__41 = delegate(IconItemCtrl iic)
					{
						isRequestSkip = true;
					});
				}
				bonusItem2.AddOnClickListener(onClick2);
				guiFrameOne.Num_Txt.text = "×" + list[j].itemNum.ToString();
				guiFrameOne.Stamp.gameObject.SetActive(false);
				guiFrameOne.Txt_Day.text = (j + 1).ToString() + "日目";
				guiFrameOne.Txt_Item.text = itemStaticBase2.GetName();
			}
		}
		for (int k = 0; k < guiData.BonusObj01.Count; k++)
		{
			if (k + 1 < loginBonus.day)
			{
				guiData.BonusObj01[k].anime.ExPauseAnimationLastFrame(SimpleAnimation.ExPguiStatus.END);
				guiData.BonusObj01[k].Stamp.gameObject.SetActive(true);
			}
			else
			{
				guiData.BonusObj01[k].Stamp.gameObject.SetActive(false);
			}
			if (k + 1 == loginBonus.day)
			{
				guiData.AEImage_Stamp.transform.SetParent(guiData.BonusObj01[k].baseObj.transform, false);
				guiData.currentFrameAnim = guiData.BonusObj01[k].anime;
			}
			else if (k == loginBonus.day)
			{
				isNextEffect = true;
				guiData.AEImage_NextEff.transform.SetParent(guiData.BonusObj01[k].baseObj.transform, false);
				guiData.AEImage_NextEffBack.transform.SetParent(guiData.BonusObj01[k].baseObj.transform, false);
				guiData.AEImage_NextEffBack.transform.SetAsFirstSibling();
				guiData.BonusObj01[k].baseObj.transform.SetAsLastSibling();
			}
		}
		float scl = 1f;
		while ((scl += TimeManager.DeltaTime * 2f) < 2f && !isRequestSkip && !SelLoginBonus.allSkip)
		{
			guiData.FrontBG.localScale = Vector3.one * scl;
			if (scl < 1.1f)
			{
				isRequestSkip = false;
			}
			else if (isRequestSkip)
			{
				break;
			}
			yield return null;
		}
		guiData.FrontBG.localScale = Vector3.one * 2f;
		CriAtomExPlayback caep = SoundManager.Play("prd_se_login_bonus_auth", false, false);
		isAnimeEnd = false;
		guiData.Btn_Skip.gameObject.SetActive(true);
		guiData.BackObj.gameObject.SetActive(true);
		guiData.TopObj.gameObject.SetActive(true);
		guiData.AEImage_Txt.PlayAnimation(PguiAECtrl.AmimeType.START, delegate
		{
			isAnimeEnd = true;
		});
		guiData.Window01.ExResumeAnimation(null);
		if (!SelLoginBonus.allSkip)
		{
			yield return null;
		}
		scl = 0.2f;
		while ((scl -= TimeManager.DeltaTime) > 0f && !SelLoginBonus.allSkip)
		{
			yield return null;
		}
		isRequestSkip = false;
		while (!isAnimeEnd && !isRequestSkip && !SelLoginBonus.allSkip)
		{
			yield return null;
		}
		guiData.AEImage_Txt.m_AEImage.playOutTime = guiData.AEImage_Txt.m_AEImage.duration;
		renderTextureChara.postion = new Vector2(-480f, -150f);
		renderTextureChara.OnValidate();
		isAnimeEnd = false;
		RenderTextureChara.FinishCallback <>9__42;
		renderTextureChara.SetAnimation(CharaMotionDefine.ActKey.LOGIN_BONUS_ENTRY, false, 0f, delegate
		{
			isAnimeEnd = true;
			renderTextureChara.SetFacePack(FacePackData.Id2PackData("FACE_SMILE_2_B"));
			RenderTextureChara renderTextureChara2 = renderTextureChara;
			CharaMotionDefine.ActKey actKey = CharaMotionDefine.ActKey.JOY;
			bool flag = false;
			RenderTextureChara.FinishCallback finishCallback;
			if ((finishCallback = <>9__42) == null)
			{
				finishCallback = (<>9__42 = delegate
				{
					renderTextureChara.SetFacePack(FacePackData.Id2PackData("FACE_SMILE_1_A"));
					renderTextureChara.SetAnimation(CharaMotionDefine.ActKey.SCENARIO_STAND_BY, true);
				});
			}
			renderTextureChara2.SetAnimation(actKey, flag, finishCallback);
		});
		scl = 0.2f;
		while ((scl -= TimeManager.DeltaTime) > 0f && !SelLoginBonus.allSkip)
		{
			yield return null;
		}
		isRequestSkip = false;
		while (!isAnimeEnd && !isRequestSkip && !SelLoginBonus.allSkip)
		{
			yield return null;
		}
		CharaStaticData charaMst = DataManager.DmChara.GetCharaStaticData(charaId);
		guiData.Txt_CharaName.text = charaMst.GetName();
		guiData.Txt_Serif.text = charaMst.baseData.loginText;
		guiData.CharSerif.gameObject.SetActive(true);
		if (!SelLoginBonus.allSkip)
		{
			SoundManager.PlayVoice(charaMst.cueSheetName, VOICE_TYPE.LOG01);
			SoundManager.Play("prd_se_login_bonus_stamp", false, false);
		}
		isAnimeEnd = false;
		SimpleAnimation crntInfo = guiData.currentFrameAnim.transform.Find("Login_ItemInfo").GetComponent<SimpleAnimation>();
		crntInfo.transform.SetParent(renderTextureChara.transform.parent.parent, true);
		crntInfo.transform.SetSiblingIndex(renderTextureChara.transform.parent.GetSiblingIndex() + 1);
		SimpleAnimation.ExFinishCallback <>9__43;
		guiData.currentFrameAnim.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, delegate
		{
			crntInfo.gameObject.SetActive(true);
			SimpleAnimation crntInfo2 = crntInfo;
			SimpleAnimation.ExPguiStatus exPguiStatus = SimpleAnimation.ExPguiStatus.START;
			SimpleAnimation.ExFinishCallback exFinishCallback;
			if ((exFinishCallback = <>9__43) == null)
			{
				exFinishCallback = (<>9__43 = delegate
				{
					isAnimeEnd = true;
				});
			}
			crntInfo2.ExPlayAnimation(exPguiStatus, exFinishCallback);
		});
		scl = 0.2f;
		while ((scl -= TimeManager.DeltaTime) > 0f && !SelLoginBonus.allSkip)
		{
			yield return null;
		}
		isRequestSkip = false;
		while (!isAnimeEnd && !isRequestSkip && !SelLoginBonus.allSkip)
		{
			yield return null;
		}
		guiData.currentFrameAnim.ExPauseAnimationLastFrame(SimpleAnimation.ExPguiStatus.START);
		crntInfo.gameObject.SetActive(true);
		crntInfo.ExPauseAnimationLastFrame(SimpleAnimation.ExPguiStatus.START);
		guiData.Touch.SetActive(true);
		scl = 0.2f;
		while ((scl -= TimeManager.DeltaTime) > 0f && !SelLoginBonus.allSkip)
		{
			yield return null;
		}
		isRequestSkip = false;
		while (!isRequestSkip && !SelLoginBonus.allSkip)
		{
			yield return null;
		}
		guiData.Touch.SetActive(false);
		guiData.currentFrameAnim.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, null);
		crntInfo.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, null);
		scl = 0.2f;
		while ((scl -= TimeManager.DeltaTime) > 0f && !SelLoginBonus.allSkip)
		{
			yield return null;
		}
		isRequestSkip = false;
		while ((guiData.currentFrameAnim.ExIsPlaying() || crntInfo.ExIsPlaying()) && !isRequestSkip && !SelLoginBonus.allSkip)
		{
			yield return null;
		}
		guiData.currentFrameAnim.ExPauseAnimationLastFrame(SimpleAnimation.ExPguiStatus.END);
		crntInfo.gameObject.SetActive(false);
		if (firstMstTotal.days == loginBonusTotal.day)
		{
			guiData.TotalGetNowDay.text = firstMstTotal.days.ToString() + PrjUtil.MakeMessage("日");
			guiData.TotalGetItemName.text = normID.staticData.GetName();
			guiNormal.baseObj.transform.SetAsFirstSibling();
			guiSpecial.baseObj.transform.SetAsFirstSibling();
			isAnimeEnd = false;
			if (!SelLoginBonus.allSkip)
			{
				SoundManager.Play("prd_se_login_bonus_get", false, false);
			}
			guiData.AEImage_Total.PlayAnimation(PguiAECtrl.AmimeType.START, delegate
			{
				isAnimeEnd = true;
			});
			scl = 0.2f;
			while ((scl -= TimeManager.DeltaTime) > 0f && !SelLoginBonus.allSkip)
			{
				yield return null;
			}
			isRequestSkip = false;
			while (!isAnimeEnd && !isRequestSkip && !SelLoginBonus.allSkip)
			{
				yield return null;
			}
			guiData.AEImage_Total.PauseAnimationLastFrame(PguiAECtrl.AmimeType.START);
			isAnimeEnd = false;
			guiData.TotalGet.PlayAnimation(PguiAECtrl.AmimeType.START, delegate
			{
				isAnimeEnd = true;
			});
			guiData.TotalGetStamp.PauseAnimation(PguiAECtrl.AmimeType.START, null);
			scl = 0.2f;
			while ((scl -= TimeManager.DeltaTime) > 0f && !SelLoginBonus.allSkip)
			{
				yield return null;
			}
			isRequestSkip = false;
			while (!isAnimeEnd && !isRequestSkip && !SelLoginBonus.allSkip)
			{
				yield return null;
			}
			guiData.TotalGet.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
			guiData.TotalGetStamp.ResumeAnimation();
			guiData.Touch.SetActive(true);
			scl = 0.2f;
			while ((scl -= TimeManager.DeltaTime) > 0f && !SelLoginBonus.allSkip)
			{
				yield return null;
			}
			isRequestSkip = false;
			while (!isRequestSkip && !SelLoginBonus.allSkip)
			{
				yield return null;
			}
			guiData.Touch.SetActive(false);
			if (!SelLoginBonus.allSkip)
			{
				SoundManager.Play("prd_se_login_bonus_get_b", false, false);
			}
			isAnimeEnd = false;
			guiData.TotalGet.PlayAnimation(PguiAECtrl.AmimeType.END, delegate
			{
				isAnimeEnd = true;
			});
			guiData.TotalGetStamp.PauseAnimationLastFrame(PguiAECtrl.AmimeType.START);
			scl = 0.2f;
			while ((scl -= TimeManager.DeltaTime) > 0f && !SelLoginBonus.allSkip)
			{
				yield return null;
			}
			isRequestSkip = false;
			while (!isAnimeEnd && !isRequestSkip && !SelLoginBonus.allSkip)
			{
				yield return null;
			}
			guiData.TotalGet.PauseAnimationLastFrame(PguiAECtrl.AmimeType.END);
			isAnimeEnd = false;
			guiData.Normal_ItemIcon.Setup(new ItemData(secondMstTotal.itemId, secondMstTotal.itemNum));
			guiData.Normal_ItemIcon.AddOnClickListener(delegate(IconItemCtrl iic)
			{
				isRequestSkip = true;
			});
			guiData.Normal_Num_NowDayTxt.text = secondMstTotal.days.ToString() + PrjUtil.MakeMessage("日");
			guiData.Normal_Num_InfoTxt.text = PrjUtil.MakeMessage("<size=18>あと</size>") + (secondMstTotal.days - loginBonusTotal.day).ToString() + PrjUtil.MakeMessage("日");
			guiData.AEImage_Total.GetComponent<PguiReplaceAECtrl>().Replace("NEXT");
			guiData.AEImage_Total.PlayAnimation(PguiAECtrl.AmimeType.START, delegate
			{
				isAnimeEnd = true;
			});
			if (nextTopObj != null)
			{
				guiData.Window02.transform.Find("Total_Normal/LoginBonus_TotalBonus_Normal/BonusItem/Icon_Item").GetComponent<IconItemCtrl>().Setup(new ItemData(secondMstTotal.itemId, secondMstTotal.itemNum));
				guiData.Window02.transform.Find("Total_Normal/LoginBonus_TotalBonus_Normal/BonusItem/Icon_Item").GetComponent<IconItemCtrl>().AddOnClickListener(delegate(IconItemCtrl iic)
				{
					isRequestSkip = true;
				});
				guiData.Window02.transform.Find("Total_Normal/LoginBonus_TotalBonus_Normal/DayInfo/Num_DayTxt").GetComponent<PguiTextCtrl>().text = secondMstTotal.days.ToString() + PrjUtil.MakeMessage("日");
				guiData.Window02.transform.Find("Total_Normal/LoginBonus_TotalBonus_Normal/Num_DayCount").GetComponent<PguiTextCtrl>().text = PrjUtil.MakeMessage("<size=18>あと</size>") + (secondMstTotal.days - loginBonusTotal.day).ToString() + PrjUtil.MakeMessage("日");
				guiData.Window02.transform.Find("Total_Normal/AEImage").GetComponent<PguiReplaceAECtrl>().Replace("NEXT");
				guiData.Window02.transform.Find("Total_Normal/AEImage").GetComponent<PguiAECtrl>().PauseAnimationLastFrame(PguiAECtrl.AmimeType.START);
			}
			scl = 0.2f;
			while ((scl -= TimeManager.DeltaTime) > 0f && !SelLoginBonus.allSkip)
			{
				yield return null;
			}
			isRequestSkip = false;
			while (!isAnimeEnd && !isRequestSkip && !SelLoginBonus.allSkip)
			{
				yield return null;
			}
			guiData.AEImage_Total.PauseAnimationLastFrame(PguiAECtrl.AmimeType.START);
		}
		if (firstMstSpecial.days == loginBonusSpecial.day)
		{
			guiData.TotalGetNowDay.text = firstMstSpecial.days.ToString() + PrjUtil.MakeMessage("日");
			guiData.TotalGetItemName.text = specID.staticData.GetName();
			guiSpecial.baseObj.transform.SetAsFirstSibling();
			guiNormal.baseObj.transform.SetAsFirstSibling();
			isAnimeEnd = false;
			if (!SelLoginBonus.allSkip)
			{
				SoundManager.Play("prd_se_login_bonus_get", false, false);
			}
			guiData.AEImage_Special.PlayAnimation(PguiAECtrl.AmimeType.START, delegate
			{
				isAnimeEnd = true;
			});
			scl = 0.2f;
			while ((scl -= TimeManager.DeltaTime) > 0f && !SelLoginBonus.allSkip)
			{
				yield return null;
			}
			isRequestSkip = false;
			while (!isAnimeEnd && !isRequestSkip && !SelLoginBonus.allSkip)
			{
				yield return null;
			}
			guiData.AEImage_Special.PauseAnimationLastFrame(PguiAECtrl.AmimeType.START);
			isAnimeEnd = false;
			guiData.TotalGet.PlayAnimation(PguiAECtrl.AmimeType.START, delegate
			{
				isAnimeEnd = true;
			});
			guiData.TotalGetStamp.PauseAnimation(PguiAECtrl.AmimeType.START, null);
			scl = 0.2f;
			while ((scl -= TimeManager.DeltaTime) > 0f && !SelLoginBonus.allSkip)
			{
				yield return null;
			}
			isRequestSkip = false;
			while (!isAnimeEnd && !isRequestSkip && !SelLoginBonus.allSkip)
			{
				yield return null;
			}
			guiData.TotalGet.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
			guiData.TotalGetStamp.ResumeAnimation();
			guiData.Touch.SetActive(true);
			scl = 0.2f;
			while ((scl -= TimeManager.DeltaTime) > 0f && !SelLoginBonus.allSkip)
			{
				yield return null;
			}
			isRequestSkip = false;
			while (!isRequestSkip && !SelLoginBonus.allSkip)
			{
				yield return null;
			}
			guiData.Touch.SetActive(false);
			if (!SelLoginBonus.allSkip)
			{
				SoundManager.Play("prd_se_login_bonus_get_b", false, false);
			}
			isAnimeEnd = false;
			guiData.TotalGet.PlayAnimation(PguiAECtrl.AmimeType.END, delegate
			{
				isAnimeEnd = true;
			});
			guiData.TotalGetStamp.PauseAnimationLastFrame(PguiAECtrl.AmimeType.START);
			scl = 0.2f;
			while ((scl -= TimeManager.DeltaTime) > 0f && !SelLoginBonus.allSkip)
			{
				yield return null;
			}
			isRequestSkip = false;
			while (!isAnimeEnd && !isRequestSkip && !SelLoginBonus.allSkip)
			{
				yield return null;
			}
			guiData.TotalGet.PauseAnimationLastFrame(PguiAECtrl.AmimeType.END);
			isAnimeEnd = false;
			guiData.Special_ItemIcon.Setup(new ItemData(secondMstSpecial.itemId, secondMstSpecial.itemNum));
			guiData.Special_ItemIcon.AddOnClickListener(delegate(IconItemCtrl iic)
			{
				isRequestSkip = true;
			});
			guiData.Special_Num_NowDayTxt.text = secondMstSpecial.days.ToString() + PrjUtil.MakeMessage("日");
			guiData.Special_Num_InfoTxt.text = PrjUtil.MakeMessage("<size=18>あと</size>") + (secondMstSpecial.days - loginBonusSpecial.day).ToString() + PrjUtil.MakeMessage("日");
			guiData.AEImage_Special.GetComponent<PguiReplaceAECtrl>().Replace("NEXT");
			guiData.AEImage_Special.PlayAnimation(PguiAECtrl.AmimeType.START, delegate
			{
				isAnimeEnd = true;
			});
			if (nextTopObj != null)
			{
				guiData.Window02.transform.Find("Total_Special/LoginBonus_TotalBonus_Special/BonusItem/Icon_Item").GetComponent<IconItemCtrl>().Setup(new ItemData(secondMstSpecial.itemId, secondMstSpecial.itemNum));
				guiData.Window02.transform.Find("Total_Special/LoginBonus_TotalBonus_Special/BonusItem/Icon_Item").GetComponent<IconItemCtrl>().AddOnClickListener(delegate(IconItemCtrl iic)
				{
					isRequestSkip = true;
				});
				guiData.Window02.transform.Find("Total_Special/LoginBonus_TotalBonus_Special/DayInfo/Num_DayTxt").GetComponent<PguiTextCtrl>().text = secondMstSpecial.days.ToString() + PrjUtil.MakeMessage("日");
				guiData.Window02.transform.Find("Total_Special/LoginBonus_TotalBonus_Special/Num_DayCount").GetComponent<PguiTextCtrl>().text = PrjUtil.MakeMessage("<size=18>あと</size>") + (secondMstSpecial.days - loginBonusSpecial.day).ToString() + PrjUtil.MakeMessage("日");
				guiData.Window02.transform.Find("Total_Special/AEImage").GetComponent<PguiReplaceAECtrl>().Replace("NEXT");
				guiData.Window02.transform.Find("Total_Special/AEImage").GetComponent<PguiAECtrl>().PauseAnimationLastFrame(PguiAECtrl.AmimeType.START);
			}
			scl = 0.2f;
			while ((scl -= TimeManager.DeltaTime) > 0f && !SelLoginBonus.allSkip)
			{
				yield return null;
			}
			isRequestSkip = false;
			while (!isAnimeEnd && !isRequestSkip && !SelLoginBonus.allSkip)
			{
				yield return null;
			}
			guiData.AEImage_Special.PauseAnimationLastFrame(PguiAECtrl.AmimeType.START);
		}
		if (isNextEffect)
		{
			if (!SelLoginBonus.allSkip)
			{
				SoundManager.PlayVoice(charaMst.cueSheetName, VOICE_TYPE.LOG02);
			}
			if (nextTopObj != null)
			{
				isAnimeEnd = false;
				nextTopObj.gameObject.SetActive(true);
				guiData.Window02.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, delegate
				{
					isAnimeEnd = true;
				});
				scl = 0.2f;
				while ((scl -= TimeManager.DeltaTime) > 0f && !SelLoginBonus.allSkip)
				{
					yield return null;
				}
				isRequestSkip = false;
				while (!isAnimeEnd && !isRequestSkip && !SelLoginBonus.allSkip)
				{
					yield return null;
				}
				guiData.Window02.ExPauseAnimationLastFrame(SimpleAnimation.ExPguiStatus.START);
			}
			guiData.Txt_Serif.text = PrjUtil.MakeMessage(charaMst.baseData.loginSubText);
			guiData.AEImage_NextEff.ResumeAnimation();
			guiData.AEImage_NextEffBack.ResumeAnimation();
			if (!SelLoginBonus.allSkip)
			{
				yield return null;
			}
			guiData.Touch.SetActive(true);
			scl = 0.2f;
			while ((scl -= TimeManager.DeltaTime) > 0f && !SelLoginBonus.allSkip)
			{
				yield return null;
			}
			isRequestSkip = false;
			while (!isRequestSkip && !SelLoginBonus.allSkip)
			{
				yield return null;
			}
			guiData.Touch.SetActive(false);
		}
		caep.Stop();
		Object.Destroy(renderTextureChara.gameObject);
		Object.Destroy(guiNormal.baseObj);
		Object.Destroy(guiSpecial.baseObj);
		Object.Destroy(guiData.baseObj);
		yield break;
	}

	private static bool allSkip;

	public static DateTime rcvDate = DateTime.MinValue;

	public bool isDebug;

	private IEnumerator debugAction;

	public class GUI
	{
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Btn_Skip = baseTr.Find("Btn_Skip").GetComponent<PguiButtonCtrl>();
			this.Txt_Serif = baseTr.Find("CharSerif/Txt_Serif").GetComponent<PguiTextCtrl>();
			this.Window01 = baseTr.Find("Base/Window01").GetComponent<SimpleAnimation>();
			this.Window02 = Object.Instantiate<GameObject>(this.Window01.gameObject, this.Window01.transform.parent).GetComponent<SimpleAnimation>();
			this.Daily_NowDay = baseTr.Find("Base/Window01/DayTotal/Num_DayTxt").GetComponent<PguiTextCtrl>();
			this.AEImage_Txt = baseTr.Find("AEImage_Txt").GetComponent<PguiAECtrl>();
			this.CharSerif = baseTr.Find("CharSerif").GetComponent<PguiImageCtrl>();
			this.BackObj = baseTr.Find("Base/TexBack").gameObject;
			this.TopObj = baseTr.Find("Base/Window01/Tex_Top").gameObject;
			this.FrontBG = baseTr.Find("FrontBG");
			this.Txt_CharaName = baseTr.Find("CharSerif/NameBase/Txt_CharaName").GetComponent<PguiTextCtrl>();
			this.AEImage_Stamp = baseTr.Find("Base/Window01/LoginBonus_List/LoginBonus_AEImage_Stamp").GetComponent<PguiAECtrl>();
			this.AEImage_NextEff = baseTr.Find("Base/Window01/LoginBonus_List/LoginBonus_AEImage_EffKira").GetComponent<PguiAECtrl>();
			this.AEImage_NextEffBack = baseTr.Find("Base/Window01/LoginBonus_List/LoginBonus_AEImage_EffCircle").GetComponent<PguiAECtrl>();
			Object.Destroy(this.Window02.transform.Find("LoginBonus_List/LoginBonus_AEImage_Stamp").gameObject);
			Object.Destroy(this.Window02.transform.Find("LoginBonus_List/LoginBonus_AEImage_EffKira").gameObject);
			Object.Destroy(this.Window02.transform.Find("LoginBonus_List/LoginBonus_AEImage_EffCircle").gameObject);
			this.BonusObj01 = new List<SelLoginBonus.GuiFrameOne>
			{
				new SelLoginBonus.GuiFrameOne(baseTr.Find("Base/Window01/LoginBonus_List/LoginBonus_day01")),
				new SelLoginBonus.GuiFrameOne(baseTr.Find("Base/Window01/LoginBonus_List/LoginBonus_day02")),
				new SelLoginBonus.GuiFrameOne(baseTr.Find("Base/Window01/LoginBonus_List/LoginBonus_day03")),
				new SelLoginBonus.GuiFrameOne(baseTr.Find("Base/Window01/LoginBonus_List/LoginBonus_day04")),
				new SelLoginBonus.GuiFrameOne(baseTr.Find("Base/Window01/LoginBonus_List/LoginBonus_day05")),
				new SelLoginBonus.GuiFrameOne(baseTr.Find("Base/Window01/LoginBonus_List/LoginBonus_day06")),
				new SelLoginBonus.GuiFrameOne(baseTr.Find("Base/Window01/LoginBonus_List/LoginBonus_day07"))
			};
			this.AEImage_Total = baseTr.Find("Base/Window01/Total_Normal/AEImage").GetComponent<PguiAECtrl>();
			this.AEImage_Total.GetComponent<PguiReplaceAECtrl>().Replace("OUT");
			this.AEImage_Total.PauseAnimation(PguiAECtrl.AmimeType.START, null);
			this.Normal_ItemIcon = baseTr.Find("Base/Window01/Total_Normal/LoginBonus_TotalBonus_Normal/BonusItem/Icon_Item").GetComponent<IconItemCtrl>();
			this.Normal_Num_NowDayTxt = baseTr.Find("Base/Window01/Total_Normal/LoginBonus_TotalBonus_Normal/DayInfo/Num_DayTxt").GetComponent<PguiTextCtrl>();
			this.Normal_Num_InfoTxt = baseTr.Find("Base/Window01/Total_Normal/LoginBonus_TotalBonus_Normal/Num_DayCount").GetComponent<PguiTextCtrl>();
			this.AEImage_Special = baseTr.Find("Base/Window01/Total_Special/AEImage").GetComponent<PguiAECtrl>();
			this.AEImage_Special.GetComponent<PguiReplaceAECtrl>().Replace("OUT");
			this.AEImage_Special.PauseAnimation(PguiAECtrl.AmimeType.START, null);
			this.Special_ItemIcon = baseTr.Find("Base/Window01/Total_Special/LoginBonus_TotalBonus_Special/BonusItem/Icon_Item").GetComponent<IconItemCtrl>();
			this.Special_Num_NowDayTxt = baseTr.Find("Base/Window01/Total_Special/LoginBonus_TotalBonus_Special/DayInfo/Num_DayTxt").GetComponent<PguiTextCtrl>();
			this.Special_Num_InfoTxt = baseTr.Find("Base/Window01/Total_Special/LoginBonus_TotalBonus_Special/Num_DayCount").GetComponent<PguiTextCtrl>();
			this.TotalGet = baseTr.Find("Total_Get/AEImage").GetComponent<PguiAECtrl>();
			this.TotalGetNull = baseTr.Find("Total_Get/Null_Total");
			this.TotalGetStamp = baseTr.Find("Total_Get/Null_Total/LoginBonus_AEImage_Stamp").GetComponent<PguiAECtrl>();
			this.TotalGetNowDay = baseTr.Find("Total_Get/Info/DayInfo/Num_DayTxt").GetComponent<PguiTextCtrl>();
			this.TotalGetItemName = baseTr.Find("Total_Get/Info/ItemInfo/Txt_Item").GetComponent<PguiTextCtrl>();
			this.Touch = baseTr.Find("Txt_Touch").gameObject;
		}

		public GameObject baseObj;

		public PguiButtonCtrl Btn_Skip;

		public PguiTextCtrl Txt_Serif;

		public SimpleAnimation Window01;

		public SimpleAnimation Window02;

		public PguiTextCtrl Daily_NowDay;

		public PguiAECtrl AEImage_Txt;

		public PguiAECtrl AEImage_Stamp;

		public PguiAECtrl AEImage_NextEff;

		public PguiAECtrl AEImage_NextEffBack;

		public PguiImageCtrl CharSerif;

		public GameObject BackObj;

		public GameObject TopObj;

		public Transform FrontBG;

		public PguiTextCtrl Txt_CharaName;

		public List<SelLoginBonus.GuiFrameOne> BonusObj01;

		public PguiAECtrl AEImage_Total;

		public IconItemCtrl Normal_ItemIcon;

		public PguiTextCtrl Normal_Num_InfoTxt;

		public PguiTextCtrl Normal_Num_NowDayTxt;

		public PguiAECtrl AEImage_Special;

		public IconItemCtrl Special_ItemIcon;

		public PguiTextCtrl Special_Num_NowDayTxt;

		public PguiTextCtrl Special_Num_InfoTxt;

		public PguiAECtrl TotalGet;

		public Transform TotalGetNull;

		public PguiAECtrl TotalGetStamp;

		public PguiTextCtrl TotalGetNowDay;

		public PguiTextCtrl TotalGetItemName;

		public GameObject Touch;

		public SimpleAnimation currentFrameAnim;
	}

	public class GuiFrameOne
	{
		public GuiFrameOne(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.anime = baseTr.GetComponent<SimpleAnimation>();
			this.Tex_ItemPlate = baseTr.Find("Base/Tex_ItemPlate").GetComponent<PguiImageCtrl>();
			this.Tex_Day = baseTr.Find("Base/Tex_Day").GetComponent<PguiImageCtrl>();
			this.Num_Txt = baseTr.Find("Base/Tex_ItemPlate/Num_Txt").GetComponent<PguiTextCtrl>();
			this.Txt_Day = baseTr.Find("Base/Tex_Day/Txt").GetComponent<PguiTextCtrl>();
			this.Stamp = baseTr.Find("Base/Stamp").GetComponent<PguiImageCtrl>();
			this.BonusItem = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item, baseTr.Find("Base/BonusItem")).GetComponent<IconItemCtrl>();
			this.Txt_Item = baseTr.Find("Login_ItemInfo/Txt_Item").GetComponent<PguiTextCtrl>();
		}

		public GameObject baseObj;

		public PguiImageCtrl Tex_ItemPlate;

		public PguiImageCtrl Tex_Day;

		public PguiTextCtrl Num_Txt;

		public PguiTextCtrl Txt_Day;

		public PguiImageCtrl Stamp;

		public IconItemCtrl BonusItem;

		public PguiTextCtrl Txt_Item;

		public SimpleAnimation anime;
	}

	public class GuiTotalGet
	{
		public GuiTotalGet(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.ItemIcon = baseTr.Find("BonusItem/Icon_Item").GetComponent<IconItemCtrl>();
			this.Num_NowDayTxt = baseTr.Find("DayInfo/Num_DayTxt").GetComponent<PguiTextCtrl>();
			this.Num_InfoTxt = baseTr.Find("Num_DayCount").GetComponent<PguiTextCtrl>();
		}

		public GameObject baseObj;

		public IconItemCtrl ItemIcon;

		public PguiTextCtrl Num_InfoTxt;

		public PguiTextCtrl Num_NowDayTxt;
	}

	public class GuiCampaign
	{
		public GuiCampaign(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Btn_Skip = baseTr.Find("Btn_Skip").GetComponent<PguiButtonCtrl>();
			this.Fukidashi = baseTr.Find("Fukidashi").GetComponent<PguiImageCtrl>();
			this.FrontBG = baseTr.Find("FrontBG").GetComponent<PguiRawImageCtrl>();
			this.Tex = baseTr.Find("Fukidashi/Tex01").GetComponent<PguiTextCtrl>();
			this.Touch = baseTr.Find("Txt_Touch").gameObject;
		}

		public void MakeList(int cnt)
		{
			foreach (SelLoginBonus.GuiCampaignFrameOne guiCampaignFrameOne in this.oneList)
			{
				Object.Destroy(guiCampaignFrameOne.baseObj);
				guiCampaignFrameOne.baseObj = null;
			}
			this.oneList = new List<SelLoginBonus.GuiCampaignFrameOne>();
			Transform transform = this.baseObj.transform.Find((cnt > 7) ? "LoginCanpaign_ListSet_Long" : "LoginCanpaign_ListSet_Short");
			GridLayoutGroup component = transform.GetComponent<GridLayoutGroup>();
			if (component != null && cnt > 7)
			{
				int num = ((cnt > 10) ? 7 : 5);
				component.constraintCount = num;
			}
			GameObject gameObject = AssetManager.GetAssetData("SceneLoginBonus/GUI/Prefab/LoginCampaign_ItemPlate") as GameObject;
			for (int i = 0; i < cnt; i++)
			{
				int num2 = i;
				if (cnt > 7 && i % 2 != 0)
				{
					num2 += 100;
				}
				this.oneList.Add(new SelLoginBonus.GuiCampaignFrameOne(Object.Instantiate<GameObject>(gameObject, transform).transform, num2));
			}
			transform.gameObject.SetActive(false);
			transform.gameObject.SetActive(true);
			this.oneList.Sort((SelLoginBonus.GuiCampaignFrameOne a, SelLoginBonus.GuiCampaignFrameOne b) => a.sort.CompareTo(b.sort));
			List<int> list = new List<int>();
			if (cnt == 7 || cnt >= 13)
			{
				list.Add(0);
				list.Add(6);
				list.Add(7);
				list.Add(13);
			}
			foreach (int num3 in list)
			{
				if (num3 < cnt)
				{
					Vector3 localPosition = this.oneList[num3].Txt_Item.transform.parent.localPosition;
					localPosition.x += ((num3 % 7 == 0) ? 40f : (-40f));
					this.oneList[num3].Txt_Item.transform.parent.localPosition = localPosition;
				}
			}
		}

		public GameObject baseObj;

		public PguiButtonCtrl Btn_Skip;

		public PguiImageCtrl Fukidashi;

		public PguiRawImageCtrl FrontBG;

		public PguiTextCtrl Tex;

		public GameObject Touch;

		public List<SelLoginBonus.GuiCampaignFrameOne> oneList = new List<SelLoginBonus.GuiCampaignFrameOne>();
	}

	public class GuiCampaignFrameOne
	{
		public GuiCampaignFrameOne(Transform baseTr, int no)
		{
			this.baseObj = baseTr.gameObject;
			this.Tex_ItemPlate = baseTr.Find("Base/Tex_ItemPlate").GetComponent<PguiImageCtrl>();
			this.Tex_bk = baseTr.Find("Base/Tex_bk").GetComponent<PguiImageCtrl>();
			this.Stamp = baseTr.Find("Base/Stamp").GetComponent<PguiImageCtrl>();
			this.Num_Txt = baseTr.Find("Base/Tex_ItemPlate/Num_Txt").GetComponent<PguiTextCtrl>();
			this.Anim = baseTr.GetComponent<SimpleAnimation>();
			this.BonusItem = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item, baseTr.Find("Base/BonusItem_Img")).GetComponent<IconItemCtrl>();
			this.Txt_Day = baseTr.Find("Base/Tex_Day/Txt").GetComponent<PguiTextCtrl>();
			this.Txt_Item = baseTr.Find("Login_ItemInfo/Txt_Item").GetComponent<PguiTextCtrl>();
			this.sort = no;
		}

		public GameObject baseObj;

		public PguiImageCtrl Tex_ItemPlate;

		public PguiImageCtrl Tex_bk;

		public PguiImageCtrl Stamp;

		public PguiTextCtrl Num_Txt;

		public SimpleAnimation Anim;

		public IconItemCtrl BonusItem;

		public PguiTextCtrl Txt_Day;

		public PguiTextCtrl Txt_Item;

		public int sort;
	}
}
