using System;
using System.Collections.Generic;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.UI;

namespace Battle
{
	// Token: 0x0200021A RID: 538
	public class SceneBattle_InfoGUI
	{
		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x060022D8 RID: 8920 RVA: 0x00194971 File Offset: 0x00192B71
		public SimpleAnimation anim
		{
			get
			{
				return this._anim;
			}
		}

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x060022D9 RID: 8921 RVA: 0x00194979 File Offset: 0x00192B79
		public PguiAECtrl animAE
		{
			get
			{
				return this._animAE;
			}
		}

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x060022DA RID: 8922 RVA: 0x00194981 File Offset: 0x00192B81
		public PguiButtonCtrl btnClose
		{
			get
			{
				return this._btnClose;
			}
		}

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x060022DB RID: 8923 RVA: 0x00194989 File Offset: 0x00192B89
		public PguiButtonCtrl btnLeft
		{
			get
			{
				return this._btnLeft;
			}
		}

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x060022DC RID: 8924 RVA: 0x00194991 File Offset: 0x00192B91
		public PguiButtonCtrl btnRight
		{
			get
			{
				return this._btnRight;
			}
		}

		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x060022DD RID: 8925 RVA: 0x00194999 File Offset: 0x00192B99
		public Text txtName
		{
			get
			{
				return this._txtName;
			}
		}

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x060022DE RID: 8926 RVA: 0x001949A1 File Offset: 0x00192BA1
		public GameObject mrkHelper
		{
			get
			{
				return this._mrkHelper;
			}
		}

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x060022DF RID: 8927 RVA: 0x001949A9 File Offset: 0x00192BA9
		public IconCharaCtrl icnChara
		{
			get
			{
				return this._icnChara;
			}
		}

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x060022E0 RID: 8928 RVA: 0x001949B1 File Offset: 0x00192BB1
		public List<Transform> infPhoto
		{
			get
			{
				return this._infPhoto;
			}
		}

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x060022E1 RID: 8929 RVA: 0x001949B9 File Offset: 0x00192BB9
		public List<Transform> icsPhoto
		{
			get
			{
				return this._icsPhoto;
			}
		}

		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x060022E2 RID: 8930 RVA: 0x001949C1 File Offset: 0x00192BC1
		public List<IconPhotoCtrl> icnPhoto
		{
			get
			{
				return this._icnPhoto;
			}
		}

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x060022E3 RID: 8931 RVA: 0x001949C9 File Offset: 0x00192BC9
		public Transform staPhoto
		{
			get
			{
				return this._staPhoto;
			}
		}

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x060022E4 RID: 8932 RVA: 0x001949D1 File Offset: 0x00192BD1
		public PguiTabGroupCtrl tabInfo
		{
			get
			{
				return this._tabInfo;
			}
		}

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x060022E5 RID: 8933 RVA: 0x001949D9 File Offset: 0x00192BD9
		public GameObject infStatus
		{
			get
			{
				return this._infStatus;
			}
		}

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x060022E6 RID: 8934 RVA: 0x001949E1 File Offset: 0x00192BE1
		public Transform infStatLv
		{
			get
			{
				return this._infStatLv;
			}
		}

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x060022E7 RID: 8935 RVA: 0x001949E9 File Offset: 0x00192BE9
		public Transform infStatPrm
		{
			get
			{
				return this._infStatPrm;
			}
		}

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x060022E8 RID: 8936 RVA: 0x001949F1 File Offset: 0x00192BF1
		public GameObject infSkill
		{
			get
			{
				return this._infSkill;
			}
		}

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x060022E9 RID: 8937 RVA: 0x001949F9 File Offset: 0x00192BF9
		public GameObject infBuff
		{
			get
			{
				return this._infBuff;
			}
		}

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x060022EA RID: 8938 RVA: 0x00194A01 File Offset: 0x00192C01
		public Transform icnOrderCard
		{
			get
			{
				return this._icnOrderCard;
			}
		}

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x060022EB RID: 8939 RVA: 0x00194A09 File Offset: 0x00192C09
		public List<Transform> scrSkill
		{
			get
			{
				return this._scrSkill;
			}
		}

		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x060022EC RID: 8940 RVA: 0x00194A11 File Offset: 0x00192C11
		public ReuseScroll scrBuff
		{
			get
			{
				return this._scrBuff;
			}
		}

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x060022ED RID: 8941 RVA: 0x00194A19 File Offset: 0x00192C19
		public PguiAECtrl closeAE
		{
			get
			{
				return this._closeAE;
			}
		}

