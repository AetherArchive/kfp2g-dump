using System;
using System.Collections.Generic;
using SGNFW.Common;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PguiOpenWindowCtrl : PguiBehaviour
{
	public PguiOpenWindowCtrl.WINDOW_TYPE windowType
	{
		get
		{
			return this.m_windowType;
		}
	}

	public GameObject choiceR
	{
		get
		{
			return this.m_BtnChoiceObj[2].gameObject;
		}
	}

	public RectTransform WindowRectTransform
	{
		get
		{
			return this.m_WindowRectTransform;
		}
	}

	public PguiTextCtrl MassageText
	{
		get
		{
			if (this.m_MassageText != null)
			{
				return this.m_MassageText.GetComponent<PguiTextCtrl>();
			}
			return null;
		}
	}

	public void Setup(string titleText, string massageText, List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>> buttonList, bool isDispClose, PguiOpenWindowCtrl.Callback callback, UnityAction finishedCloseCb = null, bool isThreeButton = false)
	{
		if (this.m_TitleText != null && titleText != null)
		{
			this.m_TitleText.text = titleText;
		}
		if (this.m_MassageText != null && massageText != null)
		{
			this.m_MassageText.text = massageText;
		}
		if (buttonList != null)
		{
			switch (buttonList.Count)
			{
			case 0:
				this.m_BtnChoice[0].SetActive(false);
				this.m_BtnChoice[1].SetActive(false);
				this.m_BtnChoice[2].SetActive(false);
				break;
			case 1:
				this.m_BtnChoice[0].SetActive(false);
				this.m_BtnChoice[1].SetActive(true);
				this.m_BtnChoice[2].SetActive(false);
				this.SettingChoiceButton(this.m_BtnChoice[1], buttonList[0], 0);
				break;
			case 2:
				this.m_BtnChoice[0].SetActive(true);
				this.m_BtnChoice[1].SetActive(false);
				this.m_BtnChoice[2].SetActive(true);
				this.SettingChoiceButton(this.m_BtnChoice[0], buttonList[0], 0);
				this.SettingChoiceButton(this.m_BtnChoice[2], buttonList[1], 1);
				break;
			case 3:
				this.m_BtnChoice[0].SetActive(true);
				this.m_BtnChoice[1].SetActive(true);
				this.m_BtnChoice[2].SetActive(true);
				this.SettingChoiceButton(this.m_BtnChoice[0], buttonList[0], 0);
				this.SettingChoiceButton(this.m_BtnChoice[1], buttonList[1], 1);
				this.SettingChoiceButton(this.m_BtnChoice[2], buttonList[2], 2);
				break;
			}
		}
		if (this.m_BtnCloseObj != null)
		{
			this.m_BtnCloseObj.gameObject.SetActive(isDispClose);
		}
		if (isThreeButton)
		{
			Vector3 vector = this.m_BtnChoice[0].m_Button.transform.localPosition;
			vector.x = -240f;
			this.m_BtnChoice[0].m_Button.transform.localPosition = vector;
			vector = this.m_BtnChoice[2].m_Button.transform.localPosition;
			vector.x = 240f;
			this.m_BtnChoice[2].m_Button.transform.localPosition = vector;
		}
		else if (this.m_BtnChoice != null && this.m_BtnChoice[0].m_Button != null && this.m_BtnChoice[2].m_Button != null && this.m_BtnChoice[0].m_Button.transform.localPosition.x == -240f && this.m_BtnChoice[2].m_Button.transform.localPosition.x == 240f)
		{
			Vector3 vector2 = this.m_BtnChoice[0].m_Button.transform.localPosition;
			vector2.x = -135f;
			this.m_BtnChoice[0].m_Button.transform.localPosition = vector2;
			vector2 = this.m_BtnChoice[2].m_Button.transform.localPosition;
			vector2.x = 135f;
			this.m_BtnChoice[2].m_Button.transform.localPosition = vector2;
		}
		this.SetupCloseBtnAndroidBackKey();
		this.m_CallBack = callback;
		this.m_FinishedCloseCallBack = finishedCloseCb;
	}

	public void SetupByNoStone(int needStoneNum, int useItemId, PguiOpenWindowCtrl.Callback callback)
	{
		bool flag = useItemId == 30002;
		if (this.m_windowType != PguiOpenWindowCtrl.WINDOW_TYPE.NO_STONE)
		{
			Verbose<PrjLog>.LogError("Error : PguiOpenWindowCtrl.SetupByNoStone : タイプの違うウィンドウ", null);
			return;
		}
		this.m_TitleText.text = PrjUtil.MakeMessage("確認");
		PguiImageCtrl component = this.m_BtnCloseObj.transform.parent.Find("ItemInfo").GetComponent<PguiImageCtrl>();
		PguiImageCtrl component2 = this.m_BtnCloseObj.transform.parent.Find("ItemInfo_ChargeOnly").GetComponent<PguiImageCtrl>();
		if (this.noStoneDefaultMassage == null)
		{
			this.noStoneDefaultMassage = new string[3];
			this.noStoneDefaultMassage[0] = this.m_MassageText.text;
			this.noStoneDefaultMassage[1] = component.transform.Find("Num_OwnAll").GetComponent<PguiTextCtrl>().text;
			this.noStoneDefaultMassage[2] = component.transform.Find("Num_OwnInfo").GetComponent<PguiTextCtrl>().text;
		}
		PguiTextCtrl pguiTextCtrl = null;
		bool flag2 = false;
		if (useItemId - 30001 <= 1 || useItemId == 30100)
		{
			flag2 = true;
		}
		PguiTextCtrl pguiTextCtrl2;
		PguiTextCtrl pguiTextCtrl3;
		if (flag || !flag2)
		{
			pguiTextCtrl2 = component2.transform.Find("Massage").GetComponent<PguiTextCtrl>();
			pguiTextCtrl3 = component2.transform.Find("Num_OwnAll").GetComponent<PguiTextCtrl>();
		}
		else
		{
			pguiTextCtrl2 = component.transform.Find("Massage").GetComponent<PguiTextCtrl>();
			pguiTextCtrl3 = component.transform.Find("Num_OwnAll").GetComponent<PguiTextCtrl>();
			pguiTextCtrl = component.transform.Find("Num_OwnInfo").GetComponent<PguiTextCtrl>();
		}
		if (flag2)
		{
			this.m_BtnChoiceObj[0].gameObject.SetActive(true);
			this.m_BtnChoiceObj[1].gameObject.SetActive(false);
			this.m_BtnChoiceObj[2].gameObject.SetActive(true);
		}
		else
		{
			this.m_BtnChoiceObj[0].gameObject.SetActive(false);
			this.m_BtnChoiceObj[1].gameObject.SetActive(true);
			this.m_BtnChoiceObj[2].gameObject.SetActive(false);
		}
		int num = DataManager.DmItem.GetUserItemData(30001).num;
		int num2 = DataManager.DmItem.GetUserItemData(30100).num;
		int num3 = DataManager.DmItem.GetUserItemData(useItemId).num;
		component.gameObject.SetActive(!flag && flag2);
		component2.gameObject.SetActive(flag || !flag2);
		if (flag || !flag2)
		{
			pguiTextCtrl2.text = this.noStoneDefaultMassage[0].Replace("Param01", (needStoneNum - num3).ToString()).Replace("キラキラ", DataManager.DmItem.GetItemStaticBase(useItemId).GetName());
			pguiTextCtrl3.text = this.noStoneDefaultMassage[1].Replace("Param01", num3.ToString());
		}
		else
		{
			pguiTextCtrl2.text = this.noStoneDefaultMassage[0].Replace("Param01", (needStoneNum - DataManager.DmItem.GetUserItemData(30100).num).ToString());
			pguiTextCtrl3.text = this.noStoneDefaultMassage[1].Replace("Param01", DataManager.DmItem.GetUserItemData(30100).num.ToString());
			if (pguiTextCtrl)
			{
				pguiTextCtrl.text = this.noStoneDefaultMassage[2].Replace("Param01", DataManager.DmItem.GetUserItemData(30002).num.ToString()).Replace("Param02", DataManager.DmItem.GetUserItemData(30001).num.ToString());
			}
		}
		this.m_BtnChoice[0].m_Index = 0;
		this.m_BtnChoice[2].m_Index = 1;
		this.m_BtnChoice[0].m_Type = PguiOpenWindowCtrl.BTN_TYPE.NEGATIVE;
		this.m_BtnChoice[2].m_Type = PguiOpenWindowCtrl.BTN_TYPE.POSITIVE;
		this.m_CallBack = callback;
		this.SetupCloseBtnAndroidBackKey();
	}

	public void SetupByUseItem(string titleText, string massageText, PguiOpenWindowCtrl.Callback callback, int needNum, int haveNum, bool isPurchaseConfirm = false)
	{
		if (this.m_windowType != PguiOpenWindowCtrl.WINDOW_TYPE.USE_ITEM)
		{
			Verbose<PrjLog>.LogError("Error : PguiOpenWindowCtrl.SetupByUseItem : タイプの違うウィンドウ", null);
			return;
		}
		this.m_TitleText.text = titleText;
		this.m_MassageText.text = massageText;
		this.m_BtnChoice[0].SetActive(true);
		this.m_BtnChoice[1].SetActive(false);
		this.m_BtnChoice[2].SetActive(true);
		this.m_BtnChoice[0].m_Index = 0;
		this.m_BtnChoice[2].m_Index = 1;
		this.m_BtnChoice[0].m_Type = PguiOpenWindowCtrl.BTN_TYPE.NEGATIVE;
		this.m_BtnChoice[2].m_Type = PguiOpenWindowCtrl.BTN_TYPE.POSITIVE;
		this.m_CallBack = callback;
		PguiTextCtrl component = this.m_MassageText.transform.parent.Find("ItemUse/Num").GetComponent<PguiTextCtrl>();
		PguiTextCtrl component2 = this.m_MassageText.transform.parent.Find("ItemOwn/Num").GetComponent<PguiTextCtrl>();
		Transform transform = this.m_BaseObject.transform.Find("Window/PurchaseConfirmButton");
		if (transform != null)
		{
			transform.gameObject.SetActive(isPurchaseConfirm && needNum > 0);
		}
		component.text = needNum.ToString();
		component2.text = haveNum.ToString();
		this.SetupCloseBtnAndroidBackKey();
	}

	public void SetupItemInfo(int itemId)
	{
		this.m_BtnChoiceObj[0].gameObject.SetActive(false);
		this.m_BtnChoiceObj[1].gameObject.SetActive(true);
		this.m_BtnChoiceObj[2].gameObject.SetActive(false);
		if (this.m_TitleText != null)
		{
			this.m_TitleText.text = "確認";
		}
		if (this.m_MassageText != null)
		{
			PguiTextCtrl.SetOverflowTypeOptions(ref this.m_MassageText, this.m_MassageText.GetComponent<PguiTextCtrl>().m_OverflowType, this.m_MassageText.fontSize);
		}
		bool flag = false;
		if (itemId - 30001 <= 1 || itemId == 30100)
		{
			flag = true;
		}
		ItemData userItemData = DataManager.DmItem.GetUserItemData(itemId);
		this.m_BaseObject.transform.Find("Window/ItemInfo_Stone").gameObject.SetActive(flag);
		this.m_BaseObject.transform.Find("Window/ItemInfo").gameObject.SetActive(!flag);
		this.m_BaseObject.transform.Find("Window/Txt_ItemName").gameObject.GetComponent<PguiTextCtrl>().text = userItemData.staticData.GetName();
		this.m_BaseObject.transform.Find("Window/Txt_Info").gameObject.GetComponent<PguiTextCtrl>().text = userItemData.staticData.GetInfo();
		IconItemCtrl component = this.m_BaseObject.transform.Find("Window/Icon_Item/Icon_Item").gameObject.GetComponent<IconItemCtrl>();
		component.transform.SetSiblingIndex(0);
		component.Setup(DataManager.DmItem.GetItemStaticBase(itemId));
		if (flag)
		{
			PguiTextCtrl component2 = this.m_BaseObject.transform.Find("Window/ItemInfo_Stone/Num_OwnAll").gameObject.GetComponent<PguiTextCtrl>();
			if (string.IsNullOrEmpty(this.itemInfoStoneAllDefaultText))
			{
				this.itemInfoStoneAllDefaultText = component2.text;
			}
			component2.text = this.itemInfoStoneAllDefaultText.Replace("Param01", DataManager.DmItem.GetUserItemData(30100).num.ToString());
			PguiTextCtrl component3 = this.m_BaseObject.transform.Find("Window/ItemInfo_Stone/Num_OwnInfo").gameObject.GetComponent<PguiTextCtrl>();
			if (string.IsNullOrEmpty(this.itemInfoStoneDefaultText))
			{
				this.itemInfoStoneDefaultText = component3.text;
			}
			component3.text = this.itemInfoStoneDefaultText.Replace("Param01", DataManager.DmItem.GetUserItemData(30002).num.ToString()).Replace("Param02", DataManager.DmItem.GetUserItemData(30001).num.ToString());
			return;
		}
		PguiTextCtrl component4 = this.m_BaseObject.transform.Find("Window/ItemInfo/Num_OwnAll").gameObject.GetComponent<PguiTextCtrl>();
		if (string.IsNullOrEmpty(this.itemInfoDefaultText))
		{
			this.itemInfoDefaultText = component4.text;
		}
		component4.text = this.itemInfoDefaultText.Replace("Param01", userItemData.num.ToString());
	}

	public void SetupItemInfo(int itemId, bool disableBestFit, string titleText, string massageText, List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>> buttonList, bool isDispClose, PguiOpenWindowCtrl.Callback callback, UnityAction finishedCloseCb = null)
	{
		this.SetupItemInfoInternal(itemId, disableBestFit, titleText, massageText, buttonList, isDispClose, callback, finishedCloseCb).Setup(DataManager.DmItem.GetItemStaticBase(itemId), new IconItemCtrl.SetupParam
		{
			noPhotoEffect = true
		});
	}

	public void SetupItemInfoNeedIconPhotoCtrl(IconPhotoCtrl ipc, bool disableBestFit, string titleText, string massageText, List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>> buttonList, bool isDispClose, PguiOpenWindowCtrl.Callback callback, UnityAction finishedCloseCb = null)
	{
		this.SetupItemInfoInternal(ipc.photoPackData.staticData.GetId(), disableBestFit, titleText, massageText, buttonList, isDispClose, callback, finishedCloseCb).Setup(DataManager.DmItem.GetItemStaticBase(ipc.photoPackData.staticData.GetId()), new IconItemCtrl.SetupParam
		{
			noPhotoEffect = true,
			isDispChangePhoto = ipc.isDispChangePhoto,
			forceSetup = true
		});
	}

	private IconItemCtrl SetupItemInfoInternal(int itemId, bool disableBestFit, string titleText, string massageText, List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>> buttonList, bool isDispClose, PguiOpenWindowCtrl.Callback callback, UnityAction finishedCloseCb)
	{
		this.Setup(titleText, massageText, buttonList, isDispClose, callback, finishedCloseCb, false);
		if (this.m_MassageText != null)
		{
			PguiTextCtrl.SetOverflowTypeOptions(ref this.m_MassageText, this.m_MassageText.GetComponent<PguiTextCtrl>().m_OverflowType, this.m_MassageText.fontSize);
			if (disableBestFit)
			{
				this.m_MassageText.resizeTextForBestFit = false;
			}
		}
		ItemData userItemData = DataManager.DmItem.GetUserItemData(itemId);
		this.m_BaseObject.transform.Find("Window/ItemInfo_Stone").gameObject.SetActive(false);
		this.m_BaseObject.transform.Find("Window/ItemInfo").gameObject.SetActive(false);
		this.m_BaseObject.transform.Find("Window/Txt_ItemName").gameObject.GetComponent<PguiTextCtrl>().text = userItemData.staticData.GetName();
		IconItemCtrl component = this.m_BaseObject.transform.Find("Window/Icon_Item/Icon_Item").gameObject.GetComponent<IconItemCtrl>();
		component.transform.SetSiblingIndex(0);
		return component;
	}

	public void SetupByStaminaSelect(PguiOpenWindowCtrl.Callback callback)
	{
		if (this.m_windowType != PguiOpenWindowCtrl.WINDOW_TYPE.STAMINA_ITEM_SELECT)
		{
			Verbose<PrjLog>.LogError("Error : PguiOpenWindowCtrl.SetupByStamina : タイプの違うウィンドウ", null);
			return;
		}
		this.StaminaSetupCommon(callback);
	}

	public void SetupByQuestSkip(PguiOpenWindowCtrl.Callback callback)
	{
		for (int i = 0; i < this.m_BtnChoice.Count; i++)
		{
			switch (i)
			{
			case 0:
				this.m_BtnChoice[i].m_Index = 1;
				break;
			case 2:
				this.m_BtnChoice[i].m_Index = 2;
				break;
			case 3:
				this.m_BtnChoice[i].m_Index = 3;
				break;
			}
		}
		this.m_BtnChoice[0].m_Type = PguiOpenWindowCtrl.BTN_TYPE.NEGATIVE;
		this.m_BtnChoice[2].m_Type = PguiOpenWindowCtrl.BTN_TYPE.POSITIVE;
		this.m_CallBack = callback;
		this.SetupCloseBtnAndroidBackKey();
	}

	public void SetupByStaminaSetting(PguiOpenWindowCtrl.Callback callback)
	{
		if (this.m_windowType != PguiOpenWindowCtrl.WINDOW_TYPE.STAMINA_ITEM_SETTING)
		{
			Verbose<PrjLog>.LogError("Error : PguiOpenWindowCtrl.SetupByStamina : タイプの違うウィンドウ", null);
			return;
		}
		this.StaminaSetupCommon(callback);
	}

	public void SetupByStaminaUse(PguiOpenWindowCtrl.Callback callback)
	{
		if (this.m_windowType != PguiOpenWindowCtrl.WINDOW_TYPE.STAMINA_ITEM_USE)
		{
			Verbose<PrjLog>.LogError("Error : PguiOpenWindowCtrl.SetupByStaminaUse : タイプの違うウィンドウ", null);
			return;
		}
		this.StaminaSetupCommon(callback);
	}

	public void SetupCheckBox(string titleText, string massageText, List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>> buttonList, bool isDispClose, PguiOpenWindowCtrl.Callback callback, Action<bool> action, string checkBoxInfo)
	{
		if (this.m_TitleText != null && titleText != null)
		{
			this.m_TitleText.text = titleText;
		}
		if (this.m_MassageText != null && massageText != null)
		{
			this.m_MassageText.text = massageText;
		}
		if (buttonList != null)
		{
			if (buttonList.Count != 1)
			{
				this.m_BtnChoice[0].SetActive(true);
				this.m_BtnChoice[1].SetActive(false);
				this.m_BtnChoice[2].SetActive(true);
				this.SettingChoiceButton(this.m_BtnChoice[0], buttonList[0], 0);
				this.SettingChoiceButton(this.m_BtnChoice[2], buttonList[1], 1);
			}
			else
			{
				this.m_BtnChoice[0].SetActive(false);
				this.m_BtnChoice[1].SetActive(true);
				this.m_BtnChoice[2].SetActive(false);
				this.SettingChoiceButton(this.m_BtnChoice[1], buttonList[0], 0);
			}
		}
		if (this.m_BtnCloseObj != null)
		{
			this.m_BtnCloseObj.gameObject.SetActive(isDispClose);
		}
		if (this.m_BtnCheckBoxObj != null)
		{
			this.m_BtnCheckBoxObj.SetActive(action != null);
		}
		if (this.m_BtnCheckBoxObj != null && this.m_BtnCheckBoxObj.activeSelf)
		{
			this.m_BtnCheckBoxObj.GetComponent<Button>().onClick.RemoveAllListeners();
			this.m_BtnCheckBoxObj.GetComponent<Button>().onClick.AddListener(delegate
			{
				this.m_ImgCheck.SetActive(!this.m_ImgCheck.activeSelf);
				Action<bool> action2 = action;
				if (action2 == null)
				{
					return;
				}
				action2(this.m_ImgCheck.activeSelf);
			});
			this.m_BtnCheckBoxObj.transform.Find("Txt_CheckInfo").GetComponent<PguiTextCtrl>().text = checkBoxInfo;
		}
		this.SetupCloseBtnAndroidBackKey();
		this.m_CallBack = callback;
	}

	private void StaminaSetupCommon(PguiOpenWindowCtrl.Callback callback)
	{
		this.m_BtnChoice[0].SetActive(true);
		this.m_BtnChoice[1].SetActive(false);
		this.m_BtnChoice[2].SetActive(true);
		for (int i = 0; i < this.m_BtnChoice.Count; i++)
		{
			switch (i)
			{
			case 0:
				this.m_BtnChoice[i].m_Index = 1;
				break;
			case 2:
				this.m_BtnChoice[i].m_Index = 2;
				break;
			case 3:
				this.m_BtnChoice[i].m_Index = 3;
				break;
			}
		}
		this.m_BtnChoice[0].m_Type = PguiOpenWindowCtrl.BTN_TYPE.NEGATIVE;
		this.m_BtnChoice[2].m_Type = PguiOpenWindowCtrl.BTN_TYPE.POSITIVE;
		this.m_CallBack = callback;
		this.SetupCloseBtnAndroidBackKey();
	}

	public void SetupByPurchaseStone(PguiOpenWindowCtrl.Callback callback)
	{
		if (this.m_windowType != PguiOpenWindowCtrl.WINDOW_TYPE.PURCHASE_STONE)
		{
			Verbose<PrjLog>.LogError("Error : PguiOpenWindowCtrl.SetupByPurchaseStone : タイプの違うウィンドウ", null);
			return;
		}
		this.m_CallBack = callback;
		this.SetupCloseBtnAndroidBackKey();
	}

	public void SetupByMonthlyPack(PguiOpenWindowCtrl.Callback callback)
	{
		if (this.m_windowType != PguiOpenWindowCtrl.WINDOW_TYPE.MONTHLYPACK)
		{
			Verbose<PrjLog>.LogError("Error : PguiOpenWindowCtrl.SetupByMonthlyPack : タイプの違うウィンドウ", null);
			return;
		}
		this.m_CallBack = callback;
		this.SetupCloseBtnAndroidBackKey();
	}

	public void SetupTerms(PguiOpenWindowCtrl.Callback callback)
	{
		if (this.m_windowType != PguiOpenWindowCtrl.WINDOW_TYPE.BASIC)
		{
			Verbose<PrjLog>.LogError("Error : PguiOpenWindowCtrl.SetupTerms : タイプの違うウィンドウ", null);
			return;
		}
		this.m_TitleText.text = "利用規約・プライバシーポリシー同意確認";
		this.m_MassageText.text = "けものフレンズ３をプレイするには\n利用規約およびプライバシーポリシー\nに同意していただく必要があります。\n\n内容を確認の上「同意する」をタップしてください。\n<size=25>※プライバシーポリシーは利用規約内のリンクよりご確認いただけます。</size>\n";
		this.m_BtnChoice[0].SetActive(true);
		this.m_BtnChoice[1].SetActive(true);
		this.m_BtnChoice[2].SetActive(true);
		this.SettingChoiceButton(this.m_BtnChoice[0], new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.NEGATIVE, "同意しない"), 0);
		this.SettingChoiceButton(this.m_BtnChoice[2], new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, "同意する"), 1);
		this.SettingChoiceButton(this.m_BtnChoice[1], new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.SELECT, "利用規約/プライバシーポリシー"), 2);
		this.m_BtnChoice[1].m_Button.m_BaseImage.color = Color.clear;
		Vector3 vector = this.m_BtnChoice[1].m_Button.transform.localPosition;
		vector.y += 70f;
		this.m_BtnChoice[1].m_Button.transform.localPosition = vector;
		this.m_BtnChoice[1].m_Text.color = new Color32(231, 179, 31, byte.MaxValue);
		int num = 0;
		while (this.m_BtnChoice[1].m_OutlineList != null && num < this.m_BtnChoice[1].m_OutlineList.Length)
		{
			this.m_BtnChoice[1].m_OutlineList[num].effectColor = new Color32(107, 81, 10, byte.MaxValue);
			num++;
		}
		if (this.m_BtnChoice != null && this.m_BtnChoice[0].m_Button != null && this.m_BtnChoice[2].m_Button != null && this.m_BtnChoice[0].m_Button.transform.localPosition.x == -240f && this.m_BtnChoice[2].m_Button.transform.localPosition.x == 240f)
		{
			vector = this.m_BtnChoice[0].m_Button.transform.localPosition;
			vector.x = -135f;
			this.m_BtnChoice[0].m_Button.transform.localPosition = vector;
			vector = this.m_BtnChoice[2].m_Button.transform.localPosition;
			vector.x = 135f;
			this.m_BtnChoice[2].m_Button.transform.localPosition = vector;
		}
		this.m_BtnCloseObj.gameObject.SetActive(true);
		this.SetupCloseBtnAndroidBackKey();
		this.m_CallBack = callback;
		this.m_FinishedCloseCallBack = null;
	}

	public void CloseTerms()
	{
		if (this.m_windowType != PguiOpenWindowCtrl.WINDOW_TYPE.BASIC)
		{
			Verbose<PrjLog>.LogError("Error : PguiOpenWindowCtrl.CloseTerms : タイプの違うウィンドウ", null);
			return;
		}
		this.m_BtnChoice[1].m_Button.m_BaseImage.color = Color.white;
		Vector3 localPosition = this.m_BtnChoice[1].m_Button.transform.localPosition;
		localPosition.y = this.m_BtnChoice[0].m_Button.transform.localPosition.y;
		this.m_BtnChoice[1].m_Button.transform.localPosition = localPosition;
		this.m_BtnChoice[1].m_Text.color = Color.white;
		int num = 0;
		while (this.m_BtnChoice[1].m_OutlineList != null && num < this.m_BtnChoice[1].m_OutlineList.Length)
		{
			this.m_BtnChoice[1].m_OutlineList[num].effectColor = new Color32(33, 82, 2, byte.MaxValue);
			num++;
		}
	}

	public void SetupTitleGraphic()
	{
		this.m_BtnChoice[0].m_Type = PguiOpenWindowCtrl.BTN_TYPE.POSITIVE;
		if (this.m_BtnCloseObj != null)
		{
			this.m_BtnCloseObj.gameObject.SetActive(true);
		}
		this.SetupCloseBtnAndroidBackKey();
		this.m_CallBack = null;
		this.m_FinishedCloseCallBack = null;
	}

	public void SetupKemoBoardResetCheck(PguiOpenWindowCtrl.Callback callback)
	{
		if (this.m_windowType != PguiOpenWindowCtrl.WINDOW_TYPE.USE_ITEM)
		{
			Verbose<PrjLog>.LogError("Error : PguiOpenWindowCtrl.SetupTerms : タイプの違うウィンドウ", null);
			return;
		}
		this.m_BtnChoice[0].SetActive(true);
		this.m_BtnChoice[1].SetActive(true);
		this.m_BtnChoice[2].SetActive(true);
		this.m_BtnChoice[0].m_Index = 0;
		this.m_BtnChoice[1].m_Index = 1;
		this.m_BtnChoice[2].m_Index = 2;
		this.m_BtnChoice[0].m_Type = PguiOpenWindowCtrl.BTN_TYPE.NEGATIVE;
		this.m_BtnChoice[1].m_Type = PguiOpenWindowCtrl.BTN_TYPE.POSITIVE;
		this.m_BtnChoice[2].m_Type = PguiOpenWindowCtrl.BTN_TYPE.POSITIVE;
		this.m_BtnCloseObj.gameObject.SetActive(true);
		this.SetupCloseBtnAndroidBackKey();
		this.m_CallBack = callback;
		this.m_FinishedCloseCallBack = null;
	}

	public void SetupButtonOnly(PguiOpenWindowCtrl.Callback callback)
	{
		if (this.m_windowType != PguiOpenWindowCtrl.WINDOW_TYPE.BASIC)
		{
			Verbose<PrjLog>.LogError("Error : PguiOpenWindowCtrl.SetupButtonOnly : タイプの違うウィンドウ", null);
			return;
		}
		this.m_BtnChoice[0].SetActive(true);
		this.m_BtnChoice[1].SetActive(true);
		this.m_BtnChoice[2].SetActive(true);
		this.m_BtnChoice[0].m_Index = 0;
		this.m_BtnChoice[1].m_Index = 1;
		this.m_BtnChoice[2].m_Index = 2;
		this.m_BtnChoice[0].m_Type = PguiOpenWindowCtrl.BTN_TYPE.NEGATIVE;
		this.m_BtnChoice[1].m_Type = PguiOpenWindowCtrl.BTN_TYPE.POSITIVE;
		this.m_BtnChoice[2].m_Type = PguiOpenWindowCtrl.BTN_TYPE.POSITIVE;
		if (this.m_BtnCloseObj != null)
		{
			this.m_BtnCloseObj.gameObject.SetActive(true);
		}
		this.SetupCloseBtnAndroidBackKey();
		this.m_CallBack = callback;
		this.m_FinishedCloseCallBack = null;
	}

	public void Open()
	{
		this.m_ReqSequence = PguiOpenWindowCtrl.Sequence.OPEN_START;
	}

	public bool FinishedOpen()
	{
		return this.m_Sequence == PguiOpenWindowCtrl.Sequence.ACTIVE;
	}

	public bool FinishedClose()
	{
		return this.m_Sequence == PguiOpenWindowCtrl.Sequence.INACTIVE;
	}

	public void ForceClose()
	{
		if (this.m_Sequence == PguiOpenWindowCtrl.Sequence.ACTIVE)
		{
			this.m_ReqSequence = PguiOpenWindowCtrl.Sequence.CLOSE_START;
		}
	}

	public bool StartOpenAnim()
	{
		return this.m_Sequence == PguiOpenWindowCtrl.Sequence.OPEN_WAIT;
	}

	public void ForceOpen()
	{
		this.m_Sequence = PguiOpenWindowCtrl.Sequence.OPEN_START;
	}

	public RectTransform GetButtonRectTransform(int btnIndex)
	{
		return this.m_BtnChoice[btnIndex].m_Button.transform as RectTransform;
	}

	public RectTransform GetCloseButtonRectTransform()
	{
		return this.m_BtnCloseObj.transform as RectTransform;
	}

	public void AddCloseListener()
	{
		this.m_BtnCloseObj.GetComponent<Button>().onClick.AddListener(delegate
		{
			this.OnClickButton(this.m_BtnCloseObj);
		});
	}

	public static List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>> GetButtonPreset(PguiOpenWindowCtrl.PresetType type)
	{
		if (PguiOpenWindowCtrl.PresetButtonList[(int)type] != null)
		{
			return PguiOpenWindowCtrl.PresetButtonList[(int)type];
		}
		switch (type)
		{
		case PguiOpenWindowCtrl.PresetType.CLOSE:
			PguiOpenWindowCtrl.PresetButtonList[(int)type] = new List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>>();
			PguiOpenWindowCtrl.PresetButtonList[(int)type].Add(new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, "閉じる"));
			break;
		case PguiOpenWindowCtrl.PresetType.NO_YES:
			PguiOpenWindowCtrl.PresetButtonList[(int)type] = new List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>>();
			PguiOpenWindowCtrl.PresetButtonList[(int)type].Add(new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.NEGATIVE, "いいえ"));
			PguiOpenWindowCtrl.PresetButtonList[(int)type].Add(new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, "はい"));
			break;
		case PguiOpenWindowCtrl.PresetType.CANCEL_OK:
			PguiOpenWindowCtrl.PresetButtonList[(int)type] = new List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>>();
			PguiOpenWindowCtrl.PresetButtonList[(int)type].Add(new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.NEGATIVE, "キャンセル"));
			PguiOpenWindowCtrl.PresetButtonList[(int)type].Add(new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, "決定"));
			break;
		case PguiOpenWindowCtrl.PresetType.LR_CURSOR:
			PguiOpenWindowCtrl.PresetButtonList[(int)type] = new List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>>();
			break;
		case PguiOpenWindowCtrl.PresetType.OK:
			PguiOpenWindowCtrl.PresetButtonList[(int)type] = new List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>>();
			PguiOpenWindowCtrl.PresetButtonList[(int)type].Add(new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, "決定"));
			break;
		case PguiOpenWindowCtrl.PresetType.STORE:
			PguiOpenWindowCtrl.PresetButtonList[(int)type] = new List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>>();
			PguiOpenWindowCtrl.PresetButtonList[(int)type].Add(new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, "ストアへ"));
			break;
		case PguiOpenWindowCtrl.PresetType.CANCEL_MOVE:
			PguiOpenWindowCtrl.PresetButtonList[(int)type] = new List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>>();
			PguiOpenWindowCtrl.PresetButtonList[(int)type].Add(new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.NEGATIVE, "キャンセル"));
			PguiOpenWindowCtrl.PresetButtonList[(int)type].Add(new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, "移動"));
			break;
		case PguiOpenWindowCtrl.PresetType.CANCEL_SET:
			PguiOpenWindowCtrl.PresetButtonList[(int)type] = new List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>>();
			PguiOpenWindowCtrl.PresetButtonList[(int)type].Add(new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.NEGATIVE, "キャンセル"));
			PguiOpenWindowCtrl.PresetButtonList[(int)type].Add(new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, "設定する"));
			break;
		case PguiOpenWindowCtrl.PresetType.CLOSE_SHOP:
			PguiOpenWindowCtrl.PresetButtonList[(int)type] = new List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>>();
			PguiOpenWindowCtrl.PresetButtonList[(int)type].Add(new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.NEGATIVE, "閉じる"));
			PguiOpenWindowCtrl.PresetButtonList[(int)type].Add(new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, "ショップへ"));
			break;
		case PguiOpenWindowCtrl.PresetType.NO_YES_EMPTY:
			PguiOpenWindowCtrl.PresetButtonList[(int)type] = new List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>>();
			PguiOpenWindowCtrl.PresetButtonList[(int)type].Add(new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.NEGATIVE, ""));
			PguiOpenWindowCtrl.PresetButtonList[(int)type].Add(new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, ""));
			break;
		case PguiOpenWindowCtrl.PresetType.REVIEW:
			PguiOpenWindowCtrl.PresetButtonList[(int)type] = new List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>>();
			PguiOpenWindowCtrl.PresetButtonList[(int)type].Add(new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.NEGATIVE, "レビューしない"));
			PguiOpenWindowCtrl.PresetButtonList[(int)type].Add(new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, "レビューする"));
			break;
		case PguiOpenWindowCtrl.PresetType.TITLE_MENT:
			PguiOpenWindowCtrl.PresetButtonList[(int)type] = new List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>>();
			PguiOpenWindowCtrl.PresetButtonList[(int)type].Add(new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.NEGATIVE, "再起動"));
			PguiOpenWindowCtrl.PresetButtonList[(int)type].Add(new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, "公式ページへ"));
			break;
		case PguiOpenWindowCtrl.PresetType.CLOSE_GREEN:
			PguiOpenWindowCtrl.PresetButtonList[(int)type] = new List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>>();
			PguiOpenWindowCtrl.PresetButtonList[(int)type].Add(new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.NEGATIVE, "閉じる"));
			break;
		case PguiOpenWindowCtrl.PresetType.NEXT:
			PguiOpenWindowCtrl.PresetButtonList[(int)type] = new List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>>();
			PguiOpenWindowCtrl.PresetButtonList[(int)type].Add(new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, "次へ"));
			break;
		}
		return PguiOpenWindowCtrl.PresetButtonList[(int)type];
	}

	private void SetupCloseBtnAndroidBackKey()
	{
		if (this.windowType == PguiOpenWindowCtrl.WINDOW_TYPE.SERVER_ERROR)
		{
			bool flag = false;
			if (this.m_BtnCloseObj != null && this.m_BtnCloseObj.activeSelf)
			{
				PguiButtonCtrl component = this.m_BtnCloseObj.GetComponent<PguiButtonCtrl>();
				if (component != null)
				{
					component.androidBackKeyTarget = true;
					flag = true;
				}
			}
			int num = 0;
			while (this.m_BtnChoice != null)
			{
				if (num >= this.m_BtnChoice.Count)
				{
					return;
				}
				if (!flag && this.m_BtnChoice[num] != null && this.m_BtnChoice[num].m_Button != null && this.m_BtnChoice[num].m_Button.gameObject.activeSelf)
				{
					PguiButtonCtrl component2 = this.m_BtnChoice[num].m_Button.GetComponent<PguiButtonCtrl>();
					if (component2 != null)
					{
						component2.androidBackKeyTarget = true;
						flag = true;
					}
				}
				num++;
			}
		}
		else if (this.m_BtnCloseObj != null)
		{
			PguiButtonCtrl component3 = this.m_BtnCloseObj.GetComponent<PguiButtonCtrl>();
			if (component3 != null)
			{
				component3.androidBackKeyTarget = true;
			}
		}
	}

	private void Awake()
	{
		this.m_BtnChoice = new List<PguiOpenWindowCtrl.ChoiceButton>();
		for (int i = 0; i < this.m_BtnChoiceObj.Length; i++)
		{
			this.m_BtnChoice.Add(new PguiOpenWindowCtrl.ChoiceButton(this.m_BtnChoiceObj[i]));
		}
		this.m_BaseObject.SetActive(false);
		this.m_WindowRectTransform.localScale = Vector3.zero;
	}

	private void Update()
	{
		this.UpdateInternal();
	}

	private void Start()
	{
		this.UpdateInternal();
	}

	private void UpdateInternal()
	{
		switch (this.m_Sequence)
		{
		case PguiOpenWindowCtrl.Sequence.INACTIVE:
			if (this.m_ReqSequence == PguiOpenWindowCtrl.Sequence.OPEN_START)
			{
				this.m_Sequence = PguiOpenWindowCtrl.Sequence.OPEN_START;
			}
			break;
		case PguiOpenWindowCtrl.Sequence.OPEN_START:
			this.m_BaseObject.SetActive(true);
			this.m_animation.ExPlayAnimation("Open", null);
			SoundManager.Play("prd_se_dialog_disp", false, false);
			this.m_Sequence = PguiOpenWindowCtrl.Sequence.OPEN_WAIT;
			break;
		case PguiOpenWindowCtrl.Sequence.OPEN_WAIT:
			if (this.m_ScrollViewContent != null)
			{
				this.m_ScrollViewContent.anchoredPosition = Vector2.zero;
			}
			if (this.m_ScrollViewContentList != null)
			{
				foreach (RectTransform rectTransform in this.m_ScrollViewContentList)
				{
					rectTransform.anchoredPosition = Vector2.zero;
				}
			}
			if (!this.m_animation.ExIsPlaying())
			{
				this.m_Sequence = PguiOpenWindowCtrl.Sequence.ACTIVE;
			}
			break;
		case PguiOpenWindowCtrl.Sequence.ACTIVE:
			if (this.m_ReqSequence == PguiOpenWindowCtrl.Sequence.CALLBACK_ACTION)
			{
				if (this.m_BtnCloseObj == this.m_LastTouchButton)
				{
					this.m_SelectButtonIndex = -1;
					SoundManager.Play("prd_se_cancel", false, false);
				}
				else
				{
					PguiOpenWindowCtrl.ChoiceButton choiceButton = this.m_BtnChoice.Find((PguiOpenWindowCtrl.ChoiceButton item) => item.m_Button != null && item.m_Button.gameObject == this.m_LastTouchButton);
					if (choiceButton != null)
					{
						if (choiceButton.m_Type == PguiOpenWindowCtrl.BTN_TYPE.POSITIVE)
						{
							SoundManager.Play("prd_se_decide", false, false);
						}
						else if (choiceButton.m_Type == PguiOpenWindowCtrl.BTN_TYPE.NEGATIVE)
						{
							SoundManager.Play("prd_se_cancel", false, false);
						}
						else if (choiceButton.m_Type == PguiOpenWindowCtrl.BTN_TYPE.SELECT)
						{
							SoundManager.Play("prd_se_click", false, false);
						}
						this.m_SelectButtonIndex = choiceButton.m_Index;
					}
				}
				this.m_Sequence = PguiOpenWindowCtrl.Sequence.CALLBACK_ACTION;
			}
			else if (this.m_ReqSequence == PguiOpenWindowCtrl.Sequence.CLOSE_START)
			{
				this.m_Sequence = PguiOpenWindowCtrl.Sequence.CLOSE_START;
			}
			break;
		case PguiOpenWindowCtrl.Sequence.CALLBACK_ACTION:
			if (this.m_CallBack == null || this.m_CallBack(this.m_SelectButtonIndex))
			{
				this.m_Sequence = PguiOpenWindowCtrl.Sequence.CLOSE_START;
			}
			else
			{
				this.m_Sequence = PguiOpenWindowCtrl.Sequence.ACTIVE;
			}
			break;
		case PguiOpenWindowCtrl.Sequence.CLOSE_START:
			this.m_animation.ExPlayAnimation("Close", null);
			SoundManager.Play("prd_se_dialog_close", false, false);
			this.m_Sequence = PguiOpenWindowCtrl.Sequence.CLOSE_WAIT;
			break;
		case PguiOpenWindowCtrl.Sequence.CLOSE_WAIT:
			if (!this.m_animation.ExIsPlaying())
			{
				UnityAction finishedCloseCallBack = this.m_FinishedCloseCallBack;
				if (finishedCloseCallBack != null)
				{
					finishedCloseCallBack();
				}
				this.m_Sequence = PguiOpenWindowCtrl.Sequence.INACTIVE;
				this.m_BaseObject.SetActive(false);
			}
			break;
		}
		this.m_ReqSequence = PguiOpenWindowCtrl.Sequence.NONE;
	}

	private void SettingChoiceButton(PguiOpenWindowCtrl.ChoiceButton button, KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string> data, int index)
	{
		if (button.m_Button == null)
		{
			return;
		}
		button.m_Type = data.Key;
		button.m_Index = index;
		if (button.m_Text != null)
		{
			if (!data.Value.Equals(""))
			{
				button.m_Text.text = data.Value;
			}
			PguiOpenWindowCtrl.BTN_TYPE key = data.Key;
			if (key != PguiOpenWindowCtrl.BTN_TYPE.POSITIVE)
			{
				if (key != PguiOpenWindowCtrl.BTN_TYPE.NEGATIVE)
				{
					return;
				}
				button.m_Button.m_BaseImage.GetComponent<PguiImageCtrl>().SetImageByName("cmn_btn_all");
				int num = 0;
				while (button.m_OutlineList != null && num < button.m_OutlineList.Length)
				{
					button.m_OutlineList[num].effectColor = new Color32(33, 82, 2, byte.MaxValue);
					num++;
				}
			}
			else
			{
				button.m_Button.m_BaseImage.GetComponent<PguiImageCtrl>().SetImageByName("cmn_btn_strong");
				int num2 = 0;
				while (button.m_OutlineList != null)
				{
					if (num2 >= button.m_OutlineList.Length)
					{
						return;
					}
					button.m_OutlineList[num2].effectColor = new Color32(52, 23, 2, byte.MaxValue);
					num2++;
				}
			}
		}
	}

	public void RegistCallback(PguiOpenWindowCtrl.Callback callback)
	{
		this.m_CallBack = callback;
	}

	public void OnClickButton(GameObject button)
	{
		this.m_ReqSequence = PguiOpenWindowCtrl.Sequence.CALLBACK_ACTION;
		this.m_LastTouchButton = button;
	}

	[SerializeField]
	private GameObject m_BaseObject;

	[SerializeField]
	private RectTransform m_WindowRectTransform;

	[SerializeField]
	private SimpleAnimation m_animation;

	[SerializeField]
	private PguiButtonCtrl[] m_BtnChoiceObj = new PguiButtonCtrl[3];

	[SerializeField]
	private GameObject m_BtnCloseObj;

	[SerializeField]
	private GameObject m_BtnCheckBoxObj;

	[SerializeField]
	private GameObject m_ImgCheck;

	[SerializeField]
	private Text m_TitleText;

	[SerializeField]
	private Text m_MassageText;

	[SerializeField]
	private RectTransform m_ScrollViewContent;

	[SerializeField]
	private List<RectTransform> m_ScrollViewContentList;

	[SerializeField]
	public RectTransform m_UserInfoContent;

	[SerializeField]
	private PguiOpenWindowCtrl.WINDOW_TYPE m_windowType;

	private PguiOpenWindowCtrl.Callback m_CallBack;

	private UnityAction m_FinishedCloseCallBack;

	public static readonly int CLOSE_BUTTON_INDEX = -1;

	private List<PguiOpenWindowCtrl.ChoiceButton> m_BtnChoice;

	private string[] noStoneDefaultMassage;

	private string itemInfoStoneAllDefaultText;

	private string itemInfoStoneDefaultText;

	private string itemInfoDefaultText;

	private static List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>>[] PresetButtonList = new List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>>[14];

	private PguiOpenWindowCtrl.Sequence m_ReqSequence;

	private PguiOpenWindowCtrl.Sequence m_Sequence = PguiOpenWindowCtrl.Sequence.INACTIVE;

	private GameObject m_LastTouchButton;

	private int m_SelectButtonIndex;

	public enum WINDOW_TYPE
	{
		INVALID,
		BASIC,
		NO_STONE,
		USE_ITEM,
		FOLLOW,
		PURCHASE_STONE,
		SERVER_ERROR,
		GET_ITEM,
		STAMINA_ITEM_SELECT,
		STAMINA_ITEM_SETTING,
		STAMINA_ITEM_USE,
		MONTHLYPACK,
		ITEM_INFO,
		GET_ITEM_MULTIPLE,
		CHECK_BOX,
		GET_ACHIEVEMENT
	}

	public enum BTN_TYPE
	{
		NONE,
		POSITIVE,
		NEGATIVE,
		SELECT
	}

	public delegate bool Callback(int index);

	public enum PresetType
	{
		CLOSE,
		NO_YES,
		CANCEL_OK,
		LR_CURSOR,
		OK,
		STORE,
		CANCEL_MOVE,
		CANCEL_SET,
		CLOSE_SHOP,
		NO_YES_EMPTY,
		REVIEW,
		TITLE_MENT,
		CLOSE_GREEN,
		NEXT,
		MAX
	}

	private enum Sequence
	{
		NONE,
		INACTIVE,
		OPEN_START,
		OPEN_WAIT,
		ACTIVE,
		CALLBACK_ACTION,
		CLOSE_START,
		CLOSE_WAIT
	}

	private class ChoiceButton
	{
		public ChoiceButton(PguiButtonCtrl button)
		{
			this.m_Button = button;
			if (this.m_Button != null && null != button.transform.Find("BaseImage/Text"))
			{
				this.m_Text = button.transform.Find("BaseImage/Text").GetComponent<Text>();
				this.m_OutlineList = button.transform.Find("BaseImage/Text").GetComponents<PguiOutline>();
			}
		}

		public void SetActive(bool act)
		{
			if (this.m_Button != null)
			{
				this.m_Button.gameObject.SetActive(act);
			}
		}

		public PguiButtonCtrl m_Button;

		public Text m_Text;

		public PguiOutline[] m_OutlineList;

		public int m_Index;

		public PguiOpenWindowCtrl.BTN_TYPE m_Type;
	}
}
