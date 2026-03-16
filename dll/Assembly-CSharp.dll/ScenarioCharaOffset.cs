using System;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioCharaOffset : ScriptableObject
{
	public List<ScenarioSetValues.CharaOffset> mCharaOffset;

	public List<ScenarioCharaOffset.CharaPosition> mCharaPosition;

	[Serializable]
	public class CharaPosition
	{
		public string name;

		public Vector3 position;

		public Vector3 rotation;

		public Vector3 scale;

		public string shake;

		public string arrows;
	}
}
