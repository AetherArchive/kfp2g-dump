using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using SGNFW.uGUI;
using UnityEngine;

public class DressUpWindowCtrl : MonoBehaviour
{
	public void Init()
	{
		if (this.guiData != null)
		{
			return;
		}
		this.guiData = new DressUpWindowCtrl.GUI(base.transform);
		this.guiData.Btn_OK.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.Btn_View.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.Btn_Yaji_Left.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.Btn_Yaji_Right.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.Btn_InfoChange.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.Btn_Accessory.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickAccessoryButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.Btn_Display.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickDisplayButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.ScrollView.InitForce();
		ReuseScroll scrollView = this.guiData.ScrollView;
		scrollView.onStartItem = (Action<int, GameObject>)Delegate.Combine(scrollView.onStartItem, new Action<int, GameObject>(this.OnStartDressup));
		ReuseScroll scrollView2 = this.guiData.ScrollView;
		scrollView2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scrollView2.onUpdateItem, new Action<int, GameObject>(this.OnUpdateDressup));
		this.guiData.ScrollView.Setup(10, 0);
		this.renderTextureChara = null;
	}

	public void Open(CharaPackData cpd, DressUpWindowCtrl.OpenParameter param)
	{
		this.openParam = param;
		this.currentCharaPackData = cpd;
		this.dispCharaPackList = param.cpdList;
		base.gameObject.SetActive(true);
		this.guiData.Btn_Yaji_Left.gameObject.SetActive(param.change && this.dispCharaPackList != null);
		this.guiData.Btn_Yaji_Right.gameObject.SetActive(param.change && this.dispCharaPackList != null);
		this.guiData.Btn_View.gameObject.SetActive(param.camera);
		this.changeChara = this.ChangeChara(this.currentCharaPackData);
	}

	public void OpenPrev()
	{
		base.gameObject.SetActive(true);
		this.changeChara = this.ChangeChara(this.currentCharaPackData);
	}

	public void Close()
	{
		base.gameObject.SetActive(false);
		if (this.renderTextureChara != null)
		{
			Object.Destroy(this.renderTextureChara.gameObject);
		}
		this.renderTextureChara = null;
	}

	public bool IsActive()
	{
		return base.gameObject.activeSelf;
	}

	private void Update()
	{
		if (this.changeChara != null && !this.changeChara.MoveNext())
		{
			this.changeChara = null;
		}
		if (this.changeClothes != null && !this.changeClothes.MoveNext())
		{
			this.changeClothes = null;
		}
		if (this.changeDispEffect != null && !this.changeDispEffect.MoveNext())
		{
			this.changeDispEffect = null;
		}
	}

	private IEnumerator ChangeChara(CharaPackData cpd)
	{
		this.currentCharaPackData = cpd;
		this.currenEquipClothId = cpd.dynamicData.equipClothesId;
		this.countInfoChange = 0;
		this.guiData.Txt_Name.text = this.currentCharaPackData.staticData.baseData.charaName;
		this.guiData.Txt_WName.text = this.currentCharaPackData.staticData.baseData.NickName;
		this.guiData.Txt_Name_EG.text = this.currentCharaPackData.staticData.baseData.charaNameEng;
		this.ChangeDispButton(this.currentCharaPackData.dynamicData.dispAccessoryEffect);
		List<CharaClothStatic> clothListByChara = DataManager.DmChara.GetClothListByChara(this.currentCharaPackData.id);
		List<ItemData> userItemListByKind = DataManager.DmItem.GetUserItemListByKind(ItemDef.Kind.CLOTHES);
		List<int> haveClothesIdList = this.currentCharaPackData.dynamicData.haveClothesIdList;
		this.haveClothes = new List<CharaClothStatic>();
		List<CharaClothStatic> list = new List<CharaClothStatic>();
		foreach (ItemData itemData in userItemListByKind)
		{
			CharaClothStatic ccs = itemData.staticData as CharaClothStatic;
			if (ccs.CharaId == this.currentCharaPackData.id && haveClothesIdList.Exists((int id) => id == ccs.GetId()))
			{
				this.haveClothes.Add(ccs);
			}
		}
		foreach (CharaClothStatic charaClothStatic in clothListByChara)
		{
			if (charaClothStatic.GetRank > 0 && charaClothStatic.GetRank <= this.currentCharaPackData.staticData.baseData.rankHigh)
			{
				list.Add(charaClothStatic);
			}
		}
		this.haveClothes.Sort((CharaClothStatic a, CharaClothStatic b) => a.GetId() - b.GetId());
		PrjUtil.InsertionSort<CharaClothStatic>(ref this.haveClothes, (CharaClothStatic a, CharaClothStatic b) => a.SortNum - b.SortNum);
		list.Sort((CharaClothStatic a, CharaClothStatic b) => a.GetId() - b.GetId());
		this.totalHpBonus = (this.totalAtkBonus = (this.totalDefBonus = 0));
		foreach (CharaClothStatic charaClothStatic2 in this.haveClothes)
		{
			this.totalAtkBonus += charaClothStatic2.AtkBonus;
			this.totalDefBonus += charaClothStatic2.DefBonus;
			this.totalHpBonus += charaClothStatic2.HpBonus;
		}
		this.guiData.Num_Param01.text = PrjUtil.MakeMessage(string.Format("+ {0}", this.totalHpBonus));
		this.guiData.Num_Param02.text = PrjUtil.MakeMessage(string.Format("+ {0}", this.totalAtkBonus));
		this.guiData.Num_Param03.text = PrjUtil.MakeMessage(string.Format("+ {0}", this.totalDefBonus));
		using (List<CharaClothStatic>.Enumerator enumerator2 = list.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				CharaClothStatic e = enumerator2.Current;
				if (!this.haveClothes.Exists((CharaClothStatic item) => item.GetId() == e.GetId()))
				{
					this.haveClothes.Add(e);
				}
			}
		}
		this.guiData.ParamInfo.SetActive(this.countInfoChange % 2 == 1);
		this.guiData.DressListAnim.enabled = true;
		this.guiData.ScrollView.Resize(this.haveClothes.Count, 0);
		this.guiData.DressListAnim.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, delegate
		{
			this.guiData.DressListAnim.enabled = false;
			this.guiData.ScrollView.Resize(this.haveClothes.Count, 0);
		});
		if (this.renderTextureChara == null)
		{
			this.renderTextureChara = Object.Instantiate<GameObject>((GameObject)Resources.Load("RenderTextureChara/Prefab/RenderTextureCharaCtrl"), this.guiData.Texture_Render.transform).GetComponent<RenderTextureChara>();
			this.renderTextureChara.fieldOfView = 30f;
		}
		this.renderTextureChara.Setup(this.currentCharaPackData, 2, CharaMotionDefine.ActKey.SCENARIO_STAND_BY, true, null, false, null, 0f, null, false);
		this.guiData.Txt_Name_Cloth.text = DataManager.DmChara.GetCharaClothesStaticData(this.currenEquipClothId).GetName();
		this.guiData.AEImage_Cloth_Name.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
		while (this.guiData.AEImage_Cloth_Name.ExIsPlaying())
		{
			yield return null;
		}
		IEnumerator wait = this.Wait(4f);
		while (wait.MoveNext())
		{
			yield return null;
		}
		if (!this.changingCloth)
		{
			this.guiData.AEImage_Cloth_Name.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, null);
		}
		wait = null;
		yield break;
	}

	private void ChangeDispButton(bool flag)
	{
		this.guiData.Display_On.SetActive(flag);
		this.guiData.Display_Off.SetActive(!flag);
	}

	private IEnumerator ChangeClothes()
	{
		this.changingCloth = true;
		SoundManager.Play("prd_se_friends_costume_change", false, false);
		this.guiData.Txt_Name_Cloth.text = DataManager.DmChara.GetCharaClothesStaticData(this.currenEquipClothId).GetName();
		this.guiData.AEImage_Cloth_Name.ExPauseAnimation(SimpleAnimation.ExPguiStatus.START, null);
		this.guiData.AEImage_Wipe.PlayAnimation(PguiAECtrl.AmimeType.START, null);
		IEnumerator wait = this.Wait(0.5f);
		while (wait.MoveNext())
		{
			yield return null;
		}
		CharaMotionDefine.ActKey[] array = new CharaMotionDefine.ActKey[]
		{
			CharaMotionDefine.ActKey.COS_CHANGE1,
			CharaMotionDefine.ActKey.COS_CHANGE2,
			CharaMotionDefine.ActKey.COS_CHANGE3
		};
		int num = new Random().Next(array.Length);
		this.renderTextureChara.Setup(this.currentCharaPackData.id, 2, array[num], DataManager.DmChara.ColothId2ImageId(this.currenEquipClothId), DataManager.DmChara.ClothLongSkirt(this.currenEquipClothId), false, delegate
		{
			this.renderTextureChara.SetAnimation(CharaMotionDefine.ActKey.SCENARIO_STAND_BY, true);
		}, false, null, 0f, null, false, false, false);
		this.renderTextureChara.StopAnimation();
		int num2;
		for (int i = 0; i < 40; i = num2 + 1)
		{
			yield return null;
			num2 = i;
		}
		this.guiData.AEImage_Cloth_Name.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
		while (this.guiData.AEImage_Wipe.IsPlaying())
		{
			yield return null;
		}
		this.renderTextureChara.RestartAnimation();
		this.renderTextureChara.PlayVoice(VOICE_TYPE.COS01);
		while (this.guiData.AEImage_Cloth_Name.ExIsPlaying())
		{
			yield return null;
		}
		wait = this.Wait(4f);
		while (wait.MoveNext())
		{
			yield return null;
		}
		this.guiData.AEImage_Cloth_Name.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, null);
		this.changingCloth = false;
		yield return null;
		yield break;
	}

	private IEnumerator Wait(float second)
	{
		float timeSinceStartup = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup - timeSinceStartup < second)
		{
			yield return null;
		}
		yield break;
	}

	private void OnClickButton(PguiButtonCtrl pbc)
	{
		if (CanvasManager.HdlDressUpWipeCtrl.IsActive())
		{
			return;
		}
		if (this.guiData.AEImage_Wipe.IsPlaying())
		{
			return;
		}
		Func<object> func = delegate
		{
			SceneManager.SceneName currentSceneName = Singleton<SceneManager>.Instance.CurrentSceneName;
			if (currentSceneName == SceneManager.SceneName.SceneBattleSelector)
			{
				return new SceneBattleSelector.Args
				{
					detailCharaId = this.currentCharaPackData.id
				};
			}
			if (currentSceneName == SceneManager.SceneName.SceneQuest)
			{
				return new SceneQuest.Args
				{
					category = QuestStaticChapter.Category.CHARA,
					selectCharaId = this.currentCharaPackData.id
				};
			}
			if (currentSceneName == SceneManager.SceneName.SceneCharaEdit)
			{
				SceneCharaEdit sceneCharaEdit = (SceneCharaEdit)Singleton<SceneManager>.Instance.CurrentScene;
				return new SceneCharaEdit.Args
				{
					growCharaId = ((this.openParam.preset == DressUpWindowCtrl.OpenParameter.Preset.MINE_EASY_NO_GROW) ? this.currentCharaPackData.id : 0),
					detailCharaId = ((this.openParam.preset != DressUpWindowCtrl.OpenParameter.Preset.MINE_EASY_NO_GROW) ? this.currentCharaPackData.id : 0),
					openDressWindow = true,
					menuBackRequestMode = sceneCharaEdit.getCurrentMode()
				};
			}
			return null;
		};
		if (!(this.guiData.Btn_OK == pbc))
		{
			if (this.guiData.Btn_View == pbc)
			{
				this.Close();
				SceneHome.Args args = new SceneHome.Args
				{
					charaPackData = this.currentCharaPackData,
					sceneName = ((Singleton<SceneManager>.Instance != null) ? Singleton<SceneManager>.Instance.CurrentSceneName : SceneManager.SceneName.None),
					menuBackSceneArgs = func()
				};
				if (Singleton<SceneManager>.Instance != null)
				{
					Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneHome, args);
					return;
				}
			}
			else if (pbc == this.guiData.Btn_Yaji_Left || pbc == this.guiData.Btn_Yaji_Right)
			{
				if (this.renderTextureChara != null && !this.renderTextureChara.FinishedSetup)
				{
					return;
				}
				if (this.dispCharaPackList == null || !this.dispCharaPackList.Contains(this.currentCharaPackData))
				{
					return;
				}
				int num = this.dispCharaPackList.IndexOf(this.currentCharaPackData);
				num += ((pbc == this.guiData.Btn_Yaji_Left) ? (-1) : 1);
				num = (num + this.dispCharaPackList.Count) % this.dispCharaPackList.Count;
				this.changeChara = this.ChangeChara(this.dispCharaPackList[num]);
				return;
			}
			else if (pbc == this.guiData.Btn_InfoChange)
			{
				this.countInfoChange++;
				this.guiData.ParamInfo.SetActive(this.countInfoChange % 2 == 1);
				this.guiData.ScrollView.Refresh();
			}
			return;
		}
		if (this.openParam.preset == DressUpWindowCtrl.OpenParameter.Preset.HOME || this.openParam.preset == DressUpWindowCtrl.OpenParameter.Preset.HOME_LIST)
		{
			CanvasManager.HdlDressUpWipeCtrl.Play(delegate
			{
				this.Close();
			});
			return;
		}
		CanvasManager.HdlDressUpWipeCtrl.Play(delegate
		{
			this.Close();
			CanvasManager.HdlCharaWindowCtrl.OpenPrev();
		});
	}

	private void OnClickDressButton(PguiButtonCtrl pbc)
	{
		if (CanvasManager.HdlDressUpWipeCtrl.IsActive())
		{
			return;
		}
		if (this.guiData.AEImage_Wipe.IsPlaying())
		{
			return;
		}
		int id = pbc.GetComponent<PguiDataHolder>().id;
		if (!this.HaveCloth(id))
		{
			return;
		}
		if (this.renderTextureChara == null || !this.renderTextureChara.FinishedSetup || id == this.currenEquipClothId)
		{
			return;
		}
		this.currenEquipClothId = id;
		this.guiData.ScrollView.Refresh();
		this.currentCharaPackData.dynamicData.equipClothesId = id;
		DataManager.DmChara.RequestActoinCharaChangeClothes(this.currentCharaPackData.id, id);
		this.changeClothes = this.ChangeClothes();
	}

	private void OnStartDressup(int index, GameObject go)
	{
		go.GetComponent<PguiButtonCtrl>().AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickDressButton), PguiButtonCtrl.SoundType.DEFAULT);
		go.transform.Find("BaseImage/All/Mark_Wear").gameObject.SetActive(false);
		go.AddComponent<PguiDataHolder>();
	}

	private void OnUpdateDressup(int index, GameObject go)
	{
		go.SetActive(true);
		GameObject gameObject = go.transform.Find("BaseImage/All/Mark_Wear").gameObject;
		GameObject gameObject2 = go.transform.Find("BaseImage/All/Num_Star").gameObject;
		go.GetComponent<PguiDataHolder>().id = 0;
		if (index < this.haveClothes.Count)
		{
			CharaClothStatic charaClothStatic = this.haveClothes[index];
			go.transform.Find("BaseImage/All/Texture_Bg").GetComponent<PguiRawImageCtrl>().SetRawImage(charaClothStatic.displayBgTexturePath, true, false, null);
			go.transform.Find("BaseImage/All/Texture_Chara").GetComponent<PguiRawImageCtrl>().SetRawImage(charaClothStatic.displayTexturePath, true, false, null);
			GameObject gameObject3 = go.transform.Find("BaseImage/All/ParamInfo").gameObject;
			go.GetComponent<PguiDataHolder>().id = charaClothStatic.GetId();
			gameObject.SetActive(this.currenEquipClothId == charaClothStatic.GetId());
			gameObject3.SetActive(this.countInfoChange % 2 == 1);
			gameObject3.transform.Find("All/Param01/Num_Param").GetComponent<PguiTextCtrl>().text = string.Format("+ {0}", charaClothStatic.HpBonus);
			gameObject3.transform.Find("All/Param02/Num_Param").GetComponent<PguiTextCtrl>().text = string.Format("+ {0}", charaClothStatic.AtkBonus);
			gameObject3.transform.Find("All/Param03/Num_Param").GetComponent<PguiTextCtrl>().text = string.Format("+ {0}", charaClothStatic.DefBonus);
			bool flag = this.HaveCloth(charaClothStatic);
			PguiColorChangeCtrl component = go.transform.Find("BaseImage/All/Texture_Chara").GetComponent<PguiColorChangeCtrl>();
			if (component != null)
			{
				component.SetEnable(!flag);
			}
			if (charaClothStatic.GetRank <= 0)
			{
				gameObject2.SetActive(false);
				return;
			}
			gameObject2.SetActive(!flag);
			if (!flag)
			{
				gameObject2.GetComponent<PguiTextCtrl>().text = "";
				for (int i = 0; i < charaClothStatic.GetRank; i++)
				{
					PguiTextCtrl component2 = gameObject2.GetComponent<PguiTextCtrl>();
					component2.text += PrjUtil.MakeMessage("★");
				}
				return;
			}
		}
		else
		{
			go.SetActive(false);
		}
	}

	private bool HaveCloth(CharaClothStatic ccs)
	{
		return this.HaveCloth(ccs.GetId());
	}

	private bool HaveCloth(int id)
	{
		bool flag = false;
		using (List<ItemData>.Enumerator enumerator = DataManager.DmItem.GetUserItemListByKind(ItemDef.Kind.CLOTHES).GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if ((enumerator.Current.staticData as CharaClothStatic).GetId() == id)
				{
					flag = true;
					break;
				}
			}
		}
		if (id == 0)
		{
			flag = true;
		}
		return flag;
	}

	private void OnClickAccessoryButton(PguiButtonCtrl pbc)
	{
		CanvasManager.HdlDetachableAccessoryWindowCtrl.Open(this.currentCharaPackData, null);
	}

	private void OnClickDisplayButton(PguiButtonCtrl pbc)
	{
		if (this.changeDispEffect == null)
		{
			this.changeDispEffect = this.ChangeDispEffect();
		}
	}

	private IEnumerator ChangeDispEffect()
	{
		bool flag = !this.currentCharaPackData.dynamicData.dispAccessoryEffect;
		DataManager.DmChara.RequestActoinCharaAccessoryEffectStatus(this.currentCharaPackData.id, flag);
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		this.ChangeDispButton(this.currentCharaPackData.dynamicData.dispAccessoryEffect);
		yield break;
	}

	private static readonly int DEFAULT_CLOTH_ID;

	private DressUpWindowCtrl.GUI guiData;

	private CharaPackData currentCharaPackData;

	private List<CharaPackData> dispCharaPackList;

	private List<CharaClothStatic> currentHaveClothes;

	private List<CharaClothStatic> haveClothes;

	private RenderTextureChara renderTextureChara;

	private int currenEquipClothId;

	private int totalHpBonus;

	private int totalAtkBonus;

	private int totalDefBonus;

	private int countInfoChange;

	private bool changingCloth;

	private DressUpWindowCtrl.OpenParameter openParam;

	private IEnumerator changeChara;

	private IEnumerator changeClothes;

	private IEnumerator changeDispEffect;

	public class GUI
	{
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Btn_View = baseTr.Find("Btn_View").GetComponent<PguiButtonCtrl>();
			this.Btn_OK = baseTr.Find("Btn_OK").GetComponent<PguiButtonCtrl>();
			this.Btn_OK.androidBackKeyTarget = true;
			this.Btn_Yaji_Right = baseTr.Find("Btn_Yaji_Right").GetComponent<PguiButtonCtrl>();
			this.Btn_Yaji_Left = baseTr.Find("Btn_Yaji_Left").GetComponent<PguiButtonCtrl>();
			this.ScrollBg = baseTr.Find("ScrollBg").GetComponent<PguiImageCtrl>();
			this.Texture_Render = baseTr.Find("Mirror/Mask/Texture_Render").GetComponent<PguiRawImageCtrl>();
			this.Txt_Class = baseTr.Find("Txt_Class").GetComponent<PguiTextCtrl>();
			this.Txt_Class.gameObject.SetActive(false);
			this.Txt_WName = baseTr.Find("Txt_WName").GetComponent<PguiTextCtrl>();
			this.Txt_Name = baseTr.Find("Txt_Name").GetComponent<PguiTextCtrl>();
			this.Txt_Name_EG = baseTr.Find("Txt_Name_EG").GetComponent<PguiTextCtrl>();
			this.ScrollView = baseTr.Find("DressList/ScrollView").GetComponent<ReuseScroll>();
			this.Txt_Name_Cloth = baseTr.Find("Mirror/Mask/Cloth_Name/Txt_Name").GetComponent<PguiTextCtrl>();
			this.AEImage_Cloth_Name = baseTr.Find("Mirror/Mask/Cloth_Name").GetComponent<SimpleAnimation>();
			this.AEImage_Wipe = baseTr.Find("Mirror/Mask/AEImage_Wipe").GetComponent<PguiAECtrl>();
			this.ParamInfo = baseTr.Find("Mirror/ParamInfo").gameObject;
			this.Num_Param01 = baseTr.Find("Mirror/ParamInfo/Param01/Num_Param").GetComponent<PguiTextCtrl>();
			this.Num_Param02 = baseTr.Find("Mirror/ParamInfo/Param02/Num_Param").GetComponent<PguiTextCtrl>();
			this.Num_Param03 = baseTr.Find("Mirror/ParamInfo/Param03/Num_Param").GetComponent<PguiTextCtrl>();
			this.DressListAnim = baseTr.Find("DressList").GetComponent<SimpleAnimation>();
			this.Btn_InfoChange = baseTr.Find("Btn_InfoChange").GetComponent<PguiButtonCtrl>();
			this.Btn_Accessory = baseTr.Find("Btn_Accessory").GetComponent<PguiButtonCtrl>();
			this.Btn_Display = baseTr.Find("Btn_Display").GetComponent<PguiButtonCtrl>();
			this.Display_On = baseTr.Find("Btn_Display/BaseImage/On").gameObject;
			this.Display_Off = baseTr.Find("Btn_Display/BaseImage/Off").gameObject;
		}

		public GameObject baseObj;

		public PguiButtonCtrl Btn_View;

		public PguiButtonCtrl Btn_OK;

		public PguiButtonCtrl Btn_Yaji_Right;

		public PguiButtonCtrl Btn_Yaji_Left;

		public PguiImageCtrl ScrollBg;

		public PguiRawImageCtrl Texture_Render;

		public PguiTextCtrl Txt_Class;

		public PguiTextCtrl Txt_WName;

		public PguiTextCtrl Txt_Name;

		public PguiTextCtrl Txt_Name_EG;

		public PguiTextCtrl Txt_Name_Cloth;

		public ReuseScroll ScrollView;

		public PguiAECtrl AEImage_Wipe;

		public SimpleAnimation AEImage_Cloth_Name;

		public GameObject ParamInfo;

		public PguiTextCtrl Num_Param01;

		public PguiTextCtrl Num_Param02;

		public PguiTextCtrl Num_Param03;

		public SimpleAnimation DressListAnim;

		public PguiButtonCtrl Btn_InfoChange;

		public PguiButtonCtrl Btn_Accessory;

		public PguiButtonCtrl Btn_Display;

		public GameObject Display_On;

		public GameObject Display_Off;
	}

	public class OpenParameter
	{
		public OpenParameter()
		{
		}

		public OpenParameter(DressUpWindowCtrl.OpenParameter.Preset type, List<CharaPackData> dispList = null)
		{
			this.change = false;
			this.camera = false;
			this.preset = type;
			this.cpdList = dispList;
			if (type != DressUpWindowCtrl.OpenParameter.Preset.HOME)
			{
				if (type != DressUpWindowCtrl.OpenParameter.Preset.NO_VIEW)
				{
					this.change = true;
					this.camera = true;
				}
				return;
			}
			this.camera = true;
		}

		public List<CharaPackData> cpdList;

		public bool change;

		public bool camera;

		public DressUpWindowCtrl.OpenParameter.Preset preset;

		public enum Preset
		{
			INVALID,
			DEFAULT,
			HOME,
			HOME_LIST,
			NO_VIEW,
			MINE_EASY_NO_GROW
		}
	}
}
