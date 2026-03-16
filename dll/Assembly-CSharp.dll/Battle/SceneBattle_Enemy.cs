using System;
using UnityEngine;

namespace Battle
{
	public class SceneBattle_Enemy : SceneBattle_Chara
	{
		public int actionNum;

		public CharaDef.EnemyActionPattern actionPattern;

		public int actCount;

		public ItemData dropItem;

		public GameObject authObj;

		public bool death;

		public int huge;

		public float hugeX;

		public SceneBattle_Enemy body;

		public string partsName;

		public string partsModelName;

		public CharaDef.PartsType partsType;

		public Transform parts;

		public GameObject partModel;

		public ModelHandle modelHandle;

		public float actZ;

		public string deathEffect;

		public float deathEffectScale;
	}
}
