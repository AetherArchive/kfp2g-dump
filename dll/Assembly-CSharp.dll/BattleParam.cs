using System;
using UnityEngine;

public class BattleParam : ScriptableObject
{
	public float attributeGood;

	public float attributeBad;

	public float[] sequenceDamage;

	public float[] sequenceHeal;

	public float[] chainBeat;

	public int[] chainAction;

	public int[] chainTry;

	public int[] chainBowl;

	public float poisonFriends;

	public float poisonEnemy;

	public float poisonBoss;

	public float sleepFriends;

	public float sleepEnemy;

	public float sleepBoss;

	public int defenseBase;

	public int iceAvoidFriends;

	public int iceAvoidEnemy;

	public int iceAvoidBoss;

	public float iceRcvFriends;

	public float iceRcvEnemy;

	public float iceRcvBoss;

	public int iceCancelFriends;

	public int iceCancelEnemy;

	public int iceCancelBoss;

	public float bleedFriends;

	public float bleedEnemy;

	public float bleedBoss;

	public float burnedFriends;

	public float burnedEnemy;

	public float burnedBoss;

	public BattleParam.Tickling mySideTickling;

	public BattleParam.Tickling enemySideTickling;

	public int paralysisFriends;

	public int paralysisEnemy;

	public int paralysisBoss;

	public int focusFriends;

	public int focusEnemy;

	public int focusBoss;

	[Serializable]
	public class Tickling
	{
		public int incidenceRate;

		public int successRate;

		public int num;

		public int mpDecreaseRate;
	}
}
