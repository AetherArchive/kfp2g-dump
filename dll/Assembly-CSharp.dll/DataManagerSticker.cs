using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using SGNFW.Common;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;

public class DataManagerSticker
{
	public DataManagerSticker(DataManager p)
	{
		this.parentData = p;
	}

	public void UpdateUserDataByServer(List<Sticker> serverDataList)
	{
		foreach (Sticker sticker in serverDataList)
		{
			int id = sticker.id;
			DataManagerSticker.StickerDynamicData stickerDynamicData = new DataManagerSticker.StickerDynamicData(sticker);
			DataManagerSticker.StickerPackData stickerPackData = new DataManagerSticker.StickerPackData(this.staticDataMap[id], stickerDynamicData);
			if (this.userDynamicDataMap.ContainsKey(id))
			{
				this.userDynamicDataMap[id] = stickerPackData;
			}
			else
			{
				this.userDynamicDataMap.Add(id, stickerPackData);
			}
		}
	}

	public List<DataManagerSticker.StickerDynamicData> GetAllStickerDynamicData()
	{
		List<DataManagerSticker.StickerDynamicData> list = new List<DataManagerSticker.StickerDynamicData>();
		foreach (KeyValuePair<int, DataManagerSticker.StickerPackData> keyValuePair in this.userDynamicDataMap)
		{
			list.Add(keyValuePair.Value.dynamicData);
		}
		return list;
	}

	public DataManagerSticker.StickerDynamicData GetStickerDynamicData(int id)
	{
		if (this.userDynamicDataMap.ContainsKey(id))
		{
			return this.userDynamicDataMap[id].dynamicData;
		}
		return null;
	}

	public int GetAllStickerWeight()
	{
		return this.userDynamicDataMap.Values.Sum<DataManagerSticker.StickerPackData>(delegate(DataManagerSticker.StickerPackData pack)
		{
			DataManagerSticker.StickerStaticData staticData = pack.staticData;
			int num = ((staticData != null) ? staticData.bonusWeight : 0);
			DataManagerSticker.StickerDynamicData dynamicData = pack.dynamicData;
			return num * ((dynamicData != null) ? dynamicData.num : 0);
		});
	}

	public bool IsExistDynamicData(int id)
	{
		return this.userDynamicDataMap.ContainsKey(id);
	}

	public void InitializeMstData(MstManager mst)
	{
		List<MstStickerData> mst2 = mst.GetMst<List<MstStickerData>>(MstType.STICKER_DATA);
		if (mst2 == null)
		{
			return;
		}
		List<ItemStaticBase> list = new List<ItemStaticBase>();
		foreach (MstStickerData mstStickerData in mst2)
		{
			DataManagerSticker.StickerStaticData stickerStaticData = new DataManagerSticker.StickerStaticData(mstStickerData);
			this.staticDataMap.Add(mstStickerData.id, stickerStaticData);
			list.Add(stickerStaticData);
		}
		DataManager.DmItem.AddMstDataByItem(list);
		this.mstStickerPlayerExpBonusList = mst.GetMst<List<MstStickerPlayerExpBonus>>(MstType.STICKER_PLAYER_EXP_BONUS);
		if (this.mstStickerPlayerExpBonusList == null)
		{
			return;
		}
		this.mstStickerPlayerExpBonusList.Sort((MstStickerPlayerExpBonus a, MstStickerPlayerExpBonus b) => b.id.CompareTo(a.id));
	}

	public Dictionary<int, DataManagerSticker.StickerStaticData> GetAllStaticData()
	{
		return this.staticDataMap;
	}

	public DataManagerSticker.StickerStaticData GetStickerStaticData(int id)
	{
		if (!this.staticDataMap.ContainsKey(id))
		{
			Verbose<PrjLog>.LogError("Error : DataManagerSticker.StickerStaticData : 定義されていないID[" + id.ToString() + "]を生成しようとしました", null);
			return null;
		}
		return this.staticDataMap[id];
	}

	public MstStickerPlayerExpBonus GetPlayerExpBonusData(int finalWeight)
	{
		if (this.mstStickerPlayerExpBonusList == null)
		{
			return null;
		}
		return (from b in this.mstStickerPlayerExpBonusList
			where b.weight <= finalWeight
			orderby b.weight descending
			select b).FirstOrDefault<MstStickerPlayerExpBonus>();
	}

