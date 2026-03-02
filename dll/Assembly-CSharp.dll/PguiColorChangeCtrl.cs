using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001CF RID: 463
public class PguiColorChangeCtrl : PguiBehaviour
{
	// Token: 0x06001FA2 RID: 8098 RVA: 0x00187128 File Offset: 0x00185328
	private void Init()
	{
		this.m_RawImage = base.gameObject.GetComponent<RawImage>();
	}

	// Token: 0x06001FA3 RID: 8099 RVA: 0x0018713B File Offset: 0x0018533B
	private void Start()
	{
		this.Init();
	}

	// Token: 0x06001FA4 RID: 8100 RVA: 0x00187143 File Offset: 0x00185343
	private void Update()
	{
	}

	// Token: 0x06001FA5 RID: 8101 RVA: 0x00187145 File Offset: 0x00185345
	public void OnValidate()
	{
		this.SetEnable(base.enabled);
	}

	// Token: 0x06001FA6 RID: 8102 RVA: 0x00187154 File Offset: 0x00185354
	public void SetEnable(bool sw)
	{
		if (this.m_RawImage == null)
		{
			this.Init();
		}
		if (this.m_RawImage != null)
		{
			if (sw)
			{
				this.m_RawImage.material = new Material(Shader.Find("UI/ColorChange"));
				this.m_RawImage.material.color = this.m_Color;
				return;
			}
			this.m_RawImage.material = new Material(Shader.Find("UI/Default"));
		}
	}

	// Token: 0x04001703 RID: 5891
	public Color m_Color;

	// Token: 0x04001704 RID: 5892
	private RawImage m_RawImage;
}
