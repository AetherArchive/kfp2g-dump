using System;
using System.Collections.Generic;
using System.Linq;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;
using SGNFW.uGUI;
using UnityEngine;

public class CmnKizunaBuffWindowCtrl : MonoBehaviour
{
	public void Setup(GameObject obj)
	{
		this.buffInfoData = new CmnKizunaBuffWindowCtrl.GUIBuffInfoData(obj);
		this.buffInfoData.Setup();
	}

	public void SetupBuffInfo()
	{
		this.buffInfoData.SetupInfo(this.buffInfoData.currentTabIndex);
	}

	public CmnKizunaBuffWindowCtrl.GUIBuffInfoData buffInfoData;

	public class GUIBuffInfoData
	{
		public GUIBuffInfoData(GameObject obj)
		{
			this.baseObj = obj;
			Transform transform = this.baseObj.transform;
			string text = "Window_BuffInfo/Base/Window/";
			this.bonusInfoObj = transform.Find(text + "Tab_All/BonusInfo").gameObject;
			this.bonusTotalInfoObj = transform.Find(text + "Tab_All/BonusTotalInfo").gameObject;
			this.owCtrl = transform.Find("Window_BuffInfo").GetComponent<PguiOpenWindowCtrl>();
			this.tab = transform.Find(text + "Tab_All/TabGroup").GetComponent<PguiTabGroupCtrl>();
			this.buffTargetInfoTxt = transform.Find(text + "Tab_All/BonusInfo/PointInfo/Txt_Target").GetComponent<PguiTextCtrl>();
			this.buffQualifiedInfoTxt = transform.Find(text + "Tab_All/BonusInfo/PointInfo/Txt_Qualified").GetComponent<PguiTextCtrl>();
			string text2 = text + "Tab_All/BonusTotalInfo/VerticalLayout/";
			this.hpInfoTxt = transform.Find(text2 + "Info_HP/Num").GetComponent<PguiTextCtrl>();
			this.atkInfoTxt = transform.Find(text2 + "Info_Attack/Num").GetComponent<PguiTextCtrl>();
			this.defInfoTxt = transform.Find(text2 + "Info_Guard/Num").GetComponent<PguiTextCtrl>();
			this.avoidInfoTxt = transform.Find(text2 + "Info_Avoid/Num").GetComponent<PguiTextCtrl>();
			this.beatInfoTxt = transform.Find(text2 + "Info_Flag_Beat/Num").GetComponent<PguiTextCtrl>();
			this.actionInfoTxt = transform.Find(text2 + "Info_Flag_Action/Num").GetComponent<PguiTextCtrl>();
			this.tryInfoTxt = transform.Find(text2 + "Info_Flag_Try/Num").GetComponent<PguiTextCtrl>();
			this.scroll = transform.Find(text + "Tab_All/BonusInfo/ScrollView").GetComponent<ReuseScroll>();
			this.scroll.InitForce();
			ReuseScroll reuseScroll = this.scroll;
			reuseScroll.onStartItem = (Action<int, GameObject>)Delegate.Combine(reuseScroll.onStartItem, new Action<int, GameObject>(this.SetupBuffInfoItem));
			ReuseScroll reuseScroll2 = this.scroll;
			reuseScroll2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(reuseScroll2.onUpdateItem, new Action<int, GameObject>(this.UpdateBuffInfoItem));
			this.currentTabIndex = 0;
		}

		public void Setup()
		{
			List<MstCharaKizunaBuffData> mstKizunaBuffList = DataManager.DmChara.GetMstKizunaBuffList();
			CharaKizunaQualified qualifiedData = DataManager.DmChara.GetUserKizunaQualified();
			this.currentBuffInfo = mstKizunaBuffList.FindAll((MstCharaKizunaBuffData mst) => mst.id == qualifiedData.buff_id).ToList<MstCharaKizunaBuffData>();
			this.tab.Setup(this.currentTabIndex, new PguiTabGroupCtrl.OnSelectTab(this.OnSelectTab));
			this.scroll.Setup((this.currentBuffInfo == null) ? 0 : this.currentBuffInfo.Count, 0);
			this.scroll.Resize((this.currentBuffInfo == null) ? 0 : this.currentBuffInfo.Count, 0);
		}

