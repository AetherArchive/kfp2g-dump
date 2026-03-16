using System;
using System.Collections.Generic;
using System.IO;
using SGNFW.Common;
using UnityEngine;

public class TestScenarioScene : MonoBehaviour
{
	private void MakeFileList()
	{
		string[] files = Directory.GetFiles("Assets/Parade/Prefabs/" + ScenarioDefine.LOAD_PATH + ScenarioDefine.DATA_FOLDER[(int)this.episodeType], "*.prefab");
		string text = ScenarioDefine.PREFAB_NAME_PREFIX[(int)this.episodeType];
		this.mFileList = new List<string>();
		string[] array = files;
		for (int i = 0; i < array.Length; i++)
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(array[i]);
			if (fileNameWithoutExtension.StartsWith(text))
			{
				this.mFileList.Add(fileNameWithoutExtension.Substring(text.Length));
			}
		}
	}

	private void Awake()
	{
		this.isInit = false;
	}

	private void Start()
	{
		Singleton<SoundManager>.Instance.Initialize();
		Singleton<DataManager>.Instance.Initialize();
		SceneManager.Initialize3DField();
		FacePackData.GetAllPackData(true);
		CanvasManager.SetBgEnable(false);
		Singleton<CanvasManager>.Instance.TouchEffectRemove();
		DataManager.DmUserInfo.UpdateUserOptionByDebug();
		this.isInit = true;
		this.episodeType = ScenarioDefine.EPISODE_TYPE.MAIN;
		this.MakeFileList();
		this.fileNumber = null;
		this.mRequest = false;
		this.scrollViewVec = Vector2.zero;
	}

	private void Update()
	{
		if (!this.isInit)
		{
			return;
		}
		if (this.mPrefabObj == null)
		{
			if (this.mRequest)
			{
				DataManager.DmUserInfo.optionData.ScenarioSpeed = this.textSpeed;
				DataManager.DmUserInfo.optionData.autoSpeed = this.autoSpeed;
				this.mPrefabObj = AssetManager.InstantiateAssetData("SceneScenario/ScenarioPrefab", null);
				this.mPrefabObj.GetComponent<ScenarioScene>().scenarioName = ScenarioDefine.DATA_FOLDER[(int)this.episodeType] + "/" + ScenarioDefine.PREFAB_NAME_PREFIX[(int)this.episodeType] + this.fileNumber;
				this.mPrefabObj.GetComponent<ScenarioScene>().questId = 0;
				this.mPrefabObj.GetComponent<ScenarioScene>().movieMode = (this.mMovie ? (this.mAuto ? 2 : 1) : 0);
				this.mPrefabObj.GetComponent<ScenarioScene>().skipScript = this.mSkip;
				this.mRequest = false;
			}
			this.mReset = false;
			return;
		}
		if (this.mPrefabObj.GetComponent<ScenarioScene>().IsFinishScenario() || this.mRequest || this.mReset)
		{
			Object.Destroy(this.mPrefabObj);
			this.mPrefabObj = null;
			this.mRequest = this.mReset;
			this.mReset = false;
		}
	}

	private void OnDestroy()
	{
	}

	private void OnGUI()
	{
		if (!this.isInit)
		{
			return;
		}
		GUIStyle guistyle = new GUIStyle();
		guistyle.normal = new GUIStyleState();
		guistyle.normal.background = Texture2D.blackTexture;
		guistyle.normal.textColor = Color.white;
		guistyle.fontSize = Screen.height / 40;
		guistyle.alignment = TextAnchor.MiddleCenter;
		if (!this.mRequest && !this.mReset)
		{
			if (this.mPrefabObj == null)
			{
				float num = 50f;
				foreach (object obj in Enum.GetValues(typeof(ScenarioDefine.EPISODE_TYPE)))
				{
					ScenarioDefine.EPISODE_TYPE episode_TYPE = (ScenarioDefine.EPISODE_TYPE)obj;
					guistyle.normal.background = ((this.episodeType == episode_TYPE) ? Texture2D.whiteTexture : Texture2D.blackTexture);
					guistyle.normal.textColor = ((this.episodeType == episode_TYPE) ? Color.black : Color.white);
					if (GUI.Button(new Rect(num, 50f, 100f, 30f), episode_TYPE.ToString(), guistyle))
					{
						this.episodeType = episode_TYPE;
						this.MakeFileList();
						this.scrollViewVec = Vector2.zero;
					}
					num += 110f;
				}
				this.scrollViewVec = GUI.BeginScrollView(new Rect(50f, 100f, 430f, 520f), this.scrollViewVec, new Rect(10f, 10f, 400f, (float)(20 * this.mFileList.Count + 20)));
				float num2 = 20f;
				foreach (string text in this.mFileList)
				{
					guistyle.normal.background = ((this.fileNumber == text) ? Texture2D.whiteTexture : Texture2D.blackTexture);
					guistyle.normal.textColor = ((this.fileNumber == text) ? Color.black : Color.white);
					if (GUI.Button(new Rect(0f, num2, 400f, 20f), text, guistyle))
					{
						this.fileNumber = text;
						this.mRequest = true;
					}
					num2 += 20f;
				}
				GUI.EndScrollView();
				guistyle.normal.background = (this.mMovie ? Texture2D.whiteTexture : Texture2D.blackTexture);
				guistyle.normal.textColor = (this.mMovie ? Color.black : Color.white);
				if (GUI.Button(new Rect(50f, 650f, 100f, 30f), "MOVIE", guistyle))
				{
					this.mMovie = !this.mMovie;
				}
				guistyle.normal.background = (this.mAuto ? Texture2D.whiteTexture : Texture2D.blackTexture);
				guistyle.normal.textColor = (this.mAuto ? Color.black : Color.white);
				if (GUI.Button(new Rect(160f, 650f, 100f, 30f), "AUTO", guistyle))
				{
					this.mAuto = !this.mAuto;
				}
				if (!int.TryParse(GUI.TextField(new Rect(270f, 650f, 80f, 30f), this.mSkip.ToString(), guistyle), out this.mSkip))
				{
					this.mSkip = 0;
					return;
				}
			}
			else
			{
				guistyle.normal.textColor = (this.mMovie ? Color.clear : Color.white);
				if (GUI.Button(new Rect((float)(Screen.width - 50), (float)(Screen.height - 50), 50f, 50f), this.mPrefabObj.GetComponent<ScenarioScene>().mScriptCnt.ToString(), guistyle))
				{
					this.mRequest = true;
				}
				guistyle.normal.textColor = Color.clear;
				if (GUI.Button(new Rect(0f, (float)(Screen.height - 50), 50f, 50f), "RST", guistyle))
				{
					this.mReset = true;
				}
			}
		}
	}

	[Range(0f, 2f)]
	public int textSpeed;

	[Range(0f, 2f)]
	public int autoSpeed;

	public ScenarioDefine.EPISODE_TYPE episodeType;

	public string fileNumber;

	private List<string> mFileList = new List<string>();

	private bool mRequest;

	private bool mReset;

	private bool mMovie;

	private bool mAuto;

	private int mSkip;

	private Vector2 scrollViewVec = Vector2.zero;

	private GameObject mPrefabObj;

	private bool isInit;
}
