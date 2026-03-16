using System;
using System.Collections.Generic;
using UnityEngine;

public class FacePackData : MonoBehaviour
{
	public static FacePackData Id2PackData(string id)
	{
		return FacePackData.GetAllPackData(false).Find((FacePackData item) => item.id == id);
	}

	public static List<FacePackData> GetAllPackData(bool isReload = false)
	{
		if (FacePackData.facePackDataList == null || isReload)
		{
			FacePackData.facePackDataList = new List<FacePackData>();
			foreach (Object @object in AssetManager.GetAssetDataByCategory(AssetManager.ASSET_CATEGORY_FACE_PACK))
			{
				GameObject gameObject = @object as GameObject;
				if (!(gameObject == null))
				{
					FacePackData.facePackDataList.Add(gameObject.GetComponent<FacePackData>());
				}
			}
		}
		return FacePackData.facePackDataList;
	}

	public string id;

	public string devName;

	public float[] blendShapeParam = new float[27];

	public Color eyeColor;

	public Color cheekColor;

	public bool isEyeFollow;

	public bool isMouseFollow;

	private static List<FacePackData> facePackDataList;
}
