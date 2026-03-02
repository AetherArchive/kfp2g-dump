using System;
using System.Collections.Generic;
using System.IO;
using SGNFW.Common;
using UnityEngine;

// Token: 0x020001F8 RID: 504
public class SoundTestMain : MonoBehaviour
{
	// Token: 0x06002150 RID: 8528 RVA: 0x0018E85C File Offset: 0x0018CA5C
	private void Start()
	{
		Singleton<SoundManager>.Instance.Initialize();
		this.Initialize();
	}

	// Token: 0x06002151 RID: 8529 RVA: 0x0018E870 File Offset: 0x0018CA70
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

	// Token: 0x06002152 RID: 8530 RVA: 0x0018E8D4 File Offset: 0x0018CAD4
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

	// Token: 0x040017F3 RID: 6131
	private static string BASE_DIR = AssetManager.ASSET_FOLDER_PATH_SOUND + AssetManager.PREFIX_PATH_SOUND;

	// Token: 0x040017F4 RID: 6132
	private Vector2 scrollViewVec = Vector2.zero;

	// Token: 0x040017F5 RID: 6133
	private Vector2 scrollViewVec2 = Vector2.zero;

	// Token: 0x040017F6 RID: 6134
	private Vector2 scrollViewVec3 = Vector2.zero;

	// Token: 0x040017F7 RID: 6135
	private static string[] CueSheetList;

	// Token: 0x040017F8 RID: 6136
	private string sheet = "BGMCueSheet";

	// Token: 0x040017F9 RID: 6137
	private string soundName = "";

	// Token: 0x040017FA RID: 6138
	private string normalSpeedText = "通常速度";

	// Token: 0x040017FB RID: 6139
	private string fastText = "倍速";

	// Token: 0x040017FC RID: 6140
	private bool isFast;
}
