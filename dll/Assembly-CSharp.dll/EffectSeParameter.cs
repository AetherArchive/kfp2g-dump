using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectSeParameter : ScriptableObject
{
	public List<EffectSeParameter.PackData> packList;

	[Serializable]
	public class PackData
	{
		public string effectName;

		public string seName;

		public string cuesheetName = "se_cb";
	}
}
