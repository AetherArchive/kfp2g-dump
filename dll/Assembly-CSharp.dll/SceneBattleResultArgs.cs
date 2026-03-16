using System;
using System.Collections.Generic;
using Battle;
using UnityEngine;

public class SceneBattleResultArgs
{
	public SceneBattleArgs battleArgs;

	public SceneBattle_DeckInfo deck;

	public SceneBattle_DeckInfo pvpDeck;

	public SceneBattle_QuestInfo quest;

	public HelperPackData helper;

	public int userLevel;

	public long userExp;

	public List<int> charaLevel;

	public List<long> charaExp;

	public List<int> kizunaLevel;

	public List<long> kizunaExp;

	public DataManagerQuest.BattleEndStatus battleEndStatus;

	public List<bool> battleMissionStatus;

	public GameObject resultField;

	public string resultVoiceFirstSheet;

	public string resultVoiceFirst;

	public float resultVoiceFirstLength;

	public string resultVoiceSecondSheet;

	public string resultVoiceSecond;

	public DateTime resultVoiceSecondTime;

	public int clearTurn;

	public int trainingRevive;

	public long trainingScore;

	public bool isSkip;

	public bool restart;

	public bool debug;

	public int tryCount;

	public int haveGoldNum;

	public string bgm;

	public DateTime battleStartTime;
}
