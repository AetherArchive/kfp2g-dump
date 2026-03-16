using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class StagePresetCtrl : MonoBehaviour
{
	public GameObject stageModelObj { get; private set; }

	public GameObject skyModelObj { get; private set; }

	public GameObject cameraEffectObj { get; private set; }

	public Light lightByStage { get; private set; }

	public Light lightByPlayer { get; private set; }

	public Light lightByEnemy { get; private set; }

	public List<KeyValuePair<GameObject, StagePresetCtrl.FbxPutInfo>> fbxPutObjectList { get; private set; }

	private void Awake()
	{
		if (this.StageAuthHideManager == null)
		{
			this.StageAuthHideManager = new GameObject("AuthHideRoot", new Type[] { typeof(StagePutObjManager) }).GetComponent<StagePutObjManager>();
			this.StageAuthHideManager.transform.SetParent(base.transform, false);
			this.StageAuthHideManager.transform.SetAsFirstSibling();
		}
	}

	private void Start()
	{
	}

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

	private void OnDestroy()
	{
		this.Destory();
	}

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

	public void SettingForHomeAuth(Camera effectCamera, Transform parent)
	{
		this.Setting(effectCamera);
		base.transform.localEulerAngles = new Vector3(0f, 10f, 0f);
		base.transform.SetParent(parent);
		base.gameObject.SetActive(false);
	}

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

	public static readonly string PackDataPath = "Stage/PackData/";

	public GameObject StageModelPrefab;

	public GameObject SkyModelPrefab;

	public GameObject CameraEffectPrefab;

	public Vector3 OffsetPosition;

	public Vector3 OffsetRotation;

	public Vector3 OffsetScale;

	public RenderSettingParam RenderSettingParam;

	public StagePutObjManager StagePutObjManager;

	public StagePutObjManager StageAuthHideManager;

	public List<StagePresetCtrl.FbxPutInfo> FbxPutInfoList = new List<StagePresetCtrl.FbxPutInfo>();

	public Color shaderColor = Color.white;

	private Camera targetEffectCamera;

	private Dictionary<StagePresetCtrl.FbxPutInfo, StagePresetCtrl.AnimePutInfo> animePutObject;

	[Serializable]
	public class FbxPutInfo
	{
		public string putType;

		public string group;

		public List<float> paramList = new List<float> { 0f, 0f, 0f };
	}

	public class AnimePutInfo
	{
		public GameObject obj;

		public Mesh mesh;

		public Vector3[] verticesOrg;

		public Vector3[] vertices;

		public Dictionary<int, Vector3> movTop;

		public Dictionary<int, Vector3> movMdl;

		public Vector3 powerTop;

		public Vector3 powerMdl;

		public List<CullingCheck> billBoard;
	}
}
