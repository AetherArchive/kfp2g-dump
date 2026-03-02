using System;
using UnityEngine;

// Token: 0x020000CC RID: 204
public class LookAtCamera : MonoBehaviour
{
	// Token: 0x0600091A RID: 2330 RVA: 0x00039C88 File Offset: 0x00037E88
	private void Awake()
	{
		base.GetComponent<Renderer>() == null;
	}

	// Token: 0x0600091B RID: 2331 RVA: 0x00039C98 File Offset: 0x00037E98
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

	// Token: 0x04000775 RID: 1909
	public bool VRotation = true;
}
