using System;
using UnityEngine;

public class ImageCapture : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	public void capture(Rect field, int width, int height)
	{
		this.data = null;
		this.cap = field;
		base.GetComponent<Camera>().targetTexture = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
		this.tex = new Texture2D((int)this.cap.width, (int)this.cap.height, TextureFormat.ARGB32, false);
	}

	private void OnPostRender()
	{
		if (this.tex != null)
		{
			this.tex.ReadPixels(this.cap, 0, 0);
			this.tex.Apply();
			this.data = this.tex.EncodeToPNG();
			this.tex = null;
			base.GetComponent<Camera>().targetTexture = null;
		}
	}

	public byte[] data;

	private Texture2D tex;

	private Rect cap;
}
