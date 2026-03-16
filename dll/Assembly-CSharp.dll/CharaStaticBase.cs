using System;
using System.Collections.Generic;
using SGNFW.Mst;

[Serializable]
public class CharaStaticBase
{
	public int maxKizunaLevel
	{
		get
		{
			return 6;
		}
	}

	public string charaName
	{
		get
		{
			return this.name;
		}
	}

	public string charaNameEng
	{
		get
		{
			return this.nameEn;
		}
	}

	public string NickName { get; private set; }

	public int OriginalId { get; private set; }

	public int TfBeforeId { get; private set; }

	public HashSet<int> SynonymIdSet { get; set; }

	public CharaDef.AttributeMask attributeMask
	{
		get
		{
			if (this.attribute != CharaDef.AttributeType.ALL)
			{
				return (CharaDef.AttributeMask)Enum.Parse(typeof(CharaDef.AttributeMask), this.attribute.ToString(), true);
			}
			return (CharaDef.AttributeMask)0;
		}
	}

	public CharaDef.AttributeMask subAttributeMask
	{
		get
		{
			if (this.subAttribute != CharaDef.AttributeType.ALL)
			{
				return (CharaDef.AttributeMask)Enum.Parse(typeof(CharaDef.AttributeMask), this.subAttribute.ToString(), true);
			}
			return (CharaDef.AttributeMask)0;
		}
	}

	public int levelTableId
	{
		get
		{
			return this.expTableId;
		}
	}

	public int kizunaTableId
	{
		get
		{
			return this.kizunaLevelId;
		}
	}

	public int gradeTableId
	{
		get
		{
			return this.rankId;
		}
	}

	public int photoFrameTableId
	{
		get
		{
			return this.ppId;
		}
	}

	public int atrsTableId
	{
		get
		{
			return this.artsId;
		}
	}

	public bool isFlyingTypeByHome
	{
		get
		{
			return this.homeTypeFlying == 2;
		}
	}

	public bool isFloating
	{
		get
		{
			return this.isFlyingTypeByHome || this.homeTypeFlying == 3;
		}
	}

	public bool isTreeTypeByHome
	{
		get
		{
			return this.homeTypeTree == 2;
		}
	}

	public bool isNightTypeByHome
	{
		get
		{
			return this.homeTypeActTime == 2;
		}
	}

	public string animalImagePath
	{
		get
		{
			return "Texture2D/AnimalPicture/animalpicture_" + this.assetId.ToString("0000");
		}
	}

	public string animalTabitat { get; set; }

	public int AnimalRedListType
	{
		get
		{
			return this.animalRedList;
		}
	}

	public bool notOpenDispFlg
	{
		get
		{
			return TimeManager.Now < this.startDatetime || this.endDatetime < TimeManager.Now;
		}
	}

	public CharaStaticBase(CharaStaticAlphaBase csab, MstCharaData mcd, int _assetId)
	{
		this.id = mcd.id;
		this.rankLow = mcd.rankLow;
		this.rankHigh = mcd.rankHigh;
		this.name = mcd.name;
		this.nameEn = mcd.nameEn;
		this.eponymName = mcd.eponymName;
		this.NickName = mcd.nickname;
		this.OriginalId = mcd.originalId;
		this.TfBeforeId = mcd.tfBeforeId;
		this.SynonymIdSet = new HashSet<int>();
		this.attribute = (CharaDef.AttributeType)mcd.attribute;
		this.subAttribute = (CharaDef.AttributeType)mcd.subAttribute;
		this.castName = mcd.castName;
		this.flavorText = mcd.flavorText;
		this.greetingText = mcd.greetingText;
		this.promotePresetId = mcd.promotePresetId;
		this.expTableId = mcd.expTableId;
		this.ppId = mcd.ppId;
		this.ppItemId = mcd.ppItemId;
		this.kizunaPhotoId = mcd.kizunaPhotoId;
		this.artsId = mcd.artsId;
		this.rankId = mcd.rankId;
		this.rankItemId = mcd.rankItemId;
		this.clothesPresetId = mcd.clothesPresetId;
		this.duplicateItemId = mcd.duplicateItemId;
		this.homeTypeFlying = mcd.homeTypeFlying;
		this.homeTypeTree = mcd.homeTypeTree;
		this.homeTypeActTime = mcd.homeTypeActTime;
		this.animalScientificName = mcd.animalScientificName;
		this.animalImageProvider = mcd.animalImageProvider;
		this.animalTabitat = mcd.animalTabitat;
		this.animalDistributionArea = mcd.animalDistributionArea;
		this.animalRedList = mcd.animalRedList;
		this.animalFlavorText = mcd.animalFlavorText;
		this.kizunaLevelId = mcd.kizunaLevelId;
		this.loginText = mcd.loginText;
		this.loginSubText = mcd.loginSubText;
		this.hometouchStandList = new List<string> { mcd.hometouchStand01, mcd.hometouchStand02, mcd.hometouchStand03, mcd.hometouchStand04 };
		this.hometouchSleepList = new List<string> { mcd.hometouchSleep01, mcd.hometouchSleep02, mcd.hometouchSleep03 };
		this.hometouchWalkList = new List<string> { mcd.hometouchWalk01, mcd.hometouchWalk02, mcd.hometouchWalk03, mcd.hometouchWalk04, mcd.hometouchWalk05 };
		this.kizunaupText = mcd.kizunaupText;
		this.startDatetime = new DateTime(PrjUtil.ConvertTimeToTicks(mcd.startTime));
		this.endDatetime = new DateTime(PrjUtil.ConvertTimeToTicks(mcd.endTime));
		this.picnicGettimeId = mcd.picnicGettimeId;
		this.spAbilityRelPp = mcd.spAbilityRelPp;
		this.plasmPoint = csab.plasmPoint;
		this.hpParamLv1 = csab.hpParamLv1;
		this.hpParamLvMiddle = csab.hpParamLvMiddle;
		this.hpLvMiddleNum = csab.hpLvMiddleNum;
		this.hpParamLv99 = csab.hpParamLv99;
		this.atkParamLv1 = csab.atkParamLv1;
		this.atkParamLvMiddle = csab.atkParamLvMiddle;
		this.atkLvMiddleNum = csab.atkLvMiddleNum;
		this.atkParamLv99 = csab.atkParamLv99;
		this.defParamLv1 = csab.defParamLv1;
		this.defParamLvMiddle = csab.defParamLvMiddle;
		this.defLvMiddleNum = csab.defLvMiddleNum;
		this.defParamLv99 = csab.defParamLv99;
		this.maxStockMp = csab.maxStockMp;
		this.avoidRatio = csab.avoidRatio;
		this.width = csab.width;
		this.height = csab.height;
		this.AbnormalEffectHeadScale = csab.AbnormalEffectHeadScale;
		this.AbnormalEffectHeadY = csab.AbnormalEffectHeadY;
		this.AbnormalEffectHeadZ = csab.AbnormalEffectHeadZ;
		this.AbnormalEffectRootScale = csab.AbnormalEffectRootScale;
		this.AbnormalEffectRootY = csab.AbnormalEffectRootY;
		this.AbnormalEffectRootZ = csab.AbnormalEffectRootZ;
		this.isSpecialFlagSupported = csab.isSpecialFlagSupported;
		this.orderCardType00 = csab.orderCardType00;
		this.orderCardType01 = csab.orderCardType01;
		this.orderCardType02 = csab.orderCardType02;
		this.orderCardType03 = csab.orderCardType03;
		this.orderCardType04 = csab.orderCardType04;
		this.orderCardValue00 = csab.orderCardValue00;
		this.orderCardValue01 = csab.orderCardValue01;
		this.orderCardValue02 = csab.orderCardValue02;
		this.orderCardValue03 = csab.orderCardValue03;
		this.orderCardValue04 = csab.orderCardValue04;
		this.orderCardSPValueMP = csab.orderCardSPValueMP;
		this.orderCardSPValuePlasm = csab.orderCardSPValuePlasm;
		this.modelEffect = new List<string>(csab.modelEffect);
		this.cameraOffsetX = csab.cameraOffsetX;
		this.cameraOffsetY = csab.cameraOffsetY;
		this.cameraOffsetZ = csab.cameraOffsetZ;
		this.cameraRotationX = csab.cameraRotationX;
		this.cameraRotationY = csab.cameraRotationY;
		this.cameraRotationZ = csab.cameraRotationZ;
		this.entryLpNum = csab.entryLpNum;
		this.entryEffect = csab.entryEffect;
		this.assetId = _assetId;
		this.risingStatusPatternId = mcd.risingStatusPatternId;
	}

