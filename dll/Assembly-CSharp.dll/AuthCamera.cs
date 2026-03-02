using System;
using UnityEngine;

// Token: 0x0200003A RID: 58
public class AuthCamera
{
	// Token: 0x060000CE RID: 206 RVA: 0x00006A2C File Offset: 0x00004C2C
	public AuthCamera(Camera cam, bool isMain)
	{
		this.cam = cam;
		if (isMain)
		{
			this.cam.name = "AuthCameraMain";
			this.cam.cullingMask = 1 << LayerMask.NameToLayer("AuthMain");
			this.cam.cullingMask |= 1 << LayerMask.NameToLayer("FieldStage");
			this.cam.cullingMask |= 1 << LayerMask.NameToLayer("AuthMainShadow");
			this.cam.depth = (float)(SceneManager.CameraDepth[SceneManager.CanvasType.BACK] + 1);
		}
		else
		{
			this.cam.name = "AuthCameraSub";
			this.cam.cullingMask = 1 << LayerMask.NameToLayer("AuthSub");
			this.cam.depth = (float)(SceneManager.CameraDepth[SceneManager.CanvasType.BACK] + 3);
			this.cam.clearFlags = CameraClearFlags.Depth;
			this.cam.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
		}
		if ((this.fcs = this.cam.GetComponent<FieldCameraScaler>()) == null)
		{
			this.fcs = this.cam.gameObject.AddComponent<FieldCameraScaler>();
		}
	}

	// Token: 0x060000CF RID: 207 RVA: 0x00006B78 File Offset: 0x00004D78
	public void SetActive(bool active)
	{
		if (this.cam == null)
		{
			return;
		}
		this.cam.gameObject.SetActive(active);
	}

	// Token: 0x060000D0 RID: 208 RVA: 0x00006B9A File Offset: 0x00004D9A
	public void SetClipPlane(float near, float far)
	{
		if (this.cam == null)
		{
			return;
		}
		this.cam.nearClipPlane = near;
		this.cam.farClipPlane = far;
	}

	// Token: 0x060000D1 RID: 209 RVA: 0x00006BC3 File Offset: 0x00004DC3
	public void SetCameraPosition(Vector3 pos)
	{
		if (this.cam == null)
		{
			return;
		}
		this.cam.transform.position = pos;
	}

	// Token: 0x060000D2 RID: 210 RVA: 0x00006BE5 File Offset: 0x00004DE5
	public void SetInterPos(Vector3 intrPos, float twist)
	{
		if (this.cam == null)
		{
			return;
		}
		this.cam.transform.LookAt(intrPos);
		this.cam.transform.Rotate(0f, 0f, twist);
	}

	// Token: 0x060000D3 RID: 211 RVA: 0x00006C22 File Offset: 0x00004E22
	public void SetFieldOfView(float val)
	{
		if (this.fcs != null)
		{
			this.fcs.fieldOfView = val;
			return;
		}
		if (this.cam != null)
		{
			this.cam.fieldOfView = AuthCamera.AdjustFieldOfView(val);
		}
	}

	// Token: 0x060000D4 RID: 212 RVA: 0x00006C5E File Offset: 0x00004E5E
	public static float AdjustFieldOfView(float inFov)
	{
		return inFov * Mathf.Max(1f, 1.7777778f / ((float)Screen.width / (float)Screen.height));
	}

	// Token: 0x17000008 RID: 8
	// (get) Token: 0x060000D5 RID: 213 RVA: 0x00006C7F File Offset: 0x00004E7F
	public Transform Trans
	{
		get
		{
			return this.cam.transform;
		}
	}

	// Token: 0x04000137 RID: 311
	public Camera cam;

	// Token: 0x04000138 RID: 312
	private FieldCameraScaler fcs;

	// Token: 0x04000139 RID: 313
	private const float defaultAspectRatio = 1.7777778f;
}
