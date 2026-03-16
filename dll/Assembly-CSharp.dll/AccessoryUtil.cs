using System;
using System.Collections.Generic;
using System.Text;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;

public class AccessoryUtil : MonoBehaviour
{
	public static ItemData GetReleaseAccessoryItemData()
	{
		return DataManager.DmItem.GetUserItemData(AccessoryUtil.ReleaseAccessoryItemId);
	}

	public static bool CanReleasedAccessory()
	{
		return DataManager.DmItem.GetUserItemData(AccessoryUtil.ReleaseAccessoryItemId).num >= AccessoryUtil.GetNeedReleaseAccessoryItemNum();
	}

	public static int GetNeedReleaseAccessoryItemNum()
	{
		return 1;
	}

	public static bool IsInvalid(DataManagerCharaAccessory.Accessory data)
	{
		return data == null;
	}

	public static DataManagerCharaAccessory.Accessory MakeDummy()
	{
		return null;
	}

	public static string MakeLevelString(DataManagerCharaAccessory.Accessory acce, bool isNeedLv)
	{
		if (acce == null)
		{
			return "";
		}
		string text = (isNeedLv ? "Lv." : "");
		return string.Format("{0}{1}/{2}", text, acce.Level, acce.AccessoryData.Rarity.LevelLimit);
	}

	public static string MakeLevelMaxString(DataManagerCharaAccessory.AccessoryData acceData)
	{
		if (acceData == null)
		{
			return "";
		}
		return string.Format("Lv.{0}/{1}", acceData.Rarity.LevelLimit, acceData.Rarity.LevelLimit);
	}

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

	public static string GetColorString(bool normal)
	{
		if (!normal)
		{
			return AccessoryUtil.UP_PARAM_COLOR_CODE;
		}
		return AccessoryUtil.NORMAL_PARAM_COLOR_CODE;
	}

	public static Color GetColorValue(bool normal)
	{
		if (!normal)
		{
			return AccessoryUtil.UP_PARAM_COLOR;
		}
		return AccessoryUtil.NORMAL_PARAM_COLOR;
	}

	public static string MakeDispTypeString(DataManagerCharaAccessory.Accessory acce)
	{
		if (acce == null)
		{
			return "-";
		}
		return AccessoryUtil.MakeDispTypeStringByAccessoryData(acce.AccessoryData);
	}

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

	public static bool CanEquipped(DataManagerCharaAccessory.Accessory acce)
	{
		return acce != null && acce.AccessoryData.LevelupNum == 0;
	}

	public static bool IsDecidedOwner(DataManagerCharaAccessory.Accessory acce)
	{
		return !AccessoryUtil.IsInvalid(acce) && acce.CharaId != 0;
	}

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

	public static bool CanStrengthened(DataManagerCharaAccessory.Accessory baseAcce, DataManagerCharaAccessory.Accessory acce)
	{
		return !AccessoryUtil.IsInvalid(baseAcce) && !AccessoryUtil.IsInvalid(acce) && (baseAcce.ItemId == acce.ItemId || (!AccessoryUtil.CanEquipped(acce) && baseAcce.AccessoryData.Rarity.Rarity <= acce.AccessoryData.Rarity.Rarity));
	}

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

	public static bool IsNeedPermillage(AccessoryUtil.ParamType type)
	{
		bool flag = true;
		if (type == AccessoryUtil.ParamType.Normal || type == AccessoryUtil.ParamType.Normal)
		{
			flag = false;
		}
		return flag;
	}

	private static readonly int ReleaseAccessoryItemId = 17201;

	public static readonly string NoSelectedText = "選択不可";

	private static readonly string UP_PARAM_COLOR_CODE = "#FF7C17FF";

	private static readonly Color UP_PARAM_COLOR = new Color32(byte.MaxValue, 124, 23, byte.MaxValue);

	private static readonly string NORMAL_PARAM_COLOR_CODE = "#533C06FF";

	private static readonly Color NORMAL_PARAM_COLOR = new Color32(83, 60, 6, byte.MaxValue);

