using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using SGNFW.Mst;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001A3 RID: 419
public class HelpWindowCtrl : MonoBehaviour
{
	// Token: 0x06001BE3 RID: 7139 RVA: 0x00162A9F File Offset: 0x00160C9F
	public void DebugInit()
	{
	}

	// Token: 0x06001BE4 RID: 7140 RVA: 0x00162AA4 File Offset: 0x00160CA4
	public void Init(List<MstHelpData> helpData)
	{
		this.guiData = new HelpWindowCtrl.GUI(base.transform);
		this.helpBigLabelList = new List<HelpWindowCtrl.HelpBigLabelData>();
		if (helpData != null)
		{
			helpData.Sort(delegate(MstHelpData a, MstHelpData b)
			{
				if (a.priority == b.priority)
				{
					return a.id - b.id;
				}
				return a.priority - b.priority;
			});
			foreach (MstHelpData mstHelpData in helpData.FindAll((MstHelpData item) => item.labelType == 1))
			{
				this.helpBigLabelList.Add(new HelpWindowCtrl.HelpBigLabelData
				{
					id = mstHelpData.id,
					title = mstHelpData.title
				});
			}
			using (List<MstHelpData>.Enumerator enumerator = helpData.FindAll((MstHelpData item) => item.labelType == 2).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MstHelpData small = enumerator.Current;
					HelpWindowCtrl.HelpBigLabelData helpBigLabelData = this.helpBigLabelList.Find((HelpWindowCtrl.HelpBigLabelData item) => item.id == small.parentId);
					if (helpBigLabelData != null)
					{
						HelpWindowCtrl.HelpSmallLabelData helpSmallLabelData = new HelpWindowCtrl.HelpSmallLabelData
						{
							id = small.id,
							title = small.title,
							text = new List<string> { small.partsText00, small.partsText01 },
							image = new List<string> { small.partsImage00, small.partsImage01 }
						};
						helpBigLabelData.smallLabelList.Add(helpSmallLabelData);
					}
				}
			}
		}
		int count = this.helpBigLabelList.Count;
		GameObject gameObject = AssetManager.GetAssetData("SceneOption/GUI/Prefab/Help_ListBar_Big") as GameObject;
		this.bigHeight = (gameObject.transform as RectTransform).sizeDelta.y + this.SPACE_SIZE_B;
		for (int i = 0; i < count; i++)
		{
			GameObject gameObject2 = Object.Instantiate<GameObject>(gameObject, this.guiData.BigGroup.transform);
			gameObject2.name = i.ToString();
			HelpWindowCtrl.GUI.GuiLabel guiLabel = new HelpWindowCtrl.GUI.GuiLabel(gameObject2.transform);
			guiLabel.Txt.text = this.helpBigLabelList[i].title;
			guiLabel.button.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
			this.guiData.labelBList.Add(guiLabel);
		}
		int smallMaxNum = 0;
		this.helpBigLabelList.ForEach(delegate(HelpWindowCtrl.HelpBigLabelData item)
		{
			smallMaxNum = ((smallMaxNum < item.smallLabelList.Count) ? item.smallLabelList.Count : smallMaxNum);
		});
		GameObject gameObject3 = AssetManager.GetAssetData("SceneOption/GUI/Prefab/Help_ListBar_Small") as GameObject;
		this.smallHeight = (gameObject3.transform as RectTransform).sizeDelta.y + this.SPACE_SIZE_S;
		for (int j = 0; j < smallMaxNum; j++)
		{
			GameObject gameObject4 = Object.Instantiate<GameObject>(gameObject3, this.guiData.SamllGroup.transform);
			gameObject4.name = j.ToString();
			HelpWindowCtrl.GUI.GuiLabel guiLabel2 = new HelpWindowCtrl.GUI.GuiLabel(gameObject4.transform);
			guiLabel2.button.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
			this.guiData.labelSList.Add(guiLabel2);
		}
		GameObject gameObject5 = Object.Instantiate(AssetManager.GetAssetData("SceneOption/GUI/Prefab/Help_Txt"), this.guiData.ContentRect.transform) as GameObject;
		gameObject5.name = "Help_Txt";
		this.guiData.guiText = new HelpWindowCtrl.GUI.GuiText(gameObject5.transform);
		this.SetupInternal();
		this.guiData.BaseAnim.ExPauseAnimation(SimpleAnimation.ExPguiStatus.START, null);
		this.guiData.InBase.SetActive(false);
		this.guiData.BtnClose.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
	}

	// Token: 0x06001BE5 RID: 7141 RVA: 0x00162EC0 File Offset: 0x001610C0
	private void SetupInternal()
	{
		this.guiData.TextGroup.gameObject.SetActive(false);
		this.guiData.guiText.baseObj.SetActive(false);
		for (int i = 0; i < 2; i++)
		{
			this.guiData.guiText.Text[i].gameObject.SetActive(false);
			this.guiData.guiText.Image[i].gameObject.SetActive(false);
		}
		for (int j = 0; j < this.guiData.labelSList.Count; j++)
		{
			this.guiData.labelSList[j].baseObj.SetActive(false);
			this.guiData.labelSList[j].BaseImage.SetImageByName("btn_yaji_nor");
		}
		for (int k = 0; k < this.guiData.labelBList.Count; k++)
		{
			this.guiData.labelBList[k].baseRt.anchoredPosition = new Vector2(0f, this.bigHeight * (float)(-(float)k));
			this.guiData.labelBList[k].BaseImage.SetImageByName("btn_yaji_nor");
		}
		this.selectBigIndex = -1;
		this.selectSmallndex = -1;
		this.ResizeContent();
		this.guiData.ContentRect.anchoredPosition = new Vector2(0f, 0f);
		if (this.openCurrentHelplId > 0)
		{
			int num = -1;
			int num2 = -1;
			for (int l = 0; l < this.helpBigLabelList.Count; l++)
			{
				if (this.helpBigLabelList[l].id == this.openCurrentHelplId)
				{
					num = l;
					break;
				}
				for (int m = 0; m < this.helpBigLabelList[l].smallLabelList.Count; m++)
				{
					if (this.helpBigLabelList[l].smallLabelList[m].id == this.openCurrentHelplId)
					{
						num = l;
						num2 = m;
						break;
					}
				}
			}
			if (num >= 0)
			{
				IEnumerator enumerator = this.LabelEffect(true, num);
				while (enumerator.MoveNext())
				{
				}
				if (num2 >= 0)
				{
					enumerator = this.LabelEffect(false, num2);
					while (enumerator.MoveNext())
					{
					}
				}
				this.guiData.ContentRect.anchoredPosition = new Vector2(0f, -this.guiData.labelBList[num].baseRt.anchoredPosition.y);
			}
		}
	}

	// Token: 0x06001BE6 RID: 7142 RVA: 0x00163148 File Offset: 0x00161348
	private void OnClickButton(PguiButtonCtrl button)
	{
		if (button == this.guiData.BtnClose)
		{
			this.Close();
			return;
		}
		if (this.labelEffect == null)
		{
			int num = this.guiData.labelBList.FindIndex((HelpWindowCtrl.GUI.GuiLabel item) => item.button == button);
			if (num >= 0)
			{
				this.labelEffect = this.LabelEffect(true, num);
			}
		}
		if (this.labelEffect == null)
		{
			int num2 = this.guiData.labelSList.FindIndex((HelpWindowCtrl.GUI.GuiLabel item) => item.button == button);
			if (num2 >= 0)
			{
				this.labelEffect = this.LabelEffect(false, num2);
			}
		}
	}

	// Token: 0x06001BE7 RID: 7143 RVA: 0x001631EF File Offset: 0x001613EF
	private IEnumerator LabelEffect(bool isBig, int index)
	{
		if (isBig)
		{
			IEnumerator f;
			if (this.selectSmallndex >= 0)
			{
				f = this.LabelEffectCloseSmall();
				while (f.MoveNext())
				{
					yield return null;
				}
				f = null;
			}
			if (this.selectBigIndex >= 0)
			{
				f = this.LabelEffectCloseBig();
				while (f.MoveNext())
				{
					yield return null;
				}
				f = null;
			}
			this.selectSmallndex = -1;
			if (this.selectBigIndex == index)
			{
				this.selectBigIndex = -1;
				this.ResizeContent();
				yield break;
			}
			this.selectBigIndex = index;
			f = this.LabelEffectOpenBig(index, false);
			while (f.MoveNext())
			{
				yield return null;
			}
			f = null;
		}
		else
		{
			IEnumerator f;
			if (this.selectSmallndex >= 0)
			{
				f = this.LabelEffectCloseSmall();
				while (f.MoveNext())
				{
					yield return null;
				}
				f = null;
			}
			if (this.selectSmallndex == index)
			{
				this.selectSmallndex = -1;
				this.ResizeContent();
				yield break;
			}
			this.selectSmallndex = index;
			f = this.LabelEffectOpenSmall(index, false);
			while (f.MoveNext())
			{
				yield return null;
			}
			f = null;
		}
		this.ResizeContent();
		yield break;
	}

	// Token: 0x06001BE8 RID: 7144 RVA: 0x0016320C File Offset: 0x0016140C
	private IEnumerator LabelEffectOpenBig(int targetBigIndex, bool isQuick)
	{
		this.guiData.labelBList[targetBigIndex].BaseImage.SetImageByName("btn_yaji_act");
		int count = this.helpBigLabelList[targetBigIndex].smallLabelList.Count;
		this.guiData.SamllGroup.transform.SetParent(this.guiData.labelBList[targetBigIndex].anchor, false);
		this.guiData.labelBList[targetBigIndex].baseRt.SetAsLastSibling();
		for (int i = 0; i < this.guiData.labelSList.Count; i++)
		{
			if (i < count)
			{
				this.guiData.labelSList[i].baseObj.SetActive(true);
				this.guiData.labelSList[i].Txt.text = this.helpBigLabelList[targetBigIndex].smallLabelList[i].title;
			}
			else
			{
				this.guiData.labelSList[i].baseObj.SetActive(false);
			}
		}
		int cnt = 0;
		while ((float)cnt <= 4f)
		{
			float num = 0f;
			for (int j = 0; j < this.guiData.labelSList.Count; j++)
			{
				if (this.guiData.labelSList[j].baseObj.activeSelf)
				{
					float num2 = this.smallHeight * (float)(j + 1);
					num2 -= num2 / 4f * (4f - (float)cnt);
					this.guiData.labelSList[j].baseRt.anchoredPosition = new Vector2(0f, -num2);
					num = this.guiData.labelSList[j].baseRt.anchoredPosition.y;
				}
			}
			for (int k = 0; k < this.guiData.labelBList.Count; k++)
			{
				this.guiData.labelBList[k].baseRt.anchoredPosition = new Vector2(0f, this.bigHeight * (float)(-(float)k) + ((targetBigIndex < k) ? num : 0f));
			}
			if (!isQuick)
			{
				yield return null;
			}
			int num3 = cnt;
			cnt = num3 + 1;
		}
		yield break;
	}

	// Token: 0x06001BE9 RID: 7145 RVA: 0x00163229 File Offset: 0x00161429
	private IEnumerator LabelEffectOpenSmall(int targetSmallIndex, bool isQuick)
	{
		this.guiData.labelSList[targetSmallIndex].BaseImage.SetImageByName("btn_yaji_act");
		float num = 0f;
		num -= 10f;
		HelpWindowCtrl.HelpSmallLabelData helpSmallLabelData = this.helpBigLabelList[this.selectBigIndex].smallLabelList[targetSmallIndex];
		for (int i = 0; i < 2; i++)
		{
			if (i < helpSmallLabelData.text.Count && helpSmallLabelData.text[i] != string.Empty)
			{
				this.guiData.guiText.Text[i].gameObject.SetActive(true);
				this.guiData.guiText.Text[i].text = helpSmallLabelData.text[i];
				this.guiData.guiText.Text[i].m_Text.rectTransform.anchoredPosition = new Vector2(0f, num);
				this.guiData.guiText.Text[i].m_Text.rectTransform.sizeDelta = new Vector2(this.guiData.guiText.Text[i].m_Text.rectTransform.sizeDelta.x, this.guiData.guiText.Text[i].m_Text.preferredHeight);
				num -= this.guiData.guiText.Text[i].m_Text.preferredHeight;
				num -= 10f;
			}
			else
			{
				this.guiData.guiText.Text[i].gameObject.SetActive(false);
			}
			if (i < helpSmallLabelData.image.Count && helpSmallLabelData.image[i] != string.Empty)
			{
				this.guiData.guiText.Image[i].gameObject.SetActive(true);
				this.guiData.guiText.Image[i].SetRawImage(helpSmallLabelData.image[i], true, false, null);
				this.guiData.guiText.Image[i].m_RawImage.rectTransform.sizeDelta = this.ImagePath2Size(helpSmallLabelData.image[i]);
				this.guiData.guiText.Image[i].m_RawImage.rectTransform.anchoredPosition = new Vector2(0f, num);
				num -= this.guiData.guiText.Image[i].m_RawImage.rectTransform.sizeDelta.y;
				num -= 10f;
			}
			else
			{
				this.guiData.guiText.Image[i].gameObject.SetActive(false);
			}
		}
		num -= 10f;
		Vector2 sizeDelta = this.guiData.guiText.baseRt.sizeDelta;
		sizeDelta.y = -num;
		this.guiData.guiText.baseRt.sizeDelta = sizeDelta;
		this.guiData.TextGroup.transform.SetParent(this.guiData.labelSList[targetSmallIndex].anchor);
		this.guiData.TextGroup.anchoredPosition = new Vector2(0f, this.guiData.TextGroup.rect.height);
		this.guiData.TextGroup.gameObject.SetActive(true);
		this.guiData.TextGroup.transform.SetParent(this.guiData.ContentRect.transform);
		this.guiData.TextGroup.transform.SetAsFirstSibling();
		this.guiData.guiText.baseObj.transform.SetParent(this.guiData.labelSList[targetSmallIndex].anchor);
		this.guiData.guiText.baseRt.anchoredPosition = Vector3.zero;
		this.guiData.guiText.baseObj.transform.SetAsFirstSibling();
		this.guiData.guiText.baseObj.SetActive(true);
		this.guiData.guiText.baseObj.transform.SetParent(this.guiData.ContentRect.transform);
		this.guiData.guiText.baseObj.transform.SetAsFirstSibling();
		Vector2 baseTextPos = this.guiData.guiText.baseRt.anchoredPosition;
		int cnt = 0;
		while ((float)cnt <= 4f)
		{
			float num2 = this.guiData.guiText.baseRt.rect.height + this.SPACE_SIZE_S;
			num2 = num2 / 4f * (4f - (float)cnt);
			this.guiData.guiText.baseRt.anchoredPosition = baseTextPos - new Vector2(0f, -num2);
			float num3 = this.guiData.guiText.baseRt.rect.height + this.SPACE_SIZE_S - num2;
			float num4 = 0f;
			for (int j = 0; j < this.guiData.labelSList.Count; j++)
			{
				if (this.guiData.labelSList[j].baseObj.activeSelf)
				{
					this.guiData.labelSList[j].baseRt.anchoredPosition = new Vector2(0f, this.smallHeight * -((float)j + 1f) + ((targetSmallIndex < j) ? (-num3) : 0f));
					num4 = this.guiData.labelSList[j].baseRt.anchoredPosition.y + ((targetSmallIndex == j) ? (-num3) : 0f);
				}
			}
			for (int k = 0; k < this.guiData.labelBList.Count; k++)
			{
				this.guiData.labelBList[k].baseRt.anchoredPosition = new Vector2(0f, this.bigHeight * (float)(-(float)k) + ((this.selectBigIndex < k) ? num4 : 0f));
			}
			if (!isQuick)
			{
				yield return null;
			}
			int num5 = cnt;
			cnt = num5 + 1;
		}
		this.guiData.TextGroup.gameObject.SetActive(false);
		baseTextPos = default(Vector2);
		yield break;
	}

	// Token: 0x06001BEA RID: 7146 RVA: 0x00163248 File Offset: 0x00161448
	private Vector2 ImagePath2Size(string imagePath)
	{
		Vector2 vector = new Vector2(100f, 100f);
		if (imagePath.StartsWith("Texture2D/HelpImage/SIZE_L/") || imagePath.StartsWith("Texture2D/Tutorial/"))
		{
			vector = new Vector2(960f, 540f);
		}
		else if (imagePath.StartsWith("Texture2D/HelpImage/SIZE_M/") || imagePath.StartsWith("Texture2D/Tutorial_Window/"))
		{
			vector = new Vector2(999f, 405f);
		}
		else if (imagePath.StartsWith("Texture2D/HelpImage/SIZE_S/") || imagePath.StartsWith("Texture2D/Loading/"))
		{
			vector = new Vector2(512f, 300f);
		}
		return vector;
	}

	// Token: 0x06001BEB RID: 7147 RVA: 0x001632EC File Offset: 0x001614EC
	private IEnumerator LabelEffectCloseBig()
	{
		this.guiData.labelBList[this.selectBigIndex].BaseImage.SetImageByName("btn_yaji_nor");
		Vector2 anchoredPosition = this.guiData.guiText.baseRt.anchoredPosition;
		int cnt = 0;
		while ((float)cnt <= 4f)
		{
			if (this.selectBigIndex >= 0)
			{
				float num = 0f;
				for (int i = 0; i < this.guiData.labelSList.Count; i++)
				{
					if (this.guiData.labelSList[i].baseObj.activeSelf)
					{
						float num2 = this.smallHeight * (float)(i + 1);
						num2 = num2 / 4f * (4f - (float)cnt);
						this.guiData.labelSList[i].baseRt.anchoredPosition = new Vector2(0f, -num2);
						num = this.guiData.labelSList[i].baseRt.anchoredPosition.y;
					}
				}
				for (int j = 0; j < this.guiData.labelBList.Count; j++)
				{
					this.guiData.labelBList[j].baseRt.anchoredPosition = new Vector2(0f, this.bigHeight * (float)(-(float)j) + ((this.selectBigIndex < j) ? num : 0f));
				}
				yield return null;
			}
			int num3 = cnt;
			cnt = num3 + 1;
		}
		this.guiData.guiText.baseObj.SetActive(false);
		for (int k = 0; k < this.guiData.labelSList.Count; k++)
		{
			this.guiData.labelSList[k].baseObj.SetActive(false);
		}
		yield break;
	}

	// Token: 0x06001BEC RID: 7148 RVA: 0x001632FB File Offset: 0x001614FB
	private IEnumerator LabelEffectCloseSmall()
	{
		this.guiData.labelSList[this.selectSmallndex].BaseImage.SetImageByName("btn_yaji_nor");
		this.guiData.TextGroup.gameObject.SetActive(true);
		Vector2 baseTextPos = this.guiData.guiText.baseRt.anchoredPosition;
		int cnt = 0;
		while ((float)cnt <= 4f)
		{
			float num = this.guiData.guiText.baseRt.rect.height + this.SPACE_SIZE_S;
			num -= num / 4f * (4f - (float)cnt);
			this.guiData.guiText.baseRt.anchoredPosition = baseTextPos - new Vector2(0f, -num);
			float num2 = this.guiData.guiText.baseRt.rect.height + this.SPACE_SIZE_S - num;
			float num3 = 0f;
			for (int i = 0; i < this.guiData.labelSList.Count; i++)
			{
				if (this.guiData.labelSList[i].baseObj.activeSelf)
				{
					this.guiData.labelSList[i].baseRt.anchoredPosition = new Vector2(0f, this.smallHeight * -((float)i + 1f) + ((this.selectSmallndex < i) ? (-num2) : 0f));
					num3 = this.guiData.labelSList[i].baseRt.anchoredPosition.y + ((this.selectSmallndex == i) ? (-num2) : 0f);
				}
			}
			for (int j = 0; j < this.guiData.labelBList.Count; j++)
			{
				this.guiData.labelBList[j].baseRt.anchoredPosition = new Vector2(0f, this.bigHeight * (float)(-(float)j) + ((this.selectBigIndex < j) ? num3 : 0f));
			}
			yield return null;
			int num4 = cnt;
			cnt = num4 + 1;
		}
		for (int k = 0; k < 2; k++)
		{
			this.guiData.guiText.Text[k].gameObject.SetActive(false);
			this.guiData.guiText.Image[k].gameObject.SetActive(false);
		}
		this.guiData.TextGroup.gameObject.SetActive(false);
		this.guiData.guiText.baseObj.SetActive(false);
		yield break;
	}

	// Token: 0x06001BED RID: 7149 RVA: 0x0016330C File Offset: 0x0016150C
	private void ResizeContent()
	{
		float num = (float)this.guiData.labelBList.Count * this.bigHeight;
		num += (float)this.guiData.labelSList.FindAll((HelpWindowCtrl.GUI.GuiLabel item) => item.baseObj.activeSelf).Count * this.smallHeight;
		if (this.guiData.guiText.baseObj.activeSelf)
		{
			num += this.guiData.guiText.baseRt.rect.height;
		}
		num += 25f;
		num -= this.guiData.ContentRect.anchoredPosition.y;
		this.guiData.ContentRect.offsetMin = new Vector2(0f, -num);
	}

	// Token: 0x06001BEE RID: 7150 RVA: 0x001633E4 File Offset: 0x001615E4
	public void Open(bool isApppay = false)
	{
		if (isApppay)
		{
			CanvasManager.HdlCmnFeedPageWindowCtrl.Open(CmnFeedPageWindowCtrl.Type.PAGE_FEED, "", new List<string> { "Texture2D/Tutorial_Window/Apppay/tutorial_apppay_01", "Texture2D/Tutorial_Window/Apppay/tutorial_apppay_02", "Texture2D/Tutorial_Window/Apppay/tutorial_apppay_03", "Texture2D/Tutorial_Window/Apppay/tutorial_apppay_04" }, null);
			return;
		}
		SceneManager.SceneName currentSceneName = Singleton<SceneManager>.Instance.CurrentSceneName;
		if (currentSceneName <= SceneManager.SceneName.ScenePvp)
		{
			switch (currentSceneName)
			{
			case SceneManager.SceneName.SceneBattleSelector:
			case SceneManager.SceneName.SceneQuest:
				switch (this.dmEventCategory)
				{
				case DataManagerEvent.Category.Growth:
					SelEventCharaGrowCtrl.OpenHelpWindow();
					return;
				case DataManagerEvent.Category.Large:
					SelEventLargeScaleCtrl.OpenHelpWindow(this.dmEventEventId);
					return;
				case DataManagerEvent.Category.Tower:
					SelEventTowerCtrl.OpenHelpWindow();
					return;
				case DataManagerEvent.Category.Coop:
					SelEventCoopCtrl.OpenHelpWindow(this.dmEventEventId, false);
					return;
				case DataManagerEvent.Category.WildRelease:
					SelEventWildReleaseCtrl.OpenHelpWindow();
					return;
				}
				this.SetCurrentOpenHelpId(100);
				goto IL_031E;
			case SceneManager.SceneName.SceneGacha:
				this.SetCurrentOpenHelpId(300);
				goto IL_031E;
			case SceneManager.SceneName.SceneHome:
			case SceneManager.SceneName.SceneScenario:
			case SceneManager.SceneName.SceneFriend:
				break;
			case SceneManager.SceneName.SceneCharaEdit:
				this.SetCurrentOpenHelpId(200);
				goto IL_031E;
			default:
				if (currentSceneName == SceneManager.SceneName.ScenePvp)
				{
					if (this.isDispSpecialPvp)
					{
						CanvasManager.HdlCmnFeedPageWindowCtrl.Open(CmnFeedPageWindowCtrl.Type.PAGE_FEED, "", new List<string> { "Texture2D/Tutorial_Window/PvP_special/tutorial_pvp_special_01", "Texture2D/Tutorial_Window/PvP_special/tutorial_pvp_special_02", "Texture2D/Tutorial_Window/PvP_special/tutorial_pvp_special_03", "Texture2D/Tutorial_Window/PvP_special/tutorial_pvp_special_04", "Texture2D/Tutorial_Window/PvP_special/tutorial_pvp_special_05" }, null);
						return;
					}
					CanvasManager.HdlCmnFeedPageWindowCtrl.Open(CmnFeedPageWindowCtrl.Type.PAGE_FEED, "", new List<string> { "Texture2D/Tutorial_Window/PvP/tutorial_pvp_01", "Texture2D/Tutorial_Window/PvP/tutorial_pvp_02", "Texture2D/Tutorial_Window/PvP/tutorial_pvp_03", "Texture2D/Tutorial_Window/PvP/tutorial_pvp_04", "Texture2D/Tutorial_Window/PvP/tutorial_pvp_05", "Texture2D/Tutorial_Window/PvP/tutorial_pvp_06", "Texture2D/Tutorial_Window/PvP/tutorial_pvp_07" }, null);
					return;
				}
				break;
			}
		}
		else
		{
			if (currentSceneName == SceneManager.SceneName.SceneShop)
			{
				this.SetCurrentOpenHelpId(400);
				goto IL_031E;
			}
			switch (currentSceneName)
			{
			case SceneManager.SceneName.ScenePicnic:
				CanvasManager.HdlCmnFeedPageWindowCtrl.Open(CmnFeedPageWindowCtrl.Type.PAGE_FEED, "", new List<string> { "Texture2D/Tutorial_Window/Picnic/tutorial_picnic_01", "Texture2D/Tutorial_Window/Picnic/tutorial_picnic_02", "Texture2D/Tutorial_Window/Picnic/tutorial_picnic_03", "Texture2D/Tutorial_Window/Picnic/tutorial_picnic_04" }, null);
				return;
			case SceneManager.SceneName.SceneTraining:
				CanvasManager.HdlCmnFeedPageWindowCtrl.Open(CmnFeedPageWindowCtrl.Type.PAGE_FEED, "", new List<string> { "Texture2D/Tutorial_Window/Training/tutorial_training_01", "Texture2D/Tutorial_Window/Training/tutorial_training_02", "Texture2D/Tutorial_Window/Training/tutorial_training_03", "Texture2D/Tutorial_Window/Training/tutorial_training_04", "Texture2D/Tutorial_Window/Training/tutorial_training_05" }, null);
				return;
			case SceneManager.SceneName.SceneKemoBoard:
				CanvasManager.HdlCmnFeedPageWindowCtrl.Open(CmnFeedPageWindowCtrl.Type.PAGE_FEED, "", new List<string> { "Texture2D/Tutorial_Window/KemoBoard/tutorial_kemoboard_01" }, null);
				return;
			case SceneManager.SceneName.SceneTreeHouse:
				CanvasManager.HdlCmnFeedPageWindowCtrl.Open(CmnFeedPageWindowCtrl.Type.PAGE_FEED, "", new List<string> { "Texture2D/Tutorial_Window/TreeHouse/tutorial_treehouse_01", "Texture2D/Tutorial_Window/TreeHouse/tutorial_treehouse_02", "Texture2D/Tutorial_Window/TreeHouse/tutorial_treehouse_03", "Texture2D/Tutorial_Window/TreeHouse/tutorial_treehouse_04", "Texture2D/Tutorial_Window/TreeHouse/tutorial_treehouse_05", "Texture2D/Tutorial_Window/TreeHouse/tutorial_treehouse_06", "Texture2D/Tutorial_Window/TreeHouse/tutorial_treehouse_07" }, null);
				return;
			}
		}
		this.SetCurrentOpenHelpId(-1);
		IL_031E:
		if (this.m_Sequence == HelpWindowCtrl.Sequence.INACTIVE)
		{
			this.m_Sequence = HelpWindowCtrl.Sequence.OPEN_START;
		}
	}

	// Token: 0x06001BEF RID: 7151 RVA: 0x0016371F File Offset: 0x0016191F
	public void Close()
	{
		if (this.m_Sequence == HelpWindowCtrl.Sequence.ACTIVE)
		{
			this.m_ReqSequence = HelpWindowCtrl.Sequence.CLOSE_START;
		}
	}

	// Token: 0x06001BF0 RID: 7152 RVA: 0x00163731 File Offset: 0x00161931
	public void SetCurrentOpenHelpId(int helpId)
	{
		this.openCurrentHelplId = helpId;
		this.dmEventCategory = DataManagerEvent.Category.INVARID;
		this.dmEventEventId = 0;
	}

	// Token: 0x06001BF1 RID: 7153 RVA: 0x00163748 File Offset: 0x00161948
	public void SetCurrentOpenHelpByTower(bool enable)
	{
		this.dmEventCategory = (enable ? DataManagerEvent.Category.Tower : DataManagerEvent.Category.INVARID);
	}

	// Token: 0x06001BF2 RID: 7154 RVA: 0x00163757 File Offset: 0x00161957
	public void SetCurrentOpenHelpByCoop(bool enable, int eventId)
	{
		this.dmEventCategory = (enable ? DataManagerEvent.Category.Coop : DataManagerEvent.Category.INVARID);
		this.dmEventEventId = eventId;
	}

	// Token: 0x06001BF3 RID: 7155 RVA: 0x0016376D File Offset: 0x0016196D
	public void SetCurrentOpenHelpByLarge(bool enable, int eventId)
	{
		this.dmEventCategory = (enable ? DataManagerEvent.Category.Large : DataManagerEvent.Category.INVARID);
		this.dmEventEventId = eventId;
	}

	// Token: 0x06001BF4 RID: 7156 RVA: 0x00163783 File Offset: 0x00161983
	public void SetCurrentOpenHelpByCharaGrow(bool enable)
	{
		this.dmEventCategory = (enable ? DataManagerEvent.Category.Growth : DataManagerEvent.Category.INVARID);
	}

	// Token: 0x06001BF5 RID: 7157 RVA: 0x00163792 File Offset: 0x00161992
	public void SetCurrentOpenHelpByWildRelease(bool enable)
	{
		this.dmEventCategory = (enable ? DataManagerEvent.Category.WildRelease : DataManagerEvent.Category.INVARID);
	}

	// Token: 0x06001BF6 RID: 7158 RVA: 0x001637A1 File Offset: 0x001619A1
	public void SetDisplaySpecialPvpHelp(bool isSpecial)
	{
		this.isDispSpecialPvp = isSpecial;
	}

	// Token: 0x06001BF7 RID: 7159 RVA: 0x001637AC File Offset: 0x001619AC
	private void Update()
	{
		switch (this.m_Sequence)
		{
		case HelpWindowCtrl.Sequence.INACTIVE:
			if (this.m_ReqSequence == HelpWindowCtrl.Sequence.OPEN_START)
			{
				this.m_Sequence = HelpWindowCtrl.Sequence.OPEN_START;
			}
			break;
		case HelpWindowCtrl.Sequence.OPEN_START:
			this.SetupInternal();
			this.guiData.InBase.SetActive(true);
			this.guiData.BaseAnim.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
			SoundManager.Play("prd_se_help_slide", false, false);
			this.m_Sequence = HelpWindowCtrl.Sequence.OPEN_WAIT;
			break;
		case HelpWindowCtrl.Sequence.OPEN_WAIT:
			if (!this.guiData.BaseAnim.ExIsPlaying())
			{
				this.m_Sequence = HelpWindowCtrl.Sequence.ACTIVE;
			}
			break;
		case HelpWindowCtrl.Sequence.ACTIVE:
			if (this.m_ReqSequence == HelpWindowCtrl.Sequence.CALLBACK_ACTION)
			{
				SoundManager.Play("prd_se_dialog_close", false, false);
				this.m_Sequence = HelpWindowCtrl.Sequence.CALLBACK_ACTION;
			}
			else if (this.m_ReqSequence == HelpWindowCtrl.Sequence.CLOSE_START)
			{
				this.m_Sequence = HelpWindowCtrl.Sequence.CLOSE_START;
			}
			else if (this.labelEffect != null && !this.labelEffect.MoveNext())
			{
				this.labelEffect = null;
			}
			break;
		case HelpWindowCtrl.Sequence.CALLBACK_ACTION:
			this.m_Sequence = HelpWindowCtrl.Sequence.CLOSE_START;
			break;
		case HelpWindowCtrl.Sequence.CLOSE_START:
			this.guiData.BaseAnim.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, null);
			this.m_Sequence = HelpWindowCtrl.Sequence.CLOSE_WAIT;
			break;
		case HelpWindowCtrl.Sequence.CLOSE_WAIT:
			if (!this.guiData.BaseAnim.ExIsPlaying())
			{
				this.guiData.InBase.SetActive(false);
				this.m_Sequence = HelpWindowCtrl.Sequence.INACTIVE;
			}
			break;
		}
		this.m_ReqSequence = HelpWindowCtrl.Sequence.NONE;
	}

	// Token: 0x040014C5 RID: 5317
	private HelpWindowCtrl.GUI guiData;

	// Token: 0x040014C6 RID: 5318
	private HelpWindowCtrl.Sequence m_ReqSequence;

	// Token: 0x040014C7 RID: 5319
	private HelpWindowCtrl.Sequence m_Sequence = HelpWindowCtrl.Sequence.INACTIVE;

	// Token: 0x040014C8 RID: 5320
	private float smallHeight;

	// Token: 0x040014C9 RID: 5321
	private float bigHeight;

	// Token: 0x040014CA RID: 5322
	private const float OPEN_TIME = 4f;

	// Token: 0x040014CB RID: 5323
	private const float CLOSE_TIME = 4f;

	// Token: 0x040014CC RID: 5324
	public float SPACE_SIZE_B = 15f;

	// Token: 0x040014CD RID: 5325
	public float SPACE_SIZE_S = 8f;

	// Token: 0x040014CE RID: 5326
	private int selectBigIndex = -1;

	// Token: 0x040014CF RID: 5327
	private int selectSmallndex = -1;

	// Token: 0x040014D0 RID: 5328
	private DataManagerEvent.Category dmEventCategory;

	// Token: 0x040014D1 RID: 5329
	private int dmEventEventId;

	// Token: 0x040014D2 RID: 5330
	private int openCurrentHelplId = -1;

	// Token: 0x040014D3 RID: 5331
	private bool isDispSpecialPvp;

	// Token: 0x040014D4 RID: 5332
	private List<HelpWindowCtrl.HelpBigLabelData> helpBigLabelList = new List<HelpWindowCtrl.HelpBigLabelData>();

	// Token: 0x040014D5 RID: 5333
	private IEnumerator labelEffect;

	// Token: 0x02000EEB RID: 3819
	public class GUI
	{
		// Token: 0x06004E18 RID: 19992 RVA: 0x00235294 File Offset: 0x00233494
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.BtnClose = baseTr.Find("Base/Window/BtnClose").GetComponent<PguiButtonCtrl>();
			this.BtnClose.androidBackKeyTarget = true;
			this.InBase = baseTr.Find("Base").gameObject;
			this.BaseAnim = baseTr.Find("Base").GetComponent<SimpleAnimation>();
			this.ContentRect = baseTr.Find("Base/Window/Scroll View/Viewport/Content").GetComponent<RectTransform>();
			this.BigGroup = baseTr.Find("Base/Window/Scroll View/Viewport/Content/BigGroup").GetComponent<RectTransform>();
			this.SamllGroup = baseTr.Find("Base/Window/Scroll View/Viewport/Content/SamllGroup").GetComponent<RectTransform>();
			this.TextGroup = baseTr.Find("Base/Window/Scroll View/Viewport/Content/TextGroup").GetComponent<RectTransform>();
			baseTr.Find("Base/Window/Scroll View").GetComponent<ScrollRect>().scrollSensitivity = ScrollParamDefine.HelpWindow;
		}

		// Token: 0x04005537 RID: 21815
		public GameObject baseObj;

		// Token: 0x04005538 RID: 21816
		public PguiButtonCtrl BtnClose;

		// Token: 0x04005539 RID: 21817
		public GameObject InBase;

		// Token: 0x0400553A RID: 21818
		public SimpleAnimation BaseAnim;

		// Token: 0x0400553B RID: 21819
		public RectTransform ContentRect;

		// Token: 0x0400553C RID: 21820
		public RectTransform BigGroup;

		// Token: 0x0400553D RID: 21821
		public RectTransform SamllGroup;

		// Token: 0x0400553E RID: 21822
		public RectTransform TextGroup;

		// Token: 0x0400553F RID: 21823
		public List<HelpWindowCtrl.GUI.GuiLabel> labelBList = new List<HelpWindowCtrl.GUI.GuiLabel>();

		// Token: 0x04005540 RID: 21824
		public List<HelpWindowCtrl.GUI.GuiLabel> labelSList = new List<HelpWindowCtrl.GUI.GuiLabel>();

		// Token: 0x04005541 RID: 21825
		public HelpWindowCtrl.GUI.GuiText guiText;

		// Token: 0x020011F4 RID: 4596
		public class GuiText
		{
			// Token: 0x06005766 RID: 22374 RVA: 0x002574E8 File Offset: 0x002556E8
			public GuiText(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.baseRt = baseTr as RectTransform;
				this.Base_Image = baseTr.Find("Base_Image").GetComponent<PguiImageCtrl>();
				this.Image = new List<PguiRawImageCtrl>
				{
					baseTr.Find("Base_Image/Image0").GetComponent<PguiRawImageCtrl>(),
					baseTr.Find("Base_Image/Image1").GetComponent<PguiRawImageCtrl>()
				};
				this.Text = new List<PguiTextCtrl>
				{
					baseTr.Find("Base_Image/Text0").GetComponent<PguiTextCtrl>(),
					baseTr.Find("Base_Image/Text1").GetComponent<PguiTextCtrl>()
				};
			}

			// Token: 0x04006274 RID: 25204
			public RectTransform baseRt;

			// Token: 0x04006275 RID: 25205
			public GameObject baseObj;

			// Token: 0x04006276 RID: 25206
			public PguiImageCtrl Base_Image;

			// Token: 0x04006277 RID: 25207
			public List<PguiRawImageCtrl> Image;

			// Token: 0x04006278 RID: 25208
			public List<PguiTextCtrl> Text;
		}

		// Token: 0x020011F5 RID: 4597
		public class GuiLabel
		{
			// Token: 0x06005767 RID: 22375 RVA: 0x00257598 File Offset: 0x00255798
			public GuiLabel(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.baseRt = baseTr as RectTransform;
				this.button = baseTr.GetComponent<PguiButtonCtrl>();
				this.BaseImage = baseTr.Find("BaseImage/BaseImage").GetComponent<PguiImageCtrl>();
				this.BaseImage.SetImageByName("btn_yaji_nor");
				this.Txt = baseTr.Find("BaseImage/Txt").GetComponent<PguiTextCtrl>();
				this.anchor = baseTr.Find("Anchor").transform;
			}

			// Token: 0x04006279 RID: 25209
			public RectTransform baseRt;

			// Token: 0x0400627A RID: 25210
			public GameObject baseObj;

			// Token: 0x0400627B RID: 25211
			public PguiButtonCtrl button;

			// Token: 0x0400627C RID: 25212
			public PguiImageCtrl BaseImage;

			// Token: 0x0400627D RID: 25213
			public PguiTextCtrl Txt;

			// Token: 0x0400627E RID: 25214
			public Transform anchor;
		}
	}

	// Token: 0x02000EEC RID: 3820
	private enum Sequence
	{
		// Token: 0x04005543 RID: 21827
		NONE,
		// Token: 0x04005544 RID: 21828
		INACTIVE,
		// Token: 0x04005545 RID: 21829
		OPEN_START,
		// Token: 0x04005546 RID: 21830
		OPEN_WAIT,
		// Token: 0x04005547 RID: 21831
		ACTIVE,
		// Token: 0x04005548 RID: 21832
		CALLBACK_ACTION,
		// Token: 0x04005549 RID: 21833
		CLOSE_START,
		// Token: 0x0400554A RID: 21834
		CLOSE_WAIT
	}

	// Token: 0x02000EED RID: 3821
	private class HelpBigLabelData
	{
		// Token: 0x0400554B RID: 21835
		public int id;

		// Token: 0x0400554C RID: 21836
		public string title;

		// Token: 0x0400554D RID: 21837
		public List<HelpWindowCtrl.HelpSmallLabelData> smallLabelList = new List<HelpWindowCtrl.HelpSmallLabelData>();
	}

	// Token: 0x02000EEE RID: 3822
	private class HelpSmallLabelData
	{
		// Token: 0x0400554E RID: 21838
		public int id;

		// Token: 0x0400554F RID: 21839
		public const int ARRAY_SIZE = 2;

		// Token: 0x04005550 RID: 21840
		public string title;

		// Token: 0x04005551 RID: 21841
		public List<string> text = new List<string>();

		// Token: 0x04005552 RID: 21842
		public List<string> image = new List<string>();
	}
}
