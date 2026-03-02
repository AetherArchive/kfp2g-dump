using System;
using System.Collections;
using System.Collections.Generic;
using AEAuth3;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000155 RID: 341
public class CommunicationCtrl
{
	// Token: 0x1700037C RID: 892
	// (get) Token: 0x0600138F RID: 5007 RVA: 0x000F1CB4 File Offset: 0x000EFEB4
	// (set) Token: 0x06001390 RID: 5008 RVA: 0x000F1CBC File Offset: 0x000EFEBC
	public CommunicationCtrl.CharaViewGuiData CharaViewGUI { get; private set; }

	// Token: 0x1700037D RID: 893
	// (get) Token: 0x06001391 RID: 5009 RVA: 0x000F1CC5 File Offset: 0x000EFEC5
	// (set) Token: 0x06001392 RID: 5010 RVA: 0x000F1CCD File Offset: 0x000EFECD
	public Dictionary<int, CommunicationCtrl.ScrollBarIconGuiData> BarIconMap { get; private set; }

	// Token: 0x1700037E RID: 894
	// (get) Token: 0x06001393 RID: 5011 RVA: 0x000F1CD6 File Offset: 0x000EFED6
	// (set) Token: 0x06001394 RID: 5012 RVA: 0x000F1CDE File Offset: 0x000EFEDE
	public List<CharaPackData> CharaPackList { get; private set; }

	// Token: 0x1700037F RID: 895
	// (get) Token: 0x06001395 RID: 5013 RVA: 0x000F1CE7 File Offset: 0x000EFEE7
	// (set) Token: 0x06001396 RID: 5014 RVA: 0x000F1CEF File Offset: 0x000EFEEF
	public List<CharaPackData> DispPackList { get; private set; }

	// Token: 0x17000380 RID: 896
	// (get) Token: 0x06001397 RID: 5015 RVA: 0x000F1CF8 File Offset: 0x000EFEF8
	// (set) Token: 0x06001398 RID: 5016 RVA: 0x000F1D00 File Offset: 0x000EFF00
	public ReuseScroll CharaSelectScroll { get; set; }

	// Token: 0x17000381 RID: 897
	// (get) Token: 0x06001399 RID: 5017 RVA: 0x000F1D09 File Offset: 0x000EFF09
	// (set) Token: 0x0600139A RID: 5018 RVA: 0x000F1D11 File Offset: 0x000EFF11
	public PguiButtonCtrl FilterButton { get; set; }

	// Token: 0x17000382 RID: 898
	// (get) Token: 0x0600139B RID: 5019 RVA: 0x000F1D1A File Offset: 0x000EFF1A
	// (set) Token: 0x0600139C RID: 5020 RVA: 0x000F1D22 File Offset: 0x000EFF22
	public PguiButtonCtrl SortButton { get; set; }

	// Token: 0x17000383 RID: 899
	// (get) Token: 0x0600139D RID: 5021 RVA: 0x000F1D2B File Offset: 0x000EFF2B
	// (set) Token: 0x0600139E RID: 5022 RVA: 0x000F1D33 File Offset: 0x000EFF33
	public PguiButtonCtrl SortUpDownButton { get; set; }

	// Token: 0x0600139F RID: 5023 RVA: 0x000F1D3C File Offset: 0x000EFF3C
	public CommunicationCtrl()
	{
		this.CharaPackList = new List<CharaPackData>();
		this.DispPackList = new List<CharaPackData>();
		this.BarIconMap = new Dictionary<int, CommunicationCtrl.ScrollBarIconGuiData>();
	}

	// Token: 0x060013A0 RID: 5024 RVA: 0x000F1D6C File Offset: 0x000EFF6C
	public void CharaSelectInitialize(List<CharaPackData> charaPackList)
	{
		this.CharaPackList = new List<CharaPackData>(charaPackList);
		this.DispPackList = new List<CharaPackData>(charaPackList);
		int num = this.CharaPackList.Count / 3 + ((this.CharaPackList.Count % 3 == 0) ? 0 : 1);
		this.CharaSelectScroll.Resize(num, 0);
		SortWindowCtrl.RegisterData registerData = new SortWindowCtrl.RegisterData
		{
			register = SortFilterDefine.RegisterType.CHARA_COMMUNICATION,
			filterButton = this.FilterButton,
			sortButton = this.SortButton,
			sortUdButton = this.SortUpDownButton,
			funcGetTargetBaseList = () => new SortWindowCtrl.SortTarget
			{
				charaList = this.CharaPackList
			},
			funcDisideTarget = delegate(SortWindowCtrl.SortTarget item)
			{
				this.DispPackList = item.charaList;
				this.SortType = item.sortType;
				this.CharaSelectScroll.Resize(this.CharaPackList.Count / 3 + ((this.CharaPackList.Count % 3 == 0) ? 0 : 1), 0);
			}
		};
		CanvasManager.HdlOpenWindowSortFilter.Register(registerData, true, null);
	}

	// Token: 0x060013A1 RID: 5025 RVA: 0x000F1E24 File Offset: 0x000F0024
	public void CharaViewInitialize(GameObject go)
	{
		this.CharaViewGUI = new CommunicationCtrl.CharaViewGuiData(go);
		this.CharaViewGUI.MemoriesMenuScroll.InitForce();
		ReuseScroll memoriesMenuScroll = this.CharaViewGUI.MemoriesMenuScroll;
		memoriesMenuScroll.onStartItem = (Action<int, GameObject>)Delegate.Combine(memoriesMenuScroll.onStartItem, new Action<int, GameObject>(this.SetupCharaViewScroll));
		ReuseScroll memoriesMenuScroll2 = this.CharaViewGUI.MemoriesMenuScroll;
		memoriesMenuScroll2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(memoriesMenuScroll2.onUpdateItem, new Action<int, GameObject>(this.UpdateCharaViewScroll));
		this.CharaViewGUI.MemoriesMenuScroll.Setup(0, 0);
	}

