using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using CriWare;
using SGNFW.Ab;
using SGNFW.Common;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
	public static bool IsFinishInitialize { get; private set; }

	public void Initialize()
	{
		if (this.isInitialize)
		{
			return;
		}
		this.isInitialize = true;
		SoundManager.IsFinishInitialize = false;
		this.sourceList = new Dictionary<string, SoundManager.SourceData>();
		this.cueToSheetList = new Dictionary<string, string>();
		CriAtomEx.RegisterAcf(null, SoundManager.MakeFilePath(SoundManager.acf));
		this.currentBgm = null;
		this.loadBgmSeetList = new List<string>(2);
		this.bgmPlayer = new CriAtomExPlayer();
		this.bgmPlayer.AttachFader();
		SoundManager.LoadCueSheet(SoundManager.secmn);
	}

	protected override void OnSingletonDestroy()
	{
		if (this.bgmPlayer != null)
		{
			SoundManager.StopBGM();
		}
		this.currentBgm = null;
		this.loadBgmSeetList = null;
		bool isActiveAndEnabled = base.gameObject.GetComponent<CriAtom>().isActiveAndEnabled;
		foreach (KeyValuePair<string, SoundManager.SourceData> keyValuePair in this.sourceList)
		{
			if (keyValuePair.Value.obj != null)
			{
				Object.Destroy(keyValuePair.Value.obj);
				keyValuePair.Value.obj = null;
			}
			if (isActiveAndEnabled)
			{
				CriAtom.RemoveCueSheet(keyValuePair.Key);
			}
		}
		this.sourceList = null;
		this.cueToSheetList = null;
		if (this.bgmPlayer != null)
		{
			this.bgmPlayer.Dispose();
			this.bgmPlayer = null;
		}
		CriAtomEx.UnregisterAcf();
	}

	private static string MakeStreamingAssetsPath(string fileName)
	{
		string text = Application.streamingAssetsPath;
		if (text.Length > 0)
		{
			text += "/";
		}
		return text + fileName;
	}

	private static string MakeFilePath(string fileName)
	{
		if (SoundManager.STREAMING_ASSETS_CUE_LIST.Contains(fileName))
		{
			return SoundManager.MakeStreamingAssetsPath(fileName);
		}
		AssetManager.PREFIX_PATH_SOUND + fileName;
		return Manager.AssetPath + "assets/" + fileName;
	}

	public static CriAtomExPlayback Play(string name, bool loop = false, bool isFast = false)
	{
		if (Singleton<SoundManager>.Instance == null)
		{
			return new CriAtomExPlayback(0U);
		}
		if (Singleton<SoundManager>.Instance.cueToSheetList == null)
		{
			return new CriAtomExPlayback(0U);
		}
		if (!Singleton<SoundManager>.Instance.cueToSheetList.ContainsKey(name))
		{
			return new CriAtomExPlayback(0U);
		}
		return SoundManager.PlayInternal(Singleton<SoundManager>.Instance.cueToSheetList[name], name, loop, isFast);
	}

	private static CriAtomExPlayback PlayInternal(string sheetName, string name, bool loop, bool isFast)
	{
		if (sheetName == null)
		{
			return new CriAtomExPlayback(0U);
		}
		if (!Singleton<SoundManager>.Instance.sourceList.ContainsKey(sheetName))
		{
			return new CriAtomExPlayback(0U);
		}
		if (!Singleton<SoundManager>.Instance.cueToSheetList.ContainsKey(name))
		{
			return new CriAtomExPlayback(0U);
		}
		SoundManager.SourceData sourceData = Singleton<SoundManager>.Instance.sourceList[sheetName];
		object obj = (loop ? sourceData.sourceLoop : sourceData.sourceOneShot);
		SoundManager.SetSESpeed(name, isFast);
		object obj2 = obj;
		obj2.loop = loop;
		return obj2.Play(name);
	}

	public static CriAtomExPlayback randPlay(string name, bool loop = false)
	{
		if (Singleton<SoundManager>.Instance == null)
		{
			return new CriAtomExPlayback(0U);
		}
		if (Singleton<SoundManager>.Instance.cueToSheetList == null)
		{
			return new CriAtomExPlayback(0U);
		}
		if (!Singleton<SoundManager>.Instance.cueToSheetList.ContainsKey(name))
		{
			return new CriAtomExPlayback(0U);
		}
		return SoundManager.randPlayInternal(Singleton<SoundManager>.Instance.cueToSheetList[name], name, loop);
	}

	private static CriAtomExPlayback randPlayInternal(string sheetName, string name, bool loop)
	{
		if (sheetName == null)
		{
			return new CriAtomExPlayback(0U);
		}
		if (!Singleton<SoundManager>.Instance.sourceList.ContainsKey(sheetName))
		{
			return new CriAtomExPlayback(0U);
		}
		if (!Singleton<SoundManager>.Instance.cueToSheetList.ContainsKey(name))
		{
			return new CriAtomExPlayback(0U);
		}
		SoundManager.SourceData sourceData = Singleton<SoundManager>.Instance.sourceList[sheetName];
		CriAtomSource criAtomSource = (loop ? sourceData.sourceLoop : sourceData.sourceOneShot);
		criAtomSource.loop = loop;
		if (criAtomSource.player != null)
		{
			criAtomSource.player.SetRandomSeed((uint)Random.Range(1, 1000));
		}
		return criAtomSource.Play(name);
	}

	public static void Stop(string sheetName)
	{
		if (!Singleton<SoundManager>.Instance.sourceList.ContainsKey(sheetName))
		{
			return;
		}
		SoundManager.SourceData sourceData = Singleton<SoundManager>.Instance.sourceList[sheetName];
		if (sourceData != null)
		{
			sourceData.Stop();
		}
	}

	public static void SetCategoryVolume(SoundCategory kind, float volume)
	{
		CriAtom.SetCategoryVolume(Enum.GetName(typeof(SoundCategory), kind), volume);
	}

	public static void SetCategoryVolume(float[] volumeList)
	{
		Array values = Enum.GetValues(typeof(SoundCategory));
		if (volumeList.Length < values.Length)
		{
			return;
		}
		foreach (object obj in values)
		{
			SoundCategory soundCategory = (SoundCategory)obj;
			SoundManager.SetCategoryVolume(soundCategory, volumeList[(int)soundCategory]);
		}
	}

	public static float GetCategoryVolume(SoundCategory kind)
	{
		return CriAtom.GetCategoryVolume(Enum.GetName(typeof(SoundCategory), kind));
	}

	public static void SetTempVolume(float changeValue)
	{
		float categoryVolume = SoundManager.GetCategoryVolume(SoundCategory.BGM);
		float num = categoryVolume * changeValue;
		SoundManager.SetCategoryVolume(SoundCategory.BGM, num);
		Singleton<SoundManager>.Instance.fadeVolume = new SoundManager.FadeVolume(categoryVolume, num);
	}

	public static void ReturnOrgVolume()
	{
		SoundManager.SetCategoryVolume(SoundCategory.BGM, Singleton<SoundManager>.Instance.fadeVolume.preVolume);
	}

	public static void PlayBGM(string bgmName)
	{
		SoundManager.PlayBGM(bgmName, 0, 0, 0);
	}

	public static void PlayBGM(string bgmName, int fadeOutTime, int fadeInTime, int startOffset)
	{
		if (string.IsNullOrEmpty(bgmName))
		{
			return;
		}
		if (startOffset >= 0)
		{
			if (Singleton<SoundManager>.Instance.nextBGM == null)
			{
				if (bgmName.Equals(Singleton<SoundManager>.Instance.currentBgm))
				{
					return;
				}
			}
			else if (bgmName.Equals(Singleton<SoundManager>.Instance.nextBGM.bgmName))
			{
				return;
			}
		}
		Singleton<SoundManager>.Instance.nextBGM = new SoundManager.NextBGM(bgmName, fadeOutTime, fadeInTime, startOffset, true);
	}

	private void CheckPlayBGM()
	{
		if (this.loadBGM != null)
		{
			if (this.loadBGM.MoveNext())
			{
				return;
			}
			this.loadBGM = null;
		}
		if (Singleton<SoundManager>.Instance.nextBGM == null)
		{
			return;
		}
		string bgmName = Singleton<SoundManager>.Instance.nextBGM.bgmName;
		if (bgmName.Equals(Singleton<SoundManager>.Instance.currentBgm) && Singleton<SoundManager>.Instance.nextBGM.fadeInStartOffset >= 0)
		{
			Singleton<SoundManager>.Instance.nextBGM = null;
			return;
		}
		CriAtomExAcb acb = CriAtom.GetAcb(bgmName);
		if (acb == null)
		{
			this.loadBGM = SoundManager.LoadCueSheetWithDownload(bgmName);
			return;
		}
		if (!Singleton<SoundManager>.Instance.loadBgmSeetList.Contains(bgmName))
		{
			if (SoundManager.BGM_SEET_LOAD_MAX <= Singleton<SoundManager>.Instance.loadBgmSeetList.Count)
			{
				SoundManager.UnloadCueSheet(Singleton<SoundManager>.Instance.loadBgmSeetList[0]);
				Singleton<SoundManager>.Instance.loadBgmSeetList.RemoveAt(0);
			}
			Singleton<SoundManager>.Instance.loadBgmSeetList.Add(bgmName);
		}
		Singleton<SoundManager>.Instance.currentBgm = bgmName;
		Singleton<SoundManager>.Instance.bgmPlayer.SetCue(acb, bgmName);
		Singleton<SoundManager>.Instance.bgmPlayer.SetFadeOutTime(Singleton<SoundManager>.Instance.nextBGM.fadeOutTime);
		Singleton<SoundManager>.Instance.bgmPlayer.SetFadeInTime(Singleton<SoundManager>.Instance.nextBGM.fadeInTime);
		Singleton<SoundManager>.Instance.bgmPlayer.SetFadeInStartOffset((Singleton<SoundManager>.Instance.nextBGM.fadeInStartOffset < 0) ? 0 : Singleton<SoundManager>.Instance.nextBGM.fadeInStartOffset);
		Singleton<SoundManager>.Instance.bgmPlayer.Loop(true);
		Singleton<SoundManager>.Instance.bgmPlayer.Start();
		Singleton<SoundManager>.Instance.nextBGM = null;
	}

	private void Update()
	{
		this.CheckPlayBGM();
	}

	public static bool IsExsistCueSheetAssetData(string sheetName)
	{
		string text = AssetManager.PREFIX_PATH_SOUND + sheetName;
		return AssetManager.IsExsistAssetData(text + "_acb_info.xml") || AssetManager.IsExsistAssetData(text + ".awb") || AssetManager.IsExsistAssetData(text + ".acb");
	}

	public static IEnumerator LoadCueSheetWithDownload(string sheetName)
	{
		string loadAssetName = AssetManager.PREFIX_PATH_SOUND + sheetName;
		AssetManager.DownloadAssetData(loadAssetName + "_acb_info.xml", AssetManager.OWNER.Sound, 0);
		AssetManager.DownloadAssetData(loadAssetName + ".awb", AssetManager.OWNER.Sound, 0);
		AssetManager.DownloadAssetData(loadAssetName + ".acb", AssetManager.OWNER.Sound, 0);
		while (!AssetManager.IsDownloadFinishAssetData(loadAssetName + "._acb_info.xml") || !AssetManager.IsDownloadFinishAssetData(loadAssetName + ".awb") || !AssetManager.IsDownloadFinishAssetData(loadAssetName + ".acb"))
		{
			yield return null;
		}
		SoundManager.LoadCueSheet(sheetName);
		yield break;
	}

	public static string LoadCueSheet(string sheetName)
	{
		if (Singleton<SoundManager>.Instance == null)
		{
			return null;
		}
		if (Singleton<SoundManager>.Instance.sourceList.ContainsKey(sheetName))
		{
			Singleton<SoundManager>.Instance.sourceList[sheetName].count++;
			return sheetName;
		}
		if (Singleton<SoundManager>.Instance.sourceList.Count >= SoundManager.CV_SEET_LOAD_MAX)
		{
			SoundManager.UnloadCVAll(sheetName);
		}
		string text = SoundManager.MakeFilePath(sheetName);
		string text2 = text + ".acb";
		string text3 = text + ".awb";
		if (Path.IsPathRooted(text))
		{
			if (!File.Exists(text2))
			{
				return null;
			}
			if (!File.Exists(text3))
			{
				text3 = null;
			}
		}
		CriAtomCueSheet criAtomCueSheet = CriAtom.AddCueSheet(sheetName, text2, text3, null);
		if (criAtomCueSheet == null)
		{
			return null;
		}
		if (criAtomCueSheet.acb == null)
		{
			CriAtom.RemoveCueSheet(sheetName);
			return null;
		}
		CriAtomEx.CueInfo[] cueInfoList = criAtomCueSheet.acb.GetCueInfoList();
		if (cueInfoList == null)
		{
			return null;
		}
		if (cueInfoList.Length == 0)
		{
			return null;
		}
		List<string> list = new List<string>();
		foreach (CriAtomEx.CueInfo cueInfo in cueInfoList)
		{
			list.Add(cueInfo.name);
			if (!Singleton<SoundManager>.Instance.cueToSheetList.ContainsKey(cueInfo.name))
			{
				Singleton<SoundManager>.Instance.cueToSheetList.Add(cueInfo.name, sheetName);
			}
		}
		GameObject gameObject = new GameObject();
		gameObject.name = sheetName;
		gameObject.transform.SetParent(Singleton<SoundManager>.Instance.transform, true);
		CriAtomSource criAtomSource = gameObject.AddComponent<CriAtomSource>();
		criAtomSource.cueSheet = sheetName;
		criAtomSource.loop = false;
		criAtomSource.use3dPositioning = false;
		CriAtomSource criAtomSource2 = null;
		if (sheetName.IndexOf(SoundManager.SE_PREF_PRJ) == 0)
		{
			criAtomSource2 = gameObject.AddComponent<CriAtomSource>();
			criAtomSource2.cueSheet = sheetName;
			criAtomSource2.loop = true;
			criAtomSource2.use3dPositioning = false;
		}
		Singleton<SoundManager>.Instance.sourceList.Add(sheetName, new SoundManager.SourceData(criAtomSource, criAtomSource2, gameObject, list));
		return sheetName;
	}

	public static void UnloadCueSheet(string sheetName)
	{
		if (Singleton<SoundManager>.Instance == null)
		{
			return;
		}
		if (Singleton<SoundManager>.Instance.sourceList.ContainsKey(sheetName))
		{
			SoundManager.SourceData sourceData = Singleton<SoundManager>.Instance.sourceList[sheetName];
			SoundManager.SourceData sourceData2 = sourceData;
			int num = sourceData2.count - 1;
			sourceData2.count = num;
			if (num > 0)
			{
				return;
			}
			foreach (string text in Singleton<SoundManager>.Instance.sourceList[sheetName].cueNameList)
			{
				Singleton<SoundManager>.Instance.cueToSheetList.Remove(text);
			}
			Object.Destroy(sourceData.obj);
			Singleton<SoundManager>.Instance.sourceList.Remove(sheetName);
			CriAtom.RemoveCueSheet(sheetName);
			string text2 = AssetManager.PREFIX_PATH_SOUND + sheetName;
			AssetManager.UnloadAssetData(text2 + ".acb", AssetManager.OWNER.Sound);
			AssetManager.UnloadAssetData(text2 + ".awb", AssetManager.OWNER.Sound);
		}
	}

	public static void UnloadCueSheetAll()
	{
		foreach (string text in new List<string>(Singleton<SoundManager>.Instance.sourceList.Keys))
		{
			if (text != SoundManager.secmn)
			{
				SoundManager.UnloadCueSheet(text);
			}
		}
	}

	public static void UnloadCVAll(string sheetName = "")
	{
		foreach (string text in new List<string>(Singleton<SoundManager>.Instance.sourceList.Keys))
		{
			if (!(text == SoundManager.secmn) && text.IndexOf(SoundManager.VOICE_PREF_PRJ) != 0 && text.IndexOf(SoundManager.SE_PREF_PRJ) != 0 && !(text == sheetName))
			{
				if (Singleton<SoundManager>.Instance.sourceList[text].obj != null)
				{
					Object.Destroy(Singleton<SoundManager>.Instance.sourceList[text].obj);
					Singleton<SoundManager>.Instance.sourceList[text].obj = null;
				}
				CriAtom.RemoveCueSheet(text);
				Singleton<SoundManager>.Instance.sourceList.Remove(text);
			}
		}
	}

	public static void StopBGM()
	{
		SoundManager.StopBGM(0);
	}

	public static void StopBGM(int fadeOutTime)
	{
		Singleton<SoundManager>.Instance.bgmPlayer.SetFadeOutTime(fadeOutTime);
		Singleton<SoundManager>.Instance.bgmPlayer.Stop();
		Singleton<SoundManager>.Instance.currentBgm = null;
		Singleton<SoundManager>.Instance.nextBGM = null;
	}

	public static CriAtomExPlayback PlayVoice(string sheetName, string name)
	{
		return SoundManager.PlayInternal(sheetName, name, false, false);
	}

	public static CriAtomExPlayback PlayVoiceByTypeName(string sheetName, string typeName)
	{
		return SoundManager.PlayVoice(sheetName, SoundManager.CreateVoiceName(sheetName, typeName));
	}

	public static CriAtomExPlayback PlayVoice(string sheetName, VOICE_TYPE voiceType)
	{
		if (voiceType >= VOICE_TYPE.BST01)
		{
			return SoundManager.PlayVoiceByTypeName(sheetName, voiceType.ToString());
		}
		return new CriAtomExPlayback(0U);
	}

	public static float GetVoiceLength(string sheetName, string name)
	{
		CriAtomExAcb criAtomExAcb = null;
		if (sheetName != null)
		{
			criAtomExAcb = CriAtom.GetAcb(sheetName);
		}
		CriAtomEx.CueInfo cueInfo;
		if (criAtomExAcb != null && criAtomExAcb.GetCueInfo(name, out cueInfo))
		{
			return (float)cueInfo.length * 0.001f;
		}
		return 0f;
	}

	public static float GetVoiceLengthByTypeName(string sheetName, string typeName)
	{
		return SoundManager.GetVoiceLength(sheetName, SoundManager.CreateVoiceName(sheetName, typeName));
	}

	public static float GetVoiceLength(string sheetName, VOICE_TYPE voiceType)
	{
		if (voiceType >= VOICE_TYPE.BST01)
		{
			return SoundManager.GetVoiceLengthByTypeName(sheetName, voiceType.ToString());
		}
		return 0f;
	}

	public static string CharaIdToSheet(int charaId)
	{
		return SoundManager.VOICE_PREF_NAME + string.Format("{0:D4}", charaId);
	}

	private static string CreateVoiceName(string sheetName, string name)
	{
		return SoundManager.VOICE_PREF_PRJ + sheetName + "_" + name.ToLower();
	}

	public static string CreateVoiceNameByType(string sheetName, VOICE_TYPE voiceType)
	{
		if (voiceType < VOICE_TYPE.BST01)
		{
			return null;
		}
		return SoundManager.CreateVoiceName(sheetName, voiceType.ToString());
	}

	public static string CreateVoiceNameByCharaCombi(int charaId, string name)
	{
		return SoundManager.VOICE_PREF_PRJ + "cv_" + charaId.ToString("0000") + "_" + name;
	}

	public static List<string> GetCueNameList(string sheetName)
	{
		if (!Singleton<SoundManager>.Instance.sourceList.ContainsKey(sheetName))
		{
			return null;
		}
		return Singleton<SoundManager>.Instance.sourceList[sheetName].cueNameList;
	}

	public static void SetPosition(string sheetName, Transform obj, float dist)
	{
		if (!Singleton<SoundManager>.Instance.sourceList.ContainsKey(sheetName))
		{
			return;
		}
		SoundManager.SourceData sourceData = Singleton<SoundManager>.Instance.sourceList[sheetName];
		bool flag = obj != null;
		if (sourceData.sourceOneShot.use3dPositioning != flag)
		{
			sourceData.sourceOneShot.use3dPositioning = flag;
			sourceData.sourceOneShot.player.SetPanType(flag ? CriAtomEx.PanType.Pos3d : CriAtomEx.PanType.Pan3d);
			sourceData.sourceOneShot.SetDistance3D(flag ? 1f : 0f, flag ? dist : 0f);
		}
		if (sourceData.sourceLoop != null && sourceData.sourceLoop.use3dPositioning != flag)
		{
			sourceData.sourceLoop.use3dPositioning = flag;
			sourceData.sourceLoop.player.SetPanType(flag ? CriAtomEx.PanType.Pos3d : CriAtomEx.PanType.Pan3d);
			sourceData.sourceLoop.SetDistance3D(flag ? 1f : 0f, flag ? dist : 0f);
		}
		if (flag)
		{
			sourceData.obj.transform.position = obj.position;
		}
	}

	public static void SetSESpeed(string name, bool isFast)
	{
		if (!name.StartsWith("prd_se_act") || name.EndsWith("common"))
		{
			return;
		}
		string text = Singleton<SoundManager>.Instance.cueToSheetList[name];
		if (text == null)
		{
			return;
		}
		if (!Singleton<SoundManager>.Instance.sourceList.ContainsKey(text))
		{
			return;
		}
		if (!Singleton<SoundManager>.Instance.cueToSheetList.ContainsKey(name))
		{
			return;
		}
		SoundManager.SourceData sourceData = Singleton<SoundManager>.Instance.sourceList[text];
		if (sourceData == null)
		{
			return;
		}
		float num = (isFast ? 1f : 0f);
		CriAtomExAcb acb = CriAtom.GetAcb(text);
		int numUsableAisacControls = acb.GetNumUsableAisacControls(name);
		for (int i = 0; i < numUsableAisacControls; i++)
		{
			CriAtomEx.AisacControlInfo aisacControlInfo;
			acb.GetUsableAisacControl(name, i, out aisacControlInfo);
			sourceData.sourceOneShot.player.SetAisacControl(aisacControlInfo.name, num);
		}
		sourceData.sourceOneShot.player.UpdateAll();
	}

	private static string VOICE_PREF_NAME = "cv_";

	private static string VOICE_PREF_PRJ = "prd_";

	private static string SE_PREF_PRJ = "se_";

	private static readonly string acf = "ParadeSound.acf";

	private static readonly string secmn = "se_common";

	private static readonly List<string> STREAMING_ASSETS_CUE_LIST = new List<string>
	{
		SoundManager.acf,
		SoundManager.secmn
	};

	private static int BGM_SEET_LOAD_MAX = 2;

	private static int CV_SEET_LOAD_MAX = 20;

	private Dictionary<string, string> cueToSheetList = new Dictionary<string, string>();

	private Dictionary<string, SoundManager.SourceData> sourceList = new Dictionary<string, SoundManager.SourceData>();

	private string currentBgm;

	private List<string> loadBgmSeetList;

	private CriAtomExPlayer bgmPlayer;

	private bool isInitialize;

	private SoundManager.NextBGM nextBGM;

	private SoundManager.FadeVolume fadeVolume = new SoundManager.FadeVolume(1f, 1f);

	private IEnumerator loadBGM;

	private class SourceData
	{
		public SourceData(CriAtomSource inSourceOneShot, CriAtomSource inSourceLoop, GameObject inObj, List<string> inCueNameList)
		{
			this.sourceOneShot = inSourceOneShot;
			this.sourceLoop = inSourceLoop;
			this.obj = inObj;
			this.cueNameList = inCueNameList;
			this.count = 1;
		}

		public void Stop()
		{
			if (this.sourceOneShot != null)
			{
				this.sourceOneShot.Stop();
			}
			if (this.sourceLoop != null)
			{
				this.sourceLoop.Stop();
			}
		}

		public CriAtomSource sourceOneShot;

		public CriAtomSource sourceLoop;

		public GameObject obj;

		public List<string> cueNameList;

		public int count;
	}

	private class NextBGM
	{
		public NextBGM(string name, int fadeOut, int fadeIn, int startOffset, bool loop)
		{
			this.bgmName = name;
			this.fadeOutTime = fadeOut;
			this.fadeInTime = fadeIn;
			this.fadeInStartOffset = startOffset;
			this.Loop = loop;
		}

		public string bgmName;

		public int fadeOutTime;

		public int fadeInTime;

		public int fadeInStartOffset;

		public bool Loop;
	}

	private class FadeVolume
	{
		public FadeVolume(float preVolume, float curVolume)
		{
			this.preVolume = preVolume;
			this.curVolume = curVolume;
		}

		public float preVolume;

		public float curVolume;
	}
}
