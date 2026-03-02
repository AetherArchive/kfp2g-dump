using System;
using System.Text;
using SGNFW.Common;
using UnityEngine;
using ZXing;
using ZXing.QrCode;
using ZXing.QrCode.Internal;

// Token: 0x020000D5 RID: 213
public class QrCodeEncoder : Singleton<QrCodeEncoder>
{
	// Token: 0x17000202 RID: 514
	// (get) Token: 0x060009A7 RID: 2471 RVA: 0x0003ADB6 File Offset: 0x00038FB6
	public Texture2D QrTexture
	{
		get
		{
			return this.qrTexture;
		}
	}

	// Token: 0x17000203 RID: 515
	// (get) Token: 0x060009A8 RID: 2472 RVA: 0x0003ADBE File Offset: 0x00038FBE
	public bool IsEncoded
	{
		get
		{
			return this.Encoded;
		}
	}

	// Token: 0x17000204 RID: 516
	// (get) Token: 0x060009A9 RID: 2473 RVA: 0x0003ADC6 File Offset: 0x00038FC6
	public bool OnEncoded
	{
		get
		{
			return this.onEncoded;
		}
	}

	// Token: 0x17000205 RID: 517
	// (get) Token: 0x060009AA RID: 2474 RVA: 0x0003ADCE File Offset: 0x00038FCE
	public QrCodeEncoder.EncodeOptions Options
	{
		get
		{
			return this.options;
		}
	}

	// Token: 0x060009AB RID: 2475 RVA: 0x0003ADD6 File Offset: 0x00038FD6
	protected override void OnSingletonAwake()
	{
		this.qrTexture = new Texture2D(128, 128);
		this.qrRect = new Rect(0f, 80f, 128f, 128f);
	}

	// Token: 0x060009AC RID: 2476 RVA: 0x0003AE0C File Offset: 0x0003900C
	private void Start()
	{
	}

	// Token: 0x060009AD RID: 2477 RVA: 0x0003AE10 File Offset: 0x00039010
	private void Update()
	{
		this.onEncoded = false;
		if (this.shouldEncodeNow)
		{
			byte[] lastResult = this.LastResult;
			if (lastResult != null)
			{
				this.Encoded = this.EncodeNow(lastResult);
				this.shouldEncodeNow = !this.Encoded;
			}
		}
	}

	// Token: 0x060009AE RID: 2478 RVA: 0x0003AE52 File Offset: 0x00039052
	protected override void OnSingletonDestroy()
	{
		if (this.qrTexture)
		{
			Object.Destroy(this.qrTexture);
		}
		this.qrTexture = null;
	}

	// Token: 0x060009AF RID: 2479 RVA: 0x0003AE74 File Offset: 0x00039074
	public bool Encode(byte[] sourceBytes)
	{
		this.LastResult = new byte[sourceBytes.Length];
		Buffer.BlockCopy(sourceBytes, 0, this.LastResult, 0, sourceBytes.Length);
		this.Encoded = this.EncodeNow(sourceBytes);
		this.shouldEncodeNow = !this.Encoded;
		return this.Encoded;
	}

