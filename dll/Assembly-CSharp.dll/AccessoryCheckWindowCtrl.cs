using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000189 RID: 393
public class AccessoryCheckWindowCtrl : MonoBehaviour
{
	// Token: 0x06001A52 RID: 6738 RVA: 0x0015625E File Offset: 0x0015445E
	private IEnumerator IESetupOwnerSettingAfter(CharaPackData cpd, DataManagerCharaAccessory.Accessory accessory, UnityAction cb)
	{
		if (this.ownerSettingAfter == null)
		{
			yield break;
		}
		this.ieLoadCueSheet = SoundManager.LoadCueSheetWithDownload(cpd.staticData.cueSheetName);
		while (this.ieLoadCueSheet != null)
		{
			yield return null;
		}
		this.ownerSettingAfter.Setup(cpd, accessory);
		this.ownerSettingAfter.ow.Setup(null, accessory.AccessoryData.Name + "の持ち主が決定しました！", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, delegate(int index)
		{
			this.ownerSettingAfter.AEImage_Back.PlayAnimation(PguiAECtrl.AmimeType.END, null);
			this.ownerSettingAfter.AEImage_Front.PlayAnimation(PguiAECtrl.AmimeType.END, null);
			SoundManager.UnloadCueSheet(cpd.staticData.cueSheetName);
			return true;
		}, delegate
		{
			cb();
		}, false);
		this.ownerSettingAfter.ow.Open();
		yield break;
	}

	// Token: 0x06001A53 RID: 6739 RVA: 0x00156282 File Offset: 0x00154482
	private void Internal()
	{
		this.onDecideCb = null;
		this.onCancelCb = null;
		this.ieSetupOwnerSettingAfter = null;
		this.ieLoadCueSheet = null;
	}

