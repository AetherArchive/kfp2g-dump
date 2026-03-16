using System;
using UnityEngine;

public class AuthCamera
{
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

	public void SetActive(bool active)
	{
		if (this.cam == null)
		{
			return;
		}
		this.cam.gameObject.SetActive(active);
	}

	public void SetClipPlane(float near, float far)
	{
		if (this.cam == null)
		{
			return;
		}
		this.cam.nearClipPlane = near;
		this.cam.farClipPlane = far;
	}

	public void SetCameraPosition(Vector3 pos)
	{
		if (this.cam == null)
		{
			return;
		}
		this.cam.transform.position = pos;
	}

	public void SetInterPos(Vector3 intrPos, float twist)
	{
		if (this.cam == null)
		{
			return;
		}
		this.cam.transform.LookAt(intrPos);
		this.cam.transform.Rotate(0f, 0f, twist);
	}

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

	public static float AdjustFieldOfView(float inFov)
	{
		return inFov * Mathf.Max(1f, 1.7777778f / ((float)Screen.width / (float)Screen.height));
	}

	public Transform Trans
	{
		get
		{
			return this.cam.transform;
		}
	}

	public Camera cam;

	private FieldCameraScaler fcs;

	private const float defaultAspectRatio = 1.7777778f;
}
