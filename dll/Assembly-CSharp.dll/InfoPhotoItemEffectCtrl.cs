using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001A9 RID: 425
public class InfoPhotoItemEffectCtrl : MonoBehaviour
{
	// Token: 0x170003F4 RID: 1012
	// (get) Token: 0x06001CA6 RID: 7334 RVA: 0x0016857A File Offset: 0x0016677A
	// (set) Token: 0x06001CA7 RID: 7335 RVA: 0x00168582 File Offset: 0x00166782
	private InfoPhotoItemEffectCtrl.GUI GuiData { get; set; }

	// Token: 0x170003F5 RID: 1013
	// (get) Token: 0x06001CA8 RID: 7336 RVA: 0x0016858B File Offset: 0x0016678B
	// (set) Token: 0x06001CA9 RID: 7337 RVA: 0x00168593 File Offset: 0x00166793
	private bool IsInit { get; set; }

	// Token: 0x170003F6 RID: 1014
	// (get) Token: 0x06001CAB RID: 7339 RVA: 0x001685A5 File Offset: 0x001667A5
	// (set) Token: 0x06001CAA RID: 7338 RVA: 0x0016859C File Offset: 0x0016679C
	private List<DataManagerPhoto.CalcDropBonusResult> PhotoBonusResults { get; set; }

	// Token: 0x170003F7 RID: 1015
	// (get) Token: 0x06001CAD RID: 7341 RVA: 0x001685B6 File Offset: 0x001667B6
	// (set) Token: 0x06001CAC RID: 7340 RVA: 0x001685AD File Offset: 0x001667AD
	private bool SwitchFlag { get; set; }

	// Token: 0x170003F8 RID: 1016
	// (get) Token: 0x06001CAF RID: 7343 RVA: 0x001685C7 File Offset: 0x001667C7
	// (set) Token: 0x06001CAE RID: 7342 RVA: 0x001685BE File Offset: 0x001667BE
	private InfoPhotoItemEffectCtrl.CalcPhotoDropItemResult PhotoDropItemResult { get; set; }

	// Token: 0x170003F9 RID: 1017
	// (get) Token: 0x06001CB1 RID: 7345 RVA: 0x001685D8 File Offset: 0x001667D8
	// (set) Token: 0x06001CB0 RID: 7344 RVA: 0x001685CF File Offset: 0x001667CF
	private List<InfoPhotoItemEffectCtrl.ToggleCtrl> ToggleCtrls { get; set; }

	// Token: 0x170003FA RID: 1018
	// (get) Token: 0x06001CB2 RID: 7346 RVA: 0x001685E0 File Offset: 0x001667E0
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

	// Token: 0x170003FB RID: 1019
	// (get) Token: 0x06001CB3 RID: 7347 RVA: 0x001686A0 File Offset: 0x001668A0
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

	// Token: 0x170003FC RID: 1020
	// (get) Token: 0x06001CB4 RID: 7348 RVA: 0x00168724 File Offset: 0x00166924
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

	// Token: 0x170003FD RID: 1021
	// (get) Token: 0x06001CB5 RID: 7349 RVA: 0x0016875C File Offset: 0x0016695C
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

	// Token: 0x170003FE RID: 1022
	// (get) Token: 0x06001CB6 RID: 7350 RVA: 0x00168832 File Offset: 0x00166A32
	// (set) Token: 0x06001CB7 RID: 7351 RVA: 0x0016883A File Offset: 0x00166A3A
	private int NextIndex { get; set; }

	// Token: 0x170003FF RID: 1023
	// (get) Token: 0x06001CB8 RID: 7352 RVA: 0x00168844 File Offset: 0x00166A44
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

	// Token: 0x17000400 RID: 1024
	// (get) Token: 0x06001CB9 RID: 7353 RVA: 0x001688A4 File Offset: 0x00166AA4
	private bool IsEarnMore
	{
		get
		{
			return this.CurrentType == InfoPhotoItemEffectCtrl.Type.EarnMore;
		}
	}

	// Token: 0x17000401 RID: 1025
	// (get) Token: 0x06001CBA RID: 7354 RVA: 0x001688AF File Offset: 0x00166AAF
	private bool IsLotteryMore
	{
		get
		{
			return this.CurrentType == InfoPhotoItemEffectCtrl.Type.LotteryMore;
		}
	}

	// Token: 0x17000402 RID: 1026
	// (get) Token: 0x06001CBC RID: 7356 RVA: 0x001688C3 File Offset: 0x00166AC3
	// (set) Token: 0x06001CBB RID: 7355 RVA: 0x001688BA File Offset: 0x00166ABA
	private int CurrentToggleIndex { get; set; }

	// Token: 0x06001CBD RID: 7357 RVA: 0x001688CC File Offset: 0x00166ACC
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

	// Token: 0x06001CBE RID: 7358 RVA: 0x001689AC File Offset: 0x00166BAC
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

	// Token: 0x06001CBF RID: 7359 RVA: 0x00168A7C File Offset: 0x00166C7C
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

	// Token: 0x06001CC0 RID: 7360 RVA: 0x00168B10 File Offset: 0x00166D10
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

