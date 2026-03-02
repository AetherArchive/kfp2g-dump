using System;
using UnityEngine;

// Token: 0x02000017 RID: 23
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Hue, Saturation, Value")]
public class CC_HueSaturationValue : CC_Base
{
	// Token: 0x17000002 RID: 2
	// (get) Token: 0x0600003A RID: 58 RVA: 0x00002FE5 File Offset: 0x000011E5
	// (set) Token: 0x0600003B RID: 59 RVA: 0x00002FED File Offset: 0x000011ED
	public float hue
	{
		get
		{
			return this.masterHue;
		}
		set
		{
			this.masterHue = value;
		}
	}

	// Token: 0x17000003 RID: 3
	// (get) Token: 0x0600003C RID: 60 RVA: 0x00002FF6 File Offset: 0x000011F6
	// (set) Token: 0x0600003D RID: 61 RVA: 0x00002FFE File Offset: 0x000011FE
	public float saturation
	{
		get
		{
			return this.masterSaturation;
		}
		set
		{
			this.masterSaturation = value;
		}
	}

	// Token: 0x17000004 RID: 4
	// (get) Token: 0x0600003E RID: 62 RVA: 0x00003007 File Offset: 0x00001207
	// (set) Token: 0x0600003F RID: 63 RVA: 0x0000300F File Offset: 0x0000120F
	public float value
	{
		get
		{
			return this.masterValue;
		}
		set
		{
			this.masterValue = value;
		}
	}

	// Token: 0x06000040 RID: 64 RVA: 0x00003018 File Offset: 0x00001218
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetVector("_Master", new Vector3(this.masterHue / 360f, this.masterSaturation * 0.01f, this.masterValue * 0.01f));
		if (this.advanced)
		{
			base.material.SetVector("_Reds", new Vector3(this.redsHue / 360f, this.redsSaturation * 0.01f, this.redsValue * 0.01f));
			base.material.SetVector("_Yellows", new Vector3(this.yellowsHue / 360f, this.yellowsSaturation * 0.01f, this.yellowsValue * 0.01f));
			base.material.SetVector("_Greens", new Vector3(this.greensHue / 360f, this.greensSaturation * 0.01f, this.greensValue * 0.01f));
			base.material.SetVector("_Cyans", new Vector3(this.cyansHue / 360f, this.cyansSaturation * 0.01f, this.cyansValue * 0.01f));
			base.material.SetVector("_Blues", new Vector3(this.bluesHue / 360f, this.bluesSaturation * 0.01f, this.bluesValue * 0.01f));
			base.material.SetVector("_Magentas", new Vector3(this.magentasHue / 360f, this.magentasSaturation * 0.01f, this.magentasValue * 0.01f));
			Graphics.Blit(source, destination, base.material, 1);
			return;
		}
		Graphics.Blit(source, destination, base.material, 0);
	}

	// Token: 0x0400009A RID: 154
	[Range(-180f, 180f)]
	public float masterHue;

	// Token: 0x0400009B RID: 155
	[Range(-100f, 100f)]
	public float masterSaturation;

	// Token: 0x0400009C RID: 156
	[Range(-100f, 100f)]
	public float masterValue;

	// Token: 0x0400009D RID: 157
	[Range(-180f, 180f)]
	public float redsHue;

	// Token: 0x0400009E RID: 158
	[Range(-100f, 100f)]
	public float redsSaturation;

	// Token: 0x0400009F RID: 159
	[Range(-100f, 100f)]
	public float redsValue;

	// Token: 0x040000A0 RID: 160
	[Range(-180f, 180f)]
	public float yellowsHue;

	// Token: 0x040000A1 RID: 161
	[Range(-100f, 100f)]
	public float yellowsSaturation;

	// Token: 0x040000A2 RID: 162
	[Range(-100f, 100f)]
	public float yellowsValue;

	// Token: 0x040000A3 RID: 163
	[Range(-180f, 180f)]
	public float greensHue;

	// Token: 0x040000A4 RID: 164
	[Range(-100f, 100f)]
	public float greensSaturation;

	// Token: 0x040000A5 RID: 165
	[Range(-100f, 100f)]
	public float greensValue;

	// Token: 0x040000A6 RID: 166
	[Range(-180f, 180f)]
	public float cyansHue;

	// Token: 0x040000A7 RID: 167
	[Range(-100f, 100f)]
	public float cyansSaturation;

	// Token: 0x040000A8 RID: 168
	[Range(-100f, 100f)]
	public float cyansValue;

	// Token: 0x040000A9 RID: 169
	[Range(-180f, 180f)]
	public float bluesHue;

	// Token: 0x040000AA RID: 170
	[Range(-100f, 100f)]
	public float bluesSaturation;

	// Token: 0x040000AB RID: 171
	[Range(-100f, 100f)]
	public float bluesValue;

	// Token: 0x040000AC RID: 172
	[Range(-180f, 180f)]
	public float magentasHue;

	// Token: 0x040000AD RID: 173
	[Range(-100f, 100f)]
	public float magentasSaturation;

	// Token: 0x040000AE RID: 174
	[Range(-100f, 100f)]
	public float magentasValue;

	// Token: 0x040000AF RID: 175
	public bool advanced;

	// Token: 0x040000B0 RID: 176
	public int currentChannel;
}
