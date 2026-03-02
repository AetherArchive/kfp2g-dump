using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Battle
{
	// Token: 0x0200021C RID: 540
	public class SceneBattle_MenuGUI
	{
		// Token: 0x060022F0 RID: 8944 RVA: 0x00194DE4 File Offset: 0x00192FE4
		public SceneBattle_MenuGUI(Transform baseTr)
		{
			this.btnInfo = baseTr.Find("Base/Window/Box00/Btn_CharaInfo").GetComponent<PguiButtonCtrl>();
			this.btnRetire = baseTr.Find("Base/Window/ButtonR").GetComponent<PguiButtonCtrl>();
			this.btnCancel = baseTr.Find("Base/Window/ButtonL").GetComponent<PguiButtonCtrl>();
			this.btnClose = baseTr.Find("Base/Window/BtnClose").GetComponent<PguiButtonCtrl>();
			this.anim = baseTr.Find("Base").GetComponent<SimpleAnimation>();
			this.txtWave = baseTr.Find("Base/Window/Box01/Param01/Txt_01").GetComponent<PguiTextCtrl>();
			this.numWave = baseTr.Find("Base/Window/Box01/Param01/Num01").GetComponent<PguiTextCtrl>();
			this.numTurn = baseTr.Find("Base/Window/Box01/Param02/Num02").GetComponent<PguiTextCtrl>();
			this.icnCharaBase = baseTr.Find("Base/Window/Box00/Icon_Chara").GetComponent<HorizontalLayoutGroup>();
			this.icnChara = new List<PguiRawImageCtrl>();
			int num = 1;
			for (;;)
			{
				Transform transform = this.icnCharaBase.transform.Find("Icon_Chara" + num.ToString("D2"));
				if (transform == null)
				{
					break;
				}
				PguiRawImageCtrl component = transform.GetComponent<PguiRawImageCtrl>();
				component.SetTexture(null, true);
				this.icnChara.Add(component);
				num++;
			}
		}

		// Token: 0x04001A16 RID: 6678
		public SimpleAnimation anim;

		// Token: 0x04001A17 RID: 6679
		public PguiButtonCtrl btnInfo;

		// Token: 0x04001A18 RID: 6680
		public PguiButtonCtrl btnRetire;

		// Token: 0x04001A19 RID: 6681
		public PguiButtonCtrl btnCancel;

		// Token: 0x04001A1A RID: 6682
		public PguiButtonCtrl btnClose;

		// Token: 0x04001A1B RID: 6683
		public PguiTextCtrl txtWave;

		// Token: 0x04001A1C RID: 6684
		public PguiTextCtrl numWave;

		// Token: 0x04001A1D RID: 6685
		public PguiTextCtrl numTurn;

		// Token: 0x04001A1E RID: 6686
		public HorizontalLayoutGroup icnCharaBase;

		// Token: 0x04001A1F RID: 6687
		public List<PguiRawImageCtrl> icnChara;
	}
}