	// Token: 0x06001CC1 RID: 7361 RVA: 0x00168C94 File Offset: 0x00166E94
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

	// Token: 0x06001CC2 RID: 7362 RVA: 0x00169300 File Offset: 0x00167500
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

	// Token: 0x06001CC3 RID: 7363 RVA: 0x001694A3 File Offset: 0x001676A3
	private void InitForce()
	{
		this.GuiData = new InfoPhotoItemEffectCtrl.GUI(base.transform);
		this.IsInit = true;
	}

	// Token: 0x06001CC4 RID: 7364 RVA: 0x001694BD File Offset: 0x001676BD
	private void Awake()
	{
		this.InitForce();
	}

	// Token: 0x06001CC5 RID: 7365 RVA: 0x001694C5 File Offset: 0x001676C5
	private void Start()
	{
	}

	// Token: 0x06001CC6 RID: 7366 RVA: 0x001694C7 File Offset: 0x001676C7
	private void Update()
	{
		if (this.SwitchFlag)
		{
			this.SwitchFlag = false;
			this.UpdateIcon();
			this.StartToggle(0f);
		}
	}

	// Token: 0x04001560 RID: 5472
	private static readonly float mskColor = 0.6f;

	// Token: 0x04001561 RID: 5473
	private InfoPhotoItemEffectCtrl.SetupParam setupParam = new InfoPhotoItemEffectCtrl.SetupParam();

	// Token: 0x04001565 RID: 5477
	private int animCounter;

	// Token: 0x02000F15 RID: 3861
	// (Invoke) Token: 0x06004E9B RID: 20123
	public delegate void OnUpdateValue<T>(GameObject go, T val);

	// Token: 0x02000F16 RID: 3862
	public enum Type
	{
		// Token: 0x040055BB RID: 21947
		None,
		// Token: 0x040055BC RID: 21948
		EarnMore,
		// Token: 0x040055BD RID: 21949
		LotteryMore
	}

	// Token: 0x02000F17 RID: 3863
	private class ToggleCtrl
	{
		// Token: 0x040055BE RID: 21950
		public InfoPhotoItemEffectCtrl.Type type;
	}

	// Token: 0x02000F18 RID: 3864
	protected class CalcPhotoDropItemResult
	{
		// Token: 0x040055BF RID: 21951
		public int bonusDrawNum;
	}

	// Token: 0x02000F19 RID: 3865
	public class SetupParam
	{
		// Token: 0x040055C0 RID: 21952
		public List<PhotoPackData> photoPackDatas;

		// Token: 0x040055C1 RID: 21953
		public List<PhotoPackData> optionPhotoPackDatas;

		// Token: 0x040055C2 RID: 21954
		public List<GameObject> optionGameObjects;

		// Token: 0x040055C3 RID: 21955
		public InfoPhotoItemEffectCtrl.OnUpdateValue<float> optionFloatValueCB;

		// Token: 0x040055C4 RID: 21956
		public InfoPhotoItemEffectCtrl.OnUpdateValue<DataManagerPhoto.CalcDropBonusResult> optionPhotoBonusResultCB;

		// Token: 0x040055C5 RID: 21957
		public PguiTextCtrl infoText;
	}

	// Token: 0x02000F1A RID: 3866
	private class GUI
	{
		// Token: 0x06004EA1 RID: 20129 RVA: 0x00236D80 File Offset: 0x00234F80
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

		// Token: 0x040055C6 RID: 21958
		public GameObject baseObj;

		// Token: 0x040055C7 RID: 21959
		public PguiImageCtrl Info_PhotoItemEffect;

		// Token: 0x040055C8 RID: 21960
		public PguiImageCtrl Base;

		// Token: 0x040055C9 RID: 21961
		public PguiImageCtrl Mark_PhotoEffect;

		// Token: 0x040055CA RID: 21962
		public PguiTextCtrl Txt_Title;

		// Token: 0x040055CB RID: 21963
		public SimpleAnimation All;

		// Token: 0x040055CC RID: 21964
		public SimpleAnimation Grid;

		// Token: 0x040055CD RID: 21965
		public List<InfoPhotoItemEffectCtrl.GUI.IconItem> iconItems;

		// Token: 0x040055CE RID: 21966
		public PguiTextCtrl Num_Add;

		// Token: 0x020011F6 RID: 4598
		public class IconItem
		{
			// Token: 0x06005768 RID: 22376 RVA: 0x00257624 File Offset: 0x00255824
			public IconItem(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.Icon_Item = baseTr.GetComponent<PguiRawImageCtrl>();
				this.Num_Percent = baseTr.Find("Num_Percent").GetComponent<PguiTextCtrl>();
				this.Num_Percent.m_Text.supportRichText = true;
			}

			// Token: 0x06005769 RID: 22377 RVA: 0x00257678 File Offset: 0x00255878
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

			// Token: 0x0400627F RID: 25215
			public GameObject baseObj;

			// Token: 0x04006280 RID: 25216
			public PguiRawImageCtrl Icon_Item;

			// Token: 0x04006281 RID: 25217
			public PguiTextCtrl Num_Percent;
		}
	}
}
