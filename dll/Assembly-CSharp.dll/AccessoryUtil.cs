using System;
using System.Collections.Generic;
using System.Text;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200018C RID: 396
public class AccessoryUtil : MonoBehaviour
{
	// Token: 0x06001A66 RID: 6758 RVA: 0x001567E1 File Offset: 0x001549E1
	public static ItemData GetReleaseAccessoryItemData()
	{
		return DataManager.DmItem.GetUserItemData(AccessoryUtil.ReleaseAccessoryItemId);
	}

	// Token: 0x06001A67 RID: 6759 RVA: 0x001567F2 File Offset: 0x001549F2
	public static bool CanReleasedAccessory()
	{
		return DataManager.DmItem.GetUserItemData(AccessoryUtil.ReleaseAccessoryItemId).num >= AccessoryUtil.GetNeedReleaseAccessoryItemNum();
	}

	// Token: 0x06001A68 RID: 6760 RVA: 0x00156812 File Offset: 0x00154A12
	public static int GetNeedReleaseAccessoryItemNum()
	{
		return 1;
	}

	// Token: 0x06001A69 RID: 6761 RVA: 0x00156815 File Offset: 0x00154A15
	public static bool IsInvalid(DataManagerCharaAccessory.Accessory data)
	{
		return data == null;
	}

	// Token: 0x06001A6A RID: 6762 RVA: 0x0015681B File Offset: 0x00154A1B
	public static DataManagerCharaAccessory.Accessory MakeDummy()
	{
		return null;
	}

	// Token: 0x06001A6B RID: 6763 RVA: 0x00156820 File Offset: 0x00154A20
	public static string MakeLevelString(DataManagerCharaAccessory.Accessory acce, bool isNeedLv)
	{
		if (acce == null)
		{
			return "";
		}
		string text = (isNeedLv ? "Lv." : "");
		return string.Format("{0}{1}/{2}", text, acce.Level, acce.AccessoryData.Rarity.LevelLimit);
	}

	// Token: 0x06001A6C RID: 6764 RVA: 0x00156871 File Offset: 0x00154A71
	public static string MakeLevelMaxString(DataManagerCharaAccessory.AccessoryData acceData)
	{
		if (acceData == null)
		{
			return "";
		}
		return string.Format("Lv.{0}/{1}", acceData.Rarity.LevelLimit, acceData.Rarity.LevelLimit);
	}

	// Token: 0x06001A6D RID: 6765 RVA: 0x001568A8 File Offset: 0x00154AA8
	public static string GetLevelString(DataManagerCharaAccessory.Accessory acce, bool denominator)
	{
		if (acce == null)
		{
			return "";
		}
		if (!denominator)
		{
			return string.Format("{0}", acce.Level);
		}
		return string.Format("{0}", acce.AccessoryData.Rarity.LevelLimit);
	}

	// Token: 0x06001A6E RID: 6766 RVA: 0x001568F6 File Offset: 0x00154AF6
	public static string GetColorString(bool normal)
	{
		if (!normal)
		{
			return AccessoryUtil.UP_PARAM_COLOR_CODE;
		}
		return AccessoryUtil.NORMAL_PARAM_COLOR_CODE;
	}

	// Token: 0x06001A6F RID: 6767 RVA: 0x00156906 File Offset: 0x00154B06
	public static Color GetColorValue(bool normal)
	{
		if (!normal)
		{
			return AccessoryUtil.UP_PARAM_COLOR;
		}
		return AccessoryUtil.NORMAL_PARAM_COLOR;
	}

	// Token: 0x06001A70 RID: 6768 RVA: 0x00156916 File Offset: 0x00154B16
	public static string MakeDispTypeString(DataManagerCharaAccessory.Accessory acce)
	{
		if (acce == null)
		{
			return "-";
		}
		return AccessoryUtil.MakeDispTypeStringByAccessoryData(acce.AccessoryData);
	}

	// Token: 0x06001A71 RID: 6769 RVA: 0x0015692C File Offset: 0x00154B2C
	public static string MakeDispTypeStringByAccessoryData(DataManagerCharaAccessory.AccessoryData acceData)
	{
		if (acceData == null)
		{
			return "-";
		}
		string text = "-";
		if (acceData.DispType == DataManagerCharaAccessory.DispType.Always)
		{
			text = "常時";
		}
		else if (acceData.DispType == DataManagerCharaAccessory.DispType.Battle)
		{
			text = "バトル中";
		}
		return text;
	}

