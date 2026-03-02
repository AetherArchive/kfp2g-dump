using System;
using UnityEngine;

// Token: 0x02000033 RID: 51
public class DebugQrTestCtrl : MonoBehaviour
{
	// Token: 0x060000BD RID: 189 RVA: 0x000068EA File Offset: 0x00004AEA
	private void Start()
	{
		this.encoder = base.transform.gameObject.AddComponent<QrCodeEncoder>();
	}

	// Token: 0x060000BE RID: 190 RVA: 0x00006902 File Offset: 0x00004B02
	private void Update()
	{
	}

	// Token: 0x060000BF RID: 191 RVA: 0x00006904 File Offset: 0x00004B04
	public void Setup()
	{
		QRCodeDef.QRMobileCard.CreateByteData(123, true);
		this.dispRawImage.SetTexture(this.encoder.QrTexture, false);
	}

	// Token: 0x04000130 RID: 304
	[SerializeField]
	private QrCodeEncoder encoder;

	// Token: 0x04000131 RID: 305
	[SerializeField]
	private PguiRawImageCtrl dispRawImage;
}
