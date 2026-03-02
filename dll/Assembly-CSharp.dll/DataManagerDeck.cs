using System;
using System.Collections.Generic;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;

// Token: 0x02000077 RID: 119
public class DataManagerDeck
{
	// Token: 0x0600042C RID: 1068 RVA: 0x0001CA7E File Offset: 0x0001AC7E
	public DataManagerDeck(DataManager p)
	{
		this.parentData = p;
	}

	// Token: 0x0600042D RID: 1069 RVA: 0x0001CA8D File Offset: 0x0001AC8D
	public List<UserDeckData> GetUserDeckList(UserDeckData.Category category)
	{
		switch (category)
		{
		case UserDeckData.Category.NORMAL:
			return this.userDeckList;
		case UserDeckData.Category.PVP:
			return this.userDeckPvpList;
		case UserDeckData.Category.TRAINING:
			return this.userDeckTrainingList;
		default:
			return this.userDeckList;
		}
	}

	// Token: 0x0600042E RID: 1070 RVA: 0x0001CAC0 File Offset: 0x0001ACC0
	public UserDeckData GetUserDeckById(int deckId)
	{
		switch (UserDeckData.Id2Category(deckId))
		{
		case UserDeckData.Category.NORMAL:
			return this.userDeckList.Find((UserDeckData item) => item.id == deckId);
		case UserDeckData.Category.PVP:
			return this.userDeckPvpList.Find((UserDeckData item) => item.id == deckId);
		case UserDeckData.Category.TRAINING:
			return this.userDeckTrainingList.Find((UserDeckData item) => item.id == deckId);
		default:
			return null;
		}
	}

	// Token: 0x0600042F RID: 1071 RVA: 0x0001CB48 File Offset: 0x0001AD48
	public void UpdateUserDataByServer(List<Decks> serverDataDeck)
	{
		this.userDeckList = this.userDeckList ?? new List<UserDeckData>();
		this.userDeckPvpList = this.userDeckPvpList ?? new List<UserDeckData>();
		this.userDeckTrainingList = this.userDeckTrainingList ?? new List<UserDeckData>();
		this.DeckUpdateInternal(serverDataDeck);
	}

	// Token: 0x06000430 RID: 1072 RVA: 0x0001CB9C File Offset: 0x0001AD9C
	public void UpdateUserDataByTutorial(int id, List<int> deckCharaList, bool isEquipPhoto, int masterSkill)
	{
		UserDeckData userDeckData = this.userDeckList.Find((UserDeckData item) => item.id == id);
		userDeckData.charaIdList = deckCharaList;
		userDeckData.equipPhotoList = new List<List<long>>
		{
			new List<long> { 0L, 0L, 0L, 0L },
			new List<long> { 0L, 0L, 0L, 0L },
			new List<long>
			{
				isEquipPhoto ? (-1L) : 0L,
				0L,
				0L,
				0L
			},
			new List<long> { 0L, 0L, 0L, 0L },
			new List<long> { 0L, 0L, 0L, 0L }
		};
		userDeckData.masterSkillId = masterSkill;
	}

	// Token: 0x06000431 RID: 1073 RVA: 0x0001CCC4 File Offset: 0x0001AEC4
	public void RequestActionUpdateDeck(UserDeckData deck)
	{
		List<Decks> list = new List<Decks> { deck.CreateByServer() };
		this.parentData.ServerRequest(DeckUpdateCmd.Create(list), new Action<Command>(this.CbDeckUpdateCmd));
	}

	// Token: 0x06000432 RID: 1074 RVA: 0x0001CD00 File Offset: 0x0001AF00
	public void RequestActionGetPvpDeck()
	{
		this.parentData.ServerRequest(DeckListCmd.Create(2), new Action<Command>(this.CbDeckListCmd));
	}

	// Token: 0x06000433 RID: 1075 RVA: 0x0001CD20 File Offset: 0x0001AF20
	private void CbDeckUpdateCmd(Command cmd)
	{
		DeckUpdateRequest deckUpdateRequest = cmd.request as DeckUpdateRequest;
		DeckUpdateResponse deckUpdateResponse = cmd.response as DeckUpdateResponse;
		this.DeckUpdateInternal(deckUpdateRequest.decks);
		this.parentData.UpdateUserAssetByAssets(deckUpdateResponse.assets);
	}

	// Token: 0x06000434 RID: 1076 RVA: 0x0001CD64 File Offset: 0x0001AF64
	private void CbDeckListCmd(Command cmd)
	{
		DeckListResponse deckListResponse = cmd.response as DeckListResponse;
		this.DeckUpdateInternal(deckListResponse.decks);
	}

	// Token: 0x06000435 RID: 1077 RVA: 0x0001CD89 File Offset: 0x0001AF89
	private void CbDeckNameChangeCmd(Command cmd)
	{
	}

	// Token: 0x06000436 RID: 1078 RVA: 0x0001CD8C File Offset: 0x0001AF8C
	private void DeckUpdateInternal(List<Decks> decks)
	{
		foreach (Decks decks2 in decks)
		{
			UserDeckData userDeckById = this.GetUserDeckById(decks2.deck_id);
			if (userDeckById != null)
			{
				userDeckById.UpdateByServer(decks2);
			}
			else
			{
				UserDeckData userDeckData = new UserDeckData();
				userDeckData.UpdateByServer(decks2);
				if (userDeckData.GetCategory() == UserDeckData.Category.NORMAL)
				{
					this.userDeckList.Add(userDeckData);
				}
				else if (userDeckData.GetCategory() == UserDeckData.Category.PVP)
				{
					this.userDeckPvpList.Add(userDeckData);
				}
				else if (userDeckData.GetCategory() == UserDeckData.Category.TRAINING)
				{
					this.userDeckTrainingList.Add(userDeckData);
				}
			}
		}
	}

	// Token: 0x06000437 RID: 1079 RVA: 0x0001CE3C File Offset: 0x0001B03C
	public static bool CheckDisableDropIcon(UserDeckData.Category deckCategory, int pvpSeasonId)
	{
		if (deckCategory == UserDeckData.Category.TRAINING)
		{
			return true;
		}
		if (deckCategory == UserDeckData.Category.PVP)
		{
			PvpStaticData pvpStaticDataBySeasonID = DataManager.DmPvp.GetPvpStaticDataBySeasonID(pvpSeasonId);
			return pvpStaticDataBySeasonID != null && pvpStaticDataBySeasonID.type == PvpStaticData.Type.NORMAL;
		}
		return false;
	}

	// Token: 0x040004EB RID: 1259
	private DataManager parentData;

	// Token: 0x040004EC RID: 1260
	private List<UserDeckData> userDeckList;

	// Token: 0x040004ED RID: 1261
	private List<UserDeckData> userDeckPvpList;

	// Token: 0x040004EE RID: 1262
	private List<UserDeckData> userDeckTrainingList;
}