	// Token: 0x06001A72 RID: 6770 RVA: 0x00156969 File Offset: 0x00154B69
	public static bool CanEquipped(DataManagerCharaAccessory.Accessory acce)
	{
		return acce != null && acce.AccessoryData.LevelupNum == 0;
	}

	// Token: 0x06001A73 RID: 6771 RVA: 0x0015697E File Offset: 0x00154B7E
	public static bool IsDecidedOwner(DataManagerCharaAccessory.Accessory acce)
	{
		return !AccessoryUtil.IsInvalid(acce) && acce.CharaId != 0;
	}

	// Token: 0x06001A74 RID: 6772 RVA: 0x00156994 File Offset: 0x00154B94
	public static bool IsEquipped(DataManagerCharaAccessory.Accessory acce)
	{
		if (AccessoryUtil.IsDecidedOwner(acce))
		{
			CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(acce.CharaId);
			if (userCharaData != null)
			{
				return !AccessoryUtil.IsInvalid(userCharaData.dynamicData.accessory) && userCharaData.dynamicData.accessory.UniqId == acce.UniqId;
			}
		}
		return false;
	}

	// Token: 0x06001A75 RID: 6773 RVA: 0x001569EC File Offset: 0x00154BEC
	public static bool CanStrengthened(DataManagerCharaAccessory.Accessory baseAcce, DataManagerCharaAccessory.Accessory acce)
	{
		return !AccessoryUtil.IsInvalid(baseAcce) && !AccessoryUtil.IsInvalid(acce) && (baseAcce.ItemId == acce.ItemId || (!AccessoryUtil.CanEquipped(acce) && baseAcce.AccessoryData.Rarity.Rarity <= acce.AccessoryData.Rarity.Rarity));
	}

	// Token: 0x06001A76 RID: 6774 RVA: 0x00156A4C File Offset: 0x00154C4C
	public static void OpenTutorialWindow()
	{
		DataManagerGameStatus.UserFlagData userFlagData = DataManager.DmGameStatus.MakeUserFlagData();
		if (userFlagData.ReleaseModeFlag.CharaAccessoryOpen == DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Locked)
		{
			CanvasManager.HdlCmnFeedPageWindowCtrl.Open(CmnFeedPageWindowCtrl.Type.PAGE_FEED, "", new List<string> { "Texture2D/Tutorial_Window/Accessory/tutorial_accessory_01", "Texture2D/Tutorial_Window/Accessory/tutorial_accessory_02" }, null);
			userFlagData.ReleaseModeFlag.CharaAccessoryOpen = DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Released;
			DataManager.DmGameStatus.RequestActionUpdateUserFlag(userFlagData);
		}
	}

	// Token: 0x06001A77 RID: 6775 RVA: 0x00156AB4 File Offset: 0x00154CB4
	public static string GetPermillageText(int value)
	{
		StringBuilder stringBuilder = new StringBuilder();
		int num = value / 10;
		int num2 = value % 10;
		stringBuilder.Append(num);
		stringBuilder.Append(".");
		stringBuilder.Append(num2);
		stringBuilder.Append("%");
		return stringBuilder.ToString();
	}

	// Token: 0x06001A78 RID: 6776 RVA: 0x00156B00 File Offset: 0x00154D00
	public static bool IsNeedPermillage(AccessoryUtil.ParamType type)
	{
		bool flag = true;
		if (type == AccessoryUtil.ParamType.Normal || type == AccessoryUtil.ParamType.Normal)
		{
			flag = false;
		}
		return flag;
	}

	// Token: 0x04001430 RID: 5168
	private static readonly int ReleaseAccessoryItemId = 17201;

	// Token: 0x04001431 RID: 5169
	public static readonly string NoSelectedText = "選択不可";

	// Token: 0x04001432 RID: 5170
	private static readonly string UP_PARAM_COLOR_CODE = "#FF7C17FF";

	// Token: 0x04001433 RID: 5171
	private static readonly Color UP_PARAM_COLOR = new Color32(byte.MaxValue, 124, 23, byte.MaxValue);

	// Token: 0x04001434 RID: 5172
	private static readonly string NORMAL_PARAM_COLOR_CODE = "#533C06FF";

	// Token: 0x04001435 RID: 5173
	private static readonly Color NORMAL_PARAM_COLOR = new Color32(83, 60, 6, byte.MaxValue);

