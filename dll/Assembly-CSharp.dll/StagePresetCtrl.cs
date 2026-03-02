using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x020000EA RID: 234
public class StagePresetCtrl : MonoBehaviour
{
	// Token: 0x1700029A RID: 666
	// (get) Token: 0x06000AAC RID: 2732 RVA: 0x0003DE12 File Offset: 0x0003C012
	// (set) Token: 0x06000AAD RID: 2733 RVA: 0x0003DE1A File Offset: 0x0003C01A
	public GameObject stageModelObj { get; private set; }

	// Token: 0x1700029B RID: 667
	// (get) Token: 0x06000AAE RID: 2734 RVA: 0x0003DE23 File Offset: 0x0003C023
	// (set) Token: 0x06000AAF RID: 2735 RVA: 0x0003DE2B File Offset: 0x0003C02B
	public GameObject skyModelObj { get; private set; }

	// Token: 0x1700029C RID: 668
	// (get) Token: 0x06000AB0 RID: 2736 RVA: 0x0003DE34 File Offset: 0x0003C034
	// (set) Token: 0x06000AB1 RID: 2737 RVA: 0x0003DE3C File Offset: 0x0003C03C
	public GameObject cameraEffectObj { get; private set; }

	// Token: 0x1700029D RID: 669
	// (get) Token: 0x06000AB2 RID: 2738 RVA: 0x0003DE45 File Offset: 0x0003C045
	// (set) Token: 0x06000AB3 RID: 2739 RVA: 0x0003DE4D File Offset: 0x0003C04D
	public Light lightByStage { get; private set; }

	// Token: 0x1700029E RID: 670
	// (get) Token: 0x06000AB4 RID: 2740 RVA: 0x0003DE56 File Offset: 0x0003C056
	// (set) Token: 0x06000AB5 RID: 2741 RVA: 0x0003DE5E File Offset: 0x0003C05E
	public Light lightByPlayer { get; private set; }

	// Token: 0x1700029F RID: 671
	// (get) Token: 0x06000AB6 RID: 2742 RVA: 0x0003DE67 File Offset: 0x0003C067
	// (set) Token: 0x06000AB7 RID: 2743 RVA: 0x0003DE6F File Offset: 0x0003C06F
	public Light lightByEnemy { get; private set; }

	// Token: 0x170002A0 RID: 672
	// (get) Token: 0x06000AB8 RID: 2744 RVA: 0x0003DE78 File Offset: 0x0003C078
	// (set) Token: 0x06000AB9 RID: 2745 RVA: 0x0003DE80 File Offset: 0x0003C080
	public List<KeyValuePair<GameObject, StagePresetCtrl.FbxPutInfo>> fbxPutObjectList { get; private set; }

	// Token: 0x06000ABA RID: 2746 RVA: 0x0003DE8C File Offset: 0x0003C08C
	private void Awake()
	{
		if (this.StageAuthHideManager == null)
		{
			this.StageAuthHideManager = new GameObject("AuthHideRoot", new Type[] { typeof(StagePutObjManager) }).GetComponent<StagePutObjManager>();
			this.StageAuthHideManager.transform.SetParent(base.transform, false);
			this.StageAuthHideManager.transform.SetAsFirstSibling();
		}
	}

	// Token: 0x06000ABB RID: 2747 RVA: 0x0003DEF6 File Offset: 0x0003C0F6
	private void Start()
	{
	}

	// Token: 0x06000ABC RID: 2748 RVA: 0x0003DEF8 File Offset: 0x0003C0F8
	private void Update()
	{
		if (this.animePutObject != null)
		{
			foreach (KeyValuePair<StagePresetCtrl.FbxPutInfo, StagePresetCtrl.AnimePutInfo> keyValuePair in this.animePutObject)
			{
				foreach (KeyValuePair<int, Vector3> keyValuePair2 in keyValuePair.Value.movTop)
				{
					Vector3 vector = keyValuePair2.Value * keyValuePair.Key.paramList[0] * Time.realtimeSinceStartup;
					Vector3 vector2 = keyValuePair.Value.powerTop * keyValuePair.Key.paramList[1];
					keyValuePair.Value.vertices[keyValuePair2.Key] = keyValuePair.Value.verticesOrg[keyValuePair2.Key] + Vector3.Scale(new Vector3(Mathf.Sin(vector.x), Mathf.Sin(vector.y), Mathf.Sin(vector.z)), vector2);
				}
				foreach (KeyValuePair<int, Vector3> keyValuePair3 in keyValuePair.Value.movMdl)
				{
					Vector3 vector3 = keyValuePair3.Value * keyValuePair.Key.paramList[0] * Time.realtimeSinceStartup;
					Vector3 vector4 = keyValuePair.Value.powerMdl * keyValuePair.Key.paramList[1];
					keyValuePair.Value.vertices[keyValuePair3.Key] = keyValuePair.Value.verticesOrg[keyValuePair3.Key] + Vector3.Scale(new Vector3(Mathf.Sin(vector3.x), Mathf.Sin(vector3.y), Mathf.Sin(vector3.z)), vector4);
				}
				keyValuePair.Value.mesh.vertices = keyValuePair.Value.vertices;
				foreach (CullingCheck cullingCheck in keyValuePair.Value.billBoard)
				{
					List<Camera> camera = cullingCheck.getCamera();
					if (camera != null && camera.Count > 0 && camera[0] != null)
					{
						cullingCheck.transform.LookAt(camera[0].transform);
					}
				}
			}
		}
	}

