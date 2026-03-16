using System;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
	public class SceneBattle_Action
	{
		public SceneBattle_Chara chara;

		public SceneBattle_CardInfo card;

		public CharaStaticAction actParam;

		public int arts;

		public bool synergy;

		public int chain;

		public SceneBattle_Chara target;

		public List<SceneBattle_Act> act;

		public List<SceneBattle_Eff> eff;

		public List<SceneBattle_Tag> total;

		public float blwTim;

		public Vector3 blwSpd;

		public Vector3 blwPos;

		public List<SceneBattle_Chara> blwChr;

		public Vector3 pos;

		public Vector3 tagPos;

		public float tagSiz;

		public SceneBattle.CMDSTEP step;

		public float actTim;

		public float lokTim;

		public float dedTim;

		public List<SceneBattle_Chara> dedChr;

		public float chrTim;

		public float quakeTim;

		public SceneBattle_Missile camMssl;

		public VOICE_TYPE voice;

		public float voiceDelay;

		public float retTim;

		public bool dmgkp;

		public Dictionary<SceneBattle_Chara, int> reflect;

		public int refFlg;

		public List<SceneBattle_Tag> refAbility;

		public List<SceneBattle_Tag> refTactics;

		public Dictionary<SceneBattle_Chara, int> perDamage;

		public List<SceneBattle_Tag> perAbility;

		public List<SceneBattle_Tag> perTactics;

		public List<KeyValuePair<SceneBattle_Tag, SceneBattle_Buff>> givBuf;

		public bool paralysis;

		public bool silence;
	}
}
