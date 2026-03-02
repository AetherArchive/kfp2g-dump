using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001E8 RID: 488
public class CharaModelReferencer : MonoBehaviour
{
	// Token: 0x06002089 RID: 8329 RVA: 0x0018B971 File Offset: 0x00189B71
	public void Awake()
	{
		this.JoinReferenceParam();
	}

	// Token: 0x0600208A RID: 8330 RVA: 0x0018B979 File Offset: 0x00189B79
	public void Update()
	{
	}

	// Token: 0x0600208B RID: 8331 RVA: 0x0018B97C File Offset: 0x00189B7C
	public void JoinReferenceParam()
	{
		if (this.isJoin || this.refAnimationObj == null)
		{
			return;
		}
		this.isJoin = true;
		this.root = base.transform.Find("root");
		SimpleAnimation simpleAnimation = this.root.GetComponent<SimpleAnimation>();
		if (simpleAnimation == null)
		{
			simpleAnimation = this.root.parent.gameObject.GetComponent<SimpleAnimation>();
		}
		simpleAnimation.ExInit();
		foreach (CharaModelReferencer.RefAnime refAnime in this.refAnimationObj.GetComponent<CharaModelReferencer>().prefabAnimeList)
		{
			if (refAnime.clip != null)
			{
				simpleAnimation.AddState(refAnime.clip, refAnime.name);
			}
		}
		this.MakeBoneList(this.root);
		this.rootPos = (this.basePos = this.root.localPosition);
		if (this.refAnimationObj.GetComponent<CharaModelReferencer>().scaleParamList.Count > 0)
		{
			this.root.localPosition = (this.rootPos = new Vector3(this.root.localPosition.x, this.refAnimationObj.GetComponent<CharaModelReferencer>().rootPosY, this.root.localPosition.z));
		}
		foreach (Transform transform in this.boneList)
		{
			this.setOsage(transform);
			this.setScale(transform);
		}
	}

	// Token: 0x0600208C RID: 8332 RVA: 0x0018BB2C File Offset: 0x00189D2C
	public static void JoinReferenceAnimParam(SimpleAnimation target, CharaModelReferencer assignData)
	{
		target.ExInit();
		foreach (CharaModelReferencer.RefAnime refAnime in assignData.prefabAnimeList)
		{
			if (refAnime.clip != null && target.GetState(refAnime.name) == null)
			{
				target.AddState(refAnime.clip, refAnime.name);
			}
		}
	}

	// Token: 0x0600208D RID: 8333 RVA: 0x0018BBB0 File Offset: 0x00189DB0
	private void MakeBoneList(Transform bone)
	{
		if (bone.GetComponent<CharaModelReferencer>() != null)
		{
			return;
		}
		if (bone.name.IndexOf("j_") == 0 || bone.name == "root" || bone.name == "pelvis")
		{
			this.boneList.Add(bone);
		}
		foreach (object obj in bone)
		{
			Transform transform = (Transform)obj;
			this.MakeBoneList(transform);
		}
	}

	// Token: 0x0600208E RID: 8334 RVA: 0x0018BC58 File Offset: 0x00189E58
	private void setOsage(Transform bone)
	{
		Osage osage = bone.GetComponent<Osage>();
		if (osage == null && bone.name.IndexOf("j_osg") == 0)
		{
			osage = bone.gameObject.AddComponent<Osage>();
		}
		CharaModelReferencer.OsageParam osageParam = this.refAnimationObj.GetComponent<CharaModelReferencer>().osageParamList.Find((CharaModelReferencer.OsageParam itm) => itm.boneName == bone.name);
		if (osageParam != null)
		{
			if (osage == null)
			{
				osage = bone.gameObject.AddComponent<Osage>();
			}
			osage.boneAxis = osageParam.boneAxis;
			osage.stiffnessForce = osageParam.stiffnessForce;
			osage.dragForce = osageParam.dragForce;
			osage.springForce = osageParam.springForce;
		}
		else if (osage != null && bone.name.IndexOf("j_osg") != 0)
		{
			Object.Destroy(osage);
			osage = null;
		}
		if (osage != null)
		{
			this.osageList.Add(osage);
		}
	}

	// Token: 0x0600208F RID: 8335 RVA: 0x0018BD60 File Offset: 0x00189F60
	private void setScale(Transform bone)
	{
		CharaModelReferencer.ScaleParam scaleParam = this.refAnimationObj.GetComponent<CharaModelReferencer>().scaleParamList.Find((CharaModelReferencer.ScaleParam itm) => itm.boneName == bone.name);
		if (scaleParam != null)
		{
			bone.localScale = scaleParam.boneScale;
		}
	}

	// Token: 0x06002090 RID: 8336 RVA: 0x0018BDB0 File Offset: 0x00189FB0
	public void setScaleOne()
	{
		if (!this.isJoin || this.refAnimationObj == null)
		{
			return;
		}
		foreach (Transform transform in this.boneList)
		{
			transform.localScale = Vector3.one;
		}
		this.root.localPosition = (this.rootPos = Vector3.zero);
	}

	// Token: 0x06002091 RID: 8337 RVA: 0x0018BE38 File Offset: 0x0018A038
	public void UpdateOsage()
	{
		if (!this.isJoin || this.refAnimationObj == null)
		{
			return;
		}
		foreach (Osage osage in this.osageList)
		{
			osage.UpdateOsage();
		}
	}