	// Token: 0x06001A54 RID: 6740 RVA: 0x001562A0 File Offset: 0x001544A0
	private void InternalCheck(AccessoryCheckWindowCtrl.Check.Type type, List<DataManagerCharaAccessory.Accessory> list, UnityAction cb)
	{
		AccessoryCheckWindowCtrl.<>c__DisplayClass15_0 CS$<>8__locals1 = new AccessoryCheckWindowCtrl.<>c__DisplayClass15_0();
		CS$<>8__locals1.type = type;
		CS$<>8__locals1.<>4__this = this;
		this.Internal();
		base.gameObject.SetActive(true);
		this.check.FeedAccessoryList = new List<DataManagerCharaAccessory.Accessory>(list);
		this.check.Setup();
		this.check.ow.Setup(CS$<>8__locals1.<InternalCheck>g__GetTitleString|0(), CS$<>8__locals1.<InternalCheck>g__GetMessageString|1(), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CANCEL_OK), true, delegate(int index)
		{
			if (index == 1)
			{
				UnityAction unityAction = CS$<>8__locals1.<>4__this.onDecideCb;
				if (unityAction != null)
				{
					unityAction();
				}
			}
			return true;
		}, null, false);
		this.check.ow.Open();
		this.onDecideCb = cb;
	}

	// Token: 0x06001A55 RID: 6741 RVA: 0x00156338 File Offset: 0x00154538
	public void Init(AccessoryCheckWindowCtrl.Type type)
	{
		switch (type)
		{
		case AccessoryCheckWindowCtrl.Type.Release:
		{
			GameObject gameObject = Resources.Load("SceneAccessory/GUI/Prefab/GUI_AccessoryOpen_Window") as GameObject;
			this.release = new AccessoryCheckWindowCtrl.Release(Object.Instantiate<GameObject>(gameObject, base.transform).transform.Find("Window_AccessoryOpen"));
			return;
		}
		case AccessoryCheckWindowCtrl.Type.OwnerSetting:
		{
			GameObject gameObject2 = Resources.Load("SceneAccessory/GUI/Prefab/GUI_AccessorySet_Window") as GameObject;
			this.ownerSetting = new AccessoryCheckWindowCtrl.OwnerSetting(Object.Instantiate<GameObject>(gameObject2, base.transform).transform.Find("Window_AccessorySet"));
			this.ownerSettingAfter = new AccessoryCheckWindowCtrl.OwnerSettingAfter(Object.Instantiate<GameObject>(gameObject2, base.transform).transform.Find("Window_AccessorySet_After"));
			return;
		}
		case AccessoryCheckWindowCtrl.Type.Check:
		{
			GameObject gameObject3 = Resources.Load("SceneAccessory/GUI/Prefab/GUI_AccessoryGrow_Window") as GameObject;
			this.check = new AccessoryCheckWindowCtrl.Check(Object.Instantiate<GameObject>(gameObject3, base.transform).transform.Find("Window_AccessoryGrow"));
			this.check.ScrollView.InitForce();
			ReuseScroll scrollView = this.check.ScrollView;
			scrollView.onStartItem = (Action<int, GameObject>)Delegate.Combine(scrollView.onStartItem, new Action<int, GameObject>(delegate(int index, GameObject go)
			{
				for (int i = 0; i < AccessoryCheckWindowCtrl.Check.SCROLL_ITEM_NUN_H; i++)
				{
					IconAccessoryCtrl component = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Accessory, go.transform).GetComponent<IconAccessoryCtrl>();
					component.name = i.ToString();
					component.transform.localScale = new Vector3(0.75f, 0.75f, 1f);
				}
			}));
			ReuseScroll scrollView2 = this.check.ScrollView;
			scrollView2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scrollView2.onUpdateItem, new Action<int, GameObject>(delegate(int index, GameObject go)
			{
				for (int j = 0; j < AccessoryCheckWindowCtrl.Check.SCROLL_ITEM_NUN_H; j++)
				{
					int num = index * AccessoryCheckWindowCtrl.Check.SCROLL_ITEM_NUN_H + j;
					DataManagerCharaAccessory.Accessory accessory = null;
					if (num < this.check.FeedAccessoryList.Count)
					{
						accessory = this.check.FeedAccessoryList[num];
					}
					IconAccessoryCtrl component2 = go.transform.Find(string.Format("{0}", j)).GetComponent<IconAccessoryCtrl>();
					if (component2 != null)
					{
						component2.Setup(new IconAccessoryCtrl.SetupParam
						{
							acce = accessory,
							isEnableRaycast = false
						});
						CharaStaticData charaStaticData = null;
						if (AccessoryUtil.IsDecidedOwner(accessory))
						{
							charaStaticData = DataManager.DmChara.GetCharaStaticData(accessory.CharaId);
						}
						component2.DispIconCharaMini(charaStaticData != null, charaStaticData);
					}
				}
			}));
			this.check.ScrollView.Setup(10, 0);
			return;
		}
		default:
			return;
		}
	}

	// Token: 0x06001A56 RID: 6742 RVA: 0x001564B4 File Offset: 0x001546B4
	public void OpenRelease(UnityAction cb)
	{
		if (this.release == null)
		{
			return;
		}
		this.Internal();
		base.gameObject.SetActive(true);
		this.release.Setup();
		this.release.ow.Setup(null, null, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CANCEL_OK), true, delegate(int index)
		{
			if (index == 1)
			{
				UnityAction unityAction = this.onDecideCb;
				if (unityAction != null)
				{
					unityAction();
				}
			}
			return true;
		}, null, false);
		bool flag = AccessoryUtil.CanReleasedAccessory();
		this.release.ow.choiceR.GetComponent<PguiButtonCtrl>().SetActEnable(flag, false, false);
		this.release.ow.Open();
		this.onDecideCb = cb;
	}

	// Token: 0x06001A57 RID: 6743 RVA: 0x00156548 File Offset: 0x00154748
	public void OpenOwnerSetting(CharaPackData cpd, DataManagerCharaAccessory.Accessory accessory, UnityAction decideCb, UnityAction cancelCb)
	{
		if (this.ownerSetting == null)
		{
			return;
		}
		this.Internal();
		base.gameObject.SetActive(true);
		this.ownerSetting.Setup(cpd, accessory);
		this.ownerSetting.ow.Setup(null, null, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CANCEL_OK), true, delegate(int index)
		{
			if (index == 1)
			{
				UnityAction unityAction = this.onDecideCb;
				if (unityAction != null)
				{
					unityAction();
				}
			}
			else
			{
				UnityAction unityAction2 = this.onCancelCb;
				if (unityAction2 != null)
				{
					unityAction2();
				}
			}
			return true;
		}, null, false);
		this.ownerSetting.ow.Open();
		this.onDecideCb = decideCb;
		this.onCancelCb = cancelCb;
	}

	// Token: 0x06001A58 RID: 6744 RVA: 0x001565C3 File Offset: 0x001547C3
	public void OpenOwnerSettingAfter(CharaPackData cpd, DataManagerCharaAccessory.Accessory accessory, UnityAction cb)
	{
		if (this.ownerSettingAfter == null)
		{
			return;
		}
		this.Internal();
		base.gameObject.SetActive(true);
		this.ieSetupOwnerSettingAfter = this.IESetupOwnerSettingAfter(cpd, accessory, cb);
	}

	// Token: 0x06001A59 RID: 6745 RVA: 0x001565EF File Offset: 0x001547EF
	public void OpenGrowth(List<DataManagerCharaAccessory.Accessory> list, UnityAction cb)
	{
		if (this.check == null)
		{
			return;
		}
		this.InternalCheck(AccessoryCheckWindowCtrl.Check.Type.Growth, list, cb);
	}

	// Token: 0x06001A5A RID: 6746 RVA: 0x00156603 File Offset: 0x00154803
	public void OpenSale(List<DataManagerCharaAccessory.Accessory> list, UnityAction cb)
	{
		if (this.check == null)
		{
			return;
		}
		this.InternalCheck(AccessoryCheckWindowCtrl.Check.Type.Sale, list, cb);
	}

	// Token: 0x06001A5B RID: 6747 RVA: 0x00156617 File Offset: 0x00154817
	private void Update()
	{
		if (this.ieLoadCueSheet != null && !this.ieLoadCueSheet.MoveNext())
		{
			this.ieLoadCueSheet = null;
		}
		if (this.ieSetupOwnerSettingAfter != null && !this.ieSetupOwnerSettingAfter.MoveNext())
		{
			this.ieSetupOwnerSettingAfter = null;
		}
	}

	// Token: 0x04001426 RID: 5158
	private AccessoryCheckWindowCtrl.Check check;

	// Token: 0x04001427 RID: 5159
	private AccessoryCheckWindowCtrl.OwnerSetting ownerSetting;

	// Token: 0x04001428 RID: 5160
	private AccessoryCheckWindowCtrl.OwnerSettingAfter ownerSettingAfter;

	// Token: 0x04001429 RID: 5161
	private AccessoryCheckWindowCtrl.Release release;

	// Token: 0x0400142A RID: 5162
	private UnityAction onDecideCb;

	// Token: 0x0400142B RID: 5163
	private UnityAction onCancelCb;

	// Token: 0x0400142C RID: 5164
	private IEnumerator ieSetupOwnerSettingAfter;

	// Token: 0x0400142D RID: 5165
	private IEnumerator ieLoadCueSheet;

	// Token: 0x02000E67 RID: 3687
	public enum Type
	{
		// Token: 0x040052DC RID: 21212
		Invalid,
		// Token: 0x040052DD RID: 21213
		Release,
		// Token: 0x040052DE RID: 21214
		OwnerSetting,
		// Token: 0x040052DF RID: 21215
		Check
	}

	// Token: 0x02000E68 RID: 3688
	public class Release
	{
		// Token: 0x06004C62 RID: 19554 RVA: 0x0022CC6C File Offset: 0x0022AE6C
		public Release(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			Transform transform = baseTr.Find("Base/Window/ItemInfo");
			this.Icon_Tex = transform.Find("Icon_Tex").GetComponent<PguiRawImageCtrl>();
			this.Txt01 = transform.Find("Txt01").GetComponent<PguiTextCtrl>();
			this.Num_BeforeTxt = transform.Find("Img_Yaji/Num_BeforeTxt").GetComponent<PguiTextCtrl>();
			this.Num_AfterTxt = transform.Find("Img_Yaji/Num_AfterTxt").GetComponent<PguiTextCtrl>();
			this.Txt_Note = baseTr.Find("Base/Window/Txt_Note").GetComponent<PguiTextCtrl>();
			this.ow = baseTr.GetComponent<PguiOpenWindowCtrl>();
		}

		// Token: 0x06004C63 RID: 19555 RVA: 0x0022CD14 File Offset: 0x0022AF14
		public void Setup()
		{
			int needReleaseAccessoryItemNum = AccessoryUtil.GetNeedReleaseAccessoryItemNum();
			ItemData releaseAccessoryItemData = AccessoryUtil.GetReleaseAccessoryItemData();
			this.Icon_Tex.SetRawImage(releaseAccessoryItemData.staticData.GetIconName(), true, false, null);
			this.Txt01.text = releaseAccessoryItemData.staticData.GetName();
			bool flag = AccessoryUtil.CanReleasedAccessory();
			this.Num_BeforeTxt.text = (flag ? string.Format("{0}", releaseAccessoryItemData.num) : string.Format("{0}{1}{2}", PrjUtil.ColorRedStartTag, releaseAccessoryItemData.num, PrjUtil.ColorEndTag));
			this.Num_AfterTxt.text = (flag ? string.Format("{0}", releaseAccessoryItemData.num - needReleaseAccessoryItemNum) : "-");
			this.Txt_Note.gameObject.SetActive(!flag);
			this.Txt_Note.ReplaceTextByDefault("Param01", releaseAccessoryItemData.staticData.GetName());
		}

		// Token: 0x040052E0 RID: 21216
		public GameObject baseObj;

		// Token: 0x040052E1 RID: 21217
		public PguiRawImageCtrl Icon_Tex;

		// Token: 0x040052E2 RID: 21218
		public PguiTextCtrl Txt01;

		// Token: 0x040052E3 RID: 21219
		public PguiTextCtrl Num_BeforeTxt;

		// Token: 0x040052E4 RID: 21220
		public PguiTextCtrl Num_AfterTxt;

		// Token: 0x040052E5 RID: 21221
		public PguiTextCtrl Txt_Note;

		// Token: 0x040052E6 RID: 21222
		public PguiOpenWindowCtrl ow;
	}

	// Token: 0x02000E69 RID: 3689
	public class OwnerSetting
	{
		// Token: 0x06004C64 RID: 19556 RVA: 0x0022CE04 File Offset: 0x0022B004
		public OwnerSetting(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Txt_Verification = baseTr.Find("Base/Window/Txt_Verification").GetComponent<PguiTextCtrl>();
			this.Icon_Accessory_Before = baseTr.Find("Base/Window/Txt/Img_Yaji/Icon_Accessory_Before").GetComponent<IconAccessoryCtrl>();
			this.Icon_Accessory_After = baseTr.Find("Base/Window/Txt/Img_Yaji/Icon_Accessory_After").GetComponent<IconAccessoryCtrl>();
			this.ow = baseTr.GetComponent<PguiOpenWindowCtrl>();
		}

		// Token: 0x06004C65 RID: 19557 RVA: 0x0022CE74 File Offset: 0x0022B074
		public void Setup(CharaPackData cpd, DataManagerCharaAccessory.Accessory accessory)
		{
			this.Icon_Accessory_Before.Setup(new IconAccessoryCtrl.SetupParam
			{
				acce = accessory
			});
			this.Icon_Accessory_After.Setup(new IconAccessoryCtrl.SetupParam
			{
				acce = accessory
			});
			this.Icon_Accessory_After.DispIconCharaMini(true, cpd.staticData);
			this.Txt_Verification.ReplaceTextByDefault("Param01", accessory.AccessoryData.Name);
		}

		// Token: 0x040052E7 RID: 21223
		public GameObject baseObj;

		// Token: 0x040052E8 RID: 21224
		public IconAccessoryCtrl Icon_Accessory_Before;

		// Token: 0x040052E9 RID: 21225
		public IconAccessoryCtrl Icon_Accessory_After;

		// Token: 0x040052EA RID: 21226
		public PguiTextCtrl Txt_Verification;

		// Token: 0x040052EB RID: 21227
		public PguiOpenWindowCtrl ow;
	}

	// Token: 0x02000E6A RID: 3690
	public class OwnerSettingAfter
	{
		// Token: 0x06004C66 RID: 19558 RVA: 0x0022CEDC File Offset: 0x0022B0DC
		public OwnerSettingAfter(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Icon_Accessory = baseTr.Find("Base/Window/Icon_Accessory").GetComponent<IconAccessoryCtrl>();
			this.Icon_Chara = baseTr.Find("Base/Window/Icon_Chara").GetComponent<IconCharaCtrl>();
			this.AEImage_Back = baseTr.Find("Base/Window/AEImage_Back").GetComponent<PguiAECtrl>();
			this.AEImage_Front = baseTr.Find("Base/Window/AEImage_Front").GetComponent<PguiAECtrl>();
			this.ow = baseTr.GetComponent<PguiOpenWindowCtrl>();
		}

		// Token: 0x06004C67 RID: 19559 RVA: 0x0022CF60 File Offset: 0x0022B160
		public void Setup(CharaPackData cpd, DataManagerCharaAccessory.Accessory accessory)
		{
			this.Icon_Accessory.Setup(new IconAccessoryCtrl.SetupParam
			{
				acce = accessory
			});
			this.Icon_Chara.SetupPrm(new IconCharaCtrl.SetupParam
			{
				cpd = cpd,
				sortType = SortFilterDefine.SortType.INVALID
			});
			this.Icon_Chara.DispRanking();
			this.AEImage_Back.PlayAnimation(PguiAECtrl.AmimeType.START, delegate
			{
				this.AEImage_Back.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
				SoundManager.PlayVoice(cpd.staticData.cueSheetName, VOICE_TYPE.JOY01);
				SoundManager.Play("prd_se_accessory_ownership", false, false);
			});
			this.AEImage_Front.PlayAnimation(PguiAECtrl.AmimeType.START, delegate
			{
				this.AEImage_Front.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
			});
		}

		// Token: 0x040052EC RID: 21228
		public GameObject baseObj;

		// Token: 0x040052ED RID: 21229
		public IconAccessoryCtrl Icon_Accessory;

		// Token: 0x040052EE RID: 21230
		public IconCharaCtrl Icon_Chara;

		// Token: 0x040052EF RID: 21231
		public PguiAECtrl AEImage_Back;

		// Token: 0x040052F0 RID: 21232
		public PguiAECtrl AEImage_Front;

		// Token: 0x040052F1 RID: 21233
		public PguiOpenWindowCtrl ow;
	}

	// Token: 0x02000E6B RID: 3691
	public class Check
	{
		// Token: 0x17000B20 RID: 2848
		// (get) Token: 0x06004C69 RID: 19561 RVA: 0x0022CFFF File Offset: 0x0022B1FF
		// (set) Token: 0x06004C68 RID: 19560 RVA: 0x0022CFF6 File Offset: 0x0022B1F6
		public List<DataManagerCharaAccessory.Accessory> FeedAccessoryList { get; set; }

		// Token: 0x06004C6A RID: 19562 RVA: 0x0022D008 File Offset: 0x0022B208
		public Check(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Massage_Caution = baseTr.Find("Base/Window/Massage_Caution").GetComponent<PguiTextCtrl>();
			this.ScrollView = baseTr.Find("Base/Window/PhotoUseInfo/ScrollView_PhotoIconAll").GetComponent<ReuseScroll>();
			this.ow = baseTr.GetComponent<PguiOpenWindowCtrl>();
		}

		// Token: 0x06004C6B RID: 19563 RVA: 0x0022D060 File Offset: 0x0022B260
		public void Setup()
		{
			if (this.FeedAccessoryList == null)
			{
				return;
			}
			string text = string.Empty;
			bool flag = false;
			List<CharaPackData> list = new List<CharaPackData>(DataManager.DmChara.GetUserCharaMap().Values);
			using (List<DataManagerCharaAccessory.Accessory>.Enumerator enumerator = this.FeedAccessoryList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DataManagerCharaAccessory.Accessory e = enumerator.Current;
					if (list.Exists((CharaPackData item) => item.dynamicData.accessory == e))
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				text += "装備中";
			}
			bool flag2 = false;
			using (List<DataManagerCharaAccessory.Accessory>.Enumerator enumerator = this.FeedAccessoryList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Level != 1)
					{
						flag2 = true;
						break;
					}
				}
			}
			if (flag2)
			{
				if (text != string.Empty)
				{
					text += "/";
				}
				text += "強化済み";
			}
			bool flag3 = false;
			using (List<DataManagerCharaAccessory.Accessory>.Enumerator enumerator = this.FeedAccessoryList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (AccessoryUtil.IsDecidedOwner(enumerator.Current))
					{
						flag3 = true;
						break;
					}
				}
			}
			if (flag3)
			{
				if (text != string.Empty)
				{
					text += "/";
				}
				text += "持ち主が確定済み";
			}
			if (text != string.Empty)
			{
				text += "のおしゃれアクセが含まれています";
			}
			this.Massage_Caution.text = text;
			this.ScrollView.Resize((this.FeedAccessoryList.Count % AccessoryCheckWindowCtrl.Check.SCROLL_ITEM_NUN_H == 0) ? (this.FeedAccessoryList.Count / AccessoryCheckWindowCtrl.Check.SCROLL_ITEM_NUN_H) : (1 + this.FeedAccessoryList.Count / AccessoryCheckWindowCtrl.Check.SCROLL_ITEM_NUN_H), 0);
		}

		// Token: 0x040052F2 RID: 21234
		public static readonly int SCROLL_ITEM_NUN_H = 5;

		// Token: 0x040052F4 RID: 21236
		public GameObject baseObj;

		// Token: 0x040052F5 RID: 21237
		public PguiTextCtrl Massage_Caution;

		// Token: 0x040052F6 RID: 21238
		public ReuseScroll ScrollView;

		// Token: 0x040052F7 RID: 21239
		public PguiOpenWindowCtrl ow;

		// Token: 0x020011D2 RID: 4562
		public enum Type
		{
			// Token: 0x040061D4 RID: 25044
			Invalid,
			// Token: 0x040061D5 RID: 25045
			Growth,
			// Token: 0x040061D6 RID: 25046
			Sale
		}
	}
}