	// Token: 0x060013A2 RID: 5026 RVA: 0x000F1EB8 File Offset: 0x000F00B8
	public void SelectChara(int chId)
	{
		CharaPackData charaPackData = this.CharaPackList.Find((CharaPackData x) => x.id == chId);
		this.PlayAnimation = null;
		this.CharaViewGUI.CharaSetup(charaPackData);
		this.CharaViewMenuList = new List<CommunicationCtrl.CharaViewMenu>
		{
			new CommunicationCtrl.CharaViewMenu("加入あいさつ", new UnityAction(this.ActionFriendsJoin), true, "新規加入"),
			new CommunicationCtrl.CharaViewMenu("なかよしレベルアップ", new UnityAction(this.ActionKizunaLvUp), 1 < charaPackData.dynamicData.kizunaLevel, "なかよしLv.2以上"),
			new CommunicationCtrl.CharaViewMenu("レベルアップ", new UnityAction(this.ActionLvUp), 1 < charaPackData.dynamicData.level, "Lv.2以上"),
			new CommunicationCtrl.CharaViewMenu("レベル上限解放", new UnityAction(this.ActionLvLimitUp), 0 < charaPackData.dynamicData.levelLimitId, "Lv.上限解放1回以上"),
			new CommunicationCtrl.CharaViewMenu("野生解放", new UnityAction(this.ActionPromoteUp), 0 < charaPackData.dynamicData.promoteNum, "野生解放2以上"),
			new CommunicationCtrl.CharaViewMenu("けも級アップ", new UnityAction(this.ActionRankUp), charaPackData.staticData.baseData.rankLow < charaPackData.dynamicData.rank, "けも級☆" + (charaPackData.staticData.baseData.rankLow + 1).ToString() + "以上"),
			new CommunicationCtrl.CharaViewMenu("けものミラクルアップ", new UnityAction(this.ActionMiracleUp), 1 < charaPackData.dynamicData.artsLevel, "けものミラクルLv.2以上"),
			new CommunicationCtrl.CharaViewMenu("フォトポケランクアップ", new UnityAction(this.ActionPhotoPcketUp), 1 < charaPackData.dynamicData.PhotoFrameTotalStep, "フォトポケランク2以上"),
			new CommunicationCtrl.CharaViewMenu("なかよしレベル上限解放", new UnityAction(this.ActionKizunaLvLimitUp), 0 < charaPackData.dynamicData.kizunaLimitOverNum, "なかよしLv.上限解放1回以上"),
			new CommunicationCtrl.CharaViewMenu("なないろとくせい解放", new UnityAction(this.ActionNanairoRelease), charaPackData.dynamicData.nanairoAbilityReleaseFlag, "なないろとくせい解放")
		};
		this.CharaViewGUI.MemoriesMenuScroll.Resize(this.CharaViewMenuList.Count, 0);
	}

	// Token: 0x060013A3 RID: 5027 RVA: 0x000F211D File Offset: 0x000F031D
	private void SetupCharaViewScroll(int index, GameObject go)
	{
	}

	// Token: 0x060013A4 RID: 5028 RVA: 0x000F2120 File Offset: 0x000F0320
	private void UpdateCharaViewScroll(int index, GameObject go)
	{
		CommunicationCtrl.CommuMenuBarGui commuMenuBarGui = new CommunicationCtrl.CommuMenuBarGui(go);
		commuMenuBarGui.AE_MArkLock.gameObject.SetActive(this.CharaViewMenuList[index].IsLock);
		commuMenuBarGui.BarButton.SetActEnable(!this.CharaViewMenuList[index].IsLock, false, false);
		commuMenuBarGui.BarButton.AddOnClickListener(delegate(PguiButtonCtrl x)
		{
			this.CharaViewMenuList[index].Action();
		}, PguiButtonCtrl.SoundType.DEFAULT);
		commuMenuBarGui.MenuName.text = this.CharaViewMenuList[index].MenuName;
		commuMenuBarGui.LockText.text = this.CharaViewMenuList[index].LockText;
	}

	// Token: 0x060013A5 RID: 5029 RVA: 0x000F21EC File Offset: 0x000F03EC
	private void ActionFriendsJoin()
	{
		if (this.CharaViewGUI.IsPlayingMotion)
		{
			return;
		}
		this.PlayAnimation = this.CharaViewGUI.FriendsJoin();
	}

	// Token: 0x060013A6 RID: 5030 RVA: 0x000F220D File Offset: 0x000F040D
	private void ActionKizunaLvUp()
	{
		if (this.CharaViewGUI.IsPlayingMotion)
		{
			return;
		}
		this.PlayAnimation = this.CharaViewGUI.KizunaLvUp();
	}

	// Token: 0x060013A7 RID: 5031 RVA: 0x000F222E File Offset: 0x000F042E
	private void ActionLvUp()
	{
		if (this.CharaViewGUI.IsPlayingMotion)
		{
			return;
		}
		this.PlayAnimation = this.CharaViewGUI.LevelUp();
	}

	// Token: 0x060013A8 RID: 5032 RVA: 0x000F224F File Offset: 0x000F044F
	private void ActionLvLimitUp()
	{
		if (this.CharaViewGUI.IsPlayingMotion)
		{
			return;
		}
		this.PlayAnimation = this.CharaViewGUI.LevelLimitUp();
	}

	// Token: 0x060013A9 RID: 5033 RVA: 0x000F2270 File Offset: 0x000F0470
	private void ActionNanairoRelease()
	{
		if (this.CharaViewGUI.IsPlayingMotion)
		{
			return;
		}
		this.PlayAnimation = this.CharaViewGUI.NanairoRelease();
	}

	// Token: 0x060013AA RID: 5034 RVA: 0x000F2291 File Offset: 0x000F0491
	private void ActionPromoteUp()
	{
		if (this.CharaViewGUI.IsPlayingMotion)
		{
			return;
		}
		this.PlayAnimation = this.CharaViewGUI.WildRelease();
	}

	// Token: 0x060013AB RID: 5035 RVA: 0x000F22B2 File Offset: 0x000F04B2
	private void ActionRankUp()
	{
		if (this.CharaViewGUI.IsPlayingMotion)
		{
			return;
		}
		this.PlayAnimation = this.CharaViewGUI.KemoRankUp();
	}

	// Token: 0x060013AC RID: 5036 RVA: 0x000F22D3 File Offset: 0x000F04D3
	private void ActionMiracleUp()
	{
		if (this.CharaViewGUI.IsPlayingMotion)
		{
			return;
		}
		this.PlayAnimation = this.CharaViewGUI.MiracleUp();
	}