	// Token: 0x02000E72 RID: 3698
	public enum ParamType
	{
		// Token: 0x0400531B RID: 21275
		None,
		// Token: 0x0400531C RID: 21276
		Normal,
		// Token: 0x0400531D RID: 21277
		Beat,
		// Token: 0x0400531E RID: 21278
		Action,
		// Token: 0x0400531F RID: 21279
		Try,
		// Token: 0x04005320 RID: 21280
		Avoid
	}

	// Token: 0x02000E73 RID: 3699
	public class ParamPackData
	{
		// Token: 0x06004CB5 RID: 19637 RVA: 0x0022E4A0 File Offset: 0x0022C6A0
		public static void CreateDispList<T>(ref List<T> list, AccessoryUtil.ParamPackData.AccessoryPackData dispAccessory)
		{
			if (dispAccessory == null)
			{
				return;
			}
			if (AccessoryUtil.IsInvalid(dispAccessory.accessory))
			{
				return;
			}
			DataManagerCharaAccessory.LevelParam levelParam = dispAccessory.accessory.AccessoryData.GrowSimulate(dispAccessory.accessory.AccessoryData.Rarity.LevelLimit);
			if (0 < levelParam.Hp)
			{
				if (typeof(T) == typeof(AccessoryUtil.ParamPackData.BaseParam))
				{
					(list as List<AccessoryUtil.ParamPackData.BaseParam>).Add(new AccessoryUtil.ParamPackData.BaseParam
					{
						name = AccessoryUtil.ParamPackData.PARAM_NAME_HP,
						type = AccessoryUtil.ParamType.Normal,
						value = dispAccessory.accessory.Param.Hp
					});
				}
				else if (typeof(T) == typeof(AccessoryUtil.ParamPackData.GrowthParam) && dispAccessory.levelParam != null)
				{
					(list as List<AccessoryUtil.ParamPackData.GrowthParam>).Add(new AccessoryUtil.ParamPackData.GrowthParam
					{
						value = dispAccessory.levelParam.Hp,
						color = AccessoryUtil.GetColorValue(dispAccessory.levelParam.Hp <= dispAccessory.accessory.Param.Hp),
						type = AccessoryUtil.ParamType.Normal
					});
				}
			}
			if (0 < levelParam.Atk)
			{
				if (typeof(T) == typeof(AccessoryUtil.ParamPackData.BaseParam))
				{
					(list as List<AccessoryUtil.ParamPackData.BaseParam>).Add(new AccessoryUtil.ParamPackData.BaseParam
					{
						name = AccessoryUtil.ParamPackData.PARAM_NAME_ATK,
						type = AccessoryUtil.ParamType.Normal,
						value = dispAccessory.accessory.Param.Atk
					});
				}
				else if (typeof(T) == typeof(AccessoryUtil.ParamPackData.GrowthParam) && dispAccessory.levelParam != null)
				{
					(list as List<AccessoryUtil.ParamPackData.GrowthParam>).Add(new AccessoryUtil.ParamPackData.GrowthParam
					{
						value = dispAccessory.levelParam.Atk,
						color = AccessoryUtil.GetColorValue(dispAccessory.levelParam.Atk <= dispAccessory.accessory.Param.Atk),
						type = AccessoryUtil.ParamType.Normal
					});
				}
			}
			if (0 < levelParam.Def)
			{
				if (typeof(T) == typeof(AccessoryUtil.ParamPackData.BaseParam))
				{
					(list as List<AccessoryUtil.ParamPackData.BaseParam>).Add(new AccessoryUtil.ParamPackData.BaseParam
					{
						name = AccessoryUtil.ParamPackData.PARAM_NAME_DEF,
						type = AccessoryUtil.ParamType.Normal,
						value = dispAccessory.accessory.Param.Def
					});
				}
				else if (typeof(T) == typeof(AccessoryUtil.ParamPackData.GrowthParam) && dispAccessory.levelParam != null)
				{
					(list as List<AccessoryUtil.ParamPackData.GrowthParam>).Add(new AccessoryUtil.ParamPackData.GrowthParam
					{
						value = dispAccessory.levelParam.Def,
						color = AccessoryUtil.GetColorValue(dispAccessory.levelParam.Def <= dispAccessory.accessory.Param.Def),
						type = AccessoryUtil.ParamType.Normal
					});
				}
			}
			if (0 < levelParam.Avoid)
			{
				if (typeof(T) == typeof(AccessoryUtil.ParamPackData.BaseParam))
				{
					(list as List<AccessoryUtil.ParamPackData.BaseParam>).Add(new AccessoryUtil.ParamPackData.BaseParam
					{
						name = AccessoryUtil.ParamPackData.PARAM_NAME_AVOID,
						type = AccessoryUtil.ParamType.Avoid,
						value = dispAccessory.accessory.Param.Avoid
					});
				}
				else if (typeof(T) == typeof(AccessoryUtil.ParamPackData.GrowthParam) && dispAccessory.levelParam != null)
				{
					(list as List<AccessoryUtil.ParamPackData.GrowthParam>).Add(new AccessoryUtil.ParamPackData.GrowthParam
					{
						value = dispAccessory.levelParam.Avoid,
						color = AccessoryUtil.GetColorValue(dispAccessory.levelParam.Avoid <= dispAccessory.accessory.Param.Avoid),
						type = AccessoryUtil.ParamType.Avoid
					});
				}
			}
			if (0 < levelParam.Beat)
			{
				if (typeof(T) == typeof(AccessoryUtil.ParamPackData.BaseParam))
				{
					(list as List<AccessoryUtil.ParamPackData.BaseParam>).Add(new AccessoryUtil.ParamPackData.BaseParam
					{
						name = AccessoryUtil.ParamPackData.PARAM_NAME_BEAT,
						type = AccessoryUtil.ParamType.Beat,
						value = dispAccessory.accessory.Param.Beat
					});
				}
				else if (typeof(T) == typeof(AccessoryUtil.ParamPackData.GrowthParam) && dispAccessory.levelParam != null)
				{
					(list as List<AccessoryUtil.ParamPackData.GrowthParam>).Add(new AccessoryUtil.ParamPackData.GrowthParam
					{
						value = dispAccessory.levelParam.Beat,
						color = AccessoryUtil.GetColorValue(dispAccessory.levelParam.Beat <= dispAccessory.accessory.Param.Beat),
						type = AccessoryUtil.ParamType.Beat
					});
				}
			}
			if (0 < levelParam.Action)
			{
				if (typeof(T) == typeof(AccessoryUtil.ParamPackData.BaseParam))
				{
					(list as List<AccessoryUtil.ParamPackData.BaseParam>).Add(new AccessoryUtil.ParamPackData.BaseParam
					{
						name = AccessoryUtil.ParamPackData.PARAM_NAME_ACTION,
						type = AccessoryUtil.ParamType.Action,
						value = dispAccessory.accessory.Param.Action
					});
				}
				else if (typeof(T) == typeof(AccessoryUtil.ParamPackData.GrowthParam) && dispAccessory.levelParam != null)
				{
					(list as List<AccessoryUtil.ParamPackData.GrowthParam>).Add(new AccessoryUtil.ParamPackData.GrowthParam
					{
						value = dispAccessory.levelParam.Action,
						color = AccessoryUtil.GetColorValue(dispAccessory.levelParam.Action <= dispAccessory.accessory.Param.Action),
						type = AccessoryUtil.ParamType.Action
					});
				}
			}
			if (0 < levelParam.Try)
			{
				if (typeof(T) == typeof(AccessoryUtil.ParamPackData.BaseParam))
				{
					(list as List<AccessoryUtil.ParamPackData.BaseParam>).Add(new AccessoryUtil.ParamPackData.BaseParam
					{
						name = AccessoryUtil.ParamPackData.PARAM_NAME_TRY,
						type = AccessoryUtil.ParamType.Try,
						value = dispAccessory.accessory.Param.Try
					});
					return;
				}
				if (typeof(T) == typeof(AccessoryUtil.ParamPackData.GrowthParam) && dispAccessory.levelParam != null)
				{
					(list as List<AccessoryUtil.ParamPackData.GrowthParam>).Add(new AccessoryUtil.ParamPackData.GrowthParam
					{
						value = dispAccessory.levelParam.Try,
						color = AccessoryUtil.GetColorValue(dispAccessory.levelParam.Try <= dispAccessory.accessory.Param.Try),
						type = AccessoryUtil.ParamType.Try
					});
				}
			}
		}

