using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using CriWare;
using SGNFW.Ab;
using SGNFW.Common;
using UnityEngine;

// Token: 0x02000107 RID: 263
public class SoundManager : Singleton<SoundManager>
{
	// Token: 0x17000322 RID: 802
	// (get) Token: 0x06000C8E RID: 3214 RVA: 0x0004CF04 File Offset: 0x0004B104
	// (set) Token: 0x06000C8F RID: 3215 RVA: 0x0004CF0B File Offset: 0x0004B10B
	public static bool IsFinishInitialize { get; private set; }

	// Token: 0x06000C90 RID: 3216 RVA: 0x0004CF14 File Offset: 0x0004B114
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

	// Token: 0x06000C91 RID: 3217 RVA: 0x0004CF94 File Offset: 0x0004B194
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

	// Token: 0x06000C92 RID: 3218 RVA: 0x0004D07C File Offset: 0x0004B27C
	private static string MakeStreamingAssetsPath(string fileName)
	{
		string text = Application.streamingAssetsPath;
		if (text.Length > 0)
		{
			text += "/";
		}
		return text + fileName;
	}

	// Token: 0x06000C93 RID: 3219 RVA: 0x0004D0B3 File Offset: 0x0004B2B3
	private static string MakeFilePath(string fileName)
	{
		if (SoundManager.STREAMING_ASSETS_CUE_LIST.Contains(fileName))
		{
			return SoundManager.MakeStreamingAssetsPath(fileName);
		}
		AssetManager.PREFIX_PATH_SOUND + fileName;
		return Manager.AssetPath + "assets/" + fileName;
	}

	// Token: 0x06000C94 RID: 3220 RVA: 0x0004D0E8 File Offset: 0x0004B2E8
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

	// Token: 0x06000C95 RID: 3221 RVA: 0x0004D150 File Offset: 0x0004B350
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

	// Token: 0x06000C96 RID: 3222 RVA: 0x0004D1D0 File Offset: 0x0004B3D0
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

	// Token: 0x06000C97 RID: 3223 RVA: 0x0004D234 File Offset: 0x0004B434
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

	// Token: 0x06000C98 RID: 3224 RVA: 0x0004D2CC File Offset: 0x0004B4CC
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

	// Token: 0x06000C99 RID: 3225 RVA: 0x0004D306 File Offset: 0x0004B506
	public static void SetCategoryVolume(SoundCategory kind, float volume)
	{
		CriAtom.SetCategoryVolume(Enum.GetName(typeof(SoundCategory), kind), volume);
	}

	// Token: 0x06000C9A RID: 3226 RVA: 0x0004D324 File Offset: 0x0004B524
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

	// Token: 0x06000C9B RID: 3227 RVA: 0x0004D398 File Offset: 0x0004B598
	public static float GetCategoryVolume(SoundCategory kind)
	{
		return CriAtom.GetCategoryVolume(Enum.GetName(typeof(SoundCategory), kind));
	}

	// Token: 0x06000C9C RID: 3228 RVA: 0x0004D3B4 File Offset: 0x0004B5B4
	public static void SetTempVolume(float changeValue)
	{
		float categoryVolume = SoundManager.GetCategoryVolume(SoundCategory.BGM);
		float num = categoryVolume * changeValue;
		SoundManager.SetCategoryVolume(SoundCategory.BGM, num);
		Singleton<SoundManager>.Instance.fadeVolume = new SoundManager.FadeVolume(categoryVolume, num);
	}

	// Token: 0x06000C9D RID: 3229 RVA: 0x0004D3E4 File Offset: 0x0004B5E4
	public static void ReturnOrgVolume()
	{
		SoundManager.SetCategoryVolume(SoundCategory.BGM, Singleton<SoundManager>.Instance.fadeVolume.preVolume);
	}

	// Token: 0x06000C9E RID: 3230 RVA: 0x0004D3FB File Offset: 0x0004B5FB
	public static void PlayBGM(string bgmName)
	{
		SoundManager.PlayBGM(bgmName, 0, 0, 0);
	}

	// Token: 0x06000C9F RID: 3231 RVA: 0x0004D408 File Offset: 0x0004B608
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

	// Token: 0x06000CA0 RID: 3232 RVA: 0x0004D470 File Offset: 0x0004B670
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

	// Token: 0x06000CA1 RID: 3233 RVA: 0x0004D611 File Offset: 0x0004B811
	private void Update()
	{
		this.CheckPlayBGM();
	}

	// Token: 0x06000CA2 RID: 3234 RVA: 0x0004D61C File Offset: 0x0004B81C
	public static bool IsExsistCueSheetAssetData(string sheetName)
	{
		string text = AssetManager.PREFIX_PATH_SOUND + sheetName;
		return AssetManager.IsExsistAssetData(text + "_acb_info.xml") || AssetManager.IsExsistAssetData(text + ".awb") || AssetManager.IsExsistAssetData(text + ".acb");
	}