	// Token: 0x060013AD RID: 5037 RVA: 0x000F22F4 File Offset: 0x000F04F4
	private void ActionPhotoPcketUp()
	{
		if (this.CharaViewGUI.IsPlayingMotion)
		{
			return;
		}
		this.PlayAnimation = this.CharaViewGUI.PhotoPcketUp();
	}

	// Token: 0x060013AE RID: 5038 RVA: 0x000F2315 File Offset: 0x000F0515
	private void ActionKizunaLvLimitUp()
	{
		if (this.CharaViewGUI.IsPlayingMotion)
		{
			return;
		}
		this.PlayAnimation = this.CharaViewGUI.KizunaLevelLimitUp();
	}

	// Token: 0x060013AF RID: 5039 RVA: 0x000F2336 File Offset: 0x000F0536
	public void CommunicationAnimationUpdate()
	{
		if (this.PlayAnimation != null && !this.PlayAnimation.MoveNext())
		{
			this.PlayAnimation = null;
		}
	}

	// Token: 0x0400104D RID: 4173
	public SortFilterDefine.SortType SortType = SortFilterDefine.SortType.LEVEL;

	// Token: 0x0400104E RID: 4174
	public IEnumerator PlayAnimation;

	// Token: 0x0400104F RID: 4175
	public List<CommunicationCtrl.CharaViewMenu> CharaViewMenuList;

	// Token: 0x02000B3C RID: 2876
	public class ScrollBarIconGuiData
	{
		// Token: 0x170009BC RID: 2492
		// (get) Token: 0x0600421F RID: 16927 RVA: 0x001FEBB3 File Offset: 0x001FCDB3
		// (set) Token: 0x06004220 RID: 16928 RVA: 0x001FEBBB File Offset: 0x001FCDBB
		public List<CommunicationCtrl.ScrollBarIconGuiData.IconOne> BarObjList { get; set; }

		// Token: 0x06004221 RID: 16929 RVA: 0x001FEBC4 File Offset: 0x001FCDC4
		public ScrollBarIconGuiData(GameObject go)
		{
			this.baseObj = go;
		}

		// Token: 0x0400468D RID: 18061
		public GameObject baseObj;

		// Token: 0x0200117A RID: 4474
		public class IconOne
		{
			// Token: 0x17000CE4 RID: 3300
			// (get) Token: 0x06005640 RID: 22080 RVA: 0x00251371 File Offset: 0x0024F571
			// (set) Token: 0x06005641 RID: 22081 RVA: 0x00251379 File Offset: 0x0024F579
			public IconCharaCtrl Icon { get; private set; }

			// Token: 0x17000CE5 RID: 3301
			// (get) Token: 0x06005642 RID: 22082 RVA: 0x00251382 File Offset: 0x0024F582
			// (set) Token: 0x06005643 RID: 22083 RVA: 0x0025138A File Offset: 0x0024F58A
			public PguiButtonCtrl Button { get; private set; }

			// Token: 0x17000CE6 RID: 3302
			// (get) Token: 0x06005644 RID: 22084 RVA: 0x00251393 File Offset: 0x0024F593
			// (set) Token: 0x06005645 RID: 22085 RVA: 0x0025139B File Offset: 0x0024F59B
			public int CharaId { get; set; }

			// Token: 0x06005646 RID: 22086 RVA: 0x002513A4 File Offset: 0x0024F5A4
			public IconOne(GameObject iconObj)
			{
				this.Icon = iconObj.GetComponent<IconCharaCtrl>();
				this.Button = iconObj.GetComponent<PguiButtonCtrl>();
			}
		}
	}

	// Token: 0x02000B3D RID: 2877
	public class CharaViewGuiData
	{
		// Token: 0x170009BD RID: 2493
		// (get) Token: 0x06004222 RID: 16930 RVA: 0x001FEBD3 File Offset: 0x001FCDD3
		// (set) Token: 0x06004223 RID: 16931 RVA: 0x001FEBDB File Offset: 0x001FCDDB
		public bool IsPlayingMotion { get; set; }

		// Token: 0x06004224 RID: 16932 RVA: 0x001FEBE4 File Offset: 0x001FCDE4
		public CharaViewGuiData(GameObject go)
		{
			this.baseObj = go;
			this.AE_LevelUp = this.baseObj.transform.Find("All/Left/AEImage_LevelUP").GetComponent<PguiAECtrl>();
			this.AE_LevelUp.gameObject.SetActive(false);
			this.AE_KemonoMiracle = this.baseObj.transform.Find("All/Left/AEImage_KemonoMiracle").GetComponent<PguiAECtrl>();
			this.AE_KemonoMiracle.gameObject.SetActive(false);
			this.AEImage_LevelLimit = this.baseObj.transform.Find("All/Left/AEImage_LevelLimit").GetComponent<AEImage>();
			this.AEImage_LevelLimit.gameObject.SetActive(false);
			this.AEImage_YaseiResult = this.baseObj.transform.Find("All/Left/AEImage_YaseiResult").GetComponent<AEImage>();
			this.AEImage_YaseiResult.gameObject.SetActive(false);
			this.AEImage_KizunaLevelLimit = this.baseObj.transform.Find("All/Left/AEImage_HeartLimit").GetComponent<AEImage>();
			this.AEImage_KizunaLevelLimit.gameObject.SetActive(false);
			this.AEImage_NanairoResult = this.baseObj.transform.Find("All/Left/AEImage_result").GetComponent<AEImage>();
			this.AEImage_NanairoResult.gameObject.SetActive(false);
			this.AE_RankUpBg = this.baseObj.transform.Find("CharaRankUpAuth/AEImage_Bg").GetComponent<PguiAECtrl>();
			this.AE_RankUpBg.gameObject.SetActive(false);
			this.AE_RankUpBack = this.baseObj.transform.Find("CharaRankUpAuth/AEImage_Back").GetComponent<PguiAECtrl>();
			this.AE_RankUpBack.gameObject.SetActive(false);
			this.AE_RankUp = this.baseObj.transform.Find("CharaRankUpAuth/AEImage_RankUp").GetComponent<PguiAECtrl>();
			this.AE_RankUp.gameObject.SetActive(false);
			this.CharaRankUpAuth = new SelCharaGrowRank.RankUpAuth(this.baseObj.transform.Find("CharaRankUpAuth"));
			this.CharaRankUpAuth.baseObj.SetActive(false);
			this.NameList = new List<CharaWindowCtrl.GUI.Name>
			{
				new CharaWindowCtrl.GUI.Name(this.baseObj.transform.Find("All/Left/Name")),
				new CharaWindowCtrl.GUI.Name(this.baseObj.transform.Find("All/Left/Name_WName"))
			};
			foreach (CharaWindowCtrl.GUI.Name name in this.NameList)
			{
				name.baseObj.SetActive(false);
			}
			this.AuthHeartLvUp = this.baseObj.transform.Find("Auth_HeartLvUp").gameObject;
			this.AuthHeartLvUp.SetActive(false);
			GameObject gameObject = this.baseObj.transform.Find("All/Left/RenderChara").gameObject;
			this.RenderChara = Object.Instantiate<GameObject>((GameObject)Resources.Load("RenderTextureChara/Prefab/RenderTextureCharaCtrl"), gameObject.transform).GetComponent<RenderTextureChara>();
			this.RenderChara.postion = new Vector2(0f, 0f);
			this.RenderChara.fieldOfView = 30f;
			this.MemoriesMenuScroll = this.baseObj.transform.Find("All/WindowAll/ScrollView").GetComponent<ReuseScroll>();
		}

