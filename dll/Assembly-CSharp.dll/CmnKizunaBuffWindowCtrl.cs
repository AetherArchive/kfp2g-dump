using System;
using System.Collections.Generic;
using System.Linq;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;
using SGNFW.uGUI;
using UnityEngine;

// Token: 0x02000198 RID: 408
public class CmnKizunaBuffWindowCtrl : MonoBehaviour
{
	// Token: 0x06001B25 RID: 6949 RVA: 0x0015D94D File Offset: 0x0015BB4D
	public void Setup(GameObject obj)
	{
		this.buffInfoData = new CmnKizunaBuffWindowCtrl.GUIBuffInfoData(obj);
		this.buffInfoData.Setup();
	}

	// Token: 0x06001B26 RID: 6950 RVA: 0x0015D966 File Offset: 0x0015BB66
	public void SetupBuffInfo()
	{
		this.buffInfoData.SetupInfo(this.buffInfoData.currentTabIndex);
	}

	// Token: 0x04001483 RID: 5251
	public CmnKizunaBuffWindowCtrl.GUIBuffInfoData buffInfoData;

	// Token: 0x02000EAB RID: 3755
	public class GUIBuffInfoData
	{
		// Token: 0x06004D51 RID: 19793 RVA: 0x00231D44 File Offset: 0x0022FF44
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

		// Token: 0x06004D52 RID: 19794 RVA: 0x00231F60 File Offset: 0x00230160
		public void Setup()
		{
			List<MstCharaKizunaBuffData> mstKizunaBuffList = DataManager.DmChara.GetMstKizunaBuffList();
			CharaKizunaQualified qualifiedData = DataManager.DmChara.GetUserKizunaQualified();
			this.currentBuffInfo = mstKizunaBuffList.FindAll((MstCharaKizunaBuffData mst) => mst.id == qualifiedData.buff_id).ToList<MstCharaKizunaBuffData>();
			this.tab.Setup(this.currentTabIndex, new PguiTabGroupCtrl.OnSelectTab(this.OnSelectTab));
			this.scroll.Setup((this.currentBuffInfo == null) ? 0 : this.currentBuffInfo.Count, 0);
			this.scroll.Resize((this.currentBuffInfo == null) ? 0 : this.currentBuffInfo.Count, 0);
		}

		// Token: 0x06004D53 RID: 19795 RVA: 0x0023200C File Offset: 0x0023020C
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

		// Token: 0x06004D54 RID: 19796 RVA: 0x0023218A File Offset: 0x0023038A
		private void SetupBuffInfoItem(int index, GameObject go)
		{
			this.UpdateBuffInfoItem(index, go);
		}

		// Token: 0x06004D55 RID: 19797 RVA: 0x00232194 File Offset: 0x00230394
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

		// Token: 0x06004D56 RID: 19798 RVA: 0x002324E4 File Offset: 0x002306E4
		private bool OnSelectTab(int index)
		{
			this.currentTabIndex = index;
			this.SetupInfo(index);
			this.bonusInfoObj.SetActive(index == 0);
			this.bonusTotalInfoObj.SetActive(index == 1);
			return true;
		}

		// Token: 0x04005422 RID: 21538
		public GameObject baseObj;

		// Token: 0x04005423 RID: 21539
		public GameObject bonusInfoObj;

		// Token: 0x04005424 RID: 21540
		public GameObject bonusTotalInfoObj;

		// Token: 0x04005425 RID: 21541
		public PguiOpenWindowCtrl owCtrl;

		// Token: 0x04005426 RID: 21542
		public PguiTabGroupCtrl tab;

		// Token: 0x04005427 RID: 21543
		public PguiTextCtrl buffTargetInfoTxt;

		// Token: 0x04005428 RID: 21544
		public PguiTextCtrl buffQualifiedInfoTxt;

		// Token: 0x04005429 RID: 21545
		public PguiTextCtrl hpInfoTxt;

		// Token: 0x0400542A RID: 21546
		public PguiTextCtrl atkInfoTxt;

		// Token: 0x0400542B RID: 21547
		public PguiTextCtrl defInfoTxt;

		// Token: 0x0400542C RID: 21548
		public PguiTextCtrl avoidInfoTxt;

		// Token: 0x0400542D RID: 21549
		public PguiTextCtrl beatInfoTxt;

		// Token: 0x0400542E RID: 21550
		public PguiTextCtrl actionInfoTxt;

		// Token: 0x0400542F RID: 21551
		public PguiTextCtrl tryInfoTxt;

		// Token: 0x04005430 RID: 21552
		public ReuseScroll scroll;

		// Token: 0x04005431 RID: 21553
		private List<MstCharaKizunaBuffData> currentBuffInfo;

		// Token: 0x04005432 RID: 21554
		public int currentTabIndex;
	}
}
