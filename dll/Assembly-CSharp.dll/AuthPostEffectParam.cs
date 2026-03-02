using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000041 RID: 65
public class AuthPostEffectParam : ScriptableObject
{
	// Token: 0x06000136 RID: 310 RVA: 0x0000A720 File Offset: 0x00008920
	public AuthPostEffectParam()
	{
		this.Technicolor = new List<AuthPostEffectParam.TechnicolorParam>();
		this.HueSaturation = new List<AuthPostEffectParam.HueSaturationParam>();
		this.RadialBlur = new List<AuthPostEffectParam.RadialBlurParam>();
		this.Wiggle = new List<AuthPostEffectParam.WiggleParam>();
		this.RGBSplit = new List<AuthPostEffectParam.RGBSplitParam>();
		this.ContrastVignette = new List<AuthPostEffectParam.ContrastVignetteParam>();
		this.DoubleVision = new List<AuthPostEffectParam.DoubleVisionParam>();
		this.Negative = new List<AuthPostEffectParam.NegativeParam>();
		this.AnalogTV = new List<AuthPostEffectParam.AnalogTVParam>();
		this.FocusLine = new List<AuthPostEffectParam.FocusLineParam>();
		this.SpeedLine = new List<AuthPostEffectParam.SpeedLineParam>();
		this.Blur = new List<AuthPostEffectParam.BlurParam>();
		this.Fog = new List<AuthPostEffectParam.FogParam>();
		this.Camouflage = new List<AuthPostEffectParam.CamouflageParam>();
	}

	// Token: 0x06000137 RID: 311 RVA: 0x0000A7D0 File Offset: 0x000089D0
	public AuthPostEffectParam(AuthPostEffectParam src)
	{
		this.Technicolor = new List<AuthPostEffectParam.TechnicolorParam>(src.Technicolor);
		this.HueSaturation = new List<AuthPostEffectParam.HueSaturationParam>(src.HueSaturation);
		this.RadialBlur = new List<AuthPostEffectParam.RadialBlurParam>(src.RadialBlur);
		this.Wiggle = new List<AuthPostEffectParam.WiggleParam>(src.Wiggle);
		this.RGBSplit = new List<AuthPostEffectParam.RGBSplitParam>(src.RGBSplit);
		this.ContrastVignette = new List<AuthPostEffectParam.ContrastVignetteParam>(src.ContrastVignette);
		this.DoubleVision = new List<AuthPostEffectParam.DoubleVisionParam>(src.DoubleVision);
		this.Negative = new List<AuthPostEffectParam.NegativeParam>(src.Negative);
		this.AnalogTV = new List<AuthPostEffectParam.AnalogTVParam>(src.AnalogTV);
		this.FocusLine = new List<AuthPostEffectParam.FocusLineParam>(src.FocusLine);
		this.SpeedLine = new List<AuthPostEffectParam.SpeedLineParam>(src.SpeedLine);
		this.Blur = new List<AuthPostEffectParam.BlurParam>(src.Blur);
		this.Fog = new List<AuthPostEffectParam.FogParam>(src.Fog);
		this.Camouflage = new List<AuthPostEffectParam.CamouflageParam>(src.Camouflage);
	}

	// Token: 0x06000138 RID: 312 RVA: 0x0000A8D4 File Offset: 0x00008AD4
	public AuthPostEffectParam.TechnicolorParam prvTechnicolor(int frm)
	{
		return this.Technicolor.FindLast((AuthPostEffectParam.TechnicolorParam itm) => Mathf.Abs(itm.frame) <= frm);
	}

	// Token: 0x06000139 RID: 313 RVA: 0x0000A908 File Offset: 0x00008B08
	public AuthPostEffectParam.TechnicolorParam nxtTechnicolor(int frm)
	{
		return this.Technicolor.Find((AuthPostEffectParam.TechnicolorParam itm) => Mathf.Abs(itm.frame) >= frm);
	}

