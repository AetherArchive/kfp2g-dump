using System;
using System.Collections.Generic;
using UnityEngine;

public class AuthPostEffectParam : ScriptableObject
{
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

	public AuthPostEffectParam.TechnicolorParam prvTechnicolor(int frm)
	{
		return this.Technicolor.FindLast((AuthPostEffectParam.TechnicolorParam itm) => Mathf.Abs(itm.frame) <= frm);
	}

	public AuthPostEffectParam.TechnicolorParam nxtTechnicolor(int frm)
	{
		return this.Technicolor.Find((AuthPostEffectParam.TechnicolorParam itm) => Mathf.Abs(itm.frame) >= frm);
	}

	public AuthPostEffectParam.HueSaturationParam prvHueSaturation(int frm)
	{
		return this.HueSaturation.FindLast((AuthPostEffectParam.HueSaturationParam itm) => Mathf.Abs(itm.frame) <= frm);
	}

	public AuthPostEffectParam.HueSaturationParam nxtHueSaturation(int frm)
	{
		return this.HueSaturation.Find((AuthPostEffectParam.HueSaturationParam itm) => Mathf.Abs(itm.frame) >= frm);
	}

	public AuthPostEffectParam.RadialBlurParam prvRadialBlur(int frm)
	{
		return this.RadialBlur.FindLast((AuthPostEffectParam.RadialBlurParam itm) => Mathf.Abs(itm.frame) <= frm);
	}

	public AuthPostEffectParam.RadialBlurParam nxtRadialBlur(int frm)
	{
		return this.RadialBlur.Find((AuthPostEffectParam.RadialBlurParam itm) => Mathf.Abs(itm.frame) >= frm);
	}

	public AuthPostEffectParam.WiggleParam prvWiggle(int frm)
	{
		return this.Wiggle.FindLast((AuthPostEffectParam.WiggleParam itm) => Mathf.Abs(itm.frame) <= frm);
	}

	public AuthPostEffectParam.WiggleParam nxtWiggle(int frm)
	{
		return this.Wiggle.Find((AuthPostEffectParam.WiggleParam itm) => Mathf.Abs(itm.frame) >= frm);
	}

	public AuthPostEffectParam.RGBSplitParam prvRGBSplit(int frm)
	{
		return this.RGBSplit.FindLast((AuthPostEffectParam.RGBSplitParam itm) => Mathf.Abs(itm.frame) <= frm);
	}

	public AuthPostEffectParam.RGBSplitParam nxtRGBSplit(int frm)
	{
		return this.RGBSplit.Find((AuthPostEffectParam.RGBSplitParam itm) => Mathf.Abs(itm.frame) >= frm);
	}

	public AuthPostEffectParam.ContrastVignetteParam prvContrastVignette(int frm)
	{
		return this.ContrastVignette.FindLast((AuthPostEffectParam.ContrastVignetteParam itm) => Mathf.Abs(itm.frame) <= frm);
	}

	public AuthPostEffectParam.ContrastVignetteParam nxtContrastVignette(int frm)
	{
		return this.ContrastVignette.Find((AuthPostEffectParam.ContrastVignetteParam itm) => Mathf.Abs(itm.frame) >= frm);
	}

	public AuthPostEffectParam.DoubleVisionParam prvDoubleVision(int frm)
	{
		return this.DoubleVision.FindLast((AuthPostEffectParam.DoubleVisionParam itm) => Mathf.Abs(itm.frame) <= frm);
	}

	public AuthPostEffectParam.DoubleVisionParam nxtDoubleVision(int frm)
	{
		return this.DoubleVision.Find((AuthPostEffectParam.DoubleVisionParam itm) => Mathf.Abs(itm.frame) >= frm);
	}

	public AuthPostEffectParam.NegativeParam prvNegative(int frm)
	{
		return this.Negative.FindLast((AuthPostEffectParam.NegativeParam itm) => Mathf.Abs(itm.frame) <= frm);
	}

	public AuthPostEffectParam.NegativeParam nxtNegative(int frm)
	{
		return this.Negative.Find((AuthPostEffectParam.NegativeParam itm) => Mathf.Abs(itm.frame) >= frm);
	}

	public AuthPostEffectParam.AnalogTVParam prvAnalogTV(int frm)
	{
		return this.AnalogTV.FindLast((AuthPostEffectParam.AnalogTVParam itm) => Mathf.Abs(itm.frame) <= frm);
	}

	public AuthPostEffectParam.AnalogTVParam nxtAnalogTV(int frm)
	{
		return this.AnalogTV.Find((AuthPostEffectParam.AnalogTVParam itm) => Mathf.Abs(itm.frame) >= frm);
	}

	public AuthPostEffectParam.FocusLineParam prvFocusLine(int frm)
	{
		return this.FocusLine.FindLast((AuthPostEffectParam.FocusLineParam itm) => Mathf.Abs(itm.frame) <= frm);
	}

	public AuthPostEffectParam.FocusLineParam nxtFocusLine(int frm)
	{
		return this.FocusLine.Find((AuthPostEffectParam.FocusLineParam itm) => Mathf.Abs(itm.frame) >= frm);
	}

