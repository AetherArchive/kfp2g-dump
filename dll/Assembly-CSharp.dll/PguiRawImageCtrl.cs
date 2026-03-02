using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SGNFW.Ab;
using SGNFW.Common;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020001DA RID: 474
public class PguiRawImageCtrl : PguiBehaviour
{
	// Token: 0x06002007 RID: 8199 RVA: 0x00189D64 File Offset: 0x00187F64
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

	// Token: 0x06002008 RID: 8200 RVA: 0x00189DE8 File Offset: 0x00187FE8
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

	// Token: 0x06002009 RID: 8201 RVA: 0x00189E38 File Offset: 0x00188038
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

	// Token: 0x0600200A RID: 8202 RVA: 0x00189E5C File Offset: 0x0018805C
	public void SetRaycastTarget(bool enabled)
	{
		if (null != this.m_RawImage)
		{
			this.m_RawImage.raycastTarget = enabled;
		}
	}

	// Token: 0x17000444 RID: 1092
	// (set) Token: 0x0600200B RID: 8203 RVA: 0x00189E78 File Offset: 0x00188078
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

	// Token: 0x17000445 RID: 1093
	// (set) Token: 0x0600200C RID: 8204 RVA: 0x00189EAD File Offset: 0x001880AD
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

	// Token: 0x0600200D RID: 8205 RVA: 0x00189EE2 File Offset: 0x001880E2
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

	// Token: 0x0600200E RID: 8206 RVA: 0x00189F00 File Offset: 0x00188100
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

	// Token: 0x0600200F RID: 8207 RVA: 0x00189F98 File Offset: 0x00188198
	private void OnDestroy()
	{
		this.DestroyInternal();
	}

	// Token: 0x06002010 RID: 8208 RVA: 0x00189FA0 File Offset: 0x001881A0
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

	// Token: 0x06002011 RID: 8209 RVA: 0x0018A02B File Offset: 0x0018822B
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

	// Token: 0x04001742 RID: 5954
	public RawImage m_RawImage;

	// Token: 0x04001743 RID: 5955
	public bool customize;

	// Token: 0x04001744 RID: 5956
	private string loadAssetName;

	// Token: 0x04001745 RID: 5957
	private IEnumerator assetLoadSetRoutine;

	// Token: 0x04001746 RID: 5958
	private static Dictionary<string, Object> bannerImage;

	// Token: 0x04001747 RID: 5959
	private IEnumerator mBannerLoad;
}
