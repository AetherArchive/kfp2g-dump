using System;
using UnityEngine;

public class DebugQrTestCtrl : MonoBehaviour
{
	private void Start()
	{
		this.encoder = base.transform.gameObject.AddComponent<QrCodeEncoder>();
	}

	private void Update()
	{
	}

	public void Setup()
	{
		QRCodeDef.QRMobileCard.CreateByteData(123, true);
		this.dispRawImage.SetTexture(this.encoder.QrTexture, false);
	}

	[SerializeField]
	private QrCodeEncoder encoder;

	[SerializeField]
	private PguiRawImageCtrl dispRawImage;
}
