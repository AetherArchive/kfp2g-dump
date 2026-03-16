using System;
using System.Collections.Generic;
using UnityEngine;

public class AssetPathParameter : ScriptableObject
{
	public List<AssetPathParameter.Data> DataList;

	[Serializable]
	public class Data
	{
		public string path;

		public Vector2 feedWindowSize;
	}
}
