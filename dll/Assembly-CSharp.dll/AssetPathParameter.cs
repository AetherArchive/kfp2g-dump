using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000EE RID: 238
public class AssetPathParameter : ScriptableObject
{
	// Token: 0x04000897 RID: 2199
	public List<AssetPathParameter.Data> DataList;

	// Token: 0x020007FE RID: 2046
	[Serializable]
	public class Data
	{
		// Token: 0x040035D7 RID: 13783
		public string path;

		// Token: 0x040035D8 RID: 13784
		public Vector2 feedWindowSize;
	}
}