		public void SetupInfo(int index)
		{
			CharaKizunaQualified userKizunaQualified = DataManager.DmChara.GetUserKizunaQualified();
			MstCharaKizunaBuffData mstCharaKizunaBuffData = this.currentBuffInfo.FirstOrDefault<MstCharaKizunaBuffData>();
			if (index == 0)
			{
				this.buffTargetInfoTxt.ReplaceTextByDefault("Param01", mstCharaKizunaBuffData.targetLevel.ToString());
				this.buffQualifiedInfoTxt.ReplaceTextByDefault("Param01", userKizunaQualified.qualified_count.ToString());
				return;
			}
			PrjUtil.ParamPreset activeKizunaBuff = DataManager.DmChara.GetActiveKizunaBuff();
			int hp = activeKizunaBuff.hp;
			int atk = activeKizunaBuff.atk;
			int def = activeKizunaBuff.def;
			int avoid = activeKizunaBuff.avoid;
			int beatDamageRatio = activeKizunaBuff.beatDamageRatio;
			int actionDamageRatio = activeKizunaBuff.actionDamageRatio;
			int tryDamageRatio = activeKizunaBuff.tryDamageRatio;
			this.hpInfoTxt.ReplaceTextByDefault("Param01", hp.ToString());
			this.atkInfoTxt.ReplaceTextByDefault("Param01", atk.ToString());
			this.defInfoTxt.ReplaceTextByDefault("Param01", def.ToString());
			this.avoidInfoTxt.ReplaceTextByDefault("Param01", ((float)avoid / 10f).ToString("F1"));
			this.beatInfoTxt.ReplaceTextByDefault("Param01", ((float)beatDamageRatio / 10f).ToString("F1"));
			this.actionInfoTxt.ReplaceTextByDefault("Param01", ((float)actionDamageRatio / 10f).ToString("F1"));
			this.tryInfoTxt.ReplaceTextByDefault("Param01", ((float)tryDamageRatio / 10f).ToString("F1"));
		}

		private void SetupBuffInfoItem(int index, GameObject go)
		{
			this.UpdateBuffInfoItem(index, go);
		}

