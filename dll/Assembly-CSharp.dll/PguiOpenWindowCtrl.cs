using System;
using System.Collections.Generic;
using SGNFW.Common;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020001D6 RID: 470
public class PguiOpenWindowCtrl : PguiBehaviour
{
	// Token: 0x1700043C RID: 1084
	// (get) Token: 0x06001FC5 RID: 8133 RVA: 0x00187784 File Offset: 0x00185984
	public PguiOpenWindowCtrl.WINDOW_TYPE windowType
	{
		get
		{
			return this.m_windowType;
		}
	}

	// Token: 0x1700043D RID: 1085
	// (get) Token: 0x06001FC6 RID: 8134 RVA: 0x0018778C File Offset: 0x0018598C
	public GameObject choiceR
	{
		get
		{
			return this.m_BtnChoiceObj[2].gameObject;
		}
	}

	// Token: 0x1700043E RID: 1086
	// (get) Token: 0x06001FC7 RID: 8135 RVA: 0x0018779B File Offset: 0x0018599B
	public RectTransform WindowRectTransform
	{
		get
		{
			return this.m_WindowRectTransform;
		}
	}

	// Token: 0x1700043F RID: 1087
	// (get) Token: 0x06001FC8 RID: 8136 RVA: 0x001877A3 File Offset: 0x001859A3
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

	// Token: 0x06001FC9 RID: 8137 RVA: 0x001877C0 File Offset: 0x001859C0
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

	// Token: 0x06001FCA RID: 8138 RVA: 0x00187B9C File Offset: 0x00185D9C
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

	// Token: 0x06001FCB RID: 8139 RVA: 0x00187F8C File Offset: 0x0018618C
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

	// Token: 0x06001FCC RID: 8140 RVA: 0x001880E0 File Offset: 0x001862E0
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

	// Token: 0x06001FCD RID: 8141 RVA: 0x001883E0 File Offset: 0x001865E0
	public void SetupItemInfo(int itemId, bool disableBestFit, string titleText, string massageText, List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>> buttonList, bool isDispClose, PguiOpenWindowCtrl.Callback callback, UnityAction finishedCloseCb = null)
	{
		this.SetupItemInfoInternal(itemId, disableBestFit, titleText, massageText, buttonList, isDispClose, callback, finishedCloseCb).Setup(DataManager.DmItem.GetItemStaticBase(itemId), new IconItemCtrl.SetupParam
		{
			noPhotoEffect = true
		});
	}

	// Token: 0x06001FCE RID: 8142 RVA: 0x0018841C File Offset: 0x0018661C
	public void SetupItemInfoNeedIconPhotoCtrl(IconPhotoCtrl ipc, bool disableBestFit, string titleText, string massageText, List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>> buttonList, bool isDispClose, PguiOpenWindowCtrl.Callback callback, UnityAction finishedCloseCb = null)
	{
		this.SetupItemInfoInternal(ipc.photoPackData.staticData.GetId(), disableBestFit, titleText, massageText, buttonList, isDispClose, callback, finishedCloseCb).Setup(DataManager.DmItem.GetItemStaticBase(ipc.photoPackData.staticData.GetId()), new IconItemCtrl.SetupParam
		{
			noPhotoEffect = true,
			isDispChangePhoto = ipc.isDispChangePhoto,
			forceSetup = true
		});
	}

	// Token: 0x06001FCF RID: 8143 RVA: 0x0018848C File Offset: 0x0018668C
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

	// Token: 0x06001FD0 RID: 8144 RVA: 0x00188594 File Offset: 0x00186794
	public void SetupByStaminaSelect(PguiOpenWindowCtrl.Callback callback)
	{
		if (this.m_windowType != PguiOpenWindowCtrl.WINDOW_TYPE.STAMINA_ITEM_SELECT)
		{
			Verbose<PrjLog>.LogError("Error : PguiOpenWindowCtrl.SetupByStamina : タイプの違うウィンドウ", null);
			return;
		}
		this.StaminaSetupCommon(callback);
	}

	// Token: 0x06001FD1 RID: 8145 RVA: 0x001885B4 File Offset: 0x001867B4
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

	// Token: 0x06001FD2 RID: 8146 RVA: 0x0018865A File Offset: 0x0018685A
	public void SetupByStaminaSetting(PguiOpenWindowCtrl.Callback callback)
	{
		if (this.m_windowType != PguiOpenWindowCtrl.WINDOW_TYPE.STAMINA_ITEM_SETTING)
		{
			Verbose<PrjLog>.LogError("Error : PguiOpenWindowCtrl.SetupByStamina : タイプの違うウィンドウ", null);
			return;
		}
		this.StaminaSetupCommon(callback);
	}

