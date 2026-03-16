using System;
using System.Collections.Generic;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;

public class DataManagerDeck
{
	public DataManagerDeck(DataManager p)
	{
		this.parentData = p;
	}

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

	public void UpdateUserDataByServer(List<Decks> serverDataDeck)
	{
		this.userDeckList = this.userDeckList ?? new List<UserDeckData>();
		this.userDeckPvpList = this.userDeckPvpList ?? new List<UserDeckData>();
		this.userDeckTrainingList = this.userDeckTrainingList ?? new List<UserDeckData>();
		this.DeckUpdateInternal(serverDataDeck);
	}

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

	public void RequestActionUpdateDeck(UserDeckData deck)
	{
		List<Decks> list = new List<Decks> { deck.CreateByServer() };
		this.parentData.ServerRequest(DeckUpdateCmd.Create(list), new Action<Command>(this.CbDeckUpdateCmd));
	}

	public void RequestActionGetPvpDeck()
	{
		this.parentData.ServerRequest(DeckListCmd.Create(2), new Action<Command>(this.CbDeckListCmd));
	}

	private void CbDeckUpdateCmd(Command cmd)
	{
		DeckUpdateRequest deckUpdateRequest = cmd.request as DeckUpdateRequest;
		DeckUpdateResponse deckUpdateResponse = cmd.response as DeckUpdateResponse;
		this.DeckUpdateInternal(deckUpdateRequest.decks);
		this.parentData.UpdateUserAssetByAssets(deckUpdateResponse.assets);
	}

	private void CbDeckListCmd(Command cmd)
	{
		DeckListResponse deckListResponse = cmd.response as DeckListResponse;
		this.DeckUpdateInternal(deckListResponse.decks);
	}

	private void CbDeckNameChangeCmd(Command cmd)
	{
	}

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

	private DataManager parentData;

	private List<UserDeckData> userDeckList;

	private List<UserDeckData> userDeckPvpList;

	private List<UserDeckData> userDeckTrainingList;
}
