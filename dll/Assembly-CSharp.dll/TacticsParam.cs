using System;
using System.Collections.Generic;
using UnityEngine;

public class TacticsParam : ScriptableObject
{
	public List<TacticsParam.Tactics> tacticsParam;

	[Serializable]
	public class Tactics
	{
		public TacticsParam.Tactics.Type type;

		public string tacticsName;

		public string paramInfo;

		public List<int> param;

		public enum Type
		{
			INVALID,
			TURN_NUM,
			GIVEUP_NUM,
			HP_PER
		}
	}
}
