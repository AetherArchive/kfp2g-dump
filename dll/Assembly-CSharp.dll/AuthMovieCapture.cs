using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x020001EB RID: 491
[RequireComponent(typeof(Camera))]
internal class AuthMovieCapture : MonoBehaviour
{
	// Token: 0x06002111 RID: 8465
	[DllImport("AuthMovieCapture")]
	private static extern int capture_Initialize(int flags);

	// Token: 0x06002112 RID: 8466
	[DllImport("AuthMovieCapture")]
	private static extern AuthMovieCapture.Status capture_GetStatus();

	// Token: 0x06002113 RID: 8467
	[DllImport("AuthMovieCapture")]
	private static extern int capture_Prepare([MarshalAs(UnmanagedType.LPStruct)] AuthMovieCapture.Config in_config);

	// Token: 0x06002114 RID: 8468
	[DllImport("AuthMovieCapture")]
	private static extern int capture_Start();

	// Token: 0x06002115 RID: 8469
	[DllImport("AuthMovieCapture")]
	private static extern int capture_Stop();

	// Token: 0x06002116 RID: 8470
	[DllImport("AuthMovieCapture")]
	private static extern bool capture_IsD3D11DeviceSingleThreaded(IntPtr texture);

	// Token: 0x06002117 RID: 8471
	[DllImport("Kernel32", CharSet = CharSet.Ansi)]
	private static extern IntPtr GetModuleHandleA(string name);

	// Token: 0x06002118 RID: 8472
	[DllImport("Kernel32", CharSet = CharSet.Ansi)]
	private static extern IntPtr GetProcAddress(IntPtr handle, string name);

	// Token: 0x17000464 RID: 1124
	// (get) Token: 0x06002119 RID: 8473 RVA: 0x0018DA40 File Offset: 0x0018BC40
	// (set) Token: 0x0600211A RID: 8474 RVA: 0x0018DA48 File Offset: 0x0018BC48
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

	// Token: 0x17000465 RID: 1125
	// (get) Token: 0x0600211B RID: 8475 RVA: 0x0018DA51 File Offset: 0x0018BC51
	// (set) Token: 0x0600211C RID: 8476 RVA: 0x0018DA59 File Offset: 0x0018BC59
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

	// Token: 0x17000466 RID: 1126
	// (get) Token: 0x0600211D RID: 8477 RVA: 0x0018DA62 File Offset: 0x0018BC62
	// (set) Token: 0x0600211E RID: 8478 RVA: 0x0018DA6A File Offset: 0x0018BC6A
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

	// Token: 0x17000467 RID: 1127
	// (get) Token: 0x0600211F RID: 8479 RVA: 0x0018DA73 File Offset: 0x0018BC73
	public AuthMovieCapture.Status status
	{
		get
		{
			return AuthMovieCapture.capture_GetStatus();
		}
	}

	// Token: 0x06002120 RID: 8480 RVA: 0x0018DA7A File Offset: 0x0018BC7A
	public bool IsPreparing()
	{
		return this.status == AuthMovieCapture.Status.Initializing;
	}

	// Token: 0x06002121 RID: 8481 RVA: 0x0018DA88 File Offset: 0x0018BC88
	public void CaptureStart()
	{
		this.CaptureStop();
		this.m_camera = base.GetComponent<Camera>();
		this.m_renderTex = new RenderTexture(this.m_camera.pixelWidth, this.m_camera.pixelHeight, 0);
		this.m_renderTex.Create();
		this.m_commandBuffer = new CommandBuffer();
		this.m_commandBuffer.name = "AuthMovieCapture: framebuffer copy";
		this.m_commandBuffer.Blit(BuiltinRenderTextureType.CameraTarget, this.m_renderTex);
		this.m_commandBuffer.IssuePluginEventAndData(this.m_putVideoFramePtr, 0, this.m_renderTex.GetNativeTexturePtr());
		int num = AuthMovieCapture.capture_Start();
		if (num < 0)
		{
			throw new Exception(string.Format("AuthMovieCapture: Start failed. hr={0:X8}", num));
		}
	}

	// Token: 0x06002122 RID: 8482 RVA: 0x0018DB4C File Offset: 0x0018BD4C
	public void CaptureStop()
	{
		AuthMovieCapture.capture_Stop();
		if (this.m_commandBuffer != null)
		{
			this.m_camera != null;
			this.m_commandBuffer.Release();
			this.m_commandBuffer = null;
		}
		if (this.m_renderTex != null)
		{
			this.m_renderTex.Release();
			this.m_renderTex = null;
		}
	}

