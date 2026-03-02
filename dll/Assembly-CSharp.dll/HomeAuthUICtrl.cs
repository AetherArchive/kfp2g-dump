using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200014B RID: 331
public class HomeAuthUICtrl
{
	// Token: 0x17000376 RID: 886
	// (get) Token: 0x06001271 RID: 4721 RVA: 0x000DF46D File Offset: 0x000DD66D
	// (set) Token: 0x06001272 RID: 4722 RVA: 0x000DF475 File Offset: 0x000DD675
	public PguiButtonCtrl ButtonSkip
	{
		get
		{
			return this._buttonSkip;
		}
		private set
		{
		}
	}

	// Token: 0x06001273 RID: 4723 RVA: 0x000DF478 File Offset: 0x000DD678
	public HomeAuthUICtrl()
	{
		Transform transform = Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneHome/GUI/Prefab/GUI_HomeAuthParts")).transform;
		this._basePanel = transform.gameObject;
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this._basePanel.transform, true);
		this._kemonoArtsAE = transform.Find("AEImage_KemonoArtsAuth").GetComponent<PguiAECtrl>();
		this._movieImage = transform.Find("MovieMask/Chara_MovieImage").GetComponent<PguiRawImageCtrl>();
		this._buttonSkip = transform.Find("Btn_Skip").GetComponent<PguiButtonCtrl>();
		this._bannerBase = transform.Find("Banner/base").GetComponent<PguiRawImageCtrl>();
		this._bannerImages = new List<PguiRawImageCtrl>();
		this._outFrame = transform.Find("OutFrame").gameObject;
		this._kemonoArtsAE.gameObject.SetActive(false);
		this._movieImage.gameObject.SetActive(false);
		this._bannerBase.gameObject.SetActive(false);
		this._buttonSkip.gameObject.SetActive(false);
		this._outFrame.SetActive(false);
		this._movieImage.gameObject.AddComponent<MoviePlayer>();
	}

	// Token: 0x06001274 RID: 4724 RVA: 0x000DF5A0 File Offset: 0x000DD7A0
	public void Destroy()
	{
		Object.Destroy(this._basePanel);
		Object.Destroy(this._kemonoArtsAE);
		Object.Destroy(this._movieImage);
		Object.Destroy(this._buttonSkip);
		Object.Destroy(this._bannerBase);
		this._basePanel = null;
		this._kemonoArtsAE = null;
		this._movieImage = null;
		this._buttonSkip = null;
		this._bannerBase = null;
	}

	// Token: 0x06001275 RID: 4725 RVA: 0x000DF608 File Offset: 0x000DD808
	public void SetupArtsUI(CharaStaticData chara)
	{
		this._kemonoArtsAE.GetComponent<PguiReplaceAECtrl>().Replace("NORMAL");
		this._kemonoArtsAE.transform.Find("Txt_CharaName").GetComponent<PguiTextCtrl>().text = chara.GetName();
		this._kemonoArtsAE.transform.Find("SkillName/Txt_SkillName").GetComponent<PguiTextCtrl>().text = chara.artsData.actionName;
		this._kemonoArtsAE.PlayAnimation(PguiAECtrl.AmimeType.START, null);
		this._kemonoArtsAE.gameObject.SetActive(true);
	}

	// Token: 0x06001276 RID: 4726 RVA: 0x000DF698 File Offset: 0x000DD898
	public void SetupBannerImages()
	{
		List<HomeBannerData> list = new List<HomeBannerData>();
		foreach (int num in DataManager.DmIntroduction.GetBannerIds())
		{
			HomeBannerData homeBannerData = DataManager.DmHome.GetHomeBannerData(num);
			if (homeBannerData != null)
			{
				list.Add(homeBannerData);
				if (list.Count == 3)
				{
					break;
				}
			}
		}
		foreach (HomeBannerData homeBannerData2 in list)
		{
			PguiRawImageCtrl component = Object.Instantiate<GameObject>(this._bannerBase.gameObject, this._bannerBase.transform.parent, true).GetComponent<PguiRawImageCtrl>();
			component.banner = homeBannerData2.bannerImagePath;
			component.name = "banner_" + this._bannerImages.Count.ToString();
			this._bannerImages.Add(component);
		}
		this._bannerBase.gameObject.SetActive(false);
	}

	// Token: 0x06001277 RID: 4727 RVA: 0x000DF7C0 File Offset: 0x000DD9C0
	public void HideArtsUI()
	{
		this._kemonoArtsAE.gameObject.SetActive(false);
	}

	// Token: 0x06001278 RID: 4728 RVA: 0x000DF7D3 File Offset: 0x000DD9D3
	public void HideMovieImage()
	{
		this._movieImage.gameObject.SetActive(false);
	}

	// Token: 0x06001279 RID: 4729 RVA: 0x000DF7E8 File Offset: 0x000DD9E8
	public void ShowBannerImages()
	{
		foreach (PguiRawImageCtrl pguiRawImageCtrl in this._bannerImages)
		{
			pguiRawImageCtrl.gameObject.SetActive(true);
		}
	}

	// Token: 0x0600127A RID: 4730 RVA: 0x000DF840 File Offset: 0x000DDA40
	public IEnumerator PlayMovie(bool isCharacterMovie = false, CharaStaticData chara = null)
	{
		this._movieImage.gameObject.SetActive(true);
		MoviePlayer component = this._movieImage.gameObject.GetComponent<MoviePlayer>();
		float categoryVolume = SoundManager.GetCategoryVolume(SoundCategory.SE);
		component.SetVolume(categoryVolume);
		GameObject charaMovieObj = this._movieImage.gameObject;
		string text;
		if (chara != null)
		{
			text = ((chara.baseData.OriginalId == 0) ? HomeAuthUICtrl.INTRO_MOVIE_PATH : HomeAuthUICtrl.INTRO_MOVIE_PATH_NIGHT);
			if (isCharacterMovie)
			{
				text = HomeAuthUICtrl.INTRO_MOVIE_PATH_CHARACTER;
			}
		}
		else
		{
			text = HomeAuthUICtrl.OUTRO_MOVIE_PATH;
		}
		if (AssetManager.IsExsistAssetData(AssetManager.PREFIX_PATH_MOVIE + text))
		{
			MoviePlayer.Play(charaMovieObj, text, false);
			while (MoviePlayer.Playing(charaMovieObj))
			{
				yield return null;
			}
			charaMovieObj.SetActive(false);
			yield break;
		}
		yield break;
	}

	// Token: 0x0600127B RID: 4731 RVA: 0x000DF85D File Offset: 0x000DDA5D
	public void SetOnClickButtonSkipListner(PguiButtonCtrl.OnClick callBack)
	{
		this._buttonSkip.AddOnClickListener(callBack, PguiButtonCtrl.SoundType.DEFAULT);
	}

	// Token: 0x0600127C RID: 4732 RVA: 0x000DF86C File Offset: 0x000DDA6C
	public void ActivateButtonSKip()
	{
		this._buttonSkip.gameObject.SetActive(true);
	}

	// Token: 0x0600127D RID: 4733 RVA: 0x000DF87F File Offset: 0x000DDA7F
	public void ActivateOutFlame()
	{
		this._outFrame.SetActive(true);
	}

	// Token: 0x04000F2B RID: 3883
	private static readonly string INTRO_MOVIE_PATH = "NewFriendsIntro";

	// Token: 0x04000F2C RID: 3884
	private static readonly string INTRO_MOVIE_PATH_NIGHT = "NewFriendsIntroNight";

	// Token: 0x04000F2D RID: 3885
	private static readonly string INTRO_MOVIE_PATH_CHARACTER = "NewFriendsIntroCharacter";

	// Token: 0x04000F2E RID: 3886
	private static readonly string OUTRO_MOVIE_PATH = "NewFriendsOutro_FCP";

	// Token: 0x04000F2F RID: 3887
	private GameObject _basePanel;

	// Token: 0x04000F30 RID: 3888
	private PguiAECtrl _kemonoArtsAE;

	// Token: 0x04000F31 RID: 3889
	private PguiRawImageCtrl _movieImage;

	// Token: 0x04000F32 RID: 3890
	private PguiButtonCtrl _buttonSkip;

	// Token: 0x04000F33 RID: 3891
	private PguiRawImageCtrl _bannerBase;

	// Token: 0x04000F34 RID: 3892
	private List<PguiRawImageCtrl> _bannerImages;

	// Token: 0x04000F35 RID: 3893
	private GameObject _outFrame;
}