	// Token: 0x0600013A RID: 314 RVA: 0x0000A93C File Offset: 0x00008B3C
	public AuthPostEffectParam.HueSaturationParam prvHueSaturation(int frm)
	{
		return this.HueSaturation.FindLast((AuthPostEffectParam.HueSaturationParam itm) => Mathf.Abs(itm.frame) <= frm);
	}

	// Token: 0x0600013B RID: 315 RVA: 0x0000A970 File Offset: 0x00008B70
	public AuthPostEffectParam.HueSaturationParam nxtHueSaturation(int frm)
	{
		return this.HueSaturation.Find((AuthPostEffectParam.HueSaturationParam itm) => Mathf.Abs(itm.frame) >= frm);
	}

	// Token: 0x0600013C RID: 316 RVA: 0x0000A9A4 File Offset: 0x00008BA4
	public AuthPostEffectParam.RadialBlurParam prvRadialBlur(int frm)
	{
		return this.RadialBlur.FindLast((AuthPostEffectParam.RadialBlurParam itm) => Mathf.Abs(itm.frame) <= frm);
	}

	// Token: 0x0600013D RID: 317 RVA: 0x0000A9D8 File Offset: 0x00008BD8
	public AuthPostEffectParam.RadialBlurParam nxtRadialBlur(int frm)
	{
		return this.RadialBlur.Find((AuthPostEffectParam.RadialBlurParam itm) => Mathf.Abs(itm.frame) >= frm);
	}

	// Token: 0x0600013E RID: 318 RVA: 0x0000AA0C File Offset: 0x00008C0C
	public AuthPostEffectParam.WiggleParam prvWiggle(int frm)
	{
		return this.Wiggle.FindLast((AuthPostEffectParam.WiggleParam itm) => Mathf.Abs(itm.frame) <= frm);
	}

	// Token: 0x0600013F RID: 319 RVA: 0x0000AA40 File Offset: 0x00008C40
	public AuthPostEffectParam.WiggleParam nxtWiggle(int frm)
	{
		return this.Wiggle.Find((AuthPostEffectParam.WiggleParam itm) => Mathf.Abs(itm.frame) >= frm);
	}

	// Token: 0x06000140 RID: 320 RVA: 0x0000AA74 File Offset: 0x00008C74
	public AuthPostEffectParam.RGBSplitParam prvRGBSplit(int frm)
	{
		return this.RGBSplit.FindLast((AuthPostEffectParam.RGBSplitParam itm) => Mathf.Abs(itm.frame) <= frm);
	}

	// Token: 0x06000141 RID: 321 RVA: 0x0000AAA8 File Offset: 0x00008CA8
	public AuthPostEffectParam.RGBSplitParam nxtRGBSplit(int frm)
	{
		return this.RGBSplit.Find((AuthPostEffectParam.RGBSplitParam itm) => Mathf.Abs(itm.frame) >= frm);
	}

	// Token: 0x06000142 RID: 322 RVA: 0x0000AADC File Offset: 0x00008CDC
	public AuthPostEffectParam.ContrastVignetteParam prvContrastVignette(int frm)
	{
		return this.ContrastVignette.FindLast((AuthPostEffectParam.ContrastVignetteParam itm) => Mathf.Abs(itm.frame) <= frm);
	}

	// Token: 0x06000143 RID: 323 RVA: 0x0000AB10 File Offset: 0x00008D10
	public AuthPostEffectParam.ContrastVignetteParam nxtContrastVignette(int frm)
	{
		return this.ContrastVignette.Find((AuthPostEffectParam.ContrastVignetteParam itm) => Mathf.Abs(itm.frame) >= frm);
	}

	// Token: 0x06000144 RID: 324 RVA: 0x0000AB44 File Offset: 0x00008D44
	public AuthPostEffectParam.DoubleVisionParam prvDoubleVision(int frm)
	{
		return this.DoubleVision.FindLast((AuthPostEffectParam.DoubleVisionParam itm) => Mathf.Abs(itm.frame) <= frm);
	}

