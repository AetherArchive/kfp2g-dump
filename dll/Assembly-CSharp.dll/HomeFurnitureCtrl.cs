using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.uGUI;
using UnityEngine;

// Token: 0x0200014F RID: 335
public class HomeFurnitureCtrl : MonoBehaviour
{
	// Token: 0x17000378 RID: 888
	// (get) Token: 0x060012C3 RID: 4803 RVA: 0x000E3880 File Offset: 0x000E1A80
	public bool isActive
	{
		get
		{
			return this.isFuniture;
		}
	}

	// Token: 0x17000379 RID: 889
	// (get) Token: 0x060012C4 RID: 4804 RVA: 0x000E3888 File Offset: 0x000E1A88
	public bool isView
	{
		get
		{
			return this.isFunitureView;
		}
	}

	// Token: 0x1700037A RID: 890
	// (get) Token: 0x060012C5 RID: 4805 RVA: 0x000E3890 File Offset: 0x000E1A90
	// (set) Token: 0x060012C6 RID: 4806 RVA: 0x000E3898 File Offset: 0x000E1A98
	public bool isSetup { get; private set; }

	// Token: 0x060012C7 RID: 4807 RVA: 0x000E38A4 File Offset: 0x000E1AA4
	public void Init(GameObject stage, Transform baseTr, Transform winTr)
	{
		this.fieldStage = stage;
		this.infoWindow = winTr.Find("Window_FurnitureInfo").GetComponent<PguiOpenWindowCtrl>();
		this.infoIcon = Object.Instantiate<GameObject>((GameObject)Resources.Load("CmnIconFrame/GUI/Prefab/Icon_Furniture_mini"));
		this.infoIcon.transform.SetParent(this.infoWindow.m_UserInfoContent, false);
		Object.Destroy(this.infoIcon.transform.Find("Current").gameObject);
		Object.Destroy(this.infoIcon.transform.Find("Badge").gameObject);
		Object.Destroy(this.infoIcon.transform.Find("Remove").gameObject);
		Object.Destroy(this.infoIcon.transform.Find("Mark_FriendsAction").gameObject);
		Object.Destroy(this.infoIcon.transform.Find("Icon_Chara_Interior").gameObject);
		this.effectWindow = winTr.Find("Window_FurnitureEffect").GetComponent<PguiOpenWindowCtrl>();
		this.furnitureWindow = baseTr.Find("Mode_Funiture/Window_Funiture").GetComponent<SimpleAnimation>();
		this.furnitureButton = baseTr.Find("Mode_Funiture/BtnAll").GetComponent<SimpleAnimation>();
		this.furnitureWindowBack = this.furnitureWindow.transform.Find("Mode_Title/Btn_Back").GetComponent<PguiButtonCtrl>();
		this.furnitureWindowBack.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickFunitureReturn), PguiButtonCtrl.SoundType.DEFAULT);
		Transform transform = this.furnitureButton.transform.Find("Btn_View");
		this.furnitureWindowView = ((transform == null) ? null : transform.GetComponent<PguiButtonCtrl>());
		if (this.furnitureWindowView != null)
		{
			this.furnitureWindowView.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickFunitureView), PguiButtonCtrl.SoundType.DEFAULT);
		}
		transform = this.furnitureButton.transform.Find("Btn_Camera");
		if (transform != null)
		{
			transform.gameObject.SetActive(false);
		}
		this.numOwn = this.furnitureWindow.transform.Find("Txt_Own").GetComponent<PguiTextCtrl>();
		this.furnitureWindowEffect = this.furnitureWindow.transform.Find("Btn_Question").GetComponent<PguiButtonCtrl>();
		this.furnitureWindowEffect.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickFunitureEffect), PguiButtonCtrl.SoundType.DEFAULT);
		this.placeMap = new List<HomePlacementStatic>();
		this.placeData = new Dictionary<HomePlacementStatic, KeyValuePair<HomeFurnitureStatic, GameObject>>();
		this.userData = new List<HomeFurniturePackData>();
		this.furnitureMap = null;
		this.furnitureNew = new List<int>();
		this.userMap = new Dictionary<HomeFurnitureStatic.Category, List<HomeFurniturePackData>>();
		this.placeBtn = new Dictionary<PguiToggleButtonCtrl, HomePlacementStatic>();
		this.iconBtn = new Dictionary<GameObject, HomeFurniturePackData>();
		this.categoryScroll = this.furnitureWindow.transform.Find("Category/ScrollViewAll/ScrollView").GetComponent<ReuseScroll>();
		this.categoryScroll.InitForce();
		ReuseScroll reuseScroll = this.categoryScroll;
		reuseScroll.onStartItem = (Action<int, GameObject>)Delegate.Combine(reuseScroll.onStartItem, new Action<int, GameObject>(this.SetupFurnitureCategory));
		ReuseScroll reuseScroll2 = this.categoryScroll;
		reuseScroll2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(reuseScroll2.onUpdateItem, new Action<int, GameObject>(this.UpdateFurnitureCategory));
		this.categoryScroll.Setup(0, 0);
		this.itemScroll = this.furnitureWindow.transform.Find("IconAll/ScrollViewAll/ScrollView").GetComponent<ReuseScroll>();
		this.itemScroll.InitForce();
		ReuseScroll reuseScroll3 = this.itemScroll;
		reuseScroll3.onStartItem = (Action<int, GameObject>)Delegate.Combine(reuseScroll3.onStartItem, new Action<int, GameObject>(this.SetupFurnitureIcon));
		ReuseScroll reuseScroll4 = this.itemScroll;
		reuseScroll4.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(reuseScroll4.onUpdateItem, new Action<int, GameObject>(this.UpdateFurnitureIcon));
		this.itemScroll.Setup(0, 0);
		this.placeId = 0;
		this.categoryId = HomeFurnitureStatic.Category.INVALID;
		this.isSetup = false;
	}

	// Token: 0x060012C8 RID: 4808 RVA: 0x000E3C68 File Offset: 0x000E1E68
	public void Setup(List<HomeFurnitureMapping> hfm, List<int> fn)
	{
		this.isSetup = false;
		this.furnitureMap = hfm;
		this.furnitureNew = fn;
		this.isFuniture = false;
		this.isFunitureView = false;
		this.isFunitureEnd = false;
		this.placeMap = new List<HomePlacementStatic>(DataManager.DmHome.GetHomePlacementStaticMap().Values);
		this.placeMap.Sort(new Comparison<HomePlacementStatic>(this.SortPlace));
		this.placeData = new Dictionary<HomePlacementStatic, KeyValuePair<HomeFurnitureStatic, GameObject>>();
		foreach (HomePlacementStatic homePlacementStatic in this.placeMap)
		{
			this.placeData.Add(homePlacementStatic, new KeyValuePair<HomeFurnitureStatic, GameObject>(null, null));
		}
		this.userData = DataManager.DmHome.GetUserHomeFurnitureList();
		this.userMap = new Dictionary<HomeFurnitureStatic.Category, List<HomeFurniturePackData>>();
		foreach (HomeFurniturePackData homeFurniturePackData in this.userData)
		{
			if (homeFurniturePackData.staticData.category != HomeFurnitureStatic.Category.INVALID)
			{
				if (!this.userMap.ContainsKey(homeFurniturePackData.staticData.category))
				{
					this.userMap.Add(homeFurniturePackData.staticData.category, new List<HomeFurniturePackData>());
				}
				this.userMap[homeFurniturePackData.staticData.category].Add(homeFurniturePackData);
			}
		}
		if (!this.userMap.ContainsKey(HomeFurnitureStatic.Category.ORNAMENT))
		{
			this.userMap.Add(HomeFurnitureStatic.Category.ORNAMENT, new List<HomeFurniturePackData>());
		}
		this.userMap[HomeFurnitureStatic.Category.ORNAMENT].Insert(0, null);
		if (!this.userMap.ContainsKey(HomeFurnitureStatic.Category.CARPET))
		{
			this.userMap.Add(HomeFurnitureStatic.Category.CARPET, new List<HomeFurniturePackData>());
		}
		this.userMap[HomeFurnitureStatic.Category.CARPET].Insert(0, null);
		if (!this.userMap.ContainsKey(HomeFurnitureStatic.Category.WINDOW))
		{
			this.userMap.Add(HomeFurnitureStatic.Category.WINDOW, new List<HomeFurniturePackData>());
		}
		this.userMap[HomeFurnitureStatic.Category.WINDOW].Insert(0, null);
		this.placeBtn = new Dictionary<PguiToggleButtonCtrl, HomePlacementStatic>();
		this.iconBtn = new Dictionary<GameObject, HomeFurniturePackData>();
		this.placeId = this.placeMap[0].id;
		this.categoryId = this.placeMap[0].enableFurnitureCategory;
		this.categoryScroll.Resize(this.placeMap.Count, 0);
		this.itemScroll.Resize(this.userMap.ContainsKey(this.categoryId) ? this.userMap[this.categoryId].Count : 0, 0);
		this.sumOwn = 0;
		foreach (List<HomeFurniturePackData> list in this.userMap.Values)
		{
			this.sumOwn += ((list.Count > 0 && list[0] == null) ? (list.Count - 1) : list.Count);
		}
		int num = (this.userMap.ContainsKey(this.categoryId) ? ((this.userMap[this.categoryId].Count > 0 && this.userMap[this.categoryId][0] == null) ? (this.userMap[this.categoryId].Count - 1) : this.userMap[this.categoryId].Count) : 0);
		this.numOwn.ReplaceTextByDefault("Param01", num.ToString() + "/" + this.sumOwn.ToString());
		HomeFurnitureCountData homeFurnitureCountData = DataManager.DmHome.GetHomeFurnitureCountData(this.sumOwn, false);
		HomeFurnitureCountData homeFurnitureCountData2 = DataManager.DmHome.GetHomeFurnitureCountData(this.sumOwn, true);
		string text = (homeFurnitureCountData.kizunaPointIncrease / 100).ToString();
		int num2 = homeFurnitureCountData.kizunaPointIncrease % 100;
		if (num2 > 0)
		{
			text = text + "." + (num2 / 10).ToString();
			if ((num2 %= 10) > 0)
			{
				text += num2.ToString();
			}
		}
		this.effectWindow.transform.Find("Base/Window/Txt_Effect").GetComponent<PguiTextCtrl>().text = "なかよしポイント +" + text + "%";
		this.effectWindow.transform.Find("Base/Window/Txt_NextEffect").GetComponent<PguiTextCtrl>().ReplaceTextByDefault("Param01", (homeFurnitureCountData2.furnitureCount - this.sumOwn).ToString());
		this.Update();
	}

	// Token: 0x060012C9 RID: 4809 RVA: 0x000E4118 File Offset: 0x000E2318
	private int SortPlace(HomePlacementStatic a, HomePlacementStatic b)
	{
		int num = a.sortPriority - b.sortPriority;
		if (num == 0)
		{
			num = a.id - b.id;
		}
		return num;
	}

	// Token: 0x060012CA RID: 4810 RVA: 0x000E4148 File Offset: 0x000E2348
	public void TearDown()
	{
		this.isSetup = false;
		if (this.isFunitureView)
		{
			this.OnClickFunitureView(null);
		}
		if (this.isFuniture)
		{
			this.OnClickFunitureReturn(null);
		}
		this.isFunitureEnd = false;
		this.placeId = 0;
		this.categoryId = HomeFurnitureStatic.Category.INVALID;
		this.userData = new List<HomeFurniturePackData>();
		this.furnitureMap = null;
		this.furnitureNew = new List<int>();
		this.userMap = new Dictionary<HomeFurnitureStatic.Category, List<HomeFurniturePackData>>();
		this.placeBtn = new Dictionary<PguiToggleButtonCtrl, HomePlacementStatic>();
		this.iconBtn = new Dictionary<GameObject, HomeFurniturePackData>();
		this.categoryScroll.Resize(0, 0);
		this.itemScroll.Resize(0, 0);
		foreach (HomePlacementStatic homePlacementStatic in new List<HomePlacementStatic>(this.placeData.Keys))
		{
			if (this.placeData[homePlacementStatic].Key != null)
			{
				this.destroyModel(this.placeData[homePlacementStatic].Key, this.placeData[homePlacementStatic].Value);
				this.placeData[homePlacementStatic] = new KeyValuePair<HomeFurnitureStatic, GameObject>(null, null);
			}
		}
		this.infoIcon.transform.Find("Texture_Item").GetComponent<PguiRawImageCtrl>().SetTexture(null, true);
	}

	// Token: 0x060012CB RID: 4811 RVA: 0x000E42A8 File Offset: 0x000E24A8
	private void updateFunitureMap()
	{
		if (this.furnitureMap != null)
		{
			List<HomeFurnitureMapping> list = new List<HomeFurnitureMapping>(this.furnitureMap);
			using (List<HomeFurnitureMapping>.Enumerator enumerator = DataManager.DmHome.GetUserHomeeFurnitureMappingList().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					HomeFurnitureMapping org = enumerator.Current;
					HomeFurnitureMapping homeFurnitureMapping = list.Find((HomeFurnitureMapping itm) => itm.placementId == org.placementId);
					if (homeFurnitureMapping == null)
					{
						if (org.furnitureId != 0)
						{
							list.Add(new HomeFurnitureMapping
							{
								placementId = org.placementId,
								furnitureId = 0
							});
						}
					}
					else if (org.furnitureId == homeFurnitureMapping.furnitureId)
					{
						list.Remove(homeFurnitureMapping);
					}
				}
			}
			if (list.Count > 0)
			{
				DataManager.DmHome.RequestActionPutFurniture(list);
			}
		}
	}

	// Token: 0x060012CC RID: 4812 RVA: 0x000E4394 File Offset: 0x000E2594
	private void destroyModel(HomeFurnitureStatic hfs, GameObject obj)
	{
		string modelFileName = hfs.modelFileName;
		string text = (string.IsNullOrEmpty(modelFileName) ? hfs.photoTexturePath : null);
		if (obj == null)
		{
			AssetManager.UnloadAssetData(modelFileName, AssetManager.OWNER.HomeStage);
		}
		else
		{
			obj.transform.SetParent(null, true);
			Object.Destroy(obj);
		}
		if (!string.IsNullOrEmpty(text))
		{
			AssetManager.UnloadAssetData(text, AssetManager.OWNER.HomeStage);
		}
		PrjUtil.ReleaseMemory(PrjUtil.UnloadUnused / 50);
	}

	// Token: 0x060012CD RID: 4813 RVA: 0x000E43FC File Offset: 0x000E25FC
	public GameObject GetFurnitureModel(int place)
	{
		foreach (HomePlacementStatic homePlacementStatic in this.placeData.Keys)
		{
			if (homePlacementStatic.id == place)
			{
				return this.placeData[homePlacementStatic].Value;
			}
		}
		return null;
	}

	// Token: 0x060012CE RID: 4814 RVA: 0x000E4470 File Offset: 0x000E2670
	public GameObject GetFurnitureModel(HomeFurnitureStatic.Category cat)
	{
		HomePlacementStatic homePlacementStatic = this.placeMap.Find((HomePlacementStatic itm) => itm.enableFurnitureCategory == cat);
		if (homePlacementStatic != null)
		{
			return this.GetFurnitureModel(homePlacementStatic.id);
		}
		return null;
	}

	// Token: 0x060012CF RID: 4815 RVA: 0x000E44B3 File Offset: 0x000E26B3
	public int GetPlaceId()
	{
		if (!this.isActive)
		{
			return 0;
		}
		return this.placeId;
	}

	// Token: 0x060012D0 RID: 4816 RVA: 0x000E44C8 File Offset: 0x000E26C8
	public bool isBadge()
	{
		bool flag = false;
		if (this.isSetup && this.userData != null && this.furnitureNew != null)
		{
			foreach (HomeFurniturePackData homeFurniturePackData in this.userData)
			{
				if (!DataManager.DmItem.GetOldItemIdList().Contains(homeFurniturePackData.id) && !this.furnitureNew.Contains(homeFurniturePackData.id))
				{
					flag = true;
					break;
				}
			}
		}
		return flag;
	}

	// Token: 0x060012D1 RID: 4817 RVA: 0x000E4560 File Offset: 0x000E2760
	private void SetupFurnitureCategory(int index, GameObject go)
	{
		go.GetComponent<PguiToggleButtonCtrl>().AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickCategory));
		this.UpdateFurnitureCategory(index, go);
	}

	// Token: 0x060012D2 RID: 4818 RVA: 0x000E4584 File Offset: 0x000E2784
	private void UpdateFurnitureCategory(int index, GameObject go)
	{
		if (index < 0 || index >= this.placeMap.Count)
		{
			return;
		}
		go.transform.Find("BaseImage/Txt").GetComponent<PguiTextCtrl>().text = this.placeMap[index].name;
		PguiToggleButtonCtrl component = go.GetComponent<PguiToggleButtonCtrl>();
		this.placeBtn[component] = this.placeMap[index];
		component.SetToggleIndex((this.placeId == this.placeMap[index].id) ? 1 : 0);
		HomeFurnitureStatic.Category category = this.placeMap[index].enableFurnitureCategory;
		if (this.userMap.ContainsKey(category))
		{
			foreach (HomeFurniturePackData homeFurniturePackData in this.userMap[category])
			{
				if (homeFurniturePackData != null && !DataManager.DmItem.GetOldItemIdList().Contains(homeFurniturePackData.id) && !this.furnitureNew.Contains(homeFurniturePackData.id))
				{
					category = HomeFurnitureStatic.Category.INVALID;
					break;
				}
			}
		}
		Transform transform = go.transform.Find("BaseImage/Badge");
		if (transform != null)
		{
			transform.gameObject.SetActive(category == HomeFurnitureStatic.Category.INVALID);
		}
	}

	// Token: 0x060012D3 RID: 4819 RVA: 0x000E46D4 File Offset: 0x000E28D4
	private void SetupFurnitureIcon(int index, GameObject go)
	{
		if (go.GetComponent<PguiTouchTrigger>() == null)
		{
			go.AddComponent<PguiTouchTrigger>().AddListener(delegate
			{
				this.ClickIcon(go);
			}, delegate
			{
				this.ClickLongIcon(go);
			}, null, null, null);
		}
		this.UpdateFurnitureIcon(index, go);
	}

	// Token: 0x060012D4 RID: 4820 RVA: 0x000E4740 File Offset: 0x000E2940
	private void UpdateFurnitureIcon(int index, GameObject go)
	{
		HomeFurniturePackData hfpd = ((this.userMap.ContainsKey(this.categoryId) && this.userMap[this.categoryId].Count > index) ? this.userMap[this.categoryId][index] : null);
		this.iconBtn[go] = hfpd;
		PguiRawImageCtrl pguiRawImageCtrl = go.transform.Find("Texture_Item").GetComponent<PguiRawImageCtrl>();
		GameObject gameObject = go.transform.Find("Texture_Photo").gameObject;
		pguiRawImageCtrl.gameObject.SetActive(true);
		gameObject.SetActive(false);
		if (hfpd == null)
		{
			pguiRawImageCtrl.SetTexture(null, true);
		}
		else
		{
			string text = hfpd.staticData.GetIconName();
			if (string.IsNullOrEmpty(hfpd.staticData.modelFileName) && !string.IsNullOrEmpty(hfpd.staticData.photoTexturePath))
			{
				text = hfpd.staticData.photoTexturePath.Replace("Card_Photo", "Icon_Photo").Replace("card_photo", "icon_photo");
				pguiRawImageCtrl.gameObject.SetActive(false);
				gameObject.SetActive(true);
				pguiRawImageCtrl = gameObject.GetComponent<PguiRawImageCtrl>();
			}
			pguiRawImageCtrl.SetRawImage(text, true, true, null);
		}
		Transform transform = go.transform.Find("Badge");
		if (transform != null)
		{
			transform.gameObject.SetActive(hfpd != null && !DataManager.DmItem.GetOldItemIdList().Contains(hfpd.id) && !this.furnitureNew.Contains(hfpd.id));
		}
		Transform transform2 = go.transform.Find("Current");
		if (transform2 != null)
		{
			transform2.gameObject.SetActive(hfpd != null && this.furnitureMap.Find((HomeFurnitureMapping itm) => itm.placementId == this.placeId && itm.furnitureId == hfpd.id) != null);
		}
		Transform transform3 = go.transform.Find("Remove");
		if (transform3 != null)
		{
			transform3.gameObject.SetActive(hfpd == null);
		}
		Transform transform4 = go.transform.Find("Mark_FriendsAction");
		if (transform4 != null)
		{
			transform4.gameObject.SetActive(false);
		}
		Transform transform5 = go.transform.Find("Icon_Chara_Interior");
		if (transform5 != null)
		{
			transform5.gameObject.SetActive(false);
		}
	}

	// Token: 0x060012D5 RID: 4821 RVA: 0x000E49D8 File Offset: 0x000E2BD8
	private void ClickIcon(GameObject go)
	{
		if (this.furnitureMap == null)
		{
			return;
		}
		HomePlacementStatic homePlacementStatic = this.placeMap.Find((HomePlacementStatic itm) => itm.id == this.placeId);
		if (homePlacementStatic == null)
		{
			return;
		}
		if (!this.iconBtn.ContainsKey(go))
		{
			return;
		}
		HomeFurniturePackData hfpd = this.iconBtn[go];
		if (hfpd != null && homePlacementStatic.enableFurnitureCategory != hfpd.staticData.category)
		{
			return;
		}
		HomeFurnitureMapping hfm = ((hfpd == null) ? null : this.furnitureMap.Find((HomeFurnitureMapping itm) => itm.furnitureId == hfpd.id));
		if (hfpd != null && !DataManager.DmItem.GetOldItemIdList().Contains(hfpd.id) && !this.furnitureNew.Contains(hfpd.id))
		{
			this.furnitureNew.Add(hfpd.id);
			Transform transform = go.transform.Find("Badge");
			if (transform != null)
			{
				transform.gameObject.SetActive(hfpd != null && !DataManager.DmItem.GetOldItemIdList().Contains(hfpd.id) && !this.furnitureNew.Contains(hfpd.id));
			}
			this.categoryScroll.Refresh();
			base.StartCoroutine(this.UpdateNewBadge());
		}
		if (hfm == null)
		{
			this.furnitureMap.RemoveAll((HomeFurnitureMapping itm) => itm.placementId == this.placeId);
			hfm = new HomeFurnitureMapping();
			hfm.furnitureId = ((hfpd == null) ? 0 : hfpd.id);
			hfm.placementId = this.placeId;
			this.furnitureMap.Add(hfm);
			SoundManager.Play("prd_se_home_furniture_change", false, false);
		}
		else if (hfm.placementId != this.placeId)
		{
			HomeFurnitureMapping homeFurnitureMapping = this.furnitureMap.Find((HomeFurnitureMapping itm) => itm.placementId == this.placeId);
			this.furnitureMap.RemoveAll((HomeFurnitureMapping itm) => itm.furnitureId == hfm.furnitureId);
			this.furnitureMap.RemoveAll((HomeFurnitureMapping itm) => itm.placementId == hfm.placementId);
			this.furnitureMap.RemoveAll((HomeFurnitureMapping itm) => itm.placementId == this.placeId);
			if (homeFurnitureMapping != null)
			{
				homeFurnitureMapping.placementId = hfm.placementId;
				this.furnitureMap.Add(homeFurnitureMapping);
			}
			hfm.placementId = this.placeId;
			this.furnitureMap.Add(hfm);
			SoundManager.Play("prd_se_home_furniture_change", false, false);
		}
		this.updateFunitureMap();
		Predicate<HomeFurnitureMapping> <>9__7;
		foreach (GameObject gameObject in this.iconBtn.Keys)
		{
			Transform transform2 = gameObject.transform.Find("Current");
			if (!(transform2 == null))
			{
				hfpd = this.iconBtn[gameObject];
				GameObject gameObject2 = transform2.gameObject;
				bool flag;
				if (hfpd != null)
				{
					List<HomeFurnitureMapping> list = this.furnitureMap;
					Predicate<HomeFurnitureMapping> predicate;
					if ((predicate = <>9__7) == null)
					{
						predicate = (<>9__7 = (HomeFurnitureMapping itm) => itm.placementId == this.placeId && itm.furnitureId == hfpd.id);
					}
					flag = list.Find(predicate) != null;
				}
				else
				{
					flag = false;
				}
				gameObject2.SetActive(flag);
			}
		}
	}

	// Token: 0x060012D6 RID: 4822 RVA: 0x000E4D64 File Offset: 0x000E2F64
	private IEnumerator UpdateNewBadge()
	{
		if (this.furnitureNew.Count > 0)
		{
			DataManager.DmItem.RequestActionUpdateNewFlag(this.furnitureNew);
			do
			{
				yield return null;
			}
			while (DataManager.IsServerRequesting());
			this.furnitureNew = new List<int>();
		}
		yield break;
	}

	// Token: 0x060012D7 RID: 4823 RVA: 0x000E4D74 File Offset: 0x000E2F74
	private void ClickLongIcon(GameObject go)
	{
		if (this.iconBtn.ContainsKey(go))
		{
			HomeFurniturePackData homeFurniturePackData = this.iconBtn[go];
			if (homeFurniturePackData != null)
			{
				PguiRawImageCtrl pguiRawImageCtrl = this.infoIcon.transform.Find("Texture_Item").GetComponent<PguiRawImageCtrl>();
				GameObject gameObject = this.infoIcon.transform.Find("Texture_Photo").gameObject;
				string text = homeFurniturePackData.staticData.GetIconName();
				if (string.IsNullOrEmpty(homeFurniturePackData.staticData.modelFileName) && !string.IsNullOrEmpty(homeFurniturePackData.staticData.photoTexturePath))
				{
					text = homeFurniturePackData.staticData.photoTexturePath.Replace("Card_Photo", "Icon_Photo").Replace("card_photo", "icon_photo");
					pguiRawImageCtrl.gameObject.SetActive(false);
					gameObject.SetActive(true);
					pguiRawImageCtrl = gameObject.GetComponent<PguiRawImageCtrl>();
				}
				else
				{
					pguiRawImageCtrl.gameObject.SetActive(true);
					gameObject.SetActive(false);
				}
				pguiRawImageCtrl.SetRawImage(text, true, true, null);
				string text2 = homeFurniturePackData.staticData.GetName();
				if (text2 == null)
				{
					text2 = "名称";
				}
				string text3 = homeFurniturePackData.staticData.GetInfo();
				if (text3 == null)
				{
					text3 = "説明";
				}
				this.infoWindow.Setup(text2, text3, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, new PguiOpenWindowCtrl.Callback(this.ClickInfo), null, false);
				this.infoWindow.Open();
			}
		}
	}

	// Token: 0x060012D8 RID: 4824 RVA: 0x000E4ECD File Offset: 0x000E30CD
	private bool ClickInfo(int index)
	{
		return true;
	}

	// Token: 0x060012D9 RID: 4825 RVA: 0x000E4ED0 File Offset: 0x000E30D0
	private bool OnClickCategory(PguiToggleButtonCtrl toggle, int index)
	{
		if (!this.isFuniture)
		{
			return false;
		}
		if (this.placeBtn.ContainsKey(toggle) && this.placeId != this.placeBtn[toggle].id)
		{
			if (this.userMap.ContainsKey(this.categoryId))
			{
				foreach (HomeFurniturePackData homeFurniturePackData in this.userMap[this.categoryId])
				{
					if (homeFurniturePackData != null && !DataManager.DmItem.GetOldItemIdList().Contains(homeFurniturePackData.id) && !this.furnitureNew.Contains(homeFurniturePackData.id))
					{
						this.furnitureNew.Add(homeFurniturePackData.id);
						base.StartCoroutine(this.UpdateNewBadge());
					}
				}
				foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl in this.placeBtn.Keys)
				{
					if (this.placeBtn[pguiToggleButtonCtrl].enableFurnitureCategory == this.categoryId)
					{
						Transform transform = pguiToggleButtonCtrl.transform.Find("BaseImage/Badge");
						if (transform != null)
						{
							transform.gameObject.SetActive(false);
						}
					}
				}
			}
			this.placeId = this.placeBtn[toggle].id;
			this.categoryId = this.placeBtn[toggle].enableFurnitureCategory;
			int num = (this.userMap.ContainsKey(this.categoryId) ? ((this.userMap[this.categoryId].Count > 0 && this.userMap[this.categoryId][0] == null) ? (this.userMap[this.categoryId].Count - 1) : this.userMap[this.categoryId].Count) : 0);
			this.numOwn.ReplaceTextByDefault("Param01", num.ToString() + "/" + this.sumOwn.ToString());
			this.itemScroll.Resize(this.userMap.ContainsKey(this.categoryId) ? this.userMap[this.categoryId].Count : 0, 0);
			foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl2 in this.placeBtn.Keys)
			{
				pguiToggleButtonCtrl2.SetToggleIndex((pguiToggleButtonCtrl2 == toggle) ? 1 : 0);
			}
		}
		if (index <= 0)
		{
			SoundManager.Play("prd_se_click", false, false);
		}
		return false;
	}

	// Token: 0x060012DA RID: 4826 RVA: 0x000E51B4 File Offset: 0x000E33B4
	public void OnClickFunitureStart()
	{
		if (!this.isFuniture)
		{
			this.furnitureWindow.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
			this.furnitureButton.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
			this.isFuniture = true;
			this.isFunitureView = false;
		}
	}

	// Token: 0x060012DB RID: 4827 RVA: 0x000E51E6 File Offset: 0x000E33E6
	private void OnClickFunitureReturn(PguiButtonCtrl button)
	{
		if (this.isFuniture)
		{
			this.furnitureWindow.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, null);
			this.furnitureButton.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, null);
			this.isFuniture = false;
			this.isFunitureView = false;
			this.isFunitureEnd = true;
		}
	}

	// Token: 0x060012DC RID: 4828 RVA: 0x000E521F File Offset: 0x000E341F
	private void OnClickFunitureView(PguiButtonCtrl button)
	{
		if (this.isFuniture)
		{
			this.furnitureWindow.ExPlayAnimation(this.isFunitureView ? SimpleAnimation.ExPguiStatus.START : SimpleAnimation.ExPguiStatus.END, null);
			this.isFunitureView = !this.isFunitureView;
		}
	}

	// Token: 0x060012DD RID: 4829 RVA: 0x000E5250 File Offset: 0x000E3450
	private void OnClickFunitureEffect(PguiButtonCtrl button)
	{
		if (this.isFuniture)
		{
			this.effectWindow.Setup(null, null, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, new PguiOpenWindowCtrl.Callback(this.ClickInfo), null, false);
			this.effectWindow.Open();
		}
	}

	// Token: 0x060012DE RID: 4830 RVA: 0x000E5288 File Offset: 0x000E3488
	private void Update()
	{
		Dictionary<int, HomeFurnitureStatic> dictionary = new Dictionary<int, HomeFurnitureStatic>();
		if (this.furnitureMap != null)
		{
			using (List<HomeFurnitureMapping>.Enumerator enumerator = this.furnitureMap.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					HomeFurnitureMapping hfm = enumerator.Current;
					if (hfm.furnitureId != 0)
					{
						HomeFurniturePackData homeFurniturePackData = this.userData.Find((HomeFurniturePackData itm) => itm.id == hfm.furnitureId);
						if (homeFurniturePackData != null && this.placeMap.Find((HomePlacementStatic itm) => itm.id == hfm.placementId) != null)
						{
							dictionary.Add(hfm.placementId, homeFurniturePackData.staticData);
						}
					}
				}
			}
		}
		this.isSetup = true;
		foreach (HomePlacementStatic homePlacementStatic in new List<HomePlacementStatic>(this.placeData.Keys))
		{
			Transform transform = this.fieldStage.transform.Find(homePlacementStatic.locatorName);
			if (!(transform == null))
			{
				HomeFurnitureStatic homeFurnitureStatic;
				if (!dictionary.TryGetValue(homePlacementStatic.id, out homeFurnitureStatic))
				{
					homeFurnitureStatic = null;
				}
				if (this.placeData[homePlacementStatic].Key != null && (homeFurnitureStatic == null || this.placeData[homePlacementStatic].Key.id != homeFurnitureStatic.id))
				{
					this.destroyModel(this.placeData[homePlacementStatic].Key, this.placeData[homePlacementStatic].Value);
					this.placeData[homePlacementStatic] = new KeyValuePair<HomeFurnitureStatic, GameObject>(null, null);
				}
				if (homeFurnitureStatic != null && this.placeData[homePlacementStatic].Value == null)
				{
					string text = homeFurnitureStatic.modelFileName;
					string text2 = homeFurnitureStatic.photoTexturePath;
					if (string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2))
					{
						text = "room_obj_ornament_mdl";
					}
					else
					{
						text2 = null;
					}
					if (!string.IsNullOrEmpty(text))
					{
						text = "Stage/Furniture/" + text;
					}
					if (this.placeData[homePlacementStatic].Key == null)
					{
						if (!string.IsNullOrEmpty(text))
						{
							AssetManager.LoadAssetData(text, AssetManager.OWNER.HomeStage, 0, null);
							if (!string.IsNullOrEmpty(text2))
							{
								AssetManager.LoadAssetData(text2, AssetManager.OWNER.HomeStage, 0, null);
							}
							this.placeData[homePlacementStatic] = new KeyValuePair<HomeFurnitureStatic, GameObject>(homeFurnitureStatic, null);
						}
					}
					else if (AssetManager.IsLoadFinishAssetData(text) && (string.IsNullOrEmpty(text2) || AssetManager.IsLoadFinishAssetData(text2)))
					{
						GameObject gameObject = AssetManager.InstantiateAssetData(text, base.transform);
						if (gameObject != null)
						{
							gameObject.name = homePlacementStatic.id.ToString() + "_" + homeFurnitureStatic.id.ToString();
							gameObject.SetLayerRecursively(this.fieldStage.layer);
							gameObject.transform.localPosition = transform.localPosition;
							gameObject.transform.localRotation = transform.localRotation;
							gameObject.transform.localScale = transform.localScale;
							this.placeData[homePlacementStatic] = new KeyValuePair<HomeFurnitureStatic, GameObject>(homeFurnitureStatic, gameObject);
							if (!string.IsNullOrEmpty(text2))
							{
								Renderer componentInChildren = gameObject.GetComponentInChildren<Renderer>();
								if (componentInChildren != null)
								{
									Texture2D texture2D = AssetManager.GetAssetData(text2) as Texture2D;
									componentInChildren.material.SetTexture("_MainTex", texture2D);
								}
							}
							AssetManager.UnloadAssetData(text, AssetManager.OWNER.HomeStage);
						}
					}
					this.isSetup = false;
				}
			}
		}
		if (this.isFunitureEnd && !this.furnitureWindow.ExIsPlaying())
		{
			this.isFunitureEnd = false;
			CanvasManager.HdlCmnMenu.SetActiveMenu(true);
		}
		this.furnitureWindowBack.androidBackKeyTarget = this.isFuniture && !this.isFunitureView;
		if (this.furnitureWindowView != null)
		{
			this.furnitureWindowView.androidBackKeyTarget = this.isFuniture && this.isFunitureView;
		}
	}

	// Token: 0x04000F6C RID: 3948
	private GameObject fieldStage;

	// Token: 0x04000F6D RID: 3949
	private PguiOpenWindowCtrl infoWindow;

	// Token: 0x04000F6E RID: 3950
	private GameObject infoIcon;

	// Token: 0x04000F6F RID: 3951
	private PguiOpenWindowCtrl effectWindow;

	// Token: 0x04000F70 RID: 3952
	private SimpleAnimation furnitureWindow;

	// Token: 0x04000F71 RID: 3953
	private SimpleAnimation furnitureButton;

	// Token: 0x04000F72 RID: 3954
	private PguiButtonCtrl furnitureWindowBack;

	// Token: 0x04000F73 RID: 3955
	private PguiButtonCtrl furnitureWindowView;

	// Token: 0x04000F74 RID: 3956
	private PguiButtonCtrl furnitureWindowEffect;

	// Token: 0x04000F75 RID: 3957
	private PguiTextCtrl numOwn;

	// Token: 0x04000F76 RID: 3958
	private int sumOwn;

	// Token: 0x04000F77 RID: 3959
	private ReuseScroll categoryScroll;

	// Token: 0x04000F78 RID: 3960
	private ReuseScroll itemScroll;

	// Token: 0x04000F79 RID: 3961
	private bool isFuniture;

	// Token: 0x04000F7A RID: 3962
	private bool isFunitureView;

	// Token: 0x04000F7B RID: 3963
	private bool isFunitureEnd;

	// Token: 0x04000F7C RID: 3964
	private List<HomePlacementStatic> placeMap;

	// Token: 0x04000F7D RID: 3965
	private Dictionary<HomePlacementStatic, KeyValuePair<HomeFurnitureStatic, GameObject>> placeData;

	// Token: 0x04000F7E RID: 3966
	private List<HomeFurnitureMapping> furnitureMap;

	// Token: 0x04000F7F RID: 3967
	private List<int> furnitureNew;

	// Token: 0x04000F80 RID: 3968
	private List<HomeFurniturePackData> userData;

	// Token: 0x04000F81 RID: 3969
	private Dictionary<HomeFurnitureStatic.Category, List<HomeFurniturePackData>> userMap;

	// Token: 0x04000F82 RID: 3970
	private int placeId;

	// Token: 0x04000F83 RID: 3971
	private HomeFurnitureStatic.Category categoryId;

	// Token: 0x04000F84 RID: 3972
	private Dictionary<PguiToggleButtonCtrl, HomePlacementStatic> placeBtn;

	// Token: 0x04000F85 RID: 3973
	private Dictionary<GameObject, HomeFurniturePackData> iconBtn;
}
