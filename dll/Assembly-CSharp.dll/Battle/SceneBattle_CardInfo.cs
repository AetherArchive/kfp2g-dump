using System;
using UnityEngine;

namespace Battle
{
	public class SceneBattle_CardInfo
	{
		public SceneBattle_CardInfo()
		{
		}

		public SceneBattle_CardInfo(SceneBattle_CardInfo original)
		{
			this.cid = original.cid;
			this.card = original.card;
			this.chara = original.chara;
			this.select = original.select;
			this.selArts = original.selArts;
			this.action = original.action;
			this.actArts = original.actArts;
			this.selGry = original.selGry;
			this.artGry = original.artGry;
		}

		public int cid;

		public CharaOrderCard card;

		public GameObject chara;

		public GameObject select;

		public GameObject selArts;

		public GameObject action;

		public GameObject actArts;

		public Grayscale selGry;

		public Grayscale artGry;
	}
}
