using System;
using System.Collections;
using System.Collections.Generic;
using AEAuth3;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;

public class CommunicationCtrl
{
	public CommunicationCtrl.CharaViewGuiData CharaViewGUI { get; private set; }

	public Dictionary<int, CommunicationCtrl.ScrollBarIconGuiData> BarIconMap { get; private set; }

	public List<CharaPackData> CharaPackList { get; private set; }

	public List<CharaPackData> DispPackList { get; private set; }

	public ReuseScroll CharaSelectScroll { get; set; }

	public PguiButtonCtrl FilterButton { get; set; }

	public PguiButtonCtrl SortButton { get; set; }

	public PguiButtonCtrl SortUpDownButton { get; set; }

	public CommunicationCtrl()
	{
		this.CharaPackList = new List<CharaPackData>();
		this.DispPackList = new List<CharaPackData>();
		this.BarIconMap = new Dictionary<int, CommunicationCtrl.ScrollBarIconGuiData>();
	}

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

	private void SetupCharaViewScroll(int index, GameObject go)
	{
	}

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

	private void ActionFriendsJoin()
	{
		if (this.CharaViewGUI.IsPlayingMotion)
		{
			return;
		}
		this.PlayAnimation = this.CharaViewGUI.FriendsJoin();
	}

	private void ActionKizunaLvUp()
	{
		if (this.CharaViewGUI.IsPlayingMotion)
		{
			return;
		}
		this.PlayAnimation = this.CharaViewGUI.KizunaLvUp();
	}

	private void ActionLvUp()
	{
		if (this.CharaViewGUI.IsPlayingMotion)
		{
			return;
		}
		this.PlayAnimation = this.CharaViewGUI.LevelUp();
	}

	private void ActionLvLimitUp()
	{
		if (this.CharaViewGUI.IsPlayingMotion)
		{
			return;
		}
		this.PlayAnimation = this.CharaViewGUI.LevelLimitUp();
	}

	private void ActionNanairoRelease()
	{
		if (this.CharaViewGUI.IsPlayingMotion)
		{
			return;
		}
		this.PlayAnimation = this.CharaViewGUI.NanairoRelease();
	}

	private void ActionPromoteUp()
	{
		if (this.CharaViewGUI.IsPlayingMotion)
		{
			return;
		}
		this.PlayAnimation = this.CharaViewGUI.WildRelease();
	}

	private void ActionRankUp()
	{
		if (this.CharaViewGUI.IsPlayingMotion)
		{
			return;
		}
		this.PlayAnimation = this.CharaViewGUI.KemoRankUp();
	}

	private void ActionMiracleUp()
	{
		if (this.CharaViewGUI.IsPlayingMotion)
		{
			return;
		}
		this.PlayAnimation = this.CharaViewGUI.MiracleUp();
	}

	private void ActionPhotoPcketUp()
	{
		if (this.CharaViewGUI.IsPlayingMotion)
		{
			return;
		}
		this.PlayAnimation = this.CharaViewGUI.PhotoPcketUp();
	}

	private void ActionKizunaLvLimitUp()
	{
		if (this.CharaViewGUI.IsPlayingMotion)
		{
			return;
		}
		this.PlayAnimation = this.CharaViewGUI.KizunaLevelLimitUp();
	}

	public void CommunicationAnimationUpdate()
	{
		if (this.PlayAnimation != null && !this.PlayAnimation.MoveNext())
		{
			this.PlayAnimation = null;
		}
	}

	public SortFilterDefine.SortType SortType = SortFilterDefine.SortType.LEVEL;

	public IEnumerator PlayAnimation;

	public List<CommunicationCtrl.CharaViewMenu> CharaViewMenuList;

	public class ScrollBarIconGuiData
	{
		public List<CommunicationCtrl.ScrollBarIconGuiData.IconOne> BarObjList { get; set; }

		public ScrollBarIconGuiData(GameObject go)
		{
			this.baseObj = go;
		}

		public GameObject baseObj;

		public class IconOne
		{
			public IconCharaCtrl Icon { get; private set; }

			public PguiButtonCtrl Button { get; private set; }

			public int CharaId { get; set; }

			public IconOne(GameObject iconObj)
			{
				this.Icon = iconObj.GetComponent<IconCharaCtrl>();
				this.Button = iconObj.GetComponent<PguiButtonCtrl>();
			}
		}
	}

	public class CharaViewGuiData
	{
		public bool IsPlayingMotion { get; set; }

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

		private void OnTouchMask(Transform tr)
		{
			this.touchScreenAuth = true;
		}

		public GameObject baseObj;

		public PguiAECtrl AE_LevelUp;

		public AEImage AEImage_LevelLimit;

		public PguiAECtrl AE_KemonoMiracle;

		public AEImage AEImage_YaseiResult;

		public AEImage AEImage_KizunaLevelLimit;

		public AEImage AEImage_NanairoResult;

		public PguiAECtrl AE_RankUpBg;

		public PguiAECtrl AE_RankUpBack;

		public PguiAECtrl AE_RankUp;

		public SelCharaGrowRank.RankUpAuth CharaRankUpAuth;

		public List<CharaWindowCtrl.GUI.Name> NameList;

		private GameObject AuthHeartLvUp;

		public RenderTextureChara RenderChara;

		public ReuseScroll MemoriesMenuScroll;

		public CharaPackData currentCharaPackData;

		private bool touchScreenAuth;
	}

	public class CommuMenuBarGui
	{
		public PguiButtonCtrl BarButton { get; private set; }

		public PguiTextCtrl MenuName { get; private set; }

		public PguiAECtrl AE_MArkLock { get; private set; }

		public PguiTextCtrl LockText { get; private set; }

		public CommuMenuBarGui(GameObject go)
		{
			this.BarButton = go.GetComponent<PguiButtonCtrl>();
			this.MenuName = go.transform.Find("BaseImage/Txt").GetComponent<PguiTextCtrl>();
			this.LockText = go.transform.Find("Mark_Lock/Txt_LockInfo").GetComponent<PguiTextCtrl>();
			this.AE_MArkLock = go.transform.Find("Mark_Lock").GetComponent<PguiAECtrl>();
		}
	}

	public class CharaViewMenu
	{
		public string MenuName { get; private set; }

		public UnityAction Action { get; private set; }

		public bool IsLock { get; private set; }

		public string LockText { get; private set; }

		public CharaViewMenu(string name, UnityAction action, bool unLock, string lockText)
		{
			this.MenuName = name;
			this.Action = action;
			this.IsLock = !unLock;
			this.LockText = lockText;
		}
	}
}
