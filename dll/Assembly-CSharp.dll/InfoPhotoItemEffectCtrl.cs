using System;
using System.Collections.Generic;
using UnityEngine;

public class InfoPhotoItemEffectCtrl : MonoBehaviour
{
	private InfoPhotoItemEffectCtrl.GUI GuiData { get; set; }

	private bool IsInit { get; set; }

	private List<DataManagerPhoto.CalcDropBonusResult> PhotoBonusResults { get; set; }

	private bool SwitchFlag { get; set; }

	private InfoPhotoItemEffectCtrl.CalcPhotoDropItemResult PhotoDropItemResult { get; set; }

	private List<InfoPhotoItemEffectCtrl.ToggleCtrl> ToggleCtrls { get; set; }

	private bool IsToggle
	{
		get
		{
			if (this.ToggleCtrls == null)
			{
				return false;
			}
			List<InfoPhotoItemEffectCtrl.ToggleCtrl> list = this.ToggleCtrls.FindAll((InfoPhotoItemEffectCtrl.ToggleCtrl item) => item.type == InfoPhotoItemEffectCtrl.Type.EarnMore);
			List<InfoPhotoItemEffectCtrl.ToggleCtrl> list2 = this.ToggleCtrls.FindAll((InfoPhotoItemEffectCtrl.ToggleCtrl item) => item.type == InfoPhotoItemEffectCtrl.Type.LotteryMore);
			return (list != null && list.Count > this.GuiData.iconItems.Count && this.IsEarnMore) || this.IsDifferentType || (list2 != null && list2.Count > this.GuiData.iconItems.Count && this.IsLotteryMore);
		}
	}

	private bool IsDifferentType
	{
		get
		{
			if (this.ToggleCtrls == null)
			{
				return false;
			}
			bool flag = this.ToggleCtrls.FindAll((InfoPhotoItemEffectCtrl.ToggleCtrl item) => item.type == InfoPhotoItemEffectCtrl.Type.EarnMore) != null;
			List<InfoPhotoItemEffectCtrl.ToggleCtrl> list = this.ToggleCtrls.FindAll((InfoPhotoItemEffectCtrl.ToggleCtrl item) => item.type == InfoPhotoItemEffectCtrl.Type.LotteryMore);
			return flag && list != null && this.CurrentType != this.NextType;
		}
	}

	private InfoPhotoItemEffectCtrl.Type CurrentType
	{
		get
		{
			if (this.ToggleCtrls == null)
			{
				return InfoPhotoItemEffectCtrl.Type.None;
			}
			if (this.ToggleCtrls.Count > this.CurrentToggleIndex)
			{
				return this.ToggleCtrls[this.CurrentToggleIndex].type;
			}
			return InfoPhotoItemEffectCtrl.Type.None;
		}
	}

	private InfoPhotoItemEffectCtrl.Type NextType
	{
		get
		{
			if (this.ToggleCtrls == null || this.ToggleCtrls.Count == 0)
			{
				return InfoPhotoItemEffectCtrl.Type.None;
			}
			this.NextIndex = 0;
			if (this.ToggleCtrls.Count - 1 == this.CurrentToggleIndex)
			{
				return this.ToggleCtrls[this.NextIndex].type;
			}
			this.NextIndex = this.CurrentToggleIndex + this.GuiData.iconItems.Count;
			if (this.NextIndex >= this.ToggleCtrls.Count)
			{
				this.NextIndex = this.ToggleCtrls.Count - 1;
				if (this.NextIndex >= this.ToggleCtrls.Count)
				{
					this.NextIndex = this.ToggleCtrls.Count - 1;
				}
			}
			return this.ToggleCtrls[this.NextIndex].type;
		}
	}

	private int NextIndex { get; set; }