	public enum ParamType
	{
		None,
		Normal,
		Beat,
		Action,
		Try,
		Avoid
	}

	public class ParamPackData
	{
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

		private static readonly string PARAM_NAME_HP = "たいりょく";

		private static readonly string PARAM_NAME_ATK = "こうげき";

		private static readonly string PARAM_NAME_DEF = "まもり";

		private static readonly string PARAM_NAME_AVOID = "かいひ";

		private static readonly string PARAM_NAME_BEAT = "Beat!!!";

		private static readonly string PARAM_NAME_ACTION = "Action!";

		private static readonly string PARAM_NAME_TRY = "Try!!";

		public class BaseParam
		{
			public BaseParam()
			{
				this.name = "";
				this.type = AccessoryUtil.ParamType.None;
				this.value = 0;
			}

			public string name;

			public AccessoryUtil.ParamType type;

			public int value;
		}

		public class GrowthParam
		{
			public GrowthParam()
			{
				this.value = 0;
				this.color = Color.white;
				this.type = AccessoryUtil.ParamType.None;
			}

			public int value;

			public Color color;

			public AccessoryUtil.ParamType type;
		}

		public class AccessoryPackData
		{
			public AccessoryPackData()
			{
				this.accessory = null;
				this.levelParam = null;
			}

			public DataManagerCharaAccessory.Accessory accessory;

			public DataManagerCharaAccessory.LevelParam levelParam;
		}
	}

	public class IconAccessorySet
	{
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

		public void SetScale(Vector3 scaleCurrent, Vector3 scaleCount)
		{
			this.currentFrame.transform.Find("Current").localScale = scaleCurrent;
			this.textCount.transform.parent.localScale = scaleCount;
		}

		public GameObject baseObj;

		public RectTransform iconBase;

		public IconAccessoryCtrl iconAccessoryCtrl;

		public GameObject currentFrame;

		public PguiTextCtrl textCount;
	}

	public class SizeChangeBtnGUI
	{
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

		public SizeChangeBtnGUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Btn_SizeChange = baseTr.GetComponent<PguiButtonCtrl>();
			this.Txt = baseTr.Find("BaseImage/On/Txt").GetComponent<PguiTextCtrl>();
		}

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

		public List<AccessoryUtil.SizeChangeBtnGUI.IconAccessoryParam> IconAccessoryParamList
		{
			get
			{
				return this.setupParam.iconAccessoryParamList;
			}
		}

		public int SizeIndex
		{
			get
			{
				return this.setupParam.sizeIndex;
			}
		}

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

		public GameObject baseObj;

		public PguiButtonCtrl Btn_SizeChange;

		public PguiTextCtrl Txt;

		private float scrollSize;

		private AccessoryUtil.SizeChangeBtnGUI.SetupParam setupParam = new AccessoryUtil.SizeChangeBtnGUI.SetupParam();

		public delegate void FuncResult(AccessoryUtil.SizeChangeBtnGUI.ResultParam result);

		public enum DataPackType
		{
			Set,
			All
		}

		public enum SizeType
		{
			S,
			M,
			L,
			XL
		}

		public class DataPack
		{
			public int num;

			public string prefabName;
		}

		public class IconAccessoryParam
		{
			public Vector3 scale;

			public Vector3 scaleCurrent;

			public Vector3 scaleCount;

			public int num;

			public GameObject prefab;
		}

		public class SetupParam
		{
			public int sizeIndex;

			public List<AccessoryUtil.SizeChangeBtnGUI.IconAccessoryParam> iconAccessoryParamList;

			public ReuseScroll refScrollView;

			public AccessoryUtil.SizeChangeBtnGUI.FuncResult funcResult;

			public Action<int, GameObject> onStartItem;

			public Action<int, GameObject> onUpdateItem;

			public UnityAction resetCallback;

			public Func<int> dispIconAccessoryCountCallback;
		}

		public class ResultParam
		{
			public int sizeIndex;
		}
	}
}
