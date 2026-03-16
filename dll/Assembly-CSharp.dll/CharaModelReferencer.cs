using System;
using System.Collections.Generic;
using UnityEngine;

public class CharaModelReferencer : MonoBehaviour
{
	public void Awake()
	{
		this.JoinReferenceParam();
	}

	public void Update()
	{
	}

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

	private void setScale(Transform bone)
	{
		CharaModelReferencer.ScaleParam scaleParam = this.refAnimationObj.GetComponent<CharaModelReferencer>().scaleParamList.Find((CharaModelReferencer.ScaleParam itm) => itm.boneName == bone.name);
		if (scaleParam != null)
		{
			bone.localScale = scaleParam.boneScale;
		}
	}

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

	public void SetRootPos(bool reset)
	{
		this.root.localPosition = (reset ? this.basePos : this.rootPos);
	}

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

	public GameObject refAnimationObj;

	private bool isJoin;

	private Transform root;

	private List<Transform> boneList = new List<Transform>();

	private List<Osage> osageList = new List<Osage>();

	private Vector3 basePos = Vector3.zero;

	private Vector3 rootPos = Vector3.zero;

	public CharaMotionDefine.MotionPersonalityType motionPersonalityType;

	public List<CharaModelReferencer.RefAnime> prefabAnimeList = new List<CharaModelReferencer.RefAnime>();

	public List<CharaModelReferencer.OsageParam> osageParamList = new List<CharaModelReferencer.OsageParam>();

	public float rootPosY;

	public List<CharaModelReferencer.ScaleParam> scaleParamList = new List<CharaModelReferencer.ScaleParam>();

	public Color eyeColor = new Color(0f, 0f, 0f, 0f);

	public string tailNodeName = "";

	public string earNodeName = "";

	public string variantCopyPrefabName = "";

	public bool ussVariantCopyPrefabVoice;

	[Serializable]
	public class RefAnime
	{
		public RefAnime()
		{
		}

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

		public string name = "";

		public AnimationClip clip;

		public string seName = "";

		public int earBefore;

		public int earFrame;

		public int earAfter;

		public int tailBefore;

		public int tailFrame;

		public int tailAfter;

		public string effectName = "";
	}

	[Serializable]
	public class OsageParam
	{
		public string boneName;

		public Vector3 boneAxis;

		public float stiffnessForce;

		public float dragForce;

		public Vector3 springForce;
	}

	[Serializable]
	public class ScaleParam
	{
		public string boneName;

		public Vector3 boneScale;
	}
}
