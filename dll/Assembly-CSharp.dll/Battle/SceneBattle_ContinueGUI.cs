using System;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
	// Token: 0x02000211 RID: 529
	public class SceneBattle_ContinueGUI
	{
		// Token: 0x06002238 RID: 8760 RVA: 0x00192280 File Offset: 0x00190480
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

		// Token: 0x0400191C RID: 6428
		public SimpleAnimation anim;

		// Token: 0x0400191D RID: 6429
		public PguiButtonCtrl btnYes;

		// Token: 0x0400191E RID: 6430
		public PguiButtonCtrl btnNo;

		// Token: 0x0400191F RID: 6431
		public PguiButtonCtrl btnClose;

		// Token: 0x04001920 RID: 6432
		public PguiButtonCtrl btnPurchaseConfirm;

		// Token: 0x04001921 RID: 6433
		public PguiTextCtrl numWave;

		// Token: 0x04001922 RID: 6434
		public PguiTextCtrl numTurn;

		// Token: 0x04001923 RID: 6435
		public PguiTextCtrl numCoin;

		// Token: 0x04001924 RID: 6436
		public PguiTextCtrl numBefore;

		// Token: 0x04001925 RID: 6437
		public PguiTextCtrl numAfter;

		// Token: 0x04001926 RID: 6438
		public GameObject notStone;

		// Token: 0x04001927 RID: 6439
		public GameObject buyStone;

		// Token: 0x04001928 RID: 6440
		public List<PguiReplaceSpriteCtrl> dropItem;
	}
}
