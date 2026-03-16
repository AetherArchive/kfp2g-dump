using System;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
	public class SceneBattle_ContinueGUI
	{
		public SceneBattle_ContinueGUI(Transform baseTr)
		{
			this.btnYes = baseTr.Find("Base/Window/ButtonR").GetComponent<PguiButtonCtrl>();
			this.btnNo = baseTr.Find("Base/Window/ButtonL").GetComponent<PguiButtonCtrl>();
			this.btnClose = baseTr.Find("Base/Window/BtnClose").GetComponent<PguiButtonCtrl>();
			this.btnPurchaseConfirm = baseTr.Find("Base/Window/PurchaseConfirmButton").GetComponent<PguiButtonCtrl>();
			this.anim = baseTr.Find("Base").GetComponent<SimpleAnimation>();
			this.numWave = baseTr.Find("Base/Window/Box01/Param01/Num01").GetComponent<PguiTextCtrl>();
			this.numTurn = baseTr.Find("Base/Window/Box01/Param02/Num02").GetComponent<PguiTextCtrl>();
			Transform transform = baseTr.Find("Base/Window/Box02");
			ItemStaticBase itemStaticBase = DataManager.DmItem.GetItemStaticBase(30101);
			transform.Find("Coin").GetComponent<PguiRawImageCtrl>().SetRawImage(itemStaticBase.GetIconName(), true, false, null);
			transform.Find("Title_03").GetComponent<PguiTextCtrl>().text = "獲得" + itemStaticBase.GetName();
			this.numCoin = transform.Find("Param01/Num03").GetComponent<PguiTextCtrl>();
			transform = baseTr.Find("Base/Window/Box03/Icon_TreasureBox");
			this.dropItem = new List<PguiReplaceSpriteCtrl>();
			foreach (object obj in transform)
			{
				Transform transform2 = (Transform)obj;
				this.dropItem.Add(transform2.GetComponent<PguiReplaceSpriteCtrl>());
			}
			transform = baseTr.Find("Base/Window/Shizai");
			transform.Find("Ishi").GetComponent<PguiRawImageCtrl>().SetRawImage(DataManager.DmItem.GetItemStaticBase(30100).GetIconName(), true, false, null);
			this.numAfter = transform.Find("Num05").GetComponent<PguiTextCtrl>();
			this.numBefore = transform.Find("Num04").GetComponent<PguiTextCtrl>();
			this.notStone = transform.Find("Txt_None").gameObject;
			this.buyStone = this.btnYes.transform.Find("BaseImage/Txt_Buy").gameObject;
			PguiTextCtrl component = baseTr.Find("Base/Window/Message").GetComponent<PguiTextCtrl>();
			component.text = component.text.Replace("Param01", DataManager.DmQuest.GetContinueStoneNum().ToString());
		}

		public SimpleAnimation anim;

		public PguiButtonCtrl btnYes;

		public PguiButtonCtrl btnNo;

		public PguiButtonCtrl btnClose;

		public PguiButtonCtrl btnPurchaseConfirm;

		public PguiTextCtrl numWave;

		public PguiTextCtrl numTurn;

		public PguiTextCtrl numCoin;

		public PguiTextCtrl numBefore;

		public PguiTextCtrl numAfter;

		public GameObject notStone;

		public GameObject buyStone;

		public List<PguiReplaceSpriteCtrl> dropItem;
	}
}