	// Token: 0x06000ABD RID: 2749 RVA: 0x0003E21C File Offset: 0x0003C41C
	private void OnDestroy()
	{
		this.Destory();
	}

	// Token: 0x06000ABE RID: 2750 RVA: 0x0003E224 File Offset: 0x0003C424
	public void Setting(Camera effectCamera)
	{
		this.Destory();
		this.targetEffectCamera = effectCamera;
		this.stageModelObj = Object.Instantiate<GameObject>(this.StageModelPrefab, base.transform);
		this.stageModelObj.SetLayerRecursively(LayerMask.NameToLayer("FieldStage"));
		foreach (Renderer renderer in this.stageModelObj.GetComponentsInChildren<Renderer>(true))
		{
			renderer.shadowCastingMode = ShadowCastingMode.Off;
			renderer.receiveShadows = true;
		}
		this.fbxPutObjectList = new List<KeyValuePair<GameObject, StagePresetCtrl.FbxPutInfo>>();
		foreach (object obj in this.stageModelObj.transform)
		{
			Transform transform = (Transform)obj;
			if (transform.name.StartsWith("put_"))
			{
				string[] array2 = transform.name.Split('_', StringSplitOptions.None);
				string type = array2[1];
				string group = array2[2];
				this.fbxPutObjectList.Add(new KeyValuePair<GameObject, StagePresetCtrl.FbxPutInfo>(transform.gameObject, this.FbxPutInfoList.Find((StagePresetCtrl.FbxPutInfo item) => item.putType == type && item.group == group)));
			}
		}
		this.MoveObjectSetting();
		if (this.SkyModelPrefab != null)
		{
			this.skyModelObj = Object.Instantiate<GameObject>(this.SkyModelPrefab, base.transform);
			this.skyModelObj.SetLayerRecursively(LayerMask.NameToLayer("FieldStage"));
			foreach (Renderer renderer2 in this.skyModelObj.GetComponentsInChildren<Renderer>(true))
			{
				renderer2.shadowCastingMode = ShadowCastingMode.Off;
				renderer2.receiveShadows = false;
			}
			this.SkyObjectOffst();
		}
		if (this.CameraEffectPrefab != null)
		{
			this.cameraEffectObj = Object.Instantiate<GameObject>(this.CameraEffectPrefab, this.targetEffectCamera.transform, false);
			this.cameraEffectObj.SetLayerRecursively(this.targetEffectCamera.gameObject.layer);
			foreach (Renderer renderer3 in this.cameraEffectObj.GetComponentsInChildren<Renderer>(true))
			{
				renderer3.shadowCastingMode = ShadowCastingMode.Off;
				renderer3.receiveShadows = false;
			}
		}
		this.lightByStage = base.transform.Find("StageLight").GetComponent<Light>();
		this.lightByStage.cullingMask = 1 << LayerMask.NameToLayer("FieldStage");
		this.lightByStage.cullingMask |= 1 << LayerMask.NameToLayer("FieldStageAlpha");
		this.lightByStage.cullingMask |= 1 << LayerMask.NameToLayer("FieldPlayerShadow");
		this.lightByStage.cullingMask |= 1 << LayerMask.NameToLayer("FieldEnemyShadow");
		Transform transform2 = base.transform.Find("CharaLight");
		if (transform2 != null && transform2.gameObject.activeSelf)
		{
			this.lightByPlayer = transform2.GetComponent<Light>();
		}
		else
		{
			this.lightByPlayer = this.lightByStage;
			this.lightByPlayer.cullingMask |= 1 << LayerMask.NameToLayer("FieldPlayer");
			this.lightByPlayer.cullingMask |= 1 << LayerMask.NameToLayer("FieldPlayerAlpha");
		}
		this.lightByPlayer.cullingMask |= 1 << LayerMask.NameToLayer("Camouflage");
		transform2 = base.transform.Find("EnemyLight");
		if (transform2 != null && transform2.gameObject.activeSelf)
		{
			this.lightByEnemy = transform2.GetComponent<Light>();
		}
		else
		{
			this.lightByEnemy = this.lightByStage;
			this.lightByEnemy.cullingMask |= 1 << LayerMask.NameToLayer("FieldEnemy");
			this.lightByEnemy.cullingMask |= 1 << LayerMask.NameToLayer("FieldEnemyAlpha");
		}
		this.StagePutObjManager.Data2Scene();
		this.StageAuthHideManager.Data2Scene();
		this.RenderSettingParam.Param2Scene();
	}

