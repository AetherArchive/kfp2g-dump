using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/Hue, Saturation, Value")]
public class CC_HueSaturationValue : CC_Base
{
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

	[Range(-180f, 180f)]
	public float masterHue;

	[Range(-100f, 100f)]
	public float masterSaturation;

	[Range(-100f, 100f)]
	public float masterValue;

	[Range(-180f, 180f)]
	public float redsHue;

	[Range(-100f, 100f)]
	public float redsSaturation;

	[Range(-100f, 100f)]
	public float redsValue;

	[Range(-180f, 180f)]
	public float yellowsHue;

	[Range(-100f, 100f)]
	public float yellowsSaturation;

	[Range(-100f, 100f)]
	public float yellowsValue;

	[Range(-180f, 180f)]
	public float greensHue;

	[Range(-100f, 100f)]
	public float greensSaturation;

	[Range(-100f, 100f)]
	public float greensValue;

	[Range(-180f, 180f)]
	public float cyansHue;

	[Range(-100f, 100f)]
	public float cyansSaturation;

	[Range(-100f, 100f)]
	public float cyansValue;

	[Range(-180f, 180f)]
	public float bluesHue;

	[Range(-100f, 100f)]
	public float bluesSaturation;

	[Range(-100f, 100f)]
	public float bluesValue;

	[Range(-180f, 180f)]
	public float magentasHue;

	[Range(-100f, 100f)]
	public float magentasSaturation;

	[Range(-100f, 100f)]
	public float magentasValue;

	public bool advanced;

	public int currentChannel;
}