		private void UpdateBuffInfoItem(int index, GameObject go)
		{
			go.SetActive(this.currentBuffInfo.Count<MstCharaKizunaBuffData>() >= index);
			if (this.currentBuffInfo.Count<MstCharaKizunaBuffData>() <= index)
			{
				return;
			}
			Transform transform = go.transform.Find("BaseImage");
			MstCharaKizunaBuffData mstCharaKizunaBuffData = this.currentBuffInfo[index];
			transform.Find("Info/Txt").GetComponent<PguiTextCtrl>().ReplaceTextByDefault("Param01", mstCharaKizunaBuffData.requiredCount.ToString());
			bool flag = mstCharaKizunaBuffData.hpBonus != 0;
			bool flag2 = mstCharaKizunaBuffData.atkBonus != 0;
			bool flag3 = mstCharaKizunaBuffData.defBonus != 0;
			bool flag4 = mstCharaKizunaBuffData.avoidBonus != 0;
			bool flag5 = mstCharaKizunaBuffData.beatBonus != 0;
			bool flag6 = mstCharaKizunaBuffData.actionBonus != 0;
			bool flag7 = mstCharaKizunaBuffData.tryBonus != 0;
			bool flag8 = flag || flag2 || flag3 || flag4;
			bool flag9 = flag5 || flag6 || flag7;
			CharaKizunaQualified userKizunaQualified = DataManager.DmChara.GetUserKizunaQualified();
			bool flag10 = mstCharaKizunaBuffData.requiredCount <= userKizunaQualified.qualified_count;
			PguiTextCtrl component = transform.Find("VerticalLayout/GridLayoutStatus/HpBuff").GetComponent<PguiTextCtrl>();
			PguiTextCtrl component2 = transform.Find("VerticalLayout/GridLayoutStatus/AttackBuff").GetComponent<PguiTextCtrl>();
			PguiTextCtrl component3 = transform.Find("VerticalLayout/GridLayoutStatus/DefBuff").GetComponent<PguiTextCtrl>();
			PguiTextCtrl component4 = transform.Find("VerticalLayout/GridLayoutStatus/AvoidBuff").GetComponent<PguiTextCtrl>();
			PguiTextCtrl component5 = transform.Find("VerticalLayout/GridLayoutFlag/BeatBuff").GetComponent<PguiTextCtrl>();
			PguiTextCtrl component6 = transform.Find("VerticalLayout/GridLayoutFlag/ActionBuff").GetComponent<PguiTextCtrl>();
			PguiTextCtrl component7 = transform.Find("VerticalLayout/GridLayoutFlag/TryBuff").GetComponent<PguiTextCtrl>();
			GameObject gameObject = transform.Find("VerticalLayout/GridLayoutStatus").gameObject;
			GameObject gameObject2 = transform.Find("VerticalLayout/GridLayoutFlag").gameObject;
			GameObject gameObject3 = transform.parent.Find("MaskBase").gameObject;
			component.gameObject.SetActive(flag);
			component2.gameObject.SetActive(flag2);
			component3.gameObject.SetActive(flag3);
			component4.gameObject.SetActive(flag4);
			component5.gameObject.SetActive(flag5);
			component6.gameObject.SetActive(flag6);
			component7.gameObject.SetActive(flag7);
			gameObject.SetActive(flag8);
			gameObject2.SetActive(flag9);
			gameObject3.SetActive(flag10);
			if (component.gameObject.activeSelf)
			{
				component.ReplaceTextByDefault("Param01", mstCharaKizunaBuffData.hpBonus.ToString());
			}
			if (component2.gameObject.activeSelf)
			{
				component2.ReplaceTextByDefault("Param01", mstCharaKizunaBuffData.atkBonus.ToString());
			}
			if (component3.gameObject.activeSelf)
			{
				component3.ReplaceTextByDefault("Param01", mstCharaKizunaBuffData.defBonus.ToString());
			}
			if (component4.gameObject.activeSelf)
			{
				component4.ReplaceTextByDefault("Param01", ((float)mstCharaKizunaBuffData.avoidBonus / 10f).ToString());
			}
			if (component5.gameObject.activeSelf)
			{
				component5.ReplaceTextByDefault("Param01", ((float)mstCharaKizunaBuffData.beatBonus / 10f).ToString());
			}
			if (component6.gameObject.activeSelf)
			{
				component6.ReplaceTextByDefault("Param01", ((float)mstCharaKizunaBuffData.actionBonus / 10f).ToString());
			}
			if (component7.gameObject.activeSelf)
			{
				component7.ReplaceTextByDefault("Param01", ((float)mstCharaKizunaBuffData.tryBonus / 10f).ToString());
			}
		}

		private bool OnSelectTab(int index)
		{
			this.currentTabIndex = index;
			this.SetupInfo(index);
			this.bonusInfoObj.SetActive(index == 0);
			this.bonusTotalInfoObj.SetActive(index == 1);
			return true;
		}

		public GameObject baseObj;

		public GameObject bonusInfoObj;

		public GameObject bonusTotalInfoObj;

		public PguiOpenWindowCtrl owCtrl;

		public PguiTabGroupCtrl tab;

		public PguiTextCtrl buffTargetInfoTxt;

		public PguiTextCtrl buffQualifiedInfoTxt;

		public PguiTextCtrl hpInfoTxt;

		public PguiTextCtrl atkInfoTxt;

		public PguiTextCtrl defInfoTxt;

		public PguiTextCtrl avoidInfoTxt;

		public PguiTextCtrl beatInfoTxt;

		public PguiTextCtrl actionInfoTxt;

		public PguiTextCtrl tryInfoTxt;

		public ReuseScroll scroll;

		private List<MstCharaKizunaBuffData> currentBuffInfo;

		public int currentTabIndex;
	}
}