	// Token: 0x06000145 RID: 325 RVA: 0x0000AB78 File Offset: 0x00008D78
	public AuthPostEffectParam.DoubleVisionParam nxtDoubleVision(int frm)
	{
		return this.DoubleVision.Find((AuthPostEffectParam.DoubleVisionParam itm) => Mathf.Abs(itm.frame) >= frm);
	}

	// Token: 0x06000146 RID: 326 RVA: 0x0000ABAC File Offset: 0x00008DAC
	public AuthPostEffectParam.NegativeParam prvNegative(int frm)
	{
		return this.Negative.FindLast((AuthPostEffectParam.NegativeParam itm) => Mathf.Abs(itm.frame) <= frm);
	}

	// Token: 0x06000147 RID: 327 RVA: 0x0000ABE0 File Offset: 0x00008DE0
	public AuthPostEffectParam.NegativeParam nxtNegative(int frm)
	{
		return this.Negative.Find((AuthPostEffectParam.NegativeParam itm) => Mathf.Abs(itm.frame) >= frm);
	}

	// Token: 0x06000148 RID: 328 RVA: 0x0000AC14 File Offset: 0x00008E14
	public AuthPostEffectParam.AnalogTVParam prvAnalogTV(int frm)
	{
		return this.AnalogTV.FindLast((AuthPostEffectParam.AnalogTVParam itm) => Mathf.Abs(itm.frame) <= frm);
	}

	// Token: 0x06000149 RID: 329 RVA: 0x0000AC48 File Offset: 0x00008E48
	public AuthPostEffectParam.AnalogTVParam nxtAnalogTV(int frm)
	{
		return this.AnalogTV.Find((AuthPostEffectParam.AnalogTVParam itm) => Mathf.Abs(itm.frame) >= frm);
	}

	// Token: 0x0600014A RID: 330 RVA: 0x0000AC7C File Offset: 0x00008E7C
	public AuthPostEffectParam.FocusLineParam prvFocusLine(int frm)
	{
		return this.FocusLine.FindLast((AuthPostEffectParam.FocusLineParam itm) => Mathf.Abs(itm.frame) <= frm);
	}

	// Token: 0x0600014B RID: 331 RVA: 0x0000ACB0 File Offset: 0x00008EB0
	public AuthPostEffectParam.FocusLineParam nxtFocusLine(int frm)
	{
		return this.FocusLine.Find((AuthPostEffectParam.FocusLineParam itm) => Mathf.Abs(itm.frame) >= frm);
	}

	// Token: 0x0600014C RID: 332 RVA: 0x0000ACE4 File Offset: 0x00008EE4
	public AuthPostEffectParam.SpeedLineParam prvSpeedLine(int frm)
	{
		return this.SpeedLine.FindLast((AuthPostEffectParam.SpeedLineParam itm) => Mathf.Abs(itm.frame) <= frm);
	}

	// Token: 0x0600014D RID: 333 RVA: 0x0000AD18 File Offset: 0x00008F18
	public AuthPostEffectParam.SpeedLineParam nxtSpeedLine(int frm)
	{
		return this.SpeedLine.Find((AuthPostEffectParam.SpeedLineParam itm) => Mathf.Abs(itm.frame) >= frm);
	}

	// Token: 0x0600014E RID: 334 RVA: 0x0000AD4C File Offset: 0x00008F4C
	public AuthPostEffectParam.BlurParam prvBlur(int frm)
	{
		return this.Blur.FindLast((AuthPostEffectParam.BlurParam itm) => Mathf.Abs(itm.frame) <= frm);
	}

	// Token: 0x0600014F RID: 335 RVA: 0x0000AD80 File Offset: 0x00008F80
	public AuthPostEffectParam.BlurParam nxtBlur(int frm)
	{
		return this.Blur.Find((AuthPostEffectParam.BlurParam itm) => Mathf.Abs(itm.frame) >= frm);
	}

	// Token: 0x06000150 RID: 336 RVA: 0x0000ADB4 File Offset: 0x00008FB4
	public AuthPostEffectParam.FogParam prvFog(int frm)
	{
		return this.Fog.FindLast((AuthPostEffectParam.FogParam itm) => Mathf.Abs(itm.frame) <= frm);
	}

