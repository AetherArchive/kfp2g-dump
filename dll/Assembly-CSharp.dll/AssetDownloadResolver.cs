using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Ab;
using SGNFW.Common;
using UnityEngine;

public class AssetDownloadResolver : BaseScene
{
	public static IEnumerator ResolveActionNeedOndly()
	{
		Utility.DownloadInfo downloadInfo = AssetManager.CreateDownloadInfo(AssetManager.DownloadType.NEED_ONLY);
		if (downloadInfo.datas.Count <= 0)
		{
			yield break;
		}
		Loader loader = Manager.Download(downloadInfo.datas, null);
		while (loader != null && !loader.IsDone)
		{
			yield return null;
		}
		loader = null;
		yield break;
	}

	private static IEnumerator ResolveActionLoad(Utility.DownloadInfo info, AssetDownloadResolver.CancelCallBack cb = null, bool isDispStop = true)
	{
		LoadAndTipsCtrl.SetupParam setupParam = new LoadAndTipsCtrl.SetupParam
		{
			dispTipsId = 0,
			isDispFade = true,
			isDispProgress = true,
			isDispStopButton = isDispStop,
			isDispTips = true
		};
		CanvasManager.HdlLoadAndTipsCtrl.Setup(setupParam);
		bool isFinishDownload = true;
		AssetDownloadResolver.<>c__DisplayClass3_1 CS$<>8__locals2 = new AssetDownloadResolver.<>c__DisplayClass3_1();
		Screen.sleepTimeout = -1;
		CS$<>8__locals2.loadall = (float)info.datas.Count;
		CS$<>8__locals2.loaded = 0f;
		setupParam.cbGetProgress = () => CS$<>8__locals2.loaded / CS$<>8__locals2.loadall;
		setupParam.cbDownloadStop = delegate
		{
			isFinishDownload = false;
		};
		while (isFinishDownload && info.datas.Count > 0)
		{
			List<Data> dts = new List<Data>((info.datas.Count > 10) ? info.datas.GetRange(0, 10) : info.datas);
			info.datas.RemoveRange(0, dts.Count);
			Loader loader = Manager.Download(dts, null);
			float ldd = CS$<>8__locals2.loaded;
			while (loader != null && !loader.IsDone)
			{
				CS$<>8__locals2.loaded = ldd + loader.Progress * (float)dts.Count;
				if (!isFinishDownload)
				{
					loader.Exit();
				}
				yield return null;
			}
			if (isFinishDownload)
			{
				CS$<>8__locals2.loaded = ldd + (float)dts.Count;
			}
			dts = null;
			loader = null;
		}
		Screen.sleepTimeout = -2;
		CS$<>8__locals2 = null;
		CanvasManager.HdlLoadAndTipsCtrl.Close(isFinishDownload);
		if (!isFinishDownload && cb != null)
		{
			cb();
		}
		yield break;
	}