	// Token: 0x06002123 RID: 8483 RVA: 0x0018DBA8 File Offset: 0x0018BDA8
	public void CapturePrepare()
	{
		int outputSampleRate = AudioSettings.outputSampleRate;
		this.m_camera = base.GetComponent<Camera>();
		int num;
		switch (AudioSettings.speakerMode)
		{
		case AudioSpeakerMode.Mono:
			num = 1;
			break;
		case AudioSpeakerMode.Stereo:
			num = 2;
			break;
		case AudioSpeakerMode.Quad:
			num = 4;
			break;
		case AudioSpeakerMode.Surround:
			num = 5;
			break;
		case AudioSpeakerMode.Mode5point1:
			num = 6;
			break;
		case AudioSpeakerMode.Mode7point1:
			num = 8;
			break;
		default:
			throw new Exception("AuthMovieCapture: Unsupported AudioSpeakerChannels");
		}
		GraphicsDeviceType graphicsDeviceType = SystemInfo.graphicsDeviceType;
		string text;
		if (graphicsDeviceType <= GraphicsDeviceType.OpenGLES2)
		{
			if (graphicsDeviceType != GraphicsDeviceType.Direct3D11)
			{
				if (graphicsDeviceType != GraphicsDeviceType.OpenGLES2)
				{
					goto IL_00DB;
				}
			}
			else
			{
				text = "capture_PutVideoFrameD3D11";
				RenderTexture renderTexture = new RenderTexture(this.m_camera.pixelWidth, this.m_camera.pixelHeight, 0);
				renderTexture.Create();
				bool flag = AuthMovieCapture.capture_IsD3D11DeviceSingleThreaded(renderTexture.GetNativeTexturePtr());
				renderTexture.Release();
				if (flag)
				{
					throw new Exception("AuthMovieCapture: Unsupported D3D11 single threaded mode");
				}
				goto IL_00E6;
			}
		}
		else if (graphicsDeviceType != GraphicsDeviceType.OpenGLES3 && graphicsDeviceType != GraphicsDeviceType.OpenGLCore)
		{
			goto IL_00DB;
		}
		text = "capture_PutVideoFrameOpenGLCore";
		goto IL_00E6;
		IL_00DB:
		throw new Exception("AuthMovieCapture: Unsupported GraphicsDeviceType");
		IL_00E6:
		string text2 = "../KemonoArts";
		if (!Directory.Exists(text2))
		{
			Directory.CreateDirectory(text2);
		}
		text2 = text2 + "/" + this.m_filePath;
		AuthMovieCapture.Config config = default(AuthMovieCapture.Config);
		config.videoCrop_Left = 0;
		config.videoCrop_Top = 0;
		config.videoCrop_Right = 0;
		config.videoCrop_Bottom = 0;
		config.videoEncodeWidth = this.m_width;
		config.videoEncodeHeight = this.m_height;
		config.videoEncodeFrameRate_Numerator = 60000;
		config.videoEncodeFrameRate_Denominator = 1000;
		config.videoEncodePixelAspect_Numerator = 1;
		config.videoEncodePixelAspect_Denominator = 1;
		config.videoEncodePreset = AuthMovieCapture.VideoPreset.VideoPreset_HQ;
		config.videoEncodeH264Profile = AuthMovieCapture.VideoH264Profile.VideoH264Profile_AutoSelect;
		config.videoEncodeH264Level = 0;
		config.videoEncodeRateControlMode = AuthMovieCapture.VideoRateControl.VideoRateControl_VBR_QP;
		config.videoEncodeGOPLength = 30;
		config.videoEncodeBFrames = 0;
		config.videoEncodeQualityMin = 16;
		config.videoEncodeQualityMax = 18;
		config.videoEncodeAvgBitrate = 0;
		config.videoEncodeMaxBitrate = 0;
		config.audioInputFormat = 3;
		config.audioInputChannels = num;
		config.audioInputFrequency = outputSampleRate;
		config.audioEncodeChannels = 2;
		config.audioEncodeFrequency = 48000;
		config.audioEncodeBitrate = 128000;
		config.watermarkPosition = 0;
		config.outputFilePath = text2;
		int num2 = AuthMovieCapture.capture_Initialize(1);
		if (num2 < 0)
		{
			throw new Exception(string.Format("AuthMovieCapture: Initialize failed. hr={0:X8}", num2));
		}
		num2 = AuthMovieCapture.capture_Prepare(config);
		if (num2 < 0)
		{
			throw new Exception(string.Format("AuthMovieCapture: Prepare failed. hr={0:X8}", num2));
		}
		this.m_putVideoFramePtr = AuthMovieCapture.GetProcAddress(AuthMovieCapture.GetModuleHandleA("AuthMovieCapture"), text);
		if (this.m_putVideoFramePtr == IntPtr.Zero)
		{
			throw new Exception("AuthMovieCapture: PutVideoFrame function not found");
		}
	}

