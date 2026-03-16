using System;
using System.Text;
using SGNFW.Common;
using UnityEngine;
using ZXing;
using ZXing.QrCode;
using ZXing.QrCode.Internal;

public class QrCodeEncoder : Singleton<QrCodeEncoder>
{
	public Texture2D QrTexture
	{
		get
		{
			return this.qrTexture;
		}
	}

	public bool IsEncoded
	{
		get
		{
			return this.Encoded;
		}
	}

	public bool OnEncoded
	{
		get
		{
			return this.onEncoded;
		}
	}

	public QrCodeEncoder.EncodeOptions Options
	{
		get
		{
			return this.options;
		}
	}

	protected override void OnSingletonAwake()
	{
		this.qrTexture = new Texture2D(128, 128);
		this.qrRect = new Rect(0f, 80f, 128f, 128f);
	}

	private void Start()
	{
	}

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

	protected override void OnSingletonDestroy()
	{
		if (this.qrTexture)
		{
			Object.Destroy(this.qrTexture);
		}
		this.qrTexture = null;
	}

	public bool Encode(byte[] sourceBytes)
	{
		this.LastResult = new byte[sourceBytes.Length];
		Buffer.BlockCopy(sourceBytes, 0, this.LastResult, 0, sourceBytes.Length);
		this.Encoded = this.EncodeNow(sourceBytes);
		this.shouldEncodeNow = !this.Encoded;
		return this.Encoded;
	}

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

	public Texture2D qrTexture;

	private Rect qrRect;

	public byte[] LastResult;

	private bool Encoded;

	private bool onEncoded;

	private bool shouldEncodeNow;

	private const int qrImgSize = 128;

	public QrCodeEncoder.EncodeOptions options = new QrCodeEncoder.EncodeOptions();

	[Serializable]
	public class EncodeOptions
	{
		public const int MinHeaderLength = 4;

		public bool Encrypt;

		public bool DisplayTexture;

		public bool DisplayDebug;

		public string Password = "";

		public byte[] Salt;

		public byte[] Header;

		public bool IsHeaderSalt;

		public int IterationCount = 1000;

		public int StretchCount = 3;
	}
}
