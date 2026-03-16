using System;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioSetValues : MonoBehaviour
{
	[SerializeField]
	public Vector3[] mCharaPosition;

	[SerializeField]
	public Vector3[] mCharaRotation;

	[SerializeField]
	public List<ScenarioSetValues.CharaOffset> mCharaOffset;

	[Serializable]
	public class CharaOffset
	{
		public string model;

		public Vector3 position;

		public Vector3 scale;
	}
}
