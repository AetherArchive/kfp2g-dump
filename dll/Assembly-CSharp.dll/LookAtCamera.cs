using System;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
	private void Awake()
	{
		base.GetComponent<Renderer>() == null;
	}

	private void OnWillRenderObject()
	{
		if (Camera.current != EffectManager.BillboardCamera)
		{
			return;
		}
		Camera billboardCamera = EffectManager.BillboardCamera;
		if (billboardCamera == null)
		{
			return;
		}
		float z = base.transform.eulerAngles.z;
		if (this.VRotation)
		{
			base.transform.LookAt(billboardCamera.transform.position, billboardCamera.transform.up);
		}
		else
		{
			Vector3 position = billboardCamera.transform.position;
			position.y = base.transform.position.y;
			base.transform.LookAt(position, billboardCamera.transform.up);
		}
		base.transform.eulerAngles = new Vector3(base.transform.eulerAngles.x, base.transform.eulerAngles.y, z);
	}

	public bool VRotation = true;
}
