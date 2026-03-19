using System;
using SGNFW.Common.Json;

namespace SGNFW.Mst
{
	public class MstClass
	{
		public static object cnv(MstType type, string json)
		{
			object obj = null;
			switch (type)
			{
			case MstType.APP_CONFIG:
				obj = PrjJson.FromJsonList<MstAppConfig>(json);
				break;
			case MstType.BONUS_PRESET_DATA:
				obj = PrjJson.FromJsonList<MstBonusPresetData>(json);
				break;
			case MstType.BONUS_DATA:
				obj = PrjJson.FromJsonList<MstBonusData>(json);
				break;
			case MstType.CHARA_ARTS_DATA:
				obj = PrjJson.FromJsonList<MstCharaArtsData>(json);
				break;
			case MstType.CHARA_CLOTHES_DATA:
				obj = PrjJson.FromJsonList<MstCharaClothesData>(json);
				break;
			case MstType.CHARA_DATA:
				obj = PrjJson.FromJsonList<MstCharaData>(json);
				break;
			case MstType.CHARA_LEVEL_DATA:
				obj = PrjJson.FromJsonList<MstCharaLevelData>(json);
				break;
			case MstType.CHARA_LEVEL_ITEM_DATA:
				obj = PrjJson.FromJsonList<MstCharaLevelItemData>(json);
				break;
			case MstType.CHARA_PP_DATA:
				obj = PrjJson.FromJsonList<MstCharaPpData>(json);
				break;
			case MstType.CHARA_PROMOTE_DATA:
				obj = PrjJson.FromJsonList<MstCharaPromoteData>(json);
				break;
			case MstType.CHARA_PROMOTE_PRESET_DATA:
				obj = PrjJson.FromJsonList<MstCharaPromotePresetData>(json);
				break;
			case MstType.CHARA_RANK_DATA:
				obj = PrjJson.FromJsonList<MstCharaRankData>(json);
				break;
			case MstType.CHARA_VOICE_COMBO_DATA:
				obj = PrjJson.FromJsonList<MstCharaVoiceComboData>(json);
				break;
			case MstType.CHARA_PX_RATE_DATA:
				obj = PrjJson.FromJsonList<MstCharaPxRateData>(json);
				break;
			case MstType.CHARA_LOT_CAMPAIGN_DATA:
				obj = PrjJson.FromJsonList<MstCharaLotCampaignData>(json);
				break;
			case MstType.CHARA_MISSION_DATA:
				obj = PrjJson.FromJsonList<MstCharaMissionData>(json);
				break;
			case MstType.CHARA_FILTER_DATA:
				obj = PrjJson.FromJsonList<MstCharaFilterData>(json);
				break;
			case MstType.EVENT_COOP_CONDITION_DATA:
				obj = PrjJson.FromJsonList<MstEventCoopConditionData>(json);
				break;
			case MstType.EVENT_COOP_HARD_QUEST_DATA:
				obj = PrjJson.FromJsonList<MstEventCoopHardQuestData>(json);
				break;
			case MstType.EVENT_DATA:
				obj = PrjJson.FromJsonList<MstEventData>(json);
				break;
			case MstType.EVENT_LARGE_EVENT_DATA:
				obj = PrjJson.FromJsonList<MstEventLargeEventData>(json);
				break;
			case MstType.EVENT_MISSION_DATA:
				obj = PrjJson.FromJsonList<MstEventMissionData>(json);
				break;
			case MstType.EVENT_BANNER_DATA:
				obj = PrjJson.FromJsonList<MstEventBannerData>(json);
				break;
			case MstType.EVENT_IMAGE_DATA:
				obj = PrjJson.FromJsonList<MstEventImageData>(json);
				break;
			case MstType.EVENT_PERIOD_DATA:
				obj = PrjJson.FromJsonList<MstEventPeriodData>(json);
				break;
			case MstType.GACHA_DATA:
				obj = PrjJson.FromJsonList<MstGachaData>(json);
				break;
			case MstType.GACHA_DISC_DATA:
				obj = PrjJson.FromJsonList<MstGachaDiscData>(json);
				break;
			case MstType.GACHA_ITEM_DATA:
				obj = PrjJson.FromJsonList<MstGachaItemData>(json);
				break;
			case MstType.GACHA_TYPE_DATA:
				obj = PrjJson.FromJsonList<MstGachaTypeData>(json);
				break;
			case MstType.GACHA_KIND_DATA:
				obj = PrjJson.FromJsonList<MstGachaKindData>(json);
				break;
			case MstType.HOME_FURNITURE_DATA:
				obj = PrjJson.FromJsonList<MstHomeFurnitureData>(json);
				break;
			case MstType.HOME_PLACEMENT_DATA:
				obj = PrjJson.FromJsonList<MstHomePlacementData>(json);
				break;
			case MstType.HOME_FURNITURE_COUNT_DATA:
				obj = PrjJson.FromJsonList<MstHomeFurnitureCountData>(json);
				break;
			case MstType.HOME_BGM_PLAYBACK_DATA:
				obj = PrjJson.FromJsonList<MstHomeBgmPlaybackData>(json);
				break;
			case MstType.ITEM_COMMON:
				obj = PrjJson.FromJsonList<MstItemCommon>(json);
				break;
			case MstType.ITEM_PRESET:
				obj = PrjJson.FromJsonList<MstItemPreset>(json);
				break;
			case MstType.ITEM_LOTTERY:
				obj = PrjJson.FromJsonList<MstItemLottery>(json);
				break;
			case MstType.ITEM_LOTTERY_LINEUP:
				obj = PrjJson.FromJsonList<MstItemLotteryLineup>(json);
				break;
			case MstType.ITEM_RECOVERY:
				obj = PrjJson.FromJsonList<MstItemRecovery>(json);
				break;
			case MstType.LEVEL_DATA:
				obj = PrjJson.FromJsonList<MstLevelData>(json);
				break;
			case MstType.MISSION_DATA:
				obj = PrjJson.FromJsonList<MstMissionData>(json);
				break;
			case MstType.PHOTO_DATA:
				obj = PrjJson.FromJsonList<MstPhotoData>(json);
				break;
			case MstType.PHOTO_LEVEL_DATA:
				obj = PrjJson.FromJsonList<MstPhotoLevelData>(json);
				break;
			case MstType.PHOTO_RARITY_DATA:
				obj = PrjJson.FromJsonList<MstPhotoRarityData>(json);
				break;
			case MstType.PHOTO_LOT_CAMPAIGN_DATA:
				obj = PrjJson.FromJsonList<MstPhotoLotCampaignData>(json);
				break;
			case MstType.PURCHASE_PRODUCT:
				obj = PrjJson.FromJsonList<MstPurchaseProduct>(json);
				break;
			case MstType.PVP_DATA:
				obj = PrjJson.FromJsonList<MstPvpData>(json);
				break;
			case MstType.PVP_RANK_DATA:
				obj = PrjJson.FromJsonList<MstPvpRankData>(json);
				break;
			case MstType.PVP_WIN_BONUS_DATA:
				obj = PrjJson.FromJsonList<MstPvpWinningBonusData>(json);
				break;
			case MstType.PVP_TURN_BONUS_DATA:
				obj = PrjJson.FromJsonList<MstPvpTurnBonusData>(json);
				break;
			case MstType.PVP_OPP_BONUS_DATA:
				obj = PrjJson.FromJsonList<MstPvpOppBonusData>(json);
				break;
			case MstType.PVP_DEFENSE_DATA:
				obj = PrjJson.FromJsonList<MstPvpDefenseData>(json);
				break;
			case MstType.PVP_SEASON_DATA:
				obj = PrjJson.FromJsonList<MstPvpSeasonData>(json);
				break;
			case MstType.PVP_CAMPAIGN_COIN_DATA:
				obj = PrjJson.FromJsonList<MstPvPCampaignCoinData>(json);
				break;
			case MstType.QUEST_CHAPTER_DATA:
				obj = PrjJson.FromJsonList<MstQuestChapterData>(json);
				break;
			case MstType.QUEST_COMP_DATA:
				obj = PrjJson.FromJsonList<MstQuestCompData>(json);
				break;
			case MstType.QUEST_DRAW_ITEM_DATA:
				obj = PrjJson.FromJsonList<MstQuestDrawItemData>(json);
				break;
			case MstType.QUEST_ENEMIES_DATA:
				obj = PrjJson.FromJsonList<MstQuestEnemiesData>(json);
				break;
			case MstType.QUEST_ENEMY_DATA:
				obj = PrjJson.FromJsonList<MstQuestEnemyData>(json);
				break;
			case MstType.QUEST_EVAL_DATA:
				obj = PrjJson.FromJsonList<MstQuestEvalData>(json);
				break;
			case MstType.QUEST_EVAL_SET_DATA:
				obj = PrjJson.FromJsonList<MstQuestEvalSetData>(json);
				break;
			case MstType.QUEST_MAP_DATA:
				obj = PrjJson.FromJsonList<MstQuestMapData>(json);
				break;
			case MstType.QUEST_QUEST_QUESTGROUP_DATA:
				obj = PrjJson.FromJsonList<MstQuestQuestgroupData>(json);
				break;
			case MstType.QUEST_QUESTONE_DATA:
				obj = PrjJson.FromJsonList<MstQuestQuestoneData>(json);
				break;
			case MstType.QUEST_WAVE_DATA:
				obj = PrjJson.FromJsonList<MstQuestWaveData>(json);
				break;
			case MstType.SHOP_DATA:
				obj = PrjJson.FromJsonList<MstShopData>(json);
				break;
			case MstType.SHOP_ITEM_DATA:
				obj = PrjJson.FromJsonList<MstShopItemData>(json);
				break;
			case MstType.SHOP_CHARA_STATUS_DATA:
				obj = PrjJson.FromJsonList<MstShopCharaStatusData>(json);
				break;
			case MstType.BANNER_DATA:
				obj = PrjJson.FromJsonList<MstBannerData>(json);
				break;
			case MstType.ITEM_ENDTIME_DATA:
				obj = PrjJson.FromJsonList<MstItemEndtimeData>(json);
				break;
			case MstType.CHARA_KIZUNA_LEVEL_DATA:
				obj = PrjJson.FromJsonList<MstCharaKizunaLevelData>(json);
				break;
			case MstType.KIZUNA_REWARD_DATA:
				obj = PrjJson.FromJsonList<MstKizunaRewardData>(json);
				break;
			case MstType.HELP_DATA:
				obj = PrjJson.FromJsonList<MstHelpData>(json);
				break;
			case MstType.ITEM_INITPRESENT_DATA:
				obj = PrjJson.FromJsonList<MstItemInitpresentData>(json);
				break;
			case MstType.ITEM_INITFUNITURE_DATA:
				obj = PrjJson.FromJsonList<MstItemInitfunitureData>(json);
				break;
			case MstType.MOVIE_DATA:
				obj = PrjJson.FromJsonList<MstMovieData>(json);
				break;
			case MstType.TIPS_DATA:
				obj = PrjJson.FromJsonList<MstTipsData>(json);
				break;
			case MstType.QUEST_REWARDGROUP_DATA:
				obj = PrjJson.FromJsonList<MstQuestRewardgroupData>(json);
				break;
			case MstType.QUEST_CAMPAIGN_KIZUNA_DATA:
				obj = PrjJson.FromJsonList<MstQuestCampaignKizunaData>(json);
				break;
			case MstType.QUEST_CAMPAIGN_KIZUNAMAP:
				obj = PrjJson.FromJsonList<MstQuestCampaignKizunamap>(json);
				break;
			case MstType.QUEST_CAMPAIGN_ITEM_DATA:
				obj = PrjJson.FromJsonList<MstQuestCampaignItemData>(json);
				break;
			case MstType.QUEST_CAMPAIGN_ITEMMAP:
				obj = PrjJson.FromJsonList<MstQuestCampaignItemmap>(json);
				break;
			case MstType.EVENT_GROWTH_DATA:
				obj = PrjJson.FromJsonList<MstEventGrowthData>(json);
				break;
			case MstType.PICNIC_DATA:
				obj = PrjJson.FromJsonList<MstPicnicData>(json);
				break;
			case MstType.PICNIC_FOOD_DATA:
				obj = PrjJson.FromJsonList<MstPicnicFoodData>(json);
				break;
			case MstType.PICNIC_GETTIME_DATA:
				obj = PrjJson.FromJsonList<MstPicnicGettimeData>(json);
				break;
			case MstType.QUEST_MODE_OPEN_DATA:
				obj = PrjJson.FromJsonList<MstQuestModeOpenData>(json);
				break;
			case MstType.PURCHASE_MONTHLYPACK_DATA:
				obj = PrjJson.FromJsonList<MstPurchaseMonthlypackData>(json);
				break;
			case MstType.MONTHLYPACK_MESSAGE_DATA:
				obj = PrjJson.FromJsonList<MstMonthlypackMessageData>(json);
				break;
			case MstType.ITEM_BONUS_DATA:
				obj = PrjJson.FromJsonList<MstItemBonusData>(json);
				break;
			case MstType.QUEST_ENEMYFRIENDS_DATA:
				obj = PrjJson.FromJsonList<MstQuestEnemyfriendsData>(json);
				break;
			case MstType.EVENT_BONUS_CHARA_DATA:
				obj = PrjJson.FromJsonList<MstEventBonusCharaData>(json);
				break;
			case MstType.QUEST_QUESTDROP_ITEM_DATA:
				obj = PrjJson.FromJsonList<MstQuestQuestdropItemData>(json);
				break;
			case MstType.QUEST_PHOTO_DROP_ITEM_DATA:
				obj = PrjJson.FromJsonList<MstQuestPhotoDropItemData>(json);
				break;
			case MstType.TRAINING_SEASON_DATA:
				obj = PrjJson.FromJsonList<MstTrainingSeasonData>(json);
				break;
			case MstType.TRAINING_DAYOFWEEK_DATA:
				obj = PrjJson.FromJsonList<MstTrainingDayofweekData>(json);
				break;
			case MstType.TRAINING_REWARD_DATA:
				obj = PrjJson.FromJsonList<MstTrainingRewardData>(json);
				break;
			case MstType.KEMOBOARD_AREA_DATA:
				obj = PrjJson.FromJsonList<MstKemoboardAreaData>(json);
				break;
			case MstType.KEMOBOARD_PANEL_DATA:
				obj = PrjJson.FromJsonList<MstKemoboardPanelData>(json);
				break;
			case MstType.GAME_APPEARANCE_DATA:
				obj = PrjJson.FromJsonList<MstGameAppearanceData>(json);
				break;
			case MstType.QUEST_CAMPAIGN_STAMINA_DATA:
				obj = PrjJson.FromJsonList<MstQuestCampaignStaminaData>(json);
				break;
			case MstType.QUEST_TEXTINFO_DATA:
				obj = PrjJson.FromJsonList<MstQuestTextinfoData>(json);
				break;
			case MstType.PICNIC_PLAY_TYPE_DATA:
				obj = PrjJson.FromJsonList<MstPicnicPlayTypeData>(json);
				break;
			case MstType.PICNIC_PLAY_ITEM_DATA:
				obj = PrjJson.FromJsonList<MstPicnicPlayItemData>(json);
				break;
			case MstType.CHARA_MOTION_ITEM_DATA:
				obj = PrjJson.FromJsonList<MstCharaMotionItemData>(json);
				break;
			case MstType.CHARA_LIMITLEVEL_DATA:
				obj = PrjJson.FromJsonList<MstCharaLimitlevelData>(json);
				break;
			case MstType.LIMITLEVEL_RISING_STATUS:
				obj = PrjJson.FromJsonList<MstLimitLevelRaisingStatus>(json);
				break;
			case MstType.PLATFORM_STATUS_DATA:
				obj = PrjJson.FromJsonList<MstPlatformStatusData>(json);
				break;
			case MstType.PHOTO_CHARACTERISTIC_DATA:
				obj = PrjJson.FromJsonList<MstPhotoCharacteristicData>(json);
				break;
			case MstType.MASTER_SKILL_DATA:
				obj = PrjJson.FromJsonList<MstMasterSkillData>(json);
				break;
			case MstType.MASTER_SKILL_LEVEL_DATA:
				obj = PrjJson.FromJsonList<MstMasterSkillLevelData>(json);
				break;
			case MstType.MASTER_SKILL_LEVEL_ITEM:
				obj = PrjJson.FromJsonList<MstMasterSkillLevelItem>(json);
				break;
			case MstType.CHARA_KIZUNA_REWARD_DATA:
				obj = PrjJson.FromJsonList<MstCharaKizunaRewardData>(json);
				break;
			case MstType.CHARA_GROUP_DATA:
				obj = PrjJson.FromJsonList<MstCharaGroupData>(json);
				break;
			case MstType.MASTER_ROOM_CONFIG_DATA:
				obj = PrjJson.FromJsonList<MstMasterRoomConfigData>(json);
				break;
			case MstType.MASTER_ROOM_FURNITURE_DATA:
				obj = PrjJson.FromJsonList<MstMasterRoomFurnitureData>(json);
				break;
			case MstType.MASTER_ROOM_KIZUNA_BONUS:
				obj = PrjJson.FromJsonList<MstMasterRoomKizunaBonus>(json);
				break;
			case MstType.MASTER_ROOM_ACTION_DATA:
				obj = PrjJson.FromJsonList<MstMasterRoomActionData>(json);
				break;
			case MstType.PVPSPECIAL_DATA:
				obj = PrjJson.FromJsonList<MstPvpspecialData>(json);
				break;
			case MstType.ACCESSORY_DATA:
				obj = PrjJson.FromJsonList<MstAccessoryData>(json);
				break;
			case MstType.ACCESSORY_RARITY_DATA:
				obj = PrjJson.FromJsonList<MstAccessoryRarityData>(json);
				break;
			case MstType.MASTER_ROOM_MACHINE_DATA:
				obj = PrjJson.FromJsonList<MstMasterRoomMachineData>(json);
				break;
			case MstType.MASTER_ROOM_FUEL_DATA:
				obj = PrjJson.FromJsonList<MstMasterRoomFuelData>(json);
				break;
			case MstType.MASTER_ROOM_STAMINA_BONUS:
				obj = PrjJson.FromJsonList<MstMasterRoomStaminaBonus>(json);
				break;
			case MstType.PHOTO_GROWBONUS_DATA:
				obj = PrjJson.FromJsonList<MstPhotoGrowbonusData>(json);
				break;
			case MstType.ADVERTISE_BANNER_DATA:
				obj = PrjJson.FromJsonList<MstAdvertiseBannerData>(json);
				break;
			case MstType.STAMINA_RECOVERY_CAMPAIGN_TIME_DATA:
				obj = PrjJson.FromJsonList<MstStaminaRecoveryCampaignTimeData>(json);
				break;
			case MstType.MONTHLYPACK_CONTINUE_DATA:
				obj = PrjJson.FromJsonList<MstMonthlypackContinueData>(json);
				break;
			case MstType.NPC_DATA:
				obj = PrjJson.FromJsonList<MstNpcData>(json);
				break;
			case MstType.ENEMY_CHARA_DATA:
				obj = PrjJson.FromJsonList<MstEnemyCharaData>(json);
				break;
			case MstType.PICNIC_CAMPAIGN_DATA:
				obj = PrjJson.FromJsonList<MstPicnicCampaignData>(json);
				break;
			case MstType.SCENARIO_LOGIN:
				obj = PrjJson.FromJsonList<MstScenarioLogin>(json);
				break;
			case MstType.RANDOM_SCENARIO_LOGIN:
				obj = PrjJson.FromJsonList<MstRandomScenarioLogin>(json);
				break;
			case MstType.CHARA_NANAIRO_ABILITY_RELEASE_DATA:
				obj = PrjJson.FromJsonList<MstCharaNanairoAbilityReleaseData>(json);
				break;
			case MstType.ACHIEVEMENT_DATA:
				obj = PrjJson.FromJsonList<MstAchievementData>(json);
				break;
			case MstType.COOP_RAID_TERM_DATA:
				obj = PrjJson.FromJsonList<MstEventCoopRaidTermData>(json);
				break;
			case MstType.COOP_RAID_DRAW_DATA:
				obj = PrjJson.FromJsonList<MstEventCoopRaidDrawData>(json);
				break;
			case MstType.HOME_INTRODUCTION_NEW_CHARA_DATA:
				obj = PrjJson.FromJsonList<MstHomeIntroductionNewCharaData>(json);
				break;
			case MstType.CHARA_KIZUNA_LOT_CAMPAIGN_DATA:
				obj = PrjJson.FromJsonList<MstCharaKizunaLotCampaignData>(json);
				break;
			case MstType.CHARA_KIZUNA_BUFF_DATA:
				obj = PrjJson.FromJsonList<MstCharaKizunaBuffData>(json);
				break;
			case MstType.ITEM_EXCHANGE_RATES_DATA:
				obj = PrjJson.FromJsonList<MstItemExchangeRatesData>(json);
				break;
			case MstType.TRAINING_PRACTICE_TRIAL_DATA:
				obj = PrjJson.FromJsonList<MstTrainingPracticeTrialData>(json);
				break;
			case MstType.CHARA_EFFECT_DATA:
				obj = PrjJson.FromJsonList<MstCharaEffectData>(json);
				break;
			case MstType.ASSISTANT_DATA:
				obj = PrjJson.FromJsonList<MstAssistantData>(json);
				break;
			case MstType.QUEST_RULE_DATA:
				obj = PrjJson.FromJsonList<MstQuestRuleData>(json);
				break;
			case MstType.STICKER_DATA:
				obj = PrjJson.FromJsonList<MstStickerData>(json);
				break;
			case MstType.STICKER_PLAYER_EXP_BONUS:
				obj = PrjJson.FromJsonList<MstStickerPlayerExpBonus>(json);
				break;
			}
			return obj;
		}
	}
}
