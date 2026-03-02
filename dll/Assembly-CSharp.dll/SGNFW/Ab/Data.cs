using System;
using UnityEngine;

namespace SGNFW.Ab
{
	// Token: 0x0200027F RID: 639
	public class Data
	{
		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x060026ED RID: 9965 RVA: 0x001A3F32 File Offset: 0x001A2132
		public virtual bool IsUnknown
		{
			get
			{
				return this.state == Data.State.Unknown;
			}
		}

		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x060026EE RID: 9966 RVA: 0x001A3F3D File Offset: 0x001A213D
		public virtual bool IsNeedDownload
		{
			get
			{
				return this.state == Data.State.NotExists;
			}
		}

		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x060026EF RID: 9967 RVA: 0x001A3F48 File Offset: 0x001A2148
		public virtual bool IsDownloading
		{
			get
			{
				return this.state == Data.State.Downloading;
			}
		}

		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x060026F0 RID: 9968 RVA: 0x001A3F53 File Offset: 0x001A2153
		public virtual bool IsExists
		{
			get
			{
				return this.state == Data.State.Exists;
			}
		}

		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x060026F1 RID: 9969 RVA: 0x001A3F5E File Offset: 0x001A215E
		public virtual bool IsLoading
		{
			get
			{
				return this.state == Data.State.Loading;
			}
		}

		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x060026F2 RID: 9970 RVA: 0x001A3F69 File Offset: 0x001A2169
		public virtual bool IsLoaded
		{
			get
			{
				return this.state == Data.State.Loaded;
			}
		}

		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x060026F3 RID: 9971 RVA: 0x001A3F74 File Offset: 0x001A2174
		public virtual bool IsAsset
		{
			get
			{
				return this.type == Define.EntryType.Asset || this.type == Define.EntryType.Pack;
			}
		}

		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x060026F4 RID: 9972 RVA: 0x001A3F89 File Offset: 0x001A2189
		public virtual string FullPath
		{
			get
			{
				return this.path + this.name;
			}
		}

		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x060026F5 RID: 9973 RVA: 0x001A3F9C File Offset: 0x001A219C
		public virtual string DownloadPath
		{
			get
			{
				return this.FullPath;
			}
		}

		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x060026F6 RID: 9974 RVA: 0x001A3FA4 File Offset: 0x001A21A4
		public virtual int DownloadedSize
		{
			get
			{
				switch (this.state)
				{
				default:
					return 0;
				case Data.State.Downloading:
					return (int)(this.downloadProgress * (float)this.size);
				case Data.State.Exists:
				case Data.State.Loading:
				case Data.State.Loaded:
					return this.size;
				}
			}
		}

		// Token: 0x060026F7 RID: 9975 RVA: 0x001A3FF0 File Offset: 0x001A21F0
		public virtual void SetType(string str)
		{
			this.type = (Define.EntryType)Enum.Parse(typeof(Define.EntryType), str);
		}

		// Token: 0x060026F8 RID: 9976 RVA: 0x001A4010 File Offset: 0x001A2210
		public virtual void SetSave(string str)
		{
			int num;
			if (int.TryParse(str, out num))
			{
				this.save = num != 0;
			}
		}

		// Token: 0x04001C7D RID: 7293
		public Action<Data> onCompleted;

		// Token: 0x04001C7E RID: 7294
		public Define.EntryType type;

		// Token: 0x04001C7F RID: 7295
		public string path = "assets/";

		// Token: 0x04001C80 RID: 7296
		public string name;

		// Token: 0x04001C81 RID: 7297
		public bool save;

		// Token: 0x04001C82 RID: 7298
		public string category;

		// Token: 0x04001C83 RID: 7299
		public string[] tags;

		// Token: 0x04001C84 RID: 7300
		public uint hash;

		// Token: 0x04001C85 RID: 7301
		public bool isHashCheck;

		// Token: 0x04001C86 RID: 7302
		public int[] dependencies;

		// Token: 0x04001C87 RID: 7303
		public AssetBundle asset;

		// Token: 0x04001C88 RID: 7304
		public byte[] bytes;

		// Token: 0x04001C89 RID: 7305
		public Data.State state = Data.State.Unknown;

		// Token: 0x04001C8A RID: 7306
		public int refcount;

		// Token: 0x04001C8B RID: 7307
		public int size;

		// Token: 0x04001C8C RID: 7308
		public bool isDownloadOnly;

		// Token: 0x04001C8D RID: 7309
		public bool retry;

		// Token: 0x04001C8E RID: 7310
		public bool async;

		// Token: 0x04001C8F RID: 7311
		public float downloadProgress;

		// Token: 0x020010A7 RID: 4263
		public enum State
		{
			// Token: 0x04005C62 RID: 23650
			Unknown = -1,
			// Token: 0x04005C63 RID: 23651
			NotExists,
			// Token: 0x04005C64 RID: 23652
			Downloading,
			// Token: 0x04005C65 RID: 23653
			Exists,
			// Token: 0x04005C66 RID: 23654
			Loading,
			// Token: 0x04005C67 RID: 23655
			Loaded
		}
	}
}