	// Token: 0x06001FD3 RID: 8147 RVA: 0x00188679 File Offset: 0x00186879
	public void SetupByStaminaUse(PguiOpenWindowCtrl.Callback callback)
	{
		if (this.m_windowType != PguiOpenWindowCtrl.WINDOW_TYPE.STAMINA_ITEM_USE)
		{
			Verbose<PrjLog>.LogError("Error : PguiOpenWindowCtrl.SetupByStaminaUse : タイプの違うウィンドウ", null);
			return;
		}
		this.StaminaSetupCommon(callback);
	}

	// Token: 0x06001FD4 RID: 8148 RVA: 0x00188698 File Offset: 0x00186898
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

	// Token: 0x06001FD5 RID: 8149 RVA: 0x00188884 File Offset: 0x00186A84
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

	// Token: 0x06001FD6 RID: 8150 RVA: 0x00188960 File Offset: 0x00186B60
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

	// Token: 0x06001FD7 RID: 8151 RVA: 0x00188984 File Offset: 0x00186B84
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

	// Token: 0x06001FD8 RID: 8152 RVA: 0x001889AC File Offset: 0x00186BAC
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

	// Token: 0x06001FD9 RID: 8153 RVA: 0x00188CBC File Offset: 0x00186EBC
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

	// Token: 0x06001FDA RID: 8154 RVA: 0x00188DD8 File Offset: 0x00186FD8
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

	// Token: 0x06001FDB RID: 8155 RVA: 0x00188E2C File Offset: 0x0018702C
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

	// Token: 0x06001FDC RID: 8156 RVA: 0x00188F18 File Offset: 0x00187118
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

	// Token: 0x06001FDD RID: 8157 RVA: 0x0018900F File Offset: 0x0018720F
	public void Open()
	{
		this.m_ReqSequence = PguiOpenWindowCtrl.Sequence.OPEN_START;
	}

	// Token: 0x06001FDE RID: 8158 RVA: 0x00189018 File Offset: 0x00187218
	public bool FinishedOpen()
	{
		return this.m_Sequence == PguiOpenWindowCtrl.Sequence.ACTIVE;
	}

	// Token: 0x06001FDF RID: 8159 RVA: 0x00189023 File Offset: 0x00187223
	public bool FinishedClose()
	{
		return this.m_Sequence == PguiOpenWindowCtrl.Sequence.INACTIVE;
	}

	// Token: 0x06001FE0 RID: 8160 RVA: 0x0018902E File Offset: 0x0018722E
	public void ForceClose()
	{
		if (this.m_Sequence == PguiOpenWindowCtrl.Sequence.ACTIVE)
		{
			this.m_ReqSequence = PguiOpenWindowCtrl.Sequence.CLOSE_START;
		}
	}

	// Token: 0x06001FE1 RID: 8161 RVA: 0x00189040 File Offset: 0x00187240
	public bool StartOpenAnim()
	{
		return this.m_Sequence == PguiOpenWindowCtrl.Sequence.OPEN_WAIT;
	}

	// Token: 0x06001FE2 RID: 8162 RVA: 0x0018904B File Offset: 0x0018724B
	public void ForceOpen()
	{
		this.m_Sequence = PguiOpenWindowCtrl.Sequence.OPEN_START;
	}

	// Token: 0x06001FE3 RID: 8163 RVA: 0x00189054 File Offset: 0x00187254
	public RectTransform GetButtonRectTransform(int btnIndex)
	{
		return this.m_BtnChoice[btnIndex].m_Button.transform as RectTransform;
	}

	// Token: 0x06001FE4 RID: 8164 RVA: 0x00189071 File Offset: 0x00187271
	public RectTransform GetCloseButtonRectTransform()
	{
		return this.m_BtnCloseObj.transform as RectTransform;
	}

	// Token: 0x06001FE5 RID: 8165 RVA: 0x00189083 File Offset: 0x00187283
	public void AddCloseListener()
	{
		this.m_BtnCloseObj.GetComponent<Button>().onClick.AddListener(delegate
		{
			this.OnClickButton(this.m_BtnCloseObj);
		});
	}

	// Token: 0x06001FE6 RID: 8166 RVA: 0x001890A8 File Offset: 0x001872A8
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

	// Token: 0x06001FE7 RID: 8167 RVA: 0x001893D8 File Offset: 0x001875D8
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

	// Token: 0x06001FE8 RID: 8168 RVA: 0x001894EC File Offset: 0x001876EC
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

