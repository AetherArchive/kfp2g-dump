using System;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
public class RenderSettingParam : MonoBehaviour
{
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

	public AmbientMode ambientMode;

	public Color ambientLight;

	public Color ambientEquatorColor;

	public Color ambientGroundColor;

	public Color ambientSkyColor;

	public float ambientIntensity;

	public float flareFadeSpeed;

	public float flareStrength;

	public bool fog;

	public Color fogColor;

	public float fogDensity;

	public float fogEndDistance;

	public FogMode fogMode;

	public float fogStartDistance;

	public float haloStrength;
}