	private bool IsNextSameTypeEarnMore
	{
		get
		{
			if (this.ToggleCtrls == null || this.ToggleCtrls.Count == 0)
			{
				return false;
			}
			int num = this.CurrentToggleIndex + this.GuiData.iconItems.Count;
			return num < this.ToggleCtrls.Count && this.ToggleCtrls[num].type == InfoPhotoItemEffectCtrl.Type.EarnMore;
		}
	}

	private bool IsEarnMore
	{
		get
		{
			return this.CurrentType == InfoPhotoItemEffectCtrl.Type.EarnMore;
		}
	}

	private bool IsLotteryMore
	{
		get
		{
			return this.CurrentType == InfoPhotoItemEffectCtrl.Type.LotteryMore;
		}
	}

	private int CurrentToggleIndex { get; set; }

	public void Setup(InfoPhotoItemEffectCtrl.SetupParam param)
	{
		if (!this.IsInit)
		{
			this.InitForce();
		}
		this.setupParam = param;
		this.PhotoBonusResults = ((this.setupParam.photoPackDatas != null) ? DataManager.DmPhoto.CalcPhotoBonus(this.setupParam.photoPackDatas, TimeManager.Now, null) : new List<DataManagerPhoto.CalcDropBonusResult>());
		this.InitInternal(this.setupParam.photoPackDatas);
		this.GuiData.baseObj.SetActive(this.ToggleCtrls.Count > 0);
		if (this.setupParam.optionGameObjects != null && this.setupParam.optionGameObjects.Count > 0)
		{
			this.setupParam.optionGameObjects[0].transform.parent.gameObject.SetActive(false);
		}
		this.UpdateIcon();
		this.StartToggle(1f);
	}

	public void Setup(List<DataManagerPhoto.CalcDropBonusResult> bonus)
	{
		if (!this.IsInit)
		{
			this.InitForce();
		}
		this.setupParam = new InfoPhotoItemEffectCtrl.SetupParam();
		this.PhotoBonusResults = new List<DataManagerPhoto.CalcDropBonusResult>(bonus);
		this.PhotoBonusResults.Sort((DataManagerPhoto.CalcDropBonusResult a, DataManagerPhoto.CalcDropBonusResult b) => b.targetItemId - a.targetItemId);
		this.PhotoDropItemResult = new InfoPhotoItemEffectCtrl.CalcPhotoDropItemResult();
		this.InitInternal(null);
		this.GuiData.baseObj.SetActive(this.ToggleCtrls.Count > 0);
		this.UpdateIcon();
		this.StartToggle(1f);
		if (!this.GuiData.baseObj.activeSelf)
		{
			this.GuiData.baseObj.transform.parent.gameObject.SetActive(false);
		}
	}

	public void Setup(List<DataManagerPhoto.CalcDropBonusResult> bonus, List<PhotoPackData> photoPackDatas)
	{
		if (!this.IsInit)
		{
			this.InitForce();
		}
		this.setupParam = new InfoPhotoItemEffectCtrl.SetupParam();
		this.PhotoBonusResults = new List<DataManagerPhoto.CalcDropBonusResult>(bonus);
		this.PhotoBonusResults.Sort((DataManagerPhoto.CalcDropBonusResult a, DataManagerPhoto.CalcDropBonusResult b) => b.targetItemId - a.targetItemId);
		this.InitInternal(photoPackDatas);
		this.GuiData.baseObj.SetActive(this.ToggleCtrls.Count > 0);
		this.UpdateIcon();
		this.StartToggle(1f);
	}

