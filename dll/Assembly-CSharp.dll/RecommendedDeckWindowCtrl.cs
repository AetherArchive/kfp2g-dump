using System;
using System.Collections.Generic;
using UnityEngine;

public class RecommendedDeckWindowCtrl : MonoBehaviour
{
	public void Init(GameObject baseWindow)
	{
		this.guiData = new RecommendedDeckWindowCtrl.GUI(baseWindow.transform);
		for (int i = 0; i < this.guiData.AttrBtn.Count; i++)
		{
			this.guiData.AttrBtn[i].AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickAttrButton));
			this.guiData.AttrBtn[i].name = i.ToString();
		}
		for (int j = 0; j < this.guiData.StatBtn.Count; j++)
		{
			this.guiData.StatBtn[j].AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickStatButton));
			this.guiData.StatBtn[j].name = j.ToString();
		}
		for (int k = 0; k < this.guiData.TypeBtn.Count; k++)
		{
			this.guiData.TypeBtn[k].AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickTypeButton));
			this.guiData.TypeBtn[k].name = k.ToString();
		}
		for (int l = 0; l < this.guiData.Stat2Btn.Count; l++)
		{
			this.guiData.Stat2Btn[l].AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickStat2Button));
			this.guiData.Stat2Btn[l].name = l.ToString();
		}
		this.guiData.CatFriends.AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickFriendsButton));
		this.guiData.CatPhoto.AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickPhotoButton));
		this.registerData = new RecommendedDeckWindowCtrl.RegisterData();
		this.registerSaveData = new RecommendedDeckWindowCtrl.RegisterSaveData();
	}

	public void Register(RecommendedDeckWindowCtrl.RegisterData data)
	{
		this.registerData = data;
		if (data.filterButton != null)
		{
			data.filterButton.AddOnClickListener(delegate(PguiButtonCtrl btn)
			{
				this.OnClickRegisterButton(btn);
			}, PguiButtonCtrl.SoundType.DECIDE);
		}
		this.DecorationRegisterButton(data);
		RecommendedDeckWindowCtrl.RegisterSaveData registerSaveData = this.registerSaveData;
	}

	private void DecorationRegisterButton(RecommendedDeckWindowCtrl.RegisterData data)
	{
	}

	private void Update()
	{
	}

	private void Open()
	{
		RecommendedDeckWindowCtrl.RegisterSaveData registerSaveData = this.registerSaveData;
		RecommendedDeckWindowCtrl.RegisterData registerData = this.registerData;
		CharaDef.AttributeMask attributeMask = (CharaDef.AttributeMask)0;
		this.guiData.EnemyInfo.SetActive(registerData.questOneId() >= 0);
		if (this.guiData.EnemyInfo.activeSelf)
		{
			attributeMask = DataManager.DmQuest.GetQuestOnePackData(registerData.questOneId()).questOne.ennemyAttrMask;
			for (int i = 0; i < this.guiData.enemyInfoList.Count; i++)
			{
				this.guiData.enemyInfoList[i].Setup((attributeMask & SelBattleHelperCtrl.GUI.EnemyInfo.attributeMaskList[i]) == SelBattleHelperCtrl.GUI.EnemyInfo.attributeMaskList[i]);
			}
		}
		for (int j = 0; j < SceneBattle.attrMatch.GetLength(0); j++)
		{
			bool flag = (attributeMask & SceneBattle.attrMatch[j, 1]) > (CharaDef.AttributeMask)0;
			bool flag2 = (attributeMask & SceneBattle.attrMatch[j, 2]) > (CharaDef.AttributeMask)0;
			if (j + 1 < this.guiData.AttrBtn.Count)
			{
				Transform transform = this.guiData.AttrBtn[j + 1].transform.Find("Mark_Good_Bad");
				transform.Find("Mark_Good").gameObject.SetActive(flag);
				transform.Find("Mark_Bad").gameObject.SetActive(flag2);
			}
		}
		this.guiData.AttrBtn[0].SetToggleIndex((registerSaveData.attrMask == (CharaDef.AttributeMask)0) ? 1 : 0);
		foreach (object obj in Enum.GetValues(typeof(CharaDef.AttributeMask)))
		{
			CharaDef.AttributeMask attributeMask2 = (CharaDef.AttributeMask)obj;
			CharaDef.AttributeType attributeType = CharaDef.AttributeMask2Type(attributeMask2);
			this.guiData.AttrBtn[(int)attributeType].SetToggleIndex(((registerSaveData.attrMask & attributeMask2) == (CharaDef.AttributeMask)0) ? 0 : 1);
		}
		foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl in this.guiData.StatBtn)
		{
			pguiToggleButtonCtrl.SetToggleIndex((pguiToggleButtonCtrl == this.guiData.StatBtn[(int)registerSaveData.sortType]) ? 1 : 0);
		}
		foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl2 in this.guiData.TypeBtn)
		{
			pguiToggleButtonCtrl2.SetToggleIndex((pguiToggleButtonCtrl2 == this.guiData.TypeBtn[(int)registerSaveData.photoType]) ? 1 : 0);
		}
		foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl3 in this.guiData.Stat2Btn)
		{
			pguiToggleButtonCtrl3.SetToggleIndex((pguiToggleButtonCtrl3 == this.guiData.Stat2Btn[(int)registerSaveData.sort2Type]) ? 1 : 0);
		}
		this.guiData.CatFriends.SetToggleIndex(registerSaveData.isFriends ? 1 : 0);
		this.guiData.CatFriends.transform.Find("BaseImage/Txt").GetComponent<PguiTextCtrl>().text = (registerSaveData.isFriends ? "ON" : "OFF");
		this.guiData.CatPhoto.SetToggleIndex(registerSaveData.isPhoto ? 1 : 0);
		this.guiData.CatPhoto.transform.Find("BaseImage/Txt").GetComponent<PguiTextCtrl>().text = (registerSaveData.isPhoto ? "ON" : "OFF");
		this.guiData.baseWindow.Setup(PrjUtil.MakeMessage("おまかせ編成"), PrjUtil.MakeMessage("重視する項目を選択してください"), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CANCEL_OK), true, new PguiOpenWindowCtrl.Callback(this.OnClickWindowButton), null, false);
		this.guiData.baseWindow.Open();
	}

	private void OnClickRegisterButton(PguiButtonCtrl button)
	{
		RecommendedDeckWindowCtrl.RegisterData registerData = this.registerData;
		if (registerData.filterButton == button)
		{
			RecommendedDeckWindowCtrl.SortTarget sortTarget = registerData.funcGetTargetBaseList();
			if (sortTarget != null)
			{
				this.currentTarget = new RecommendedDeckWindowCtrl.SortTarget(sortTarget);
				this.Open();
			}
		}
	}

	private bool OnClickWindowButton(int index)
	{
		RecommendedDeckWindowCtrl.RegisterData registerData = this.registerData;
		if (index == 1)
		{
			RecommendedDeckWindowCtrl.RegisterSaveData rSaveData = this.registerSaveData;
			if (rSaveData.isFriends || rSaveData.isPhoto)
			{
				rSaveData.SolutionList(ref this.currentTarget);
				registerData.funcDisideTarget(this.currentTarget);
				this.DecorationRegisterButton(registerData);
			}
			else
			{
				CanvasManager.HdlOpenWindowBasic.Setup("おまかせ編成", "おまかせ設定がOFFになっています\n\n「フレンズ」と「フォト」のおまかせ設定をONにしました", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, delegate(int idx)
				{
					rSaveData.isFriends = (rSaveData.isPhoto = true);
					this.Open();
					return true;
				}, null, false);
				CanvasManager.HdlOpenWindowBasic.Open();
			}
		}
		else
		{
			registerData.funcDisideTarget(null);
		}
		return true;
	}

	private bool OnClickAttrButton(PguiToggleButtonCtrl button, int index)
	{
		RecommendedDeckWindowCtrl.RegisterSaveData registerSaveData = this.registerSaveData;
		if (int.Parse(button.name) == 0)
		{
			this.guiData.AttrBtn[0].SetToggleIndex(1);
			foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl in this.guiData.AttrBtn)
			{
				if (pguiToggleButtonCtrl != button)
				{
					pguiToggleButtonCtrl.SetToggleIndex(0);
				}
			}
			registerSaveData.attrMask = (CharaDef.AttributeMask)0;
			if (index == 0)
			{
				SoundManager.Play("prd_se_click", false, false);
			}
		}
		else
		{
			button.SetToggleIndex((index == 0) ? 1 : 0);
			CharaDef.AttributeMask attributeMask = (CharaDef.AttributeMask)(1 << int.Parse(button.name) - 1);
			if (index == 0)
			{
				registerSaveData.attrMask |= attributeMask;
			}
			else
			{
				registerSaveData.attrMask &= ~attributeMask;
			}
			this.guiData.AttrBtn[0].SetToggleIndex((registerSaveData.attrMask == (CharaDef.AttributeMask)0) ? 1 : 0);
			SoundManager.Play("prd_se_click", false, false);
		}
		return false;
	}

	private bool OnClickStatButton(PguiToggleButtonCtrl button, int index)
	{
		this.registerSaveData.sortType = (RecommendedDeckWindowCtrl.SortType)int.Parse(button.name);
		if (index == 0)
		{
			foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl in this.guiData.StatBtn)
			{
				if (pguiToggleButtonCtrl != button)
				{
					pguiToggleButtonCtrl.SetToggleIndex(0);
				}
			}
			return true;
		}
		return false;
	}

	private bool OnClickTypeButton(PguiToggleButtonCtrl button, int index)
	{
		this.registerSaveData.photoType = (PhotoDef.Type)int.Parse(button.name);
		if (index == 0)
		{
			foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl in this.guiData.TypeBtn)
			{
				if (pguiToggleButtonCtrl != button)
				{
					pguiToggleButtonCtrl.SetToggleIndex(0);
				}
			}
			return true;
		}
		return false;
	}

	private bool OnClickStat2Button(PguiToggleButtonCtrl button, int index)
	{
		this.registerSaveData.sort2Type = (RecommendedDeckWindowCtrl.SortType)int.Parse(button.name);
		if (index == 0)
		{
			foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl in this.guiData.Stat2Btn)
			{
				if (pguiToggleButtonCtrl != button)
				{
					pguiToggleButtonCtrl.SetToggleIndex(0);
				}
			}
			return true;
		}
		return false;
	}

	private bool OnClickFriendsButton(PguiToggleButtonCtrl button, int index)
	{
		RecommendedDeckWindowCtrl.RegisterSaveData registerSaveData = this.registerSaveData;
		registerSaveData.isFriends = index == 0;
		button.transform.Find("BaseImage/Txt").GetComponent<PguiTextCtrl>().text = (registerSaveData.isFriends ? "ON" : "OFF");
		return true;
	}

	private bool OnClickPhotoButton(PguiToggleButtonCtrl button, int index)
	{
		RecommendedDeckWindowCtrl.RegisterSaveData registerSaveData = this.registerSaveData;
		registerSaveData.isPhoto = index == 0;
		button.transform.Find("BaseImage/Txt").GetComponent<PguiTextCtrl>().text = (registerSaveData.isPhoto ? "ON" : "OFF");
		return true;
	}

	private RecommendedDeckWindowCtrl.GUI guiData;

	private RecommendedDeckWindowCtrl.RegisterData registerData;

	private RecommendedDeckWindowCtrl.RegisterSaveData registerSaveData;

	private RecommendedDeckWindowCtrl.SortTarget currentTarget;

	public enum SortType
	{
		TOTAL,
		HP,
		ATK,
		DEF
	}

	public class GUI
	{
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.baseWindow = baseTr.Find("Window_AutoDeck").GetComponent<PguiOpenWindowCtrl>();
			this.EnemyInfo = baseTr.Find("Window_AutoDeck/Base/Window/Contents/EnemyInfo").gameObject;
			this.AttrBtn = new List<PguiToggleButtonCtrl>
			{
				baseTr.Find("Window_AutoDeck/Base/Window/Contents/AtrSelect/Btn00").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("Window_AutoDeck/Base/Window/Contents/AtrSelect/Btn01").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("Window_AutoDeck/Base/Window/Contents/AtrSelect/Btn02").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("Window_AutoDeck/Base/Window/Contents/AtrSelect/Btn03").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("Window_AutoDeck/Base/Window/Contents/AtrSelect/Btn04").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("Window_AutoDeck/Base/Window/Contents/AtrSelect/Btn05").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("Window_AutoDeck/Base/Window/Contents/AtrSelect/Btn06").GetComponent<PguiToggleButtonCtrl>()
			};
			this.StatBtn = new List<PguiToggleButtonCtrl>
			{
				baseTr.Find("Window_AutoDeck/Base/Window/Contents/StatusSelect/Btn01").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("Window_AutoDeck/Base/Window/Contents/StatusSelect/Btn02").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("Window_AutoDeck/Base/Window/Contents/StatusSelect/Btn03").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("Window_AutoDeck/Base/Window/Contents/StatusSelect/Btn04").GetComponent<PguiToggleButtonCtrl>()
			};
			this.TypeBtn = new List<PguiToggleButtonCtrl>
			{
				baseTr.Find("Window_AutoDeck/Base/Window/Contents/TypeSelect/Btn00").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("Window_AutoDeck/Base/Window/Contents/TypeSelect/Btn02").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("Window_AutoDeck/Base/Window/Contents/TypeSelect/Btn01").GetComponent<PguiToggleButtonCtrl>()
			};
			this.Stat2Btn = new List<PguiToggleButtonCtrl>
			{
				baseTr.Find("Window_AutoDeck/Base/Window/Contents/StatusSelect02/Btn01").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("Window_AutoDeck/Base/Window/Contents/StatusSelect02/Btn02").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("Window_AutoDeck/Base/Window/Contents/StatusSelect02/Btn03").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("Window_AutoDeck/Base/Window/Contents/StatusSelect02/Btn04").GetComponent<PguiToggleButtonCtrl>()
			};
			this.enemyInfoList = new List<SelBattleHelperCtrl.GUI.EnemyInfo>
			{
				new SelBattleHelperCtrl.GUI.EnemyInfo(baseTr.Find("Window_AutoDeck/Base/Window/Contents/EnemyInfo/AtrInfo01/Icon_Atr_R")),
				new SelBattleHelperCtrl.GUI.EnemyInfo(baseTr.Find("Window_AutoDeck/Base/Window/Contents/EnemyInfo/AtrInfo01/Icon_Atr_G")),
				new SelBattleHelperCtrl.GUI.EnemyInfo(baseTr.Find("Window_AutoDeck/Base/Window/Contents/EnemyInfo/AtrInfo01/Icon_Atr_B")),
				new SelBattleHelperCtrl.GUI.EnemyInfo(baseTr.Find("Window_AutoDeck/Base/Window/Contents/EnemyInfo/AtrInfo02/Icon_Atr_R")),
				new SelBattleHelperCtrl.GUI.EnemyInfo(baseTr.Find("Window_AutoDeck/Base/Window/Contents/EnemyInfo/AtrInfo02/Icon_Atr_G")),
				new SelBattleHelperCtrl.GUI.EnemyInfo(baseTr.Find("Window_AutoDeck/Base/Window/Contents/EnemyInfo/AtrInfo02/Icon_Atr_B"))
			};
			this.CatFriends = baseTr.Find("Window_AutoDeck/Base/Window/Contents/Category_Friends/Btn_Radio02").GetComponent<PguiToggleButtonCtrl>();
			this.CatPhoto = baseTr.Find("Window_AutoDeck/Base/Window/Contents/Category_Photo/Btn_Radio02").GetComponent<PguiToggleButtonCtrl>();
		}

		public GameObject baseObj;

		public GameObject EnemyInfo;

		public List<SelBattleHelperCtrl.GUI.EnemyInfo> enemyInfoList = new List<SelBattleHelperCtrl.GUI.EnemyInfo>();

		public List<PguiToggleButtonCtrl> AttrBtn;

		public List<PguiToggleButtonCtrl> StatBtn;

		public List<PguiToggleButtonCtrl> TypeBtn;

		public List<PguiToggleButtonCtrl> Stat2Btn;

		public PguiOpenWindowCtrl baseWindow;

		public PguiToggleButtonCtrl CatFriends;

		public PguiToggleButtonCtrl CatPhoto;
	}

	public class SortTarget
	{
		public SortTarget()
		{
		}

		public SortTarget(RecommendedDeckWindowCtrl.SortTarget st)
		{
			this.charaList = ((st == null || st.charaList == null) ? new List<CharaPackData>() : new List<CharaPackData>(st.charaList));
			this.deckCharaList = ((st == null || st.deckCharaList == null) ? new List<int>() : new List<int>(st.deckCharaList));
			this.photoList = ((st == null || st.photoList == null) ? new List<PhotoPackData>() : new List<PhotoPackData>(st.photoList));
			this.deckPhotoList = new List<List<long>>();
			if (st != null && st.charaList != null)
			{
				foreach (List<long> list in st.deckPhotoList)
				{
					this.deckPhotoList.Add((list == null) ? new List<long>() : new List<long>(list));
				}
			}
		}

		public List<CharaPackData> charaList;

		public List<int> deckCharaList;

		public List<PhotoPackData> photoList;

		public List<List<long>> deckPhotoList;
	}

	public delegate RecommendedDeckWindowCtrl.SortTarget FuncGetTarget();

	public delegate void FuncDisideTarget(RecommendedDeckWindowCtrl.SortTarget result);

	public delegate int FuncGetQuest();

	public class RegisterData
	{
		public PguiButtonCtrl filterButton;

		public RecommendedDeckWindowCtrl.FuncGetTarget funcGetTargetBaseList;

		public RecommendedDeckWindowCtrl.FuncDisideTarget funcDisideTarget;

		public RecommendedDeckWindowCtrl.FuncGetQuest questOneId;
	}

	public class RegisterSaveData
	{
		public void SolutionList(ref RecommendedDeckWindowCtrl.SortTarget target)
		{
			if (this.isFriends)
			{
				List<CharaPackData> list1 = new List<CharaPackData>();
				List<CharaPackData> list2 = new List<CharaPackData>();
				List<CharaPackData> list3 = new List<CharaPackData>();
				foreach (object obj in Enum.GetValues(typeof(CharaDef.AttributeMask)))
				{
					CharaDef.AttributeMask attributeMask = (CharaDef.AttributeMask)obj;
					CharaDef.AttributeType at = CharaDef.AttributeMask2Type(attributeMask);
					List<CharaPackData> list = target.charaList.FindAll((CharaPackData itm) => itm.staticData.baseData.attribute == at);
					if ((this.attrMask & attributeMask) == (CharaDef.AttributeMask)0)
					{
						int i = 0;
						while (i < SceneBattle.attrMatch.GetLength(0))
						{
							if ((attributeMask & SceneBattle.attrMatch[i, 0]) != (CharaDef.AttributeMask)0)
							{
								if ((this.attrMask & SceneBattle.attrMatch[i, 1]) == (CharaDef.AttributeMask)0)
								{
									list2.AddRange(list);
									break;
								}
								list3.AddRange(list);
								break;
							}
							else
							{
								i++;
							}
						}
					}
					else
					{
						list1.AddRange(list);
					}
				}
				list3.AddRange(target.charaList.FindAll((CharaPackData itm) => !list1.Contains(itm) && !list2.Contains(itm) && !list3.Contains(itm)));
				list1.Sort(new Comparison<CharaPackData>(this.SortList));
				list1.Reverse();
				list2.Sort(new Comparison<CharaPackData>(this.SortList));
				list2.Reverse();
				list3.Sort(new Comparison<CharaPackData>(this.SortList));
				list3.Reverse();
				for (int j = 0; j < target.deckCharaList.Count; j++)
				{
					if (target.deckCharaList[j] != -1)
					{
						if (list1.Count > 0)
						{
							target.deckCharaList[j] = list1[0].id;
							list1.RemoveAt(0);
						}
						else if (list2.Count > 0)
						{
							target.deckCharaList[j] = list2[0].id;
							list2.RemoveAt(0);
						}
						else if (list3.Count > 0)
						{
							target.deckCharaList[j] = list3[0].id;
							list3.RemoveAt(0);
						}
						else
						{
							target.deckCharaList[j] = 0;
						}
						int id = target.deckCharaList[j];
						CharaPackData cpd = target.charaList.Find((CharaPackData itm) => itm.id == id);
						if (cpd != null)
						{
							list1.RemoveAll((CharaPackData itm) => DataManager.DmChara.CheckSameChara(itm.id, cpd.id));
							list2.RemoveAll((CharaPackData itm) => DataManager.DmChara.CheckSameChara(itm.id, cpd.id));
							list3.RemoveAll((CharaPackData itm) => DataManager.DmChara.CheckSameChara(itm.id, cpd.id));
						}
					}
				}
			}
			if (this.isPhoto)
			{
				List<PhotoPackData> list6 = new List<PhotoPackData>();
				List<PhotoPackData> list7 = new List<PhotoPackData>();
				foreach (PhotoPackData photoPackData in target.photoList)
				{
					if (photoPackData != null && !photoPackData.IsInvalid())
					{
						if (this.photoType == PhotoDef.Type.PARAMETER)
						{
							if (photoPackData.staticData.baseData.type == PhotoDef.Type.PARAMETER)
							{
								list6.Add(photoPackData);
							}
							else if (photoPackData.staticData.baseData.type == PhotoDef.Type.ABILITY)
							{
								list7.Add(photoPackData);
							}
						}
						else if (this.photoType == PhotoDef.Type.ABILITY)
						{
							if (photoPackData.staticData.baseData.type == PhotoDef.Type.ABILITY)
							{
								list6.Add(photoPackData);
							}
							else if (photoPackData.staticData.baseData.type == PhotoDef.Type.PARAMETER)
							{
								list7.Add(photoPackData);
							}
						}
						else if (photoPackData.staticData.baseData.type == PhotoDef.Type.PARAMETER || photoPackData.staticData.baseData.type == PhotoDef.Type.ABILITY)
						{
							list6.Add(photoPackData);
						}
					}
				}
				int num = 0;
				for (;;)
				{
					bool flag = false;
					for (int k = 0; k < target.deckPhotoList.Count; k++)
					{
						if (num < target.deckPhotoList[k].Count)
						{
							flag = true;
							long num2 = 0L;
							int cid = ((k < target.deckCharaList.Count) ? target.deckCharaList[k] : 0);
							if (cid != -1)
							{
								CharaPackData chkChara = target.charaList.Find((CharaPackData itm) => itm.id == cid);
								if (chkChara != null && num < chkChara.dynamicData.PhotoPocket.Count && chkChara.dynamicData.PhotoPocket[num].Flag)
								{
									List<PhotoPackData> list4 = new List<PhotoPackData>(list6);
									List<PhotoPackData> list5 = new List<PhotoPackData>(list7);
									list4.RemoveAll((PhotoPackData itm) => itm.staticData.baseData.kizunaPhotoFlg && itm.staticData.baseData.id != chkChara.staticData.baseData.kizunaPhotoId);
									list5.RemoveAll((PhotoPackData itm) => itm.staticData.baseData.kizunaPhotoFlg && itm.staticData.baseData.id != chkChara.staticData.baseData.kizunaPhotoId);
									int num3 = 0;
									int num4 = 0;
									for (int l = 0; l < num; l++)
									{
										long pid = target.deckPhotoList[k][l];
										PhotoPackData ppd = target.photoList.Find((PhotoPackData itm) => itm.dataId == pid);
										if (ppd != null && !ppd.IsInvalid())
										{
											if (ppd.staticData.baseData.type == PhotoDef.Type.PARAMETER)
											{
												num3++;
											}
											else if (ppd.staticData.baseData.type == PhotoDef.Type.ABILITY)
											{
												num4++;
											}
											list4.RemoveAll((PhotoPackData itm) => itm.staticData.baseData.id == ppd.staticData.baseData.id);
											list5.RemoveAll((PhotoPackData itm) => itm.staticData.baseData.id == ppd.staticData.baseData.id);
										}
									}
									if (num3 >= 2)
									{
										list4.RemoveAll((PhotoPackData itm) => itm.staticData.baseData.type == PhotoDef.Type.PARAMETER);
										list5.RemoveAll((PhotoPackData itm) => itm.staticData.baseData.type == PhotoDef.Type.PARAMETER);
									}
									if (num4 >= 2)
									{
										list4.RemoveAll((PhotoPackData itm) => itm.staticData.baseData.type == PhotoDef.Type.ABILITY);
										list5.RemoveAll((PhotoPackData itm) => itm.staticData.baseData.type == PhotoDef.Type.ABILITY);
									}
									if (list4.Count > 0)
									{
										list4.Sort(new Comparison<PhotoPackData>(this.SortList));
										list4.Reverse();
										num2 = list4[0].dataId;
										list6.Remove(list4[0]);
									}
									else if (list5.Count > 0)
									{
										list5.Sort(new Comparison<PhotoPackData>(this.SortList));
										list5.Reverse();
										num2 = list5[0].dataId;
										list7.Remove(list5[0]);
									}
								}
								target.deckPhotoList[k][num] = num2;
							}
						}
					}
					if (!flag)
					{
						break;
					}
					num++;
				}
			}
		}

		private int SortList(CharaPackData a, CharaPackData b)
		{
			int num = 0;
			PrjUtil.ParamPreset paramPreset = PrjUtil.CalcParamByChara(a.dynamicData, null, null, null);
			PrjUtil.ParamPreset paramPreset2 = PrjUtil.CalcParamByChara(b.dynamicData, null, null, null);
			DataManagerKemoBoard.KemoBoardBonusParam kemoBoardBonusParam;
			if (!DataManager.DmKemoBoard.KemoBoardBonusParamMap.TryGetValue(a.staticData.baseData.attribute, out kemoBoardBonusParam))
			{
				kemoBoardBonusParam = new DataManagerKemoBoard.KemoBoardBonusParam(a.staticData.baseData.attribute);
			}
			DataManagerKemoBoard.KemoBoardBonusParam kemoBoardBonusParam2;
			if (!DataManager.DmKemoBoard.KemoBoardBonusParamMap.TryGetValue(b.staticData.baseData.attribute, out kemoBoardBonusParam2))
			{
				kemoBoardBonusParam2 = new DataManagerKemoBoard.KemoBoardBonusParam(b.staticData.baseData.attribute);
			}
			int num2 = paramPreset.totalParam + kemoBoardBonusParam.KemoStatus - (paramPreset2.totalParam + kemoBoardBonusParam2.KemoStatus);
			int num3 = paramPreset.hp + kemoBoardBonusParam.Hp - (paramPreset2.hp + kemoBoardBonusParam2.Hp);
			int num4 = paramPreset.atk + kemoBoardBonusParam.Attack - (paramPreset2.atk + kemoBoardBonusParam2.Attack);
			int num5 = paramPreset.def + kemoBoardBonusParam.Difence - (paramPreset2.def + kemoBoardBonusParam2.Difence);
			switch (this.sortType)
			{
			case RecommendedDeckWindowCtrl.SortType.TOTAL:
				num = num2;
				break;
			case RecommendedDeckWindowCtrl.SortType.HP:
				num = num3;
				break;
			case RecommendedDeckWindowCtrl.SortType.ATK:
				num = num4;
				break;
			case RecommendedDeckWindowCtrl.SortType.DEF:
				num = num5;
				break;
			}
			if (num == 0 && (num = num2) == 0 && (num = num4) == 0 && (num = num3) == 0)
			{
				num = num5 ?? (a.id - b.id);
			}
			return num;
		}

		private int SortList(PhotoPackData a, PhotoPackData b)
		{
			int num = 0;
			PrjUtil.ParamPreset paramPreset = PrjUtil.CalcParamByPhoto(a);
			PrjUtil.ParamPreset paramPreset2 = PrjUtil.CalcParamByPhoto(b);
			switch (this.sort2Type)
			{
			case RecommendedDeckWindowCtrl.SortType.TOTAL:
				num = paramPreset.totalParam - paramPreset2.totalParam;
				break;
			case RecommendedDeckWindowCtrl.SortType.HP:
				num = paramPreset.hp - paramPreset2.hp;
				break;
			case RecommendedDeckWindowCtrl.SortType.ATK:
				num = paramPreset.atk - paramPreset2.atk;
				break;
			case RecommendedDeckWindowCtrl.SortType.DEF:
				num = paramPreset.def - paramPreset2.def;
				break;
			}
			if (num == 0 && (num = paramPreset.totalParam - paramPreset2.totalParam) == 0 && (num = paramPreset.atk - paramPreset2.atk) == 0 && (num = paramPreset.hp - paramPreset2.hp) == 0)
			{
				num = (paramPreset.def - paramPreset2.def) ?? (a.staticData.baseData.id - b.staticData.baseData.id);
			}
			return num;
		}

		public RecommendedDeckWindowCtrl.SortType sortType;

		public CharaDef.AttributeMask attrMask;

		public RecommendedDeckWindowCtrl.SortType sort2Type;

		public PhotoDef.Type photoType;

		public bool isFriends = true;

		public bool isPhoto = true;
	}
}