	// Token: 0x06001FE9 RID: 8169 RVA: 0x0018954B File Offset: 0x0018774B
	private void Update()
	{
		this.UpdateInternal();
	}

	// Token: 0x06001FEA RID: 8170 RVA: 0x00189553 File Offset: 0x00187753
	private void Start()
	{
		this.UpdateInternal();
	}

	// Token: 0x06001FEB RID: 8171 RVA: 0x0018955C File Offset: 0x0018775C
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

	// Token: 0x06001FEC RID: 8172 RVA: 0x001897C4 File Offset: 0x001879C4
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

	// Token: 0x06001FED RID: 8173 RVA: 0x001898E8 File Offset: 0x00187AE8
	public void RegistCallback(PguiOpenWindowCtrl.Callback callback)
	{
		this.m_CallBack = callback;
	}

	// Token: 0x06001FEE RID: 8174 RVA: 0x001898F1 File Offset: 0x00187AF1
	public void OnClickButton(GameObject button)
	{
		this.m_ReqSequence = PguiOpenWindowCtrl.Sequence.CALLBACK_ACTION;
		this.m_LastTouchButton = button;
	}

	// Token: 0x04001722 RID: 5922
	[SerializeField]
	private GameObject m_BaseObject;

	// Token: 0x04001723 RID: 5923
	[SerializeField]
	private RectTransform m_WindowRectTransform;

	// Token: 0x04001724 RID: 5924
	[SerializeField]
	private SimpleAnimation m_animation;

	// Token: 0x04001725 RID: 5925
	[SerializeField]
	private PguiButtonCtrl[] m_BtnChoiceObj = new PguiButtonCtrl[3];

	// Token: 0x04001726 RID: 5926
	[SerializeField]
	private GameObject m_BtnCloseObj;

	// Token: 0x04001727 RID: 5927
	[SerializeField]
	private GameObject m_BtnCheckBoxObj;

	// Token: 0x04001728 RID: 5928
	[SerializeField]
	private GameObject m_ImgCheck;

	// Token: 0x04001729 RID: 5929
	[SerializeField]
	private Text m_TitleText;

	// Token: 0x0400172A RID: 5930
	[SerializeField]
	private Text m_MassageText;

	// Token: 0x0400172B RID: 5931
	[SerializeField]
	private RectTransform m_ScrollViewContent;

	// Token: 0x0400172C RID: 5932
	[SerializeField]
	private List<RectTransform> m_ScrollViewContentList;

	// Token: 0x0400172D RID: 5933
	[SerializeField]
	public RectTransform m_UserInfoContent;

	// Token: 0x0400172E RID: 5934
	[SerializeField]
	private PguiOpenWindowCtrl.WINDOW_TYPE m_windowType;

	// Token: 0x0400172F RID: 5935
	private PguiOpenWindowCtrl.Callback m_CallBack;

	// Token: 0x04001730 RID: 5936
	private UnityAction m_FinishedCloseCallBack;

	// Token: 0x04001731 RID: 5937
	public static readonly int CLOSE_BUTTON_INDEX = -1;

	// Token: 0x04001732 RID: 5938
	private List<PguiOpenWindowCtrl.ChoiceButton> m_BtnChoice;

	// Token: 0x04001733 RID: 5939
	private string[] noStoneDefaultMassage;

	// Token: 0x04001734 RID: 5940
	private string itemInfoStoneAllDefaultText;

	// Token: 0x04001735 RID: 5941
	private string itemInfoStoneDefaultText;

	// Token: 0x04001736 RID: 5942
	private string itemInfoDefaultText;

	// Token: 0x04001737 RID: 5943
	private static List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>>[] PresetButtonList = new List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>>[14];

	// Token: 0x04001738 RID: 5944
	private PguiOpenWindowCtrl.Sequence m_ReqSequence;

	// Token: 0x04001739 RID: 5945
	private PguiOpenWindowCtrl.Sequence m_Sequence = PguiOpenWindowCtrl.Sequence.INACTIVE;

	// Token: 0x0400173A RID: 5946
	private GameObject m_LastTouchButton;

	// Token: 0x0400173B RID: 5947
	private int m_SelectButtonIndex;

