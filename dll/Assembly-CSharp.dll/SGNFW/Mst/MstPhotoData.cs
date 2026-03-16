using System;

namespace SGNFW.Mst
{
	[Serializable]
	public class MstPhotoData
	{
		public int id;

		public int rarity;

		public string name;

		public string reading;

		public string flavorTextBefore;

		public string flavorTextAfter;

		public int type;

		public string illustratorName;

		public string illustratorNameAfter;

		public int levelTableId;

		public int hpParamLv1;

		public int hpParamLvMiddle;

		public int hpLvMiddleNum;

		public int hpParamLvMax;

		public int atkParamLv1;

		public int atkParamLvMiddle;

		public int atkLvMiddleNum;

		public int atkParamLvMax;

		public int defParamLv1;

		public int defParamLvMiddle;

		public int defLvMiddleNum;

		public int defParamLvMax;

		public int imgFlg;

		public int kizunaPhotoFlg;

		public int noDestoryFlg;

		public int expPhotoFlg;

		public int limitOverFlg;

		public int abilityEffectChangeFlg;

		public long qeDispDatetime;

		public long startTime;

		public int albumCategory;

		public string infoGettext;

		public int limitMaxRewardId;

		public int levelMaxRewardId;

		public int noUseLimitOverPhotoFlg;
	}
}