	// Token: 0x06002124 RID: 8484 RVA: 0x0018DE44 File Offset: 0x0018C044
	private void Update()
	{
		if (this._recording != this.m_recording)
		{
			this._recording = this.m_recording;
			if (this._recording)
			{
				this.CapturePrepare();
				this._preparing = true;
			}
			else
			{
				this._preparing = false;
				this.CaptureStop();
			}
		}
		if (this._preparing && !this.IsPreparing())
		{
			this._preparing = false;
			this.CaptureStart();
		}
	}

	// Token: 0x06002125 RID: 8485 RVA: 0x0018DEAC File Offset: 0x0018C0AC
	private void OnDisable()
	{
		this._preparing = false;
		this.CaptureStop();
	}

	// Token: 0x06002126 RID: 8486 RVA: 0x0018DEBB File Offset: 0x0018C0BB
	private void OnEnable()
	{
	}

	// Token: 0x06002127 RID: 8487 RVA: 0x0018DEBD File Offset: 0x0018C0BD
	private IEnumerator OnPostRender()
	{
		yield return new WaitForEndOfFrame();
		if (this.m_renderTex != null)
		{
			Graphics.ExecuteCommandBuffer(this.m_commandBuffer);
		}
		yield break;
	}

	// Token: 0x040017C5 RID: 6085
	private Camera m_camera;

	// Token: 0x040017C6 RID: 6086
	private RenderTexture m_renderTex;

	// Token: 0x040017C7 RID: 6087
	private CommandBuffer m_commandBuffer;

	// Token: 0x040017C8 RID: 6088
	private IntPtr m_putVideoFramePtr = IntPtr.Zero;

	// Token: 0x040017C9 RID: 6089
	private bool _recording;

	// Token: 0x040017CA RID: 6090
	private bool _preparing;

	// Token: 0x040017CB RID: 6091
	[SerializeField]
	protected int m_width = 1280;

	// Token: 0x040017CC RID: 6092
	[SerializeField]
	protected int m_height = 720;

	// Token: 0x040017CD RID: 6093
	[SerializeField]
	protected string m_filePath = "./AuthMovieCapture.m4v";

	// Token: 0x040017CE RID: 6094
	[SerializeField]
	protected bool m_recording;

	// Token: 0x0200103C RID: 4156
	private enum VideoPreset
	{
		// Token: 0x04005AFB RID: 23291
		VideoPreset_Default,
		// Token: 0x04005AFC RID: 23292
		VideoPreset_HP,
		// Token: 0x04005AFD RID: 23293
		VideoPreset_HQ,
		// Token: 0x04005AFE RID: 23294
		VideoPreset_BD,
		// Token: 0x04005AFF RID: 23295
		VideoPreset_LowLatencyDefault,
		// Token: 0x04005B00 RID: 23296
		VideoPreset_LowLatencyHP,
		// Token: 0x04005B01 RID: 23297
		VideoPreset_LowLatencyHQ,
		// Token: 0x04005B02 RID: 23298
		VideoPreset_LosslessDefault,
		// Token: 0x04005B03 RID: 23299
		VideoPreset_LosslessHP
	}

	// Token: 0x0200103D RID: 4157
	private enum VideoRateControl
	{
		// Token: 0x04005B05 RID: 23301
		VideoRateControl_VBR_QP,
		// Token: 0x04005B06 RID: 23302
		VideoRateControl_CONST_QP,
		// Token: 0x04005B07 RID: 23303
		VideoRateControl_VBR,
		// Token: 0x04005B08 RID: 23304
		VideoRateControl_CBR,
		// Token: 0x04005B09 RID: 23305
		VideoRateControl_2PASS_QP,
		// Token: 0x04005B0A RID: 23306
		VideoRateControl_2PASS_VBR
	}

