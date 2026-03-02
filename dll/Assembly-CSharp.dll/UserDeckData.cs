using System;
using System.Collections.Generic;
using SGNFW.HttpRequest.Protocol;

// Token: 0x02000078 RID: 120
[Serializable]
public class UserDeckData
{
	// Token: 0x06000438 RID: 1080 RVA: 0x0001CE6F File Offset: 0x0001B06F
	public UserDeckData.Category GetCategory()
	{
		return UserDeckData.Id2Category(this.id);
	}

	// Token: 0x06000439 RID: 1081 RVA: 0x0001CE7C File Offset: 0x0001B07C
	public static UserDeckData.Category Id2Category(int id)
	{
		if (id <= 100)
		{
			return UserDeckData.Category.NORMAL;
		}
		if (id <= 200)
		{
			return UserDeckData.Category.PVP;
		}
		if (id <= 300)
		{
			return UserDeckData.Category.TRAINING;
		}
		return UserDeckData.Category.INVALID;
	}

	// Token: 0x170000DF RID: 223
	// (get) Token: 0x0600043A RID: 1082 RVA: 0x0001CE9A File Offset: 0x0001B09A
	// (set) Token: 0x0600043B RID: 1083 RVA: 0x0001CEA2 File Offset: 0x0001B0A2
	public List<List<long>> equipPhotoList { get; set; }

	// Token: 0x170000E0 RID: 224
	// (get) Token: 0x0600043C RID: 1084 RVA: 0x0001CEAB File Offset: 0x0001B0AB
	// (set) Token: 0x0600043D RID: 1085 RVA: 0x0001CEB3 File Offset: 0x0001B0B3
	public List<bool> waitSkillList { get; set; }

	// Token: 0x0600043E RID: 1086 RVA: 0x0001CEBC File Offset: 0x0001B0BC
	public UserDeckData()
	{
	}

	// Token: 0x0600043F RID: 1087 RVA: 0x0001CEC4 File Offset: 0x0001B0C4
	private UserDeckData(UserDeckData udd)
	{
		this.id = udd.id;
		this.name = udd.name;
		this.charaIdList = new List<int>(udd.charaIdList);
		this.masterSkillId = udd.masterSkillId;
		this.equipPhotoList = new List<List<long>>();
		this.waitSkillList = new List<bool>(udd.waitSkillList);
		foreach (List<long> list in udd.equipPhotoList)
		{
			this.equipPhotoList.Add(new List<long>(list));
		}
		this.pvpTacticsTypeId = udd.pvpTacticsTypeId;
		this.pvpTacticsTermsTypeId = udd.pvpTacticsTermsTypeId;
		this.pvpTacticsTermsValueId = udd.pvpTacticsTermsValueId;
	}

	// Token: 0x06000440 RID: 1088 RVA: 0x0001CF9C File Offset: 0x0001B19C
	public UserDeckData Clone()
	{
		return new UserDeckData(this);
	}

	// Token: 0x06000441 RID: 1089 RVA: 0x0001CFA4 File Offset: 0x0001B1A4
	public int GetHelperIndex()
	{
		return this.charaIdList.FindIndex((int item) => item == -1);
	}