	// Token: 0x06000ABF RID: 2751 RVA: 0x0003E628 File Offset: 0x0003C828
	public void SettingForHomeAuth(Camera effectCamera, Transform parent)
	{
		this.Setting(effectCamera);
		base.transform.localEulerAngles = new Vector3(0f, 10f, 0f);
		base.transform.SetParent(parent);
		base.gameObject.SetActive(false);
	}

	// Token: 0x06000AC0 RID: 2752 RVA: 0x0003E668 File Offset: 0x0003C868
	public void SkyObjectOffst()
	{
		if (this.OffsetScale.x <= 0f)
		{
			this.OffsetScale.x = 1f;
		}
		if (this.OffsetScale.y <= 0f)
		{
			this.OffsetScale.y = 1f;
		}
		if (this.OffsetScale.z <= 0f)
		{
			this.OffsetScale.z = 1f;
		}
		if (this.skyModelObj != null)
		{
			this.skyModelObj.transform.localPosition = this.OffsetPosition;
			this.skyModelObj.transform.localEulerAngles = this.OffsetRotation;
			this.skyModelObj.transform.localScale = this.OffsetScale;
		}
	}

	// Token: 0x06000AC1 RID: 2753 RVA: 0x0003E72C File Offset: 0x0003C92C
	private void MoveObjectSetting()
	{
		this.animePutObject = new Dictionary<StagePresetCtrl.FbxPutInfo, StagePresetCtrl.AnimePutInfo>();
		if (this.fbxPutObjectList == null)
		{
			return;
		}
		foreach (KeyValuePair<GameObject, StagePresetCtrl.FbxPutInfo> keyValuePair in this.fbxPutObjectList)
		{
			GameObject key = keyValuePair.Key;
			StagePresetCtrl.FbxPutInfo fbxPutInfo = keyValuePair.Value;
			if (fbxPutInfo == null)
			{
				string[] array = key.name.Split('_', StringSplitOptions.None);
				string type = array[1];
				string group = array[2];
				fbxPutInfo = this.FbxPutInfoList.Find((StagePresetCtrl.FbxPutInfo item) => item.putType == type && item.group == group);
			}
			MeshFilter component = key.GetComponent<MeshFilter>();
			if (fbxPutInfo != null && component != null)
			{
				CullingCheck cullingCheck = key.gameObject.AddComponent<CullingCheck>();
				if (this.animePutObject.ContainsKey(fbxPutInfo))
				{
					component.sharedMesh = this.animePutObject[fbxPutInfo].mesh;
					this.animePutObject[fbxPutInfo].billBoard.Add(cullingCheck);
				}
				else
				{
					StagePresetCtrl.AnimePutInfo animePutInfo = new StagePresetCtrl.AnimePutInfo();
					animePutInfo.obj = Object.Instantiate<GameObject>(key, base.transform);
					animePutInfo.obj.gameObject.SetActive(false);
					animePutInfo.obj.GetComponent<MeshFilter>().mesh.vertices = component.sharedMesh.vertices;
					animePutInfo.mesh = animePutInfo.obj.GetComponent<MeshFilter>().sharedMesh;
					animePutInfo.verticesOrg = animePutInfo.mesh.vertices;
					animePutInfo.vertices = animePutInfo.mesh.vertices;
					float num = 99999f;
					float num2 = -num;
					foreach (Vector3 vector in animePutInfo.vertices)
					{
						if (num > vector.y)
						{
							num = vector.y;
						}
						if (num2 < vector.y)
						{
							num2 = vector.y;
						}
					}
					float num3 = (num2 - num) / 3f;
					num += num3;
					num2 -= num3;
					animePutInfo.movTop = new Dictionary<int, Vector3>();
					animePutInfo.movMdl = new Dictionary<int, Vector3>();
					for (int j = 0; j < animePutInfo.vertices.Length; j++)
					{
						Vector3 vector2 = new Vector3(Random.Range(0.9f, 1.1f), Random.Range(0.9f, 1.1f), Random.Range(0.9f, 1.1f));
						num3 = animePutInfo.vertices[j].y;
						if (num3 > num2)
						{
							animePutInfo.movTop.Add(j, vector2);
						}
						else if (num3 > num)
						{
							animePutInfo.movMdl.Add(j, vector2);
						}
					}
					animePutInfo.powerTop = new Vector3(Random.Range(0.9f, 1.1f), Random.Range(0.9f, 1.1f) * 0.25f, Random.Range(0.9f, 1.1f) * 0.5f);
					animePutInfo.powerMdl = new Vector3(Random.Range(0.9f, 1.1f), Random.Range(0.9f, 1.1f) * 0.25f, Random.Range(0.9f, 1.1f) * 0.5f) * 0.5f;
					animePutInfo.billBoard = new List<CullingCheck>();
					animePutInfo.billBoard.Add(cullingCheck);
					this.animePutObject.Add(fbxPutInfo, animePutInfo);
					component.sharedMesh = animePutInfo.mesh;
				}
			}
		}
	}

