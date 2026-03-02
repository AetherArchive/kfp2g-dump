using System;
using System.Collections.Generic;
using SGNFW.HttpRequest.Protocol;
using UnityEngine;

namespace Battle
{
	// Token: 0x02000212 RID: 530
	public class SceneBattle_DeckInfo
	{
		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x06002239 RID: 8761 RVA: 0x001924E0 File Offset: 0x001906E0
		public List<CharaPackData> deckData
		{
			get
			{
				return this._deckData;
			}
		}

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x0600223A RID: 8762 RVA: 0x001924E8 File Offset: 0x001906E8
		public MasterSkillPackData masterSkill
		{
			get
			{
				return this._masterSkill;
			}
		}

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x0600223B RID: 8763 RVA: 0x001924F0 File Offset: 0x001906F0
		public List<List<PhotoPackData>> equipPhotoList
		{
			get
			{
				return this._equipPhotoList;
			}
		}

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x0600223C RID: 8764 RVA: 0x001924F8 File Offset: 0x001906F8
		public int deckHelperIndex
		{
			get
			{
				return this._deckHelperIndex;
			}
		}

		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x0600223D RID: 8765 RVA: 0x00192500 File Offset: 0x00190700
		public CharaDef.AiType aiType
		{
			get
			{
				return this._aiType;
			}
		}

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x0600223E RID: 8766 RVA: 0x00192508 File Offset: 0x00190708
		public TacticsStaticSkill tacticsSkill
		{
			get
			{
				return this._tacticsSkill;
			}
		}

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x0600223F RID: 8767 RVA: 0x00192510 File Offset: 0x00190710
		public TacticsParam.Tactics.Type tacticsType
		{
			get
			{
				return this._tacticsType;
			}
		}

		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x06002240 RID: 8768 RVA: 0x00192518 File Offset: 0x00190718
		public int tacticsValue
		{
			get
			{
				return this._tacticsValue;
			}
		}

		// Token: 0x06002241 RID: 8769 RVA: 0x00192520 File Offset: 0x00190720
		public int CalcTotalPlasmPoint()
		{
			int num = 0;
			foreach (CharaPackData charaPackData in this._deckData)
			{
				if (charaPackData != null && charaPackData.staticData != null)
				{
					num += charaPackData.staticData.baseData.plasmPoint;
				}
			}
			return num;
		}

		// Token: 0x06002242 RID: 8770 RVA: 0x00192590 File Offset: 0x00190790
		public SceneBattle_DeckInfo()
		{
		}

		// Token: 0x06002243 RID: 8771 RVA: 0x001925C8 File Offset: 0x001907C8
		public SceneBattle_DeckInfo(int selectDeckId, HelperPackData helper, int attrIndex = 0)
		{
			DataManagerChara dmChara = DataManager.DmChara;
			DataManagerPhoto dmPhoto = DataManager.DmPhoto;
			UserDeckData userDeckById = DataManager.DmDeck.GetUserDeckById(selectDeckId);
			for (int i = 0; i < userDeckById.charaIdList.Count; i++)
			{
				CharaPackData charaPackData = dmChara.GetUserCharaData(userDeckById.charaIdList[i]);
				if (userDeckById.charaIdList[i] == -1)
				{
					this._deckHelperIndex = i;
					charaPackData = ((helper == null) ? null : helper.HelperCharaSetList[attrIndex].helpChara);
				}
				this._deckData.Add(charaPackData);
				List<PhotoPackData> list = new List<PhotoPackData>();
				for (int j = 0; j < userDeckById.equipPhotoList[i].Count; j++)
				{
					if (charaPackData == null || j >= charaPackData.dynamicData.PhotoPocket.Count || !charaPackData.dynamicData.PhotoPocket[j].Flag)
					{
						list.Add(null);
					}
					else if (userDeckById.charaIdList[i] == -1)
					{
						list.Add((helper == null) ? null : helper.HelperCharaSetList[attrIndex].helpPhotoList[j]);
					}
					else
					{
						long num = userDeckById.equipPhotoList[i][j];
						list.Add(dmPhoto.GetUserPhotoData(num));
					}
				}
				this._equipPhotoList.Add(list);
				this.waitAction.Add(userDeckById.waitSkillList[i]);
			}
			this._masterSkill = dmChara.GetUserMasterSkillData(userDeckById.masterSkillId);
			this._aiType = CharaDef.AiType.STUPID;
			this.SetTactics(dmChara, userDeckById.pvpTacticsTypeId, userDeckById.pvpTacticsTermsTypeId, userDeckById.pvpTacticsTermsValueId);
		}

		// Token: 0x06002244 RID: 8772 RVA: 0x001927A8 File Offset: 0x001909A8
		public SceneBattle_DeckInfo(OppUser oppUser)
		{
			this.isNpc = oppUser.npc_flag == 1;
			int num = (this.isNpc ? 0 : oppUser.tactics_param1);
			int num2 = (this.isNpc ? 0 : oppUser.tactics_param2);
			int num3 = (this.isNpc ? 0 : oppUser.tactics_param3);
			this.SetupByCharaList(oppUser.opp_chara_list);
			this.SetTactics(DataManager.DmChara, num, num2, num3);
		}

