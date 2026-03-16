using System;
using System.Collections;
using System.IO;
using UnityEngine;

public class AuthPngCapture : MonoBehaviour
{
	public int width
	{
		get
		{
			return this.m_width;
		}
		set
		{
			this.m_width = value;
		}
	}

	public int height
	{
		get
		{
			return this.m_height;
		}
		set
		{
			this.m_height = value;
		}
	}

	public string filePath
	{
		get
		{
			return this.m_filePath;
		}
		set
		{
			this.m_filePath = value;
		}
	}

	public void CapturePrepare()
	{
		this.m_path = "../KemonoArts";
		if (!Directory.Exists(this.m_path))
		{
			Directory.CreateDirectory(this.m_path);
		}
		this.m_path = this.m_path + "/" + this.filePath;
		if (!Directory.Exists(this.m_path))
		{
			Directory.CreateDirectory(this.m_path);
		}
		this.m_rect = new Rect(0f, 0f, (float)this.width, (float)this.height);
		this.m_frame = 0;
		this.m_capture = false;
	}

	public void CaptureStart()
	{
		this.m_capture = true;
	}

	public void CaptureStop()
	{
		this.m_capture = false;
	}

	private void Update()
	{
	}

	private IEnumerator OnPostRender()
	{
		yield return new WaitForEndOfFrame();
		if (this.m_capture)
		{
			if (this.m_frame > 0)
			{
				ScreenCapture.CaptureScreenshot(this.m_path + "/" + this.m_frame.ToString("D5") + ".png");
			}
			this.m_frame++;
		}
		yield break;
	}

	[SerializeField]
	protected int m_width = 1280;

	[SerializeField]
	protected int m_height = 720;

	[SerializeField]
	protected string m_filePath = "./AuthPngCapture";

	private string m_path;

	private Rect m_rect;

	private int m_frame;

	private bool m_capture;
}
