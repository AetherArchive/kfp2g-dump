using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeAuthUICtrl
{
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

	public void SetupArtsUI(CharaStaticData chara)
	{
		this._kemonoArtsAE.GetComponent<PguiReplaceAECtrl>().Replace("NORMAL");
		this._kemonoArtsAE.transform.Find("Txt_CharaName").GetComponent<PguiTextCtrl>().text = chara.GetName();
		this._kemonoArtsAE.transform.Find("SkillName/Txt_SkillName").GetComponent<PguiTextCtrl>().text = chara.artsData.actionName;
		this._kemonoArtsAE.PlayAnimation(PguiAECtrl.AmimeType.START, null);
		this._kemonoArtsAE.gameObject.SetActive(true);
	}

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

	public void HideArtsUI()
	{
		this._kemonoArtsAE.gameObject.SetActive(false);
	}

	public void HideMovieImage()
	{
		this._movieImage.gameObject.SetActive(false);
	}

	public void ShowBannerImages()
	{
		foreach (PguiRawImageCtrl pguiRawImageCtrl in this._bannerImages)
		{
			pguiRawImageCtrl.gameObject.SetActive(true);
		}
	}

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

	public void SetOnClickButtonSkipListner(PguiButtonCtrl.OnClick callBack)
	{
		this._buttonSkip.AddOnClickListener(callBack, PguiButtonCtrl.SoundType.DEFAULT);
	}

	public void ActivateButtonSKip()
	{
		this._buttonSkip.gameObject.SetActive(true);
	}

	public void ActivateOutFlame()
	{
		this._outFrame.SetActive(true);
	}

	private static readonly string INTRO_MOVIE_PATH = "NewFriendsIntro";

	private static readonly string INTRO_MOVIE_PATH_NIGHT = "NewFriendsIntroNight";

	private static readonly string INTRO_MOVIE_PATH_CHARACTER = "NewFriendsIntroCharacter";

	private static readonly string OUTRO_MOVIE_PATH = "NewFriendsOutro_FCP";

	private GameObject _basePanel;

	private PguiAECtrl _kemonoArtsAE;

	private PguiRawImageCtrl _movieImage;

	private PguiButtonCtrl _buttonSkip;

	private PguiRawImageCtrl _bannerBase;

	private List<PguiRawImageCtrl> _bannerImages;

	private GameObject _outFrame;
}