	// Token: 0x06002092 RID: 8338 RVA: 0x0018BEA0 File Offset: 0x0018A0A0
	public void LateUpdateOsage(Transform thighL, Transform thighR)
	{
		if (!this.isJoin || this.refAnimationObj == null)
		{
			return;
		}
		foreach (Osage osage in this.osageList)
		{
			osage.LateUpdateOsage(thighL, thighR);
		}
	}

	// Token: 0x06002093 RID: 8339 RVA: 0x0018BF0C File Offset: 0x0018A10C
	public void SetRootPos(bool reset)
	{
		this.root.localPosition = (reset ? this.basePos : this.rootPos);
	}

	// Token: 0x06002094 RID: 8340 RVA: 0x0018BF2C File Offset: 0x0018A12C
	public void ResetOsage()
	{
		if (!this.isJoin || this.refAnimationObj == null)
		{
			return;
		}
		foreach (Osage osage in this.osageList)
		{
			osage.Reset();
		}
	}

	// Token: 0x0400179C RID: 6044
	public GameObject refAnimationObj;

	// Token: 0x0400179D RID: 6045
	private bool isJoin;

	// Token: 0x0400179E RID: 6046
	private Transform root;

	// Token: 0x0400179F RID: 6047
	private List<Transform> boneList = new List<Transform>();

	// Token: 0x040017A0 RID: 6048
	private List<Osage> osageList = new List<Osage>();

	// Token: 0x040017A1 RID: 6049
	private Vector3 basePos = Vector3.zero;

	// Token: 0x040017A2 RID: 6050
	private Vector3 rootPos = Vector3.zero;

	// Token: 0x040017A3 RID: 6051
	public CharaMotionDefine.MotionPersonalityType motionPersonalityType;

	// Token: 0x040017A4 RID: 6052
	public List<CharaModelReferencer.RefAnime> prefabAnimeList = new List<CharaModelReferencer.RefAnime>();

	// Token: 0x040017A5 RID: 6053
	public List<CharaModelReferencer.OsageParam> osageParamList = new List<CharaModelReferencer.OsageParam>();

	// Token: 0x040017A6 RID: 6054
	public float rootPosY;

	// Token: 0x040017A7 RID: 6055
	public List<CharaModelReferencer.ScaleParam> scaleParamList = new List<CharaModelReferencer.ScaleParam>();

	// Token: 0x040017A8 RID: 6056
	public Color eyeColor = new Color(0f, 0f, 0f, 0f);

	// Token: 0x040017A9 RID: 6057
	public string tailNodeName = "";

	// Token: 0x040017AA RID: 6058
	public string earNodeName = "";

	// Token: 0x040017AB RID: 6059
	public string variantCopyPrefabName = "";

	// Token: 0x040017AC RID: 6060
	public bool ussVariantCopyPrefabVoice;

	// Token: 0x0200102B RID: 4139
	[Serializable]
	public class RefAnime
	{
		// Token: 0x06005211 RID: 21009 RVA: 0x00248453 File Offset: 0x00246653
		public RefAnime()
		{
		}

		// Token: 0x06005212 RID: 21010 RVA: 0x0024847C File Offset: 0x0024667C
		public RefAnime(string nam, AnimationClip clp, string se, int eb, int ef, int ea, int tb, int tf, int ta, string en)
		{
			this.name = nam;
			this.clip = clp;
			this.seName = se;
			this.earBefore = eb;
			this.earFrame = ef;
			this.earAfter = ea;
			this.tailBefore = tb;
			this.tailFrame = tf;
			this.tailAfter = ta;
			this.effectName = en;
		}

		// Token: 0x04005ABF RID: 23231
		public string name = "";

		// Token: 0x04005AC0 RID: 23232
		public AnimationClip clip;

		// Token: 0x04005AC1 RID: 23233
		public string seName = "";

		// Token: 0x04005AC2 RID: 23234
		public int earBefore;

		// Token: 0x04005AC3 RID: 23235
		public int earFrame;

		// Token: 0x04005AC4 RID: 23236
		public int earAfter;

		// Token: 0x04005AC5 RID: 23237
		public int tailBefore;

		// Token: 0x04005AC6 RID: 23238
		public int tailFrame;

		// Token: 0x04005AC7 RID: 23239
		public int tailAfter;

		// Token: 0x04005AC8 RID: 23240
		public string effectName = "";
	}

	// Token: 0x0200102C RID: 4140
	[Serializable]
	public class OsageParam
	{
		// Token: 0x04005AC9 RID: 23241
		public string boneName;

		// Token: 0x04005ACA RID: 23242
		public Vector3 boneAxis;

		// Token: 0x04005ACB RID: 23243
		public float stiffnessForce;

		// Token: 0x04005ACC RID: 23244
		public float dragForce;

		// Token: 0x04005ACD RID: 23245
		public Vector3 springForce;
	}

	// Token: 0x0200102D RID: 4141
	[Serializable]
	public class ScaleParam
	{
		// Token: 0x04005ACE RID: 23246
		public string boneName;

		// Token: 0x04005ACF RID: 23247
		public Vector3 boneScale;
	}
}