	public AuthPostEffectParam.SpeedLineParam prvSpeedLine(int frm)
	{
		return this.SpeedLine.FindLast((AuthPostEffectParam.SpeedLineParam itm) => Mathf.Abs(itm.frame) <= frm);
	}

	public AuthPostEffectParam.SpeedLineParam nxtSpeedLine(int frm)
	{
		return this.SpeedLine.Find((AuthPostEffectParam.SpeedLineParam itm) => Mathf.Abs(itm.frame) >= frm);
	}

	public AuthPostEffectParam.BlurParam prvBlur(int frm)
	{
		return this.Blur.FindLast((AuthPostEffectParam.BlurParam itm) => Mathf.Abs(itm.frame) <= frm);
	}

	public AuthPostEffectParam.BlurParam nxtBlur(int frm)
	{
		return this.Blur.Find((AuthPostEffectParam.BlurParam itm) => Mathf.Abs(itm.frame) >= frm);
	}

	public AuthPostEffectParam.FogParam prvFog(int frm)
	{
		return this.Fog.FindLast((AuthPostEffectParam.FogParam itm) => Mathf.Abs(itm.frame) <= frm);
	}

	public AuthPostEffectParam.FogParam nxtFog(int frm)
	{
		return this.Fog.Find((AuthPostEffectParam.FogParam itm) => Mathf.Abs(itm.frame) >= frm);
	}

	public AuthPostEffectParam.CamouflageParam prvCamouflage(int frm)
	{
		return this.Camouflage.FindLast((AuthPostEffectParam.CamouflageParam itm) => Mathf.Abs(itm.frame) <= frm);
	}

	public AuthPostEffectParam.CamouflageParam nxtCamouflage(int frm)
	{
		return this.Camouflage.Find((AuthPostEffectParam.CamouflageParam itm) => Mathf.Abs(itm.frame) >= frm);
	}

	public List<AuthPostEffectParam.TechnicolorParam> Technicolor;

	public List<AuthPostEffectParam.HueSaturationParam> HueSaturation;

	public List<AuthPostEffectParam.RadialBlurParam> RadialBlur;

	public List<AuthPostEffectParam.WiggleParam> Wiggle;

	public List<AuthPostEffectParam.RGBSplitParam> RGBSplit;

	public List<AuthPostEffectParam.ContrastVignetteParam> ContrastVignette;

	public List<AuthPostEffectParam.DoubleVisionParam> DoubleVision;

	public List<AuthPostEffectParam.NegativeParam> Negative;

	public List<AuthPostEffectParam.AnalogTVParam> AnalogTV;

	public List<AuthPostEffectParam.FocusLineParam> FocusLine;

	public List<AuthPostEffectParam.SpeedLineParam> SpeedLine;

	public List<AuthPostEffectParam.BlurParam> Blur;

	public List<AuthPostEffectParam.FogParam> Fog;

	public List<AuthPostEffectParam.CamouflageParam> Camouflage;

	[Serializable]
	public class TechnicolorParam
	{
		public int frame;

		public float exposure;

		public Vector3 balance;

		public float amount;
	}

	[Serializable]
	public class HueSaturationParam
	{
		public int frame;

		public float hue;

		public float saturation;

		public float value;
	}

	[Serializable]
	public class RadialBlurParam
	{
		public int frame;

		public float amount;

		public Vector2 center;
	}

	[Serializable]
	public class WiggleParam
	{
		public int frame;

		public float speed;

		public float scale;
	}

	[Serializable]
	public class RGBSplitParam
	{
		public int frame;

		public float amount;

		public float angle;
	}

	[Serializable]
	public class ContrastVignetteParam
	{
		public int frame;

		public Vector2 center;

		public float sharpness;

		public float darkness;

		public float contrast;

		public float redCoeff;

		public float greenCoeff;

		public float blueCoeff;

		public float edge;

		public float redAmbient;

		public float greenAmbient;

		public float blueAmbient;
	}

	[Serializable]
	public class DoubleVisionParam
	{
		public int frame;

		public Vector2 displace;

		public float amount;
	}

	[Serializable]
	public class NegativeParam
	{
		public int frame;

		public float amount;
	}

	[Serializable]
	public class AnalogTVParam
	{
		public int frame;

		public float noiseIntensity;

		public float scanlinesIntensity;
	}

	[Serializable]
	public class FocusLineParam
	{
		public int frame;

		public Vector2 focusPos;

		public Color lineColor;
	}

	[Serializable]
	public class SpeedLineParam
	{
		public int frame;

		public float lineAngle;

		public float lineSpeed;

		public Color lineColor;
	}

	[Serializable]
	public class BlurParam
	{
		public int frame;

		public int iterations;

		public float blurSpread;

		public float amount;
	}

	[Serializable]
	public class FogParam
	{
		public int frame;

		public Color color;

		public float start;

		public float end;
	}

	[Serializable]
	public class CamouflageParam
	{
		public int frame;
	}
}