	public int id;

	public CharaDef.Type charaType;

	public int rankLow;

	public int rankHigh;

	private string name;

	public string eponymName;

	private string nameEn;

	public string greetingText;

	public CharaDef.AttributeType attribute;

	public CharaDef.AttributeType subAttribute;

	public int plasmPoint;

	public string castName;

	public string flavorText;

	public int clothesPresetId;

	public int kizunaPhotoId;

	public int promotePresetId;

	public int rankId;

	public int artsId;

	public int ppId;

	public int expTableId;

	public int kizunaLevelId;

	public int rankItemId;

	public int ppItemId;

	public int duplicateItemId;

	public int hpParamLv1;

	public int hpParamLvMiddle;

	public int hpLvMiddleNum;

	public int hpParamLv99;

	public int atkParamLv1;

	public int atkParamLvMiddle;

	public int atkLvMiddleNum;

	public int atkParamLv99;

	public int defParamLv1;

	public int defParamLvMiddle;

	public int defLvMiddleNum;

	public int defParamLv99;

	public int maxStockMp;

	public int avoidRatio;

	public float width;

	public float height;

	public float AbnormalEffectHeadScale;

	public float AbnormalEffectHeadY;

	public float AbnormalEffectHeadZ;

	public float AbnormalEffectRootScale;

	public float AbnormalEffectRootY;

	public float AbnormalEffectRootZ;

	public bool isSpecialFlagSupported;

	public CharaDef.OrderCardType orderCardType00;

	public CharaDef.OrderCardType orderCardType01;

	public CharaDef.OrderCardType orderCardType02;

	public CharaDef.OrderCardType orderCardType03;

	public CharaDef.OrderCardType orderCardType04;

	public int orderCardValue00;

	public int orderCardValue01;

	public int orderCardValue02;

	public int orderCardValue03;

	public int orderCardValue04;

	public int orderCardSPValueMP;

	public int orderCardSPValuePlasm;

	public List<string> modelEffect;

	public float cameraOffsetX;

	public float cameraOffsetY;

	public float cameraOffsetZ;

	public float cameraRotationX;

	public float cameraRotationY;

	public float cameraRotationZ;

	public int entryLpNum;

	public string entryEffect;

	public int homeTypeFlying;

	public int homeTypeTree;

	public int homeTypeActTime;

	public string animalScientificName;

	public string animalImageProvider;

	public string animalDistributionArea;

	public int animalRedList;

	public string animalFlavorText;

	public string loginText;

	public string loginSubText;

	public List<string> hometouchStandList;

	public List<string> hometouchSleepList;

	public List<string> hometouchWalkList;

	public string kizunaupText;

	public int picnicGettimeId;

	public DateTime startDatetime;

	public DateTime endDatetime;

	public int spAbilityRelPp;

	public int assetId;

	public int risingStatusPatternId;
}
