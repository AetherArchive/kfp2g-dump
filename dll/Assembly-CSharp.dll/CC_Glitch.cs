using System;
using UnityEngine;

// Token: 0x02000013 RID: 19
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Glitch")]
public class CC_Glitch : CC_Base
{
	// Token: 0x0600002E RID: 46 RVA: 0x00002CC9 File Offset: 0x00000EC9
	private void OnEnable()
	{
		this.m_Camera = base.GetComponent<Camera>();
	}

	// Token: 0x0600002F RID: 47 RVA: 0x00002CD8 File Offset: 0x00000ED8
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

	// Token: 0x06000030 RID: 48 RVA: 0x00002D54 File Offset: 0x00000F54
	private void DoInterferences(RenderTexture source, RenderTexture destination, CC_Glitch.InterferenceSettings settings)
	{
		base.material.SetVector("_Params", new Vector3(settings.speed, settings.density, settings.maxDisplacement));
		Graphics.Blit(source, destination, base.material, 0);
	}

	// Token: 0x06000031 RID: 49 RVA: 0x00002D90 File Offset: 0x00000F90
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

	// Token: 0x0400008B RID: 139
	public CC_Glitch.Mode mode;

	// Token: 0x0400008C RID: 140
	public CC_Glitch.InterferenceSettings interferencesSettings;

	// Token: 0x0400008D RID: 141
	public CC_Glitch.TearingSettings tearingSettings;

	// Token: 0x0400008E RID: 142
	protected Camera m_Camera;

	// Token: 0x0200058B RID: 1419
	public enum Mode
	{
		// Token: 0x04002919 RID: 10521
		Interferences,
		// Token: 0x0400291A RID: 10522
		Tearing,
		// Token: 0x0400291B RID: 10523
		Complete
	}

	// Token: 0x0200058C RID: 1420
	[Serializable]
	public class InterferenceSettings
	{
		// Token: 0x0400291C RID: 10524
		public float speed = 10f;

		// Token: 0x0400291D RID: 10525
		public float density = 8f;

		// Token: 0x0400291E RID: 10526
		public float maxDisplacement = 2f;
	}

	// Token: 0x0200058D RID: 1421
	[Serializable]
	public class TearingSettings
	{
		// Token: 0x0400291F RID: 10527
		public float speed = 1f;

		// Token: 0x04002920 RID: 10528
		[Range(0f, 1f)]
		public float intensity = 0.25f;

		// Token: 0x04002921 RID: 10529
		[Range(0f, 0.5f)]
		public float maxDisplacement = 0.05f;

		// Token: 0x04002922 RID: 10530
		public bool allowFlipping;

		// Token: 0x04002923 RID: 10531
		public bool yuvColorBleeding = true;

		// Token: 0x04002924 RID: 10532
		[Range(-2f, 2f)]
		public float yuvOffset = 0.5f;
	}
}
