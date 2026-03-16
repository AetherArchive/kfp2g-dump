using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Camera))]
internal class AuthMovieCapture : MonoBehaviour
{
	[DllImport("AuthMovieCapture")]
	private static extern int capture_Initialize(int flags);

	[DllImport("AuthMovieCapture")]
	private static extern AuthMovieCapture.Status capture_GetStatus();

	[DllImport("AuthMovieCapture")]
	private static extern int capture_Prepare([MarshalAs(UnmanagedType.LPStruct)] AuthMovieCapture.Config in_config);

	[DllImport("AuthMovieCapture")]
	private static extern int capture_Start();

	[DllImport("AuthMovieCapture")]
	private static extern int capture_Stop();

	[DllImport("AuthMovieCapture")]
	private static extern bool capture_IsD3D11DeviceSingleThreaded(IntPtr texture);

	[DllImport("Kernel32", CharSet = CharSet.Ansi)]
	private static extern IntPtr GetModuleHandleA(string name);

	[DllImport("Kernel32", CharSet = CharSet.Ansi)]
	private static extern IntPtr GetProcAddress(IntPtr handle, string name);

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

	public AuthMovieCapture.Status status
	{
		get
		{
			return AuthMovieCapture.capture_GetStatus();
		}
	}

	public bool IsPreparing()
	{
		return this.status == AuthMovieCapture.Status.Initializing;
	}

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

	private void OnDisable()
	{
		this._preparing = false;
		this.CaptureStop();
	}

	private void OnEnable()
	{
	}

	private IEnumerator OnPostRender()
	{
		yield return new WaitForEndOfFrame();
		if (this.m_renderTex != null)
		{
			Graphics.ExecuteCommandBuffer(this.m_commandBuffer);
		}
		yield break;
	}

	private Camera m_camera;

	private RenderTexture m_renderTex;

	private CommandBuffer m_commandBuffer;

	private IntPtr m_putVideoFramePtr = IntPtr.Zero;

	private bool _recording;

	private bool _preparing;

	[SerializeField]
	protected int m_width = 1280;

	[SerializeField]
	protected int m_height = 720;

	[SerializeField]
	protected string m_filePath = "./AuthMovieCapture.m4v";

	[SerializeField]
	protected bool m_recording;

	private enum VideoPreset
	{
		VideoPreset_Default,
		VideoPreset_HP,
		VideoPreset_HQ,
		VideoPreset_BD,
		VideoPreset_LowLatencyDefault,
		VideoPreset_LowLatencyHP,
		VideoPreset_LowLatencyHQ,
		VideoPreset_LosslessDefault,
		VideoPreset_LosslessHP
	}

	private enum VideoRateControl
	{
		VideoRateControl_VBR_QP,
		VideoRateControl_CONST_QP,
		VideoRateControl_VBR,
		VideoRateControl_CBR,
		VideoRateControl_2PASS_QP,
		VideoRateControl_2PASS_VBR
	}

	private enum VideoH264Profile
	{
		VideoH264Profile_AutoSelect,
		VideoH264Profile_Baseline,
		VideoH264Profile_Main,
		VideoH264Profile_High
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 4)]
	private struct Config
	{
		public int audioInputFormat;

		public int audioInputChannels;

		public int audioInputFrequency;

		public int videoCrop_Left;

		public int videoCrop_Top;

		public int videoCrop_Right;

		public int videoCrop_Bottom;

		public int videoEncodeWidth;

		public int videoEncodeHeight;

		public int videoEncodeFrameRate_Numerator;

		public int videoEncodeFrameRate_Denominator;

		public int videoEncodePixelAspect_Numerator;

		public int videoEncodePixelAspect_Denominator;

		public AuthMovieCapture.VideoPreset videoEncodePreset;

		public AuthMovieCapture.VideoH264Profile videoEncodeH264Profile;

		public int videoEncodeH264Level;

		public AuthMovieCapture.VideoRateControl videoEncodeRateControlMode;

		public int videoEncodeGOPLength;

		public int videoEncodeBFrames;

		public int videoEncodeQualityMin;

		public int videoEncodeQualityMax;

		public int videoEncodeAvgBitrate;

		public int videoEncodeMaxBitrate;

		public int audioEncodeChannels;

		public int audioEncodeFrequency;

		public int audioEncodeBitrate;

		public int watermarkPosition;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string outputFilePath;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string watermarkFilePath;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string metaTitle;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string metaSubTitle;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string metaComment;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string metaKeywords;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string metaAuthor;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string metaPublisher;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string metaCopyright;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string metaGenre;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string metaAuthorUrl;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string metaPromotionUrl;
	}

	public enum Status
	{
		NotInitialized,
		Initializing,
		Initialized,
		Starting,
		Started,
		Stopping,
		Stopped,
		Shutdown
	}
}
