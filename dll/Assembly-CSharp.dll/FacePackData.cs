using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000065 RID: 101
public class FacePackData : MonoBehaviour
{
	// Token: 0x0600029C RID: 668 RVA: 0x0001574C File Offset: 0x0001394C
	public static FacePackData Id2PackData(string id)
	{
		return FacePackData.GetAllPackData(false).Find((FacePackData item) => item.id == id);
	}

	// Token: 0x0600029D RID: 669 RVA: 0x00015780 File Offset: 0x00013980
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

	// Token: 0x0400042D RID: 1069
	public string id;

	// Token: 0x0400042E RID: 1070
	public string devName;

	// Token: 0x0400042F RID: 1071
	public float[] blendShapeParam = new float[27];

	// Token: 0x04000430 RID: 1072
	public Color eyeColor;

	// Token: 0x04000431 RID: 1073
	public Color cheekColor;

	// Token: 0x04000432 RID: 1074
	public bool isEyeFollow;

	// Token: 0x04000433 RID: 1075
	public bool isMouseFollow;

	// Token: 0x04000434 RID: 1076
	private static List<FacePackData> facePackDataList;
}
