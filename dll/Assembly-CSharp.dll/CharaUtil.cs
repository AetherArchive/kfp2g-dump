using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000193 RID: 403
public class CharaUtil : MonoBehaviour
{
	// Token: 0x06001ACD RID: 6861 RVA: 0x001583C0 File Offset: 0x001565C0
	public static List<CharaStaticData> CreateNotHaveCharaStaticDataList(List<CharaPackData> haveCharaPackList)
	{
		List<CharaStaticData> tempList = new List<CharaStaticData>();
		using (List<CharaPackData>.Enumerator enumerator = haveCharaPackList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				CharaPackData haveCharaPack = enumerator.Current;
				CharaStaticData charaStaticData = DataManager.DmChara.CharaStaticDataList.Find((CharaStaticData item) => haveCharaPack.staticData.GetId() == item.GetId());
				if (charaStaticData != null)
				{
					tempList.Add(charaStaticData);
				}
			}
		}
		List<CharaStaticData> list = new List<CharaStaticData>(DataManager.DmChara.CharaStaticDataList);
		list.RemoveAll((CharaStaticData item) => tempList.Contains(item));
		list.RemoveAll((CharaStaticData item) => item.baseData.notOpenDispFlg);
		return list;
	}

	// Token: 0x06001ACE RID: 6862 RVA: 0x00158498 File Offset: 0x00156698
	public static int SortCharaName(CharaStaticData a, CharaStaticData b)
	{
		int num = 0;
		char[] array = (a.GetName() + a.baseData.NickName).ToCharArray();
		char[] array2 = (b.GetName() + b.baseData.NickName).ToCharArray();
		int num2 = ((array.Length > array2.Length) ? array2.Length : array.Length);
		for (int i = 0; i < num2; i++)
		{
			num = (int)(array[i] - array2[i]);
			if (num != 0)
			{
				break;
			}
		}
		if (num == 0)
		{
			num = array.Length - array2.Length;
		}
		return num;
	}

	// Token: 0x06001ACF RID: 6863 RVA: 0x0015851C File Offset: 0x0015671C
	public static void SortCharaStaticData(ref List<CharaStaticData> charaStaticDataList)
	{
		charaStaticDataList.Sort((CharaStaticData a, CharaStaticData b) => a.baseData.rankLow - b.baseData.rankLow);
		PrjUtil.InsertionSort<CharaStaticData>(ref charaStaticDataList, (CharaStaticData a, CharaStaticData b) => CharaUtil.SortCharaName(a, b));
	}

	// Token: 0x06001AD0 RID: 6864 RVA: 0x00158574 File Offset: 0x00156774
	public static void FilterCharaStaticData(ref List<CharaStaticData> charaStaticDataList, SortFilterDefine.RegisterType registerType)
	{
		List<CharaDef.AttributeType> attr = CanvasManager.HdlOpenWindowSortFilter.GetAttributeType(registerType);
		charaStaticDataList.RemoveAll((CharaStaticData chara) => attr != null && attr.Count > 0 && !attr.Contains(chara.baseData.attribute));
		List<int> bonusCharaIdList = CanvasManager.HdlOpenWindowSortFilter.GetBonusCharaIdList(registerType);
		charaStaticDataList.RemoveAll((CharaStaticData chara) => bonusCharaIdList != null && bonusCharaIdList.Count > 0 && !bonusCharaIdList.Contains(chara.GetId()));
		bool[] hanamaru = CanvasManager.HdlOpenWindowSortFilter.GetHanamaru(registerType);
		if (hanamaru[0])
		{
			charaStaticDataList.RemoveAll((CharaStaticData chara) => chara.baseData.OriginalId == 0);
		}
		if (hanamaru[1])
		{
			charaStaticDataList.RemoveAll((CharaStaticData chara) => chara.baseData.OriginalId != 0);
		}
		if (CanvasManager.HdlOpenWindowSortFilter.GetFavorite(registerType)[0])
		{
			charaStaticDataList.Clear();
		}
		List<DataManagerChara.FilterData> miracleTargetList = CanvasManager.HdlOpenWindowSortFilter.GetMiracleTargetList(registerType);
		CharaUtil.FriendsMiracleTarget(ref charaStaticDataList, miracleTargetList);
		List<DataManagerChara.FilterData> miracleEffectList = CanvasManager.HdlOpenWindowSortFilter.GetMiracleEffectList(registerType);
		SortFilterDefine.AndOrState miracleAndOrStatus = CanvasManager.HdlOpenWindowSortFilter.GetMiracleAndOrStatus(registerType);
		CharaUtil.FriendsMiracleEffect(ref charaStaticDataList, miracleAndOrStatus, miracleEffectList);
		List<DataManagerChara.FilterData> characteristicConditionList = CanvasManager.HdlOpenWindowSortFilter.GetCharacteristicConditionList(registerType);
		CharaUtil.FriendsCharacteristicCondition(ref charaStaticDataList, characteristicConditionList);
		List<DataManagerChara.FilterData> characteristicTargetList = CanvasManager.HdlOpenWindowSortFilter.GetCharacteristicTargetList(registerType);
		CharaUtil.FriendsCharacteristicTarget(ref charaStaticDataList, characteristicTargetList);
		List<DataManagerChara.FilterData> characteristicEffectList = CanvasManager.HdlOpenWindowSortFilter.GetCharacteristicEffectList(registerType);
		SortFilterDefine.AndOrState characteristicEffectAndOrStatus = CanvasManager.HdlOpenWindowSortFilter.GetCharacteristicEffectAndOrStatus(registerType);
		CharaUtil.FriendsCharacteristicEffect(ref charaStaticDataList, characteristicEffectAndOrStatus, characteristicEffectList);
		List<DataManagerChara.FilterData> characteristicResistList = CanvasManager.HdlOpenWindowSortFilter.GetCharacteristicResistList(registerType);
		SortFilterDefine.AndOrState characteristicResistAndOrStatus = CanvasManager.HdlOpenWindowSortFilter.GetCharacteristicResistAndOrStatus(registerType);
		CharaUtil.FriendsCharacteristicResist(ref charaStaticDataList, characteristicResistAndOrStatus, characteristicResistList);
	}

	// Token: 0x06001AD1 RID: 6865 RVA: 0x001586F0 File Offset: 0x001568F0
	public static void OnUpdateChara(int index, GameObject go, List<CharaPackData> dispCharaPackList, List<CharaStaticData> notHaveCharaStaticDataList, SortFilterDefine.SortType sortType, GameObject Txt_CharaSelect, bool isCharaQuest)
	{
		GridLayoutGroup component = go.GetComponent<GridLayoutGroup>();
		component.padding.top = 0;
		bool flag = false;
		for (int i = 0; i < 3; i++)
		{
			int num = index * 3 + i;
			Transform transform = go.transform.Find(i.ToString());
			CharaStaticData charaStaticData = null;
			int num2 = num - dispCharaPackList.Count;
			if (0 <= num2 && num2 < notHaveCharaStaticDataList.Count)
			{
				charaStaticData = notHaveCharaStaticDataList[num2];
			}
			bool flag2 = charaStaticData == null;
			IconCharaCtrl component2 = transform.GetComponent<IconCharaCtrl>();
			if (num < dispCharaPackList.Count)
			{
				component2.Setup((dispCharaPackList[num].id != 0) ? dispCharaPackList[num] : null, sortType, false, null, 0, -1, 0);
				if (isCharaQuest)
				{
					if (dispCharaPackList[num].id != 0)
					{
						component2.SetupStoryInfo(dispCharaPackList[num], flag2);
					}
					component2.DispMarkEvent(false, false, false);
				}
				else
				{
					component2.DispLevel(true);
				}
				component2.DispPhotoPocketLevel(true);
			}
			else if (num < dispCharaPackList.Count + 3 && charaStaticData != null)
			{
				CharaPackData charaPackData = CharaPackData.MakeInitial(charaStaticData.GetId());
				component2.Setup(charaPackData, SortFilterDefine.SortType.LEVEL, !isCharaQuest, new CharaWindowCtrl.DetailParamSetting(CharaWindowCtrl.DetailParamSetting.Preset.DISPLAY, null), 0, -1, 0);
				if (isCharaQuest)
				{
					component2.SetupStoryInfo(charaPackData, flag2);
					component2.DispMarkEvent(false, false, false);
				}
				else
				{
					component2.DispLevel(false);
				}
				flag = true;
				if (num2 == 0 && Txt_CharaSelect != null)
				{
					Txt_CharaSelect.transform.SetParent(transform.transform);
					RectTransform rectTransform = Txt_CharaSelect.transform as RectTransform;
					rectTransform.anchorMin = new Vector2(0f, 0.5f);
					rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
					rectTransform.offsetMin = new Vector2(0f, 0f);
					rectTransform.offsetMax = new Vector2(0f, 0f);
					rectTransform.pivot = new Vector2(0f, 0.5f);
					rectTransform.anchoredPosition = new Vector2(0f, 84f);
				}
			}
			else if (num < dispCharaPackList.Count + notHaveCharaStaticDataList.Count && charaStaticData != null)
			{
				CharaPackData charaPackData2 = CharaPackData.MakeInitial(charaStaticData.GetId());
				component2.Setup(charaPackData2, SortFilterDefine.SortType.LEVEL, !isCharaQuest, new CharaWindowCtrl.DetailParamSetting(CharaWindowCtrl.DetailParamSetting.Preset.DISPLAY, null), 0, -1, 0);
				if (isCharaQuest)
				{
					component2.SetupStoryInfo(charaPackData2, flag2);
					component2.DispMarkEvent(false, false, false);
				}
				else
				{
					component2.DispLevel(false);
				}
				flag = true;
			}
			else
			{
				transform.GetComponent<IconCharaCtrl>().Setup(null, SortFilterDefine.SortType.LEVEL, false, null, 0, -1, 0);
			}
		}
		if (flag)
		{
			component.padding.top = CharaUtil.SCROLL_ITEM_ADD_SIZE;
		}
		if (Txt_CharaSelect != null)
		{
			IconCharaCtrl componentInParent = Txt_CharaSelect.GetComponentInParent<IconCharaCtrl>();
			int num3 = ((componentInParent != null) ? componentInParent.charaPackData.id : 0);
			if (notHaveCharaStaticDataList != null)
			{
				Txt_CharaSelect.gameObject.SetActive(notHaveCharaStaticDataList.Count > 0 && notHaveCharaStaticDataList[0].GetId() == num3);
			}
		}
		component.enabled = false;
		component.enabled = true;
	}

	// Token: 0x06001AD2 RID: 6866 RVA: 0x001589E8 File Offset: 0x00156BE8
	public static void SetupRectTransformOutScreenRange(GameObject go, Vector2 pos)
	{
		RectTransform rectTransform = go.transform as RectTransform;
		rectTransform.anchorMin = new Vector2(0f, 0.5f);
		rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
		rectTransform.offsetMin = new Vector2(0f, 0f);
		rectTransform.offsetMax = new Vector2(0f, 0f);
		rectTransform.pivot = new Vector2(0f, 0.5f);
		rectTransform.anchoredPosition = pos;
	}

	// Token: 0x06001AD3 RID: 6867 RVA: 0x00158A70 File Offset: 0x00156C70
	public static void FriendsMiracleTarget(ref List<CharaStaticData> charaStaticDataList, List<DataManagerChara.FilterData> miracleTargetList)
	{
		List<CharaStaticData> list = new List<CharaStaticData>();
		using (List<CharaStaticData>.Enumerator enumerator = charaStaticDataList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				CharaStaticData charaStaticData = enumerator.Current;
				Predicate<CharaStaticData> <>9__2;
				foreach (DataManagerChara.FilterData filterData in miracleTargetList)
				{
					using (List<DataManagerChara.FilterData.FilterElementOne>.Enumerator enumerator3 = filterData.ElementList.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							DataManagerChara.FilterData.FilterElementOne element = enumerator3.Current;
							CharaStaticAction artsData = charaStaticData.artsData;
							CharaDamageParam charaDamageParam = artsData.damageList.Find((CharaDamageParam attack) => attack.targetType == element.TargetType);
							CharaBuffParam charaBuffParam = artsData.buffList.Find((CharaBuffParam buff) => buff.targetType == element.TargetType);
							if (charaDamageParam != null || charaBuffParam != null)
							{
								List<CharaStaticData> list2 = list;
								Predicate<CharaStaticData> predicate;
								if ((predicate = <>9__2) == null)
								{
									predicate = (<>9__2 = (CharaStaticData item) => item == charaStaticData);
								}
								if (list2.Find(predicate) == null)
								{
									list.Add(charaStaticData);
									break;
								}
								break;
							}
						}
					}
				}
			}
		}
		if (0 < miracleTargetList.Count)
		{
			charaStaticDataList = list;
		}
	}

	// Token: 0x06001AD4 RID: 6868 RVA: 0x00158C08 File Offset: 0x00156E08
	private static void FriendsMiracleEffect(ref List<CharaStaticData> charaStaticDataList, SortFilterDefine.AndOrState miracleEffectAndOrStatus, List<DataManagerChara.FilterData> miracleEffectList)
	{
		switch (miracleEffectAndOrStatus)
		{
		case SortFilterDefine.AndOrState.Invalid:
			break;
		case SortFilterDefine.AndOrState.And:
			CharaUtil.FriendsMiracleEffectAnd(ref charaStaticDataList, miracleEffectList);
			return;
		case SortFilterDefine.AndOrState.Or:
			CharaUtil.FriendsMiracleEffectOr(ref charaStaticDataList, miracleEffectList);
			break;
		default:
			return;
		}
	}

	// Token: 0x06001AD5 RID: 6869 RVA: 0x00158C2C File Offset: 0x00156E2C
	private static void FriendsMiracleEffectAnd(ref List<CharaStaticData> charaStaticDataList, List<DataManagerChara.FilterData> miracleEffectList)
	{
		if (miracleEffectList.Count <= 0)
		{
			return;
		}
		List<CharaStaticData> list = new List<CharaStaticData>();
		foreach (CharaStaticData charaStaticData in charaStaticDataList)
		{
			int num = 0;
			if (CharaUtil.isCharaHaveAttack(charaStaticData, miracleEffectList))
			{
				num++;
			}
			foreach (DataManagerChara.FilterData filterData in miracleEffectList)
			{
				using (List<DataManagerChara.FilterData.FilterElementOne>.Enumerator enumerator3 = filterData.ElementList.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						DataManagerChara.FilterData.FilterElementOne element = enumerator3.Current;
						if (charaStaticData.artsData.buffList.Find((CharaBuffParam buff) => buff.buffType == element.BuffType) != null)
						{
							num++;
							break;
						}
					}
				}
			}
			if (num == miracleEffectList.Count)
			{
				list.Add(charaStaticData);
			}
		}
		charaStaticDataList = list;
	}

	// Token: 0x06001AD6 RID: 6870 RVA: 0x00158D50 File Offset: 0x00156F50
	private static void FriendsMiracleEffectOr(ref List<CharaStaticData> charaStaicDataList, List<DataManagerChara.FilterData> miracleEffectList)
	{
		if (miracleEffectList.Count <= 0)
		{
			return;
		}
		List<CharaStaticData> list = new List<CharaStaticData>();
		foreach (CharaStaticData charaStaticData in charaStaicDataList)
		{
			bool flag = false;
			foreach (DataManagerChara.FilterData filterData in miracleEffectList)
			{
				using (List<DataManagerChara.FilterData.FilterElementOne>.Enumerator enumerator3 = filterData.ElementList.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						DataManagerChara.FilterData.FilterElementOne element = enumerator3.Current;
						if (charaStaticData.artsData.buffList.Find((CharaBuffParam buff) => buff.buffType == element.BuffType) != null || CharaUtil.isCharaHaveAttack(charaStaticData, miracleEffectList))
						{
							list.Add(charaStaticData);
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					break;
				}
			}
		}
		charaStaicDataList = list;
	}

	// Token: 0x06001AD7 RID: 6871 RVA: 0x00158E6C File Offset: 0x0015706C
	private static bool isCharaHaveAttack(CharaStaticData charaStaticData, List<DataManagerChara.FilterData> miracleEffectList)
	{
		bool flag = false;
		foreach (DataManagerChara.FilterData filterData in miracleEffectList)
		{
			using (List<DataManagerChara.FilterData.FilterElementOne>.Enumerator enumerator2 = filterData.ElementList.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.BuffType == CharaDef.ActionBuffType.ATTACK)
					{
						flag = true;
					}
				}
			}
		}
		return flag && charaStaticData.artsData.damageList.Count != 0;
	}

	// Token: 0x06001AD8 RID: 6872 RVA: 0x00158F14 File Offset: 0x00157114
	private static void FriendsCharacteristicCondition(ref List<CharaStaticData> charaStaticDataList, List<DataManagerChara.FilterData> conditionList)
	{
		if (conditionList.Count <= 0)
		{
			return;
		}
		List<CharaStaticData> list = new List<CharaStaticData>();
		Dictionary<int, CharaPackData> userCharaMap = DataManager.DmChara.GetUserCharaMap();
		List<CharaPackData> list2 = new List<CharaPackData>();
		foreach (KeyValuePair<int, CharaPackData> keyValuePair in userCharaMap)
		{
			list2.Add(keyValuePair.Value);
		}
		foreach (CharaStaticData charaStaticData in CharaUtil.CreateNotHaveCharaStaticDataList(list2))
		{
			bool flag = false;
			foreach (DataManagerChara.FilterData filterData in conditionList)
			{
				if (flag)
				{
					break;
				}
				foreach (DataManagerChara.FilterData.FilterElementOne filterElementOne in filterData.ElementList)
				{
					foreach (CharaBuffParamAbility charaBuffParamAbility in CharaUtil.GetCharaAbilityList(charaStaticData))
					{
						switch (filterElementOne.FilterType)
						{
						case SortFilterDefine.FilterElementType.Condition:
							if (filterElementOne.Condition != CharaDef.ConditionType.EQUAL && !flag)
							{
								if (charaBuffParamAbility.condition == CharaDef.ConditionType.UPPER && charaBuffParamAbility.conditionHpRate != 0)
								{
									list.Add(charaStaticData);
									flag = true;
								}
								else if (charaBuffParamAbility.condition == CharaDef.ConditionType.LOWER && 100 != charaBuffParamAbility.conditionHpRate)
								{
									list.Add(charaStaticData);
									flag = true;
								}
							}
							break;
						case SortFilterDefine.FilterElementType.Terrain:
							if (!flag && charaBuffParamAbility.traitsTerrain == filterElementOne.Traits)
							{
								list.Add(charaStaticData);
								flag = true;
							}
							break;
						case SortFilterDefine.FilterElementType.Night:
							if (!flag && charaBuffParamAbility.traitsTimezone == CharaDef.AbilityTraits2.night)
							{
								list.Add(charaStaticData);
								flag = true;
							}
							break;
						}
					}
				}
			}
		}
		List<CharaStaticData> list3 = new List<CharaStaticData>();
		using (List<CharaStaticData>.Enumerator enumerator2 = charaStaticDataList.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				CharaStaticData charaPackData = enumerator2.Current;
				if (list.Find((CharaStaticData data) => data.GetId() == charaPackData.GetId()) != null)
				{
					list3.Add(charaPackData);
				}
			}
		}
		charaStaticDataList = list3;
	}

	// Token: 0x06001AD9 RID: 6873 RVA: 0x00159204 File Offset: 0x00157404
	private static void FriendsCharacteristicTarget(ref List<CharaStaticData> charaPackDataList, List<DataManagerChara.FilterData> targetList)
	{
		if (targetList.Count <= 0)
		{
			return;
		}
		List<CharaStaticData> list = new List<CharaStaticData>();
		foreach (CharaStaticData charaStaticData in charaPackDataList)
		{
			bool flag = false;
			List<CharaBuffParamAbility> buffList = charaStaticData.abilityData[0].buffList;
			foreach (DataManagerChara.FilterData filterData in targetList)
			{
				if (flag)
				{
					break;
				}
				using (List<DataManagerChara.FilterData.FilterElementOne>.Enumerator enumerator3 = filterData.ElementList.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						DataManagerChara.FilterData.FilterElementOne element = enumerator3.Current;
						if (CharaUtil.GetCharaAbilityList(charaStaticData).Find((CharaBuffParamAbility item) => item.targetType == element.TargetType) != null)
						{
							list.Add(charaStaticData);
							flag = true;
							break;
						}
					}
				}
			}
		}
		charaPackDataList = list;
	}

	// Token: 0x06001ADA RID: 6874 RVA: 0x00159328 File Offset: 0x00157528
	private static void FriendsCharacteristicEffect(ref List<CharaStaticData> charaStaticDataList, SortFilterDefine.AndOrState andOrStatus, List<DataManagerChara.FilterData> effectList)
	{
		if (effectList.Count <= 0)
		{
			return;
		}
		switch (andOrStatus)
		{
		case SortFilterDefine.AndOrState.Invalid:
			break;
		case SortFilterDefine.AndOrState.And:
			CharaUtil.FriendsCharacteristicEffectAnd(ref charaStaticDataList, effectList);
			return;
		case SortFilterDefine.AndOrState.Or:
			CharaUtil.FriendsCharacteristicEffectOr(ref charaStaticDataList, effectList);
			break;
		default:
			return;
		}
	}

	// Token: 0x06001ADB RID: 6875 RVA: 0x00159358 File Offset: 0x00157558
	private static void FriendsCharacteristicEffectAnd(ref List<CharaStaticData> charaPackDataList, List<DataManagerChara.FilterData> effectList)
	{
		List<CharaStaticData> list = new List<CharaStaticData>();
		foreach (CharaStaticData charaStaticData in charaPackDataList)
		{
			int num = 0;
			foreach (DataManagerChara.FilterData filterData in effectList)
			{
				using (List<DataManagerChara.FilterData.FilterElementOne>.Enumerator enumerator3 = filterData.ElementList.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						DataManagerChara.FilterData.FilterElementOne element = enumerator3.Current;
						if (CharaUtil.GetCharaAbilityList(charaStaticData).Find((CharaBuffParamAbility buff) => buff.buffType == element.BuffType) != null)
						{
							num++;
							break;
						}
					}
				}
			}
			if (num == effectList.Count)
			{
				list.Add(charaStaticData);
			}
		}
		charaPackDataList = list;
	}

	// Token: 0x06001ADC RID: 6876 RVA: 0x00159460 File Offset: 0x00157660
	private static void FriendsCharacteristicEffectOr(ref List<CharaStaticData> charaPackDataList, List<DataManagerChara.FilterData> effectList)
	{
		List<CharaStaticData> list = new List<CharaStaticData>();
		foreach (CharaStaticData charaStaticData in charaPackDataList)
		{
			bool flag = false;
			foreach (DataManagerChara.FilterData filterData in effectList)
			{
				if (flag)
				{
					break;
				}
				using (List<DataManagerChara.FilterData.FilterElementOne>.Enumerator enumerator3 = filterData.ElementList.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						DataManagerChara.FilterData.FilterElementOne element = enumerator3.Current;
						if (CharaUtil.GetCharaAbilityList(charaStaticData).Find((CharaBuffParamAbility buff) => buff.buffType == element.BuffType) != null)
						{
							list.Add(charaStaticData);
							flag = true;
							break;
						}
					}
				}
			}
		}
		charaPackDataList = list;
	}

	// Token: 0x06001ADD RID: 6877 RVA: 0x00159568 File Offset: 0x00157768
	private static void FriendsCharacteristicResist(ref List<CharaStaticData> charaPackDataList, SortFilterDefine.AndOrState andOrState, List<DataManagerChara.FilterData> resistList)
	{
		if (resistList.Count <= 0)
		{
			return;
		}
		long num = 0L;
		foreach (DataManagerChara.FilterData filterData in resistList)
		{
			foreach (DataManagerChara.FilterData.FilterElementOne filterElementOne in filterData.ElementList)
			{
				num |= CharaDef.AbnormalMask(filterElementOne.AbnormalMask, filterElementOne.AbnormalMask2);
			}
		}
		List<CharaStaticData> list = new List<CharaStaticData>();
		foreach (CharaStaticData charaStaticData in charaPackDataList)
		{
			long num2 = 0L;
			foreach (CharaBuffParamAbility charaBuffParamAbility in CharaUtil.GetCharaAbilityList(charaStaticData))
			{
				num2 |= charaBuffParamAbility.abTyp;
			}
			switch (andOrState)
			{
			case SortFilterDefine.AndOrState.Invalid:
				return;
			case SortFilterDefine.AndOrState.And:
				if ((num & num2) == num)
				{
					list.Add(charaStaticData);
				}
				break;
			case SortFilterDefine.AndOrState.Or:
				if ((num & num2) != 0L)
				{
					list.Add(charaStaticData);
				}
				break;
			}
		}
		charaPackDataList = list;
	}

	// Token: 0x06001ADE RID: 6878 RVA: 0x001596D8 File Offset: 0x001578D8
	private static List<CharaBuffParamAbility> GetCharaAbilityList(CharaStaticData charaData)
	{
		List<CharaBuffParamAbility> list = new List<CharaBuffParamAbility>();
		foreach (CharaStaticAbility charaStaticAbility in charaData.abilityData)
		{
			foreach (CharaBuffParamAbility charaBuffParamAbility in charaStaticAbility.buffList)
			{
				list.Add(charaBuffParamAbility);
			}
		}
		if (charaData.spAbilityData != null)
		{
			using (List<CharaBuffParamAbility>.Enumerator enumerator2 = charaData.spAbilityData.buffList.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					CharaBuffParamAbility spBuff = enumerator2.Current;
					if (list.Find((CharaBuffParamAbility buff) => buff == spBuff) == null)
					{
						list.Add(spBuff);
					}
				}
			}
		}
		if (charaData.nanairoAbilityData != null)
		{
			using (List<CharaBuffParamAbility>.Enumerator enumerator2 = charaData.nanairoAbilityData.buffList.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					CharaBuffParamAbility nanairoBuff = enumerator2.Current;
					if (list.Find((CharaBuffParamAbility buff) => buff == nanairoBuff) == null)
					{
						list.Add(nanairoBuff);
					}
				}
			}
		}
		return list;
	}

	// Token: 0x04001463 RID: 5219
	private static readonly int SCROLL_ITEM_ADD_SIZE = 30;

	// Token: 0x02000E86 RID: 3718
	public class GUISkillInfo
	{
		// Token: 0x06004CF0 RID: 19696 RVA: 0x0022FC74 File Offset: 0x0022DE74
		public GUISkillInfo(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Txt_Kind = baseTr.Find("Txt_Kind").GetComponent<PguiTextCtrl>();
			this.Txt_Name = baseTr.Find("Txt_Name").GetComponent<PguiTextCtrl>();
			this.Num_Lv = baseTr.Find("Num_Lv").GetComponent<PguiTextCtrl>();
			Transform transform = baseTr.Find("Icon_OrderCard");
			if (transform != null)
			{
				this.Icon_OrderCard = transform.gameObject;
				this.Icon_OrderCard.SetActive(false);
			}
			Transform transform2 = baseTr.Find("Txt_Info");
			if (transform2 == null)
			{
				transform2 = baseTr.Find("ScrollView/Viewport/Content/Txt_Info");
			}
			this.Txt_Info = transform2.GetComponent<PguiTextCtrl>();
			Transform transform3 = baseTr.Find("Disable");
			if (transform3 != null)
			{
				this.Disable = transform3.gameObject;
				this.markLockCtrl = transform3.Find("Mark_Lock").GetComponent<MarkLockCtrl>();
			}
			Transform transform4 = baseTr.Find("Num_Mp");
			if (transform4 != null)
			{
				this.Num_Mp = transform4.GetComponent<PguiTextCtrl>();
			}
		}

		// Token: 0x06004CF1 RID: 19697 RVA: 0x0022FD88 File Offset: 0x0022DF88
		public void Setup(CharaUtil.GUISkillInfo.SetupParam setupParam)
		{
			List<string> list = new List<string> { "けものミラクル", "とくいわざ", "たいきスキル", "とくせい", "キセキとくせい", "なないろとくせい" };
			List<string> list2 = new List<string>
			{
				setupParam.charaPackData.staticData.artsData.actionName,
				setupParam.charaPackData.staticData.specialAttackData.actionName,
				setupParam.charaPackData.staticData.waitActionData.skillName,
				setupParam.charaPackData.staticData.abilityData[0].abilityName,
				(setupParam.charaPackData.staticData.spAbilityData == null) ? "" : setupParam.charaPackData.staticData.spAbilityData.abilityName,
				(setupParam.charaPackData.staticData.nanairoAbilityData == null) ? "" : setupParam.charaPackData.staticData.nanairoAbilityData.abilityName
			};
			if (setupParam.type < (CharaUtil.GUISkillInfo.Type)list.Count)
			{
				this.Txt_Name.text = list2[(int)setupParam.type];
				this.Txt_Kind.text = list[(int)setupParam.type];
				this.Num_Lv.gameObject.SetActive(false);
				if (this.Icon_OrderCard != null)
				{
					this.Icon_OrderCard.SetActive(false);
				}
				if (setupParam.type == CharaUtil.GUISkillInfo.Type.KemonoMiracle)
				{
					bool miracleAvailable = setupParam.charaPackData.dynamicData.miracleAvailable;
					if (!miracleAvailable)
					{
						this.Txt_Name.text = "－";
					}
					this.Txt_Info.text = (miracleAvailable ? setupParam.charaPackData.staticData.artsData.MakeSkillText(setupParam.maxDisp ? 5 : (setupParam.charaPackData.dynamicData.artsLevel + setupParam.offsetKemonoMiracleLv)) : "※ミラクルポイントを獲得できません");
					this.Num_Lv.gameObject.SetActive(true);
					string text;
					if (miracleAvailable)
					{
						text = (setupParam.maxDisp ? string.Format("{0}", 5) : string.Format("{0}", setupParam.charaPackData.dynamicData.artsLevel + setupParam.offsetKemonoMiracleLv));
					}
					else
					{
						text = "－";
					}
					this.Num_Lv.ReplaceTextByDefault("Param01", text);
					if (this.Icon_OrderCard != null && miracleAvailable)
					{
						this.Icon_OrderCard.SetActive(setupParam.maxDisp || setupParam.charaPackData.dynamicData.isArtsMaxEnable);
						PguiReplaceSpriteCtrl component = this.Icon_OrderCard.GetComponent<PguiReplaceSpriteCtrl>();
						switch (setupParam.charaPackData.staticData.artsData.authParam.SynergyFlag)
						{
						case CharaDef.OrderCardType.BEAT:
							component.Replace(1);
							break;
						case CharaDef.OrderCardType.ACTION:
							component.Replace(3);
							break;
						case CharaDef.OrderCardType.TRY:
							component.Replace(2);
							break;
						}
					}
					if (this.Num_Mp != null)
					{
						this.Num_Mp.gameObject.SetActive(true);
						this.Num_Mp.ReplaceTextByDefault("Param01", string.Format("{0}", setupParam.charaPackData.staticData.baseData.maxStockMp));
						return;
					}
				}
				else if (setupParam.type == CharaUtil.GUISkillInfo.Type.SpecialtyAttack)
				{
					this.Txt_Info.text = setupParam.charaPackData.staticData.specialAttackData.actionEffect;
					if (this.Num_Mp != null)
					{
						this.Num_Mp.gameObject.SetActive(false);
						return;
					}
				}
				else
				{
					if (setupParam.type == CharaUtil.GUISkillInfo.Type.WaitAction)
					{
						this.Txt_Info.text = setupParam.charaPackData.staticData.waitActionData.skillEffect;
						return;
					}
					if (setupParam.type == CharaUtil.GUISkillInfo.Type.Ability)
					{
						this.Txt_Info.text = setupParam.charaPackData.staticData.abilityData[0].abilityEffect;
						return;
					}
					if (setupParam.type == CharaUtil.GUISkillInfo.Type.SpecialtyAbility && setupParam.charaPackData.IsHaveSpAbility)
					{
						this.baseObj.SetActive(true);
						this.Txt_Info.text = setupParam.charaPackData.staticData.spAbilityData.abilityEffect;
						return;
					}
					if (setupParam.type == CharaUtil.GUISkillInfo.Type.NanairoAbility && setupParam.charaPackData.IsHaveNanairoAbility)
					{
						this.baseObj.SetActive(true);
						this.Txt_Info.text = setupParam.charaPackData.staticData.nanairoAbilityData.abilityEffect;
					}
				}
			}
		}

		// Token: 0x0400536C RID: 21356
		public GameObject baseObj;

		// Token: 0x0400536D RID: 21357
		public PguiTextCtrl Txt_Kind;

		// Token: 0x0400536E RID: 21358
		public PguiTextCtrl Txt_Name;

		// Token: 0x0400536F RID: 21359
		public PguiTextCtrl Num_Lv;

		// Token: 0x04005370 RID: 21360
		public GameObject Icon_OrderCard;

		// Token: 0x04005371 RID: 21361
		public PguiTextCtrl Txt_Info;

		// Token: 0x04005372 RID: 21362
		public GameObject Disable;

		// Token: 0x04005373 RID: 21363
		public MarkLockCtrl markLockCtrl;

		// Token: 0x04005374 RID: 21364
		public PguiTextCtrl Num_Mp;

		// Token: 0x020011E6 RID: 4582
		public enum Type
		{
			// Token: 0x0400620E RID: 25102
			KemonoMiracle,
			// Token: 0x0400620F RID: 25103
			SpecialtyAttack,
			// Token: 0x04006210 RID: 25104
			WaitAction,
			// Token: 0x04006211 RID: 25105
			Ability,
			// Token: 0x04006212 RID: 25106
			SpecialtyAbility,
			// Token: 0x04006213 RID: 25107
			NanairoAbility
		}

		// Token: 0x020011E7 RID: 4583
		public class SetupParam
		{
			// Token: 0x04006214 RID: 25108
			public CharaUtil.GUISkillInfo.Type type;

			// Token: 0x04006215 RID: 25109
			public CharaPackData charaPackData;

			// Token: 0x04006216 RID: 25110
			public bool maxDisp;

			// Token: 0x04006217 RID: 25111
			public int offsetKemonoMiracleLv;
		}
	}
}
