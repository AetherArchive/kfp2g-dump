using System;
using System.Collections;
using System.IO;
using UnityEngine;

// Token: 0x020001EC RID: 492
public class AuthPngCapture : MonoBehaviour
{
	// Token: 0x17000468 RID: 1128
	// (get) Token: 0x06002129 RID: 8489 RVA: 0x0018DF00 File Offset: 0x0018C100
	// (set) Token: 0x0600212A RID: 8490 RVA: 0x0018DF08 File Offset: 0x0018C108
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

	// Token: 0x17000469 RID: 1129
	// (get) Token: 0x0600212B RID: 8491 RVA: 0x0018DF11 File Offset: 0x0018C111
	// (set) Token: 0x0600212C RID: 8492 RVA: 0x0018DF19 File Offset: 0x0018C119
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

	// Token: 0x1700046A RID: 1130
	// (get) Token: 0x0600212D RID: 8493 RVA: 0x0018DF22 File Offset: 0x0018C122
	// (set) Token: 0x0600212E RID: 8494 RVA: 0x0018DF2A File Offset: 0x0018C12A
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

	// Token: 0x0600212F RID: 8495 RVA: 0x0018DF34 File Offset: 0x0018C134
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

	// Token: 0x06002130 RID: 8496 RVA: 0x0018DFCB File Offset: 0x0018C1CB
	public void CaptureStart()
	{
		this.m_capture = true;
	}

	// Token: 0x06002131 RID: 8497 RVA: 0x0018DFD4 File Offset: 0x0018C1D4
	public void CaptureStop()
	{
		this.m_capture = false;
	}

	// Token: 0x06002132 RID: 8498 RVA: 0x0018DFDD File Offset: 0x0018C1DD
	private void Update()
	{
	}

	// Token: 0x06002133 RID: 8499 RVA: 0x0018DFDF File Offset: 0x0018C1DF
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

	// Token: 0x040017CF RID: 6095
	[SerializeField]
	protected int m_width = 1280;

	// Token: 0x040017D0 RID: 6096
	[SerializeField]
	protected int m_height = 720;

	// Token: 0x040017D1 RID: 6097
	[SerializeField]
	protected string m_filePath = "./AuthPngCapture";

	// Token: 0x040017D2 RID: 6098
	private string m_path;

	// Token: 0x040017D3 RID: 6099
	private Rect m_rect;

	// Token: 0x040017D4 RID: 6100
	private int m_frame;

	// Token: 0x040017D5 RID: 6101
	private bool m_capture;
}
