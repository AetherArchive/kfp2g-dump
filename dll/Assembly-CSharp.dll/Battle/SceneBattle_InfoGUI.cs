using System;
using System.Collections.Generic;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.UI;

namespace Battle
{
	public class SceneBattle_InfoGUI
	{
		public SimpleAnimation anim
		{
			get
			{
				return this._anim;
			}
		}

		public PguiAECtrl animAE
		{
			get
			{
				return this._animAE;
			}
		}

		public PguiButtonCtrl btnClose
		{
			get
			{
				return this._btnClose;
			}
		}

		public PguiButtonCtrl btnLeft
		{
			get
			{
				return this._btnLeft;
			}
		}

		public PguiButtonCtrl btnRight
		{
			get
			{
				return this._btnRight;
			}
		}

		public Text txtName
		{
			get
			{
				return this._txtName;
			}
		}

		public GameObject mrkHelper
		{
			get
			{
				return this._mrkHelper;
			}
		}

		public IconCharaCtrl icnChara
		{
			get
			{
				return this._icnChara;
			}
		}

		public List<Transform> infPhoto
		{
			get
			{
				return this._infPhoto;
			}
		}

		public List<Transform> icsPhoto
		{
			get
			{
				return this._icsPhoto;
			}
		}

		public List<IconPhotoCtrl> icnPhoto
		{
			get
			{
				return this._icnPhoto;
			}
		}

		public Transform staPhoto
		{
			get
			{
				return this._staPhoto;
			}
		}

		public PguiTabGroupCtrl tabInfo
		{
			get
			{
				return this._tabInfo;
			}
		}

		public GameObject infStatus
		{
			get
			{
				return this._infStatus;
			}
		}

		public Transform infStatLv
		{
			get
			{
				return this._infStatLv;
			}
		}

		public Transform infStatPrm
		{
			get
			{
				return this._infStatPrm;
			}
		}

		public GameObject infSkill
		{
			get
			{
				return this._infSkill;
			}
		}

		public GameObject infBuff
		{
			get
			{
				return this._infBuff;
			}
		}

		public Transform icnOrderCard
		{
			get
			{
				return this._icnOrderCard;
			}
		}

		public List<Transform> scrSkill
		{
			get
			{
				return this._scrSkill;
			}
		}

		public ReuseScroll scrBuff
		{
			get
			{
				return this._scrBuff;
			}
		}

		public PguiAECtrl closeAE
		{
			get
			{
				return this._closeAE;
			}
		}

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

		private SimpleAnimation _anim;

		private PguiAECtrl _animAE;

		private PguiButtonCtrl _btnClose;

		private PguiButtonCtrl _btnLeft;

		private PguiButtonCtrl _btnRight;

		private Text _txtName;

		private GameObject _mrkHelper;

		private IconCharaCtrl _icnChara;

		private List<Transform> _infPhoto;

		private List<Transform> _icsPhoto;

		private List<IconPhotoCtrl> _icnPhoto;

		private Transform _staPhoto;

		private PguiTabGroupCtrl _tabInfo;

		private GameObject _infStatus;

		private Transform _infStatLv;

		private Transform _infStatPrm;

		private GameObject _infSkill;

		private GameObject _infBuff;

		private Transform _icnOrderCard;

		private List<Transform> _scrSkill;

		private ReuseScroll _scrBuff;

		private PguiAECtrl _closeAE;
	}
}
