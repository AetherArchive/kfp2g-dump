using System;
using System.Collections.Generic;
using UnityEngine;

namespace BoneViewer
{
	[ExecuteInEditMode]
	public class BoneViewer : MonoBehaviour
	{
		private void Awake()
		{
			this.mMesh = base.gameObject.GetComponent<SkinnedMeshRenderer>();
			if (this.mMesh == null)
			{
				return;
			}
			this.mBones = this.mMesh.bones;
		}

		private SkinnedMeshRenderer mMesh;

		private Transform[] mBones;

		private List<Vector3> mBoneVector;

		private List<string> mBoneName;

		[Header("【骨を球で表示するか】")]
		public bool ShowBoneMarker = true;

		[Header("【骨表示球の大きさ】")]
		[Range(0f, 2f)]
		public float BoneMarkerSize = 0.1f;

		[Header("【骨を表示する球の色】")]
		public Color mBonecolor = Color.blue;

		[Header("【骨を結ぶ線の表示】")]
		public bool ShowLines = true;

		[Header("【骨を接続する線の色】")]
		public Color mLinecolor = Color.red;

		[Header("【骨の名前の表示】")]
		public bool ShowBoneName;

		[Header("【骨の名前表示の色】")]
		public Color mBoneNameColor = Color.black;
	}
}
