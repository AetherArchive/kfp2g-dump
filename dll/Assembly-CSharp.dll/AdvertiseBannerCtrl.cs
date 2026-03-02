using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Login;
using UnityEngine;

// Token: 0x02000190 RID: 400
public class AdvertiseBannerCtrl : MonoBehaviour
{
	// Token: 0x06001AAA RID: 6826 RVA: 0x001579A4 File Offset: 0x00155BA4
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

	// Token: 0x06001AAB RID: 6827 RVA: 0x00157B18 File Offset: 0x00155D18
	public void StartProgress()
	{
		this.animStart = true;
	}

	// Token: 0x06001AAC RID: 6828 RVA: 0x00157B21 File Offset: 0x00155D21
	public void ClaerProgress()
	{
		if (this.guiDataList != null && this.guiDataList.Count > 0)
		{
			this.guiDataList.Clear();
		}
		this.forceAnimEnd = true;
	}

	// Token: 0x06001AAD RID: 6829 RVA: 0x00157B4C File Offset: 0x00155D4C
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

	// Token: 0x06001AAE RID: 6830 RVA: 0x00157BBB File Offset: 0x00155DBB
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

	// Token: 0x06001AAF RID: 6831 RVA: 0x00157BD1 File Offset: 0x00155DD1
	private void OnTouchCloseButton()
	{
		this.forceAnimEnd = true;
	}

	// Token: 0x06001AB0 RID: 6832 RVA: 0x00157BDA File Offset: 0x00155DDA
	private void OnTouchBanner(string link)
	{
		Application.OpenURL(link);
		this.forceAnimEnd = true;
	}

	// Token: 0x0400144A RID: 5194
	private List<AdvertiseBannerCtrl.GUI> guiDataList;

	// Token: 0x0400144B RID: 5195
	private bool animStart;

	// Token: 0x0400144C RID: 5196
	private bool forceAnimEnd;

	// Token: 0x0400144D RID: 5197
	private IEnumerator currentPopupMission;

	// Token: 0x02000E82 RID: 3714
	public class GUI
	{
		// Token: 0x06004CE6 RID: 19686 RVA: 0x0022FA79 File Offset: 0x0022DC79
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.BannerAnim = baseTr.Find("Banner").GetComponent<SimpleAnimation>();
			this.Cmn_Btn_Close = baseTr.Find("Banner/Cmn_Btn_Close").GetComponent<PguiButtonCtrl>();
		}

		// Token: 0x0400535F RID: 21343
		public GameObject baseObj;

		// Token: 0x04005360 RID: 21344
		public SimpleAnimation BannerAnim;

		// Token: 0x04005361 RID: 21345
		public PguiButtonCtrl Cmn_Btn_Close;
	}
}