		// Token: 0x06004225 RID: 16933 RVA: 0x001FEF34 File Offset: 0x001FD134
		public void CharaSetup(CharaPackData chPackData)
		{
			this.currentCharaPackData = chPackData;
			foreach (CharaWindowCtrl.GUI.Name name in this.NameList)
			{
				name.baseObj.SetActive(false);
			}
			if (string.IsNullOrEmpty(chPackData.staticData.baseData.NickName))
			{
				this.NameList[0].Setup(chPackData, false);
			}
			else
			{
				this.NameList[1].Setup(chPackData, false);
			}
			this.IsPlayingMotion = false;
			this.RenderChara.Setup(this.currentCharaPackData, 1, CharaMotionDefine.ActKey.SCENARIO_STAND_BY, false, delegate
			{
				this.RenderChara.SetAnimation(CharaMotionDefine.ActKey.SCENARIO_STAND_BY, true);
			}, false, null, 0f, null, false);
		}

		// Token: 0x06004226 RID: 16934 RVA: 0x001FF004 File Offset: 0x001FD204
		public IEnumerator FriendsJoin()
		{
			if (this.IsPlayingMotion)
			{
				yield break;
			}
			this.IsPlayingMotion = true;
			CanvasManager.SetEnableCmnTouchMask(true);
			GachaAuthCtrl playGachaAuth = new GachaAuthCtrl();
			playGachaAuth.Initialize();
			yield return null;
			SoundManager.SetTempVolume(0.2f);
			IEnumerator func = playGachaAuth.PlayGreetingForOtherScene(new List<GachaAuthCtrl.AuthItem>
			{
				new GachaAuthCtrl.AuthItem
				{
					isNew = true,
					itemId = this.currentCharaPackData.id
				}
			}, false);
			while (func.MoveNext())
			{
				yield return null;
			}
			playGachaAuth.DestroyAllObject();
			playGachaAuth = null;
			SoundManager.ReturnOrgVolume();
			this.IsPlayingMotion = false;
			CanvasManager.SetEnableCmnTouchMask(false);
			yield break;
		}

