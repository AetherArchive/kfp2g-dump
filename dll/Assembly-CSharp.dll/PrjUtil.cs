using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using LitJson;
using SGNFW.Login;
using SGNFW.Mst;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PrjUtil
{
	public static IEnumerator Wait(float second)
	{
		float timeSinceStartup = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup - timeSinceStartup < second)
		{
			yield return null;
		}
		yield break;
	}

	public static Color GetColorByCode(string code)
	{
		Color color = Color.white;
		if (!ColorUtility.TryParseHtmlString(code, out color))
		{
			color = Color.white;
		}
		return color;
	}

	public static string MakeMessage(string message)
	{
		return message;
	}

	public static string ModifiedName(string name)
	{
		int num = 10;
		string text = name.Replace("<", "＜");
		text = text.Replace(">", "＞");
		return text.Substring(0, Mathf.Min(text.Length, num));
	}

	public static string ModifiedComment(string comment)
	{
		return comment.Replace(" ", "\u3000");
	}

	public static string ModifiedPartyName(string name)
	{
		int num = 15;
		string text = name.Replace("<", "＜");
		text = text.Replace(">", "＞");
		return text.Substring(0, Mathf.Min(text.Length, num));
	}

	public static string ModifiedRoomName(string name)
	{
		int num = 20;
		string text = name.Replace("<", "＜");
		text = text.Replace(">", "＞");
		return text.Substring(0, Mathf.Min(text.Length, num));
	}

	public static string FbxPropertiesFilePath(string fbxPath)
	{
		string directoryName = Path.GetDirectoryName(fbxPath);
		string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fbxPath);
		return directoryName + "/" + fileNameWithoutExtension + ".txt";
	}

	public static Dictionary<string, List<MayaClip>> ReadFbxProperties(string fbxPath)
	{
		Dictionary<string, List<MayaClip>> dictionary = new Dictionary<string, List<MayaClip>>();
		string text = PrjUtil.FbxPropertiesFilePath(fbxPath);
		if (File.Exists(text))
		{
			using (StreamReader streamReader = new StreamReader(text, Encoding.GetEncoding("shift_jis")))
			{
				if (streamReader != null)
				{
					while (!streamReader.EndOfStream)
					{
						string text2 = streamReader.ReadLine();
						int num = text2.IndexOf(',');
						if (num > 0)
						{
							string text3 = text2.Substring(0, num);
							List<MayaClip> list = JsonMapper.ToObject<List<MayaClip>>(text2.Substring(num + 1));
							dictionary.Add(text3, list);
						}
					}
					streamReader.Close();
				}
			}
		}
		return dictionary;
	}

	public static void AddTouchEventTrigger(RectTransform target, UnityAction<Transform> callback)
	{
		ClickEventTrigger clickEventTrigger = target.GetComponent<ClickEventTrigger>();
		if (clickEventTrigger == null)
		{
			clickEventTrigger = target.gameObject.AddComponent<ClickEventTrigger>();
		}
		clickEventTrigger.callback = callback;
		Graphic graphic = target.GetComponent<Graphic>();
		if (graphic == null)
		{
			graphic = target.gameObject.AddComponent<PguiCollider>();
		}
		graphic.raycastTarget = true;
	}

	public static void AddTouchEventTrigger(GameObject target, UnityAction<Transform> callback)
	{
		RectTransform rectTransform = target.transform as RectTransform;
		if (rectTransform != null)
		{
			PrjUtil.AddTouchEventTrigger(rectTransform, callback);
		}
	}

	public static void RemoveTouchEventTrigger(RectTransform target)
	{
		ClickEventTrigger clickEventTrigger = target.GetComponent<ClickEventTrigger>();
		if (clickEventTrigger == null)
		{
			clickEventTrigger = target.gameObject.AddComponent<ClickEventTrigger>();
		}
		clickEventTrigger.callback = null;
	}

	public static void RemoveTouchEventTrigger(GameObject target)
	{
		RectTransform rectTransform = target.transform as RectTransform;
		if (rectTransform != null)
		{
			PrjUtil.RemoveTouchEventTrigger(rectTransform);
		}
	}

	private static int CalcParamInternal(int lvNow, int lvMax, int prmMin, int prmMax, int prmMid, int lvMid)
	{
		int num2;
		if (lvNow < lvMid)
		{
			float num = ((lvNow == lvMid) ? 1f : (((float)lvNow - 1f) / ((float)lvMid - 1f)));
			num2 = (int)Mathf.Lerp((float)prmMin, (float)prmMid, num);
		}
		else
		{
			float num3 = ((lvNow == lvMax) ? 1f : (((float)lvNow - (float)lvMid) / ((float)lvMax - (float)lvMid)));
			num2 = (int)Mathf.Lerp((float)prmMid, (float)prmMax, num3);
		}
		return num2;
	}

	public static PrjUtil.ParamPreset CalcParamByChara(CharaDynamicData cdd, int level, int rank)
	{
		return PrjUtil.CalcParamByChara(cdd.id, level, rank, cdd.promoteNum, cdd.promoteFlag, cdd.haveClothesIdList, null, cdd.PhotoPocket, null, null, null);
	}

	public static PrjUtil.ParamPreset CalcParamByChara(CharaDynamicData cdd, int level, int rank, int promoteNum, List<bool> promoteFlagList)
	{
		return PrjUtil.CalcParamByChara(cdd.id, level, rank, promoteNum, promoteFlagList, cdd.haveClothesIdList, null, cdd.PhotoPocket, null, null, null);
	}

	public static PrjUtil.ParamPreset CalcParamByChara(CharaDynamicData cdd, List<PhotoPackData> equipPhotoList = null, List<DataManagerChara.BonusCharaData> bonusCharaList = null, PrjUtil.ParamPreset activeKizunaBuff = null)
	{
		return PrjUtil.CalcParamByChara(cdd.id, cdd.level, cdd.rank, cdd.promoteNum, cdd.promoteFlag, cdd.haveClothesIdList, null, cdd.PhotoPocket, equipPhotoList, bonusCharaList, activeKizunaBuff);
	}

	public static PrjUtil.ParamPreset CalcBattleParamByChara(CharaDynamicData cdd, List<PhotoPackData> equipPhotoList = null, List<DataManagerChara.BonusCharaData> bonusCharaList = null, PrjUtil.ParamPreset activeKizunaBuff = null)
	{
		return PrjUtil.CalcParamByChara(cdd.id, cdd.level, cdd.rank, cdd.promoteNum, cdd.promoteFlag, cdd.haveClothesIdList, cdd.accessory, cdd.PhotoPocket, equipPhotoList, bonusCharaList, activeKizunaBuff);
	}

	public static PrjUtil.ParamPreset CalcParamByCharaWithKemoBoard(CharaPackData cpd)
	{
		CharaDynamicData dynamicData = cpd.dynamicData;
		PrjUtil.ParamPreset paramPreset = PrjUtil.CalcParamByChara(dynamicData.id, dynamicData.level, dynamicData.rank, dynamicData.promoteNum, dynamicData.promoteFlag, dynamicData.haveClothesIdList, null, dynamicData.PhotoPocket, null, null, null);
		DataManagerKemoBoard.KemoBoardBonusParam kemoBoardBonusParam = DataManager.DmKemoBoard.KemoBoardBonusParamMap[cpd.staticData.baseData.attribute];
		paramPreset.hp += kemoBoardBonusParam.Hp;
		paramPreset.atk += kemoBoardBonusParam.Attack;
		paramPreset.def += kemoBoardBonusParam.Difence;
		paramPreset.avoid += kemoBoardBonusParam.Avoid;
		paramPreset.beatDamageRatio += kemoBoardBonusParam.BeatDamage;
		paramPreset.actionDamageRatio += kemoBoardBonusParam.BeatDamage;
		paramPreset.tryDamageRatio += kemoBoardBonusParam.TryDamage;
		return paramPreset;
	}

	private static PrjUtil.ParamPreset CalcParamByChara(int charaId, int level, int rank, int promoteNum, List<bool> promoteFlag, List<int> haveClothesIdList, DataManagerCharaAccessory.Accessory accessory, List<CharaDynamicData.PPParam> photoPocketList, List<PhotoPackData> equipPhotoList, List<DataManagerChara.BonusCharaData> bonusCharaList, PrjUtil.ParamPreset activeKizunaBuff)
	{
		List<bool> list = new List<bool>();
		foreach (CharaDynamicData.PPParam ppparam in photoPocketList)
		{
			list.Add(ppparam.Flag);
		}
		CharaStaticData charaStaticData = DataManager.DmChara.GetCharaStaticData(charaId);
		PrjUtil.ParamPreset paramPreset = new PrjUtil.ParamPreset();
		paramPreset.hp = PrjUtil.CalcParamInternal(level, 99, charaStaticData.baseData.hpParamLv1, charaStaticData.baseData.hpParamLv99, charaStaticData.baseData.hpParamLvMiddle, charaStaticData.baseData.hpLvMiddleNum);
		paramPreset.atk = PrjUtil.CalcParamInternal(level, 99, charaStaticData.baseData.atkParamLv1, charaStaticData.baseData.atkParamLv99, charaStaticData.baseData.atkParamLvMiddle, charaStaticData.baseData.atkLvMiddleNum);
		paramPreset.def = PrjUtil.CalcParamInternal(level, 99, charaStaticData.baseData.defParamLv1, charaStaticData.baseData.defParamLv99, charaStaticData.baseData.defParamLvMiddle, charaStaticData.baseData.defLvMiddleNum);
		paramPreset.avoid = charaStaticData.baseData.avoidRatio * 10;
		int num = 0;
		foreach (CharaPromotePreset charaPromotePreset in charaStaticData.promoteList)
		{
			if (promoteNum > num)
			{
				PrjUtil.ParamPreset paramPreset2 = paramPreset;
				paramPreset2.hp += charaPromotePreset.promoteOneList.Sum<CharaPromoteOne>((CharaPromoteOne x) => x.promoteHp);
				PrjUtil.ParamPreset paramPreset3 = paramPreset;
				paramPreset3.atk += charaPromotePreset.promoteOneList.Sum<CharaPromoteOne>((CharaPromoteOne x) => x.promoteAtk);
				PrjUtil.ParamPreset paramPreset4 = paramPreset;
				paramPreset4.def += charaPromotePreset.promoteOneList.Sum<CharaPromoteOne>((CharaPromoteOne x) => x.promoteDef);
				PrjUtil.ParamPreset paramPreset5 = paramPreset;
				paramPreset5.avoid += charaPromotePreset.promoteOneList.Sum<CharaPromoteOne>((CharaPromoteOne x) => x.promoteAvoid);
				PrjUtil.ParamPreset paramPreset6 = paramPreset;
				paramPreset6.actionDamageRatio += charaPromotePreset.promoteOneList.Sum<CharaPromoteOne>((CharaPromoteOne x) => x.promoteActionDamageRatio);
				PrjUtil.ParamPreset paramPreset7 = paramPreset;
				paramPreset7.tryDamageRatio += charaPromotePreset.promoteOneList.Sum<CharaPromoteOne>((CharaPromoteOne x) => x.promoteTryDamageRatio);
				PrjUtil.ParamPreset paramPreset8 = paramPreset;
				paramPreset8.beatDamageRatio += charaPromotePreset.promoteOneList.Sum<CharaPromoteOne>((CharaPromoteOne x) => x.promoteBeatDamageRatio);
			}
			else
			{
				int num2 = 0;
				using (List<CharaPromoteOne>.Enumerator enumerator3 = charaPromotePreset.promoteOneList.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						CharaPromoteOne charaPromoteOne = enumerator3.Current;
						if (num2 >= promoteFlag.Count)
						{
							break;
						}
						if (promoteFlag[num2])
						{
							paramPreset.hp += charaPromoteOne.promoteHp;
							paramPreset.atk += charaPromoteOne.promoteAtk;
							paramPreset.def += charaPromoteOne.promoteDef;
							paramPreset.avoid += charaPromoteOne.promoteAvoid;
							paramPreset.actionDamageRatio += charaPromoteOne.promoteActionDamageRatio;
							paramPreset.tryDamageRatio += charaPromoteOne.promoteTryDamageRatio;
							paramPreset.beatDamageRatio += charaPromoteOne.promoteBeatDamageRatio;
						}
						num2++;
					}
					break;
				}
			}
			num++;
		}
		MstCharaRankData mstCharaRankData = DataManager.DmServerMst.mstCharaRankDataList.Find((MstCharaRankData item) => item.rankId == charaStaticData.baseData.rankId && item.rank == rank);
		if (mstCharaRankData != null)
		{
			paramPreset.hp = paramPreset.hp * mstCharaRankData.hp / 100 + ((paramPreset.hp * mstCharaRankData.hp % 100 == 0) ? 0 : 1);
			paramPreset.atk = paramPreset.atk * mstCharaRankData.atk / 100 + ((paramPreset.atk * mstCharaRankData.atk % 100 == 0) ? 0 : 1);
			paramPreset.def = paramPreset.def * mstCharaRankData.def / 100 + ((paramPreset.def * mstCharaRankData.def % 100 == 0) ? 0 : 1);
		}
		int num3 = 100;
		int risingStatusPatternId = charaStaticData.baseData.risingStatusPatternId;
		DataManagerChara.LevelLimitRisingStatus limitRisingStatus = DataManager.DmChara.GetLimitRisingStatus(risingStatusPatternId, charaStaticData.baseData.id);
		while (level >= 100 && num3 <= level)
		{
			paramPreset.hp += limitRisingStatus.addHp;
			paramPreset.atk += limitRisingStatus.addAtk;
			paramPreset.def += limitRisingStatus.addDef;
			num3++;
		}
		List<CharaClothStatic> list2 = new List<CharaClothStatic>();
		foreach (int num4 in haveClothesIdList)
		{
			CharaClothStatic charaClothesStaticData = DataManager.DmChara.GetCharaClothesStaticData(num4);
			if (charaClothesStaticData != null)
			{
				list2.Add(charaClothesStaticData);
			}
		}
		foreach (CharaClothStatic charaClothStatic in list2)
		{
			paramPreset.hp += charaClothStatic.HpBonus;
			paramPreset.atk += charaClothStatic.AtkBonus;
			paramPreset.def += charaClothStatic.DefBonus;
		}
		if (bonusCharaList != null)
		{
			foreach (DataManagerChara.BonusCharaData bonusCharaData in bonusCharaList)
			{
				if (bonusCharaData.charaId == charaId)
				{
					paramPreset.hp += paramPreset.hp * bonusCharaData.hpBonusRatio / 1000 + ((paramPreset.hp * bonusCharaData.hpBonusRatio % 1000 == 0) ? 0 : 1);
					paramPreset.atk += paramPreset.atk * bonusCharaData.strBonusRatio / 1000 + ((paramPreset.atk * bonusCharaData.strBonusRatio % 1000 == 0) ? 0 : 1);
					paramPreset.def += paramPreset.def * bonusCharaData.defBonusRatio / 1000 + ((paramPreset.def * bonusCharaData.defBonusRatio % 1000 == 0) ? 0 : 1);
				}
			}
		}
		if (equipPhotoList != null)
		{
			for (int i = 0; i < equipPhotoList.Count; i++)
			{
				if (equipPhotoList[i] != null && !equipPhotoList[i].IsInvalid() && i < list.Count && list[i] && (!equipPhotoList[i].staticData.baseData.kizunaPhotoFlg || equipPhotoList[i].staticData.baseData.id == charaStaticData.baseData.kizunaPhotoId))
				{
					PrjUtil.ParamPreset paramPreset9 = PrjUtil.CalcParamByPhoto(equipPhotoList[i]);
					PrjUtil.ParamPreset paramPreset10 = photoPocketList[i].CalcPhotoParam(paramPreset9);
					paramPreset.hp += paramPreset10.hp;
					paramPreset.atk += paramPreset10.atk;
					paramPreset.def += paramPreset10.def;
				}
			}
		}
		if (accessory != null)
		{
			paramPreset.hp += accessory.Param.Hp;
			paramPreset.atk += accessory.Param.Atk;
			paramPreset.def += accessory.Param.Def;
			paramPreset.avoid += accessory.Param.Avoid;
			paramPreset.beatDamageRatio += accessory.Param.Beat;
			paramPreset.actionDamageRatio += accessory.Param.Action;
			paramPreset.tryDamageRatio += accessory.Param.Try;
		}
		if (activeKizunaBuff != null)
		{
			paramPreset.hp += activeKizunaBuff.hp;
			paramPreset.atk += activeKizunaBuff.atk;
			paramPreset.def += activeKizunaBuff.def;
			paramPreset.avoid += activeKizunaBuff.avoid;
			paramPreset.beatDamageRatio += activeKizunaBuff.beatDamageRatio;
			paramPreset.actionDamageRatio += activeKizunaBuff.actionDamageRatio;
			paramPreset.tryDamageRatio += activeKizunaBuff.tryDamageRatio;
		}
		return paramPreset;
	}

	public static PrjUtil.ParamPreset CalcParamByEnemy(EnemyStaticBase esb, int charaLevel)
	{
		return new PrjUtil.ParamPreset
		{
			hp = PrjUtil.CalcParamInternal(charaLevel, 99, esb.hpParamLv1, esb.hpParamLv99, esb.hpParamLvMiddle, esb.hpLvMiddleNum),
			atk = PrjUtil.CalcParamInternal(charaLevel, 99, esb.atkParamLv1, esb.atkParamLv99, esb.atkParamLvMiddle, esb.atkLvMiddleNum),
			def = PrjUtil.CalcParamInternal(charaLevel, 99, esb.defParamLv1, esb.defParamLv99, esb.defParamLvMiddle, esb.defLvMiddleNum),
			avoid = esb.avoidRatio * 10
		};
	}

	public static PrjUtil.ParamPreset CalcParamByPhoto(PhotoPackData ppd)
	{
		return PrjUtil.CalcParamByPhoto(ppd.staticData, ppd.dynamicData.level);
	}

	public static PrjUtil.ParamPreset CalcParamByPhoto(PhotoStaticData psd, int photoLevel)
	{
		PhotoStaticBase baseData = psd.baseData;
		int limitLevel = psd.getLimitLevel(4);
		return new PrjUtil.ParamPreset
		{
			hp = PrjUtil.CalcParamInternal(photoLevel, limitLevel, baseData.hpParamLv1, baseData.hpParamLvMax, baseData.hpParamLvMiddle, baseData.hpLvMiddleNum),
			atk = PrjUtil.CalcParamInternal(photoLevel, limitLevel, baseData.atkParamLv1, baseData.atkParamLvMax, baseData.atkParamLvMiddle, baseData.atkLvMiddleNum),
			def = PrjUtil.CalcParamInternal(photoLevel, limitLevel, baseData.defParamLv1, baseData.defParamLvMax, baseData.defParamLvMiddle, baseData.defLvMiddleNum)
		};
	}

	public static PrjUtil.ParamPreset CalcShowWindowParamByChara(CharaStaticData csd, List<DataManagerChara.BonusCharaData> bonusCharaList)
	{
		CharaPackData charaPackData = CharaPackData.MakeInitial(csd.GetId());
		MstCharaRankData mstCharaRankData = DataManager.DmServerMst.mstCharaRankDataList.Find((MstCharaRankData item) => item.rankId == csd.baseData.rankId && item.rank == csd.baseData.rankLow);
		charaPackData.dynamicData.level = CharaPackData.CalcLimitLevel(csd.GetId(), csd.baseData.rankLow, 0);
		charaPackData.dynamicData.rank = mstCharaRankData.rank;
		charaPackData.dynamicData.promoteNum = csd.maxPromoteNum;
		return PrjUtil.CalcParamByChara(charaPackData.dynamicData, null, bonusCharaList, null);
	}

	public static long ConvertTimeToTicks(long time)
	{
		return time * 10000L + PrjUtil.CONVERT_BASE_TIME.Ticks;
	}

	public static long ConvertTicksToTime(long ticks)
	{
		return (ticks - PrjUtil.CONVERT_BASE_TIME.Ticks) / 10000L;
	}

	public static bool EnumTryParse<T>(string value, bool ignoreCase, out T result)
	{
		bool flag;
		try
		{
			result = (T)((object)Enum.Parse(typeof(T), value, ignoreCase));
			flag = true;
		}
		catch
		{
			result = default(T);
			flag = false;
		}
		return flag;
	}

	public static GameObject CreateEmptyStretchPanel(Transform transform, string objName)
	{
		GameObject gameObject = new GameObject();
		gameObject.transform.SetParent(transform, false);
		gameObject.name = objName;
		RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
		rectTransform.localScale = Vector3.one;
		rectTransform.anchorMin = Vector2.zero;
		rectTransform.anchorMax = Vector2.one;
		rectTransform.offsetMax = Vector2.zero;
		rectTransform.offsetMin = Vector2.zero;
		return gameObject;
	}

	public static void InsertionSort<T>(ref List<T> list, Comparison<T> comparison)
	{
		if (list.Count <= 1)
		{
			return;
		}
		IComparer<T> comparer = ((comparison != null) ? new PrjUtil.FunctorComparer<T>(comparison) : null);
		comparer = comparer ?? Comparer<T>.Default;
		int count = list.Count;
		for (int i = 1; i < count; i++)
		{
			T t = list[i];
			if (comparer.Compare(list[i - 1], t) > 0)
			{
				int num = i;
				do
				{
					list[num] = list[num - 1];
					num--;
				}
				while (num > 0 && comparer.Compare(list[num - 1], t) > 0);
				list[num] = t;
			}
		}
	}

	public static void InsertionSortLight<T>(ref List<T> list, Comparison<T> comparison)
	{
		if (list.Count <= 1)
		{
			return;
		}
		IComparer<T> comparer = ((comparison != null) ? new PrjUtil.FunctorComparer<T>(comparison) : null);
		comparer = comparer ?? Comparer<T>.Default;
		list.Sort(comparer);
	}

	public static int CompareByName(int a, int b)
	{
		string name = DataManager.DmItem.GetItemStaticBase(a).GetName();
		string name2 = DataManager.DmItem.GetItemStaticBase(b).GetName();
		return PrjUtil.CompareByName(name, name2);
	}

	public static int CompareByName(string itemNameA, string itemNameB)
	{
		PrjUtil.<>c__DisplayClass46_0 CS$<>8__locals1;
		CS$<>8__locals1.alphabetConvertMap = new Dictionary<char, char>
		{
			{ 'Ａ', 'A' },
			{ 'Ｂ', 'B' },
			{ 'Ｃ', 'C' },
			{ 'Ｄ', 'D' },
			{ 'Ｅ', 'E' },
			{ 'Ｆ', 'F' },
			{ 'Ｇ', 'G' },
			{ 'Ｈ', 'H' },
			{ 'Ｉ', 'I' },
			{ 'Ｊ', 'J' },
			{ 'Ｋ', 'K' },
			{ 'Ｌ', 'L' },
			{ 'Ｍ', 'M' },
			{ 'Ｎ', 'N' },
			{ 'Ｏ', 'O' },
			{ 'Ｐ', 'P' },
			{ 'Ｑ', 'Q' },
			{ 'Ｒ', 'R' },
			{ 'Ｓ', 'S' },
			{ 'Ｔ', 'T' },
			{ 'Ｕ', 'U' },
			{ 'Ｖ', 'V' },
			{ 'Ｗ', 'W' },
			{ 'Ｘ', 'X' },
			{ 'Ｙ', 'Y' },
			{ 'Ｚ', 'Z' },
			{ 'ａ', 'a' },
			{ 'ｂ', 'b' },
			{ 'ｃ', 'c' },
			{ 'ｄ', 'd' },
			{ 'ｅ', 'e' },
			{ 'ｆ', 'f' },
			{ 'ｇ', 'g' },
			{ 'ｈ', 'h' },
			{ 'ｉ', 'i' },
			{ 'ｊ', 'j' },
			{ 'ｋ', 'k' },
			{ 'ｌ', 'l' },
			{ 'ｍ', 'm' },
			{ 'ｎ', 'n' },
			{ 'ｏ', 'o' },
			{ 'ｐ', 'p' },
			{ 'ｑ', 'q' },
			{ 'ｒ', 'r' },
			{ 'ｓ', 's' },
			{ 'ｔ', 't' },
			{ 'ｕ', 'u' },
			{ 'ｖ', 'v' },
			{ 'ｗ', 'w' },
			{ 'ｘ', 'x' },
			{ 'ｙ', 'y' },
			{ 'ｚ', 'z' }
		};
		CS$<>8__locals1.numberConvertMap = new Dictionary<char, char>
		{
			{ '０', '0' },
			{ '１', '1' },
			{ '２', '2' },
			{ '３', '3' },
			{ '４', '4' },
			{ '５', '5' },
			{ '６', '6' },
			{ '７', '7' },
			{ '８', '8' },
			{ '９', '9' }
		};
		int num = 0;
		int num2 = itemNameA.IndexOf("【");
		int num3 = itemNameA.LastIndexOf("】");
		if (0 <= num2 && 0 <= num3)
		{
			itemNameA = itemNameA.Remove(num2, num3 - num2 + 1);
		}
		int num4 = itemNameB.IndexOf("【");
		int num5 = itemNameB.LastIndexOf("】");
		if (0 <= num4 && 0 <= num5)
		{
			itemNameB = itemNameB.Remove(num4, num5 - num4 + 1);
		}
		char[] array = itemNameA.ToCharArray();
		char[] array2 = itemNameB.ToCharArray();
		int num6 = ((array.Length > array2.Length) ? array2.Length : array.Length);
		int i = 0;
		while (i < num6)
		{
			num = (int)(array[i] - array2[i]);
			if (num != 0)
			{
				bool flag = false;
				char c = array[i];
				char c2 = array2[i];
				if (PrjUtil.<CompareByName>g__IsFullWidthAlphabet|46_0(array[i]))
				{
					c = PrjUtil.<CompareByName>g__GetHalfWidthAlphabet|46_1(array[i], ref CS$<>8__locals1);
					flag = true;
				}
				else if (PrjUtil.<CompareByName>g__IsFullWidthNumber|46_2(array[i]))
				{
					c = PrjUtil.<CompareByName>g__GetHalfWidthNumber|46_3(array[i], ref CS$<>8__locals1);
					flag = true;
				}
				if (PrjUtil.<CompareByName>g__IsFullWidthAlphabet|46_0(array2[i]))
				{
					c2 = PrjUtil.<CompareByName>g__GetHalfWidthAlphabet|46_1(array2[i], ref CS$<>8__locals1);
					flag = true;
				}
				else if (PrjUtil.<CompareByName>g__IsFullWidthNumber|46_2(array2[i]))
				{
					c2 = PrjUtil.<CompareByName>g__GetHalfWidthNumber|46_3(array2[i], ref CS$<>8__locals1);
					flag = true;
				}
				if (flag)
				{
					num = (int)(c - c2);
					break;
				}
				break;
			}
			else
			{
				i++;
			}
		}
		if (num == 0)
		{
			num = array.Length - array2.Length;
		}
		return num;
	}

	public static void IsPriorityZeroSort(ShopData shopData)
	{
		int count = shopData.oneDataList.Count;
		for (int i = 0; i < count; i++)
		{
			if (shopData.oneDataList[i].priority == 0 && i != count)
			{
				shopData.oneDataList.Add(shopData.oneDataList[i]);
				shopData.oneDataList.RemoveAt(i);
			}
		}
	}

	public static void ReleaseMemory(int weight)
	{
		if (weight < 1)
		{
			weight = 1;
		}
		int num = PrjUtil.ReleaseCount / PrjUtil.UnloadUnused;
		int num2 = (PrjUtil.ReleaseCount += weight) / PrjUtil.UnloadUnused;
		if (num2 != num)
		{
			Resources.UnloadUnusedAssets();
			PrjUtil.ReleaseCount = num2 * PrjUtil.UnloadUnused;
		}
		if (PrjUtil.ReleaseCount >= PrjUtil.Garbagecollection)
		{
			GC.Collect();
			PrjUtil.ReleaseCount = 0;
		}
	}

	public static void AppsFlyerActivate(int friendId)
	{
	}

	public static void AppsFlyerOnDeviceTokenRegistered(string deviceToken)
	{
	}

	private static byte[] HexToStringBytes(string hexString)
	{
		int num = hexString.Length;
		if (num % 2 == 1)
		{
			num++;
			hexString = "0" + hexString;
		}
		byte[] array = new byte[num / 2];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
		}
		return array;
	}

	public static void SendAppsFlyerLtvId(string ltvId, Dictionary<string, string> option = null)
	{
	}

	public static void SendAppsFlyerPurcaseLtvId(double price, string currency, string orderId, int quantity)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary["af_currency"] = currency;
		dictionary["af_revenue"] = price.ToString();
		dictionary["af_quantity"] = quantity.ToString();
		dictionary["af_order_id"] = orderId;
		PrjUtil.SendAppsFlyerLtvId("af_purchase", dictionary);
	}

	public static void SendAppsFlyerLtvIdByRankup(int befRank, int aftRank)
	{
		if (befRank != aftRank)
		{
			if (aftRank == 10)
			{
				PrjUtil.SendAppsFlyerLtvId("af_level_010", null);
				return;
			}
			if (aftRank == 20)
			{
				PrjUtil.SendAppsFlyerLtvId("af_level_020", null);
				return;
			}
			if (aftRank != 50)
			{
				return;
			}
			PrjUtil.SendAppsFlyerLtvId("af_level_050", null);
		}
	}

	public static void SendAppsFlyerLtvIdByTutorial(TutorialUtil.Sequence nowSequence)
	{
		if (nowSequence == TutorialUtil.Sequence.FIRST)
		{
			PrjUtil.SendAppsFlyerLtvId("af_accountregistry", null);
			return;
		}
		if (nowSequence == TutorialUtil.Sequence.GACHA_PLAY)
		{
			PrjUtil.SendAppsFlyerLtvId("af_tutorial_invite", null);
			return;
		}
		if (nowSequence != TutorialUtil.Sequence.QUEST_GUIDE)
		{
			return;
		}
		PrjUtil.SendAppsFlyerLtvId("af_tutorial_completion", null);
	}

	public static void SendAppsFlyerLtvIdByQuestClear(DataManagerServerMst.ModeReleaseData.ModeCategory modeCategory)
	{
		switch (modeCategory)
		{
		case DataManagerServerMst.ModeReleaseData.ModeCategory.GrowthQuest:
			PrjUtil.SendAppsFlyerLtvId("af_quest_chapter01_09", null);
			return;
		case DataManagerServerMst.ModeReleaseData.ModeCategory.FriendsStory:
			PrjUtil.SendAppsFlyerLtvId("af_quest_chapter01_06", null);
			return;
		case DataManagerServerMst.ModeReleaseData.ModeCategory.PvpMode:
			PrjUtil.SendAppsFlyerLtvId("af_quest_chapter02_12", null);
			break;
		case DataManagerServerMst.ModeReleaseData.ModeCategory.Picnic:
			break;
		case DataManagerServerMst.ModeReleaseData.ModeCategory.Picnic2:
			PrjUtil.SendAppsFlyerLtvId("af_quest_chapter01_03", null);
			return;
		case DataManagerServerMst.ModeReleaseData.ModeCategory.Picnic3:
			PrjUtil.SendAppsFlyerLtvId("af_quest_chapter01_12", null);
			return;
		case DataManagerServerMst.ModeReleaseData.ModeCategory.Picnic4:
			PrjUtil.SendAppsFlyerLtvId("af_quest_chapter02_06", null);
			return;
		default:
			return;
		}
	}

	public static string Rarity2String(int rarity)
	{
		string text = string.Empty;
		switch (rarity)
		{
		case 1:
			text = "C";
			break;
		case 2:
			text = "UC";
			break;
		case 3:
			text = "R";
			break;
		case 4:
			text = "SR";
			break;
		case 5:
			text = "SSR";
			break;
		default:
			text = "-";
			break;
		}
		return text;
	}

	public static string SetRate(double rate)
	{
		if (0.0 < rate)
		{
			return rate.ToString() + "%";
		}
		return "-";
	}

	public static void OpenNoahWebOffer()
	{
		if (!DataManager.DmServerMst.IsEnableNoahWeb())
		{
			return;
		}
		string text = ((DataManager.DmServerMst.MstAppConfig.enableNoahweb == 1) ? "https://noahweb.noahapps.jp/offer?" : "https://api-dev.noahweb2.pj-noah.com/offer?");
		text += string.Format("m_id={0}&user_id={1}&pf={2}&crypt={3}", new object[]
		{
			16,
			DataManager.DmUserInfo.friendId,
			LoginManager.Platform,
			LoginManager.NoahCrypt
		});
		CanvasManager.HdlWebViewWindowCtrl.Open(text);
	}

	public static void OpenOfferWallWebview()
	{
		if (!DataManager.DmServerMst.IsEnableNoahWeb())
		{
			return;
		}
		string text = ((DataManager.DmServerMst.MstAppConfig.enableNoahweb == 1) ? "https://ow.skyflag.jp/ad/p/ow/index?" : "https://ow.stg.skyflag.jp/ad/p/ow/index?");
		text += string.Format("_owp={0}&suid={1}", LoginManager.OfferParameter, DataManager.DmUserInfo.friendId);
		CanvasManager.HdlWebViewWindowCtrl.Open(text);
	}

	public static void ForceShutdown()
	{
		CanvasManager.winClose = -1;
		Application.Quit();
	}

	[CompilerGenerated]
	internal static bool <CompareByName>g__IsFullWidthAlphabet|46_0(char c)
	{
		return ('Ａ' <= c && c <= 'Ｚ') || ('ａ' <= c && c <= 'ｚ');
	}

	[CompilerGenerated]
	internal static char <CompareByName>g__GetHalfWidthAlphabet|46_1(char fullWidthAlphabet, ref PrjUtil.<>c__DisplayClass46_0 A_1)
	{
		return A_1.alphabetConvertMap[fullWidthAlphabet];
	}

	[CompilerGenerated]
	internal static bool <CompareByName>g__IsFullWidthNumber|46_2(char c)
	{
		return '０' <= c && c <= '９';
	}

	[CompilerGenerated]
	internal static char <CompareByName>g__GetHalfWidthNumber|46_3(char fullWidthNumber, ref PrjUtil.<>c__DisplayClass46_0 A_1)
	{
		return A_1.numberConvertMap[fullWidthNumber];
	}

	public const int SCROLL_ITEM_COLUMN_MAX = 3;

	public static readonly string WARNING_COLOR_CODE = "#FF0000FF";

	public static readonly string DISABLE_COLOR_CODE = "#AAAAAAFF";

	public static readonly string BLACK_ALPHA_COLOR_CODE = "#00000077";

	public static readonly string ColorRedStartTag = "<color=" + PrjUtil.WARNING_COLOR_CODE + ">";

	public static readonly string ColorEndTag = "</color>";

	public static readonly string PARTY_FORMATION = "パーティ編成";

	public static readonly string PvP_FORMATION = "ちからくらべ編成";

	public static readonly string SPECIAL_PvP_FORMATION = "とくべつくんれん編成";

	public static readonly string TRAINING_FORMATION = "道場パーティ編成";

	public static readonly string HELPER_FRIENDS = "助っ人フレンズ";

	public static readonly string HELPER_FRIENDS_EDIT = PrjUtil.HELPER_FRIENDS + "変更";

	private static readonly DateTime CONVERT_BASE_TIME = new DateTime(1970, 1, 1, 9, 0, 0, DateTimeKind.Utc);

	private static int ReleaseCount = 0;

	public static readonly int UnloadUnused = 1000;

	public static readonly int Garbagecollection = 10000;

	public static readonly string APPSFLAYER_DEV_KEY = "4S9ocRDCURq8MUxzmBcsfW";

	public static readonly string ANDROID_BUNDLE_IDENTIFIER = "com.sega.KemonoFriends3";

	public static readonly string ITUNES_CONNECT_APPLE_ID = "id1454346070";

	public class ParamPreset
	{
		public int totalParam
		{
			get
			{
				return 0 + this.hp * 8 / 10 + ((this.hp * 8 % 10 > 0) ? 1 : 0) + this.atk * 3 + this.def * 2;
			}
		}

		public int hp;

		public int atk;

		public int def;

		public int avoid;

		public int actionDamageRatio;

		public int tryDamageRatio;

		public int beatDamageRatio;
	}

	public class NameDef
	{
		public const string NAME_BY_ARTS = "けものミラクル";

		public const string NAME_BY_SPECIAL_ATTACK = "とくいわざ";

		public const string NAME_BY_WAIT_ACTION = "たいきスキル";

		public const string NAME_BY_ABILITY = "とくせい";

		public const string NAME_BY_SP_ABILITY = "キセキとくせい";

		public const string NAME_BY_NANAIRO_ABILITY = "なないろとくせい";
	}

	public class FunctorComparer<T> : IComparer<T>
	{
		public FunctorComparer(Comparison<T> comparison)
		{
			this.comparison = comparison;
		}

		public int Compare(T x, T y)
		{
			return this.comparison(x, y);
		}

		private Comparison<T> comparison;
	}
}
