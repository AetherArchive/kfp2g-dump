using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/Wiggle")]
public class CC_Wiggle : CC_Base
{
	private void Update()
	{
		if (this.autoTimer)
		{
			this.timer += this.speed * Time.deltaTime;
		}
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetFloat("_Timer", this.timer);
		base.material.SetFloat("_Scale", this.scale);
		Graphics.Blit(source, destination, base.material);
	}

	public float timer;

	public float speed = 1f;

	public float scale = 12f;

	public bool autoTimer = true;
}
