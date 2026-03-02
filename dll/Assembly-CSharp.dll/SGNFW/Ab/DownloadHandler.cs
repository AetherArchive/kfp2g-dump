using System;
using System.IO;
using UnityEngine.Networking;

namespace SGNFW.Ab
{
	// Token: 0x02000281 RID: 641
	internal class DownloadHandler : DownloadHandlerScript
	{
		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x060026FB RID: 9979 RVA: 0x001A4053 File Offset: 0x001A2253
		public uint Hash
		{
			get
			{
				return this.hash;
			}
		}

		// Token: 0x060026FC RID: 9980 RVA: 0x001A405B File Offset: 0x001A225B
		public void CloseFileStream()
		{
			if (this.fs != null)
			{
				this.fs.Dispose();
				this.fs = null;
			}
		}

		// Token: 0x060026FD RID: 9981 RVA: 0x001A4077 File Offset: 0x001A2277
		public DownloadHandler(string path)
			: base(new byte[131072])
		{
			this.path = path;
			this.hash = uint.MaxValue;
		}

		// Token: 0x060026FE RID: 9982 RVA: 0x001A4098 File Offset: 0x001A2298
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

		// Token: 0x060026FF RID: 9983 RVA: 0x001A414C File Offset: 0x001A234C
		protected override void CompleteContent()
		{
			this.hash = CRC32.GetHashUint(this.hash);
			this.CloseFileStream();
			if (this.onFinished != null)
			{
				this.onFinished();
			}
		}

		// Token: 0x06002700 RID: 9984 RVA: 0x001A4178 File Offset: 0x001A2378
		protected override void ReceiveContentLength(int contentLength)
		{
			this.length = contentLength;
		}

		// Token: 0x06002701 RID: 9985 RVA: 0x001A4181 File Offset: 0x001A2381
		protected override float GetProgress()
		{
			if (this.length == 0)
			{
				return 0f;
			}
			return (float)this.value / (float)this.length;
		}

		// Token: 0x04001C95 RID: 7317
		protected const int BUFFER_SIZE = 131072;

		// Token: 0x04001C96 RID: 7318
		public Action<string, Exception> onFailedWrite;

		// Token: 0x04001C97 RID: 7319
		public Action onFinished;

		// Token: 0x04001C98 RID: 7320
		protected FileStream fs;

		// Token: 0x04001C99 RID: 7321
		protected string path;

		// Token: 0x04001C9A RID: 7322
		protected int value;

		// Token: 0x04001C9B RID: 7323
		protected int length;

		// Token: 0x04001C9C RID: 7324
		protected uint hash;
	}
}