	// Token: 0x06000CA3 RID: 3235 RVA: 0x0004D66B File Offset: 0x0004B86B
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

	// Token: 0x06000CA4 RID: 3236 RVA: 0x0004D67C File Offset: 0x0004B87C
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

	// Token: 0x06000CA5 RID: 3237 RVA: 0x0004D850 File Offset: 0x0004BA50
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

	// Token: 0x06000CA6 RID: 3238 RVA: 0x0004D954 File Offset: 0x0004BB54
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

	// Token: 0x06000CA7 RID: 3239 RVA: 0x0004D9C4 File Offset: 0x0004BBC4
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

	// Token: 0x06000CA8 RID: 3240 RVA: 0x0004DABC File Offset: 0x0004BCBC
	public static void StopBGM()
	{
		SoundManager.StopBGM(0);
	}

	// Token: 0x06000CA9 RID: 3241 RVA: 0x0004DAC4 File Offset: 0x0004BCC4
	public static void StopBGM(int fadeOutTime)
	{
		Singleton<SoundManager>.Instance.bgmPlayer.SetFadeOutTime(fadeOutTime);
		Singleton<SoundManager>.Instance.bgmPlayer.Stop();
		Singleton<SoundManager>.Instance.currentBgm = null;
		Singleton<SoundManager>.Instance.nextBGM = null;
	}

	// Token: 0x06000CAA RID: 3242 RVA: 0x0004DAFB File Offset: 0x0004BCFB
	public static CriAtomExPlayback PlayVoice(string sheetName, string name)
	{
		return SoundManager.PlayInternal(sheetName, name, false, false);
	}

	// Token: 0x06000CAB RID: 3243 RVA: 0x0004DB06 File Offset: 0x0004BD06
	public static CriAtomExPlayback PlayVoiceByTypeName(string sheetName, string typeName)
	{
		return SoundManager.PlayVoice(sheetName, SoundManager.CreateVoiceName(sheetName, typeName));
	}

	// Token: 0x06000CAC RID: 3244 RVA: 0x0004DB15 File Offset: 0x0004BD15
	public static CriAtomExPlayback PlayVoice(string sheetName, VOICE_TYPE voiceType)
	{
		if (voiceType >= VOICE_TYPE.BST01)
		{
			return SoundManager.PlayVoiceByTypeName(sheetName, voiceType.ToString());
		}
		return new CriAtomExPlayback(0U);
	}

	// Token: 0x06000CAD RID: 3245 RVA: 0x0004DB38 File Offset: 0x0004BD38
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

	// Token: 0x06000CAE RID: 3246 RVA: 0x0004DB72 File Offset: 0x0004BD72
	public static float GetVoiceLengthByTypeName(string sheetName, string typeName)
	{
		return SoundManager.GetVoiceLength(sheetName, SoundManager.CreateVoiceName(sheetName, typeName));
	}

	// Token: 0x06000CAF RID: 3247 RVA: 0x0004DB81 File Offset: 0x0004BD81
	public static float GetVoiceLength(string sheetName, VOICE_TYPE voiceType)
	{
		if (voiceType >= VOICE_TYPE.BST01)
		{
			return SoundManager.GetVoiceLengthByTypeName(sheetName, voiceType.ToString());
		}
		return 0f;
	}

	// Token: 0x06000CB0 RID: 3248 RVA: 0x0004DBA0 File Offset: 0x0004BDA0
	public static string CharaIdToSheet(int charaId)
	{
		return SoundManager.VOICE_PREF_NAME + string.Format("{0:D4}", charaId);
	}

	// Token: 0x06000CB1 RID: 3249 RVA: 0x0004DBC9 File Offset: 0x0004BDC9
	private static string CreateVoiceName(string sheetName, string name)
	{
		return SoundManager.VOICE_PREF_PRJ + sheetName + "_" + name.ToLower();
	}

	// Token: 0x06000CB2 RID: 3250 RVA: 0x0004DBE6 File Offset: 0x0004BDE6
	public static string CreateVoiceNameByType(string sheetName, VOICE_TYPE voiceType)
	{
		if (voiceType < VOICE_TYPE.BST01)
		{
			return null;
		}
		return SoundManager.CreateVoiceName(sheetName, voiceType.ToString());
	}

	// Token: 0x06000CB3 RID: 3251 RVA: 0x0004DC01 File Offset: 0x0004BE01
	public static string CreateVoiceNameByCharaCombi(int charaId, string name)
	{
		return SoundManager.VOICE_PREF_PRJ + "cv_" + charaId.ToString("0000") + "_" + name;
	}