	private void InitInternal(List<PhotoPackData> photoPackDatas)
	{
		this.animCounter = 0;
		this.SwitchFlag = false;
		this.CurrentToggleIndex = 0;
		this.ToggleCtrls = new List<InfoPhotoItemEffectCtrl.ToggleCtrl>();
		foreach (DataManagerPhoto.CalcDropBonusResult calcDropBonusResult in this.PhotoBonusResults)
		{
			this.ToggleCtrls.Add(new InfoPhotoItemEffectCtrl.ToggleCtrl
			{
				type = InfoPhotoItemEffectCtrl.Type.EarnMore
			});
		}
		if (photoPackDatas != null)
		{
			this.PhotoDropItemResult = new InfoPhotoItemEffectCtrl.CalcPhotoDropItemResult();
			List<DataManagerPhoto.PhotoDropItemData> photoQuestDropItemList = DataManager.DmPhoto.GetPhotoQuestDropItemList(TimeManager.Now);
			using (List<PhotoPackData>.Enumerator enumerator2 = photoPackDatas.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					PhotoPackData e = enumerator2.Current;
					if (e != null && !e.IsInvalid())
					{
						DataManagerPhoto.PhotoDropItemData photoDropItemData = photoQuestDropItemList.Find((DataManagerPhoto.PhotoDropItemData item) => item.PhotoId == e.staticData.GetId());
						if (photoDropItemData != null && (e.dynamicData.levelRank >= photoDropItemData.PhotoLimitOverNum || this.setupParam.infoText != null))
						{
							this.PhotoDropItemResult.bonusDrawNum += photoDropItemData.BonusDrawNum;
						}
					}
				}
			}
			if (this.PhotoDropItemResult.bonusDrawNum > 0)
			{
				this.ToggleCtrls.Add(new InfoPhotoItemEffectCtrl.ToggleCtrl
				{
					type = InfoPhotoItemEffectCtrl.Type.LotteryMore
				});
			}
		}
	}

	private void UpdateIcon()
	{
		if (this.CurrentType == InfoPhotoItemEffectCtrl.Type.None)
		{
			return;
		}
		List<InfoPhotoItemEffectCtrl.ToggleCtrl> list = this.ToggleCtrls.FindAll((InfoPhotoItemEffectCtrl.ToggleCtrl item) => this.CurrentType == item.type);
		int num = ((list != null && list.Count > 0) ? list.Count : 0);
		int num2 = ((num > 0) ? (this.animCounter % num * this.GuiData.iconItems.Count) : 0);
		if (num2 >= num)
		{
			this.animCounter = 0;
			num2 = 0;
		}
		for (int i = 0; i < this.GuiData.iconItems.Count; i++)
		{
			int num3 = num2 + i;
			InfoPhotoItemEffectCtrl.GUI.IconItem iconItem = this.GuiData.iconItems[i];
			if (this.IsEarnMore)
			{
				if (num3 < this.PhotoBonusResults.Count)
				{
					iconItem.baseObj.SetActive(true);
					iconItem.Setup(this.PhotoBonusResults[num3]);
				}
				else
				{
					iconItem.baseObj.SetActive(false);
				}
			}
			else if (this.IsLotteryMore)
			{
				iconItem.baseObj.SetActive(false);
			}
		}
		PguiReplaceSpriteCtrl component = this.GuiData.Mark_PhotoEffect.GetComponent<PguiReplaceSpriteCtrl>();
		int num4 = 1;
		string text = "獲得量アップ！";
		if (this.IsEarnMore)
		{
			num4 = 1;
			text = "獲得量アップ！";
		}
		else if (this.IsLotteryMore)
		{
			num4 = 2;
			text = "追加報酬抽選";
		}
		this.GuiData.Mark_PhotoEffect.SetImageByName(component.GetSpriteById(num4).name);
		this.GuiData.Num_Add.gameObject.SetActive(this.IsLotteryMore);
		this.GuiData.Num_Add.text = string.Format("+{0}", this.PhotoDropItemResult.bonusDrawNum);
		this.GuiData.Txt_Title.text = text;
		if (this.setupParam.infoText != null && this.setupParam.photoPackDatas != null)
		{
			bool flag = false;
			bool flag2 = false;
			List<DataManagerPhoto.PhotoDropItemData> photoQuestDropItemList = DataManager.DmPhoto.GetPhotoQuestDropItemList(TimeManager.Now);
			using (List<PhotoPackData>.Enumerator enumerator = this.setupParam.photoPackDatas.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PhotoPackData e = enumerator.Current;
					DataManagerPhoto.PhotoDropItemData photoDropItemData = photoQuestDropItemList.Find((DataManagerPhoto.PhotoDropItemData item) => item.PhotoId == e.staticData.GetId());
					if (photoDropItemData != null)
					{
						flag = !photoDropItemData.HelperEnabled;
						flag2 = e.dynamicData.levelRank >= photoDropItemData.PhotoLimitOverNum;
						break;
					}
				}
			}
			foreach (PguiDataHolder pguiDataHolder in this.GuiData.baseObj.GetComponentsInChildren<PguiDataHolder>())
			{
				if (this.IsEarnMore)
				{
					if (pguiDataHolder.GetComponent<PguiImageCtrl>() != null)
					{
						pguiDataHolder.GetComponent<PguiImageCtrl>().m_Image.color = pguiDataHolder.color;
					}
					else if (pguiDataHolder.GetComponent<PguiTextCtrl>() != null)
					{
						pguiDataHolder.GetComponent<PguiTextCtrl>().m_Text.color = pguiDataHolder.color;
					}
				}
				else if (this.IsLotteryMore)
				{
					if (pguiDataHolder.GetComponent<PguiImageCtrl>() != null)
					{
						pguiDataHolder.GetComponent<PguiImageCtrl>().m_Image.color = (flag2 ? pguiDataHolder.color : new Color(InfoPhotoItemEffectCtrl.mskColor * pguiDataHolder.color.r, InfoPhotoItemEffectCtrl.mskColor * pguiDataHolder.color.g, InfoPhotoItemEffectCtrl.mskColor * pguiDataHolder.color.b, 1f));
					}
					else if (pguiDataHolder.GetComponent<PguiTextCtrl>() != null)
					{
						pguiDataHolder.GetComponent<PguiTextCtrl>().m_Text.color = (flag2 ? pguiDataHolder.color : new Color(InfoPhotoItemEffectCtrl.mskColor * pguiDataHolder.color.r, InfoPhotoItemEffectCtrl.mskColor * pguiDataHolder.color.g, InfoPhotoItemEffectCtrl.mskColor * pguiDataHolder.color.b, 1f));
					}
				}
			}
			this.setupParam.infoText.gameObject.SetActive(this.IsLotteryMore && (flag || !flag2));
			this.setupParam.infoText.text = ((!flag2) ? "最大まで限界突破で解放！" : "助っ人のフォトは効果対象外");
		}
		if (this.setupParam.optionPhotoPackDatas != null && this.setupParam.optionGameObjects != null)
		{
			if (this.IsEarnMore)
			{
				PhotoPackData photoPackData = new PhotoPackData();
				if (this.setupParam.photoPackDatas != null && this.setupParam.photoPackDatas.Count > 0)
				{
					photoPackData = this.setupParam.photoPackDatas[0];
				}
				List<DataManagerPhoto.CalcDropBonusResult> list2 = DataManager.DmPhoto.CalcPhotoClampBonus(photoPackData, this.setupParam.optionPhotoPackDatas, TimeManager.Now);
				int num5 = 0;
				for (int k = 0; k < this.setupParam.optionGameObjects.Count; k++)
				{
					int num6 = num2 + k;
					GameObject gameObject = this.setupParam.optionGameObjects[k];
					if (num6 < list2.Count)
					{
						gameObject.SetActive(true);
						if (this.setupParam.optionFloatValueCB != null && this.setupParam.optionPhotoBonusResultCB == null)
						{
							this.setupParam.optionFloatValueCB(gameObject, (float)list2[num6].ratio / 100f);
						}
						else if (this.setupParam.optionFloatValueCB == null && this.setupParam.optionPhotoBonusResultCB != null)
						{
							this.setupParam.optionPhotoBonusResultCB(gameObject, list2[num6]);
						}
					}
					else
					{
						gameObject.SetActive(false);
						num5++;
					}
				}
				this.setupParam.optionGameObjects[0].transform.parent.gameObject.SetActive(num5 < this.setupParam.optionGameObjects.Count);
			}
			else
			{
				this.setupParam.optionGameObjects[0].transform.parent.gameObject.SetActive(false);
			}
		}
		if (this.IsEarnMore)
		{
			this.CurrentToggleIndex = num2;
			return;
		}
		this.CurrentToggleIndex = num2 + this.ToggleCtrls.FindAll((InfoPhotoItemEffectCtrl.ToggleCtrl item) => item.type == InfoPhotoItemEffectCtrl.Type.EarnMore).Count;
	}

	private void StartToggle(float startTime = 0f)
	{
		this.GuiData.All.ExInit();
		this.GuiData.Grid.ExInit();
		this.GuiData.Grid.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START.ToString(), startTime, this.IsToggle ? 1f : 0f);
		this.GuiData.All.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START.ToString(), startTime, this.IsToggle ? 1f : 0f);
		if (this.IsToggle)
		{
			Action CallFinishedAnim = delegate
			{
				this.animCounter++;
				this.SwitchFlag = true;
				if (this.IsDifferentType)
				{
					if (this.IsEarnMore)
					{
						this.CurrentToggleIndex += this.NextIndex;
						if (this.CurrentToggleIndex >= this.ToggleCtrls.Count)
						{
							this.CurrentToggleIndex = this.ToggleCtrls.Count - 1;
						}
					}
					else
					{
						this.CurrentToggleIndex = 0;
					}
					this.animCounter = 0;
				}
			};
			if (this.IsEarnMore)
			{
				if (this.IsNextSameTypeEarnMore)
				{
					this.GuiData.Grid.ExResumeAnimation(delegate
					{
						CallFinishedAnim();
					});
				}
				else
				{
					this.GuiData.All.ExResumeAnimation(delegate
					{
						CallFinishedAnim();
					});
				}
			}
			else if (this.IsLotteryMore)
			{
				this.GuiData.All.ExResumeAnimation(delegate
				{
					CallFinishedAnim();
				});
			}
		}
		if (this.setupParam.optionGameObjects != null)
		{
			SimpleAnimation component = this.setupParam.optionGameObjects[0].transform.parent.GetComponent<SimpleAnimation>();
			if (component)
			{
				component.ExInit();
				component.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START.ToString(), startTime, 0f);
				if (this.PhotoBonusResults.Count > this.GuiData.iconItems.Count)
				{
					component.ExResumeAnimation(null);
				}
			}
		}
	}

	private void InitForce()
	{
		this.GuiData = new InfoPhotoItemEffectCtrl.GUI(base.transform);
		this.IsInit = true;
	}

	private void Awake()
	{
		this.InitForce();
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (this.SwitchFlag)
		{
			this.SwitchFlag = false;
			this.UpdateIcon();
			this.StartToggle(0f);
		}
	}

	private static readonly float mskColor = 0.6f;

	private InfoPhotoItemEffectCtrl.SetupParam setupParam = new InfoPhotoItemEffectCtrl.SetupParam();

	private int animCounter;

	public delegate void OnUpdateValue<T>(GameObject go, T val);

	public enum Type
	{
		None,
		EarnMore,
		LotteryMore
	}

	private class ToggleCtrl
	{
		public InfoPhotoItemEffectCtrl.Type type;
	}

	protected class CalcPhotoDropItemResult
	{
		public int bonusDrawNum;
	}

	public class SetupParam
	{
		public List<PhotoPackData> photoPackDatas;

		public List<PhotoPackData> optionPhotoPackDatas;

		public List<GameObject> optionGameObjects;

		public InfoPhotoItemEffectCtrl.OnUpdateValue<float> optionFloatValueCB;

		public InfoPhotoItemEffectCtrl.OnUpdateValue<DataManagerPhoto.CalcDropBonusResult> optionPhotoBonusResultCB;

		public PguiTextCtrl infoText;
	}

	private class GUI
	{
		public GUI(Transform baseTr)
		{
			object obj3 = new Action<GameObject>(delegate(GameObject obj)
			{
				if (obj.GetComponent<PguiDataHolder>() != null)
				{
					return;
				}
				obj.gameObject.AddComponent<PguiDataHolder>();
				Color color = default(Color);
				if (obj.GetComponent<PguiImageCtrl>() != null)
				{
					color = obj.GetComponent<PguiImageCtrl>().m_Image.color;
				}
				else if (obj.GetComponent<PguiTextCtrl>() != null)
				{
					color = obj.GetComponent<PguiTextCtrl>().m_Text.color;
				}
				obj.GetComponent<PguiDataHolder>().color = color;
			});
			this.baseObj = baseTr.gameObject;
			this.Info_PhotoItemEffect = baseTr.GetComponent<PguiImageCtrl>();
			object obj2 = obj3;
			obj2(this.Info_PhotoItemEffect.gameObject);
			this.Base = baseTr.Find("Base").GetComponent<PguiImageCtrl>();
			obj2(this.Base.gameObject);
			this.All = baseTr.Find("All").GetComponent<SimpleAnimation>();
			this.Grid = this.All.transform.Find("Grid").GetComponent<SimpleAnimation>();
			this.Mark_PhotoEffect = this.All.transform.Find("Mark_PhotoEffect").GetComponent<PguiImageCtrl>();
			this.Mark_PhotoEffect.gameObject.SetActive(true);
			obj2(this.Mark_PhotoEffect.gameObject);
			this.Txt_Title = this.All.transform.Find("Txt_Title").GetComponent<PguiTextCtrl>();
			obj2(this.Txt_Title.gameObject);
			this.iconItems = new List<InfoPhotoItemEffectCtrl.GUI.IconItem>
			{
				new InfoPhotoItemEffectCtrl.GUI.IconItem(this.Grid.transform.Find("Icon_Item01")),
				new InfoPhotoItemEffectCtrl.GUI.IconItem(this.Grid.transform.Find("Icon_Item02"))
			};
			this.Num_Add = this.All.transform.Find("Num_Add").GetComponent<PguiTextCtrl>();
			obj2(this.Num_Add.gameObject);
		}

		public GameObject baseObj;

		public PguiImageCtrl Info_PhotoItemEffect;

		public PguiImageCtrl Base;

		public PguiImageCtrl Mark_PhotoEffect;

		public PguiTextCtrl Txt_Title;

		public SimpleAnimation All;

		public SimpleAnimation Grid;

		public List<InfoPhotoItemEffectCtrl.GUI.IconItem> iconItems;

		public PguiTextCtrl Num_Add;

		public class IconItem
		{
			public IconItem(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.Icon_Item = baseTr.GetComponent<PguiRawImageCtrl>();
				this.Num_Percent = baseTr.Find("Num_Percent").GetComponent<PguiTextCtrl>();
				this.Num_Percent.m_Text.supportRichText = true;
			}

			public void Setup(DataManagerPhoto.CalcDropBonusResult result)
			{
				string text = "+" + ((float)result.ratio / 100f).ToString((result.ratio != 0) ? "#.#" : "F0") + "%";
				if (result.ratio <= 0)
				{
					text = "<color=#000000>" + text + "</color>";
				}
				this.Num_Percent.text = text;
				ItemStaticBase itemStaticBase = DataManager.DmItem.GetItemStaticBase(result.targetItemId);
				if (itemStaticBase != null)
				{
					this.Icon_Item.SetRawImage(itemStaticBase.GetIconName(), true, false, null);
				}
			}

			public GameObject baseObj;

			public PguiRawImageCtrl Icon_Item;

			public PguiTextCtrl Num_Percent;
		}
	}
}