		// Token: 0x06004CB6 RID: 19638 RVA: 0x0022EAD4 File Offset: 0x0022CCD4
		public static List<AccessoryUtil.ParamPackData.BaseParam> CreateDispListForLvMax(DataManagerCharaAccessory.AccessoryData acceData)
		{
			List<AccessoryUtil.ParamPackData.BaseParam> list = new List<AccessoryUtil.ParamPackData.BaseParam>();
			if (acceData == null)
			{
				return list;
			}
			DataManagerCharaAccessory.LevelParam levelParam = acceData.GrowSimulate(acceData.Rarity.LevelLimit);
			if (0 < levelParam.Hp)
			{
				list.Add(new AccessoryUtil.ParamPackData.BaseParam
				{
					name = AccessoryUtil.ParamPackData.PARAM_NAME_HP,
					type = AccessoryUtil.ParamType.Normal,
					value = levelParam.Hp
				});
			}
			if (0 < levelParam.Atk)
			{
				list.Add(new AccessoryUtil.ParamPackData.BaseParam
				{
					name = AccessoryUtil.ParamPackData.PARAM_NAME_ATK,
					type = AccessoryUtil.ParamType.Normal,
					value = levelParam.Atk
				});
			}
			if (0 < levelParam.Def)
			{
				list.Add(new AccessoryUtil.ParamPackData.BaseParam
				{
					name = AccessoryUtil.ParamPackData.PARAM_NAME_DEF,
					type = AccessoryUtil.ParamType.Normal,
					value = levelParam.Def
				});
			}
			if (0 < levelParam.Avoid)
			{
				list.Add(new AccessoryUtil.ParamPackData.BaseParam
				{
					name = AccessoryUtil.ParamPackData.PARAM_NAME_AVOID,
					type = AccessoryUtil.ParamType.Avoid,
					value = levelParam.Avoid
				});
			}
			if (0 < levelParam.Beat)
			{
				list.Add(new AccessoryUtil.ParamPackData.BaseParam
				{
					name = AccessoryUtil.ParamPackData.PARAM_NAME_BEAT,
					type = AccessoryUtil.ParamType.Beat,
					value = levelParam.Beat
				});
			}
			if (0 < levelParam.Action)
			{
				list.Add(new AccessoryUtil.ParamPackData.BaseParam
				{
					name = AccessoryUtil.ParamPackData.PARAM_NAME_ACTION,
					type = AccessoryUtil.ParamType.Action,
					value = levelParam.Action
				});
			}
			if (0 < levelParam.Try)
			{
				list.Add(new AccessoryUtil.ParamPackData.BaseParam
				{
					name = AccessoryUtil.ParamPackData.PARAM_NAME_TRY,
					type = AccessoryUtil.ParamType.Try,
					value = levelParam.Try
				});
			}
			return list;
		}

