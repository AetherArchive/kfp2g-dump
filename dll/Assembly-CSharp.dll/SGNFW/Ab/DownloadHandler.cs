using System;
using System.IO;
using UnityEngine.Networking;

namespace SGNFW.Ab
{
	internal class DownloadHandler : DownloadHandlerScript
	{
		public uint Hash
		{
			get
			{
				return this.hash;
			}
		}

		public void CloseFileStream()
		{
			if (this.fs != null)
			{
				this.fs.Dispose();
				this.fs = null;
			}
		}

		public DownloadHandler(string path)
			: base(new byte[131072])
		{
			this.path = path;
			this.hash = uint.MaxValue;
		}

		protected override bool ReceiveData(byte[] data, int dataLength)
		{
			if (data == null || data.Length < 1)
			{
				return false;
			}
			this.value += dataLength;
			try
			{
				if (this.fs == null)
				{
					string directoryName = Path.GetDirectoryName(this.path);
					if (!Directory.Exists(directoryName))
					{
						Directory.CreateDirectory(directoryName);
					}
					this.fs = new FileStream(this.path, FileMode.Create, FileAccess.Write);
				}
				this.fs.Write(data, 0, dataLength);
				this.hash = CRC32.GetHash(data, dataLength, this.hash);
			}
			catch (Exception ex)
			{
				this.CloseFileStream();
				if (this.onFailedWrite != null)
				{
					this.onFailedWrite(this.path, ex);
				}
			}
			return true;
		}

		protected override void CompleteContent()
		{
			this.hash = CRC32.GetHashUint(this.hash);
			this.CloseFileStream();
			if (this.onFinished != null)
			{
				this.onFinished();
			}
		}

		protected override void ReceiveContentLength(int contentLength)
		{
			this.length = contentLength;
		}

		protected override float GetProgress()
		{
			if (this.length == 0)
			{
				return 0f;
			}
			return (float)this.value / (float)this.length;
		}

		protected const int BUFFER_SIZE = 131072;

		public Action<string, Exception> onFailedWrite;

		public Action onFinished;

		protected FileStream fs;

		protected string path;

		protected int value;

		protected int length;

		protected uint hash;
	}
}
