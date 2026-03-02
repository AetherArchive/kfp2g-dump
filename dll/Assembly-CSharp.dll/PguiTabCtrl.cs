using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001E1 RID: 481
public class PguiTabCtrl : PguiBehaviour
{
	// Token: 0x17000449 RID: 1097
	// (get) Token: 0x0600203A RID: 8250 RVA: 0x0018A587 File Offset: 0x00188787
	public bool ActEnable
	{
		get
		{
			return this.m_ActEnable;
		}
	}

	// Token: 0x1700044A RID: 1098
	// (get) Token: 0x0600203B RID: 8251 RVA: 0x0018A58F File Offset: 0x0018878F
	private Sprite mainCurrentImage
	{
		get
		{
			if (!this.m_ChgMainSub)
			{
				return this.m_CurrentImage;
			}
			return this.m_SubCurrentImage;
		}
	}

	// Token: 0x1700044B RID: 1099
	// (get) Token: 0x0600203C RID: 8252 RVA: 0x0018A5A6 File Offset: 0x001887A6
	private Sprite mainDcurrentImage
	{
		get
		{
			if (!this.m_ChgMainSub)
			{
				return this.m_DcurrentImage;
			}
			return this.m_SubDcurrentImage;
		}
	}

	// Token: 0x1700044C RID: 1100
	// (get) Token: 0x0600203D RID: 8253 RVA: 0x0018A5BD File Offset: 0x001887BD
	private Sprite subCurrentImage
	{
		get
		{
			if (!this.m_ChgMainSub)
			{
				return this.m_SubCurrentImage;
			}
			return this.m_CurrentImage;
		}
	}

	// Token: 0x1700044D RID: 1101
	// (get) Token: 0x0600203E RID: 8254 RVA: 0x0018A5D4 File Offset: 0x001887D4
	private Sprite subDcurrentImage
	{
		get
		{
			if (!this.m_ChgMainSub)
			{
				return this.m_SubDcurrentImage;
			}
			return this.m_DcurrentImage;
		}
	}

	// Token: 0x0600203F RID: 8255 RVA: 0x0018A5EC File Offset: 0x001887EC
	private void Awake()
	{
		if (this.m_BaseImage != null)
		{
			this.m_ClildText.AddRange(this.m_BaseImage.GetComponentsInChildren<PguiTextCtrl>());
		}
		if (this.m_Button != null)
		{
			this.m_Button.transition = Selectable.Transition.None;
			this.m_Button.navigation = new Navigation
			{
				mode = Navigation.Mode.None
			};
		}
	}

	// Token: 0x06002040 RID: 8256 RVA: 0x0018A654 File Offset: 0x00188854
	public void TabAction(bool tabCurrent, bool tabEnable)
	{
		if (this.m_DisableImage != null)
		{
			this.m_DisableImage.gameObject.SetActive(!tabEnable);
			this.m_Collider.enabled = tabEnable;
		}
		this.m_BaseImage.sprite = (tabCurrent ? this.mainCurrentImage : this.mainDcurrentImage);
		if (this.m_SubBaseImage != null)
		{
			this.m_SubBaseImage.sprite = (tabCurrent ? this.subCurrentImage : this.subDcurrentImage);
		}
	}

	// Token: 0x06002041 RID: 8257 RVA: 0x0018A6DC File Offset: 0x001888DC
	public void SetActEnable(bool e)
	{
		if (this.m_DisableImage != null)
		{
			this.m_DisableImage.gameObject.SetActive(!e);
		}
		this.m_BaseImage.color = (e ? Color.white : PguiTabCtrl.DisableColor);
		if (this.m_SubBaseImage != null)
		{
			this.m_SubBaseImage.color = (e ? Color.white : PguiTabCtrl.DisableColor);
		}
		for (int i = 0; i < this.m_ClildText.Count; i++)
		{
			this.m_ClildText[i].textGrayScale = (e ? 1f : PguiTabCtrl.DisableColor.r);
		}
		this.m_ActEnable = e;
		this.m_Collider.enabled = e;
	}

	// Token: 0x06002042 RID: 8258 RVA: 0x0018A79B File Offset: 0x0018899B
	public void SetChgMainSub(bool sw)
	{
		this.m_ChgMainSub = sw;
	}

	// Token: 0x0400175B RID: 5979
	public Button m_Button;

	// Token: 0x0400175C RID: 5980
	public PguiCollider m_Collider;

	// Token: 0x0400175D RID: 5981
	public Sprite m_CurrentImage;

	// Token: 0x0400175E RID: 5982
	public Sprite m_DcurrentImage;

	// Token: 0x0400175F RID: 5983
	public Image m_BaseImage;

	// Token: 0x04001760 RID: 5984
	public Image m_DisableImage;

	// Token: 0x04001761 RID: 5985
	public Sprite m_SubCurrentImage;

	// Token: 0x04001762 RID: 5986
	public Sprite m_SubDcurrentImage;

	// Token: 0x04001763 RID: 5987
	public Image m_SubBaseImage;

	// Token: 0x04001764 RID: 5988
	public List<PguiTextCtrl> m_ClildText = new List<PguiTextCtrl>();

	// Token: 0x04001765 RID: 5989
	private bool m_ActEnable = true;

	// Token: 0x04001766 RID: 5990
	private static readonly Color DisableColor = new Color(0.7f, 0.7f, 0.7f, 1f);

	// Token: 0x04001767 RID: 5991
	private bool m_ChgMainSub;
}