	// Token: 0x06000151 RID: 337 RVA: 0x0000ADE8 File Offset: 0x00008FE8
	public AuthPostEffectParam.FogParam nxtFog(int frm)
	{
		return this.Fog.Find((AuthPostEffectParam.FogParam itm) => Mathf.Abs(itm.frame) >= frm);
	}

	// Token: 0x06000152 RID: 338 RVA: 0x0000AE1C File Offset: 0x0000901C
	public AuthPostEffectParam.CamouflageParam prvCamouflage(int frm)
	{
		return this.Camouflage.FindLast((AuthPostEffectParam.CamouflageParam itm) => Mathf.Abs(itm.frame) <= frm);
	}

	// Token: 0x06000153 RID: 339 RVA: 0x0000AE50 File Offset: 0x00009050
	public AuthPostEffectParam.CamouflageParam nxtCamouflage(int frm)
	{
		return this.Camouflage.Find((AuthPostEffectParam.CamouflageParam itm) => Mathf.Abs(itm.frame) >= frm);
	}

	// Token: 0x040001A8 RID: 424
	public List<AuthPostEffectParam.TechnicolorParam> Technicolor;

	// Token: 0x040001A9 RID: 425
	public List<AuthPostEffectParam.HueSaturationParam> HueSaturation;

	// Token: 0x040001AA RID: 426
	public List<AuthPostEffectParam.RadialBlurParam> RadialBlur;

	// Token: 0x040001AB RID: 427
	public List<AuthPostEffectParam.WiggleParam> Wiggle;

	// Token: 0x040001AC RID: 428
	public List<AuthPostEffectParam.RGBSplitParam> RGBSplit;

	// Token: 0x040001AD RID: 429
	public List<AuthPostEffectParam.ContrastVignetteParam> ContrastVignette;

	// Token: 0x040001AE RID: 430
	public List<AuthPostEffectParam.DoubleVisionParam> DoubleVision;

	// Token: 0x040001AF RID: 431
	public List<AuthPostEffectParam.NegativeParam> Negative;

	// Token: 0x040001B0 RID: 432
	public List<AuthPostEffectParam.AnalogTVParam> AnalogTV;

	// Token: 0x040001B1 RID: 433
	public List<AuthPostEffectParam.FocusLineParam> FocusLine;

	// Token: 0x040001B2 RID: 434
	public List<AuthPostEffectParam.SpeedLineParam> SpeedLine;

	// Token: 0x040001B3 RID: 435
	public List<AuthPostEffectParam.BlurParam> Blur;

	// Token: 0x040001B4 RID: 436
	public List<AuthPostEffectParam.FogParam> Fog;

	// Token: 0x040001B5 RID: 437
	public List<AuthPostEffectParam.CamouflageParam> Camouflage;

	// Token: 0x020005AD RID: 1453
	[Serializable]
	public class TechnicolorParam
	{
		// Token: 0x040029DE RID: 10718
		public int frame;

		// Token: 0x040029DF RID: 10719
		public float exposure;

		// Token: 0x040029E0 RID: 10720
		public Vector3 balance;

		// Token: 0x040029E1 RID: 10721
		public float amount;
	}

	// Token: 0x020005AE RID: 1454
	[Serializable]
	public class HueSaturationParam
	{
		// Token: 0x040029E2 RID: 10722
		public int frame;

		// Token: 0x040029E3 RID: 10723
		public float hue;

		// Token: 0x040029E4 RID: 10724
		public float saturation;

		// Token: 0x040029E5 RID: 10725
		public float value;
	}

	// Token: 0x020005AF RID: 1455
	[Serializable]
	public class RadialBlurParam
	{
		// Token: 0x040029E6 RID: 10726
		public int frame;

		// Token: 0x040029E7 RID: 10727
		public float amount;

		// Token: 0x040029E8 RID: 10728
		public Vector2 center;
	}

	// Token: 0x020005B0 RID: 1456
	[Serializable]
	public class WiggleParam
	{
		// Token: 0x040029E9 RID: 10729
		public int frame;