		// Token: 0x06004227 RID: 16935 RVA: 0x001FF013 File Offset: 0x001FD213
		public IEnumerator KizunaLvUp()
		{
			CommunicationCtrl.CharaViewGuiData.<>c__DisplayClass23_0 CS$<>8__locals1 = new CommunicationCtrl.CharaViewGuiData.<>c__DisplayClass23_0();
			if (this.IsPlayingMotion)
			{
				yield break;
			}
			this.IsPlayingMotion = true;
			CanvasManager.SetEnableCmnTouchMask(true);
			CanvasManager.AddCallbackCmnTouchMask(new UnityAction<Transform>(this.OnTouchMask));
			this.touchScreenAuth = false;
			GameObject kizunaWindow = this.AuthHeartLvUp;
			CS$<>8__locals1.kizunaWinWhite = kizunaWindow.transform.Find("AEImage_White").GetComponent<PguiAECtrl>();
			CS$<>8__locals1.kizunaWinBack = kizunaWindow.transform.Find("AEImage_Back").GetComponent<PguiAECtrl>();
			CS$<>8__locals1.kizunaWinFront = kizunaWindow.transform.Find("AEImage_Front").GetComponent<PguiAECtrl>();
			CS$<>8__locals1.kizunaWinInfo = kizunaWindow.transform.Find("AEImage_Info").GetComponent<PguiAECtrl>();
			kizunaWindow.SetActive(false);
			CS$<>8__locals1.kizunaWinChara = AssetManager.InstantiateAssetData("RenderTextureChara/Prefab/RenderTextureCharaCtrl", kizunaWindow.transform.Find("RenderTexture")).GetComponent<RenderTextureChara>();
			CS$<>8__locals1.kizunaWinChara.SetupRenderTexture(1654, 1024);
			CS$<>8__locals1.kizunaWinChara.gameObject.SetActive(false);
			yield return null;
			IEnumerator seLoad = SoundManager.LoadCueSheetWithDownload("se_cb");
			do
			{
				yield return null;
			}
			while (seLoad.MoveNext() && this.baseObj.activeSelf);
			seLoad = null;
			SoundManager.SetTempVolume(0.2f);
			SoundManager.Play("prd_se_result_bond_levelup_window", false, false);
			CanvasManager.HdlCmnMenu.SetActiveMenu(false);
			CS$<>8__locals1.kizunaWinInfo.transform.Find("Serif_Info03/Txt").GetComponent<PguiTextCtrl>().text = this.currentCharaPackData.staticData.baseData.kizunaupText;
			CS$<>8__locals1.kizunaWinInfo.transform.Find("Item_Info04/Txt").GetComponent<PguiTextCtrl>().text = "";
			kizunaWindow.SetActive(true);
			CS$<>8__locals1.kizunaWinWhite.PlayAnimation(PguiAECtrl.AmimeType.START, null);
			CS$<>8__locals1.kizunaWinBack.PlayAnimation(PguiAECtrl.AmimeType.START, null);
			CS$<>8__locals1.kizunaWinFront.gameObject.SetActive(false);
			CS$<>8__locals1.kizunaWinInfo.gameObject.SetActive(false);
			CS$<>8__locals1.kizunaWinChara.Setup(0, 0, CharaMotionDefine.ActKey.INVALID, 0, false, true, null, false, null, 0f, null, false, false, false);
			this.touchScreenAuth = false;
			CS$<>8__locals1.kizunaWinCharaVoice = false;
			float kizunaWinTime = 0f;
			CS$<>8__locals1.kizunaWinChrY = 0f;
			while (this.baseObj.activeSelf)
			{
				if (this.touchScreenAuth)
				{
					CS$<>8__locals1.<KizunaLvUp>g__skip|0();
					this.touchScreenAuth = false;
				}
				if (CS$<>8__locals1.kizunaWinWhite.GetAnimeType() == PguiAECtrl.AmimeType.END && (!CS$<>8__locals1.kizunaWinWhite.IsPlaying() || (CS$<>8__locals1.kizunaWinChara.gameObject.activeSelf && !CS$<>8__locals1.kizunaWinBack.IsPlaying())))
				{
					break;
				}
				if (CS$<>8__locals1.kizunaWinWhite.GetAnimeType() == PguiAECtrl.AmimeType.START && !CS$<>8__locals1.kizunaWinWhite.IsPlaying())
				{
					CS$<>8__locals1.kizunaWinWhite.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
				}
				if (CS$<>8__locals1.kizunaWinBack.GetAnimeType() == PguiAECtrl.AmimeType.START)
				{
					if (!CS$<>8__locals1.kizunaWinBack.IsPlaying())
					{
						CS$<>8__locals1.kizunaWinBack.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
						CS$<>8__locals1.kizunaWinCharaVoice = false;
						CS$<>8__locals1.kizunaWinChara.gameObject.SetActive(true);
						CS$<>8__locals1.kizunaWinChara.Setup(this.currentCharaPackData.id, 0, CharaMotionDefine.ActKey.GACHA_ST, this.currentCharaPackData.equipClothImageId, this.currentCharaPackData.equipClothImageId > 0 && this.currentCharaPackData.equipLongSkirt, false, delegate
						{
							CS$<>8__locals1.kizunaWinChrY = CS$<>8__locals1.kizunaWinChara.GetNodePos("j_neck").y + 0.005f;
							CS$<>8__locals1.kizunaWinChara.SetAnimation(CharaMotionDefine.ActKey.GACHA_LP, true);
							if (!CS$<>8__locals1.kizunaWinInfo.gameObject.activeSelf)
							{
								base.<KizunaLvUp>g__skip|0();
							}
						}, false, null, 1.8333334f, delegate
						{
							CS$<>8__locals1.kizunaWinCharaVoice = true;
						}, false, false, false);
						CS$<>8__locals1.kizunaWinChara.SetCameraPosition(new Vector3(0f, 1.07f, 5.4f));
						kizunaWinTime = 0f;
						CS$<>8__locals1.kizunaWinChrY = 1.225f;
					}
				}
				else if (CS$<>8__locals1.kizunaWinInfo.gameObject.activeSelf)
				{
					if (CS$<>8__locals1.kizunaWinChara.IsCurrentAnimation(CharaMotionDefine.ActKey.GACHA_ST))
					{
						CS$<>8__locals1.kizunaWinChrY = CS$<>8__locals1.kizunaWinChara.GetNodePos("j_neck").y + 0.005f;
					}
					kizunaWinTime = Mathf.Clamp01(kizunaWinTime + TimeManager.DeltaTime * 3f);
					CS$<>8__locals1.kizunaWinChara.SetCameraPosition(Vector3.Lerp(new Vector3(0f, 1.07f, 5.4f), new Vector3(0f, CS$<>8__locals1.kizunaWinChrY, 3.7f), kizunaWinTime));
					if (CS$<>8__locals1.kizunaWinCharaVoice)
					{
						CS$<>8__locals1.kizunaWinChara.PlayVoice(VOICE_TYPE.KUP01);
						CS$<>8__locals1.kizunaWinCharaVoice = false;
					}
				}
				else
				{
					float num = CS$<>8__locals1.kizunaWinChara.AnimationLength();
					if (num > 0f)
					{
						float num2 = CS$<>8__locals1.kizunaWinChara.AnimationTime();
						if ((1f - num2) * num < 1f)
						{
							CS$<>8__locals1.<KizunaLvUp>g__skip|0();
						}
					}
				}
				yield return null;
			}
			CanvasManager.HdlCmnMenu.SetActiveMenu(true);
			kizunaWindow.SetActive(false);
			SoundManager.ReturnOrgVolume();
			if (CS$<>8__locals1.kizunaWinChara != null)
			{
				Object.Destroy(CS$<>8__locals1.kizunaWinChara.gameObject);
			}
			CS$<>8__locals1.kizunaWinChara = null;
			kizunaWindow = null;
			CS$<>8__locals1.kizunaWinWhite = null;
			CS$<>8__locals1.kizunaWinBack = null;
			CS$<>8__locals1.kizunaWinFront = null;
			CS$<>8__locals1.kizunaWinInfo = null;
			this.IsPlayingMotion = false;
			CanvasManager.RemoveCallbackCmnTouchMask();
			CanvasManager.SetEnableCmnTouchMask(false);
			yield break;
		}

		// Token: 0x06004228 RID: 16936 RVA: 0x001FF022 File Offset: 0x001FD222
		public IEnumerator LevelUp()
		{
			if (this.IsPlayingMotion)
			{
				yield break;
			}
			this.IsPlayingMotion = true;
			CanvasManager.SetEnableCmnTouchMask(true);
			while (!this.RenderChara.FinishedSetup && this.baseObj.activeSelf)
			{
				yield return null;
			}
			bool isAnim = true;
			this.RenderChara.SetAnimation(CharaMotionDefine.ActKey.POSITIVE, false, delegate
			{
				this.RenderChara.SetAnimation(CharaMotionDefine.ActKey.SCENARIO_STAND_BY, true);
				isAnim = false;
			});
			this.RenderChara.PlayVoice(VOICE_TYPE.LUP01);
			while (isAnim && this.baseObj.activeSelf)
			{
				yield return null;
			}
			this.IsPlayingMotion = false;
			CanvasManager.SetEnableCmnTouchMask(false);
			yield break;
		}