		// Token: 0x06002245 RID: 8773 RVA: 0x0019284C File Offset: 0x00190A4C
		public SceneBattle_DeckInfo(Party party)
		{
			this.SetupByCharaList(party.charaList);
		}

		// Token: 0x06002246 RID: 8774 RVA: 0x0019289C File Offset: 0x00190A9C
		private void SetupByCharaList(List<Chara> serverCharaList)
		{
			for (int i = 0; i < serverCharaList.Count; i++)
			{
				Chara chara = serverCharaList[i];
				if (chara.chara_id != 0)
				{
					CharaDynamicData charaDynamicData = new CharaDynamicData();
					charaDynamicData.UpdateByServer(chara);
					this._deckData.Add(new CharaPackData(charaDynamicData));
				}
				else
				{
					this._deckData.Add(null);
				}
				List<PhotoPackData> list = new List<PhotoPackData>();
				for (int j = 0; j < 4; j++)
				{
					if (serverCharaList[i].photo_list != null && serverCharaList[i].photo_list[j].item_id != 0)
					{
						Photo photo = serverCharaList[i].photo_list[j];
						PhotoDynamicData photoDynamicData = new PhotoDynamicData();
						photoDynamicData.UpdateByServer(photo);
						list.Add(new PhotoPackData(photoDynamicData));
					}
					else
					{
						list.Add(null);
					}
				}
				this._equipPhotoList.Add(list);
				this.waitAction.Add(true);
			}
		}

		// Token: 0x06002247 RID: 8775 RVA: 0x00192990 File Offset: 0x00190B90
		public SceneBattle_DeckInfo(ScenarioParty party)
		{
			if (party == null)
			{
				return;
			}
			foreach (ScenarioParty.Friends friends2 in party.friends)
			{
				if (friends2.id <= 0)
				{
					this._deckData.Add(null);
					this._equipPhotoList.Add(new List<PhotoPackData>());
					this.waitAction.Add(true);
				}
				else
				{
					CharaDynamicData charaDynamicData = new CharaDynamicData();
					charaDynamicData.id = friends2.id;
					charaDynamicData.equipClothesId = friends2.clothItem;
					charaDynamicData.rank = friends2.rank;
					charaDynamicData.limitLevel = CharaPackData.CalcLimitLevel(charaDynamicData.id, charaDynamicData.rank, charaDynamicData.levelLimitId);
					if ((charaDynamicData.level = friends2.level) > charaDynamicData.limitLevel)
					{
						charaDynamicData.level = charaDynamicData.limitLevel;
					}
					charaDynamicData.exp = 0L;
					charaDynamicData.kizunaLevel = friends2.kizunaLevel;
					charaDynamicData.kizunaExp = 0L;
					charaDynamicData.promoteNum = friends2.yasei;
					charaDynamicData.promoteFlag = new List<bool> { false, false, false, false, false, false };
					charaDynamicData.artsLevel = friends2.miracleLevel;
					charaDynamicData.clearScenarioNum = (friends2.miracleMax ? 4 : 0);
					int num = 0;
					List<PhotoPackData> list = new List<PhotoPackData>();
					foreach (ScenarioParty.Photo photo2 in friends2.photo)
					{
						num++;
						if (photo2.id > 0)
						{
							PhotoPackData photoPackData = new PhotoPackData(new PhotoDynamicData
							{
								dataId = -1L,
								photoId = photo2.id,
								level = photo2.level,
								levelRank = photo2.limit,
								exp = 0L,
								lockFlag = true
							});
							if (photoPackData.limitLevel < photoPackData.dynamicData.level)
							{
								photoPackData.dynamicData.level = photoPackData.limitLevel;
							}
							list.Add(photoPackData);
						}
						else
						{
							list.Add(null);
						}
					}
					charaDynamicData.PhotoFrameTotalStep = num;
					charaDynamicData.nanairoAbilityReleaseFlag = friends2.nanairoAbilityReleaseFlag;
					CharaPackData charaPackData = new CharaPackData(charaDynamicData);
					this._deckData.Add(charaPackData);
					this._equipPhotoList.Add(list);
					this.waitAction.Add(true);
				}
			}
		}