		// Token: 0x060022EE RID: 8942 RVA: 0x00194A24 File Offset: 0x00192C24
		public SceneBattle_InfoGUI(Transform baseTr)
		{
			this._anim = baseTr.Find("All").GetComponent<SimpleAnimation>();
			this._animAE = baseTr.Find("All/AEImage_WindowOpen").GetComponent<PguiAECtrl>();
			this._btnClose = baseTr.Find("All/Btn_Close").GetComponent<PguiButtonCtrl>();
			this._btnLeft = baseTr.Find("All/Btn_Yaji_Left").GetComponent<PguiButtonCtrl>();
			this._btnRight = baseTr.Find("All/Btn_Yaji_Right").GetComponent<PguiButtonCtrl>();
			this._txtName = baseTr.Find("All/Name/Txt_Name").GetComponent<Text>();
			this._mrkHelper = baseTr.Find("All/Name/Mark_Friend").gameObject;
			this._icnChara = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Chara).GetComponent<IconCharaCtrl>();
			this._icnChara.transform.SetParent(baseTr.Find("All/Left/Icon_Chara"), false);
			this._infPhoto = new List<Transform>();
			this._icsPhoto = new List<Transform>();
			this._icnPhoto = new List<IconPhotoCtrl>();
			int num = 1;
			for (;;)
			{
				Transform transform = baseTr.Find("All/Left/Icon_Photo" + num.ToString("D2"));
				if (transform == null)
				{
					break;
				}
				this._infPhoto.Add(transform);
				transform = transform.Find("Icon_Photo").GetChild(0);
				this._icsPhoto.Add(transform);
				IconPhotoCtrl iconPhotoCtrl = ((transform == null) ? null : Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Photo).GetComponent<IconPhotoCtrl>());
				if (iconPhotoCtrl != null)
				{
					iconPhotoCtrl.transform.SetParent(transform.Find("Icon_Photo"), false);
				}
				this._icnPhoto.Add(iconPhotoCtrl);
				if (transform != null)
				{
					transform = transform.Find("Count");
				}
				if (transform != null)
				{
					transform.gameObject.SetActive(false);
				}
				num++;
			}
			this._staPhoto = baseTr.Find("All/Left/CurrentInfo");
			this._tabInfo = baseTr.Find("All/RightBase/TabGroup").GetComponent<PguiTabGroupCtrl>();
			this._infStatus = baseTr.Find("All/RightBase/Info_Status").gameObject;
			this._infSkill = baseTr.Find("All/RightBase/Info_Skill").gameObject;
			this._infSkill.transform.Find("ScrollView").GetComponent<ScrollRect>().scrollSensitivity = ScrollParamDefine.BattleCharaInfo;
			this._infBuff = baseTr.Find("All/RightBase/Info_BattleStatus").gameObject;
			this._infStatLv = this._infStatus.transform.Find("Box01/Base_Lv");
			this._infStatPrm = this._infStatus.transform.Find("Box01/Parameter");
			this._icnOrderCard = this._infStatus.transform.Find("Box02");
			this._scrSkill = new List<Transform>();
			this._scrSkill.Add(this._infSkill.transform.Find("ScrollView/Viewport/Content/CharaInfo_List_Skill_Short_01"));
			this._scrSkill.Add(this._infSkill.transform.Find("ScrollView/Viewport/Content/CharaInfo_List_Skill_Short_02"));
			this._scrSkill.Add(this._infSkill.transform.Find("ScrollView/Viewport/Content/CharaInfo_List_Skill_Short_03"));
			this._scrSkill.Add(this._infSkill.transform.Find("ScrollView/Viewport/Content/CharaInfo_List_Skill_Short_04"));
			this._scrSkill.Add(this._infSkill.transform.Find("ScrollView/Viewport/Content/CharaInfo_List_Skill_Short_05_kiseki"));
			this._scrSkill.Add(this._infSkill.transform.Find("ScrollView/Viewport/Content/CharaInfo_List_Skill_Short_06_nanairo"));
			this._scrBuff = this._infBuff.transform.Find("ScrollView").GetComponent<ReuseScroll>();
			this._closeAE = baseTr.Find("AEImage_Close").GetComponent<PguiAECtrl>();
		}

		// Token: 0x040019FA RID: 6650
		private SimpleAnimation _anim;

		// Token: 0x040019FB RID: 6651
		private PguiAECtrl _animAE;

		// Token: 0x040019FC RID: 6652
		private PguiButtonCtrl _btnClose;

		// Token: 0x040019FD RID: 6653
		private PguiButtonCtrl _btnLeft;

		// Token: 0x040019FE RID: 6654
		private PguiButtonCtrl _btnRight;

		// Token: 0x040019FF RID: 6655
		private Text _txtName;

		// Token: 0x04001A00 RID: 6656
		private GameObject _mrkHelper;

		// Token: 0x04001A01 RID: 6657
		private IconCharaCtrl _icnChara;

		// Token: 0x04001A02 RID: 6658
		private List<Transform> _infPhoto;

		// Token: 0x04001A03 RID: 6659
		private List<Transform> _icsPhoto;

		// Token: 0x04001A04 RID: 6660
		private List<IconPhotoCtrl> _icnPhoto;

		// Token: 0x04001A05 RID: 6661
		private Transform _staPhoto;

		// Token: 0x04001A06 RID: 6662
		private PguiTabGroupCtrl _tabInfo;

		// Token: 0x04001A07 RID: 6663
		private GameObject _infStatus;

		// Token: 0x04001A08 RID: 6664
		private Transform _infStatLv;

		// Token: 0x04001A09 RID: 6665
		private Transform _infStatPrm;

		// Token: 0x04001A0A RID: 6666
		private GameObject _infSkill;

		// Token: 0x04001A0B RID: 6667
		private GameObject _infBuff;

		// Token: 0x04001A0C RID: 6668
		private Transform _icnOrderCard;

		// Token: 0x04001A0D RID: 6669
		private List<Transform> _scrSkill;

		// Token: 0x04001A0E RID: 6670
		private ReuseScroll _scrBuff;

		// Token: 0x04001A0F RID: 6671
		private PguiAECtrl _closeAE;
	}
}