		// Token: 0x06004229 RID: 16937 RVA: 0x001FF031 File Offset: 0x001FD231
		public IEnumerator LevelLimitUp()
		{
			if (this.IsPlayingMotion)
			{
				yield break;
			}
			this.IsPlayingMotion = true;
			CanvasManager.SetEnableCmnTouchMask(true);
			this.AEImage_LevelLimit.playTime = 0f;
			this.AEImage_LevelLimit.autoPlay = true;
			this.AEImage_LevelLimit.playLoop = false;
			this.AEImage_LevelLimit.gameObject.SetActive(true);
			while (!this.RenderChara.FinishedSetup && this.baseObj.activeSelf)
			{
				yield return null;
			}
			bool isAnim = true;
			this.RenderChara.SetAnimation(CharaMotionDefine.ActKey.JOY, false, delegate
			{
				this.RenderChara.SetAnimation(CharaMotionDefine.ActKey.SCENARIO_STAND_BY, true);
				isAnim = false;
			});
			SoundManager.Play("prd_se_friends_level_expansion", false, false);
			this.RenderChara.PlayVoice(VOICE_TYPE.JOY01);
			while (isAnim)
			{
				if (!this.baseObj.activeSelf)
				{
					break;
				}
				yield return null;
			}
			while (!this.AEImage_LevelLimit.end && this.baseObj.activeSelf)
			{
				yield return null;
			}
			this.AEImage_LevelLimit.gameObject.SetActive(false);
			this.IsPlayingMotion = false;
			CanvasManager.SetEnableCmnTouchMask(false);
			yield break;
		}

		// Token: 0x0600422A RID: 16938 RVA: 0x001FF040 File Offset: 0x001FD240
		public IEnumerator NanairoRelease()
		{
			if (this.IsPlayingMotion)
			{
				yield break;
			}
			this.IsPlayingMotion = true;
			CanvasManager.SetEnableCmnTouchMask(true);
			this.AEImage_NanairoResult.playTime = 0f;
			this.AEImage_NanairoResult.autoPlay = true;
			this.AEImage_NanairoResult.playLoop = false;
			this.AEImage_NanairoResult.gameObject.SetActive(true);
			while (!this.RenderChara.FinishedSetup && this.baseObj.activeSelf)
			{
				yield return null;
			}
			bool isAnim = true;
			this.RenderChara.SetAnimation(CharaMotionDefine.ActKey.POSITIVE, false, delegate
			{
				this.RenderChara.SetAnimation(CharaMotionDefine.ActKey.SCENARIO_STAND_BY, true);
				isAnim = false;
			});
			SoundManager.Play("prd_se_friends_arts_levelup", false, false);
			this.RenderChara.PlayVoice(VOICE_TYPE.AUP01);
			while (isAnim)
			{
				if (!this.baseObj.activeSelf)
				{
					break;
				}
				yield return null;
			}
			while (!this.AEImage_NanairoResult.end && this.baseObj.activeSelf)
			{
				yield return null;
			}
			this.AEImage_NanairoResult.gameObject.SetActive(false);
			this.IsPlayingMotion = false;
			CanvasManager.SetEnableCmnTouchMask(false);
			yield break;
		}

		// Token: 0x0600422B RID: 16939 RVA: 0x001FF04F File Offset: 0x001FD24F
		public IEnumerator WildRelease()
		{
			if (this.IsPlayingMotion)
			{
				yield break;
			}
			this.IsPlayingMotion = true;
			CanvasManager.SetEnableCmnTouchMask(true);
			this.AEImage_YaseiResult.playTime = 0f;
			this.AEImage_YaseiResult.autoPlay = true;
			this.AEImage_YaseiResult.playLoop = false;
			this.AEImage_YaseiResult.gameObject.SetActive(true);
			while (!this.RenderChara.FinishedSetup && this.baseObj.activeSelf)
			{
				yield return null;
			}
			bool isAnim = true;
			this.RenderChara.SetAnimation(CharaMotionDefine.ActKey.POSITIVE, false, delegate
			{
				this.RenderChara.SetAnimation(CharaMotionDefine.ActKey.SCENARIO_STAND_BY, true);
				isAnim = false;
			});
			SoundManager.Play("prd_se_friends_liberation_auth", false, false);
			this.RenderChara.PlayVoice(VOICE_TYPE.PMT01);
			while (!this.AEImage_YaseiResult.end && this.baseObj.activeSelf)
			{
				yield return null;
			}
			this.AEImage_YaseiResult.gameObject.SetActive(false);
			while (isAnim && this.baseObj.activeSelf)
			{
				yield return null;
			}
			this.IsPlayingMotion = false;
			CanvasManager.SetEnableCmnTouchMask(false);
			yield break;
		}

