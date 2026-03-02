using System;
using System.Collections.Generic;
using Battle;
using UnityEngine;

// Token: 0x0200011F RID: 287
public class SceneBattleResultArgs
{
	// Token: 0x04000D3A RID: 3386
	public SceneBattleArgs battleArgs;

	// Token: 0x04000D3B RID: 3387
	public SceneBattle_DeckInfo deck;

	// Token: 0x04000D3C RID: 3388
	public SceneBattle_DeckInfo pvpDeck;

	// Token: 0x04000D3D RID: 3389
	public SceneBattle_QuestInfo quest;

	// Token: 0x04000D3E RID: 3390
	public HelperPackData helper;

	// Token: 0x04000D3F RID: 3391
	public int userLevel;

	// Token: 0x04000D40 RID: 3392
	public long userExp;

	// Token: 0x04000D41 RID: 3393
	public List<int> charaLevel;

	// Token: 0x04000D42 RID: 3394
	public List<long> charaExp;

	// Token: 0x04000D43 RID: 3395
	public List<int> kizunaLevel;

	// Token: 0x04000D44 RID: 3396
	public List<long> kizunaExp;

	// Token: 0x04000D45 RID: 3397
	public DataManagerQuest.BattleEndStatus battleEndStatus;

	// Token: 0x04000D46 RID: 3398
	public List<bool> battleMissionStatus;

	// Token: 0x04000D47 RID: 3399
	public GameObject resultField;

	// Token: 0x04000D48 RID: 3400
	public string resultVoiceFirstSheet;

	// Token: 0x04000D49 RID: 3401
	public string resultVoiceFirst;

	// Token: 0x04000D4A RID: 3402
	public float resultVoiceFirstLength;

	// Token: 0x04000D4B RID: 3403
	public string resultVoiceSecondSheet;

	// Token: 0x04000D4C RID: 3404
	public string resultVoiceSecond;

	// Token: 0x04000D4D RID: 3405
	public DateTime resultVoiceSecondTime;

	// Token: 0x04000D4E RID: 3406
	public int clearTurn;

	// Token: 0x04000D4F RID: 3407
	public int trainingRevive;

	// Token: 0x04000D50 RID: 3408
	public long trainingScore;

	// Token: 0x04000D51 RID: 3409
	public bool isSkip;

	// Token: 0x04000D52 RID: 3410
	public bool restart;

	// Token: 0x04000D53 RID: 3411
	public bool debug;

	// Token: 0x04000D54 RID: 3412
	public int tryCount;

	// Token: 0x04000D55 RID: 3413
	public int haveGoldNum;

	// Token: 0x04000D56 RID: 3414
	public string bgm;

	// Token: 0x04000D57 RID: 3415
	public DateTime battleStartTime;
}