		// Token: 0x040029EA RID: 10730
		public float speed;

		// Token: 0x040029EB RID: 10731
		public float scale;
	}

	// Token: 0x020005B1 RID: 1457
	[Serializable]
	public class RGBSplitParam
	{
		// Token: 0x040029EC RID: 10732
		public int frame;

		// Token: 0x040029ED RID: 10733
		public float amount;

		// Token: 0x040029EE RID: 10734
		public float angle;
	}

	// Token: 0x020005B2 RID: 1458
	[Serializable]
	public class ContrastVignetteParam
	{
		// Token: 0x040029EF RID: 10735
		public int frame;

		// Token: 0x040029F0 RID: 10736
		public Vector2 center;

		// Token: 0x040029F1 RID: 10737
		public float sharpness;

		// Token: 0x040029F2 RID: 10738
		public float darkness;

		// Token: 0x040029F3 RID: 10739
		public float contrast;

		// Token: 0x040029F4 RID: 10740
		public float redCoeff;

		// Token: 0x040029F5 RID: 10741
		public float greenCoeff;

		// Token: 0x040029F6 RID: 10742
		public float blueCoeff;

		// Token: 0x040029F7 RID: 10743
		public float edge;

		// Token: 0x040029F8 RID: 10744
		public float redAmbient;

		// Token: 0x040029F9 RID: 10745
		public float greenAmbient;

		// Token: 0x040029FA RID: 10746
		public float blueAmbient;
	}

	// Token: 0x020005B3 RID: 1459
	[Serializable]
	public class DoubleVisionParam
	{
		// Token: 0x040029FB RID: 10747
		public int frame;

		// Token: 0x040029FC RID: 10748
		public Vector2 displace;

		// Token: 0x040029FD RID: 10749
		public float amount;
	}

	// Token: 0x020005B4 RID: 1460
	[Serializable]
	public class NegativeParam
	{
		// Token: 0x040029FE RID: 10750
		public int frame;

		// Token: 0x040029FF RID: 10751
		public float amount;
	}

	// Token: 0x020005B5 RID: 1461
	[Serializable]
	public class AnalogTVParam
	{
		// Token: 0x04002A00 RID: 10752
		public int frame;

		// Token: 0x04002A01 RID: 10753
		public float noiseIntensity;

		// Token: 0x04002A02 RID: 10754
		public float scanlinesIntensity;
	}

	// Token: 0x020005B6 RID: 1462
	[Serializable]
	public class FocusLineParam
	{
		// Token: 0x04002A03 RID: 10755
		public int frame;

		// Token: 0x04002A04 RID: 10756
		public Vector2 focusPos;

		// Token: 0x04002A05 RID: 10757
		public Color lineColor;
	}

	// Token: 0x020005B7 RID: 1463
	[Serializable]
	public class SpeedLineParam
	{
		// Token: 0x04002A06 RID: 10758
		public int frame;

		// Token: 0x04002A07 RID: 10759
		public float lineAngle;

		// Token: 0x04002A08 RID: 10760
		public float lineSpeed;

		// Token: 0x04002A09 RID: 10761
		public Color lineColor;
	}

	// Token: 0x020005B8 RID: 1464
	[Serializable]
	public class BlurParam
	{
		// Token: 0x04002A0A RID: 10762
		public int frame;

		// Token: 0x04002A0B RID: 10763
		public int iterations;

		// Token: 0x04002A0C RID: 10764
		public float blurSpread;

		// Token: 0x04002A0D RID: 10765
		public float amount;
	}

	// Token: 0x020005B9 RID: 1465
	[Serializable]
	public class FogParam
	{
		// Token: 0x04002A0E RID: 10766
		public int frame;

		// Token: 0x04002A0F RID: 10767
		public Color color;

		// Token: 0x04002A10 RID: 10768
		public float start;

		// Token: 0x04002A11 RID: 10769
		public float end;
	}

	// Token: 0x020005BA RID: 1466
	[Serializable]
	public class CamouflageParam
	{
		// Token: 0x04002A12 RID: 10770
		public int frame;
	}
}
