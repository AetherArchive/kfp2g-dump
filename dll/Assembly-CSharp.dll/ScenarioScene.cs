using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CriWare;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioScene : MonoBehaviour
{
	public int movieMode { get; set; }

	public int skipScript { get; set; }

	private void Start()
	{
		this.root = base.transform.Find("root");
		this.light = base.transform.GetComponentsInChildren<Light>(true);
		this.mScenarioCamera = this.root.Find("ScenarioCamera").gameObject;
		EffectManager.BillboardCamera = this.mScenarioCamera.GetComponent<Camera>();
		CC_BrightnessContrastGamma component = this.mScenarioCamera.GetComponent<CC_BrightnessContrastGamma>();
		this.bcg_brightness = component.brightness;
		this.bcg_contrast = component.contrast;
		this.bcg_gamma = component.gamma;
		string text = "SceneScenario/GUI/Prefab/Auth_Telop_Ftont";
		this.nameTelop = Object.Instantiate<GameObject>((GameObject)Resources.Load(text));
		this.nameTelop.transform.SetParent(this.root.Find("GUIRoot"), false);
		this.nameTelop.transform.localScale = new Vector3(1f, 1f, 1f);
		this.nameTelop.AddComponent<SafeAreaScaler>();
		this.nameTelop.SetActive(false);
		this.GUIs = new ScenarioScene.GUI();
		string text2 = "SceneScenario/GUI/Prefab/GUI_Scenario";
		this.GUIs.mGUI_Scenario = Object.Instantiate<GameObject>((GameObject)Resources.Load(text2));
		this.GUIs.mGUI_Scenario.transform.SetParent(this.root.Find("GUIRoot"), false);
		this.GUIs.mGUI_Scenario.transform.localScale = new Vector3(1f, 1f, 1f);
		this.bgTelop = this.root.Find("GUIBgRoot/Bg").gameObject;
		this.bgTelop.transform.Find("BackPanel").gameObject.AddComponent<PguiTouchTrigger>().AddListener(delegate
		{
			this.bClickScreen = true;
			this.bClickWindow = false;
			this.bClickPhoto = false;
		}, null, null, null, null);
		this.GUIs.mGuiPlyBtns = this.GUIs.mGUI_Scenario.AddComponent<ScenarioGUIPlyBtns>();
		this.GUIs.mGUI_Scenario.AddComponent<SafeAreaScaler>();
		this.GUIs.mGuiSelect = this.GUIs.mGUI_Scenario.transform.Find("ChoiceBtns").gameObject.AddComponent<ScenarioGUISelect>();
		this.GUIs.mGuiSelect.InitialiseSelect();
		this.GUIs.mGuiSelect.gameObject.SetActive(false);
		this.GUIs.mTitleObj = this.GUIs.mGUI_Scenario.transform.Find("LocationInfo").gameObject;
		Transform transform = this.GUIs.mGUI_Scenario.transform.Find("SerifWindow");
		this.GUIs.mSerifAnim = transform.GetComponent<SimpleAnimation>();
		this.GUIs.mSerifImage_normal = transform.Find("WindowBase01");
		this.GUIs.mSerifImage_needle = transform.Find("WindowBase03");
		this.GUIs.mSerifImage_cloud = transform.Find("WindowBase02");
		this.GUIs.mSerifText = transform.Find("Txt_Serif").gameObject.GetComponent<TypewriterEffect>();
		this.GUIs.mSerifTextJpn = transform.Find("Txt_Serif_Eng").GetComponent<PguiTextCtrl>();
		this.GUIs.mSerifChara = transform.Find("Img_Namebase").gameObject;
		this.GUIs.mImgMiraiObj = transform.Find("CharaWindow").gameObject;
		this.logList = new List<KeyValuePair<string, string>>();
		this.GUIs.mLogWindow = this.GUIs.mGUI_Scenario.transform.Find("LogWindow").gameObject;
		this.GUIs.mLogWindow.transform.Find("Bg").gameObject.AddComponent<PguiTouchTrigger>().AddListener(delegate
		{
			this.ClickLog();
		}, null, null, null, null);
		ReuseScroll component2 = this.GUIs.mLogWindow.transform.Find("ScrollView").GetComponent<ReuseScroll>();
		component2.onStartItem = (Action<int, GameObject>)Delegate.Combine(component2.onStartItem, new Action<int, GameObject>(this.SetupLog));
		component2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(component2.onUpdateItem, new Action<int, GameObject>(this.SetupLog));
		component2.Setup(0, 0);
		this.GUIs.mLogEnd = this.GUIs.mLogWindow.transform.Find("Btn_End").gameObject;
		this.GUIs.mLogEnd.GetComponent<PguiButtonCtrl>().AddOnClickListener(new PguiButtonCtrl.OnClick(this.ClickLogEnd), PguiButtonCtrl.SoundType.DEFAULT);
		this.GUIs.mLogEnd.GetComponent<PguiButtonCtrl>().androidBackKeyTarget = true;
		this.GUIs.mLogWindow.SetActive(false);
		this.GUIs.mLogEnd.SetActive(false);
		this.GUIs.mKomado = this.GUIs.mGUI_Scenario.transform.Find("Window_Komado").gameObject;
		this.GUIs.mKomado.SetActive(false);
		this.GUIs.mNarration = this.GUIs.mGUI_Scenario.transform.Find("Narration").gameObject;
		this.GUIs.mNarrationText = this.GUIs.mNarration.transform.Find("Txt_Narration").GetComponent<TypewriterEffect>();
		this.GUIs.mNarration.SetActive(false);
		this.GUIs.mPhoto = this.GUIs.mGUI_Scenario.transform.Find("PhotoAuth").gameObject;
		this.GUIs.mPhoto.SetActive(false);
		this.ResetGUIShows();
		string text3 = "SceneScenario/GUI/Prefab/GUI_Scenario_Window";
		this.GUIs.mSkipWindow = Object.Instantiate<GameObject>((GameObject)Resources.Load(text3));
		this.GUIs.mSkipWindow.transform.SetParent(this.root.Find("GUIRoot"), false);
		this.GUIs.mSkipWindow.transform.localScale = new Vector3(1f, 1f, 1f);
		this.GUIs.sSkipWindow = this.GUIs.mSkipWindow.AddComponent<ScenarioSkipWindow>();
		this.GUIs.sSkipWindow.sGuiPlyBtn = this.GUIs.mGUI_Scenario.GetComponent<ScenarioGUIPlyBtns>();
		this.GUIs.mSkipWindow.AddComponent<SafeAreaScaler>();
		this.bgTelop.transform.Find("Auth_Telop_Back").gameObject.SetActive(false);
		this.mSetValues = base.GetComponent<ScenarioSetValues>();
		this.mCharaOffset = new List<ScenarioSetValues.CharaOffset>();
		this.mCharaPosition = new List<ScenarioCharaOffset.CharaPosition>();
		this.mFollow = new List<Transform>();
		this.waitParam = default(ScenarioScene.WaitParam);
		this.waitParam.ResetParam();
		this.mScriptCnt = 0;
		this.mCharaCtrl = -1;
		this.mSerifCtrl = -1;
		this.mSerifCharaID = -1;
		this.mNarrationCtrl = -1;
		this.bAuth = false;
		this.authPlayer = null;
		this.bMovie = false;
		this.moviePlayer = null;
		this.bgImage = "";
		this.bgImageDisp = false;
		this.bgMask = "";
		this.bgMaskDisp = false;
		this.bgZoomType = ScenarioDefine.BG_EFFECT_TYPE.ZOOM_NONE;
		this.bgZoomPos = Vector3.zero;
		this.bgZoomScl = 1f;
		this.bgZoomRate = 1f;
		this.bgShakeType = ScenarioDefine.BG_EFFECT_TYPE.NORMAL;
		this.bgShakePos = Vector3.zero;
		this.bgShakePwr = Vector3.zero;
		this.bClickScreen = false;
		this.bClickWindow = false;
		this.bClickPhoto = false;
		this.bNext = true;
		this.fAutoWaitTime = 0f;
		this.fAutoWaitSpeed = 5f;
		this.fAutoSerifTime = 0f;
		this.writerSpeed = 0;
		if (DataManager.DmUserInfo != null && DataManager.DmUserInfo.optionData != null)
		{
			if (DataManager.DmUserInfo.optionData.autoSpeed == 1)
			{
				this.fAutoWaitSpeed *= 0.5f;
			}
			else if (DataManager.DmUserInfo.optionData.autoSpeed == 2)
			{
				this.fAutoWaitSpeed = 0.1f;
			}
			this.writerSpeed = DataManager.DmUserInfo.optionData.ScenarioSpeed;
		}
		AssetManager.LoadAssetData(ScenarioScene.CHARA_OFFSET_PATH, AssetManager.OWNER.Scenario, 0, null);
		this.scenarioPath = ScenarioDefine.LOAD_PATH + this.scenarioName;
		AssetManager.LoadAssetData(this.scenarioPath, AssetManager.OWNER.Scenario, 0, null);
		this.scenarioScriptData = null;
		this.loopSE = false;
	}

	private void SetupLog(int index, GameObject go)
	{
		KeyValuePair<string, string> keyValuePair = new KeyValuePair<string, string>("", "");
		if (this.logList.Count > index)
		{
			keyValuePair = this.logList[index];
		}
		List<string> list;
		List<string> list2;
		this.CheckText(this.SizeCut(keyValuePair.Value), out list, out list2);
		for (int i = 0; i < list2.Count; i++)
		{
			int num = list2[i].IndexOf('\u3000');
			if (num == -1)
			{
				num = list2[i].IndexOf(' ');
			}
			if (num > 0 && !(list2[i][num - 1].ToString() != ">"))
			{
				int num2 = 0;
				for (int j = num; j < list2[i].Count<char>(); j++)
				{
					string text = list2[i][j].ToString();
					if (text != " " && text != "\u3000")
					{
						break;
					}
					num2++;
				}
				list2[i] = list2[i].Remove(num, num2);
			}
		}
		bool flag = keyValuePair.Key == "NARRATION";
		Transform transform = go.transform.Find("Normal");
		transform.gameObject.SetActive(!flag);
		Text text2 = transform.Find("Txt_CharaName").GetComponent<Text>();
		text2.text = keyValuePair.Key;
		PguiTextCtrl.OverflowTypeOptions overflowTypeOptions = (text2.GetComponent<PguiTextCtrl>().m_OverflowType = (keyValuePair.Key.Contains("\n") ? PguiTextCtrl.OverflowTypeOptions.ShrinkContents : PguiTextCtrl.OverflowTypeOptions.ResizeFreely));
		PguiTextCtrl.SetOverflowTypeOptions(ref text2, overflowTypeOptions, text2.fontSize);
		text2 = transform.Find("Txt_Serif").GetComponent<Text>();
		transform = go.transform.Find("Narration");
		transform.gameObject.SetActive(flag);
		if (flag)
		{
			text2 = transform.Find("Txt_Serif").GetComponent<Text>();
		}
		text2.lineSpacing = ((list2.Count > 0) ? 0.42f : 0.8f);
		if (!flag)
		{
			Vector3 localPosition = text2.transform.localPosition;
			localPosition.y = ((list2.Count > 0) ? (-5f) : (-15f));
			text2.transform.localPosition = localPosition;
		}
		string text3 = "";
		for (int k = 0; k < list.Count; k++)
		{
			if (k > 0)
			{
				text3 += "\n";
			}
			if (k < list2.Count)
			{
				text3 = string.Concat(new string[]
				{
					text3,
					"<size=",
					(text2.fontSize / 2).ToString(),
					"><color=#00000000>",
					list2[k],
					"</color></size>\n"
				});
			}
			text3 += list[k];
		}
		text2.text = text3;
	}

	private string SizeCut(string str)
	{
		for (;;)
		{
			int num = str.IndexOf("<size=");
			if (num < 0)
			{
				break;
			}
			int num2 = str.Substring(num).IndexOf(">");
			if (num2 < 0)
			{
				break;
			}
			num2 += num + 1;
			str = ((num > 0) ? str.Substring(0, num) : "") + ((str.Length > num2) ? str.Substring(num2) : "");
			num = str.IndexOf("</size>");
			if (num >= 0)
			{
				num2 = num + 7;
				str = ((num > 0) ? str.Substring(0, num) : "") + ((str.Length > num2) ? str.Substring(num2) : "");
			}
		}
		return str;
	}

	private void CheckText(string str, out List<string> txt, out List<string> rub)
	{
		txt = new List<string>();
		rub = new List<string>();
		txt.Add("");
		int i = str.IndexOf("[");
		int num = str.IndexOf(":");
		int num2 = str.IndexOf("]");
		if (i >= 0 && num - i > 1 && num2 - num > 1)
		{
			rub.Add("");
		}
		int num3 = 0;
		while (str.Length > 0)
		{
			if (str[0] == '\\')
			{
				List<string> list = txt;
				int num4 = txt.Count - 1;
				list[num4] += str.Substring(0, 2);
				if (rub.Count > 0)
				{
					list = rub;
					num4 = rub.Count - 1;
					list[num4] += "‐";
				}
			}
			else
			{
				if (str.Length >= 3)
				{
					if (str.IndexOf("</") == 0)
					{
						i = str.IndexOf(">");
						if (i > 0)
						{
							List<string> list = txt;
							int num4 = txt.Count - 1;
							list[num4] += str.Substring(0, i + 1);
							str = str.Substring(i + 1);
							if (num3 > 0)
							{
								num3 -= i + 1;
								continue;
							}
							continue;
						}
					}
					else if (str[0] == '<')
					{
						i = str.IndexOf(">");
						if (i > 0)
						{
							string text = str.Substring(1, 1);
							int num5 = str.IndexOf("</" + text);
							if (num5 > i && str.Substring(num5).IndexOf(">") > 0)
							{
								List<string> list = txt;
								int num4 = txt.Count - 1;
								list[num4] += str.Substring(0, i + 1);
								str = str.Substring(i + 1);
								if (num3 > 0)
								{
									num3 -= i + 1;
									continue;
								}
								continue;
							}
						}
					}
					else if (rub.Count > 0 && str[0] == '[')
					{
						num = str.IndexOf(":");
						num2 = str.IndexOf("]") - num;
						if (num > 1 && num2 > 1)
						{
							num--;
							num2--;
							num3 = num;
							str = str.Substring(1);
							continue;
						}
					}
					else if (str[0] == ':')
					{
						i = str.IndexOf("]");
						if (num3 > 0 && i > 1)
						{
							int num6 = rub.Count - 1;
							num = num3 * 2 - i + 1;
							num2 = num / 2;
							num %= 2;
							int num7 = num2;
							num3 = 0;
							if (num < 0)
							{
								num7 = 0;
								num = -num;
								num2--;
								int num8 = rub[num6].Length + num2;
								if (num8 < 0)
								{
									num2 += num8;
									num8 = 0;
									num7 = num;
									num = 0;
								}
								rub[num6] = rub[num6].Substring(0, num8);
								num3 = num2;
								num2 = 0;
							}
							List<string> list;
							int num4;
							if (num2 > 0)
							{
								list = rub;
								num4 = num6;
								list[num4] += new string('‐', num2);
							}
							if (num > 0)
							{
								list = rub;
								num4 = num6;
								list[num4] += new string('-', num);
							}
							list = rub;
							num4 = num6;
							list[num4] = list[num4] + "</color>" + str.Substring(1, i - 1) + "<color=#00000000>";
							if (num7 > 0)
							{
								list = rub;
								num4 = num6;
								list[num4] += new string('‐', num7);
							}
							if (num > 0)
							{
								list = rub;
								num4 = num6;
								list[num4] += new string('-', num);
							}
							str = str.Substring(i + 1);
							continue;
						}
					}
				}
				if (str[0] == '\r')
				{
					str = str.Substring(1);
				}
				else if (str[0] == '\n')
				{
					str = str.Substring(1);
					txt.Add("");
					if (rub.Count > 0)
					{
						rub.Add("");
					}
					num3 = 0;
				}
				else
				{
					List<string> list = txt;
					int num4 = txt.Count - 1;
					list[num4] += str.Substring(0, 1);
					str = str.Substring(1);
					if (rub.Count > 0)
					{
						for (i = 0; i < 2; i++)
						{
							if (num3 < 0)
							{
								num3++;
							}
							else if (num3 == 0)
							{
								list = rub;
								num4 = rub.Count - 1;
								list[num4] += "‐";
							}
						}
					}
				}
			}
		}
	}

	private void Update()
	{
		switch (this.status)
		{
		case ScenarioScene.Status.LOAD:
		{
			if (this.scenarioScriptData == null)
			{
				if (!AssetManager.IsLoadFinishAssetData(ScenarioScene.CHARA_OFFSET_PATH) || !AssetManager.IsLoadFinishAssetData(this.scenarioPath))
				{
					break;
				}
				ScenarioCharaOffset scenarioCharaOffset = AssetManager.GetAssetData(ScenarioScene.CHARA_OFFSET_PATH) as ScenarioCharaOffset;
				if (scenarioCharaOffset != null)
				{
					this.mCharaOffset = new List<ScenarioSetValues.CharaOffset>(scenarioCharaOffset.mCharaOffset);
					this.mCharaPosition = new List<ScenarioCharaOffset.CharaPosition>(scenarioCharaOffset.mCharaPosition);
				}
				if (this.mCharaOffset.Count <= 0)
				{
					this.mCharaOffset = new List<ScenarioSetValues.CharaOffset>(this.mSetValues.mCharaOffset);
				}
				if (this.mCharaPosition.Count <= 0)
				{
					foreach (object obj in Enum.GetValues(typeof(ScenarioDefine.CHARA_POSITION)))
					{
						ScenarioDefine.CHARA_POSITION chara_POSITION = (ScenarioDefine.CHARA_POSITION)obj;
						ScenarioCharaOffset.CharaPosition charaPosition = new ScenarioCharaOffset.CharaPosition();
						charaPosition.name = chara_POSITION.ToString();
						charaPosition.position = ((chara_POSITION < (ScenarioDefine.CHARA_POSITION)this.mSetValues.mCharaPosition.Length) ? this.mSetValues.mCharaPosition[(int)chara_POSITION] : Vector3.zero);
						charaPosition.rotation = ((chara_POSITION < (ScenarioDefine.CHARA_POSITION)this.mSetValues.mCharaRotation.Length) ? this.mSetValues.mCharaRotation[(int)chara_POSITION] : Vector3.zero);
						charaPosition.scale = Vector3.one;
						switch (chara_POSITION)
						{
						case ScenarioDefine.CHARA_POSITION.FRONT_5_1:
						case ScenarioDefine.CHARA_POSITION.FRONT_5_2:
						case ScenarioDefine.CHARA_POSITION.BACK_5_1:
						case ScenarioDefine.CHARA_POSITION.BACK_5_2:
						case ScenarioDefine.CHARA_POSITION.OUTSIDE_5_1:
						case ScenarioDefine.CHARA_POSITION.OUTSIDE_5_2:
						case ScenarioDefine.CHARA_POSITION.BETWEEN_4_1:
						case ScenarioDefine.CHARA_POSITION.PIP_4_1:
						case ScenarioDefine.CHARA_POSITION.HIDDEN_LEFT:
							charaPosition.shake = "L";
							break;
						case ScenarioDefine.CHARA_POSITION.FRONT_5_3:
						case ScenarioDefine.CHARA_POSITION.BACK_5_3:
						case ScenarioDefine.CHARA_POSITION.BETWEEN_4_2:
						case ScenarioDefine.CHARA_POSITION.BETWEEN_4_3:
						case ScenarioDefine.CHARA_POSITION.PIP_4_2:
						case ScenarioDefine.CHARA_POSITION.PIP_4_3:
							charaPosition.shake = "C";
							break;
						case ScenarioDefine.CHARA_POSITION.FRONT_5_4:
						case ScenarioDefine.CHARA_POSITION.FRONT_5_5:
						case ScenarioDefine.CHARA_POSITION.BACK_5_4:
						case ScenarioDefine.CHARA_POSITION.BACK_5_5:
						case ScenarioDefine.CHARA_POSITION.OUTSIDE_5_4:
						case ScenarioDefine.CHARA_POSITION.OUTSIDE_5_5:
						case ScenarioDefine.CHARA_POSITION.BETWEEN_4_4:
						case ScenarioDefine.CHARA_POSITION.PIP_4_4:
						case ScenarioDefine.CHARA_POSITION.HIDDEN_RIGHT:
							charaPosition.shake = "R";
							break;
						case ScenarioDefine.CHARA_POSITION.INTRODUCE:
							goto IL_0217;
						default:
							goto IL_0217;
						}
						IL_0223:
						switch (chara_POSITION)
						{
						case ScenarioDefine.CHARA_POSITION.FRONT_5_1:
						case ScenarioDefine.CHARA_POSITION.BACK_5_1:
						case ScenarioDefine.CHARA_POSITION.OUTSIDE_5_1:
						case ScenarioDefine.CHARA_POSITION.HIDDEN_LEFT:
							charaPosition.arrows = "L01";
							break;
						case ScenarioDefine.CHARA_POSITION.FRONT_5_2:
						case ScenarioDefine.CHARA_POSITION.BACK_5_2:
						case ScenarioDefine.CHARA_POSITION.OUTSIDE_5_2:
							charaPosition.arrows = "L02";
							break;
						case ScenarioDefine.CHARA_POSITION.FRONT_5_3:
						case ScenarioDefine.CHARA_POSITION.BACK_5_3:
							charaPosition.arrows = "C";
							break;
						case ScenarioDefine.CHARA_POSITION.FRONT_5_4:
						case ScenarioDefine.CHARA_POSITION.BACK_5_4:
						case ScenarioDefine.CHARA_POSITION.OUTSIDE_5_4:
							charaPosition.arrows = "R02";
							break;
						case ScenarioDefine.CHARA_POSITION.FRONT_5_5:
						case ScenarioDefine.CHARA_POSITION.BACK_5_5:
						case ScenarioDefine.CHARA_POSITION.OUTSIDE_5_5:
						case ScenarioDefine.CHARA_POSITION.HIDDEN_RIGHT:
							charaPosition.arrows = "R01";
							break;
						case ScenarioDefine.CHARA_POSITION.INTRODUCE:
							goto IL_0355;
						case ScenarioDefine.CHARA_POSITION.BETWEEN_4_1:
							charaPosition.arrows = "B01";
							break;
						case ScenarioDefine.CHARA_POSITION.BETWEEN_4_2:
							charaPosition.arrows = "B02";
							break;
						case ScenarioDefine.CHARA_POSITION.BETWEEN_4_3:
							charaPosition.arrows = "B03";
							break;
						case ScenarioDefine.CHARA_POSITION.BETWEEN_4_4:
							charaPosition.arrows = "B04";
							break;
						case ScenarioDefine.CHARA_POSITION.PIP_4_1:
							charaPosition.arrows = "P01";
							break;
						case ScenarioDefine.CHARA_POSITION.PIP_4_2:
							charaPosition.arrows = "P02";
							break;
						case ScenarioDefine.CHARA_POSITION.PIP_4_3:
							charaPosition.arrows = "P03";
							break;
						case ScenarioDefine.CHARA_POSITION.PIP_4_4:
							charaPosition.arrows = "P04";
							break;
						default:
							goto IL_0355;
						}
						IL_0361:
						this.mCharaPosition.Add(charaPosition);
						continue;
						IL_0355:
						charaPosition.arrows = "";
						goto IL_0361;
						IL_0217:
						charaPosition.shake = "";
						goto IL_0223;
					}
				}
				List<GameObject> list = new List<GameObject>();
				foreach (object obj2 in this.mScenarioCamera.transform)
				{
					Transform transform = (Transform)obj2;
					list.Add(transform.gameObject);
				}
				foreach (GameObject gameObject in list)
				{
					Object.Destroy(gameObject);
				}
				list = null;
				this.mFollow = new List<Transform>();
				for (int i = 0; i < this.mCharaPosition.Count; i++)
				{
					GameObject gameObject2 = new GameObject();
					gameObject2.transform.position = new Vector3(this.mCharaPosition[i].position.x * 3f, 1f, 0f);
					gameObject2.transform.SetParent(this.mScenarioCamera.transform, false);
					gameObject2.name = "follow" + i.ToString();
					this.mFollow.Add(gameObject2.transform);
					gameObject2.SetActive(false);
				}
				GameObject gameObject3 = AssetManager.InstantiateAssetData(this.scenarioPath, null);
				if (gameObject3 == null)
				{
					gameObject3 = new GameObject(this.scenarioName);
					this.scenarioScriptData = gameObject3.AddComponent<ScenarioScriptData>();
					this.scenarioScriptData.mTitleName = "None";
					this.scenarioScriptData.cueSheetList = new List<string>();
					this.scenarioScriptData.seSheetList = new List<string>();
					this.scenarioScriptData.effectSheetList = new List<string>();
					this.scenarioScriptData.charaDatas = new List<ScenarioScriptData.CharacterData>();
					this.scenarioScriptData.miraiDatas = new List<ScenarioScriptData.CharacterData>();
					this.scenarioScriptData.rowDatas = new List<ScenarioScriptData.ScenarioRowData>();
					ScenarioScriptData.ScenarioRowData scenarioRowData = new ScenarioScriptData.ScenarioRowData();
					scenarioRowData.mType = ScenarioDefine.TYPE.BACKGROUND;
					scenarioRowData.mStrParams[0] = "bg_black";
					this.scenarioScriptData.rowDatas.Add(scenarioRowData);
					this.scenarioScriptData.rowDatas.Add(new ScenarioScriptData.ScenarioRowData());
				}
				else
				{
					this.scenarioScriptData = gameObject3.GetComponent<ScenarioScriptData>();
					gameObject3.name = this.scenarioName;
					gameObject3.transform.SetParent(base.transform, false);
				}
				AssetManager.UnloadAssetData(ScenarioScene.CHARA_OFFSET_PATH, AssetManager.OWNER.Scenario);
				AssetManager.UnloadAssetData(this.scenarioPath, AssetManager.OWNER.Scenario);
			}
			this.mFriendList = new List<ScenarioScene.friend>();
			Transform transform2 = this.root.Find("Model");
			int num = 0;
			foreach (ScenarioScriptData.CharacterData characterData in this.scenarioScriptData.charaDatas)
			{
				ScenarioScene.friend fr = new ScenarioScene.friend();
				if (!string.IsNullOrEmpty(characterData.model))
				{
					GameObject gameObject4 = new GameObject();
					int num2 = int.Parse(characterData.model.Substring(3, 4));
					gameObject4.name = "Model" + this.modelCnt.ToString("D2") + "_" + num2.ToString();
					fr.charaModelHandle = gameObject4.AddComponent<CharaModelHandle>();
					fr.charaModelHandle.Initialize(new CharaModelHandle.InitializeParam(characterData.model, false, false)
					{
						isDisableVoice = true
					});
					fr.charaModelHandle.SetAlpha(0f);
					gameObject4.transform.SetParent(transform2, false);
					fr.obj = gameObject4;
					fr.charaModelOffset = this.mCharaOffset.Find((ScenarioSetValues.CharaOffset itm) => itm.model == fr.charaModelHandle.modelName);
					fr.mEff = new GameObject("eff");
					fr.mEff.transform.SetParent(fr.obj.transform, false);
					fr.mEff.transform.localPosition = new Vector3(0f, 0f, 0f);
					fr.puyo = characterData.model.IndexOf("_1015_") > 0 || characterData.model.IndexOf("_1016_") > 0;
					this.modelCnt++;
				}
				fr.ID = num;
				fr.mName = characterData.name;
				fr.faceId = "INVALID";
				fr.idleFaceId = "INVALID";
				fr.standby = CharaMotionDefine.ActKey.SCENARIO_STAND_BY;
				fr.standbyCF = 0.5f;
				this.mFriendList.Add(fr);
				num++;
			}
			this.mLoadSe = new List<IEnumerator>();
			foreach (string text in this.scenarioScriptData.cueSheetList)
			{
				this.mLoadSe.Add(SoundManager.LoadCueSheetWithDownload(text));
			}
			this.mLoadEffect = new List<string>();
			foreach (string text2 in this.scenarioScriptData.effectSheetList)
			{
				this.mLoadEffect.Add(text2);
				EffectManager.ReqLoadEffect(text2, AssetManager.OWNER.Scenario, 0, null);
			}
			this.mUseEffect = new List<string>();
			List<ScenarioScriptData.ScenarioRowData> list2 = this.scenarioScriptData.rowDatas.FindAll((ScenarioScriptData.ScenarioRowData itm) => itm.mType == ScenarioDefine.TYPE.EFFECT);
			if (list2.Find((ScenarioScriptData.ScenarioRowData itm) => itm.mIntParams[0] == 4) != null)
			{
				this.mUseEffect.Add(ScenarioScene.effectSandstarName);
			}
			if (list2.Find((ScenarioScriptData.ScenarioRowData itm) => itm.mIntParams[0] == 5) != null)
			{
				this.mUseEffect.Add(ScenarioScene.effectShineName);
			}
			if (list2.Find((ScenarioScriptData.ScenarioRowData itm) => itm.mIntParams[0] == 6) != null)
			{
				this.mUseEffect.Add(ScenarioScene.effectRainName);
			}
			if (list2.Find((ScenarioScriptData.ScenarioRowData itm) => itm.mIntParams[0] == 8) != null)
			{
				this.mUseEffect.Add(ScenarioScene.effectSteamName);
			}
			if (list2.Find((ScenarioScriptData.ScenarioRowData itm) => itm.mIntParams[0] == 9) != null)
			{
				this.mUseEffect.Add(ScenarioScene.effectSnowName);
			}
			if (list2.Find((ScenarioScriptData.ScenarioRowData itm) => itm.mIntParams[0] == 10) != null)
			{
				this.mUseEffect.Add(ScenarioScene.effectDarknessName);
			}
			if (list2.Find((ScenarioScriptData.ScenarioRowData itm) => itm.mIntParams[0] == 11) != null)
			{
				this.mUseEffect.Add(ScenarioScene.effectSpeedLineUpName);
			}
			if (list2.Find((ScenarioScriptData.ScenarioRowData itm) => itm.mIntParams[0] == 12) != null)
			{
				this.mUseEffect.Add(ScenarioScene.effectSpeedLineDownName);
			}
			if (list2.Find((ScenarioScriptData.ScenarioRowData itm) => itm.mIntParams[0] == 13) != null)
			{
				this.mUseEffect.Add(ScenarioScene.effectSpeedLineLeftName);
			}
			if (list2.Find((ScenarioScriptData.ScenarioRowData itm) => itm.mIntParams[0] == 14) != null)
			{
				this.mUseEffect.Add(ScenarioScene.effectSpeedLineRightName);
			}
			List<ScenarioScriptData.ScenarioRowData> list3 = this.scenarioScriptData.rowDatas.FindAll((ScenarioScriptData.ScenarioRowData itm) => itm.mType == ScenarioDefine.TYPE.ALL_FADE);
			if (list3.Find((ScenarioScriptData.ScenarioRowData itm) => itm.mIntParams[1] == 9) != null)
			{
				this.mUseEffect.Add(ScenarioScene.effectSmokeName);
			}
			if (list3.Find((ScenarioScriptData.ScenarioRowData itm) => itm.mIntParams[1] == 8) != null)
			{
				this.mUseEffect.Add(ScenarioScene.effectSoilSmokeName);
			}
			foreach (string text3 in this.mUseEffect)
			{
				EffectManager.ReqLoadEffect(text3, AssetManager.OWNER.Scenario, 0, null);
			}
			this.effectStage = null;
			this.effectSmoke = null;
			this.status = ScenarioScene.Status.LOAD_WAIT;
			break;
		}
		case ScenarioScene.Status.LOAD_WAIT:
		{
			int num3 = 0;
			num3 = 0;
			using (List<IEnumerator>.Enumerator enumerator5 = this.mLoadSe.GetEnumerator())
			{
				while (enumerator5.MoveNext())
				{
					if (enumerator5.Current.MoveNext())
					{
						num3++;
					}
				}
			}
			if (num3 <= 0)
			{
				num3 = 0;
				using (List<string>.Enumerator enumerator4 = this.mLoadEffect.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						if (EffectManager.IsLoadFinishEffect(enumerator4.Current))
						{
							num3++;
						}
					}
				}
				if (num3 == this.mLoadEffect.Count)
				{
					num3 = 0;
					using (List<string>.Enumerator enumerator4 = this.mUseEffect.GetEnumerator())
					{
						while (enumerator4.MoveNext())
						{
							if (EffectManager.IsLoadFinishEffect(enumerator4.Current))
							{
								num3++;
							}
						}
					}
					if (num3 == this.mUseEffect.Count)
					{
						num3 = 0;
						for (int j = 0; j < this.mFriendList.Count; j++)
						{
							if (this.mFriendList[j].charaModelHandle != null && this.mFriendList[j].charaModelHandle.IsFinishInitialize())
							{
								this.mFriendList[j].charaModelHandle.SetModelActive(true);
								this.mFriendList[j].charaModelHandle.PlayAnimation(this.mFriendList[j].standby, true, 1f, 0f, 0f, false);
								this.mFriendList[j].charaModelHandle.enabledFaceMotion = true;
								num3++;
							}
						}
						if (num3 == this.modelCnt)
						{
							this.SetBloomEffect();
							if (this.GUIs.mGuiPlyBtns.IsFadeEnd())
							{
								this.logList = new List<KeyValuePair<string, string>>();
								this.status = ScenarioScene.Status.IN;
							}
						}
					}
				}
			}
			break;
		}
		case ScenarioScene.Status.IN:
			this.UpdateProcess();
			if (this.bgImageDisp && this.bgMaskDisp)
			{
				this.GUIs.mGuiPlyBtns.SetFade(false, ScenarioDefine.FADE_TYPE.WHITE_IN, false);
				this.status = ScenarioScene.Status.PROCESS;
			}
			break;
		case ScenarioScene.Status.PROCESS:
			if (this.UpdateProcess())
			{
				this.GUIs.mGuiPlyBtns.SetFade(false, ScenarioDefine.FADE_TYPE.WHITE_OUT, false);
				this.status = ScenarioScene.Status.OUT;
			}
			break;
		case ScenarioScene.Status.OUT:
			if (this.GUIs.mGuiPlyBtns.IsFadeEnd() && !DataManager.IsServerRequesting())
			{
				this.Disable();
				this.status = ScenarioScene.Status.TERM;
			}
			break;
		}
		float num4 = Mathf.Abs(this.bgShakePwr.x);
		float num5 = Mathf.Abs(this.bgShakePwr.y);
		if (num4 < num5)
		{
			num4 = num5;
		}
		if (this.bgShakeType == ScenarioDefine.BG_EFFECT_TYPE.QUAKE || this.bgShakeType == ScenarioDefine.BG_EFFECT_TYPE.SHAKE_ONCE)
		{
			float num6 = ((this.bgShakeType == ScenarioDefine.BG_EFFECT_TYPE.SHAKE_ONCE) ? (-0.75f) : (-1f));
			if ((this.bgShakePwr.x < 0f) ? ((this.bgShakePos.x = this.bgShakePos.x - this.BgShakeSpeed.x) <= this.bgShakePwr.x) : ((this.bgShakePos.x = this.bgShakePos.x + this.BgShakeSpeed.x) >= this.bgShakePwr.x))
			{
				this.bgShakePos.x = this.bgShakePwr.x;
				this.bgShakePwr.x = this.bgShakePwr.x * num6;
			}
			if ((this.bgShakePwr.y < 0f) ? ((this.bgShakePos.y = this.bgShakePos.y - this.BgShakeSpeed.y) <= this.bgShakePwr.y) : ((this.bgShakePos.y = this.bgShakePos.y + this.BgShakeSpeed.y) >= this.bgShakePwr.y))
			{
				this.bgShakePos.y = this.bgShakePwr.y;
				this.bgShakePwr.y = this.bgShakePwr.y * num6;
			}
			if (this.bgShakeType == ScenarioDefine.BG_EFFECT_TYPE.SHAKE_ONCE && Mathf.Abs(this.bgShakePwr.x) <= this.BgShakePower.x * 0.1f && Mathf.Abs(this.bgShakePwr.y) <= this.BgShakePower.y * 0.1f)
			{
				this.bgShakeType = ScenarioDefine.BG_EFFECT_TYPE.NORMAL;
			}
		}
		else
		{
			this.bgShakePos = Vector3.zero;
			this.bgShakePwr = Vector3.zero;
		}
		float num7 = this.BgZoomScale - 1f;
		Vector3 zero = Vector3.zero;
		if (this.bgZoomType == ScenarioDefine.BG_EFFECT_TYPE.ZOOM1)
		{
			zero = new Vector3(1f, -1f, 0f);
		}
		else if (this.bgZoomType == ScenarioDefine.BG_EFFECT_TYPE.ZOOM2)
		{
			zero = new Vector3(0f, -1f, 0f);
		}
		else if (this.bgZoomType == ScenarioDefine.BG_EFFECT_TYPE.ZOOM3)
		{
			zero = new Vector3(-1f, -1f, 0f);
		}
		else if (this.bgZoomType == ScenarioDefine.BG_EFFECT_TYPE.ZOOM4)
		{
			zero = new Vector3(1f, 0f, 0f);
		}
		else if (this.bgZoomType == ScenarioDefine.BG_EFFECT_TYPE.ZOOM5)
		{
			zero = new Vector3(0f, 0f, 0f);
		}
		else if (this.bgZoomType == ScenarioDefine.BG_EFFECT_TYPE.ZOOM6)
		{
			zero = new Vector3(-1f, 0f, 0f);
		}
		else if (this.bgZoomType == ScenarioDefine.BG_EFFECT_TYPE.ZOOM7)
		{
			zero = new Vector3(1f, 1f, 0f);
		}
		else if (this.bgZoomType == ScenarioDefine.BG_EFFECT_TYPE.ZOOM8)
		{
			zero = new Vector3(0f, 1f, 0f);
		}
		else if (this.bgZoomType == ScenarioDefine.BG_EFFECT_TYPE.ZOOM9)
		{
			zero = new Vector3(-1f, 1f, 0f);
		}
		else
		{
			num7 = 0f;
		}
		zero.x *= 640f * num7;
		zero.y *= 360f * num7;
		num7 += 1f;
		if (this.backgroundMat.m_RawImage.rectTransform.rect.width > 1f)
		{
			num7 *= (1280f + num4 + num4) / this.backgroundMat.m_RawImage.rectTransform.rect.width;
		}
		this.backgroundMat.transform.localPosition = Vector3.Lerp(this.bgZoomPos, zero, this.bgZoomRate) + this.bgShakePos;
		this.backgroundMat.transform.localScale = Vector3.one * Mathf.Lerp(this.bgZoomScl, num7, this.bgZoomRate);
		float num8 = ((this.BgZoomTime > 0.01f) ? (TimeManager.DeltaTime / this.BgZoomTime) : 1f);
		if ((this.bgZoomRate += num8) > 1f)
		{
			this.bgZoomRate = 1f;
		}
		this.SetBloomEffect();
	}

	private bool UpdateProcess()
	{
		if (this.GUIs.sSkipWindow.mIsSkip)
		{
			if (this.questId > 0)
			{
				DataManager.DmQuest.RequestStorySkip(this.questId, this.storyType);
			}
			foreach (CriAtomExPlayback criAtomExPlayback in this.playloopSEList)
			{
				criAtomExPlayback.Stop();
			}
			return true;
		}
		if (this.GUIs.sSkipWindow.mIsStory)
		{
			if (this.GUIs.mLogWindow.activeSelf)
			{
				if (!this.GUIs.mGuiPlyBtns.bLogWindowShow)
				{
					if (this.questId > 0)
					{
						DataManager.DmQuest.RequestStorySkip(this.questId, this.storyType);
					}
					foreach (CriAtomExPlayback criAtomExPlayback2 in this.playloopSEList)
					{
						criAtomExPlayback2.Stop();
					}
					return true;
				}
			}
			else
			{
				int i = this.mScriptCnt;
				if (!this.bNext)
				{
					i++;
				}
				while (i < this.scenarioScriptData.rowDatas.Count)
				{
					ScenarioScriptData.ScenarioRowData scenarioRowData = this.scenarioScriptData.rowDatas[i++];
					for (int j = 0; j < scenarioRowData.mStrParams.Length; j++)
					{
						scenarioRowData.mStrParams[j] = this.FixSpecialText(scenarioRowData.mStrParams[j]);
					}
					if (scenarioRowData.mType == ScenarioDefine.TYPE.SERIF)
					{
						this.SerifChara(scenarioRowData, false);
					}
					else if (scenarioRowData.mType == ScenarioDefine.TYPE.SERIF_MIRAI)
					{
						this.SerifChara(scenarioRowData, true);
					}
					else if (scenarioRowData.mType == ScenarioDefine.TYPE.SELECT)
					{
						this.logList.Add(new KeyValuePair<string, string>("【選択肢】", this.CheckSerif(scenarioRowData.mStrParams[0])));
						i = scenarioRowData.mIntParams[2];
					}
					else if (scenarioRowData.mType == ScenarioDefine.TYPE.JUMP)
					{
						i = scenarioRowData.mIntParams[1];
					}
					else if (scenarioRowData.mType == ScenarioDefine.TYPE.NARRATION)
					{
						this.logList.Add(new KeyValuePair<string, string>("NARRATION", this.CheckSerif(scenarioRowData.mStrParams[0])));
					}
				}
				this.GUIs.mLogWindow.SetActive(true);
				this.GUIs.mLogEnd.SetActive(true);
				this.GUIs.mLogWindow.transform.Find("ScrollView").GetComponent<ReuseScroll>().Resize(this.logList.Count, 0);
				this.GUIs.mGuiPlyBtns.bLogWindowShow = true;
			}
			return false;
		}
		if (this.GUIs.mGuiPlyBtns.bLogWindowShow)
		{
			if (!this.GUIs.mLogWindow.activeSelf)
			{
				this.GUIs.mLogWindow.SetActive(true);
				this.GUIs.mLogEnd.SetActive(false);
				this.GUIs.mLogWindow.transform.Find("ScrollView").GetComponent<ReuseScroll>().Resize(this.logList.Count, this.logList.Count - 1);
			}
		}
		else if (this.GUIs.mLogWindow.activeSelf)
		{
			this.GUIs.mLogWindow.SetActive(false);
		}
		if (this.GUIs.mGuiPlyBtns.bAutoFlag && !this.GUIs.mGuiPlyBtns.bSkipWindowShow && !this.GUIs.mLogWindow.activeSelf)
		{
			this.fAutoWaitTime += TimeManager.DeltaTime;
			bool flag = this.fAutoWaitTime >= this.fAutoWaitSpeed;
			if (this.mSerifCtrl == this.mScriptCnt)
			{
				if (this.GUIs.mSerifText.IsCompleteDisplayText && ((this.fAutoSerifTime += TimeManager.DeltaTime) > 0.1f && flag))
				{
					this.fAutoWaitTime = 0f;
					this.fAutoSerifTime = 0f;
					this.bClickScreen = true;
				}
			}
			else if (this.mNarrationCtrl == this.mScriptCnt)
			{
				if (this.GUIs.mNarrationText.IsCompleteDisplayText && ((this.fAutoSerifTime += TimeManager.DeltaTime) > 0.1f && flag))
				{
					this.fAutoWaitTime = 0f;
					this.fAutoSerifTime = 0f;
					this.bClickScreen = true;
				}
			}
			else if (flag)
			{
				this.fAutoWaitTime = 0f;
				this.fAutoSerifTime = 0f;
				if (this.GUIs.mGuiSelect.gameObject.activeSelf)
				{
					this.GUIs.mGuiSelect.mSelectedRow = 1;
				}
				else
				{
					this.bClickScreen = true;
					this.bClickWindow = false;
					this.bClickPhoto = false;
				}
			}
			Screen.sleepTimeout = -1;
		}
		else
		{
			this.fAutoWaitTime = 0f;
			this.fAutoSerifTime = 0f;
			Screen.sleepTimeout = -2;
		}
		foreach (ScenarioScene.friend friend in this.mFriendList)
		{
			if (friend.mEffData != null && friend.mEffData.IsFinishByAnimation())
			{
				EffectManager.DestroyEffect(friend.mEffData);
				friend.mEffData = null;
			}
		}
		if (this.effectStage != null && this.effectStage.IsFinishByAnimation())
		{
			EffectManager.DestroyEffect(this.effectStage);
			this.effectStage = null;
		}
		if (this.effectSmoke != null && this.effectSmoke.IsFinishByAnimation())
		{
			EffectManager.DestroyEffect(this.effectSmoke);
			this.effectSmoke = null;
		}
		if (this.GUIs.mGuiSelect.gameObject.activeSelf)
		{
			if (this.GUIs.mGuiSelect.mSelectedRow > 0)
			{
				this.GUIs.mGuiSelect.gameObject.SetActive(false);
				this.bNext = true;
				if (this.GUIs.mGuiSelect.mSelectedRow == 1)
				{
					this.mScriptCnt = this.waitParam.jumpRows[0];
					this.logList.Add(new KeyValuePair<string, string>("【選択肢】", this.waitParam.jumpText[0]));
				}
				else if (this.GUIs.mGuiSelect.mSelectedRow == 2)
				{
					this.mScriptCnt = this.waitParam.jumpRows[1];
					this.logList.Add(new KeyValuePair<string, string>("【選択肢】", this.waitParam.jumpText[1]));
				}
				this.GUIs.mGuiSelect.mSelectedRow = -1;
			}
		}
		else if (this.mSerifCtrl == this.mScriptCnt)
		{
			if (this.bClickScreen && this.GUIs.mSerifText.ClickScreen())
			{
				this.bNext = true;
				this.mScriptCnt++;
				if (this.mScriptCnt >= this.scenarioScriptData.rowDatas.Count)
				{
					return true;
				}
			}
		}
		else if (this.mNarrationCtrl == this.mScriptCnt)
		{
			if (this.bClickScreen && this.GUIs.mNarrationText.ClickScreen())
			{
				int num = this.mScriptCnt + 1;
				if (num < this.scenarioScriptData.rowDatas.Count && this.scenarioScriptData.rowDatas[num].mType == ScenarioDefine.TYPE.NARRATION)
				{
					this.bNext = true;
					this.mNarrationCtrl = (this.mScriptCnt = num);
				}
				else
				{
					this.GUIs.mNarration.GetComponent<SimpleAnimation>().ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, null);
					this.mNarrationCtrl = -1;
				}
			}
		}
		else if (this.GUIs.mNarration.activeSelf && !this.GUIs.mNarration.GetComponent<SimpleAnimation>().ExIsPlaying())
		{
			this.GUIs.mNarration.SetActive(false);
			this.bNext = true;
			this.mScriptCnt++;
			if (this.mScriptCnt >= this.scenarioScriptData.rowDatas.Count)
			{
				return true;
			}
		}
		if (this.bAuth)
		{
			this.UpdateAuth();
		}
		if (this.bMovie)
		{
			this.UpdateMovie();
		}
		this.CharacterUpdate();
		if (this.mScriptCnt >= this.scenarioScriptData.rowDatas.Count)
		{
			return true;
		}
		ScenarioScriptData.ScenarioRowData scenarioRowData2 = this.scenarioScriptData.rowDatas[this.mScriptCnt];
		for (int k = 0; k < scenarioRowData2.mStrParams.Length; k++)
		{
			scenarioRowData2.mStrParams[k] = this.FixSpecialText(scenarioRowData2.mStrParams[k]);
		}
		if (this.bIntroduce)
		{
			this.UpdateIntroduce(scenarioRowData2);
			return false;
		}
		if (this.bNext)
		{
			this.bNext = false;
			this.DoOrderParts(scenarioRowData2);
			this.fAutoWaitTime = 0f;
			this.fAutoSerifTime = 0f;
		}
		this.bClickScreen = false;
		return false;
	}

	private void ClickLog()
	{
		if (!this.GUIs.sSkipWindow.mIsStory)
		{
			this.GUIs.mGuiPlyBtns.bLogWindowShow = false;
		}
	}

	private void ClickLogEnd(PguiButtonCtrl button)
	{
		if (this.GUIs.sSkipWindow.mIsStory)
		{
			this.GUIs.mGuiPlyBtns.bLogWindowShow = false;
		}
	}

	private void ResetGUIShows()
	{
		this.GUIs.mSerifImage_normal.gameObject.SetActive(false);
		this.GUIs.mSerifImage_cloud.gameObject.SetActive(false);
		this.GUIs.mSerifImage_needle.gameObject.SetActive(false);
		this.GUIs.mSerifChara.SetActive(false);
		this.GUIs.mSerifText.gameObject.SetActive(false);
		this.GUIs.mSerifTextJpn.gameObject.SetActive(false);
		this.GUIs.mImgMiraiObj.SetActive(false);
		this.GUIs.mTitleObj.SetActive(false);
	}

	private void DoOrderParts(ScenarioScriptData.ScenarioRowData data)
	{
		switch (data.mType)
		{
		case ScenarioDefine.TYPE.INITIALISE:
			this.DoOrderInitialise(data);
			return;
		case ScenarioDefine.TYPE.TITLE:
			this.DoOrderTitle(data);
			return;
		case ScenarioDefine.TYPE.SERIF:
			this.DoOrderSerif(data);
			return;
		case ScenarioDefine.TYPE.SERIF_MIRAI:
			this.DoOrderSerifMirai(data);
			return;
		case ScenarioDefine.TYPE.SPECIAL_AUTH:
			this.DoOrderSpecialAuth(data);
			return;
		case ScenarioDefine.TYPE.BACKGROUND:
			this.DoOrderBackground(data);
			return;
		case ScenarioDefine.TYPE.BGM:
			this.DoOrderBGM(data);
			return;
		case ScenarioDefine.TYPE.SE:
			this.DoOrderSE(data);
			return;
		case ScenarioDefine.TYPE.SELECT:
			this.DoOrderSelect(data);
			return;
		case ScenarioDefine.TYPE.JUMP:
			this.DoOrderJump(data);
			return;
		case ScenarioDefine.TYPE.LABEL:
			this.DoOrderLebel(data);
			return;
		case ScenarioDefine.TYPE.CHARA_CTRL:
			this.DoOrderCharCtrl(data);
			return;
		case ScenarioDefine.TYPE.EFFECT:
			this.DoOrderEffect(data);
			return;
		case ScenarioDefine.TYPE.ALL_FADE:
			this.DoOrderAllFade(data);
			return;
		case ScenarioDefine.TYPE.INTRODUCE:
			this.DoOrderIntroduce(data);
			return;
		case ScenarioDefine.TYPE.SERIF_OFF:
			this.DoOrderSerifOff(data);
			return;
		case ScenarioDefine.TYPE.WAIT:
			this.DoOrderWait(data);
			return;
		case ScenarioDefine.TYPE.WINDOW:
			this.DoOrderWindow(data);
			return;
		case ScenarioDefine.TYPE.NARRATION:
			this.DoOrderNarration(data);
			return;
		case ScenarioDefine.TYPE.PHOTO:
			this.DoOrderPhoto(data);
			return;
		case ScenarioDefine.TYPE.BG_EFFECT:
			this.DoOrderBgEffect(data);
			return;
		case ScenarioDefine.TYPE.SPECIAL_MOVIE:
			this.DoOrderSpecialMovie(data);
			return;
		default:
			return;
		}
	}

	private void DoOrderInitialise(ScenarioScriptData.ScenarioRowData data)
	{
		this.OrderCharacter(data);
		this.bNext = true;
		this.mScriptCnt++;
	}

	private void DoOrderTitle(ScenarioScriptData.ScenarioRowData data)
	{
		this.ResetGUIShows();
		this.GUIs.mTitleObj.SetActive(true);
		this.GUIs.mTitleObj.GetComponent<SimpleAnimation>().ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
		this.GUIs.mTitleObj.transform.Find("Txt_Location").GetComponent<Text>().text = data.mStrParams[0];
		base.StartCoroutine(this.Title(this.mScriptCnt));
	}

	private IEnumerator Title(int cnt)
	{
		while (this.GUIs.mTitleObj.activeSelf && this.GUIs.mTitleObj.GetComponent<SimpleAnimation>().ExIsPlaying())
		{
			yield return null;
		}
		if (this.mScriptCnt == cnt)
		{
			this.bNext = true;
			this.mScriptCnt++;
			this.GUIs.mTitleObj.SetActive(false);
		}
		yield break;
	}

	private void DoOrderSerif(ScenarioScriptData.ScenarioRowData data)
	{
		this.SetSerifCommon(data);
		string text = this.SerifChara(data, false);
		this.GUIs.mSerifChara.SetActive(!string.IsNullOrEmpty(text));
		if (this.GUIs.mSerifChara.activeSelf)
		{
			this.GUIs.mSerifChara.transform.Find("Txt_name").gameObject.GetComponent<Text>().text = text;
		}
		this.GUIs.mImgMiraiObj.SetActive(false);
		this.SetSerifFrame(data, false);
		this.mSerifCtrl = this.mScriptCnt;
	}

	private void DoOrderSerifMirai(ScenarioScriptData.ScenarioRowData data)
	{
		this.SetSerifCommon(data);
		string text = this.SerifChara(data, true);
		this.GUIs.mSerifChara.SetActive(!string.IsNullOrEmpty(text));
		if (this.GUIs.mSerifChara.activeSelf)
		{
			this.GUIs.mSerifChara.transform.Find("Txt_name").gameObject.GetComponent<Text>().text = text;
		}
		this.GUIs.mImgMiraiObj.SetActive(data.mSerifCharaID >= 0);
		if (data.mSerifCharaID >= 0)
		{
			string text2 = "Texture2D/Icon_Chara/Chara/" + this.scenarioScriptData.miraiDatas[data.mSerifCharaID].model;
			this.GUIs.mImgMiraiObj.transform.Find("Icon_Chara").GetComponent<PguiRawImageCtrl>().SetRawImage(text2, true, false, null);
		}
		this.SetSerifFrame(data, true);
		this.mSerifCtrl = this.mScriptCnt;
	}

	private void SetSerifCommon(ScenarioScriptData.ScenarioRowData data)
	{
		this.ResetGUIShows();
		this.GUIs.mSerifText.gameObject.SetActive(true);
		this.GUIs.mSerifText.SetCurrentText(this.CheckSerif(data.mStrParams[0]), data.mIntParams[2], this.writerSpeed);
		this.GUIs.mSerifTextJpn.gameObject.SetActive(!string.IsNullOrEmpty(data.mStrParams[2]));
		List<string> list;
		List<string> list2;
		this.CheckText(this.CheckSerif(data.mStrParams[2]), out list, out list2);
		string text = "";
		for (int i = 0; i < list.Count; i++)
		{
			if (i > 0)
			{
				text += "\n";
			}
			text += list[i];
		}
		this.GUIs.mSerifTextJpn.text = text;
		this.OrderCharacter(data);
		if (data.mIntParams[4] > 0 && ScenarioDefine.SeName.Length > data.mIntParams[4])
		{
			SoundManager.Play(ScenarioDefine.SeName[data.mIntParams[4]], false, false);
		}
		else if (data.mIntParams[4] < 0 && this.scenarioScriptData.seSheetList != null && this.scenarioScriptData.seSheetList.Count >= -data.mIntParams[4])
		{
			string text2 = this.scenarioScriptData.seSheetList[-1 - data.mIntParams[4]];
			bool flag = text2.EndsWith("_loop") || text2.IndexOf("drumroll_start") != -1;
			CriAtomExPlayback criAtomExPlayback = SoundManager.Play(text2, false, false);
			if (flag)
			{
				this.playloopSEList.Add(criAtomExPlayback);
			}
		}
		SoundManager.Play(data.mStrParams[1], false, false);
	}

	private string CheckSerif(string serif)
	{
		string text = ((DataManager.DmUserInfo == null) ? "カバン" : DataManager.DmUserInfo.userName);
		return serif.Replace("※※※※※※※※※※", text);
	}

	private string SerifChara(ScenarioScriptData.ScenarioRowData data, bool mirai)
	{
		string text = data.mSerifCharaName;
		if (string.IsNullOrEmpty(text) && data.mSerifCharaID >= 0)
		{
			text = (mirai ? this.scenarioScriptData.miraiDatas[data.mSerifCharaID].name : this.scenarioScriptData.charaDatas[data.mSerifCharaID].name);
		}
		string[] array = text.Split('｜', StringSplitOptions.None);
		if (array.Length > 1)
		{
			text = array[0] + "\n" + (array[1] + "\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000").Substring(0, 14);
		}
		int num = (string.IsNullOrEmpty(data.mStrParams[2]) ? 0 : 2);
		this.logList.Add(new KeyValuePair<string, string>(text, this.CheckSerif(data.mStrParams[num])));
		return text;
	}

	private void SetSerifFrame(ScenarioScriptData.ScenarioRowData data, bool isMirai = false)
	{
		int num = 5;
		if (!isMirai)
		{
			foreach (ScenarioScene.friend friend in this.mFriendList)
			{
				if (friend.ID == data.mSerifCharaID)
				{
					if (friend.charaModelHandle.IsDisp())
					{
						num = friend.posID;
						break;
					}
					break;
				}
			}
		}
		string text = null;
		bool flag = false;
		if (data.mIntParams[0] == 1)
		{
			this.GUIs.mSerifImage_needle.gameObject.SetActive(true);
			text = "03_CMN";
			flag = true;
		}
		else
		{
			Transform transform = this.GUIs.mSerifImage_normal;
			if (data.mIntParams[0] == 2)
			{
				transform = this.GUIs.mSerifImage_cloud;
				text = "02_CMN";
				flag = true;
			}
			else if (num >= 0 && num < this.mCharaPosition.Count && !string.IsNullOrEmpty(this.mCharaPosition[num].shake))
			{
				text = "01_" + this.mCharaPosition[num].shake;
			}
			string text2 = null;
			if (!isMirai && num >= 0 && num < this.mCharaPosition.Count && !string.IsNullOrEmpty(this.mCharaPosition[num].arrows))
			{
				text2 = "_" + this.mCharaPosition[num].arrows;
			}
			transform.gameObject.SetActive(true);
			foreach (object obj in transform)
			{
				Transform transform2 = (Transform)obj;
				transform2.gameObject.SetActive(!string.IsNullOrEmpty(text2) && transform2.gameObject.name.EndsWith(text2));
			}
		}
		if (data.mIntParams[1] == 1)
		{
			text = "SHAKE_V";
			flag = true;
		}
		else if (data.mIntParams[1] == 2)
		{
			text = "SHAKE_H";
			flag = true;
		}
		if (this.mSerifCharaID != data.mSerifCharaID || flag)
		{
			this.mSerifCharaID = (flag ? (-1) : data.mSerifCharaID);
			if (!string.IsNullOrEmpty(text))
			{
				this.GUIs.mSerifAnim.ExPlayAnimation(text, null);
			}
		}
	}

	private void DoOrderSpecialAuth(ScenarioScriptData.ScenarioRowData data)
	{
		this.authPhase = ScenarioScene.AUTH_PHASE.INITIALISE;
		this.authName = data.mStrParams[0];
		this.bAuth = true;
	}

	private void DoOrderSpecialMovie(ScenarioScriptData.ScenarioRowData data)
	{
		this.moviePhase = ScenarioScene.MOVIE_PHASE.INITIALISE;
		this.movieName = data.mStrParams[0];
		this.bMovie = true;
	}

	private void DoOrderBackground(ScenarioScriptData.ScenarioRowData data)
	{
		if (this.bgImage != data.mStrParams[0])
		{
			this.bgImage = data.mStrParams[0];
			string text = "Texture2D/Bg_Scenario/" + this.bgImage;
			this.bgImageDisp = false;
			this.backgroundMat.SetRawImage(text, true, true, delegate
			{
				this.bgImageDisp = true;
			});
		}
		string text2 = data.mStrParams[1];
		if (string.IsNullOrEmpty(text2))
		{
			text2 = "none";
		}
		if (this.bgMask != text2)
		{
			this.bgMask = text2;
			this.bgMaskDisp = true;
			Transform transform = this.backgroundMat.transform.Find("MaskPanel");
			if (transform != null)
			{
				string text3 = "";
				if (ScenarioScene.ENABLE_ALPHA_DIC.ContainsKey(this.bgMask))
				{
					text3 = ScenarioScene.ENABLE_ALPHA_DIC[this.bgMask];
				}
				if (string.IsNullOrEmpty(text3))
				{
					transform.gameObject.SetActive(false);
				}
				else
				{
					transform.gameObject.SetActive(true);
					PguiRawImageCtrl component = transform.GetComponent<PguiRawImageCtrl>();
					if (component != null)
					{
						this.bgMaskDisp = false;
						component.SetRawImage(text3, true, true, delegate
						{
							this.bgMaskDisp = true;
						});
					}
				}
			}
		}
		this.DoOrderBgEffect(data);
	}

	private void DoOrderBgEffect(ScenarioScriptData.ScenarioRowData data)
	{
		ScenarioDefine.BG_EFFECT_TYPE bg_EFFECT_TYPE = (ScenarioDefine.BG_EFFECT_TYPE)data.mIntParams[0];
		if (bg_EFFECT_TYPE >= ScenarioDefine.BG_EFFECT_TYPE.ZOOM_NONE && bg_EFFECT_TYPE <= ScenarioDefine.BG_EFFECT_TYPE.ZOOM9)
		{
			base.StartCoroutine(this.Zoom(this.mScriptCnt, bg_EFFECT_TYPE));
			return;
		}
		if (bg_EFFECT_TYPE >= ScenarioDefine.BG_EFFECT_TYPE.NORMAL && bg_EFFECT_TYPE <= ScenarioDefine.BG_EFFECT_TYPE.SHAKE_ONCE)
		{
			base.StartCoroutine(this.Shake(this.mScriptCnt, bg_EFFECT_TYPE));
			return;
		}
		base.StartCoroutine(this.BgDisp(this.mScriptCnt));
	}

	private IEnumerator Zoom(int cnt, ScenarioDefine.BG_EFFECT_TYPE et)
	{
		do
		{
			yield return null;
		}
		while (!this.bgImageDisp || !this.bgMaskDisp);
		this.bgZoomType = et;
		this.bgZoomRate = 0f;
		this.bgZoomPos = this.backgroundMat.transform.localPosition;
		this.bgZoomScl = this.backgroundMat.transform.localScale.x;
		while (this.bgZoomRate < 1f && this.mScriptCnt == cnt)
		{
			yield return null;
		}
		if (this.mScriptCnt == cnt)
		{
			this.bNext = true;
			this.mScriptCnt++;
		}
		yield break;
	}

	private IEnumerator Shake(int cnt, ScenarioDefine.BG_EFFECT_TYPE et)
	{
		while (!this.bgImageDisp || !this.bgMaskDisp)
		{
			yield return null;
		}
		this.bgShakeType = et;
		this.bgShakePwr = this.BgShakePower;
		while (this.bgShakeType == ScenarioDefine.BG_EFFECT_TYPE.SHAKE_ONCE && this.mScriptCnt == cnt)
		{
			yield return null;
		}
		if (this.mScriptCnt == cnt)
		{
			this.bNext = true;
			this.mScriptCnt++;
		}
		yield break;
	}

	private IEnumerator BgDisp(int cnt)
	{
		while (!this.bgImageDisp || !this.bgMaskDisp)
		{
			yield return null;
		}
		if (this.mScriptCnt == cnt)
		{
			this.bNext = true;
			this.mScriptCnt++;
		}
		yield break;
	}

	private void DoOrderSelect(ScenarioScriptData.ScenarioRowData data)
	{
		string text = this.CheckSerif(data.mStrParams[0]);
		string text2 = this.CheckSerif(data.mStrParams[1]);
		this.GUIs.mGuiSelect.gameObject.SetActive(true);
		bool flag = 0f < data.mFloatParams[0];
		this.GUIs.mGuiSelect.SetSelectLabel(text, text2, flag);
		this.GUIs.mGuiSelect.GetComponent<SimpleAnimation>().ExPlayAnimation(flag ? SimpleAnimation.ExPguiStatus.LOOP : SimpleAnimation.ExPguiStatus.START, null);
		this.GUIs.mGuiSelect.mSelectedRow = -1;
		this.waitParam.ResetParam();
		this.waitParam.jumpRows[0] = data.mIntParams[2];
		this.waitParam.jumpRows[1] = data.mIntParams[3];
		this.waitParam.jumpText[0] = text;
		this.waitParam.jumpText[1] = text2;
	}

	private void DoOrderJump(ScenarioScriptData.ScenarioRowData data)
	{
		this.mScriptCnt = data.mIntParams[1];
		this.bNext = true;
	}

	private void DoOrderLebel(ScenarioScriptData.ScenarioRowData data)
	{
		this.bNext = true;
		this.mScriptCnt++;
	}

	private void DoOrderCharCtrl(ScenarioScriptData.ScenarioRowData data)
	{
		this.OrderCharacter(data);
		this.mCharaCtrl = this.mScriptCnt;
	}

	private void DoOrderEffect(ScenarioScriptData.ScenarioRowData data)
	{
		this.bNext = true;
		switch (data.mIntParams[0])
		{
		case 1:
			this.mScenarioCamera.GetComponent<FocusLine>().enabled = true;
			break;
		case 2:
		case 7:
		{
			this.mScenarioCamera.GetComponent<CC_Vintage>().enabled = true;
			CC_ContrastVignette component = this.mScenarioCamera.GetComponent<CC_ContrastVignette>();
			component.enabled = true;
			ScenarioDefine.ContrastVignette contrastVignette = ((data.mIntParams[0] == 2) ? ScenarioDefine.cv_recollection : ScenarioDefine.cv_dirtcloud);
			component.sharpness = contrastVignette.sharpness;
			component.darkness = contrastVignette.darkness;
			component.contrast = contrastVignette.contrast;
			component.redCoeff = contrastVignette.redCoeff;
			component.greenCoeff = contrastVignette.greenCoeff;
			component.blueCoeff = contrastVignette.blueCoeff;
			component.edge = contrastVignette.edge;
			component.redAmbient = contrastVignette.redAmbient;
			component.greenAmbient = contrastVignette.greenAmbient;
			component.blueAmbient = contrastVignette.blueAmbient;
			break;
		}
		case 3:
			this.bNext = false;
			base.StartCoroutine(this.Flash(this.mScriptCnt));
			break;
		case 4:
			if (this.effectStage != null)
			{
				EffectManager.DestroyEffect(this.effectStage);
			}
			this.effectStage = EffectManager.InstantiateEffect(ScenarioScene.effectSandstarName, this.root.Find("Model"), 1, 1f);
			this.effectStage.effectObject.transform.localPosition = EffectManager.BillboardCamera.transform.localPosition + new Vector3(0f, -1f, 0f);
			this.effectStage.PlayEffect(false);
			break;
		case 5:
			if (this.effectStage != null)
			{
				EffectManager.DestroyEffect(this.effectStage);
			}
			this.effectStage = EffectManager.InstantiateEffect(ScenarioScene.effectShineName, this.root.Find("Model"), 1, 1f);
			this.effectStage.effectObject.transform.localPosition = EffectManager.BillboardCamera.transform.localPosition + new Vector3(0f, -1f, 0f);
			this.effectStage.PlayEffect(false);
			break;
		case 6:
			if (this.effectStage != null)
			{
				EffectManager.DestroyEffect(this.effectStage);
			}
			this.effectStage = EffectManager.InstantiateEffect(ScenarioScene.effectRainName, this.root.Find("Model"), 1, 1f);
			this.effectStage.effectObject.transform.localPosition = EffectManager.BillboardCamera.transform.localPosition;
			this.effectStage.PlayEffect(false);
			break;
		case 8:
			if (this.effectStage != null)
			{
				EffectManager.DestroyEffect(this.effectStage);
			}
			this.effectStage = EffectManager.InstantiateEffect(ScenarioScene.effectSteamName, this.root.Find("Model"), 1, 1f);
			this.effectStage.effectObject.SetLayerRecursively(LayerMask.NameToLayer("FieldEffect"));
			this.effectStage.effectObject.transform.localPosition = EffectManager.BillboardCamera.transform.localPosition;
			this.effectStage.PlayEffect(false);
			break;
		case 9:
			if (this.effectStage != null)
			{
				EffectManager.DestroyEffect(this.effectStage);
			}
			this.effectStage = EffectManager.InstantiateEffect(ScenarioScene.effectSnowName, this.root.Find("Model"), 1, 1f);
			this.effectStage.effectObject.transform.localPosition = EffectManager.BillboardCamera.transform.localPosition;
			this.effectStage.PlayEffect(false);
			break;
		case 10:
			if (this.effectStage != null)
			{
				EffectManager.DestroyEffect(this.effectStage);
			}
			this.effectStage = EffectManager.InstantiateEffect(ScenarioScene.effectDarknessName, this.root.Find("Model"), 1, 1f);
			this.effectStage.effectObject.transform.localPosition = EffectManager.BillboardCamera.transform.localPosition;
			this.effectStage.PlayEffect(false);
			break;
		case 11:
			if (this.effectStage != null)
			{
				EffectManager.DestroyEffect(this.effectStage);
			}
			this.effectStage = EffectManager.InstantiateEffect(ScenarioScene.effectSpeedLineUpName, this.root.Find("Model"), 1, 1f);
			this.effectStage.effectObject.transform.localPosition = EffectManager.BillboardCamera.transform.localPosition;
			this.effectStage.PlayEffect(false);
			break;
		case 12:
			if (this.effectStage != null)
			{
				EffectManager.DestroyEffect(this.effectStage);
			}
			this.effectStage = EffectManager.InstantiateEffect(ScenarioScene.effectSpeedLineDownName, this.root.Find("Model"), 1, 1f);
			this.effectStage.effectObject.transform.localPosition = EffectManager.BillboardCamera.transform.localPosition;
			this.effectStage.PlayEffect(false);
			break;
		case 13:
			if (this.effectStage != null)
			{
				EffectManager.DestroyEffect(this.effectStage);
			}
			this.effectStage = EffectManager.InstantiateEffect(ScenarioScene.effectSpeedLineLeftName, this.root.Find("Model"), 1, 1f);
			this.effectStage.effectObject.transform.localPosition = EffectManager.BillboardCamera.transform.localPosition;
			this.effectStage.PlayEffect(false);
			break;
		case 14:
			if (this.effectStage != null)
			{
				EffectManager.DestroyEffect(this.effectStage);
			}
			this.effectStage = EffectManager.InstantiateEffect(ScenarioScene.effectSpeedLineRightName, this.root.Find("Model"), 1, 1f);
			this.effectStage.effectObject.transform.localPosition = EffectManager.BillboardCamera.transform.localPosition;
			this.effectStage.PlayEffect(false);
			break;
		default:
			this.mScenarioCamera.GetComponent<FocusLine>().enabled = false;
			this.mScenarioCamera.GetComponent<CC_Vintage>().enabled = false;
			this.mScenarioCamera.GetComponent<CC_ContrastVignette>().enabled = false;
			if (this.effectStage != null)
			{
				EffectManager.DestroyEffect(this.effectStage);
				this.effectStage = null;
			}
			break;
		}
		if (this.bNext)
		{
			this.mScriptCnt++;
		}
	}

	private IEnumerator Flash(int cnt)
	{
		CC_BrightnessContrastGamma bcg = this.mScenarioCamera.GetComponent<CC_BrightnessContrastGamma>();
		bcg.enabled = true;
		float r = 0f;
		while ((r += TimeManager.DeltaTime * 5f) < 1f)
		{
			this.FlashParam(bcg, r);
			yield return null;
		}
		this.FlashParam(bcg, 1f);
		r = 0f;
		while ((r += TimeManager.DeltaTime * 10f) < 1f)
		{
			yield return null;
		}
		r = 1f;
		while ((r -= TimeManager.DeltaTime * 2.5f) > 0f)
		{
			this.FlashParam(bcg, r);
			yield return null;
		}
		bcg.enabled = false;
		if (this.mScriptCnt == cnt)
		{
			this.bNext = true;
			this.mScriptCnt++;
		}
		yield break;
	}

	private void FlashParam(CC_BrightnessContrastGamma bcg, float rate)
	{
		bcg.brightness = Mathf.Lerp(0f, this.bcg_brightness, rate);
		bcg.contrast = Mathf.Lerp(0f, this.bcg_contrast, rate);
		bcg.gamma = Mathf.Lerp(1f, this.bcg_gamma, rate);
	}

	private void DoOrderAllFade(ScenarioScriptData.ScenarioRowData data)
	{
		ScenarioDefine.FADE_TYPE fade_TYPE = (ScenarioDefine.FADE_TYPE)data.mIntParams[1];
		if (fade_TYPE == ScenarioDefine.FADE_TYPE.SMOKE || fade_TYPE == ScenarioDefine.FADE_TYPE.SOILSMOKE)
		{
			if (this.effectSmoke != null)
			{
				EffectManager.DestroyEffect(this.effectSmoke);
			}
			this.effectSmoke = EffectManager.InstantiateEffect((fade_TYPE == ScenarioDefine.FADE_TYPE.SMOKE) ? ScenarioScene.effectSmokeName : ScenarioScene.effectSoilSmokeName, this.root.Find("Model"), 1, 1f);
			this.effectSmoke.effectObject.SetLayerRecursively(LayerMask.NameToLayer("FieldEffect"));
			this.effectSmoke.effectObject.transform.localPosition = EffectManager.BillboardCamera.transform.localPosition;
			this.effectSmoke.PlayEffect(false);
			this.ResetGUIShows();
			this.bNext = true;
			this.mScriptCnt++;
			return;
		}
		bool flag = false;
		this.GUIs.mGuiPlyBtns.SetFade(data.mIntParams[0] != 0, fade_TYPE, flag);
		if (fade_TYPE == ScenarioDefine.FADE_TYPE.KEMONO_FADE)
		{
			this.ResetGUIShows();
			this.bNext = true;
			this.mScriptCnt++;
			return;
		}
		base.StartCoroutine(this.Fade(this.mScriptCnt));
	}

	private IEnumerator Fade(int cnt)
	{
		while (!this.GUIs.mGuiPlyBtns.IsFadeEnd())
		{
			yield return null;
		}
		if (this.mScriptCnt == cnt)
		{
			this.bNext = true;
			this.mScriptCnt++;
		}
		yield break;
	}

	private void DoOrderIntroduce(ScenarioScriptData.ScenarioRowData data)
	{
		this.bIntroduce = true;
		this.introCharaId = data.mSerifCharaID;
		this.introPhase = ScenarioScene.INTRODECE_PHASE.CHARA_ALL_HIDE;
		string text = data.mStrParams[0];
		string text2 = "";
		int num = text.IndexOf("::");
		if (num >= 0)
		{
			text2 = text.Substring(num + 2);
			text = text.Substring(0, num);
		}
		string text3 = "";
		string text4 = "";
		ScenarioScene.friend friend = this.mFriendList[this.introCharaId];
		CharaStaticData charaStaticData = ((friend.obj == null) ? null : DataManager.DmChara.GetCharaStaticData(int.Parse(friend.obj.name.Substring(8))));
		string text5;
		if (charaStaticData == null)
		{
			text5 = "【" + this.mFriendList[this.introCharaId].mName + "】";
		}
		else
		{
			text5 = charaStaticData.baseData.charaName;
			text3 = charaStaticData.baseData.charaNameEng;
			text4 = charaStaticData.baseData.eponymName;
		}
		this.nameTelop.transform.Find("Txt_CharaKind").GetComponent<Text>().text = (string.IsNullOrEmpty(text2) ? text4 : text2);
		this.nameTelop.transform.Find("Txt_CharaName").GetComponent<Text>().text = text5;
		this.nameTelop.transform.Find("Txt_CharaName_Eng").GetComponent<Text>().text = text3;
		this.bgTelop.transform.Find("Auth_Telop_Back").Find("ColorBg").GetComponent<Image>()
			.color = ScenarioDefine.GetIntroColor(data.mIntParams[0], text);
		Color introColor = ScenarioDefine.GetIntroColor(data.mIntParams[1], data.mStrParams[1]);
		this.nameTelop.transform.Find("Txt_CharaKind").GetComponent<Text>().color = introColor;
		this.nameTelop.transform.Find("Txt_CharaName").GetComponent<Text>().color = introColor;
		this.nameTelop.transform.Find("Txt_CharaName_Eng").GetComponent<Text>().color = introColor;
		this.nameTelop.transform.Find("Img_No").GetComponent<Image>().color = ScenarioDefine.GetIntroColor(data.mIntParams[2], data.mStrParams[2]);
	}

	private void DoOrderSerifOff(ScenarioScriptData.ScenarioRowData data)
	{
		this.ResetGUIShows();
		this.bNext = true;
		this.mScriptCnt++;
	}

	private void DoOrderWait(ScenarioScriptData.ScenarioRowData data)
	{
		base.StartCoroutine(this.Wait(this.mScriptCnt, (float)data.mIntParams[0]));
	}

	private IEnumerator Wait(int cnt, float wait)
	{
		while ((wait -= TimeManager.DeltaTime) > 0f)
		{
			yield return null;
		}
		if (this.mScriptCnt == cnt)
		{
			this.bNext = true;
			this.mScriptCnt++;
		}
		yield break;
	}

	private void DoOrderWindow(ScenarioScriptData.ScenarioRowData data)
	{
		base.StartCoroutine(this.Window(this.mScriptCnt, data.mStrParams[0]));
	}

	private IEnumerator Window(int cnt, string img)
	{
		this.GUIs.mKomado.SetActive(true);
		bool load = false;
		this.GUIs.mKomado.transform.Find("Texture").GetComponent<PguiRawImageCtrl>().SetRawImage("Texture2D/ScenarioImage/" + img, true, false, delegate
		{
			load = true;
		});
		SimpleAnimation sa = this.GUIs.mKomado.GetComponent<SimpleAnimation>();
		sa.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
		while (sa.ExIsPlaying())
		{
			yield return null;
		}
		while (!load)
		{
			yield return null;
		}
		this.bClickWindow = true;
		while (this.bClickWindow)
		{
			yield return null;
		}
		sa.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, null);
		while (sa.ExIsPlaying())
		{
			yield return null;
		}
		this.GUIs.mKomado.SetActive(false);
		if (this.mScriptCnt == cnt)
		{
			this.bNext = true;
			this.mScriptCnt++;
		}
		yield break;
	}

	private void DoOrderNarration(ScenarioScriptData.ScenarioRowData data)
	{
		if (!this.GUIs.mNarration.activeSelf)
		{
			this.GUIs.mNarration.SetActive(true);
			this.GUIs.mNarration.GetComponent<SimpleAnimation>().ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
		}
		this.GUIs.mNarrationText.SetCurrentText(this.CheckSerif(data.mStrParams[0]), 0, this.writerSpeed);
		this.logList.Add(new KeyValuePair<string, string>("NARRATION", this.CheckSerif(data.mStrParams[0])));
		this.mNarrationCtrl = this.mScriptCnt;
	}

	private void DoOrderPhoto(ScenarioScriptData.ScenarioRowData data)
	{
		base.StartCoroutine(this.Photo(this.mScriptCnt, data.mStrParams[0]));
	}

	private IEnumerator Photo(int cnt, string img)
	{
		this.GUIs.mPhoto.SetActive(true);
		bool load = false;
		this.GUIs.mPhoto.transform.Find("Null_Photo/Texture_Photo").GetComponent<PguiRawImageCtrl>().SetRawImage("Texture2D/Photo/Card_Photo/" + img, true, false, delegate
		{
			load = true;
		});
		SimpleAnimation sa = this.GUIs.mPhoto.GetComponent<SimpleAnimation>();
		sa.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
		while (sa.ExIsPlaying())
		{
			yield return null;
		}
		while (!load)
		{
			yield return null;
		}
		this.bClickPhoto = true;
		while (this.bClickPhoto)
		{
			yield return null;
		}
		sa.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, null);
		while (sa.ExIsPlaying())
		{
			yield return null;
		}
		this.GUIs.mPhoto.SetActive(false);
		if (this.mScriptCnt == cnt)
		{
			this.bNext = true;
			this.mScriptCnt++;
		}
		yield break;
	}

	private void DoOrderBGM(ScenarioScriptData.ScenarioRowData data)
	{
		string text = "";
		if (data.mIntParams[0] < 0)
		{
			text = data.mStrParams[0];
		}
		else if (data.mIntParams[0] > 0 && data.mIntParams[0] < ScenarioDefine.BgmName.Length)
		{
			text = ScenarioDefine.BgmName[data.mIntParams[0]];
		}
		if (string.IsNullOrEmpty(text))
		{
			SoundManager.StopBGM();
		}
		else if (data.mIntParams[1] == 0)
		{
			SoundManager.PlayBGM(text);
		}
		else if (1 == data.mIntParams[1])
		{
			SoundManager.PlayBGM(text, 0, ScenarioDefine.BgmSpeed[data.mIntParams[2]], 0);
		}
		else if (2 == data.mIntParams[1])
		{
			SoundManager.StopBGM(ScenarioDefine.BgmSpeed[data.mIntParams[2]]);
		}
		this.bNext = true;
		this.mScriptCnt++;
	}

	private void DoOrderSE(ScenarioScriptData.ScenarioRowData data)
	{
		string text = "";
		bool flag = false;
		if (data.mIntParams[0] > 0 && data.mIntParams[0] < ScenarioDefine.SeName.Length)
		{
			text = ScenarioDefine.SeName[data.mIntParams[0]];
			flag = ScenarioDefine.loopSE((ScenarioDefine.SE_TYPE)data.mIntParams[0]);
		}
		else if (data.mIntParams[0] < 0 && this.scenarioScriptData.seSheetList != null && this.scenarioScriptData.seSheetList.Count >= -data.mIntParams[0])
		{
			text = this.scenarioScriptData.seSheetList[-1 - data.mIntParams[0]];
			flag = text.EndsWith("_loop") || text.IndexOf("drumroll_start") != -1;
		}
		if (!string.IsNullOrEmpty(text))
		{
			CriAtomExPlayback criAtomExPlayback = SoundManager.Play(text, false, false);
			if (flag)
			{
				if (this.loopSE)
				{
					this.loopSEhdl.Stop();
				}
				this.loopSEhdl = criAtomExPlayback;
				this.loopSE = true;
			}
		}
		else if (this.loopSE)
		{
			this.loopSEhdl.Stop();
			this.loopSE = false;
		}
		this.bNext = true;
		this.mScriptCnt++;
	}

	private void OrderCharacter(ScenarioScriptData.ScenarioRowData data)
	{
		foreach (ScenarioScene.friend friend in this.mFriendList)
		{
			if (friend.bFade != 0)
			{
				friend.charaModelHandle.SetAlpha((friend.bFade > 0) ? 1f : 0f);
			}
			if (friend.bDisappear)
			{
				friend.charaModelHandle.SetAlpha(0f);
			}
			if (Mathf.Abs(friend.bMove) > 2)
			{
				Vector3 localPosition = friend.obj.transform.localPosition;
				localPosition.y = friend.fTagPos;
				friend.obj.transform.localPosition = localPosition;
			}
			else if (friend.bMove != 0)
			{
				Vector3 localPosition2 = friend.obj.transform.localPosition;
				localPosition2.x = friend.fTagPos;
				friend.obj.transform.localPosition = localPosition2;
				Vector3 localEulerAngles = friend.obj.transform.localEulerAngles;
				localEulerAngles.y = friend.fTagRot;
				friend.obj.transform.localEulerAngles = localEulerAngles;
			}
			if (friend.bMotion || friend.bMove != 0)
			{
				if (friend.bMove < 0)
				{
					friend.charaModelHandle.SetAlpha(0f);
				}
				friend.charaModelHandle.PlayAnimation(friend.standby, true, 1f, 0f, 0f, false);
				if (!friend.puyo)
				{
					FacePackData facePackData = FacePackData.Id2PackData(friend.faceId = friend.idleFaceId);
					friend.charaModelHandle.SetFacePackData(facePackData, null, null);
					friend.charaModelHandle.enabledFaceMotion = facePackData == null;
					friend.charaModelHandle.eyeFollowObj = friend.follow;
					friend.charaModelHandle.headFollowObj = friend.follow;
				}
			}
			friend.bFade = 0;
			friend.bMove = 0;
			friend.bDisappear = false;
			friend.bMotion = false;
		}
		for (int i = 0; i < data.ID.Length; i++)
		{
			ScenarioScene.friend friend2 = this.mFriendList[data.ID[i]];
			int num = (friend2.posID = data.mCharaPosition[i]);
			if (num < 0 || num >= this.mCharaPosition.Count)
			{
				num = 0;
			}
			CharaMotionDefine.ActKey actKey = data.mModelMotion[i];
			float num2 = 0.5f;
			if (i < data.mMotionFade.Length)
			{
				switch (data.mMotionFade[i])
				{
				case ScenarioDefine.MOTION_FADE.OFF_NORM:
				case ScenarioDefine.MOTION_FADE.OFF_OFF:
				case ScenarioDefine.MOTION_FADE.OFF_FIRST:
				case ScenarioDefine.MOTION_FADE.OFF_SLOW:
					num2 = 0f;
					break;
				case ScenarioDefine.MOTION_FADE.FIRST_NORM:
				case ScenarioDefine.MOTION_FADE.FIRST_OFF:
				case ScenarioDefine.MOTION_FADE.FIRST_FIRST:
				case ScenarioDefine.MOTION_FADE.FIRST_SLOW:
					num2 = 0.2f;
					break;
				case ScenarioDefine.MOTION_FADE.SLOW_NORM:
				case ScenarioDefine.MOTION_FADE.SLOW_OFF:
				case ScenarioDefine.MOTION_FADE.SLOW_FIRST:
				case ScenarioDefine.MOTION_FADE.SLOW_SLOW:
					num2 = 0.8f;
					break;
				}
			}
			Vector3 position = this.mCharaPosition[num].position;
			if (friend2.charaModelOffset != null)
			{
				position.y = friend2.charaModelOffset.position.y;
			}
			Vector3 rotation = this.mCharaPosition[num].rotation;
			friend2.obj.transform.localScale = ((friend2.charaModelOffset == null) ? this.mCharaPosition[num].scale : friend2.charaModelOffset.scale);
			switch (data.mCharaMove[i])
			{
			case ScenarioDefine.CHARA_MOVE.DISAPPEAR_FADE_OUT:
				friend2.charaModelHandle.FadeOut(0.5f, null);
				friend2.bFade = -1;
				break;
			case ScenarioDefine.CHARA_MOVE.DISAPPEAR_TO_RIGHT:
			case ScenarioDefine.CHARA_MOVE.DISAPPEAR_FAST_RIGHT:
				friend2.bMove = ((data.mCharaMove[i] == ScenarioDefine.CHARA_MOVE.DISAPPEAR_FAST_RIGHT) ? (-2) : (-1));
				friend2.fTagPos = position.x + 2.2f;
				friend2.fTagRot = 90f;
				break;
			case ScenarioDefine.CHARA_MOVE.DISAPPEAR_TO_LEFT:
			case ScenarioDefine.CHARA_MOVE.DISAPPEAR_FAST_LEFT:
				friend2.bMove = ((data.mCharaMove[i] == ScenarioDefine.CHARA_MOVE.DISAPPEAR_FAST_LEFT) ? (-2) : (-1));
				friend2.fTagPos = position.x - 2.2f;
				friend2.fTagRot = -90f;
				break;
			case ScenarioDefine.CHARA_MOVE.DISAPPEAR_TO_UPWARD:
				actKey = CharaMotionDefine.ActKey.BIRD_OUT;
				friend2.bDisappear = true;
				break;
			case ScenarioDefine.CHARA_MOVE.DISAPPEAR_TO_DOWNWARD:
				friend2.bMove = -3;
				friend2.fTagPos = position.y - 1.6f;
				friend2.fTagRot = 0f;
				break;
			case ScenarioDefine.CHARA_MOVE.APPEAR_FADE_IN:
				friend2.charaModelHandle.FadeIn(0.5f);
				friend2.bFade = 1;
				num2 = 0f;
				break;
			case ScenarioDefine.CHARA_MOVE.APPEAR_FROM_RIGHT:
			case ScenarioDefine.CHARA_MOVE.APPEAR_FAST_RIGHT:
				friend2.charaModelHandle.FadeIn(0.1f);
				friend2.bMove = ((data.mCharaMove[i] == ScenarioDefine.CHARA_MOVE.APPEAR_FAST_RIGHT) ? 2 : 1);
				friend2.fTagPos = position.x;
				position.x += 2.2f;
				friend2.fTagRot = rotation.y;
				rotation.y = -90f;
				num2 = 0f;
				break;
			case ScenarioDefine.CHARA_MOVE.APPEAR_FROM_LEFT:
			case ScenarioDefine.CHARA_MOVE.APPEAR_FAST_LEFT:
				friend2.charaModelHandle.FadeIn(0.1f);
				friend2.bMove = ((data.mCharaMove[i] == ScenarioDefine.CHARA_MOVE.APPEAR_FAST_LEFT) ? 2 : 1);
				friend2.fTagPos = position.x;
				position.x -= 2.2f;
				friend2.fTagRot = rotation.y;
				rotation.y = 90f;
				num2 = 0f;
				break;
			case ScenarioDefine.CHARA_MOVE.APPEAR_FROM_UPWARD:
				friend2.charaModelHandle.FadeIn(0.1f);
				actKey = CharaMotionDefine.ActKey.BIRD_IN;
				num2 = 0f;
				friend2.bMotion = true;
				break;
			case ScenarioDefine.CHARA_MOVE.APPEAR_FROM_DOWNWARD:
				friend2.charaModelHandle.FadeIn(0.1f);
				friend2.bMove = 3;
				friend2.fTagPos = position.y;
				position.y -= 1.6f;
				num2 = 0f;
				break;
			case ScenarioDefine.CHARA_MOVE.DISAPPEAR_JUMP_OUT:
				actKey = CharaMotionDefine.ActKey.JUMP_OUT;
				friend2.bDisappear = true;
				break;
			case ScenarioDefine.CHARA_MOVE.APPEAR_JUMP_IN:
				friend2.charaModelHandle.FadeIn(0.05f);
				actKey = CharaMotionDefine.ActKey.JUMP_IN;
				num2 = 0f;
				friend2.bMotion = true;
				break;
			}
			friend2.obj.transform.localPosition = position;
			friend2.obj.transform.localEulerAngles = rotation;
			if (actKey == CharaMotionDefine.ActKey.INVALID)
			{
				actKey = friend2.standby;
			}
			if (!friend2.charaModelHandle.IsCurrentAnimation(actKey) || !friend2.charaModelHandle.IsLoopAnimation())
			{
				int num3 = Mathf.Abs(friend2.bMove);
				friend2.charaModelHandle.PlayAnimation(actKey, num3 >= 1 && num3 <= 2, 1f, num2, num2, false);
				friend2.standby = this.get_standby(actKey);
				friend2.standbyCF = 0.5f;
				if (i < data.mMotionFade.Length)
				{
					switch (data.mMotionFade[i])
					{
					case ScenarioDefine.MOTION_FADE.NORM_OFF:
					case ScenarioDefine.MOTION_FADE.OFF_OFF:
					case ScenarioDefine.MOTION_FADE.FIRST_OFF:
					case ScenarioDefine.MOTION_FADE.SLOW_OFF:
						friend2.standbyCF = 0f;
						break;
					case ScenarioDefine.MOTION_FADE.NORM_FIRST:
					case ScenarioDefine.MOTION_FADE.OFF_FIRST:
					case ScenarioDefine.MOTION_FADE.FIRST_FIRST:
					case ScenarioDefine.MOTION_FADE.SLOW_FIRST:
						friend2.standbyCF = 0.2f;
						break;
					case ScenarioDefine.MOTION_FADE.NORM_SLOW:
					case ScenarioDefine.MOTION_FADE.OFF_SLOW:
					case ScenarioDefine.MOTION_FADE.FIRST_SLOW:
					case ScenarioDefine.MOTION_FADE.SLOW_SLOW:
						friend2.standbyCF = 0.8f;
						break;
					}
				}
			}
			if (friend2.puyo)
			{
				friend2.charaModelHandle.SetPuyoTex("Texture2D/ScenarioImage/" + data.mModelFaceId[i]);
			}
			else
			{
				friend2.faceId = data.mModelFaceId[i];
				friend2.idleFaceId = ((i < data.mIdleFaceId.Length) ? data.mIdleFaceId[i] : "INVALID");
				FacePackData facePackData2 = FacePackData.Id2PackData(friend2.faceId);
				friend2.charaModelHandle.SetFacePackData(facePackData2, null, null);
				friend2.charaModelHandle.enabledFaceMotion = facePackData2 == null;
				friend2.follow = (data.mCharaFaceRot[i] ? this.mFollow[num] : null);
				friend2.charaModelHandle.eyeFollowObj = friend2.follow;
				friend2.charaModelHandle.headFollowObj = friend2.follow;
			}
			if (friend2.mEffData != null)
			{
				EffectManager.DestroyEffect(friend2.mEffData);
				friend2.mEffData = null;
			}
			if (data.mCharaEffect.Length != 0 && data.mCharaEffect[i] - 1 >= 0)
			{
				friend2.mEffData = EffectManager.InstantiateEffect(this.mLoadEffect[data.mCharaEffect[i] - 1], friend2.mEff.transform, 1, 1f);
				friend2.mEffData.PlayEffect(false);
			}
		}
	}

	private CharaMotionDefine.ActKey get_standby(CharaMotionDefine.ActKey mot)
	{
		if (mot == CharaMotionDefine.ActKey.WEAPON_TAKE || mot == CharaMotionDefine.ActKey.WEAPON_STAND_BY || mot == CharaMotionDefine.ActKey.WEAPON_ATTACK)
		{
			mot = CharaMotionDefine.ActKey.WEAPON_STAND_BY;
		}
		else if (mot == CharaMotionDefine.ActKey.SING_ST || mot == CharaMotionDefine.ActKey.SING_LP)
		{
			mot = CharaMotionDefine.ActKey.SING_LP;
		}
		else
		{
			mot = CharaMotionDefine.ActKey.SCENARIO_STAND_BY;
		}
		return mot;
	}

	private void CharacterUpdate()
	{
		bool flag = true;
		foreach (ScenarioScene.friend friend in this.mFriendList)
		{
			if (friend.bDisappear)
			{
				if (friend.charaModelHandle.IsPlaying())
				{
					flag = false;
					return;
				}
				friend.charaModelHandle.PlayAnimation(friend.standby, true, 1f, 0f, 0f, false);
				friend.charaModelHandle.SetAlpha(0f);
				friend.bDisappear = false;
				return;
			}
			else
			{
				bool flag2 = friend.charaModelHandle.IsPlaying();
				if (flag2 && !friend.charaModelHandle.IsLoopAnimation() && (1f - friend.charaModelHandle.GetAnimationTime(null)) * friend.charaModelHandle.GetAnimationLength(null) < friend.standbyCF)
				{
					flag2 = false;
				}
				if (flag2)
				{
					if (friend.charaModelHandle.IsLoopAnimation())
					{
						friend.bMotion = false;
					}
					else if (friend.bMotion)
					{
						flag = false;
					}
				}
				else if (!friend.charaModelHandle.IsLoopAnimation() || !friend.charaModelHandle.IsCurrentAnimation(friend.standby))
				{
					friend.charaModelHandle.PlayAnimation(friend.standby, true, 1f, friend.standbyCF, friend.standbyCF, false);
					FacePackData facePackData = FacePackData.Id2PackData(friend.faceId = friend.idleFaceId);
					friend.charaModelHandle.SetFacePackData(facePackData, null, null);
					friend.charaModelHandle.enabledFaceMotion = facePackData == null;
					friend.charaModelHandle.eyeFollowObj = friend.follow;
					friend.charaModelHandle.headFollowObj = friend.follow;
					friend.bMotion = false;
				}
				else
				{
					friend.bMotion = false;
				}
				int num = Mathf.Abs(friend.bMove);
				if (num > 2)
				{
					Vector3 localPosition = friend.obj.transform.localPosition;
					if (friend.bMove > 0)
					{
						friend.fTagRot = (friend.fTagPos - localPosition.y) / 5f;
						localPosition.y += friend.fTagRot;
						if (!friend.charaModelHandle.IsLoopAnimation())
						{
							flag = false;
						}
					}
					else if (friend.charaModelHandle.IsLoopAnimation() || friend.charaModelHandle.GetAnimationTime(null) * friend.charaModelHandle.GetAnimationLength(null) >= 0.5f)
					{
						float num2 = TimeManager.DeltaTime;
						friend.fTagRot += num2 * 12f;
						num2 *= friend.fTagRot;
						if ((localPosition.y -= num2) > friend.fTagPos)
						{
							flag = false;
						}
					}
					else
					{
						flag = false;
					}
					if (flag)
					{
						localPosition.y = friend.fTagPos;
						if (friend.bMove < 0)
						{
							friend.charaModelHandle.SetAlpha(0f);
						}
						friend.bMove = 0;
						friend.fTagPos = 0f;
						friend.fTagRot = 0f;
					}
					friend.obj.transform.localPosition = localPosition;
				}
				else if (friend.bMove != 0)
				{
					bool flag3 = true;
					Vector3 localEulerAngles = friend.obj.transform.localEulerAngles;
					float num3 = TimeManager.DeltaTime * 300f;
					if (num > 1)
					{
						num3 *= 5f;
					}
					if (friend.bMove < 0)
					{
						if (Mathf.DeltaAngle(localEulerAngles.y, friend.fTagRot) > 0f)
						{
							if (Mathf.DeltaAngle(localEulerAngles.y += num3, friend.fTagRot) > 0f)
							{
								flag3 = false;
							}
						}
						else if (Mathf.DeltaAngle(localEulerAngles.y -= num3, friend.fTagRot) < 0f)
						{
							flag3 = false;
						}
						if (flag3)
						{
							localEulerAngles.y = friend.fTagRot;
						}
					}
					if (flag3)
					{
						Vector3 localPosition2 = friend.obj.transform.localPosition;
						float num4 = TimeManager.DeltaTime * 1.2f;
						if (num > 1)
						{
							num4 *= 5f;
						}
						if (friend.fTagPos > localPosition2.x)
						{
							if ((localPosition2.x += num4) < friend.fTagPos)
							{
								flag3 = false;
							}
						}
						else if ((localPosition2.x -= num4) > friend.fTagPos)
						{
							flag3 = false;
						}
						if (flag3)
						{
							localPosition2.x = friend.fTagPos;
							if (friend.bMove > 0)
							{
								if (Mathf.DeltaAngle(localEulerAngles.y, friend.fTagRot) > 0f)
								{
									if (Mathf.DeltaAngle(localEulerAngles.y += num3, friend.fTagRot) > 0f)
									{
										flag3 = false;
									}
								}
								else if (Mathf.DeltaAngle(localEulerAngles.y -= num3, friend.fTagRot) < 0f)
								{
									flag3 = false;
								}
								if (flag3)
								{
									localEulerAngles.y = friend.fTagRot;
								}
							}
							if (flag3)
							{
								if (friend.bMove < 0)
								{
									friend.charaModelHandle.SetAlpha(0f);
								}
								friend.bMove = 0;
								friend.fTagPos = 0f;
								friend.fTagRot = 0f;
								friend.charaModelHandle.PlayAnimation(friend.standby, true, 1f, friend.standbyCF, friend.standbyCF, false);
								FacePackData facePackData2 = FacePackData.Id2PackData(friend.faceId = friend.idleFaceId);
								friend.charaModelHandle.SetFacePackData(facePackData2, null, null);
								friend.charaModelHandle.enabledFaceMotion = facePackData2 == null;
								friend.charaModelHandle.eyeFollowObj = friend.follow;
								friend.charaModelHandle.headFollowObj = friend.follow;
							}
							else
							{
								flag = false;
							}
						}
						else
						{
							flag = false;
						}
						friend.obj.transform.localPosition = localPosition2;
					}
					else
					{
						flag = false;
					}
					friend.obj.transform.localEulerAngles = localEulerAngles;
				}
				if (friend.bFade > 0)
				{
					if (friend.charaModelHandle.GetAlpha() < 0.99f)
					{
						flag = false;
					}
				}
				else if (friend.bFade < 0 && friend.charaModelHandle.GetAlpha() > 0.01f)
				{
					flag = false;
				}
			}
		}
		if ((flag || this.bClickScreen) && this.mCharaCtrl == this.mScriptCnt)
		{
			this.bNext = true;
			this.mScriptCnt++;
		}
	}

	private void UpdateIntroduce(ScenarioScriptData.ScenarioRowData data)
	{
		switch (this.introPhase)
		{
		case ScenarioScene.INTRODECE_PHASE.CHARA_ALL_HIDE:
			this.introFriends = new List<CharaModelHandle>();
			foreach (ScenarioScene.friend friend in this.mFriendList)
			{
				if (friend.charaModelHandle.IsDisp())
				{
					this.introFriends.Add(friend.charaModelHandle);
					friend.charaModelHandle.FadeOut(0.5f, null);
					if (friend.mEffData != null)
					{
						EffectManager.DestroyEffect(friend.mEffData);
						friend.mEffData = null;
					}
				}
			}
			this.ResetGUIShows();
			this.introPhase = ScenarioScene.INTRODECE_PHASE.SHOW_INTRODUCE_CHARA;
			return;
		case ScenarioScene.INTRODECE_PHASE.SHOW_INTRODUCE_CHARA:
			if ((double)this.introTime >= 0.5)
			{
				ScenarioScene.friend friend2 = this.mFriendList[this.introCharaId];
				this.introCharaPos = friend2.obj.transform.localPosition;
				this.introCharaRot = friend2.obj.transform.localEulerAngles;
				this.introCharaScl = friend2.obj.transform.localScale;
				this.introFaceId = friend2.faceId;
				this.introIdleFaceId = friend2.idleFaceId;
				if (Mathf.Abs(friend2.bMove) > 2)
				{
					this.introCharaPos.y = friend2.fTagPos;
				}
				else if (friend2.bMove != 0)
				{
					this.introCharaPos.x = friend2.fTagPos;
					this.introCharaRot.y = friend2.fTagRot;
				}
				friend2.charaModelHandle.FadeIn(0.5f);
				friend2.charaModelHandle.PlayAnimation(data.mModelMotion[0], false, 1f, 0f, 0f, false);
				friend2.standby = this.get_standby(data.mModelMotion[0]);
				friend2.standbyCF = 0.5f;
				Vector3 position = this.mCharaPosition[5].position;
				if (friend2.charaModelOffset != null)
				{
					position.y = friend2.charaModelOffset.position.y;
				}
				friend2.obj.transform.localPosition = position;
				friend2.obj.transform.localEulerAngles = this.mCharaPosition[5].rotation;
				friend2.obj.transform.localScale = ((friend2.charaModelOffset == null) ? this.mCharaPosition[5].scale : friend2.charaModelOffset.scale);
				friend2.bFade = 0;
				friend2.bMove = 0;
				friend2.bDisappear = false;
				friend2.bMotion = false;
				friend2.faceId = data.mModelFaceId[0];
				FacePackData facePackData = FacePackData.Id2PackData(friend2.faceId);
				friend2.charaModelHandle.SetFacePackData(facePackData, null, null);
				friend2.idleFaceId = ((data.mIdleFaceId.Length != 0) ? data.mIdleFaceId[0] : "INVALID");
				friend2.charaModelHandle.enabledFaceMotion = facePackData == null;
				this.introPhase = ScenarioScene.INTRODECE_PHASE.SHOW_TELOP;
				return;
			}
			this.introTime += TimeManager.DeltaTime;
			return;
		case ScenarioScene.INTRODECE_PHASE.SHOW_TELOP:
			this.nameTelop.SetActive(true);
			this.nameTelop.GetComponent<SimpleAnimation>().ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
			this.bgTelop.transform.Find("Auth_Telop_Back").gameObject.SetActive(true);
			this.bgTelop.transform.Find("Auth_Telop_Back").GetComponent<SimpleAnimation>().ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
			this.bgTelop.transform.Find("Auth_Telop_Back/Pattern_No").GetComponent<SimpleAnimation>().ExPlayAnimation(SimpleAnimation.ExPguiStatus.LOOP, null);
			this.introPhase = ScenarioScene.INTRODECE_PHASE.WAIT_USER_CLICK;
			this.bClickScreen = false;
			return;
		case ScenarioScene.INTRODECE_PHASE.WAIT_USER_CLICK:
			if (this.bClickScreen)
			{
				this.introPhase = ScenarioScene.INTRODECE_PHASE.HIDE_INTRODUCE_CHARA;
				this.introTime = 0f;
				return;
			}
			break;
		case ScenarioScene.INTRODECE_PHASE.HIDE_INTRODUCE_CHARA:
			this.mFriendList[this.introCharaId].charaModelHandle.FadeOut(0.5f, null);
			this.introPhase = ScenarioScene.INTRODECE_PHASE.BACK_ORIGINAL_STATE;
			return;
		case ScenarioScene.INTRODECE_PHASE.BACK_ORIGINAL_STATE:
			if (this.introTime >= 0.5f)
			{
				ScenarioScene.friend friend3 = this.mFriendList[this.introCharaId];
				friend3.obj.transform.localPosition = this.introCharaPos;
				friend3.obj.transform.localEulerAngles = this.introCharaRot;
				friend3.obj.transform.localScale = this.introCharaScl;
				friend3.charaModelHandle.PlayAnimation(friend3.standby, true, 1f, 0f, 0f, false);
				friend3.faceId = this.introFaceId;
				friend3.idleFaceId = this.introIdleFaceId;
				FacePackData facePackData2 = FacePackData.Id2PackData(friend3.faceId);
				friend3.charaModelHandle.SetFacePackData(facePackData2, null, null);
				friend3.charaModelHandle.enabledFaceMotion = facePackData2 == null;
				friend3.charaModelHandle.eyeFollowObj = friend3.follow;
				friend3.charaModelHandle.headFollowObj = friend3.follow;
				this.nameTelop.GetComponent<SimpleAnimation>().ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, null);
				this.bgTelop.transform.Find("Auth_Telop_Back").GetComponent<SimpleAnimation>().ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, null);
				foreach (CharaModelHandle charaModelHandle in this.introFriends)
				{
					charaModelHandle.FadeIn(0.5f);
				}
				this.introPhase++;
				this.introTime = 0f;
				return;
			}
			this.introTime += TimeManager.DeltaTime;
			return;
		default:
			if ((this.introTime += TimeManager.DeltaTime) > 0.5f)
			{
				this.bIntroduce = false;
				this.introTime = 0f;
				this.bNext = true;
				this.mScriptCnt++;
				this.bgTelop.transform.Find("Auth_Telop_Back").gameObject.SetActive(false);
				this.nameTelop.SetActive(false);
			}
			break;
		}
	}

	private void UpdateAuth()
	{
		switch (this.authPhase)
		{
		case ScenarioScene.AUTH_PHASE.INITIALISE:
			this.authPlayer = AuthPlayer.InstantiateAuthPlayer(base.transform, false);
			this.authLoading = TimeManager.SystemNow;
			this.authPlayer.Initialize(this.authName, null, null, null, null, false, false, false);
			this.authPhase = ScenarioScene.AUTH_PHASE.WAIT;
			return;
		case ScenarioScene.AUTH_PHASE.WAIT:
			if (this.authPlayer.IsFinishInitialize())
			{
				this.root.gameObject.SetActive(false);
				Light[] array = this.light;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].gameObject.SetActive(false);
				}
				this.authPlayer.PlayAuth(false);
				this.authPhase = ScenarioScene.AUTH_PHASE.PLAY;
				CanvasManager.HdlLoadAndTipsCtrl.SetDispLoading(false);
				return;
			}
			if ((TimeManager.SystemNow - this.authLoading).TotalSeconds > 1.0)
			{
				CanvasManager.HdlLoadAndTipsCtrl.SetDispLoading(true);
				return;
			}
			break;
		case ScenarioScene.AUTH_PHASE.PLAY:
			if (!this.authPlayer.IsPlaying())
			{
				this.authPlayer.DestroyProcessing();
				this.authPlayer.gameObject.SetActive(false);
				this.root.gameObject.SetActive(true);
				Light[] array = this.light;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].gameObject.SetActive(true);
				}
				Object.Destroy(this.authPlayer.gameObject);
				this.authPlayer = null;
				this.authPhase = ScenarioScene.AUTH_PHASE.MAX;
				return;
			}
			break;
		default:
			this.bAuth = false;
			this.bNext = true;
			this.mScriptCnt++;
			break;
		}
	}

	private void UpdateMovie()
	{
		switch (this.moviePhase)
		{
		case ScenarioScene.MOVIE_PHASE.INITIALISE:
		{
			this.moviePlayer = new GameObject("movie", new Type[] { typeof(RectTransform) });
			RectTransform component = this.moviePlayer.GetComponent<RectTransform>();
			component.SetParent(this.root.Find("GUIRoot"), false);
			component.sizeDelta = new Vector2(1280f, 720f);
			this.moviePlayer.AddComponent<RawImage>();
			this.moviePlayer.SetActive(true);
			MoviePlayer.Play(this.moviePlayer, this.movieName, false);
			this.movieLoading = TimeManager.SystemNow;
			this.moviePhase = ScenarioScene.MOVIE_PHASE.WAIT;
			return;
		}
		case ScenarioScene.MOVIE_PHASE.WAIT:
			if (!MoviePlayer.Loading(this.moviePlayer))
			{
				CanvasManager.HdlLoadAndTipsCtrl.SetDispLoading(false);
				this.moviePhase = ScenarioScene.MOVIE_PHASE.PLAY;
				return;
			}
			if ((TimeManager.SystemNow - this.movieLoading).TotalSeconds > 1.0)
			{
				CanvasManager.HdlLoadAndTipsCtrl.SetDispLoading(true);
				return;
			}
			break;
		case ScenarioScene.MOVIE_PHASE.PLAY:
			if (!MoviePlayer.Playing(this.moviePlayer))
			{
				this.moviePlayer.SetActive(false);
				Object.Destroy(this.moviePlayer);
				this.moviePlayer = null;
				this.moviePhase = ScenarioScene.MOVIE_PHASE.MAX;
				return;
			}
			break;
		default:
			this.bMovie = false;
			this.bNext = true;
			this.mScriptCnt++;
			break;
		}
	}

	private void Disable()
	{
		if (this.authPlayer != null)
		{
			Object.Destroy(this.authPlayer.gameObject);
			this.authPlayer = null;
		}
		if (this.moviePlayer != null)
		{
			Object.Destroy(this.moviePlayer);
			this.moviePlayer = null;
		}
		foreach (ScenarioScene.friend friend in this.mFriendList)
		{
			if (friend.mEffData != null)
			{
				EffectManager.DestroyEffect(friend.mEffData);
				friend.mEffData = null;
			}
		}
		this.mFriendList = new List<ScenarioScene.friend>();
		if (this.effectStage != null)
		{
			EffectManager.DestroyEffect(this.effectStage);
			this.effectStage = null;
		}
		if (this.effectSmoke != null)
		{
			EffectManager.DestroyEffect(this.effectSmoke);
			this.effectSmoke = null;
		}
		foreach (string text in this.mUseEffect)
		{
			EffectManager.UnloadEffect(text, AssetManager.OWNER.Scenario);
		}
		this.mUseEffect = new List<string>();
		foreach (string text2 in this.mLoadEffect)
		{
			EffectManager.UnloadEffect(text2, AssetManager.OWNER.Scenario);
		}
		this.mLoadEffect = new List<string>();
		if (this.scenarioScriptData != null)
		{
			Object.Destroy(this.scenarioScriptData);
			this.scenarioScriptData = null;
		}
		if (this.loopSE)
		{
			this.loopSEhdl.Stop();
			this.loopSE = false;
		}
		Screen.sleepTimeout = -2;
	}

	public bool IsFinishLoad()
	{
		return this.status > ScenarioScene.Status.IN;
	}

	public bool IsFinishPlay()
	{
		return this.status >= ScenarioScene.Status.OUT;
	}

	public bool IsFinishScenario()
	{
		return this.status >= ScenarioScene.Status.TERM;
	}

	private void OnDestroy()
	{
		this.Disable();
	}

	private string FixSpecialText(string text)
	{
		text.IndexOf("&#10014;");
		return HttpUtility.HtmlDecode(text);
	}

	private void SetBloomEffect()
	{
		bool flag = false;
		foreach (ScenarioScene.friend friend in this.mFriendList)
		{
			if (!(friend.charaModelHandle == null))
			{
				List<EffectData> charaEffect = friend.charaModelHandle.charaEffect;
				if (charaEffect != null && charaEffect.Count != 0 && charaEffect[0] != null)
				{
					flag = true;
					if (charaEffect[0].effectObject.layer != LayerMask.NameToLayer("Bloom"))
					{
						charaEffect[0].effectObject.SetLayerRecursively(LayerMask.NameToLayer("Bloom"));
					}
					if (friend.charaModelHandle.GetAlpha() < 0.8f)
					{
						this.mScenarioCamera.GetComponent<Camera>().cullingMask &= ~(1 << LayerMask.NameToLayer("Bloom"));
					}
					else
					{
						this.mScenarioCamera.GetComponent<Camera>().cullingMask |= 1 << LayerMask.NameToLayer("Bloom");
					}
				}
			}
		}
		this.mScenarioCamera.GetComponent<MultipleGaussianBloom>().enabled = flag;
		this.mScenarioCamera.GetComponent<FieldAlpha>().enabled = !flag;
	}

	public string scenarioName;

	public int questId;

	public int storyType;

	[SerializeField]
	private PguiRawImageCtrl backgroundMat;

	[SerializeField]
	private Vector3 BgShakePower;

	[SerializeField]
	private Vector3 BgShakeSpeed;

	[SerializeField]
	private float BgZoomScale;

	[SerializeField]
	private float BgZoomTime;

	private ScenarioScene.Status status;

	private Transform root;

	private Light[] light;

	private string scenarioPath;

	private ScenarioScriptData scenarioScriptData;

	public int mScriptCnt;

	private int mCharaCtrl = -1;

	private int mSerifCtrl = -1;

	private int mSerifCharaID = -1;

	private int mNarrationCtrl = -1;

	private bool bClickScreen;

	private bool bClickWindow;

	private bool bClickPhoto;

	private bool bNext = true;

	private bool bAuth;

	private bool bMovie;

	private bool bIntroduce;

	private int introCharaId;

	private ScenarioScene.INTRODECE_PHASE introPhase;

	private float introTime;

	private List<CharaModelHandle> introFriends;

	private Vector3 introCharaPos;

	private Vector3 introCharaRot;

	private Vector3 introCharaScl;

	private string introFaceId;

	private string introIdleFaceId;

	private GameObject mScenarioCamera;

	private List<Transform> mFollow;

	private string authName;

	private ScenarioScene.AUTH_PHASE authPhase;

	private string movieName;

	private ScenarioScene.MOVIE_PHASE moviePhase;

	private List<ScenarioScene.friend> mFriendList = new List<ScenarioScene.friend>();

	private ScenarioSetValues mSetValues;

	private List<ScenarioSetValues.CharaOffset> mCharaOffset;

	private List<ScenarioCharaOffset.CharaPosition> mCharaPosition;

	private List<IEnumerator> mLoadSe = new List<IEnumerator>();

	private List<string> mLoadEffect = new List<string>();

	private List<string> mUseEffect = new List<string>();

	private int modelCnt;

	private ScenarioScene.GUI GUIs;

	private string bgImage;

	private bool bgImageDisp;

	private string bgMask;

	private bool bgMaskDisp;

	private ScenarioDefine.BG_EFFECT_TYPE bgZoomType;

	private Vector3 bgZoomPos;

	private float bgZoomScl;

	private float bgZoomRate;

	private ScenarioDefine.BG_EFFECT_TYPE bgShakeType;

	private Vector3 bgShakePos;

	private Vector3 bgShakePwr;

	private float bcg_brightness;

	private float bcg_contrast;

	private float bcg_gamma;

	private AuthPlayer authPlayer;

	private DateTime authLoading;

	private GameObject moviePlayer;

	private DateTime movieLoading;

	private GameObject nameTelop;

	private GameObject bgTelop;

	private ScenarioScene.WaitParam waitParam;

	private float fAutoWaitTime;

	private float fAutoWaitSpeed = 5f;

	private float fAutoSerifTime;

	private int writerSpeed;

	private List<KeyValuePair<string, string>> logList;

	private static readonly string effectSandstarName = "Ef_scenario_sandstar";

	private static readonly string effectShineName = "Ef_scenario_shine";

	private static readonly string effectRainName = "Ef_scenario_rain";

	private static readonly string effectSteamName = "Ef_scenario_steam";

	private static readonly string effectSnowName = "Ef_scenario_snow";

	private static readonly string effectDarknessName = "Ef_scenario_darkness";

	private static readonly string effectSpeedLineUpName = "Ef_scenario_speedline_up";

	private static readonly string effectSpeedLineDownName = "Ef_scenario_speedline_down";

	private static readonly string effectSpeedLineLeftName = "Ef_scenario_speedline_left";

	private static readonly string effectSpeedLineRightName = "Ef_scenario_speedline_right";

	private EffectData effectStage;

	private static readonly string effectSmokeName = "Ef_scenario_smoke";

	private static readonly string effectSoilSmokeName = "Ef_scenario_soilsmoke";

	private EffectData effectSmoke;

	private bool loopSE;

	private CriAtomExPlayback loopSEhdl;

	private List<CriAtomExPlayback> playloopSEList = new List<CriAtomExPlayback>();

	public static readonly string CHARA_OFFSET_PATH = "Scenario/ScenarioCharaOffset.asset";

	public static readonly Dictionary<string, string> ENABLE_ALPHA_DIC = new Dictionary<string, string>
	{
		{ "car", "Texture2D/Bg_Scenario/Car/bg_car" },
		{ "tent", "Texture2D/Bg_Scenario/Tent/bg_tent" },
		{ "heli", "Texture2D/Bg_Scenario/Heli/bg_heli" },
		{ "heli_evening", "Texture2D/Bg_Scenario/Heli/bg_heli_evening" },
		{ "heli_night", "Texture2D/Bg_Scenario/Heli/bg_heli_night" },
		{ "heli_cloud", "Texture2D/Bg_Scenario/Heli/bg_heli_cloud" },
		{ "heli_open", "Texture2D/Bg_Scenario/Heli/bg_heli_open" },
		{ "heli_evening_open", "Texture2D/Bg_Scenario/Heli/bg_heli_evening_open" },
		{ "heli_night_open", "Texture2D/Bg_Scenario/Heli/bg_heli_night_open" },
		{ "heli_cloud_open", "Texture2D/Bg_Scenario/Heli/bg_heli_cloud_open" },
		{ "trainwindow", "Texture2D/Bg_Scenario/Trainwindow/bg_trainwindow" },
		{ "trainwindow_coloroff", "Texture2D/Bg_Scenario/Trainwindow/bg_trainwindow_coloroff" },
		{ "pier", "Texture2D/Bg_Scenario/Pier/bg_pier" },
		{ "pier_evening", "Texture2D/Bg_Scenario/Pier/bg_pier_evening" },
		{ "pier_night", "Texture2D/Bg_Scenario/Pier/bg_pier_night" },
		{ "pier_cloud", "Texture2D/Bg_Scenario/Pier/bg_pier_cloud" }
	};

	private enum Status
	{
		LOAD,
		LOAD_WAIT,
		IN,
		PROCESS,
		OUT,
		TERM
	}

	private enum INTRODECE_PHASE
	{
		CHARA_ALL_HIDE,
		SHOW_INTRODUCE_CHARA,
		SHOW_TELOP,
		WAIT_USER_CLICK,
		HIDE_INTRODUCE_CHARA,
		BACK_ORIGINAL_STATE,
		NONE
	}

	private enum AUTH_PHASE
	{
		INITIALISE,
		WAIT,
		PLAY,
		MAX
	}

	private enum MOVIE_PHASE
	{
		INITIALISE,
		WAIT,
		PLAY,
		MAX
	}

	public class friend
	{
		public int ID;

		public int posID;

		public GameObject obj;

		public CharaModelHandle charaModelHandle;

		public ScenarioSetValues.CharaOffset charaModelOffset;

		public string mName;

		public GameObject mEff;

		public EffectData mEffData;

		public int bMove;

		public float fTagPos;

		public float fTagRot;

		public bool bDisappear;

		public bool bMotion;

		public int bFade;

		public string faceId;

		public string idleFaceId;

		public Transform follow;

		public CharaMotionDefine.ActKey standby;

		public float standbyCF;

		public bool puyo;
	}

	private class GUI
	{
		public GameObject mGUI_Scenario;

		public SimpleAnimation mSerifAnim;

		public Transform mSerifImage_normal;

		public Transform mSerifImage_needle;

		public Transform mSerifImage_cloud;

		public TypewriterEffect mSerifText;

		public PguiTextCtrl mSerifTextJpn;

		public GameObject mSerifChara;

		public GameObject mImgMiraiObj;

		public GameObject mTitleObj;

		public ScenarioGUISelect mGuiSelect;

		public ScenarioGUIPlyBtns mGuiPlyBtns;

		public GameObject mLogWindow;

		public GameObject mLogEnd;

		public GameObject mKomado;

		public GameObject mNarration;

		public TypewriterEffect mNarrationText;

		public GameObject mPhoto;

		public GameObject mSkipWindow;

		public ScenarioSkipWindow sSkipWindow;
	}

	public struct WaitParam
	{
		public void ResetParam()
		{
			this.type = ScenarioDefine.TYPE.LABEL;
			this.isWaitInput = false;
			if (this.jumpRows == null)
			{
				this.jumpRows = new int[3];
				this.jumpText = new string[3];
				return;
			}
			for (int i = 0; i < this.jumpRows.Length; i++)
			{
				this.jumpRows[i] = -1;
				this.jumpText[i] = "";
			}
		}

		public bool isAnyWait()
		{
			return false | this.isWaitInput;
		}

		public ScenarioDefine.TYPE type;

		public bool isWaitInput;

		public int[] jumpRows;

		public string[] jumpText;
	}
}