	public MstStickerPlayerExpBonus GetPlayerExpNextBonusData(int finalWeight)
	{
		if (this.mstStickerPlayerExpBonusList == null)
		{
			return null;
		}
		return (from b in this.mstStickerPlayerExpBonusList
			where b.weight > finalWeight
			orderby b.weight
			select b).FirstOrDefault<MstStickerPlayerExpBonus>();
	}

	public int ComparePackDataNyName(DataManagerSticker.StickerPackData a, DataManagerSticker.StickerPackData b)
	{
		return this.ComparePackDataNyName(new string[] { a.staticData.GetName() }, new string[] { b.staticData.GetName() });
	}

	public int ComparePackDataNyName(string[] dataA, string[] dataB)
	{
		int num = 0;
		char[] array = dataA[0].ToCharArray();
		char[] array2 = dataB[0].ToCharArray();
		int num2 = ((array.Length > array2.Length) ? array2.Length : array.Length);
		int i = 0;
		while (i < num2)
		{
			num = (int)(array2[i] - array[i]);
			if (num < 0)
			{
				if (char.IsDigit(array2[i]))
				{
					if (DataManagerSticker.<ComparePackDataNyName>g__IsAlphabet|20_0(array[i]) || DataManagerSticker.<ComparePackDataNyName>g__IsHiragana|20_1(array[i]))
					{
						num = 1;
						break;
					}
					break;
				}
				else if (DataManagerSticker.<ComparePackDataNyName>g__IsAlphabet|20_0(array2[i]))
				{
					if (DataManagerSticker.<ComparePackDataNyName>g__IsHiragana|20_1(array[i]))
					{
						num = 1;
						break;
					}
					break;
				}
				else
				{
					if (!DataManagerSticker.<ComparePackDataNyName>g__IsHiragana|20_1(array2[i]) && (char.IsDigit(array[i]) || DataManagerSticker.<ComparePackDataNyName>g__IsAlphabet|20_0(array[i]) || DataManagerSticker.<ComparePackDataNyName>g__IsHiragana|20_1(array[i])))
					{
						num = 1;
						break;
					}
					break;
				}
			}
			else if (num > 0)
			{
				if (char.IsDigit(array2[i]))
				{
					if (!char.IsDigit(array[i]) && !DataManagerSticker.<ComparePackDataNyName>g__IsAlphabet|20_0(array[i]) && !DataManagerSticker.<ComparePackDataNyName>g__IsHiragana|20_1(array[i]))
					{
						num = -1;
						break;
					}
					break;
				}
				else if (DataManagerSticker.<ComparePackDataNyName>g__IsAlphabet|20_0(array2[i]))
				{
					if (char.IsDigit(array[i]))
					{
						num = -1;
						break;
					}
					if (!DataManagerSticker.<ComparePackDataNyName>g__IsAlphabet|20_0(array[i]) && !DataManagerSticker.<ComparePackDataNyName>g__IsHiragana|20_1(array[i]))
					{
						num = -1;
						break;
					}
					break;
				}
				else
				{
					if (!DataManagerSticker.<ComparePackDataNyName>g__IsHiragana|20_1(array2[i]))
					{
						break;
					}
					if (char.IsDigit(array[i]) || DataManagerSticker.<ComparePackDataNyName>g__IsAlphabet|20_0(array[i]))
					{
						num = -1;
						break;
					}
					if (!DataManagerSticker.<ComparePackDataNyName>g__IsHiragana|20_1(array[i]))
					{
						num = -1;
						break;
					}
					break;
				}
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
		if (num == 0)
		{
			char[] array3 = dataA[1].ToCharArray();
			char[] array4 = dataB[1].ToCharArray();
			int num3 = ((array3.Length > array4.Length) ? array4.Length : array3.Length);
			for (int j = 0; j < num3; j++)
			{
				num = (int)(array4[j] - array3[j]);
				if (num != 0)
				{
					break;
				}
			}
			if (num == 0)
			{
				num = array3.Length - array4.Length;
			}
		}
		return num;
	}

	[CompilerGenerated]
	internal static bool <ComparePackDataNyName>g__IsAlphabet|20_0(char c)
	{
		return ('A' <= c && c <= 'Z') || ('Ａ' <= c && c <= 'Ｚ') || ('a' <= c && c <= 'z') || ('ａ' <= c && c <= 'ｚ');
	}

	[CompilerGenerated]
	internal static bool <ComparePackDataNyName>g__IsHiragana|20_1(char c)
	{
		return 'ぁ' <= c && c <= 'ん';
	}

	private DataManager parentData;

	private Dictionary<int, DataManagerSticker.StickerStaticData> staticDataMap = new Dictionary<int, DataManagerSticker.StickerStaticData>();

	private Dictionary<int, DataManagerSticker.StickerPackData> userDynamicDataMap = new Dictionary<int, DataManagerSticker.StickerPackData>();

	private List<MstStickerPlayerExpBonus> mstStickerPlayerExpBonusList;

	public enum StickerType
	{
		ITEM,
		FRIENDS,
		PHOTO,
		SPECIAL
	}

	public class StickerStaticData : ItemStaticBase
	{
		public StickerStaticData(MstStickerData mstData)
		{
			this.id = mstData.id;
			this.rarity = mstData.rarity;
			this.stickerType = (DataManagerSticker.StickerType)mstData.stickerType;
			this.name = mstData.name;
			this.flavorText = mstData.flavorText;
			this.iconName = mstData.iconName;
			this.stackMax = mstData.stackMax;
			this.bgTextureName = mstData.bgTextureName;
			this.bonusWeight = mstData.bonusWeight;
			this.startTime = new DateTime(PrjUtil.ConvertTimeToTicks(mstData.startTime));
		}

		public string GetCardImageName()
		{
			return "Texture2D/Sticker/Card_Sticker/card_sticker_" + this.id.ToString("00000");
		}

		public string GetBgTextureName()
		{
			return "Texture2D/Sticker/Card_Sticker_Bg/" + this.bgTextureName;
		}

		public override int GetId()
		{
			return this.id;
		}

		public override ItemDef.Kind GetKind()
		{
			return ItemDef.Kind.STICKER;
		}

		public override string GetName()
		{
			return this.name;
		}

		public override string GetInfo()
		{
			return this.flavorText;
		}

		public override ItemDef.Rarity GetRarity()
		{
			return (ItemDef.Rarity)this.rarity;
		}

		public override int GetStackMax()
		{
			return this.stackMax;
		}

		public override string GetIconName()
		{
			return "Texture2D/Icon_Sticker/" + this.iconName;
		}

		public override int GetSalePrice()
		{
			return 0;
		}

		public int id;

		public int rarity;

		public DataManagerSticker.StickerType stickerType;

		public string name;

		public string flavorText;

		public string iconName;

		public int stackMax;

		public string bgTextureName;

		public int bonusWeight;

		public DateTime startTime;
	}

	public class StickerDynamicData
	{
		public StickerDynamicData(Sticker data)
		{
			this.id = data.id;
			this.num = data.num;
			this.insertTime = new DateTime(PrjUtil.ConvertTimeToTicks(data.insert_time));
			this.updateTime = new DateTime(PrjUtil.ConvertTimeToTicks(data.update_time));
		}

		public StickerDynamicData()
		{
		}

		public int id;

		public int num;

		public DateTime insertTime;

		public DateTime updateTime;
	}

	public class StickerPackData
	{
		public StickerPackData(DataManagerSticker.StickerStaticData staticData, DataManagerSticker.StickerDynamicData dynamicData)
		{
			this.staticData = staticData;
			this.dynamicData = dynamicData;
		}

		public StickerPackData(DataManagerSticker.StickerDynamicData dynamicData)
		{
			DataManagerSticker.StickerDynamicData stickerDynamicData = DataManager.DmSticker.GetStickerDynamicData(dynamicData.id);
			this.dynamicData = ((stickerDynamicData == null) ? dynamicData : stickerDynamicData);
			this.staticData = DataManager.DmSticker.GetStickerStaticData(this.dynamicData.id);
		}

		public StickerPackData()
		{
		}

		public DataManagerSticker.StickerStaticData staticData;

		public DataManagerSticker.StickerDynamicData dynamicData;
	}
}