	public static IEnumerator ResolveActionFull(bool isNothingInfo)
	{
		if (!Singleton<AssetManager>.Instance.IsEndAbChek())
		{
			Singleton<AssetManager>.Instance.SetAbChkType(isNothingInfo ? AssetManager.abCheckType.HIGH : AssetManager.abCheckType.MIDDLE);
			CanvasManager.HdlLoadAndTipsCtrl.SetDispLoading(true);
			while (!Singleton<AssetManager>.Instance.IsEndAbChek())
			{
				yield return null;
			}
			CanvasManager.HdlLoadAndTipsCtrl.SetDispLoading(false);
		}
		Utility.DownloadInfo info = AssetManager.CreateDownloadInfo(AssetManager.DownloadType.FULL);
		if (info.datas.Count <= 0)
		{
			if (isNothingInfo)
			{
				AssetDownloadResolver.<>c__DisplayClass4_1 CS$<>8__locals2 = new AssetDownloadResolver.<>c__DisplayClass4_1();
				CS$<>8__locals2.isWindowFinish = false;
				CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("追加ダウンロード"), PrjUtil.MakeMessage("追加のダウンロードデータは\nありませんでした"), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, delegate(int index)
				{
					CS$<>8__locals2.isWindowFinish = true;
					return true;
				}, null, false);
				CanvasManager.HdlOpenWindowBasic.Open();
				while (!CS$<>8__locals2.isWindowFinish)
				{
					yield return null;
				}
				CS$<>8__locals2 = null;
			}
			yield break;
		}
		AssetDownloadResolver.<>c__DisplayClass4_2 CS$<>8__locals3 = new AssetDownloadResolver.<>c__DisplayClass4_2();
		CS$<>8__locals3.isWindowFinish = false;
		CS$<>8__locals3.owAnswer = 0;
		CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("追加ダウンロード"), PrjUtil.MakeMessage("最新の各種ゲームデータを一括ダウンロードします。\nダウンロードすることで通信を減らし、\n快適にプレイできます。\n\n<color=" + PrjUtil.WARNING_COLOR_CODE + ">※通信容量が大きい為、Wi-Fi環境など、\n通信の安定した状態を推奨します。</color>\n\n" + string.Format("ダウンロードしますか？(約{0}MB)", info.totalSize / 1000000L)), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, delegate(int index)
		{
			CS$<>8__locals3.owAnswer = index;
			CS$<>8__locals3.isWindowFinish = true;
			return true;
		}, null, false);
		CanvasManager.HdlOpenWindowBasic.Open();
		while (!CS$<>8__locals3.isWindowFinish)
		{
			yield return null;
		}
		if (CS$<>8__locals3.owAnswer != 1)
		{
			yield break;
		}
		CS$<>8__locals3 = null;
		bool isFinishDownload = true;
		IEnumerator load = AssetDownloadResolver.ResolveActionLoad(info, delegate
		{
			isFinishDownload = false;
		}, true);
		while (load.MoveNext())
		{
			yield return null;
		}
		if (isFinishDownload)
		{
			AssetDownloadResolver.<>c__DisplayClass4_3 CS$<>8__locals4 = new AssetDownloadResolver.<>c__DisplayClass4_3();
			CS$<>8__locals4.isWindowFinish = false;
			CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("追加ダウンロード"), PrjUtil.MakeMessage("追加データのダウンロードが\n完了しました。"), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, delegate(int index)
			{
				CS$<>8__locals4.isWindowFinish = true;
				return true;
			}, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			while (!CS$<>8__locals4.isWindowFinish)
			{
				yield return null;
			}
			CS$<>8__locals4 = null;
		}
		yield break;
	}

	public static IEnumerator ResolveActionOpMovie(AssetDownloadResolver.CancelCallBack cb = null)
	{
		Utility.DownloadInfo info = AssetManager.CreateDownloadInfo(AssetManager.DownloadType.OP_MOVIE);
		if (info.datas.Count <= 0)
		{
			yield break;
		}
		AssetDownloadResolver.<>c__DisplayClass5_1 CS$<>8__locals2 = new AssetDownloadResolver.<>c__DisplayClass5_1();
		CS$<>8__locals2.isWindowFinish = false;
		CS$<>8__locals2.owAnswer = 0;
		CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("ムービーダウンロード"), PrjUtil.MakeMessage("ＯＰムービーをダウンロードし再生します\nいまダウンロードしなくても\n後でダウンロードしてご視聴いただけます\n<color=" + PrjUtil.WARNING_COLOR_CODE + ">※通信容量が大きい為、Wi-Fi環境など、\n通信の安定した状態を推奨します。</color>\n\n" + string.Format("ダウンロードしますか？(約{0}MB)", info.totalSize / 1000000L)), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, delegate(int index)
		{
			CS$<>8__locals2.owAnswer = index;
			CS$<>8__locals2.isWindowFinish = true;
			return true;
		}, null, false);
		CanvasManager.HdlOpenWindowBasic.Open();
		while (!CS$<>8__locals2.isWindowFinish)
		{
			yield return null;
		}
		if (CS$<>8__locals2.owAnswer != 1)
		{
			if (cb != null)
			{
				cb();
			}
			yield break;
		}
		CS$<>8__locals2 = null;
		bool isFinishDownload = true;
		IEnumerator load = AssetDownloadResolver.ResolveActionLoad(info, delegate
		{
			isFinishDownload = false;
		}, true);
		while (load.MoveNext())
		{
			yield return null;
		}
		if (!isFinishDownload)
		{
			if (cb != null)
			{
				cb();
			}
			yield break;
		}
		AssetDownloadResolver.<>c__DisplayClass5_2 CS$<>8__locals3 = new AssetDownloadResolver.<>c__DisplayClass5_2();
		CS$<>8__locals3.isWindowFinish = false;
		CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("ムービーダウンロード"), PrjUtil.MakeMessage("ＯＰムービーのダウンロードが完了しました。\n\nＯＰムービーを再生します"), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, delegate(int index)
		{
			CS$<>8__locals3.isWindowFinish = true;
			return true;
		}, null, false);
		CanvasManager.HdlOpenWindowBasic.Open();
		while (!CS$<>8__locals3.isWindowFinish)
		{
			yield return null;
		}
		CS$<>8__locals3 = null;
		yield break;
	}

	public static IEnumerator ResolveActionIntroductionMovie()
	{
		Utility.DownloadInfo downloadInfo = AssetManager.CreateDownloadInfo(AssetManager.DownloadType.INTRODUCTION);
		if (downloadInfo.datas.Count <= 0)
		{
			yield break;
		}
		bool isFinishDownload = true;
		IEnumerator load = AssetDownloadResolver.ResolveActionLoad(downloadInfo, delegate
		{
			isFinishDownload = false;
		}, false);
		while (load.MoveNext())
		{
			yield return null;
		}
		bool isFinishDownload2 = isFinishDownload;
		yield break;
	}

	public static IEnumerator ResolveActionMovie(string mov, string nam, AssetDownloadResolver.CancelCallBack cb = null)
	{
		Data data = Manager.DataList.Find((Data itm) => itm.name == (mov + AssetManager.ASSET_MOVIE_EXT).ToLower());
		Utility.DownloadInfo info = ((data == null) ? new Utility.DownloadInfo() : Utility.GetDownloadInfo(new List<Data> { data }));
		if (info.datas.Count <= 0)
		{
			yield break;
		}
		AssetDownloadResolver.<>c__DisplayClass7_1 CS$<>8__locals2 = new AssetDownloadResolver.<>c__DisplayClass7_1();
		CS$<>8__locals2.isWindowFinish = false;
		CS$<>8__locals2.owAnswer = 0;
		CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("ムービーダウンロード"), PrjUtil.MakeMessage(string.Concat(new string[]
		{
			nam,
			"\nをダウンロードし再生します\n\n<color=",
			PrjUtil.WARNING_COLOR_CODE,
			">※通信容量が大きい為、Wi-Fi環境など、\n通信の安定した状態を推奨します。</color>\n\n",
			string.Format("ダウンロードしますか？(約{0}MB)", info.totalSize / 1000000L)
		})), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, delegate(int index)
		{
			CS$<>8__locals2.owAnswer = index;
			CS$<>8__locals2.isWindowFinish = true;
			return true;
		}, null, false);
		CanvasManager.HdlOpenWindowBasic.Open();
		while (!CS$<>8__locals2.isWindowFinish)
		{
			yield return null;
		}
		if (CS$<>8__locals2.owAnswer != 1)
		{
			if (cb != null)
			{
				cb();
			}
			yield break;
		}
		CS$<>8__locals2 = null;
		bool isFinishDownload = true;
		IEnumerator load = AssetDownloadResolver.ResolveActionLoad(info, delegate
		{
			isFinishDownload = false;
		}, true);
		while (load.MoveNext())
		{
			yield return null;
		}
		if (!isFinishDownload && cb != null)
		{
			cb();
		}
		yield break;
	}

	public class GUI
	{
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.test = baseTr.Find("Text").GetComponent<PguiTextCtrl>();
		}

		public GameObject baseObj;

		public PguiTextCtrl test;
	}

	public delegate void CancelCallBack();
}
