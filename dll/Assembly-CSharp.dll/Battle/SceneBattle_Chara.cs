using System;
using System.Collections.Generic;
using AEAuth3;
using UnityEngine;
using UnityEngine.UI;

namespace Battle
{
	public class SceneBattle_Chara
	{
		public int idx;

		public int number;

		public int wave;

		public int charaId;

		public string charaName;

		public CharaModelHandle chara;

		public Vector3 basPos;

		public Vector3 pos;

		public float offZ;

		public CharaDef.Type type;

		public bool rare;

		public CharaDef.AttributeType attribute;

		public CharaDef.AttributeMask attributeMask;

		public CharaDef.AttributeType subAttribute;

		public CharaDef.AttributeMask subAttributeMask;

		public Dictionary<int, int> abilityBuff;

		public List<int> abilityBuffKey;

		public CharaStaticAction arts;

		public List<EnemyAttackData.Param> normalAttack;

		public CharaStaticAction specialAttack;

		public CharaStaticAction specialFlagAttack;

		public int level;

		public int total;

		public int nowHp;

		public int maxHp;

		public int dspHp;

		public string helHp;

		public int nowKp;

		public int maxKp;

		public int dspKp;

		public int atkPwr;

		public int defPwr;

		public int avoidRatio;

		public int actionDamageRatio;

		public int tryDamageRatio;

		public int beatDamageRatio;

		public List<SceneBattle_Buff> buff;

		public int newHp;

		public int newKp;

		public List<SceneBattle_Buff> newBuff;

		public int newGuts;

		public int guts;

		public List<int> recover;

		public List<int> newRecover;

		public int escapeHp;

		public Vector3 hpPos;

		public GameObject hpGage;

		public Image hpBar;

		public Image kpBar;

		public AEImage kpAE;

		public GameObject kpLock;

		public GameObject kpNoMP;

		public GameObject kpDbl;

		public Transform touchGage;

		public GameObject tagMark;

		public PguiReplaceAECtrl tagMarkAE;

		public AEImage dmgAE;

		public Transform statIcon;

		public List<SceneBattle_InfoBuff> stat;

		public int statIdx;

		public float statTim;

		public List<SceneBattle_InfoBuff> goodStat;

		public float goodStatTim;

		public List<SceneBattle_InfoBuff> badStat;

		public float badStatTim;

		public EffectData effStun;

		public EffectData effPoison;

		public EffectData effSleep;

		public EffectData effIce;

		public float width;

		public float height;

		public Vector3 abnormalHead;

		public Vector3 abnormalRoot;

		public int artsLevel;

		public bool artsMax;

		public int increaseKpByAttack;

		public int increaseKpByDamage;

		public bool bleed;

		public bool burned;

		public SceneBattle_Chara cover;

		public int coverStep;

		public Vector3 coverPos;

		public int wildPoint;
	}
}
