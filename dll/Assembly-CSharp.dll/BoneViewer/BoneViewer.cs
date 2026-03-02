using System;
using System.Collections.Generic;
using UnityEngine;

namespace BoneViewer
{
	// Token: 0x0200056F RID: 1391
	[ExecuteInEditMode]
	public class BoneViewer : MonoBehaviour
	{
		// Token: 0x06002E12 RID: 11794 RVA: 0x001B1272 File Offset: 0x001AF472
		private void Awake()
		{
			this.mMesh = base.gameObject.GetComponent<SkinnedMeshRenderer>();
			if (this.mMesh == null)
			{
				return;
			}
			this.mBones = this.mMesh.bones;
		}

		// Token: 0x04002890 RID: 10384
		private SkinnedMeshRenderer mMesh;

		// Token: 0x04002891 RID: 10385
		private Transform[] mBones;

		// Token: 0x04002892 RID: 10386
		private List<Vector3> mBoneVector;

		// Token: 0x04002893 RID: 10387
		private List<string> mBoneName;

		// Token: 0x04002894 RID: 10388
		[Header("【骨を球で表示するか】")]
		public bool ShowBoneMarker = true;

		// Token: 0x04002895 RID: 10389
		[Header("【骨表示球の大きさ】")]
		[Range(0f, 2f)]
		public float BoneMarkerSize = 0.1f;

		// Token: 0x04002896 RID: 10390
		[Header("【骨を表示する球の色】")]
		public Color mBonecolor = Color.blue;

		// Token: 0x04002897 RID: 10391
		[Header("【骨を結ぶ線の表示】")]
		public bool ShowLines = true;

		// Token: 0x04002898 RID: 10392
		[Header("【骨を接続する線の色】")]
		public Color mLinecolor = Color.red;

		// Token: 0x04002899 RID: 10393
		[Header("【骨の名前の表示】")]
		public bool ShowBoneName;

		// Token: 0x0400289A RID: 10394
		[Header("【骨の名前表示の色】")]
		public Color mBoneNameColor = Color.black;
	}
}
