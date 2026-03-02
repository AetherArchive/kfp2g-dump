using System;
using System.Collections.Generic;
using SGNFW.Mst;

// Token: 0x02000056 RID: 86
[Serializable]
public class CharaStaticBase
{
	// Token: 0x1700005D RID: 93
	// (get) Token: 0x06000265 RID: 613 RVA: 0x00014B1E File Offset: 0x00012D1E
	public int maxKizunaLevel
	{
		get
		{
			return 6;
		}
	}

	// Token: 0x1700005E RID: 94
	// (get) Token: 0x06000266 RID: 614 RVA: 0x00014B21 File Offset: 0x00012D21
	public string charaName
	{
		get
		{
			return this.name;
		}
	}

	// Token: 0x1700005F RID: 95
	// (get) Token: 0x06000267 RID: 615 RVA: 0x00014B29 File Offset: 0x00012D29
	public string charaNameEng
	{
		get
		{
			return this.nameEn;
		}
	}

	// Token: 0x17000060 RID: 96
	// (get) Token: 0x06000268 RID: 616 RVA: 0x00014B31 File Offset: 0x00012D31
	// (set) Token: 0x06000269 RID: 617 RVA: 0x00014B39 File Offset: 0x00012D39
	public string NickName { get; private set; }

	// Token: 0x17000061 RID: 97
	// (get) Token: 0x0600026A RID: 618 RVA: 0x00014B42 File Offset: 0x00012D42
	// (set) Token: 0x0600026B RID: 619 RVA: 0x00014B4A File Offset: 0x00012D4A
	public int OriginalId { get; private set; }

	// Token: 0x17000062 RID: 98
	// (get) Token: 0x0600026C RID: 620 RVA: 0x00014B53 File Offset: 0x00012D53
	// (set) Token: 0x0600026D RID: 621 RVA: 0x00014B5B File Offset: 0x00012D5B
	public int TfBeforeId { get; private set; }

	// Token: 0x17000063 RID: 99
	// (get) Token: 0x0600026E RID: 622 RVA: 0x00014B64 File Offset: 0x00012D64
	// (set) Token: 0x0600026F RID: 623 RVA: 0x00014B6C File Offset: 0x00012D6C
	public HashSet<int> SynonymIdSet { get; set; }

	// Token: 0x17000064 RID: 100
	// (get) Token: 0x06000270 RID: 624 RVA: 0x00014B75 File Offset: 0x00012D75
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

	// Token: 0x17000065 RID: 101
	// (get) Token: 0x06000271 RID: 625 RVA: 0x00014BA7 File Offset: 0x00012DA7
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

	// Token: 0x17000066 RID: 102
	// (get) Token: 0x06000272 RID: 626 RVA: 0x00014BD9 File Offset: 0x00012DD9
	public int levelTableId
	{
		get
		{
			return this.expTableId;
		}
	}

	// Token: 0x17000067 RID: 103
	// (get) Token: 0x06000273 RID: 627 RVA: 0x00014BE1 File Offset: 0x00012DE1
	public int kizunaTableId
	{
		get
		{
			return this.kizunaLevelId;
		}
	}

	// Token: 0x17000068 RID: 104
	// (get) Token: 0x06000274 RID: 628 RVA: 0x00014BE9 File Offset: 0x00012DE9
	public int gradeTableId
	{
		get
		{
			return this.rankId;
		}
	}

	// Token: 0x17000069 RID: 105
	// (get) Token: 0x06000275 RID: 629 RVA: 0x00014BF1 File Offset: 0x00012DF1
	public int photoFrameTableId
	{
		get
		{
			return this.ppId;
		}
	}

	// Token: 0x1700006A RID: 106
	// (get) Token: 0x06000276 RID: 630 RVA: 0x00014BF9 File Offset: 0x00012DF9
	public int atrsTableId
	{
		get
		{
			return this.artsId;
		}
	}

	// Token: 0x1700006B RID: 107
	// (get) Token: 0x06000277 RID: 631 RVA: 0x00014C01 File Offset: 0x00012E01
	public bool isFlyingTypeByHome
	{
		get
		{
			return this.homeTypeFlying == 2;
		}
	}

	// Token: 0x1700006C RID: 108
	// (get) Token: 0x06000278 RID: 632 RVA: 0x00014C0C File Offset: 0x00012E0C
	public bool isFloating
	{
		get
		{
			return this.isFlyingTypeByHome || this.homeTypeFlying == 3;
		}
	}

