using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000045 RID: 69
[Serializable]
public class RenderSettingParam : MonoBehaviour
{
	// Token: 0x06000164 RID: 356 RVA: 0x0000B4F4 File Offset: 0x000096F4
	public void Param2Scene()
	{
		RenderSettings.ambientMode = this.ambientMode;
		RenderSettings.ambientLight = this.ambientLight;
		RenderSettings.ambientEquatorColor = this.ambientEquatorColor;
		RenderSettings.ambientGroundColor = this.ambientGroundColor;
		RenderSettings.ambientSkyColor = this.ambientSkyColor;
		RenderSettings.ambientIntensity = this.ambientIntensity;
		RenderSettings.flareFadeSpeed = this.flareFadeSpeed;
		RenderSettings.flareStrength = this.flareStrength;
		RenderSettings.fog = this.fog;
		RenderSettings.fogColor = this.fogColor;
		RenderSettings.fogDensity = this.fogDensity;
		RenderSettings.fogEndDistance = this.fogEndDistance;
		RenderSettings.fogMode = this.fogMode;
		RenderSettings.fogStartDistance = this.fogStartDistance;
		RenderSettings.haloStrength = this.haloStrength;
	}

	// Token: 0x06000165 RID: 357 RVA: 0x0000B5A8 File Offset: 0x000097A8
	public void Scene2Param()
	{
		this.ambientMode = RenderSettings.ambientMode;
		this.ambientLight = RenderSettings.ambientLight;
		this.ambientGroundColor = RenderSettings.ambientGroundColor;
		this.ambientEquatorColor = RenderSettings.ambientEquatorColor;
		this.ambientSkyColor = RenderSettings.ambientSkyColor;
		this.ambientIntensity = RenderSettings.ambientIntensity;
		this.flareFadeSpeed = RenderSettings.flareFadeSpeed;
		this.flareStrength = RenderSettings.flareStrength;
		this.fog = RenderSettings.fog;
		this.fogColor = RenderSettings.fogColor;
		this.fogDensity = RenderSettings.fogDensity;
		this.fogEndDistance = RenderSettings.fogEndDistance;
		this.fogMode = RenderSettings.fogMode;
		this.fogStartDistance = RenderSettings.fogStartDistance;
		this.haloStrength = RenderSettings.haloStrength;
	}

	// Token: 0x040001C1 RID: 449
	public AmbientMode ambientMode;

	// Token: 0x040001C2 RID: 450
	public Color ambientLight;

	// Token: 0x040001C3 RID: 451
	public Color ambientEquatorColor;

	// Token: 0x040001C4 RID: 452
	public Color ambientGroundColor;

	// Token: 0x040001C5 RID: 453
	public Color ambientSkyColor;

	// Token: 0x040001C6 RID: 454
	public float ambientIntensity;

	// Token: 0x040001C7 RID: 455
	public float flareFadeSpeed;

	// Token: 0x040001C8 RID: 456
	public float flareStrength;

	// Token: 0x040001C9 RID: 457
	public bool fog;

	// Token: 0x040001CA RID: 458
	public Color fogColor;

	// Token: 0x040001CB RID: 459
	public float fogDensity;

	// Token: 0x040001CC RID: 460
	public float fogEndDistance;

	// Token: 0x040001CD RID: 461
	public FogMode fogMode;

	// Token: 0x040001CE RID: 462
	public float fogStartDistance;

	// Token: 0x040001CF RID: 463
	public float haloStrength;
}
