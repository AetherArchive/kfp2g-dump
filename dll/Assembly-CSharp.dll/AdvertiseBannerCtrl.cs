using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Login;
using UnityEngine;

public class AdvertiseBannerCtrl : MonoBehaviour
{
	public void Init()
	{
		List<AdvertiseBannerData> advertiseBannerDataList = DataManager.DmServerMst.advertiseBannerDataList;
		this.guiDataList = new List<AdvertiseBannerCtrl.GUI>();
		using (List<AdvertiseBannerData>.Enumerator enumerator = advertiseBannerDataList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				AdvertiseBannerData banner = enumerator.Current;
				if (!(banner.startTime > TimeManager.Now) && !(TimeManager.Now > banner.endTime) && (banner.platform == 0 || banner.platform == LoginManager.Platform))
				{
					AdvertiseBannerCtrl.GUI gui = new AdvertiseBannerCtrl.GUI(AssetManager.InstantiateAssetData("Cmn/GUI/Prefab/Cmn_AdvertiseBanner", base.transform).transform);
					gui.BannerAnim.transform.GetComponent<PguiRawImageCtrl>().banner = banner.bannerImagePath;
					gui.BannerAnim.ExPauseAnimation(SimpleAnimation.ExPguiStatus.START, null);
					gui.Cmn_Btn_Close.AddOnClickListener(delegate(PguiButtonCtrl x)
					{
						this.OnTouchCloseButton();
					}, PguiButtonCtrl.SoundType.INVALID);
					gui.BannerAnim.gameObject.AddComponent<PguiTouchTrigger>().AddListener(delegate
					{
						this.OnTouchBanner(banner.actionParamURL);
					}, null, null, null, null);
					gui.baseObj.SetActive(false);
					this.guiDataList.Add(gui);
				}
			}
		}
	}

	public void StartProgress()
	{
		this.animStart = true;
	}

	public void ClaerProgress()
	{
		if (this.guiDataList != null && this.guiDataList.Count > 0)
		{
			this.guiDataList.Clear();
		}
		this.forceAnimEnd = true;
	}

	private void Update()
	{
		if (!this.animStart)
		{
			return;
		}
		if (this.currentPopupMission != null)
		{
			if (!this.currentPopupMission.MoveNext())
			{
				this.currentPopupMission = null;
				return;
			}
		}
		else if (this.guiDataList != null && this.guiDataList.Count > 0)
		{
			AdvertiseBannerCtrl.GUI gui = this.guiDataList[0];
			this.guiDataList.RemoveAt(0);
			this.currentPopupMission = this.PopupAdvertise(gui);
		}
	}

	private IEnumerator PopupAdvertise(AdvertiseBannerCtrl.GUI guiData)
	{
		guiData.baseObj.SetActive(true);
		guiData.BannerAnim.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
		while (guiData.BannerAnim.ExIsPlaying())
		{
			yield return null;
		}
		while (!this.forceAnimEnd)
		{
			yield return null;
		}
		guiData.BannerAnim.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, null);
		while (guiData.BannerAnim.ExIsPlaying())
		{
			yield return null;
		}
		this.forceAnimEnd = false;
		guiData.baseObj.SetActive(false);
		if (this.guiDataList.Count <= 0)
		{
			Object.Destroy(base.gameObject);
		}
		yield break;
	}

	private void OnTouchCloseButton()
	{
		this.forceAnimEnd = true;
	}

	private void OnTouchBanner(string link)
	{
		Application.OpenURL(link);
		this.forceAnimEnd = true;
	}

	private List<AdvertiseBannerCtrl.GUI> guiDataList;

	private bool animStart;

	private bool forceAnimEnd;

	private IEnumerator currentPopupMission;

	public class GUI
	{
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.BannerAnim = baseTr.Find("Banner").GetComponent<SimpleAnimation>();
			this.Cmn_Btn_Close = baseTr.Find("Banner/Cmn_Btn_Close").GetComponent<PguiButtonCtrl>();
		}

		public GameObject baseObj;

		public SimpleAnimation BannerAnim;

		public PguiButtonCtrl Cmn_Btn_Close;
	}
}
