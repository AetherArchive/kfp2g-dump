using System;
using System.Collections.Generic;
using SGNFW.Mst;

public class GameLevelInfo
{
	public int level;

	public long userLevelExp;

	public int staminaLimit;

	public Dictionary<int, GameLevelInfo.KizunaLevelData> kizunaLevelExp = new Dictionary<int, GameLevelInfo.KizunaLevelData>();

	public Dictionary<int, long> charaLevelExp = new Dictionary<int, long>();

	public Dictionary<int, long> photoLevelExp = new Dictionary<int, long>();

	public class KizunaLevelData
	{
		public KizunaLevelData(long exp, MstCharaKizunaLevelData data)
		{
			this.LevelExp = exp;
			this.releaseItemId = data.releaseItemId;
			this.releaseItemNum = data.releaseItemNum;
		}

		public long LevelExp;

		public int releaseItemId;

		public int releaseItemNum;
	}
}