		// Token: 0x04005321 RID: 21281
		private static readonly string PARAM_NAME_HP = "たいりょく";

		// Token: 0x04005322 RID: 21282
		private static readonly string PARAM_NAME_ATK = "こうげき";

		// Token: 0x04005323 RID: 21283
		private static readonly string PARAM_NAME_DEF = "まもり";

		// Token: 0x04005324 RID: 21284
		private static readonly string PARAM_NAME_AVOID = "かいひ";

		// Token: 0x04005325 RID: 21285
		private static readonly string PARAM_NAME_BEAT = "Beat!!!";

		// Token: 0x04005326 RID: 21286
		private static readonly string PARAM_NAME_ACTION = "Action!";

		// Token: 0x04005327 RID: 21287
		private static readonly string PARAM_NAME_TRY = "Try!!";

		// Token: 0x020011DA RID: 4570
		public class BaseParam
		{
			// Token: 0x06005746 RID: 22342 RVA: 0x0025678E File Offset: 0x0025498E
			public BaseParam()
			{
				this.name = "";
				this.type = AccessoryUtil.ParamType.None;
				this.value = 0;
			}

			// Token: 0x040061E6 RID: 25062
			public string name;

			// Token: 0x040061E7 RID: 25063
			public AccessoryUtil.ParamType type;

			// Token: 0x040061E8 RID: 25064
			public int value;
		}

		// Token: 0x020011DB RID: 4571
		public class GrowthParam
		{
			// Token: 0x06005747 RID: 22343 RVA: 0x002567AF File Offset: 0x002549AF
			public GrowthParam()
			{
				this.value = 0;
				this.color = Color.white;
				this.type = AccessoryUtil.ParamType.None;
			}

			// Token: 0x040061E9 RID: 25065
			public int value;

			// Token: 0x040061EA RID: 25066
			public Color color;

			// Token: 0x040061EB RID: 25067
			public AccessoryUtil.ParamType type;
		}

		// Token: 0x020011DC RID: 4572
		public class AccessoryPackData
		{
			// Token: 0x06005748 RID: 22344 RVA: 0x002567D0 File Offset: 0x002549D0
			public AccessoryPackData()
			{
				this.accessory = null;
				this.levelParam = null;
			}