		// Token: 0x0600422C RID: 16940 RVA: 0x001FF05E File Offset: 0x001FD25E
		public IEnumerator KemoRankUp()
		{
			if (this.IsPlayingMotion)
			{
				yield break;
			}
			this.IsPlayingMotion = true;
			CanvasManager.SetEnableCmnTouchMask(true);
			Vector3 charaScale = this.RenderChara.GetCharaScale();
			SelCharaGrowRank.RankUpAuth.SIZE size = SelCharaGrowRank.RankUpAuth.SIZE.SIZE_S;
			if (Math.Abs(charaScale.y - 1f) <= 1E-45f)
			{
				size = SelCharaGrowRank.RankUpAuth.SIZE.SIZE_M;
			}
			else if (charaScale.y < 1f)
			{
				size = SelCharaGrowRank.RankUpAuth.SIZE.SIZE_S;
			}
			else if (charaScale.y > 1f)
			{
				size = SelCharaGrowRank.RankUpAuth.SIZE.SIZE_L;
			}
			GameObject gameObject = Object.Instantiate<GameObject>(this.CharaRankUpAuth.baseObj);
			SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, gameObject.transform, true);
			SelCharaGrowRank.RankUpAuth RankUpAuth = new SelCharaGrowRank.RankUpAuth(gameObject.transform);
			RankUpAuth.baseObj.SetActive(true);
			RankUpAuth.Setup(this.currentCharaPackData.id, size, 0, 0);
			while (!RankUpAuth.rtc.FinishedSetup)
			{
				yield return null;
			}
			this.RenderChara.PlayVoice(VOICE_TYPE.RUP01);
			CanvasManager.AddCallbackCmnTouchMask(new UnityAction<Transform>(this.OnTouchMask));
			this.touchScreenAuth = false;
			while (!this.touchScreenAuth)
			{
				if (Input.GetKeyDown(KeyCode.Escape))
				{
					this.touchScreenAuth = true;
				}
				yield return null;
			}
			RankUpAuth.AEImage_Back.PlayAnimation(PguiAECtrl.AmimeType.END, delegate
			{
				RankUpAuth.AEImage_Back.gameObject.SetActive(false);
			});
			RankUpAuth.AEImage_RankUp.PlayAnimation(PguiAECtrl.AmimeType.END, delegate
			{
				RankUpAuth.AEImage_RankUp.gameObject.SetActive(false);
			});
			RankUpAuth.AEImage_Bg.PlayAnimation(PguiAECtrl.AmimeType.END, delegate
			{
				RankUpAuth.AEImage_Bg.gameObject.SetActive(false);
			});
			using (List<PguiAECtrl>.Enumerator enumerator = RankUpAuth.AEImage_StarAll.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PguiAECtrl pguiAECtrl = enumerator.Current;
					pguiAECtrl.PlayAnimation(PguiAECtrl.AmimeType.END, null);
				}
				goto IL_0256;
			}
			IL_023F:
			yield return null;
			IL_0256:
			if (!RankUpAuth.AEImage_Back.gameObject.activeSelf)
			{
				RankUpAuth.baseObj.SetActive(false);
				RankUpAuth.Teardown();
				Object.Destroy(RankUpAuth.baseObj);
				CanvasManager.RemoveCallbackCmnTouchMask();
				this.IsPlayingMotion = false;
				CanvasManager.SetEnableCmnTouchMask(false);
				yield break;
			}
			goto IL_023F;
		}

		// Token: 0x0600422D RID: 16941 RVA: 0x001FF06D File Offset: 0x001FD26D
		public IEnumerator MiracleUp()
		{
			if (this.IsPlayingMotion)
			{
				yield break;
			}
			this.IsPlayingMotion = true;
			CanvasManager.SetEnableCmnTouchMask(true);
			this.AE_KemonoMiracle.m_AEImage.playTime = 0f;
			this.AE_KemonoMiracle.m_AEImage.autoPlay = true;
			this.AE_KemonoMiracle.gameObject.SetActive(true);
			while (!this.RenderChara.FinishedSetup && this.baseObj.activeSelf)
			{
				yield return null;
			}
			bool isAnim = true;
			this.RenderChara.SetAnimation(CharaMotionDefine.ActKey.POSITIVE, false, delegate
			{
				this.RenderChara.SetAnimation(CharaMotionDefine.ActKey.SCENARIO_STAND_BY, true);
				isAnim = false;
			});
			SoundManager.Play("prd_se_friends_arts_levelup", false, false);
			this.RenderChara.PlayVoice(VOICE_TYPE.AUP01);
			while (!this.AE_KemonoMiracle.m_AEImage.end && this.baseObj.activeSelf)
			{
				yield return null;
			}
			this.AE_KemonoMiracle.gameObject.SetActive(false);
			while (isAnim && this.baseObj.activeSelf)
			{
				yield return null;
			}
			this.IsPlayingMotion = false;
			CanvasManager.SetEnableCmnTouchMask(false);
			yield break;
		}

		// Token: 0x0600422E RID: 16942 RVA: 0x001FF07C File Offset: 0x001FD27C
		public IEnumerator PhotoPcketUp()
		{
			if (this.IsPlayingMotion)
			{
				yield break;
			}
			this.IsPlayingMotion = true;
			CanvasManager.SetEnableCmnTouchMask(true);
			while (!this.RenderChara.FinishedSetup && this.baseObj.activeSelf)
			{
				yield return null;
			}
			bool isAnim = true;
			this.RenderChara.SetAnimation(CharaMotionDefine.ActKey.POSITIVE, false, delegate
			{
				this.RenderChara.SetAnimation(CharaMotionDefine.ActKey.SCENARIO_STAND_BY, true);
				isAnim = false;
			});
			SoundManager.Play("prd_se_friends_photo_pocket_add", false, false);
			this.RenderChara.PlayVoice(VOICE_TYPE.PHT01);
			while (isAnim && this.baseObj.activeSelf)
			{
				yield return null;
			}
			this.IsPlayingMotion = false;
			CanvasManager.SetEnableCmnTouchMask(false);
			yield break;
		}

		// Token: 0x0600422F RID: 16943 RVA: 0x001FF08B File Offset: 0x001FD28B
		public IEnumerator KizunaLevelLimitUp()
		{
			if (this.IsPlayingMotion)
			{
				yield break;
			}
			this.IsPlayingMotion = true;
			CanvasManager.SetEnableCmnTouchMask(true);
			this.AEImage_KizunaLevelLimit.playTime = 0f;
			this.AEImage_KizunaLevelLimit.autoPlay = true;
			this.AEImage_KizunaLevelLimit.playLoop = false;
			this.AEImage_KizunaLevelLimit.gameObject.SetActive(true);
			while (!this.RenderChara.FinishedSetup && this.baseObj.activeSelf)
			{
				yield return null;
			}
			bool isAnim = true;
			this.RenderChara.SetAnimation(CharaMotionDefine.ActKey.JOY, false, delegate
			{
				this.RenderChara.SetAnimation(CharaMotionDefine.ActKey.SCENARIO_STAND_BY, true);
				isAnim = false;
			});
			SoundManager.Play("prd_se_friends_bond_level_expansion", false, false);
			this.RenderChara.PlayVoice(VOICE_TYPE.JOY01);
			while (isAnim)
			{
				if (!this.baseObj.activeSelf)
				{
					break;
				}
				yield return null;
			}
			while (!this.AEImage_KizunaLevelLimit.end && this.baseObj.activeSelf)
			{
				yield return null;
			}
			this.AEImage_KizunaLevelLimit.gameObject.SetActive(false);
			this.IsPlayingMotion = false;
			CanvasManager.SetEnableCmnTouchMask(false);
			yield break;
		}

		// Token: 0x06004230 RID: 16944 RVA: 0x001FF09A File Offset: 0x001FD29A
		private void OnTouchMask(Transform tr)
		{
			this.touchScreenAuth = true;
		}

		// Token: 0x0400468F RID: 18063
		public GameObject baseObj;

		// Token: 0x04004690 RID: 18064
		public PguiAECtrl AE_LevelUp;

		// Token: 0x04004691 RID: 18065
		public AEImage AEImage_LevelLimit;

		// Token: 0x04004692 RID: 18066
		public PguiAECtrl AE_KemonoMiracle;

		// Token: 0x04004693 RID: 18067
		public AEImage AEImage_YaseiResult;

		// Token: 0x04004694 RID: 18068
		public AEImage AEImage_KizunaLevelLimit;

		// Token: 0x04004695 RID: 18069
		public AEImage AEImage_NanairoResult;

		// Token: 0x04004696 RID: 18070
		public PguiAECtrl AE_RankUpBg;

		// Token: 0x04004697 RID: 18071
		public PguiAECtrl AE_RankUpBack;

		// Token: 0x04004698 RID: 18072
		public PguiAECtrl AE_RankUp;

		// Token: 0x04004699 RID: 18073
		public SelCharaGrowRank.RankUpAuth CharaRankUpAuth;

		// Token: 0x0400469A RID: 18074
		public List<CharaWindowCtrl.GUI.Name> NameList;

		// Token: 0x0400469B RID: 18075
		private GameObject AuthHeartLvUp;

		// Token: 0x0400469C RID: 18076
		public RenderTextureChara RenderChara;

		// Token: 0x0400469E RID: 18078
		public ReuseScroll MemoriesMenuScroll;

		// Token: 0x0400469F RID: 18079
		public CharaPackData currentCharaPackData;

		// Token: 0x040046A0 RID: 18080
		private bool touchScreenAuth;
	}

	// Token: 0x02000B3E RID: 2878
	public class CommuMenuBarGui
	{
		// Token: 0x170009BE RID: 2494
		// (get) Token: 0x06004232 RID: 16946 RVA: 0x001FF0B3 File Offset: 0x001FD2B3
		// (set) Token: 0x06004233 RID: 16947 RVA: 0x001FF0BB File Offset: 0x001FD2BB
		public PguiButtonCtrl BarButton { get; private set; }

		// Token: 0x170009BF RID: 2495
		// (get) Token: 0x06004234 RID: 16948 RVA: 0x001FF0C4 File Offset: 0x001FD2C4
		// (set) Token: 0x06004235 RID: 16949 RVA: 0x001FF0CC File Offset: 0x001FD2CC
		public PguiTextCtrl MenuName { get; private set; }

		// Token: 0x170009C0 RID: 2496
		// (get) Token: 0x06004236 RID: 16950 RVA: 0x001FF0D5 File Offset: 0x001FD2D5
		// (set) Token: 0x06004237 RID: 16951 RVA: 0x001FF0DD File Offset: 0x001FD2DD
		public PguiAECtrl AE_MArkLock { get; private set; }

		// Token: 0x170009C1 RID: 2497
		// (get) Token: 0x06004238 RID: 16952 RVA: 0x001FF0E6 File Offset: 0x001FD2E6
		// (set) Token: 0x06004239 RID: 16953 RVA: 0x001FF0EE File Offset: 0x001FD2EE
		public PguiTextCtrl LockText { get; private set; }

		// Token: 0x0600423A RID: 16954 RVA: 0x001FF0F8 File Offset: 0x001FD2F8
		public CommuMenuBarGui(GameObject go)
		{
			this.BarButton = go.GetComponent<PguiButtonCtrl>();
			this.MenuName = go.transform.Find("BaseImage/Txt").GetComponent<PguiTextCtrl>();
			this.LockText = go.transform.Find("Mark_Lock/Txt_LockInfo").GetComponent<PguiTextCtrl>();
			this.AE_MArkLock = go.transform.Find("Mark_Lock").GetComponent<PguiAECtrl>();
		}
	}

	// Token: 0x02000B3F RID: 2879
	public class CharaViewMenu
	{
		// Token: 0x170009C2 RID: 2498
		// (get) Token: 0x0600423B RID: 16955 RVA: 0x001FF168 File Offset: 0x001FD368
		// (set) Token: 0x0600423C RID: 16956 RVA: 0x001FF170 File Offset: 0x001FD370
		public string MenuName { get; private set; }

		// Token: 0x170009C3 RID: 2499
		// (get) Token: 0x0600423D RID: 16957 RVA: 0x001FF179 File Offset: 0x001FD379
		// (set) Token: 0x0600423E RID: 16958 RVA: 0x001FF181 File Offset: 0x001FD381
		public UnityAction Action { get; private set; }

		// Token: 0x170009C4 RID: 2500
		// (get) Token: 0x0600423F RID: 16959 RVA: 0x001FF18A File Offset: 0x001FD38A
		// (set) Token: 0x06004240 RID: 16960 RVA: 0x001FF192 File Offset: 0x001FD392
		public bool IsLock { get; private set; }

		// Token: 0x170009C5 RID: 2501
		// (get) Token: 0x06004241 RID: 16961 RVA: 0x001FF19B File Offset: 0x001FD39B
		// (set) Token: 0x06004242 RID: 16962 RVA: 0x001FF1A3 File Offset: 0x001FD3A3
		public string LockText { get; private set; }

		// Token: 0x06004243 RID: 16963 RVA: 0x001FF1AC File Offset: 0x001FD3AC
		public CharaViewMenu(string name, UnityAction action, bool unLock, string lockText)
		{
			this.MenuName = name;
			this.Action = action;
			this.IsLock = !unLock;
			this.LockText = lockText;
		}
	}
}