	// Token: 0x06000442 RID: 1090 RVA: 0x0001CFD0 File Offset: 0x0001B1D0
	public int IsEquipPhoto(long photoDataId)
	{
		int num = 0;
		if (this.equipPhotoList == null)
		{
			return num;
		}
		int num2 = 0;
		foreach (List<long> list in this.equipPhotoList)
		{
			using (List<long>.Enumerator enumerator2 = list.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current == photoDataId)
					{
						num = this.charaIdList[num2];
						return num;
					}
				}
			}
			num2++;
		}
		return num;
	}

	// Token: 0x06000443 RID: 1091 RVA: 0x0001D07C File Offset: 0x0001B27C
	public int CalcDeckKemoStatusWithPhoto(bool withoutDhole, int questOneId = 0)
	{
		int num = 0;
		PrjUtil.ParamPreset activeKizunaBuff = DataManager.DmChara.GetActiveKizunaBuff();
		List<int> list = new List<int>();
		QuestOnePackData questOnePackData = DataManager.DmQuest.GetQuestOnePackData(questOneId);
		if (questOnePackData != null && (questOnePackData.questOne.ruleId != 0 || questOnePackData.questGroup.limitGroupFlag))
		{
			List<CharaStaticData> list2 = new List<CharaStaticData>();
			foreach (int num2 in this.charaIdList)
			{
				if (num2 != 0)
				{
					CharaStaticData charaStaticData = DataManager.DmChara.GetCharaStaticData(num2);
					if (charaStaticData != null)
					{
						list2.Add(charaStaticData);
					}
				}
			}
			List<CharaStaticData> list3 = new List<CharaStaticData>();
			foreach (CharaStaticData charaStaticData2 in list2)
			{
				if (QuestUtil.IsBanTarget(charaStaticData2, questOnePackData, list3))
				{
					list.Add(charaStaticData2.GetId());
				}
				list3.Add(charaStaticData2);
			}
		}
		for (int i = 0; i < this.charaIdList.Count; i++)
		{
			int num3 = 1;
			int num4 = -1;
			if (!withoutDhole || this.charaIdList[i] == 0 || num4 == this.charaIdList[i] || !DataManager.DmChara.GetSameCharaList(this.charaIdList[i], true).Contains(num3))
			{
				if (questOnePackData != null && this.charaIdList[i] != 0)
				{
					bool flag = false;
					foreach (int num5 in list)
					{
						flag = DataManager.DmChara.GetSameCharaList(this.charaIdList[i], true).Contains(num5);
						if (flag)
						{
							break;
						}
					}
					if (flag)
					{
						goto IL_0256;
					}
				}
				List<PhotoPackData> list4 = new List<PhotoPackData>();
				foreach (long num6 in this.equipPhotoList[i])
				{
					list4.Add(DataManager.DmPhoto.GetUserPhotoData(num6));
				}
				if (this.charaIdList[i] != 0 && num4 != this.charaIdList[i])
				{
					PrjUtil.ParamPreset paramPreset = PrjUtil.CalcBattleParamByChara(DataManager.DmChara.GetUserCharaData(this.charaIdList[i]).dynamicData, list4, null, activeKizunaBuff);
					num += paramPreset.totalParam;
				}
			}
			IL_0256:;
		}
		return num;
	}

	// Token: 0x06000444 RID: 1092 RVA: 0x0001D32C File Offset: 0x0001B52C
	public int CalcTotalPlasmPoint(bool withoutDhole)
	{
		int num = 0;
		for (int i = 0; i < this.charaIdList.Count; i++)
		{
			int num2 = 1;
			int num3 = -1;
			if ((!withoutDhole || this.charaIdList[i] == 0 || num3 == this.charaIdList[i] || !DataManager.DmChara.GetSameCharaList(this.charaIdList[i], true).Contains(num2)) && this.charaIdList[i] != 0 && num3 != this.charaIdList[i])
			{
				int plasmPoint = DataManager.DmChara.GetUserCharaData(this.charaIdList[i]).staticData.baseData.plasmPoint;
				num += plasmPoint;
			}
		}
		return num;
	}

	// Token: 0x06000445 RID: 1093 RVA: 0x0001D3E4 File Offset: 0x0001B5E4
	public void UpdateByServer(Decks serverData)
	{
		this.id = serverData.deck_id;
		this.name = serverData.deck_name;
		this.masterSkillId = serverData.master_skil_id;
		this.charaIdList = new List<int>();
		this.equipPhotoList = new List<List<long>>();
		this.waitSkillList = new List<bool>();
		for (int i = 0; i < 5; i++)
		{
			this.charaIdList.Add(0);
			this.waitSkillList.Add(true);
			this.equipPhotoList.Add(new List<long>());
		}
		foreach (Deck deck in serverData.deck)
		{
			int num = deck.deck_idx - 1;
			this.charaIdList[num] = deck.chara_id;
			this.waitSkillList[num] = 1 == deck.wait_skill;
			this.equipPhotoList[num] = new List<long> { deck.photo_id1, deck.photo_id2, deck.photo_id3, deck.photo_id4 };
		}
		this.pvpTacticsTypeId = serverData.tactics_param1;
		this.pvpTacticsTermsTypeId = serverData.tactics_param2;
		this.pvpTacticsTermsValueId = serverData.tactics_param3;
	}

	// Token: 0x06000446 RID: 1094 RVA: 0x0001D544 File Offset: 0x0001B744
	public Decks CreateByServer()
	{
		Decks decks = new Decks
		{
			deck_id = this.id,
			deck_name = this.name,
			master_skil_id = this.masterSkillId,
			kemostatus = this.CalcDeckKemoStatusWithPhoto(false, 0),
			deck = new List<Deck>()
		};
		for (int i = 0; i < 5; i++)
		{
			Deck deck = new Deck
			{
				deck_idx = i + 1,
				chara_id = this.charaIdList[i],
				wait_skill = (this.waitSkillList[i] ? 1 : 0),
				photo_id1 = this.equipPhotoList[i][0],
				photo_id2 = this.equipPhotoList[i][1],
				photo_id3 = this.equipPhotoList[i][2],
				photo_id4 = this.equipPhotoList[i][3]
			};
			decks.deck.Add(deck);
		}
		decks.tactics_param1 = this.pvpTacticsTypeId;
		decks.tactics_param2 = this.pvpTacticsTermsTypeId;
		decks.tactics_param3 = this.pvpTacticsTermsValueId;
		return decks;
	}

	// Token: 0x040004EF RID: 1263
	public const int MAX_FRAME_NUM = 5;

	// Token: 0x040004F0 RID: 1264
	public const int DECK_HELPER_CHARA_ID = -1;

	// Token: 0x040004F1 RID: 1265
	public const int DECK_EMPTY_CHARA_ID = 0;

	// Token: 0x040004F2 RID: 1266
	public int id;

	// Token: 0x040004F3 RID: 1267
	public string name;

	// Token: 0x040004F4 RID: 1268
	public List<int> charaIdList;

	// Token: 0x040004F5 RID: 1269
	public int masterSkillId;

	// Token: 0x040004F8 RID: 1272
	public int pvpTacticsTypeId;

	// Token: 0x040004F9 RID: 1273
	public int pvpTacticsTermsTypeId;

	// Token: 0x040004FA RID: 1274
	public int pvpTacticsTermsValueId;

	// Token: 0x02000668 RID: 1640
	public enum Category
	{
		// Token: 0x04002EDB RID: 11995
		INVALID,
		// Token: 0x04002EDC RID: 11996
		NORMAL,
		// Token: 0x04002EDD RID: 11997
		PVP,
		// Token: 0x04002EDE RID: 11998
		TRAINING
	}
}