	// Token: 0x02001017 RID: 4119
	public enum WINDOW_TYPE
	{
		// Token: 0x04005A65 RID: 23141
		INVALID,
		// Token: 0x04005A66 RID: 23142
		BASIC,
		// Token: 0x04005A67 RID: 23143
		NO_STONE,
		// Token: 0x04005A68 RID: 23144
		USE_ITEM,
		// Token: 0x04005A69 RID: 23145
		FOLLOW,
		// Token: 0x04005A6A RID: 23146
		PURCHASE_STONE,
		// Token: 0x04005A6B RID: 23147
		SERVER_ERROR,
		// Token: 0x04005A6C RID: 23148
		GET_ITEM,
		// Token: 0x04005A6D RID: 23149
		STAMINA_ITEM_SELECT,
		// Token: 0x04005A6E RID: 23150
		STAMINA_ITEM_SETTING,
		// Token: 0x04005A6F RID: 23151
		STAMINA_ITEM_USE,
		// Token: 0x04005A70 RID: 23152
		MONTHLYPACK,
		// Token: 0x04005A71 RID: 23153
		ITEM_INFO,
		// Token: 0x04005A72 RID: 23154
		GET_ITEM_MULTIPLE,
		// Token: 0x04005A73 RID: 23155
		CHECK_BOX,
		// Token: 0x04005A74 RID: 23156
		GET_ACHIEVEMENT
	}

	// Token: 0x02001018 RID: 4120
	public enum BTN_TYPE
	{
		// Token: 0x04005A76 RID: 23158
		NONE,
		// Token: 0x04005A77 RID: 23159
		POSITIVE,
		// Token: 0x04005A78 RID: 23160
		NEGATIVE,
		// Token: 0x04005A79 RID: 23161
		SELECT
	}

	// Token: 0x02001019 RID: 4121
	// (Invoke) Token: 0x060051EA RID: 20970
	public delegate bool Callback(int index);

	// Token: 0x0200101A RID: 4122
	public enum PresetType
	{
		// Token: 0x04005A7B RID: 23163
		CLOSE,
		// Token: 0x04005A7C RID: 23164
		NO_YES,
		// Token: 0x04005A7D RID: 23165
		CANCEL_OK,
		// Token: 0x04005A7E RID: 23166
		LR_CURSOR,
		// Token: 0x04005A7F RID: 23167
		OK,
		// Token: 0x04005A80 RID: 23168
		STORE,
		// Token: 0x04005A81 RID: 23169
		CANCEL_MOVE,
		// Token: 0x04005A82 RID: 23170
		CANCEL_SET,
		// Token: 0x04005A83 RID: 23171
		CLOSE_SHOP,
		// Token: 0x04005A84 RID: 23172
		NO_YES_EMPTY,
		// Token: 0x04005A85 RID: 23173
		REVIEW,
		// Token: 0x04005A86 RID: 23174
		TITLE_MENT,
		// Token: 0x04005A87 RID: 23175
		CLOSE_GREEN,
		// Token: 0x04005A88 RID: 23176
		NEXT,
		// Token: 0x04005A89 RID: 23177
		MAX
	}

	// Token: 0x0200101B RID: 4123
	private enum Sequence
	{
		// Token: 0x04005A8B RID: 23179
		NONE,
		// Token: 0x04005A8C RID: 23180
		INACTIVE,
		// Token: 0x04005A8D RID: 23181
		OPEN_START,
		// Token: 0x04005A8E RID: 23182
		OPEN_WAIT,
		// Token: 0x04005A8F RID: 23183
		ACTIVE,
		// Token: 0x04005A90 RID: 23184
		CALLBACK_ACTION,
		// Token: 0x04005A91 RID: 23185
		CLOSE_START,
		// Token: 0x04005A92 RID: 23186
		CLOSE_WAIT
	}

	// Token: 0x0200101C RID: 4124
	private class ChoiceButton
	{
		// Token: 0x060051ED RID: 20973 RVA: 0x00247F9C File Offset: 0x0024619C
		public ChoiceButton(PguiButtonCtrl button)
		{
			this.m_Button = button;
			if (this.m_Button != null && null != button.transform.Find("BaseImage/Text"))
			{
				this.m_Text = button.transform.Find("BaseImage/Text").GetComponent<Text>();
				this.m_OutlineList = button.transform.Find("BaseImage/Text").GetComponents<PguiOutline>();
			}
		}

		// Token: 0x060051EE RID: 20974 RVA: 0x00248012 File Offset: 0x00246212
		public void SetActive(bool act)
		{
			if (this.m_Button != null)
			{
				this.m_Button.gameObject.SetActive(act);
			}
		}

		// Token: 0x04005A93 RID: 23187
		public PguiButtonCtrl m_Button;

		// Token: 0x04005A94 RID: 23188
		public Text m_Text;

		// Token: 0x04005A95 RID: 23189
		public PguiOutline[] m_OutlineList;

		// Token: 0x04005A96 RID: 23190
		public int m_Index;

		// Token: 0x04005A97 RID: 23191
		public PguiOpenWindowCtrl.BTN_TYPE m_Type;
	}
}
