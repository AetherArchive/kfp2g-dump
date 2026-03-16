using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Battle
{
	public class SceneBattle_MenuGUI
	{
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

		public SimpleAnimation anim;

		public PguiButtonCtrl btnInfo;

		public PguiButtonCtrl btnRetire;

		public PguiButtonCtrl btnCancel;

		public PguiButtonCtrl btnClose;

		public PguiTextCtrl txtWave;

		public PguiTextCtrl numWave;

		public PguiTextCtrl numTurn;

		public HorizontalLayoutGroup icnCharaBase;

		public List<PguiRawImageCtrl> icnChara;
	}
}