	// Token: 0x1700006D RID: 109
	// (get) Token: 0x06000279 RID: 633 RVA: 0x00014C21 File Offset: 0x00012E21
	public bool isTreeTypeByHome
	{
		get
		{
			return this.homeTypeTree == 2;
		}
	}

	// Token: 0x1700006E RID: 110
	// (get) Token: 0x0600027A RID: 634 RVA: 0x00014C2C File Offset: 0x00012E2C
	public bool isNightTypeByHome
	{
		get
		{
			return this.homeTypeActTime == 2;
		}
	}

	// Token: 0x1700006F RID: 111
	// (get) Token: 0x0600027B RID: 635 RVA: 0x00014C37 File Offset: 0x00012E37
	public string animalImagePath
	{
		get
		{
			return "Texture2D/AnimalPicture/animalpicture_" + this.assetId.ToString("0000");
		}
	}

	// Token: 0x17000070 RID: 112
	// (get) Token: 0x0600027C RID: 636 RVA: 0x00014C53 File Offset: 0x00012E53
	// (set) Token: 0x0600027D RID: 637 RVA: 0x00014C5B File Offset: 0x00012E5B
	public string animalTabitat { get; set; }

	// Token: 0x17000071 RID: 113
	// (get) Token: 0x0600027E RID: 638 RVA: 0x00014C64 File Offset: 0x00012E64
	public int AnimalRedListType
	{
		get
		{
			return this.animalRedList;
		}
	}

	// Token: 0x17000072 RID: 114
	// (get) Token: 0x0600027F RID: 639 RVA: 0x00014C6C File Offset: 0x00012E6C
	public bool notOpenDispFlg
	{
		get
		{
			return TimeManager.Now < this.startDatetime || this.endDatetime < TimeManager.Now;
		}
	}

	// Token: 0x06000280 RID: 640 RVA: 0x00014C94 File Offset: 0x00012E94
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

	// Token: 0x040002BD RID: 701
	public int id;

	// Token: 0x040002BE RID: 702
	public CharaDef.Type charaType;

	// Token: 0x040002BF RID: 703
	public int rankLow;

	// Token: 0x040002C0 RID: 704
	public int rankHigh;

	// Token: 0x040002C1 RID: 705
	private string name;

	// Token: 0x040002C2 RID: 706
	public string eponymName;

	// Token: 0x040002C3 RID: 707
	private string nameEn;

	// Token: 0x040002C8 RID: 712
	public string greetingText;

	// Token: 0x040002C9 RID: 713
	public CharaDef.AttributeType attribute;

	// Token: 0x040002CA RID: 714
	public CharaDef.AttributeType subAttribute;

	// Token: 0x040002CB RID: 715
	public int plasmPoint;

	// Token: 0x040002CC RID: 716
	public string castName;

	// Token: 0x040002CD RID: 717
	public string flavorText;

	// Token: 0x040002CE RID: 718
	public int clothesPresetId;

	// Token: 0x040002CF RID: 719
	public int kizunaPhotoId;

	// Token: 0x040002D0 RID: 720
	public int promotePresetId;

	// Token: 0x040002D1 RID: 721
	public int rankId;

	// Token: 0x040002D2 RID: 722
	public int artsId;

	// Token: 0x040002D3 RID: 723
	public int ppId;

	// Token: 0x040002D4 RID: 724
	public int expTableId;

	// Token: 0x040002D5 RID: 725
	public int kizunaLevelId;

	// Token: 0x040002D6 RID: 726
	public int rankItemId;

	// Token: 0x040002D7 RID: 727
	public int ppItemId;

	// Token: 0x040002D8 RID: 728
	public int duplicateItemId;

	// Token: 0x040002D9 RID: 729
	public int hpParamLv1;

	// Token: 0x040002DA RID: 730
	public int hpParamLvMiddle;

	// Token: 0x040002DB RID: 731
	public int hpLvMiddleNum;

	// Token: 0x040002DC RID: 732
	public int hpParamLv99;

	// Token: 0x040002DD RID: 733
	public int atkParamLv1;

	// Token: 0x040002DE RID: 734
	public int atkParamLvMiddle;

	// Token: 0x040002DF RID: 735
	public int atkLvMiddleNum;

	// Token: 0x040002E0 RID: 736
	public int atkParamLv99;

	// Token: 0x040002E1 RID: 737
	public int defParamLv1;