		// Token: 0x06002248 RID: 8776 RVA: 0x00192C24 File Offset: 0x00190E24
		public SceneBattle_DeckInfo(DebugParty party)
		{
			this._deckHelperIndex = 4;
			for (int i = 0; i < party.friends.Length; i++)
			{
				DebugParty.Friends friends = party.friends[i];
				bool flag = this._deckHelperIndex == i;
				if (friends.id <= 0)
				{
					this._deckData.Add(null);
					this._equipPhotoList.Add(new List<PhotoPackData>());
					this.waitAction.Add(true);
				}
				else
				{
					CharaDynamicData charaDynamicData = new CharaDynamicData();
					charaDynamicData.OwnerType = (flag ? CharaDynamicData.CharaOwnerType.Helper : CharaDynamicData.CharaOwnerType.User);
					charaDynamicData.id = friends.id;
					charaDynamicData.rank = friends.rank;
					charaDynamicData.limitLevel = CharaPackData.CalcLimitLevel(charaDynamicData.id, charaDynamicData.rank, charaDynamicData.levelLimitId);
					if ((charaDynamicData.level = friends.level) > charaDynamicData.limitLevel)
					{
						charaDynamicData.level = charaDynamicData.limitLevel;
					}
					charaDynamicData.exp = 0L;
					charaDynamicData.kizunaLevel = friends.kizuna;
					charaDynamicData.kizunaExp = 0L;
					charaDynamicData.promoteNum = friends.yasei;
					charaDynamicData.promoteFlag = new List<bool> { false, false, false, false, false, false };
					charaDynamicData.artsLevel = friends.miracleLevel;
					charaDynamicData.clearScenarioNum = (friends.miracleMax ? 4 : 0);
					charaDynamicData.nanairoAbilityReleaseFlag = friends.nanairoAbilityFlag;
					int num = 0;
					List<PhotoPackData> list = new List<PhotoPackData>();
					foreach (DebugParty.Photo photo2 in friends.photo)
					{
						num++;
						if (photo2.id > 0)
						{
							PhotoPackData photoPackData = new PhotoPackData(new PhotoDynamicData
							{
								OwnerType = (flag ? PhotoDynamicData.PhotoOwnerType.Helper : PhotoDynamicData.PhotoOwnerType.User),
								dataId = -1L,
								photoId = photo2.id,
								level = photo2.level,
								levelRank = photo2.limit,
								exp = 0L,
								lockFlag = true
							});
							if (photoPackData.limitLevel < photoPackData.dynamicData.level)
							{
								photoPackData.dynamicData.level = photoPackData.limitLevel;
							}
							list.Add(photoPackData);
						}
						else
						{
							list.Add(null);
						}
					}
					charaDynamicData.PhotoFrameTotalStep = num;
					if (friends.accessory.id > 0)
					{
						Accessory accessory = new Accessory();
						accessory.accessory_id = 0L;
						accessory.item_id = friends.accessory.id;
						accessory.level = friends.accessory.level;
						accessory.owner_id = friends.id;
						accessory.lock_flg = 0;
						accessory.manage_status = 0;
						accessory.insert_time = 0L;
						accessory.update_time = 0L;
						charaDynamicData.dispAccessoryEffect = true;
						charaDynamicData.accessory = new DataManagerCharaAccessory.Accessory(accessory);
					}
					CharaPackData charaPackData = new CharaPackData(charaDynamicData);
					this._deckData.Add(charaPackData);
					this._equipPhotoList.Add(list);
					this.waitAction.Add(true);
				}
			}
			this._masterSkill = new MasterSkillPackData(new MasterSkillDynamicData
			{
				id = party.masterSkill,
				level = party.masterSkillLevel
			});
			this._aiType = party.aiType;
			this.SetTactics(DataManager.DmChara, party.trainingHp, party.trainingAtk, party.trainingDef);
		}

		// Token: 0x06002249 RID: 8777 RVA: 0x00192FC0 File Offset: 0x001911C0
		private void SetTactics(DataManagerChara dmChara, int typeId, int termsTypeId, int termsValueId)
		{
			this._tacticsSkill = dmChara.GetTacticsSkillStaticData(typeId);
			TacticsParam.Tactics tacticsParamData = dmChara.GetTacticsParamData((TacticsParam.Tactics.Type)termsTypeId);
			this._tacticsType = tacticsParamData.type;
			this._tacticsValue = ((termsValueId > 0 && termsValueId <= tacticsParamData.param.Count) ? (termsValueId - 1) : ((tacticsParamData.param.Count > 0) ? Random.Range(0, tacticsParamData.param.Count) : (-1)));
			if (this._tacticsValue >= 0)
			{
				this._tacticsValue = tacticsParamData.param[this._tacticsValue];
			}
		}

		// Token: 0x04001929 RID: 6441
		private List<CharaPackData> _deckData = new List<CharaPackData>();

		// Token: 0x0400192A RID: 6442
		private MasterSkillPackData _masterSkill;

		// Token: 0x0400192B RID: 6443
		private List<List<PhotoPackData>> _equipPhotoList = new List<List<PhotoPackData>>();

		// Token: 0x0400192C RID: 6444
		private int _deckHelperIndex = -1;

		// Token: 0x0400192D RID: 6445
		private CharaDef.AiType _aiType = CharaDef.AiType.CLEVER;

		// Token: 0x0400192E RID: 6446
		private TacticsStaticSkill _tacticsSkill;

		// Token: 0x0400192F RID: 6447
		private TacticsParam.Tactics.Type _tacticsType;

		// Token: 0x04001930 RID: 6448
		private int _tacticsValue;

		// Token: 0x04001931 RID: 6449
		public List<bool> waitAction = new List<bool>();

		// Token: 0x04001932 RID: 6450
		public bool isNpc;
	}
}
