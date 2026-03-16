using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public class DebugQrPhoto : MonoBehaviour
{
	private void Start()
	{
	}

	public void MakePhotoList()
	{
		this.photo = new List<KeyValuePair<int, bool>>();
		foreach (PhotoStaticData photoStaticData in DataManager.DmPhoto.GetPhotoStaticMap().Values)
		{
			if (!File.Exists(DebugQrPhoto.QrPath + photoStaticData.GetQrImageName() + ".png"))
			{
				this.photo.Add(new KeyValuePair<int, bool>(photoStaticData.GetId(), false));
			}
			if (!File.Exists(DebugQrPhoto.QrPath + photoStaticData.GetQrImageName() + "_2.png"))
			{
				this.photo.Add(new KeyValuePair<int, bool>(photoStaticData.GetId(), true));
			}
		}
	}

	private void Update()
	{
		if (this.nam == null)
		{
			this.nam = base.transform.parent.GetComponentInChildren<PguiTextCtrl>();
			this.nam.text = "START";
		}
		if (this.photo == null)
		{
			return;
		}
		if (this.img == null)
		{
			this.img = base.transform.parent.GetComponentInChildren<PguiRawImageCtrl>();
			this.rect = this.img.GetComponent<RectTransform>().rect;
			this.rect.position = this.rect.position + new Vector2((float)Screen.width * 0.5f, (float)Screen.height * 0.5f);
		}
		if (this.capture == null)
		{
			Camera componentInChildren = base.transform.parent.GetComponentInChildren<Camera>();
			this.capture = componentInChildren.gameObject.AddComponent<ImageCapture>();
		}
		if (this.encoder == null)
		{
			List<string> list = new List<string>();
			byte[] array = File.ReadAllBytes("Assets/Parade/Prefabs/Debug/QrCode.txt");
			string text = "";
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == 13 && array[i + 1] == 10)
				{
					list.Add(text);
					text = "";
					i++;
				}
				else if (array[i] == 92)
				{
					if (array[i + 1] == 116)
					{
						text += "\t";
						i++;
					}
					else if (array[i + 1] == 114)
					{
						text += "\r";
						i++;
					}
					else if (array[i + 1] == 110)
					{
						text += "\n";
						i++;
					}
				}
				else
				{
					string text2 = text;
					char c = (char)array[i];
					text = text2 + c.ToString();
				}
			}
			if (text.Length > 0)
			{
				list.Add(text);
			}
			if (list == null || list.Count != 3)
			{
				this.photo = null;
				return;
			}
			this.encoder = base.gameObject.AddComponent<QrCodeEncoder>();
			this.encoder.Options.Encrypt = true;
			this.encoder.Options.DisplayTexture = false;
			this.encoder.Options.DisplayDebug = false;
			this.encoder.Options.Password = list[0];
			this.encoder.Options.Salt = Encoding.UTF8.GetBytes(list[1]);
			this.encoder.Options.IsHeaderSalt = true;
			this.encoder.Options.IterationCount = 1000;
			this.encoder.Options.StretchCount = 3;
			this.encoder.Options.Header = new byte[9];
			QRCodeDef.QRCardGameIDInfo qrcardGameIDInfo = new QRCodeDef.QRCardGameIDInfo();
			qrcardGameIDInfo.CardID = Encoding.ASCII.GetBytes(list[2]);
			qrcardGameIDInfo.PrintDate = (ushort)(TimeManager.SystemNow - DebugQrPhoto.StandardDate).TotalDays;
			qrcardGameIDInfo.GameVersion = 0;
			GCHandle gchandle = GCHandle.Alloc(this.encoder.Options.Header, GCHandleType.Pinned);
			Marshal.StructureToPtr<QRCodeDef.QRCardGameIDInfo>(qrcardGameIDInfo, gchandle.AddrOfPinnedObject(), false);
			gchandle.Free();
		}
		if (this.step == 0)
		{
			if (this.photo.Count > 0)
			{
				this.path = DebugQrPhoto.QrPath + DataManager.DmPhoto.GetPhotoStaticData(this.photo[0].Key).GetQrImageName() + (this.photo[0].Value ? "_2" : "") + ".png";
				this.nam.text = Path.GetFileNameWithoutExtension(this.path).Substring(9);
				byte[] array2 = QRCodeDef.QRMobileCard.CreateByteData(this.photo[0].Key, this.photo[0].Value);
				this.encoder.Encode(array2);
				this.img.SetTexture(this.encoder.QrTexture, false);
				this.step++;
				return;
			}
			this.nam.text = "END";
			return;
		}
		else
		{
			if (this.step == 1)
			{
				this.capture.capture(this.rect, Screen.width, Screen.height);
				this.step++;
				return;
			}
			if (this.step == 2)
			{
				if (this.capture.data != null)
				{
					string directoryName = Path.GetDirectoryName(this.path);
					if (!Directory.Exists(directoryName))
					{
						Directory.CreateDirectory(directoryName);
					}
					File.WriteAllBytes(this.path, this.capture.data);
					this.capture.data = null;
					this.photo.RemoveAt(0);
					this.step++;
					return;
				}
			}
			else
			{
				this.step = 0;
			}
			return;
		}
	}

	private static readonly string QrPath = "Assets/DesignBaseData/";

	private static readonly DateTime StandardDate = new DateTime(2019, 1, 1);

	private int step;

	private PguiTextCtrl nam;

	private PguiRawImageCtrl img;

	private Rect rect = Rect.zero;

	private ImageCapture capture;

	private QrCodeEncoder encoder;

	private List<KeyValuePair<int, bool>> photo;

	private string path;
}