	// Token: 0x040002E2 RID: 738
	public int defParamLvMiddle;

	// Token: 0x040002E3 RID: 739
	public int defLvMiddleNum;

	// Token: 0x040002E4 RID: 740
	public int defParamLv99;

	// Token: 0x040002E5 RID: 741
	public int maxStockMp;

	// Token: 0x040002E6 RID: 742
	public int avoidRatio;

	// Token: 0x040002E7 RID: 743
	public float width;

	// Token: 0x040002E8 RID: 744
	public float height;

	// Token: 0x040002E9 RID: 745
	public float AbnormalEffectHeadScale;

	// Token: 0x040002EA RID: 746
	public float AbnormalEffectHeadY;

	// Token: 0x040002EB RID: 747
	public float AbnormalEffectHeadZ;

	// Token: 0x040002EC RID: 748
	public float AbnormalEffectRootScale;

	// Token: 0x040002ED RID: 749
	public float AbnormalEffectRootY;

	// Token: 0x040002EE RID: 750
	public float AbnormalEffectRootZ;

	// Token: 0x040002EF RID: 751
	public bool isSpecialFlagSupported;

	// Token: 0x040002F0 RID: 752
	public CharaDef.OrderCardType orderCardType00;

	// Token: 0x040002F1 RID: 753
	public CharaDef.OrderCardType orderCardType01;

	// Token: 0x040002F2 RID: 754
	public CharaDef.OrderCardType orderCardType02;

	// Token: 0x040002F3 RID: 755
	public CharaDef.OrderCardType orderCardType03;

	// Token: 0x040002F4 RID: 756
	public CharaDef.OrderCardType orderCardType04;

	// Token: 0x040002F5 RID: 757
	public int orderCardValue00;

	// Token: 0x040002F6 RID: 758
	public int orderCardValue01;

	// Token: 0x040002F7 RID: 759
	public int orderCardValue02;

	// Token: 0x040002F8 RID: 760
	public int orderCardValue03;

	// Token: 0x040002F9 RID: 761
	public int orderCardValue04;

	// Token: 0x040002FA RID: 762
	public int orderCardSPValueMP;

	// Token: 0x040002FB RID: 763
	public int orderCardSPValuePlasm;

	// Token: 0x040002FC RID: 764
	public List<string> modelEffect;

	// Token: 0x040002FD RID: 765
	public float cameraOffsetX;

	// Token: 0x040002FE RID: 766
	public float cameraOffsetY;

	// Token: 0x040002FF RID: 767
	public float cameraOffsetZ;

	// Token: 0x04000300 RID: 768
	public float cameraRotationX;

	// Token: 0x04000301 RID: 769
	public float cameraRotationY;

	// Token: 0x04000302 RID: 770
	public float cameraRotationZ;

	// Token: 0x04000303 RID: 771
	public int entryLpNum;

	// Token: 0x04000304 RID: 772
	public string entryEffect;

	// Token: 0x04000305 RID: 773
	public int homeTypeFlying;

	// Token: 0x04000306 RID: 774
	public int homeTypeTree;

	// Token: 0x04000307 RID: 775
	public int homeTypeActTime;

	// Token: 0x04000308 RID: 776
	public string animalScientificName;

	// Token: 0x04000309 RID: 777
	public string animalImageProvider;

	// Token: 0x0400030B RID: 779
	public string animalDistributionArea;

	// Token: 0x0400030C RID: 780
	public int animalRedList;

	// Token: 0x0400030D RID: 781
	public string animalFlavorText;

	// Token: 0x0400030E RID: 782
	public string loginText;

	// Token: 0x0400030F RID: 783
	public string loginSubText;

	// Token: 0x04000310 RID: 784
	public List<string> hometouchStandList;

	// Token: 0x04000311 RID: 785
	public List<string> hometouchSleepList;

	// Token: 0x04000312 RID: 786
	public List<string> hometouchWalkList;

	// Token: 0x04000313 RID: 787
	public string kizunaupText;

	// Token: 0x04000314 RID: 788
	public int picnicGettimeId;

	// Token: 0x04000315 RID: 789
	public DateTime startDatetime;

	// Token: 0x04000316 RID: 790
	public DateTime endDatetime;

	// Token: 0x04000317 RID: 791
	public int spAbilityRelPp;

	// Token: 0x04000318 RID: 792
	public int assetId;

	// Token: 0x04000319 RID: 793
	public int risingStatusPatternId;
}
