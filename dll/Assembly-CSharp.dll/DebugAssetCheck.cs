using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SGNFW.Ab;
using SGNFW.Common;
using SGNFW.Mst;
using UnityEngine;
using UnityEngine.UI;

public class DebugAssetCheck
{
	public void CreateGui(PguiButtonCtrl baseBtn)
	{
		GameObject gameObject = new GameObject();
		gameObject.AddComponent<RectTransform>();
		gameObject.transform.SetParent(baseBtn.transform.parent, false);
		gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(1280f, 720f);
		gameObject.AddComponent<Image>().color = new Color(0.2f, 0.2f, 0.2f, 0.9f);
		gameObject.name = "ScenarioAssetCheck";
		this.DuplicateButton(baseBtn, gameObject, "mainScenarioButton", "メイン <color=#f00>ON</color>", new Vector3(200f, 240f, 0f)).AddOnClickListener(delegate(PguiButtonCtrl btn)
		{
			this.SwitchEpisode(btn, ScenarioDefine.EPISODE_TYPE.MAIN);
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this.DuplicateButton(baseBtn, gameObject, "friendsScenarioButton", "フレンズ <color=#f00>ON</color>", new Vector3(200f, 140f, 0f)).AddOnClickListener(delegate(PguiButtonCtrl btn)
		{
			this.SwitchEpisode(btn, ScenarioDefine.EPISODE_TYPE.CHAR);
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this.DuplicateButton(baseBtn, gameObject, "eventScenarioButton", "イベント <color=#f00>ON</color>", new Vector3(200f, 40f, 0f)).AddOnClickListener(delegate(PguiButtonCtrl btn)
		{
			this.SwitchEpisode(btn, ScenarioDefine.EPISODE_TYPE.EVENT);
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this.DuplicateButton(baseBtn, gameObject, "anotherScenarioButton", "その他 <color=#f00>ON</color>", new Vector3(200f, -60f, 0f)).AddOnClickListener(delegate(PguiButtonCtrl btn)
		{
			this.SwitchEpisode(btn, ScenarioDefine.EPISODE_TYPE.ANOTHER);
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this.DuplicateButton(baseBtn, gameObject, "localFileButton", "ローカルファイルから\nパスを取得", new Vector3(450f, 240f, 0f)).AddOnClickListener(new PguiButtonCtrl.OnClick(this.ExecuteCheckLocalFile), PguiButtonCtrl.SoundType.DEFAULT);
		this.DuplicateButton(baseBtn, gameObject, "assetBundleButton", "AssetListから\nパスを取得", new Vector3(450f, 140f, 0f)).AddOnClickListener(new PguiButtonCtrl.OnClick(this.ExecuteCheckAssetBundleList), PguiButtonCtrl.SoundType.DEFAULT);
		this.DuplicateButton(baseBtn, gameObject, "chaeckQuestOneButton", "QuestOneから\nパスを取得", new Vector3(450f, 40f, 0f)).AddOnClickListener(new PguiButtonCtrl.OnClick(this.ExecuteCheckMasterQuestOne), PguiButtonCtrl.SoundType.DEFAULT);
		this.DuplicateButton(baseBtn, gameObject, "enemyModelCheckButton", "AssetListから\nエネミーのパスを取得", new Vector3(-480f, 240f, 0f)).AddOnClickListener(new PguiButtonCtrl.OnClick(this.ExecuteEnemyAssetCheck), PguiButtonCtrl.SoundType.DEFAULT);
		this.DuplicateButton(baseBtn, gameObject, "friendsModelCheckButton", "AssetListから\nフレンズのパスを取得", new Vector3(-480f, 140f, 0f)).AddOnClickListener(new PguiButtonCtrl.OnClick(this.ExecuteCharaModelAssetCheck), PguiButtonCtrl.SoundType.DEFAULT);
		this.DuplicateButton(baseBtn, gameObject, "QuestEnemyModelCheckButton", "QuestOneから\nエネミーのパスを取得", new Vector3(-260f, 240f, 0f)).AddOnClickListener(new PguiButtonCtrl.OnClick(this.ExecuteMasterEnemyAssetCheck), PguiButtonCtrl.SoundType.DEFAULT);
		this.DuplicateButton(baseBtn, gameObject, "charaClothesModelCheckButton", "CharaClothesから\nフレンズのパスを取得", new Vector3(-260f, 140f, 0f)).AddOnClickListener(new PguiButtonCtrl.OnClick(this.ExecuteMasterCharaClothesAssetCheck), PguiButtonCtrl.SoundType.DEFAULT);
		this.DuplicateButton(baseBtn, gameObject, "CVCheckButton", "AssetListから\nCVのパスを取得", new Vector3(-480f, 40f, 0f)).AddOnClickListener(new PguiButtonCtrl.OnClick(this.ExecuteCVCheck), PguiButtonCtrl.SoundType.DEFAULT);
		this.DuplicateButton(baseBtn, gameObject, "closeButton", "再起動", new Vector3(-450f, -240f, 0f)).AddOnClickListener(new PguiButtonCtrl.OnClick(this.CloseAssetBundleCheck), PguiButtonCtrl.SoundType.DEFAULT);
		this.CopyBtn = this.DuplicateButton(baseBtn, gameObject, "copyButton", "結果をコピーする", new Vector3(450f, -240f, 0f));
		this.CopyBtn.AddOnClickListener(new PguiButtonCtrl.OnClick(this.CopyCheckLog), PguiButtonCtrl.SoundType.DEFAULT);
		this.CopyBtn.SetActEnable(false, false, false);
		PguiTextCtrl component = baseBtn.transform.Find("BaseImage/Txt_Name").GetComponent<PguiTextCtrl>();
		this.ProgressText = this.DuplicateText(component, gameObject, "progressText", "0 / 0", new Vector3(450f, -180f, 0f), new Vector3(0.8f, 0.8f, 0.8f));
		this.ErrorCountText = this.DuplicateText(component, gameObject, "errorCountText", "0", new Vector3(450f, -300f, 0f), new Vector3(0.8f, 0.8f, 0.8f));
		this.DuplicateText(component, gameObject, "titleText", "アセットチェック", new Vector3(0f, -280f, 0f), new Vector3(1f, 1f, 1f));
		this.AssetBundleInfoText = this.DuplicateText(component, gameObject, "assetBundleInfoText", "AssetBundle<color=#f00>有効</color>", new Vector3(-480f, 320f, 0f), new Vector3(0.8f, 0.8f, 0.8f));
		this.DuplicateText(component, gameObject, "assetCheckInfoText", "マスターからチェック", new Vector3(-260f, 320f, 0f), new Vector3(0.8f, 0.8f, 0.8f));
		this.DuplicateText(component, gameObject, "ScenarioCheckText", "シナリオチェック\n\u3000検索対象\u3000\u3000\u3000\u3000\u3000\u3000\u3000ファイルパス", new Vector3(325f, 320f, 0f), new Vector3(0.8f, 0.8f, 0.8f));
		this.epsodeTypeEnableMap = new Dictionary<ScenarioDefine.EPISODE_TYPE, bool>();
		foreach (object obj in Enum.GetValues(typeof(ScenarioDefine.EPISODE_TYPE)))
		{
			ScenarioDefine.EPISODE_TYPE episode_TYPE = (ScenarioDefine.EPISODE_TYPE)obj;
			this.epsodeTypeEnableMap.Add(episode_TYPE, true);
		}
	}

	private void CloseAssetBundleCheck(PguiButtonCtrl btn)
	{
	}

	public PguiButtonCtrl DuplicateButton(PguiButtonCtrl baseBtn, GameObject parentObj, string btnName, string btnText, Vector3 pos)
	{
		PguiButtonCtrl component = Object.Instantiate<GameObject>(baseBtn.gameObject, parentObj.transform).GetComponent<PguiButtonCtrl>();
		component.gameObject.name = btnName;
		component.transform.localPosition = pos;
		component.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
		PguiTextCtrl component2 = component.transform.Find("BaseImage/Txt_Name").GetComponent<PguiTextCtrl>();
		component2.m_Text.supportRichText = true;
		component2.text = btnText;
		return component;
	}

	public PguiTextCtrl DuplicateText(PguiTextCtrl baseText, GameObject parentObj, string textName, string textmsg, Vector3 pos, Vector3 scale)
	{
		PguiTextCtrl component = Object.Instantiate<GameObject>(baseText.gameObject, parentObj.transform).GetComponent<PguiTextCtrl>();
		component.gameObject.name = textName;
		component.transform.localPosition = pos;
		component.transform.localScale = scale;
		component.m_Text.supportRichText = true;
		component.text = textmsg;
		return component;
	}

	private void SwitchEpisode(PguiButtonCtrl btn, ScenarioDefine.EPISODE_TYPE episodeType)
	{
		if (this.NowCheking)
		{
			return;
		}
		this.epsodeTypeEnableMap[episodeType] = !this.epsodeTypeEnableMap[episodeType];
		PguiTextCtrl component = btn.transform.Find("BaseImage/Txt_Name").GetComponent<PguiTextCtrl>();
		switch (episodeType)
		{
		case ScenarioDefine.EPISODE_TYPE.MAIN:
			component.text = "メイン ";
			break;
		case ScenarioDefine.EPISODE_TYPE.CHAR:
			component.text = "フレンズ ";
			break;
		case ScenarioDefine.EPISODE_TYPE.EVENT:
			component.text = "イベント ";
			break;
		case ScenarioDefine.EPISODE_TYPE.ANOTHER:
			component.text = "その他 ";
			break;
		}
		PguiTextCtrl pguiTextCtrl = component;
		pguiTextCtrl.text += (this.epsodeTypeEnableMap[episodeType] ? "<color=#f00>ON</color>" : "<color=#000>OFF</color>");
	}

	private void ExecuteCheckMasterQuestOne(PguiButtonCtrl btn)
	{
		if (this.NowCheking)
		{
			return;
		}
		this.CheckTarget = DebugAssetCheck.CheckTargetType.QuestOne;
		Singleton<SceneManager>.Instance.StartCoroutine(this.ScenarioAssetCheck());
	}

	private void ExecuteCheckAssetBundleList(PguiButtonCtrl btn)
	{
		if (this.NowCheking)
		{
			return;
		}
		this.CheckTarget = DebugAssetCheck.CheckTargetType.AssetBundle;
		Singleton<SceneManager>.Instance.StartCoroutine(this.ScenarioAssetCheck());
	}

	private void ExecuteCheckLocalFile(PguiButtonCtrl btn)
	{
		if (this.NowCheking)
		{
			return;
		}
		this.CheckTarget = DebugAssetCheck.CheckTargetType.LocalFile;
		Singleton<SceneManager>.Instance.StartCoroutine(this.ScenarioAssetCheck());
	}

	private void ExecuteCharaModelAssetCheck(PguiButtonCtrl btn)
	{
		if (this.NowCheking)
		{
			return;
		}
		Singleton<SceneManager>.Instance.StartCoroutine(this.ModelCheck(DebugAssetCheck.ModelCheckTarget.AssetListChara));
	}

	private void ExecuteEnemyAssetCheck(PguiButtonCtrl btn)
	{
		if (this.NowCheking)
		{
			return;
		}
		Singleton<SceneManager>.Instance.StartCoroutine(this.ModelCheck(DebugAssetCheck.ModelCheckTarget.AssetListEnemy));
	}

	private void ExecuteMasterEnemyAssetCheck(PguiButtonCtrl btn)
	{
		if (this.NowCheking)
		{
			return;
		}
		Singleton<SceneManager>.Instance.StartCoroutine(this.ModelCheck(DebugAssetCheck.ModelCheckTarget.QuestOneEnemy));
	}

	private void ExecuteMasterCharaClothesAssetCheck(PguiButtonCtrl btn)
	{
		if (this.NowCheking)
		{
			return;
		}
		Singleton<SceneManager>.Instance.StartCoroutine(this.ModelCheck(DebugAssetCheck.ModelCheckTarget.CharaClothes));
	}

	private void ExecuteCVCheck(PguiButtonCtrl btn)
	{
		if (this.NowCheking)
		{
			return;
		}
		Singleton<SceneManager>.Instance.StartCoroutine(this.CVCheck());
	}

	public IEnumerator ScenarioAssetCheck()
	{
		this.CopyBtn.SetActEnable(false, false, false);
		this.ProgressText.text = "0 / 0";
		this.NowCheking = true;
		this.LoadedCharaMap = new Dictionary<string, string>();
		this.ResetCheckLog();
		this.AddCheckLog("TimeManager.Now:" + TimeManager.Now.ToString(), DebugAssetCheck.CheckLogStatus.Info);
		DataManagerQuest dmQuest = DataManager.DmQuest;
		bool flag;
		if (dmQuest == null)
		{
			flag = null != null;
		}
		else
		{
			QuestStaticData questStaticData = dmQuest.QuestStaticData;
			flag = ((questStaticData != null) ? questStaticData.oneDataList : null) != null;
		}
		if (!flag)
		{
			IEnumerator initAction = DataInitializeResolver.InitializeActionAfterTitle(false);
			while (initAction.MoveNext())
			{
				yield return null;
			}
			initAction = null;
		}
		List<string> scenarioIdPathList = new List<string>();
		switch (this.CheckTarget)
		{
		case DebugAssetCheck.CheckTargetType.QuestOne:
		{
			using (List<QuestStaticQuestOne>.Enumerator enumerator = DataManager.DmQuest.QuestStaticData.oneDataList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					QuestStaticQuestOne questStaticQuestOne = enumerator.Current;
					if (!string.IsNullOrEmpty(questStaticQuestOne.scenarioBeforeId))
					{
						scenarioIdPathList.Add(ScenarioDefine.LOAD_PATH + questStaticQuestOne.scenarioBeforeId);
					}
					if (!string.IsNullOrEmpty(questStaticQuestOne.scenarioAfterId))
					{
						scenarioIdPathList.Add(ScenarioDefine.LOAD_PATH + questStaticQuestOne.scenarioAfterId);
					}
					using (IEnumerator enumerator2 = Enum.GetValues(typeof(ScenarioDefine.EPISODE_TYPE)).GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							ScenarioDefine.EPISODE_TYPE episodeType = (ScenarioDefine.EPISODE_TYPE)enumerator2.Current;
							if (this.epsodeTypeEnableMap.ContainsKey(episodeType) && !this.epsodeTypeEnableMap[episodeType])
							{
								scenarioIdPathList.RemoveAll((string x) => x.Contains(ScenarioDefine.PREFAB_NAME_PREFIX[(int)episodeType]));
							}
						}
					}
				}
				goto IL_04DD;
			}
			break;
		}
		case DebugAssetCheck.CheckTargetType.AssetBundle:
			break;
		case DebugAssetCheck.CheckTargetType.LocalFile:
			goto IL_03A5;
		default:
			goto IL_04DD;
		}
		List<string> list = new List<string>();
		foreach (Data data in Manager.DataList)
		{
			list.Add(data.name);
		}
		using (IEnumerator enumerator2 = Enum.GetValues(typeof(ScenarioDefine.EPISODE_TYPE)).GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				object obj = enumerator2.Current;
				ScenarioDefine.EPISODE_TYPE episode_TYPE = (ScenarioDefine.EPISODE_TYPE)obj;
				if (!this.epsodeTypeEnableMap.ContainsKey(episode_TYPE) || this.epsodeTypeEnableMap[episode_TYPE])
				{
					string text = ScenarioDefine.PREFAB_NAME_PREFIX[(int)episode_TYPE];
					string text2 = text.ToLower();
					int length = text.Length;
					foreach (string text3 in list)
					{
						string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text3);
						if (fileNameWithoutExtension.StartsWith(text) || fileNameWithoutExtension.StartsWith(text2))
						{
							string text4 = string.Concat(new string[]
							{
								ScenarioDefine.LOAD_PATH,
								ScenarioDefine.DATA_FOLDER[(int)episode_TYPE],
								"/",
								ScenarioDefine.PREFAB_NAME_PREFIX[(int)episode_TYPE],
								fileNameWithoutExtension.Substring(length)
							});
							scenarioIdPathList.Add(text4);
						}
					}
				}
			}
			goto IL_04DD;
		}
		IL_03A5:
		scenarioIdPathList = new List<string>();
		foreach (object obj2 in Enum.GetValues(typeof(ScenarioDefine.EPISODE_TYPE)))
		{
			ScenarioDefine.EPISODE_TYPE episode_TYPE2 = (ScenarioDefine.EPISODE_TYPE)obj2;
			if (!this.epsodeTypeEnableMap.ContainsKey(episode_TYPE2) || this.epsodeTypeEnableMap[episode_TYPE2])
			{
				string[] files = Directory.GetFiles("Assets/Parade/Prefabs/" + ScenarioDefine.LOAD_PATH + ScenarioDefine.DATA_FOLDER[(int)episode_TYPE2], "*.prefab");
				string text5 = ScenarioDefine.PREFAB_NAME_PREFIX[(int)episode_TYPE2];
				string text6 = text5.ToLower();
				int length2 = text5.Length;
				string[] array = files;
				for (int i = 0; i < array.Length; i++)
				{
					string fileNameWithoutExtension2 = Path.GetFileNameWithoutExtension(array[i]);
					if (fileNameWithoutExtension2.StartsWith(text5) || fileNameWithoutExtension2.StartsWith(text6))
					{
						string text7 = string.Concat(new string[]
						{
							ScenarioDefine.LOAD_PATH,
							ScenarioDefine.DATA_FOLDER[(int)episode_TYPE2],
							"/",
							ScenarioDefine.PREFAB_NAME_PREFIX[(int)episode_TYPE2],
							fileNameWithoutExtension2.Substring(length2)
						});
						scenarioIdPathList.Add(text7);
					}
				}
			}
		}
		IL_04DD:
		this.AddCheckLog("AssetCheck() CheckTarget:" + this.CheckTarget.ToString(), DebugAssetCheck.CheckLogStatus.Info);
		foreach (object obj3 in Enum.GetValues(typeof(ScenarioDefine.EPISODE_TYPE)))
		{
			ScenarioDefine.EPISODE_TYPE episode_TYPE3 = (ScenarioDefine.EPISODE_TYPE)obj3;
			if (this.epsodeTypeEnableMap.ContainsKey(episode_TYPE3))
			{
				string text8 = string.Empty;
				switch (episode_TYPE3)
				{
				case ScenarioDefine.EPISODE_TYPE.MAIN:
					text8 = "メイン\u3000:" + this.epsodeTypeEnableMap[episode_TYPE3].ToString();
					break;
				case ScenarioDefine.EPISODE_TYPE.CHAR:
					text8 = "フレンズ:" + this.epsodeTypeEnableMap[episode_TYPE3].ToString();
					break;
				case ScenarioDefine.EPISODE_TYPE.EVENT:
					text8 = "イベント:" + this.epsodeTypeEnableMap[episode_TYPE3].ToString();
					break;
				case ScenarioDefine.EPISODE_TYPE.ANOTHER:
					text8 = "その他\u3000:" + this.epsodeTypeEnableMap[episode_TYPE3].ToString();
					break;
				}
				this.AddCheckLog(text8, DebugAssetCheck.CheckLogStatus.Info);
			}
		}
		this.AddCheckLog("AssetCheck() CheckSampleNum:" + scenarioIdPathList.Count.ToString(), DebugAssetCheck.CheckLogStatus.Info);
		AssetManager.LoadAssetData(ScenarioScene.CHARA_OFFSET_PATH, AssetManager.OWNER.Scenario, 0, null);
		while (!AssetManager.IsLoadFinishAssetData(ScenarioScene.CHARA_OFFSET_PATH))
		{
			yield return null;
		}
		long lastUpdate = DateTime.Now.Ticks;
		this.progressCount = 0;
		this.errorCount = 0;
		foreach (string scenarioIdPath in scenarioIdPathList)
		{
			AssetManager.LoadAssetData(scenarioIdPath, AssetManager.OWNER.Scenario, 0, null);
			DateTime start = DateTime.Now;
			int waitTime = 0;
			switch (this.CheckTarget)
			{
			case DebugAssetCheck.CheckTargetType.QuestOne:
			case DebugAssetCheck.CheckTargetType.LocalFile:
				waitTime = 1;
				break;
			case DebugAssetCheck.CheckTargetType.AssetBundle:
				waitTime = 5;
				break;
			}
			while (!AssetManager.IsLoadFinishAssetData(scenarioIdPath) && waitTime >= (DateTime.Now - start).Seconds)
			{
				yield return null;
			}
			this.ScenarioCheck(scenarioIdPath);
			this.progressCount++;
			this.ProgressText.text = string.Format("{0} / {1}", this.progressCount, scenarioIdPathList.Count);
			this.ErrorCountText.text = string.Format("errorCount = {0}", this.errorCount);
			if (333333L < DateTime.Now.Ticks - lastUpdate)
			{
				lastUpdate = DateTime.Now.Ticks;
				yield return null;
			}
			scenarioIdPath = null;
		}
		List<string>.Enumerator enumerator5 = default(List<string>.Enumerator);
		AssetManager.UnloadAssetData(ScenarioScene.CHARA_OFFSET_PATH, AssetManager.OWNER.Scenario);
		Resources.UnloadUnusedAssets();
		yield return null;
		this.NowCheking = false;
		this.CopyBtn.SetActEnable(true, false, false);
		yield break;
		yield break;
	}

	private void ScenarioCheck(string scenarioIdPath)
	{
		GameObject gameObject = AssetManager.InstantiateAssetData(scenarioIdPath, null);
		if (gameObject == null)
		{
			this.AddCheckLog("ScenarioData not found error!! scenario : " + scenarioIdPath, DebugAssetCheck.CheckLogStatus.Error);
			this.errorCount++;
			return;
		}
		this.AddCheckLog("Asset is Loaded. name:" + scenarioIdPath, DebugAssetCheck.CheckLogStatus.Info);
		ScenarioScriptData component = gameObject.GetComponent<ScenarioScriptData>();
		gameObject.name = scenarioIdPath;
		AssetManager.UnloadAssetData(scenarioIdPath, AssetManager.OWNER.Scenario);
		int num = 0;
		int num2 = 0;
		using (List<ScenarioScriptData.CharacterData>.Enumerator enumerator = component.charaDatas.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ScenarioScriptData.CharacterData data = enumerator.Current;
				if (!this.LoadedCharaMap.ContainsKey(data.model))
				{
					ScenarioScene.friend friend = new ScenarioScene.friend();
					if (!string.IsNullOrEmpty(data.model))
					{
						GameObject gameObject2 = new GameObject();
						int num3 = int.Parse(data.model.Substring(3, 4));
						gameObject2.name = "Model" + num.ToString("D2") + "_" + num3.ToString();
						friend.obj = gameObject2;
						friend.charaModelHandle = gameObject2.AddComponent<CharaModelHandle>();
						List<Data> list = Manager.DataList.FindAll((Data x) => x.name.Contains(data.model) && x.name.StartsWith("ch_"));
						if (list.Count == 0)
						{
							this.errorCount++;
							this.AddCheckLog("enemyPrefab is not Registered in AssetList. data.model:" + data.model, DebugAssetCheck.CheckLogStatus.Error);
						}
						CharaModelHandle.InitializeParam initializeParam = new CharaModelHandle.InitializeParam(data.model, false, false)
						{
							isDisableVoice = true
						};
						friend.charaModelHandle.Initialize(initializeParam);
						friend.mEff = new GameObject("eff");
						friend.mEff.transform.SetParent(friend.obj.transform, false);
						num++;
					}
					friend.ID = num2;
					friend.mName = data.name;
					friend.faceId = "INVALID";
					friend.idleFaceId = "INVALID";
					friend.standby = CharaMotionDefine.ActKey.SCENARIO_STAND_BY;
					friend.standbyCF = 0.5f;
					this.LoadedCharaMap.Add(data.model, data.name);
					Object.Destroy(friend.obj);
					num2++;
				}
			}
		}
		List<IEnumerator> list2 = new List<IEnumerator>();
		foreach (string text in component.cueSheetList)
		{
			list2.Add(SoundManager.LoadCueSheetWithDownload(text));
		}
		List<string> list3 = new List<string>();
		foreach (string text2 in component.effectSheetList)
		{
			list3.Add(text2);
			EffectManager.ReqLoadEffect(text2, AssetManager.OWNER.Scenario, 0, null);
		}
		Object.Destroy(gameObject);
	}

	private IEnumerator ModelCheck(DebugAssetCheck.ModelCheckTarget checkTarget)
	{
		this.CopyBtn.SetActEnable(false, false, false);
		this.ProgressText.text = "0 / 0";
		this.NowCheking = true;
		this.ResetCheckLog();
		this.progressCount = 0;
		this.errorCount = 0;
		DataManagerQuest dmQuest = DataManager.DmQuest;
		bool flag;
		if (dmQuest == null)
		{
			flag = null != null;
		}
		else
		{
			QuestStaticData questStaticData = dmQuest.QuestStaticData;
			flag = ((questStaticData != null) ? questStaticData.oneDataList : null) != null;
		}
		if (!flag)
		{
			IEnumerator initAction = DataInitializeResolver.InitializeActionAfterTitle(false);
			while (initAction.MoveNext())
			{
				yield return null;
			}
			initAction = null;
		}
		this.AddCheckLog("Model Check Target:" + checkTarget.ToString(), DebugAssetCheck.CheckLogStatus.Info);
		this.questEnemyPrefabList = new Dictionary<int, List<string>>();
		List<string> targetPrefabList = new List<string>();
		switch (checkTarget)
		{
		case DebugAssetCheck.ModelCheckTarget.AssetListChara:
		{
			using (List<Data>.Enumerator enumerator = Manager.DataList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Data data = enumerator.Current;
					if (data.name.StartsWith("ch_"))
					{
						string[] array = data.name.Split('_', StringSplitOptions.None);
						if (4 == array[1].Length)
						{
							targetPrefabList.Add(CharaModelHandle.CHARA_MODEL_PATH + data.name.Replace(".prefab", ""));
						}
					}
				}
				goto IL_0746;
			}
			break;
		}
		case DebugAssetCheck.ModelCheckTarget.AssetListEnemy:
			break;
		case DebugAssetCheck.ModelCheckTarget.QuestOneEnemy:
			goto IL_024A;
		case DebugAssetCheck.ModelCheckTarget.CharaClothes:
			goto IL_0438;
		default:
			goto IL_0746;
		}
		using (List<Data>.Enumerator enumerator = Manager.DataList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Data data2 = enumerator.Current;
				if (data2.name.StartsWith("ch_"))
				{
					string[] array2 = data2.name.Split('_', StringSplitOptions.None);
					if (5 == array2[1].Length)
					{
						targetPrefabList.Add(CharaModelHandle.CHARA_MODEL_PATH + data2.name.Replace(".prefab", ""));
					}
				}
			}
			goto IL_0746;
		}
		IL_024A:
		List<MstQuestEnemyData> mst = Singleton<MstManager>.Instance.GetMst<List<MstQuestEnemyData>>(MstType.QUEST_ENEMY_DATA);
		List<int> QuestEnemyCharaIdList = new List<int>();
		long update = DateTime.Now.Ticks;
		foreach (MstQuestEnemyData mstQuestEnemyData in mst)
		{
			if (ItemDef.Id2Kind(mstQuestEnemyData.enemyCharaId) == ItemDef.Kind.ENEMY_CHARA && !QuestEnemyCharaIdList.Contains(mstQuestEnemyData.enemyCharaId))
			{
				QuestEnemyCharaIdList.Add(mstQuestEnemyData.enemyCharaId);
			}
			if (333333L < DateTime.Now.Ticks - update)
			{
				update = DateTime.Now.Ticks;
				yield return null;
			}
		}
		List<MstQuestEnemyData>.Enumerator enumerator2 = default(List<MstQuestEnemyData>.Enumerator);
		using (List<int>.Enumerator enumerator3 = QuestEnemyCharaIdList.GetEnumerator())
		{
			while (enumerator3.MoveNext())
			{
				int enemyCharaId = enumerator3.Current;
				if (!this.questEnemyPrefabList.ContainsKey(enemyCharaId))
				{
					List<string> list = Manager.DataList.FindAll((Data x) => x.name.Contains(enemyCharaId.ToString()) && x.name.StartsWith("ch_")).ConvertAll<string>((Data x) => CharaModelHandle.CHARA_MODEL_PATH + x.name.Replace(".prefab", ""));
					if (list.Count == 0)
					{
						this.AddCheckLog("enemyPrefab is not Registered in AssetList. enemyId:" + enemyCharaId.ToString(), DebugAssetCheck.CheckLogStatus.Error);
						this.errorCount++;
					}
					else
					{
						this.questEnemyPrefabList.Add(enemyCharaId, list);
						targetPrefabList.AddRange(list);
					}
				}
			}
			goto IL_0746;
		}
		IL_0438:
		List<string> ImageID2suffix = new List<string>
		{
			"a", "b", "c", "d", "e", "f", "g", "h", "i", "j",
			"k", "l", "m", "n", "o", "p", "q", "r", "s", "t",
			"u", "v", "w", "x", "y", "z"
		};
		List<MstCharaClothesData> mst2 = Singleton<MstManager>.Instance.GetMst<List<MstCharaClothesData>>(MstType.CHARA_CLOTHES_DATA);
		string text = string.Empty;
		update = DateTime.Now.Ticks;
		foreach (MstCharaClothesData mstCharaClothesData in mst2)
		{
			text = string.Empty;
			while (mstCharaClothesData.imgId >= 0)
			{
				text = ImageID2suffix[mstCharaClothesData.imgId % ImageID2suffix.Count] + text;
				mstCharaClothesData.imgId = mstCharaClothesData.imgId / ImageID2suffix.Count - 1;
			}
			string chModelName = "ch_" + mstCharaClothesData.clothesPresetId.ToString("0000") + "_" + text;
			List<string> list2 = Manager.DataList.FindAll((Data x) => x.name.StartsWith(chModelName)).ConvertAll<string>((Data x) => CharaModelHandle.CHARA_MODEL_PATH + x.name.Replace(".prefab", ""));
			if (list2.Count == 0)
			{
				this.AddCheckLog("charaPrefab is not Registered in AssetList. enemyId:" + chModelName, DebugAssetCheck.CheckLogStatus.Error);
				this.errorCount++;
			}
			else
			{
				targetPrefabList.AddRange(list2);
			}
			if (333333L < DateTime.Now.Ticks - update)
			{
				update = DateTime.Now.Ticks;
				this.ProgressText.text = string.Format("0 / {0}", targetPrefabList.Count);
				yield return null;
			}
		}
		List<MstCharaClothesData>.Enumerator enumerator4 = default(List<MstCharaClothesData>.Enumerator);
		IL_0746:
		this.AddCheckLog("target count:" + targetPrefabList.Count.ToString(), DebugAssetCheck.CheckLogStatus.Info);
		long lastUpdate = DateTime.Now.Ticks;
		foreach (string chPrefab in targetPrefabList)
		{
			AssetManager.LoadAssetData(chPrefab, AssetManager.OWNER.CharaModel, 0, null);
			DateTime start = DateTime.Now;
			int waitTime = 5;
			while (!AssetManager.IsLoadFinishAssetData(chPrefab))
			{
				yield return null;
				if (waitTime < (DateTime.Now - start).Seconds)
				{
					break;
				}
			}
			GameObject gameObject = AssetManager.InstantiateAssetData(chPrefab, null);
			if (null == gameObject)
			{
				this.AddCheckLog("prefab not found error!! name:" + chPrefab, DebugAssetCheck.CheckLogStatus.Error);
				this.errorCount++;
			}
			else
			{
				this.AddCheckLog("Asset is Loaded. name:" + chPrefab, DebugAssetCheck.CheckLogStatus.Info);
				Object.Destroy(gameObject);
			}
			this.progressCount++;
			this.ProgressText.text = string.Format("{0} / {1}", this.progressCount, targetPrefabList.Count);
			this.ErrorCountText.text = string.Format("errorCount = {0}", this.errorCount);
			if (333333L < DateTime.Now.Ticks - lastUpdate)
			{
				lastUpdate = DateTime.Now.Ticks;
				yield return null;
			}
			chPrefab = null;
		}
		List<string>.Enumerator enumerator5 = default(List<string>.Enumerator);
		Resources.UnloadUnusedAssets();
		yield return null;
		this.NowCheking = false;
		this.CopyBtn.SetActEnable(true, false, false);
		yield break;
		yield break;
	}

	private IEnumerator CVCheck()
	{
		this.CopyBtn.SetActEnable(false, false, false);
		this.ProgressText.text = "0 / 0";
		this.NowCheking = true;
		this.ResetCheckLog();
		DataManagerQuest dmQuest = DataManager.DmQuest;
		bool flag;
		if (dmQuest == null)
		{
			flag = null != null;
		}
		else
		{
			QuestStaticData questStaticData = dmQuest.QuestStaticData;
			flag = ((questStaticData != null) ? questStaticData.oneDataList : null) != null;
		}
		if (!flag)
		{
			IEnumerator initAction = DataInitializeResolver.InitializeActionAfterTitle(false);
			while (initAction.MoveNext())
			{
				yield return null;
			}
			initAction = null;
		}
		List<string> cvFileList = new List<string>();
		foreach (Data data in Manager.DataList)
		{
			if (data.name.StartsWith("cv_") && data.name.EndsWith(".acb"))
			{
				cvFileList.Add(data.name.Replace(".acb", ""));
			}
		}
		this.AddCheckLog("target count:" + cvFileList.Count.ToString(), DebugAssetCheck.CheckLogStatus.Info);
		this.progressCount = 0;
		this.errorCount = 0;
		foreach (string cvFile in cvFileList)
		{
			IEnumerator initAction = SoundManager.LoadCueSheetWithDownload(cvFile);
			DateTime start = DateTime.Now;
			int waitTime = 5;
			bool isFailed = false;
			while (initAction.MoveNext())
			{
				yield return null;
				if (waitTime < (DateTime.Now - start).Seconds)
				{
					isFailed = true;
					break;
				}
			}
			if (isFailed)
			{
				this.AddCheckLog("cvFile not Loaded error!! name:" + cvFile, DebugAssetCheck.CheckLogStatus.Error);
				this.errorCount++;
			}
			else
			{
				this.AddCheckLog("cvFile(_acb_info.xml, .awb, .acb) Load success. name:" + cvFile, DebugAssetCheck.CheckLogStatus.Info);
				SoundManager.UnloadCueSheet(cvFile);
			}
			this.progressCount++;
			this.ProgressText.text = string.Format("{0} / {1}", this.progressCount, cvFileList.Count);
			this.ErrorCountText.text = string.Format("errorCount = {0}", this.errorCount);
			initAction = null;
			cvFile = null;
		}
		List<string>.Enumerator enumerator2 = default(List<string>.Enumerator);
		Resources.UnloadUnusedAssets();
		yield return null;
		this.NowCheking = false;
		this.CopyBtn.SetActEnable(true, false, false);
		yield break;
		yield break;
	}

	private void ResetCheckLog()
	{
		this.AssetCheckInfoLog = string.Empty;
		this.AssetCheckWarningLog = string.Empty;
		this.AssetCheckErrorLog = string.Empty;
	}

	private void AddCheckLog(string log, DebugAssetCheck.CheckLogStatus logStatus)
	{
		switch (logStatus)
		{
		case DebugAssetCheck.CheckLogStatus.Info:
			this.AssetCheckInfoLog = this.AssetCheckInfoLog + log + "\n";
			return;
		case DebugAssetCheck.CheckLogStatus.Warning:
			this.AssetCheckWarningLog = this.AssetCheckWarningLog + log + "\n";
			return;
		case DebugAssetCheck.CheckLogStatus.Error:
			this.AssetCheckErrorLog = this.AssetCheckErrorLog + log + "\n";
			return;
		default:
			return;
		}
	}

	private void CopyCheckLog(PguiButtonCtrl btn)
	{
		GUIUtility.systemCopyBuffer = string.Concat(new string[] { "-------------------- Info    --------------------\n", this.AssetCheckInfoLog, "-------------------- warning --------------------\n", this.AssetCheckWarningLog, "-------------------- error   --------------------\n", this.AssetCheckErrorLog });
	}

	private string AssetCheckInfoLog;

	private string AssetCheckWarningLog;

	private string AssetCheckErrorLog;

	private bool NowCheking;

	private int progressCount;

	private int errorCount;

	private Dictionary<string, string> LoadedCharaMap;

	private DebugAssetCheck.CheckTargetType CheckTarget;

	private PguiButtonCtrl CopyBtn;

	private PguiTextCtrl ProgressText;

	private PguiTextCtrl ErrorCountText;

	private PguiTextCtrl AssetBundleInfoText;

	private Dictionary<ScenarioDefine.EPISODE_TYPE, bool> epsodeTypeEnableMap;

	private Dictionary<int, List<string>> questEnemyPrefabList;

	private enum CheckTargetType
	{
		QuestOne,
		AssetBundle,
		LocalFile
	}

	private enum CheckLogStatus
	{
		Info,
		Warning,
		Error
	}

	private enum ModelCheckTarget
	{
		AssetListChara,
		AssetListEnemy,
		QuestOneEnemy,
		CharaClothes
	}
}
