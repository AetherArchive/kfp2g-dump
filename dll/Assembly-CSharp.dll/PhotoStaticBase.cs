using System;
using SGNFW.Mst;

public class PhotoStaticBase
{
	public int id { get; private set; }

	public string photoName { get; private set; }

	public string Reading { get; private set; }

	public string illustrator { get; private set; }

	public string illustratorafter { get; private set; }

	public string flavorText { get; private set; }

	public string flavorTextAfter { get; private set; }

	public ItemDef.Rarity rarity { get; private set; }

	public int levelTableId { get; private set; }

	public bool imageChangeFlg { get; private set; }

	public PhotoDef.Type type { get; private set; }

	public bool kizunaPhotoFlg { get; private set; }

	public bool forbiddenDiscardFlg { get; private set; }

	public PhotoDef.ExpPhotoType expPhotoType { get; private set; }

	public bool isForbiddenGrowBase
	{
		get
		{
			return this.type == PhotoDef.Type.OTHER;
		}
	}

	public bool isForbiddenEquip
	{
		get
		{
			return this.type == PhotoDef.Type.OTHER;
		}
	}

	public bool isForbiddenUseLimitOverPhoto { get; private set; }

	public PhotoDef.AlbumCategory albumCategory { get; private set; }

	public string InfoGetText { get; private set; }

	public int hpParamLv1 { get; private set; }

	public int hpParamLvMiddle { get; private set; }

	public int hpLvMiddleNum { get; private set; }

	public int hpParamLvMax { get; private set; }

	public int atkParamLv1 { get; private set; }

	public int atkParamLvMiddle { get; private set; }

	public int atkLvMiddleNum { get; private set; }

	public int atkParamLvMax { get; private set; }

	public int defParamLv1 { get; private set; }

	public int defParamLvMiddle { get; private set; }

	public int defLvMiddleNum { get; private set; }

	public int defParamLvMax { get; private set; }

	public bool AbilityEffectChange { get; private set; }

	public DateTime StartDateTime { get; private set; }

	public DateTime qrDispStartTime { get; private set; }

	public PhotoStaticBase(MstPhotoData mst)
	{
		this.id = mst.id;
		this.type = (PhotoDef.Type)mst.type;
		this.photoName = mst.name;
		this.Reading = mst.reading;
		this.illustrator = mst.illustratorName;
		this.illustratorafter = mst.illustratorNameAfter;
		this.flavorText = mst.flavorTextBefore;
		this.flavorTextAfter = mst.flavorTextAfter;
		this.rarity = (ItemDef.Rarity)mst.rarity;
		this.levelTableId = mst.levelTableId;
		this.expPhotoType = (PhotoDef.ExpPhotoType)mst.expPhotoFlg;
		this.forbiddenDiscardFlg = mst.noDestoryFlg != 0;
		this.isForbiddenUseLimitOverPhoto = mst.noUseLimitOverPhotoFlg != 0;
		this.imageChangeFlg = mst.imgFlg != 0;
		this.kizunaPhotoFlg = mst.kizunaPhotoFlg != 0;
		this.albumCategory = (PhotoDef.AlbumCategory)mst.albumCategory;
		this.InfoGetText = mst.infoGettext;
		this.hpParamLv1 = mst.hpParamLv1;
		this.hpParamLvMiddle = mst.hpParamLvMiddle;
		this.hpLvMiddleNum = mst.hpLvMiddleNum;
		this.hpParamLvMax = mst.hpParamLvMax;
		this.atkParamLv1 = mst.atkParamLv1;
		this.atkParamLvMiddle = mst.atkParamLvMiddle;
		this.atkLvMiddleNum = mst.atkLvMiddleNum;
		this.atkParamLvMax = mst.atkParamLvMax;
		this.defParamLv1 = mst.defParamLv1;
		this.defParamLvMiddle = mst.defParamLvMiddle;
		this.defLvMiddleNum = mst.defLvMiddleNum;
		this.defParamLvMax = mst.defParamLvMax;
		this.AbilityEffectChange = 1 == mst.abilityEffectChangeFlg;
		this.StartDateTime = new DateTime(PrjUtil.ConvertTimeToTicks(mst.startTime));
		this.qrDispStartTime = new DateTime(PrjUtil.ConvertTimeToTicks(mst.qeDispDatetime));
	}

	public const int MAX_LEVEL_RANK = 4;
}
