using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/Glitch")]
public class CC_Glitch : CC_Base
{
	private void OnEnable()
	{
		this.m_Camera = base.GetComponent<Camera>();
	}

	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.mode == CC_Glitch.Mode.Interferences)
		{
			this.DoInterferences(source, destination, this.interferencesSettings);
			return;
		}
		if (this.mode == CC_Glitch.Mode.Tearing)
		{
			this.DoTearing(source, destination, this.tearingSettings);
			return;
		}
		RenderTexture temporary = RenderTexture.GetTemporary(this.m_Camera.pixelWidth, this.m_Camera.pixelHeight, 0, RenderTextureFormat.ARGB32);
		this.DoTearing(source, temporary, this.tearingSettings);
		this.DoInterferences(temporary, destination, this.interferencesSettings);
		temporary.Release();
	}

	private void DoInterferences(RenderTexture source, RenderTexture destination, CC_Glitch.InterferenceSettings settings)
	{
		base.material.SetVector("_Params", new Vector3(settings.speed, settings.density, settings.maxDisplacement));
		Graphics.Blit(source, destination, base.material, 0);
	}

	private void DoTearing(RenderTexture source, RenderTexture destination, CC_Glitch.TearingSettings settings)
	{
		base.material.SetVector("_Params", new Vector4(settings.speed, settings.intensity, settings.maxDisplacement, settings.yuvOffset));
		int num = 1;
		if (settings.allowFlipping && settings.yuvColorBleeding)
		{
			num = 4;
		}
		else if (settings.allowFlipping)
		{
			num = 2;
		}
		else if (settings.yuvColorBleeding)
		{
			num = 3;
		}
		Graphics.Blit(source, destination, base.material, num);
	}

	public CC_Glitch.Mode mode;

	public CC_Glitch.InterferenceSettings interferencesSettings;

	public CC_Glitch.TearingSettings tearingSettings;

	protected Camera m_Camera;

	public enum Mode
	{
		Interferences,
		Tearing,
		Complete
	}

	[Serializable]
	public class InterferenceSettings
	{
		public float speed = 10f;

		public float density = 8f;

		public float maxDisplacement = 2f;
	}

	[Serializable]
	public class TearingSettings
	{
		public float speed = 1f;

		[Range(0f, 1f)]
		public float intensity = 0.25f;

		[Range(0f, 0.5f)]
		public float maxDisplacement = 0.05f;

		public bool allowFlipping;

		public bool yuvColorBleeding = true;

		[Range(-2f, 2f)]
		public float yuvOffset = 0.5f;
	}
}
