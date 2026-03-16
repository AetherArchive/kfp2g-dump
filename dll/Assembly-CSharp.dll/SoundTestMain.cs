using System;
using System.Collections.Generic;
using System.IO;
using SGNFW.Common;
using UnityEngine;

public class SoundTestMain : MonoBehaviour
{
	private void Start()
	{
		Singleton<SoundManager>.Instance.Initialize();
		this.Initialize();
	}

	private void Initialize()
	{
		string[] files = Directory.GetFiles(SoundTestMain.BASE_DIR, "*.acb");
		SoundTestMain.CueSheetList = new string[files.Length + 2];
		SoundTestMain.CueSheetList[0] = "se_common";
		SoundTestMain.CueSheetList[1] = "cv_dojo";
		for (int i = 0; i < files.Length; i++)
		{
			SoundTestMain.CueSheetList[i + 2] = Path.GetFileNameWithoutExtension(files[i]);
		}
	}

	private void OnGUI()
	{
		List<string> cueNameList = SoundManager.GetCueNameList(this.sheet);
		if (cueNameList != null)
		{
			cueNameList.Sort((string a, string b) => a.CompareTo(b));
		}
		this.scrollViewVec = GUI.BeginScrollView(new Rect(10f, 40f, 420f, 200f), this.scrollViewVec, new Rect(10f, 10f, 100f, (float)(20 * SoundTestMain.CueSheetList.Length + 10)));
		int num = 1;
		foreach (string text in SoundTestMain.CueSheetList)
		{
			if (GUI.Button(new Rect(10f, (float)(20 * num), 400f, 20f), text))
			{
				SoundManager.UnloadCueSheet(this.sheet);
				this.sheet = text;
				SoundManager.LoadCueSheet(this.sheet);
			}
			num++;
		}
		GUI.EndScrollView();
		if (cueNameList == null)
		{
			return;
		}
		GUI.Label(new Rect(10f, 250f, 180f, 20f), "Sound : " + this.soundName);
		this.scrollViewVec2 = GUI.BeginScrollView(new Rect(10f, 270f, 420f, 200f), this.scrollViewVec2, new Rect(10f, 10f, 100f, (float)(20 * cueNameList.Count + 10)));
		num = 1;
		foreach (string text2 in cueNameList)
		{
			if (GUI.Button(new Rect(10f, (float)(20 * num), 400f, 20f), text2))
			{
				if (text2.StartsWith("prd_bgm"))
				{
					SoundManager.StopBGM();
					SoundManager.PlayBGM(text2, 0, 0, 0);
				}
				else if (text2.StartsWith("prd_cv_"))
				{
					SoundManager.PlayVoice(this.sheet, text2);
				}
				else
				{
					SoundManager.Play(text2, false, this.isFast);
				}
				this.soundName = text2;
			}
			num++;
		}
		GUI.EndScrollView();
		string text3 = (this.isFast ? this.fastText : this.normalSpeedText);
		if (GUI.Button(new Rect(500f, 250f, 80f, 50f), text3))
		{
			this.isFast = !this.isFast;
			string text4 = (this.isFast ? this.fastText : this.normalSpeedText);
		}
	}

	private static string BASE_DIR = AssetManager.ASSET_FOLDER_PATH_SOUND + AssetManager.PREFIX_PATH_SOUND;

	private Vector2 scrollViewVec = Vector2.zero;

	private Vector2 scrollViewVec2 = Vector2.zero;

	private Vector2 scrollViewVec3 = Vector2.zero;

	private static string[] CueSheetList;

	private string sheet = "BGMCueSheet";

	private string soundName = "";

	private string normalSpeedText = "通常速度";

	private string fastText = "倍速";

	private bool isFast;
}
