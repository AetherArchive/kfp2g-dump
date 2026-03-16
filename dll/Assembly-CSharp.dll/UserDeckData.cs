using System;
using System.Collections.Generic;
using SGNFW.HttpRequest.Protocol;

[Serializable]
public class UserDeckData
{
	public UserDeckData.Category GetCategory()
	{
		return UserDeckData.Id2Category(this.id);
	}

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

	public List<List<long>> equipPhotoList { get; set; }

	public List<bool> waitSkillList { get; set; }

	public UserDeckData()
	{
	}

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

	public UserDeckData Clone()
	{
		return new UserDeckData(this);
	}

	public int GetHelperIndex()
	{
		return this.charaIdList.FindIndex((int item) => item == -1);
	}

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

	public const int MAX_FRAME_NUM = 5;

	public const int DECK_HELPER_CHARA_ID = -1;

	public const int DECK_EMPTY_CHARA_ID = 0;

	public int id;

	public string name;

	public List<int> charaIdList;

	public int masterSkillId;

	public int pvpTacticsTypeId;

	public int pvpTacticsTermsTypeId;

	public int pvpTacticsTermsValueId;

	public enum Category
	{
		INVALID,
		NORMAL,
		PVP,
		TRAINING
	}
}
