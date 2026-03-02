using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001FC RID: 508
public class uGUISpritAnimation : MonoBehaviour
{
	// Token: 0x0600216C RID: 8556 RVA: 0x0018F594 File Offset: 0x0018D794
	private void Start()
	{
		if (this.LTime <= 0f)
		{
			base.GetComponent<uGUISpritAnimation>().enabled = false;
		}
		if (base.GetComponent<Image>() == null)
		{
			base.gameObject.AddComponent<Image>();
		}
		this.WTime -= this.delay / this.LTime;
		this.m_image = base.GetComponent<Image>();
	}

	// Token: 0x0600216D RID: 8557 RVA: 0x0018F5FC File Offset: 0x0018D7FC
	private void Update()
	{
		this.WTime += Time.deltaTime / this.LTime;
		if (1f <= this.WTime && this.Loop)
		{
			this.WTime = 0f;
		}
		this.ACurvTime = Mathf.Clamp(this.AA.Evaluate(this.WTime), 0f, 1f);
		this.EVCount = Mathf.FloorToInt(this.ACurvTime * (float)this._image.Length);
		if (this._image.Length <= this.EVCount)
		{
			this.EVCount = 0;
		}
		this.m_image.sprite = this._image[this.EVCount];
	}

	// Token: 0x04001811 RID: 6161
	[SerializeField]
	public AnimationCurve AA;

	// Token: 0x04001812 RID: 6162
	public float delay = 0.5f;

	// Token: 0x04001813 RID: 6163
	public float LTime = 2f;

	// Token: 0x04001814 RID: 6164
	public bool Loop = true;

	// Token: 0x04001815 RID: 6165
	private float ACurvTime;

	// Token: 0x04001816 RID: 6166
	private float WTime;

	// Token: 0x04001817 RID: 6167
	public Sprite[] _image;

	// Token: 0x04001818 RID: 6168
	private Image m_image;

	// Token: 0x04001819 RID: 6169
	private int EVCount;

	// Token: 0x0400181A RID: 6170
	private Transform _tran;
}