	// Token: 0x06000AC2 RID: 2754 RVA: 0x0003EAD8 File Offset: 0x0003CCD8
	public void Destory()
	{
		if (this.animePutObject != null)
		{
			foreach (StagePresetCtrl.AnimePutInfo animePutInfo in this.animePutObject.Values)
			{
				Object.Destroy(animePutInfo.obj);
			}
			this.animePutObject.Clear();
			this.animePutObject = null;
		}
		if (this.fbxPutObjectList != null)
		{
			this.fbxPutObjectList.Clear();
			this.fbxPutObjectList = null;
		}
		if (this.stageModelObj != null)
		{
			Object.Destroy(this.stageModelObj);
			this.stageModelObj = null;
		}
		if (this.skyModelObj != null)
		{
			Object.Destroy(this.skyModelObj);
			this.skyModelObj = null;
		}
		if (this.cameraEffectObj != null)
		{
			Object.Destroy(this.cameraEffectObj);
			this.cameraEffectObj = null;
		}
		this.StagePutObjManager.DestoryPutObj();
		this.StageAuthHideManager.DestoryPutObj();
	}

	// Token: 0x04000851 RID: 2129
	public static readonly string PackDataPath = "Stage/PackData/";

	// Token: 0x04000852 RID: 2130
	public GameObject StageModelPrefab;

	// Token: 0x04000853 RID: 2131
	public GameObject SkyModelPrefab;

	// Token: 0x04000854 RID: 2132
	public GameObject CameraEffectPrefab;

	// Token: 0x04000855 RID: 2133
	public Vector3 OffsetPosition;

	// Token: 0x04000856 RID: 2134
	public Vector3 OffsetRotation;

	// Token: 0x04000857 RID: 2135
	public Vector3 OffsetScale;

	// Token: 0x04000858 RID: 2136
	public RenderSettingParam RenderSettingParam;

	// Token: 0x04000859 RID: 2137
	public StagePutObjManager StagePutObjManager;

	// Token: 0x0400085A RID: 2138
	public StagePutObjManager StageAuthHideManager;

	// Token: 0x0400085B RID: 2139
	public List<StagePresetCtrl.FbxPutInfo> FbxPutInfoList = new List<StagePresetCtrl.FbxPutInfo>();

	// Token: 0x0400085C RID: 2140
	public Color shaderColor = Color.white;

	// Token: 0x0400085D RID: 2141
	private Camera targetEffectCamera;

	// Token: 0x04000865 RID: 2149
	private Dictionary<StagePresetCtrl.FbxPutInfo, StagePresetCtrl.AnimePutInfo> animePutObject;

	// Token: 0x020007F0 RID: 2032
	[Serializable]
	public class FbxPutInfo
	{
		// Token: 0x04003580 RID: 13696
		public string putType;

		// Token: 0x04003581 RID: 13697
		public string group;

		// Token: 0x04003582 RID: 13698
		public List<float> paramList = new List<float> { 0f, 0f, 0f };
	}

	// Token: 0x020007F1 RID: 2033
	public class AnimePutInfo
	{
		// Token: 0x04003583 RID: 13699
		public GameObject obj;

		// Token: 0x04003584 RID: 13700
		public Mesh mesh;

		// Token: 0x04003585 RID: 13701
		public Vector3[] verticesOrg;

		// Token: 0x04003586 RID: 13702
		public Vector3[] vertices;

		// Token: 0x04003587 RID: 13703
		public Dictionary<int, Vector3> movTop;

		// Token: 0x04003588 RID: 13704
		public Dictionary<int, Vector3> movMdl;

		// Token: 0x04003589 RID: 13705
		public Vector3 powerTop;

		// Token: 0x0400358A RID: 13706
		public Vector3 powerMdl;

		// Token: 0x0400358B RID: 13707
		public List<CullingCheck> billBoard;
	}
}
