using System;
using System.Collections.Generic;
using UnityEngine;

public class TreeHouseSmallFurnitureData : ScriptableObject
{
	public int reactionId;

	public List<TreeHouseSmallFurnitureData.MotionData> motionDataList;

	public List<TreeHouseSmallFurnitureData.OptData> optDataList;

	[Serializable]
	public class MotionData
	{
		public CharaMotionDefine.ActKey actKey;

		public string modelName;

		public string nodeName;

		public Vector3 havePos;

		public Vector3 haveRot;
	}

	[Serializable]
	public class OptData
	{
		public string modelName;

		public string nodeName;

		public Vector3 putPos;

		public Vector3 putRot;
	}
}