			// Token: 0x040061EC RID: 25068
			public DataManagerCharaAccessory.Accessory accessory;

			// Token: 0x040061ED RID: 25069
			public DataManagerCharaAccessory.LevelParam levelParam;
		}
	}

	// Token: 0x02000E74 RID: 3700
	public class IconAccessorySet
	{
		// Token: 0x06004CB9 RID: 19641 RVA: 0x0022ECBC File Offset: 0x0022CEBC
		public IconAccessorySet(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			GameObject gameObject = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Accessory, baseTr.Find("Icon_Accessory"));
			this.iconAccessoryCtrl = gameObject.GetComponent<IconAccessoryCtrl>();
			this.iconBase = baseTr.Find("Icon_Accessory").GetComponent<RectTransform>();
			this.currentFrame = baseTr.Find("Current").gameObject;
			this.currentFrame.SetActive(false);
			this.textCount = baseTr.Find("Count/Num_Count").GetComponent<PguiTextCtrl>();
			this.textCount.transform.parent.gameObject.SetActive(false);
		}

		// Token: 0x06004CBA RID: 19642 RVA: 0x0022ED6B File Offset: 0x0022CF6B
		public void DispCount(bool flag, string str = null)
		{
			if (this.textCount != null)
			{
				if (str != null)
				{
					this.textCount.text = str;
				}
				this.textCount.transform.parent.gameObject.SetActive(flag);
			}
		}

		// Token: 0x06004CBB RID: 19643 RVA: 0x0022EDA5 File Offset: 0x0022CFA5
		public void SetScale(Vector3 scaleCurrent, Vector3 scaleCount)
		{
			this.currentFrame.transform.Find("Current").localScale = scaleCurrent;
			this.textCount.transform.parent.localScale = scaleCount;
		}

		// Token: 0x04005328 RID: 21288
		public GameObject baseObj;

		// Token: 0x04005329 RID: 21289
		public RectTransform iconBase;

		// Token: 0x0400532A RID: 21290
		public IconAccessoryCtrl iconAccessoryCtrl;

		// Token: 0x0400532B RID: 21291
		public GameObject currentFrame;

