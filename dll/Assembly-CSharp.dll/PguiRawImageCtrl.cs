using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SGNFW.Ab;
using SGNFW.Common;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PguiRawImageCtrl : PguiBehaviour
{
	public void SetRawImage(string imageName, bool isLoadAsset, bool native = false, UnityAction loadFinishCallBack = null)
	{
		if (Singleton<SceneManager>.Instance == null)
		{
			return;
		}
		this.DestroyLoadingData();
		if (isLoadAsset)
		{
			this.assetLoadSetRoutine = this.LoadAndSetRawImage(imageName, native, loadFinishCallBack);
			Singleton<SceneManager>.Instance.StartCoroutine(this.assetLoadSetRoutine);
			return;
		}
		this.m_RawImage.enabled = true;
		this.m_RawImage.texture = AssetManager.GetAssetData(imageName) as Texture;
		if (native)
		{
			this.m_RawImage.SetNativeSize();
		}
		if (loadFinishCallBack != null)
		{
			loadFinishCallBack();
		}
	}

	public void SetTexture(Texture tex, bool native = true)
	{
		if (Singleton<SceneManager>.Instance == null)
		{
			return;
		}
		this.DestroyLoadingData();
		this.m_RawImage.enabled = true;
		this.m_RawImage.texture = tex;
		if (native && tex != null)
		{
			this.m_RawImage.SetNativeSize();
		}
	}

	private IEnumerator LoadAndSetRawImage(string imageName, bool native, UnityAction loadFinishCallBack)
	{
		this.loadAssetName = imageName;
		AssetManager.LoadAssetData(this.loadAssetName, AssetManager.OWNER.PguiWrapper, 1, null);
		while (!AssetManager.IsLoadFinishAssetData(this.loadAssetName))
		{
			yield return null;
		}
		if (null != this.m_RawImage)
		{
			this.m_RawImage.enabled = true;
			this.m_RawImage.texture = AssetManager.GetAssetData(this.loadAssetName) as Texture;
			if (native)
			{
				this.m_RawImage.SetNativeSize();
			}
		}
		if (loadFinishCallBack != null)
		{
			loadFinishCallBack();
		}
		yield break;
	}

	public void SetRaycastTarget(bool enabled)
	{
		if (null != this.m_RawImage)
		{
			this.m_RawImage.raycastTarget = enabled;
		}
	}

	public string banner
	{
		set
		{
			this.DestroyLoadingData();
			if (this.m_RawImage != null)
			{
				this.mBannerLoad = this.BannerLoad(value, false);
				Singleton<SceneManager>.Instance.StartCoroutine(this.mBannerLoad);
			}
		}
	}

	public string bannerOnly
	{
		set
		{
			this.DestroyLoadingData();
			if (this.m_RawImage != null)
			{
				this.mBannerLoad = this.BannerLoad(value, true);
				Singleton<SceneManager>.Instance.StartCoroutine(this.mBannerLoad);
			}
		}
	}

	private IEnumerator BannerLoad(string bnr, bool isBannerOnly = false)
	{
		if (!bnr.StartsWith("Texture2D/"))
		{
			yield break;
		}
		string key = Path.GetFileNameWithoutExtension(bnr);
		if (string.IsNullOrEmpty(key))
		{
			yield break;
		}
		if (PguiRawImageCtrl.bannerImage == null)
		{
			PguiRawImageCtrl.bannerImage = new Dictionary<string, Object>();
		}
		if (!PguiRawImageCtrl.bannerImage.ContainsKey(key) || PguiRawImageCtrl.bannerImage[key] == null)
		{
			PguiRawImageCtrl.bannerImage[key] = null;
			string text = Manager.DownloadUrl;
			text = text.Substring(0, text.IndexOf("/AssetBundles/") + 1);
			long tim = TimeManager.SystemNow.Ticks;
			WWW www = new WWW(text + bnr + ".png?tim=" + tim.ToString());
			for (;;)
			{
				yield return null;
				if (www.isDone)
				{
					break;
				}
				if ((TimeManager.SystemNow.Ticks - tim) / 10000000L >= 3L)
				{
					goto IL_0170;
				}
			}
			if (string.IsNullOrEmpty(www.error))
			{
				PguiRawImageCtrl.bannerImage[key] = Object.Instantiate<Texture2D>(www.texture);
			}
			IL_0170:
			www.Dispose();
			www = null;
			www = null;
		}
		if (PguiRawImageCtrl.bannerImage[key] != null)
		{
			this.m_RawImage.enabled = true;
			this.m_RawImage.texture = PguiRawImageCtrl.bannerImage[key] as Texture;
			this.m_RawImage.texture.wrapMode = TextureWrapMode.Clamp;
		}
		else if (!isBannerOnly)
		{
			this.SetRawImage(bnr, true, false, null);
		}
		yield break;
	}

	public static void ClearBanner()
	{
		if (PguiRawImageCtrl.bannerImage != null)
		{
			foreach (string text in new List<string>(PguiRawImageCtrl.bannerImage.Keys))
			{
				if (PguiRawImageCtrl.bannerImage[text] != null)
				{
					Object.Destroy(PguiRawImageCtrl.bannerImage[text]);
					PguiRawImageCtrl.bannerImage[text] = null;
				}
			}
			PguiRawImageCtrl.bannerImage = null;
			PrjUtil.ReleaseMemory(1);
		}
	}

	private void OnDestroy()
	{
		this.DestroyInternal();
	}

	private void DestroyLoadingData()
	{
		if (this.assetLoadSetRoutine != null && Singleton<SceneManager>.Instance != null)
		{
			Singleton<SceneManager>.Instance.StopCoroutine(this.assetLoadSetRoutine);
			this.assetLoadSetRoutine = null;
		}
		if (this.mBannerLoad != null && Singleton<SceneManager>.Instance != null)
		{
			Singleton<SceneManager>.Instance.StopCoroutine(this.mBannerLoad);
			this.mBannerLoad = null;
		}
		if (null != this.m_RawImage)
		{
			this.m_RawImage.texture = null;
			this.m_RawImage.enabled = false;
		}
	}

	private void DestroyInternal()
	{
		this.DestroyLoadingData();
		if (this.loadAssetName != null)
		{
			AssetManager.UnloadAssetData(this.loadAssetName, AssetManager.OWNER.PguiWrapper);
			this.loadAssetName = null;
			PrjUtil.ReleaseMemory(1);
		}
	}

	public RawImage m_RawImage;

	public bool customize;

	private string loadAssetName;

	private IEnumerator assetLoadSetRoutine;

	private static Dictionary<string, Object> bannerImage;

	private IEnumerator mBannerLoad;
}