	// Token: 0x0200103E RID: 4158
	private enum VideoH264Profile
	{
		// Token: 0x04005B0C RID: 23308
		VideoH264Profile_AutoSelect,
		// Token: 0x04005B0D RID: 23309
		VideoH264Profile_Baseline,
		// Token: 0x04005B0E RID: 23310
		VideoH264Profile_Main,
		// Token: 0x04005B0F RID: 23311
		VideoH264Profile_High
	}

	// Token: 0x0200103F RID: 4159
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 4)]
	private struct Config
	{
		// Token: 0x04005B10 RID: 23312
		public int audioInputFormat;

		// Token: 0x04005B11 RID: 23313
		public int audioInputChannels;

		// Token: 0x04005B12 RID: 23314
		public int audioInputFrequency;

		// Token: 0x04005B13 RID: 23315
		public int videoCrop_Left;

		// Token: 0x04005B14 RID: 23316
		public int videoCrop_Top;

		// Token: 0x04005B15 RID: 23317
		public int videoCrop_Right;

		// Token: 0x04005B16 RID: 23318
		public int videoCrop_Bottom;

		// Token: 0x04005B17 RID: 23319
		public int videoEncodeWidth;

		// Token: 0x04005B18 RID: 23320
		public int videoEncodeHeight;

		// Token: 0x04005B19 RID: 23321
		public int videoEncodeFrameRate_Numerator;

		// Token: 0x04005B1A RID: 23322
		public int videoEncodeFrameRate_Denominator;

		// Token: 0x04005B1B RID: 23323
		public int videoEncodePixelAspect_Numerator;

		// Token: 0x04005B1C RID: 23324
		public int videoEncodePixelAspect_Denominator;

		// Token: 0x04005B1D RID: 23325
		public AuthMovieCapture.VideoPreset videoEncodePreset;

		// Token: 0x04005B1E RID: 23326
		public AuthMovieCapture.VideoH264Profile videoEncodeH264Profile;

		// Token: 0x04005B1F RID: 23327
		public int videoEncodeH264Level;

		// Token: 0x04005B20 RID: 23328
		public AuthMovieCapture.VideoRateControl videoEncodeRateControlMode;

		// Token: 0x04005B21 RID: 23329
		public int videoEncodeGOPLength;

		// Token: 0x04005B22 RID: 23330
		public int videoEncodeBFrames;

		// Token: 0x04005B23 RID: 23331
		public int videoEncodeQualityMin;

		// Token: 0x04005B24 RID: 23332
		public int videoEncodeQualityMax;

		// Token: 0x04005B25 RID: 23333
		public int videoEncodeAvgBitrate;

		// Token: 0x04005B26 RID: 23334
		public int videoEncodeMaxBitrate;

		// Token: 0x04005B27 RID: 23335
		public int audioEncodeChannels;

		// Token: 0x04005B28 RID: 23336
		public int audioEncodeFrequency;

		// Token: 0x04005B29 RID: 23337
		public int audioEncodeBitrate;

		// Token: 0x04005B2A RID: 23338
		public int watermarkPosition;

		// Token: 0x04005B2B RID: 23339
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string outputFilePath;

		// Token: 0x04005B2C RID: 23340
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string watermarkFilePath;

		// Token: 0x04005B2D RID: 23341
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string metaTitle;

		// Token: 0x04005B2E RID: 23342
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string metaSubTitle;

		// Token: 0x04005B2F RID: 23343
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string metaComment;

		// Token: 0x04005B30 RID: 23344
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string metaKeywords;

		// Token: 0x04005B31 RID: 23345
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string metaAuthor;

		// Token: 0x04005B32 RID: 23346
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string metaPublisher;

		// Token: 0x04005B33 RID: 23347
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string metaCopyright;

		// Token: 0x04005B34 RID: 23348
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string metaGenre;

		// Token: 0x04005B35 RID: 23349
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string metaAuthorUrl;

		// Token: 0x04005B36 RID: 23350
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string metaPromotionUrl;
	}

	// Token: 0x02001040 RID: 4160
	public enum Status
	{
		// Token: 0x04005B38 RID: 23352
		NotInitialized,
		// Token: 0x04005B39 RID: 23353
		Initializing,
		// Token: 0x04005B3A RID: 23354
		Initialized,
		// Token: 0x04005B3B RID: 23355
		Starting,
		// Token: 0x04005B3C RID: 23356
		Started,
		// Token: 0x04005B3D RID: 23357
		Stopping,
		// Token: 0x04005B3E RID: 23358
		Stopped,
		// Token: 0x04005B3F RID: 23359
		Shutdown
	}
}