		// Token: 0x0400532C RID: 21292
		public PguiTextCtrl textCount;
	}

	// Token: 0x02000E75 RID: 3701
	public class SizeChangeBtnGUI
	{
		// Token: 0x06004CBC RID: 19644 RVA: 0x0022EDD8 File Offset: 0x0022CFD8
		public static AccessoryUtil.SizeChangeBtnGUI.DataPack[] GetDataPacks(AccessoryUtil.SizeChangeBtnGUI.DataPackType type)
		{
			if (type == AccessoryUtil.SizeChangeBtnGUI.DataPackType.Set)
			{
				return AccessoryUtil.SizeChangeBtnGUI.setDataPacks;
			}
			if (type != AccessoryUtil.SizeChangeBtnGUI.DataPackType.All)
			{
				return AccessoryUtil.SizeChangeBtnGUI.setDataPacks;
			}
			return AccessoryUtil.SizeChangeBtnGUI.allDataPacks;
		}

		// Token: 0x06004CBD RID: 19645 RVA: 0x0022EDF4 File Offset: 0x0022CFF4
		public SizeChangeBtnGUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Btn_SizeChange = baseTr.GetComponent<PguiButtonCtrl>();
			this.Txt = baseTr.Find("BaseImage/On/Txt").GetComponent<PguiTextCtrl>();
		}

		// Token: 0x06004CBE RID: 19646 RVA: 0x0022EE40 File Offset: 0x0022D040
		public void Setup(AccessoryUtil.SizeChangeBtnGUI.SetupParam param)
		{
			this.setupParam = param;
			this.scrollSize = this.setupParam.refScrollView.Size;
			this.Btn_SizeChange.AddOnClickListener(delegate(PguiButtonCtrl btn)
			{
				if (btn == this.Btn_SizeChange)
				{
					this.setupParam.sizeIndex--;
					this.setupParam.sizeIndex = (this.setupParam.sizeIndex + this.setupParam.iconAccessoryParamList.Count) % this.setupParam.iconAccessoryParamList.Count;
					this.ResetScrollView();
					this.setupParam.funcResult(new AccessoryUtil.SizeChangeBtnGUI.ResultParam
					{
						sizeIndex = this.setupParam.sizeIndex
					});
				}
			}, PguiButtonCtrl.SoundType.DEFAULT);
		}

		// Token: 0x06004CBF RID: 19647 RVA: 0x0022EE78 File Offset: 0x0022D078
		private void UpdateText(int index)
		{
			switch (index)
			{
			case 0:
				this.Txt.text = PrjUtil.MakeMessage("小");
				return;
			case 1:
				this.Txt.text = PrjUtil.MakeMessage("中");
				return;
			case 2:
				this.Txt.text = PrjUtil.MakeMessage("大");
				return;
			case 3:
				this.Txt.text = PrjUtil.MakeMessage("特大");
				return;
			default:
				this.Txt.text = PrjUtil.MakeMessage("エラー");
				return;
			}
		}

		// Token: 0x06004CC0 RID: 19648 RVA: 0x0022EF0C File Offset: 0x0022D10C
		public void ResetScrollView()
		{
			this.UpdateText(this.setupParam.sizeIndex);
			this.setupParam.refScrollView.Clear();
			this.setupParam.refScrollView.SetPrefab(this.setupParam.iconAccessoryParamList[this.setupParam.sizeIndex].prefab);
			UnityAction resetCallback = this.setupParam.resetCallback;
			if (resetCallback != null)
			{
				resetCallback();
			}
			if (this.setupParam.refScrollView.onStartItem != null)
			{
				ReuseScroll refScrollView = this.setupParam.refScrollView;
				refScrollView.onStartItem = (Action<int, GameObject>)Delegate.Remove(refScrollView.onStartItem, this.setupParam.onStartItem);
			}
			if (this.setupParam.refScrollView.onUpdateItem != null)
			{
				ReuseScroll refScrollView2 = this.setupParam.refScrollView;
				refScrollView2.onUpdateItem = (Action<int, GameObject>)Delegate.Remove(refScrollView2.onUpdateItem, this.setupParam.onUpdateItem);
			}
			ReuseScroll refScrollView3 = this.setupParam.refScrollView;
			refScrollView3.onStartItem = (Action<int, GameObject>)Delegate.Combine(refScrollView3.onStartItem, this.setupParam.onStartItem);
			ReuseScroll refScrollView4 = this.setupParam.refScrollView;
			refScrollView4.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(refScrollView4.onUpdateItem, this.setupParam.onUpdateItem);
			this.setupParam.refScrollView.Size = this.scrollSize * this.setupParam.iconAccessoryParamList[this.setupParam.sizeIndex].scale.y;
			int num = ((this.setupParam.dispIconAccessoryCountCallback == null) ? 10 : this.setupParam.dispIconAccessoryCountCallback());
			this.setupParam.refScrollView.Setup(num / this.setupParam.iconAccessoryParamList[this.setupParam.sizeIndex].num + 1, 0);
			this.setupParam.refScrollView.Refresh();
		}

		// Token: 0x17000B31 RID: 2865
		// (get) Token: 0x06004CC1 RID: 19649 RVA: 0x0022F0F3 File Offset: 0x0022D2F3
		public List<AccessoryUtil.SizeChangeBtnGUI.IconAccessoryParam> IconAccessoryParamList
		{
			get
			{
				return this.setupParam.iconAccessoryParamList;
			}
		}

		// Token: 0x17000B32 RID: 2866
		// (get) Token: 0x06004CC2 RID: 19650 RVA: 0x0022F100 File Offset: 0x0022D300
		public int SizeIndex
		{
			get
			{
				return this.setupParam.sizeIndex;
			}
		}

		// Token: 0x0400532D RID: 21293
		private static readonly AccessoryUtil.SizeChangeBtnGUI.DataPack[] setDataPacks = new AccessoryUtil.SizeChangeBtnGUI.DataPack[]
		{
			new AccessoryUtil.SizeChangeBtnGUI.DataPack
			{
				num = 7,
				prefabName = "SceneAccessory/GUI/Prefab/AccessorySet_Icon_Accessory_List_S"
			},
			new AccessoryUtil.SizeChangeBtnGUI.DataPack
			{
				num = 6,
				prefabName = "SceneAccessory/GUI/Prefab/AccessorySet_Icon_Accessory_List_M"
			},
			new AccessoryUtil.SizeChangeBtnGUI.DataPack
			{
				num = 5,
				prefabName = "SceneAccessory/GUI/Prefab/AccessorySet_Icon_Accessory_List_L"
			},
			new AccessoryUtil.SizeChangeBtnGUI.DataPack
			{
				num = 4,
				prefabName = "SceneAccessory/GUI/Prefab/AccessorySet_Icon_Accessory_List_XL"
			}
		};

		// Token: 0x0400532E RID: 21294
		private static readonly AccessoryUtil.SizeChangeBtnGUI.DataPack[] allDataPacks = new AccessoryUtil.SizeChangeBtnGUI.DataPack[]
		{
			new AccessoryUtil.SizeChangeBtnGUI.DataPack
			{
				num = 10,
				prefabName = "SceneAccessory/GUI/Prefab/AccessoryAll_Icon_Accessory_List_S"
			},
			new AccessoryUtil.SizeChangeBtnGUI.DataPack
			{
				num = 8,
				prefabName = "SceneAccessory/GUI/Prefab/AccessoryAll_Icon_Accessory_List_M"
			},
			new AccessoryUtil.SizeChangeBtnGUI.DataPack
			{
				num = 7,
				prefabName = "SceneAccessory/GUI/Prefab/AccessoryAll_Icon_Accessory_List_L"
			},
			new AccessoryUtil.SizeChangeBtnGUI.DataPack
			{
				num = 6,
				prefabName = "SceneAccessory/GUI/Prefab/AccessoryAll_Icon_Accessory_List_XL"
			}
		};

		// Token: 0x0400532F RID: 21295
		public GameObject baseObj;

		// Token: 0x04005330 RID: 21296
		public PguiButtonCtrl Btn_SizeChange;

		// Token: 0x04005331 RID: 21297
		public PguiTextCtrl Txt;

		// Token: 0x04005332 RID: 21298
		private float scrollSize;

		// Token: 0x04005333 RID: 21299
		private AccessoryUtil.SizeChangeBtnGUI.SetupParam setupParam = new AccessoryUtil.SizeChangeBtnGUI.SetupParam();

		// Token: 0x020011DD RID: 4573
		// (Invoke) Token: 0x0600574A RID: 22346
		public delegate void FuncResult(AccessoryUtil.SizeChangeBtnGUI.ResultParam result);

		// Token: 0x020011DE RID: 4574
		public enum DataPackType
		{
			// Token: 0x040061EF RID: 25071
			Set,
			// Token: 0x040061F0 RID: 25072
			All
		}

		// Token: 0x020011DF RID: 4575
		public enum SizeType
		{
			// Token: 0x040061F2 RID: 25074
			S,
			// Token: 0x040061F3 RID: 25075
			M,
			// Token: 0x040061F4 RID: 25076
			L,
			// Token: 0x040061F5 RID: 25077
			XL
		}

		// Token: 0x020011E0 RID: 4576
		public class DataPack
		{
			// Token: 0x040061F6 RID: 25078
			public int num;

			// Token: 0x040061F7 RID: 25079
			public string prefabName;
		}

		// Token: 0x020011E1 RID: 4577
		public class IconAccessoryParam
		{
			// Token: 0x040061F8 RID: 25080
			public Vector3 scale;

			// Token: 0x040061F9 RID: 25081
			public Vector3 scaleCurrent;

			// Token: 0x040061FA RID: 25082
			public Vector3 scaleCount;

			// Token: 0x040061FB RID: 25083
			public int num;

			// Token: 0x040061FC RID: 25084
			public GameObject prefab;
		}

		// Token: 0x020011E2 RID: 4578
		public class SetupParam
		{
			// Token: 0x040061FD RID: 25085
			public int sizeIndex;

			// Token: 0x040061FE RID: 25086
			public List<AccessoryUtil.SizeChangeBtnGUI.IconAccessoryParam> iconAccessoryParamList;

			// Token: 0x040061FF RID: 25087
			public ReuseScroll refScrollView;

			// Token: 0x04006200 RID: 25088
			public AccessoryUtil.SizeChangeBtnGUI.FuncResult funcResult;

			// Token: 0x04006201 RID: 25089
			public Action<int, GameObject> onStartItem;

			// Token: 0x04006202 RID: 25090
			public Action<int, GameObject> onUpdateItem;

			// Token: 0x04006203 RID: 25091
			public UnityAction resetCallback;

			// Token: 0x04006204 RID: 25092
			public Func<int> dispIconAccessoryCountCallback;
		}

		// Token: 0x020011E3 RID: 4579
		public class ResultParam
		{
			// Token: 0x04006205 RID: 25093
			public int sizeIndex;
		}
	}
}