	// Token: 0x06000CB4 RID: 3252 RVA: 0x0004DC29 File Offset: 0x0004BE29
	public static List<string> GetCueNameList(string sheetName)
	{
		if (!Singleton<SoundManager>.Instance.sourceList.ContainsKey(sheetName))
		{
			return null;
		}
		return Singleton<SoundManager>.Instance.sourceList[sheetName].cueNameList;
	}

	// Token: 0x06000CB5 RID: 3253 RVA: 0x0004DC54 File Offset: 0x0004BE54
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

	// Token: 0x06000CB6 RID: 3254 RVA: 0x0004DD60 File Offset: 0x0004BF60
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

	// Token: 0x04000A14 RID: 2580
	private static string VOICE_PREF_NAME = "cv_";

	// Token: 0x04000A15 RID: 2581
	private static string VOICE_PREF_PRJ = "prd_";

	// Token: 0x04000A16 RID: 2582
	private static string SE_PREF_PRJ = "se_";

	// Token: 0x04000A17 RID: 2583
	private static readonly string acf = "ParadeSound.acf";

	// Token: 0x04000A18 RID: 2584
	private static readonly string secmn = "se_common";

	// Token: 0x04000A19 RID: 2585
	private static readonly List<string> STREAMING_ASSETS_CUE_LIST = new List<string>
	{
		SoundManager.acf,
		SoundManager.secmn
	};

	// Token: 0x04000A1A RID: 2586
	private static int BGM_SEET_LOAD_MAX = 2;

	// Token: 0x04000A1B RID: 2587
	private static int CV_SEET_LOAD_MAX = 20;

	// Token: 0x04000A1C RID: 2588
	private Dictionary<string, string> cueToSheetList = new Dictionary<string, string>();

	// Token: 0x04000A1D RID: 2589
	private Dictionary<string, SoundManager.SourceData> sourceList = new Dictionary<string, SoundManager.SourceData>();

	// Token: 0x04000A1E RID: 2590
	private string currentBgm;

	// Token: 0x04000A1F RID: 2591
	private List<string> loadBgmSeetList;

	// Token: 0x04000A20 RID: 2592
	private CriAtomExPlayer bgmPlayer;

	// Token: 0x04000A21 RID: 2593
	private bool isInitialize;

	// Token: 0x04000A23 RID: 2595
	private SoundManager.NextBGM nextBGM;

	// Token: 0x04000A24 RID: 2596
	private SoundManager.FadeVolume fadeVolume = new SoundManager.FadeVolume(1f, 1f);

	// Token: 0x04000A25 RID: 2597
	private IEnumerator loadBGM;

	// Token: 0x0200083E RID: 2110
	private class SourceData
	{
		// Token: 0x0600383F RID: 14399 RVA: 0x001CA7A4 File Offset: 0x001C89A4
		public SourceData(CriAtomSource inSourceOneShot, CriAtomSource inSourceLoop, GameObject inObj, List<string> inCueNameList)
		{
			this.sourceOneShot = inSourceOneShot;
			this.sourceLoop = inSourceLoop;
			this.obj = inObj;
			this.cueNameList = inCueNameList;
			this.count = 1;
		}

		// Token: 0x06003840 RID: 14400 RVA: 0x001CA7D0 File Offset: 0x001C89D0
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

		// Token: 0x0400370D RID: 14093
		public CriAtomSource sourceOneShot;

		// Token: 0x0400370E RID: 14094
		public CriAtomSource sourceLoop;

		// Token: 0x0400370F RID: 14095
		public GameObject obj;

		// Token: 0x04003710 RID: 14096
		public List<string> cueNameList;

		// Token: 0x04003711 RID: 14097
		public int count;
	}

	// Token: 0x0200083F RID: 2111
	private class NextBGM
	{
		// Token: 0x06003841 RID: 14401 RVA: 0x001CA804 File Offset: 0x001C8A04
		public NextBGM(string name, int fadeOut, int fadeIn, int startOffset, bool loop)
		{
			this.bgmName = name;
			this.fadeOutTime = fadeOut;
			this.fadeInTime = fadeIn;
			this.fadeInStartOffset = startOffset;
			this.Loop = loop;
		}

		// Token: 0x04003712 RID: 14098
		public string bgmName;

		// Token: 0x04003713 RID: 14099
		public int fadeOutTime;

		// Token: 0x04003714 RID: 14100
		public int fadeInTime;

		// Token: 0x04003715 RID: 14101
		public int fadeInStartOffset;

		// Token: 0x04003716 RID: 14102
		public bool Loop;
	}

	// Token: 0x02000840 RID: 2112
	private class FadeVolume
	{
		// Token: 0x06003842 RID: 14402 RVA: 0x001CA831 File Offset: 0x001C8A31
		public FadeVolume(float preVolume, float curVolume)
		{
			this.preVolume = preVolume;
			this.curVolume = curVolume;
		}

		// Token: 0x04003717 RID: 14103
		public float preVolume;

		// Token: 0x04003718 RID: 14104
		public float curVolume;
	}
}