	// Token: 0x060009B0 RID: 2480 RVA: 0x0003AEC4 File Offset: 0x000390C4
	private bool EncodeNow(byte[] sourceBytes)
	{
		this.Encoded = false;
		if (this.qrTexture)
		{
			byte[] array = null;
			if (this.options.Encrypt)
			{
				try
				{
					if (this.options.Header.Length >= 4)
					{
						this.options.Header[0] = (byte)Random.Range(96, 255);
						for (int i = 1; i < 4; i++)
						{
							this.options.Header[i] = (byte)Random.Range(0, 255);
						}
					}
					byte[] array2 = new byte[this.options.Salt.Length];
					Buffer.BlockCopy(this.options.Salt, 0, array2, 0, this.options.Salt.Length);
					int num = 0;
					int num2 = 0;
					string text = this.options.Password;
					if (this.options.Header != null && this.options.Header.Length >= 4 && this.Options.IsHeaderSalt)
					{
						text = QrCodeUtil.Encription.convertHeaderToParam(this.options.Header, this.options.Password, this.options.Salt, array2, out num, out num2);
					}
					byte[] array3 = QrCodeUtil.Encription.EncryptBuffer(sourceBytes, text, array2, this.options.IterationCount + num, this.options.StretchCount + num2);
					array = new byte[this.options.Header.Length + array3.Length + 1];
					Buffer.BlockCopy(this.options.Header, 0, array, 0, this.options.Header.Length);
					Buffer.BlockCopy(array3, 0, array, this.options.Header.Length, array3.Length);
					goto IL_01AD;
				}
				catch (Exception)
				{
					goto IL_01AD;
				}
			}
			array = new byte[sourceBytes.Length + 1];
			Buffer.BlockCopy(sourceBytes, 0, array, 0, sourceBytes.Length);
			IL_01AD:
			if (array != null)
			{
				array[array.Length - 1] = QRCodeDef.Util.CalcCRC7(array, 0, array.Length - 1);
				Color32[] array4 = QrCodeEncoder.EncodeImage(array, this.qrTexture.width, this.qrTexture.height);
				this.qrTexture.SetPixels32(array4);
				this.qrTexture.Apply();
				this.Encoded = true;
				this.onEncoded = true;
			}
		}
		return this.Encoded;
	}

	// Token: 0x060009B1 RID: 2481 RVA: 0x0003B0FC File Offset: 0x000392FC
	private static Color32[] EncodeImage(byte[] bytes, int width, int height)
	{
		BarcodeWriter barcodeWriter = new BarcodeWriter
		{
			Format = BarcodeFormat.QR_CODE,
			Options = new QrCodeEncodingOptions
			{
				Height = height,
				Width = width,
				Margin = 2,
				ErrorCorrection = ErrorCorrectionLevel.M,
				CharacterSet = "windows-1252",
				DisableECI = true
			}
		};
		string @string = Encoding.GetEncoding(1252).GetString(bytes);
		Color32[] array;
		try
		{
			array = barcodeWriter.Write(@string);
		}
		catch (Exception)
		{
			array = null;
		}
		return array;
	}

	// Token: 0x040007C8 RID: 1992
	public Texture2D qrTexture;

	// Token: 0x040007C9 RID: 1993
	private Rect qrRect;

	// Token: 0x040007CA RID: 1994
	public byte[] LastResult;

	// Token: 0x040007CB RID: 1995
	private bool Encoded;

	// Token: 0x040007CC RID: 1996
	private bool onEncoded;

	// Token: 0x040007CD RID: 1997
	private bool shouldEncodeNow;

	// Token: 0x040007CE RID: 1998
	private const int qrImgSize = 128;

	// Token: 0x040007CF RID: 1999
	public QrCodeEncoder.EncodeOptions options = new QrCodeEncoder.EncodeOptions();

	// Token: 0x020007CE RID: 1998
	[Serializable]
	public class EncodeOptions
	{
		// Token: 0x040034BE RID: 13502
		public const int MinHeaderLength = 4;

		// Token: 0x040034BF RID: 13503
		public bool Encrypt;

		// Token: 0x040034C0 RID: 13504
		public bool DisplayTexture;

		// Token: 0x040034C1 RID: 13505
		public bool DisplayDebug;

		// Token: 0x040034C2 RID: 13506
		public string Password = "";

		// Token: 0x040034C3 RID: 13507
		public byte[] Salt;

		// Token: 0x040034C4 RID: 13508
		public byte[] Header;

		// Token: 0x040034C5 RID: 13509
		public bool IsHeaderSalt;

		// Token: 0x040034C6 RID: 13510
		public int IterationCount = 1000;

		// Token: 0x040034C7 RID: 13511
		public int StretchCount = 3;
	}
}
