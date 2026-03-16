using System;
using UnityEngine;

namespace SGNFW.Ab
{
	public class Data
	{
		public virtual bool IsUnknown
		{
			get
			{
				return this.state == Data.State.Unknown;
			}
		}

		public virtual bool IsNeedDownload
		{
			get
			{
				return this.state == Data.State.NotExists;
			}
		}

		public virtual bool IsDownloading
		{
			get
			{
				return this.state == Data.State.Downloading;
			}
		}

		public virtual bool IsExists
		{
			get
			{
				return this.state == Data.State.Exists;
			}
		}

		public virtual bool IsLoading
		{
			get
			{
				return this.state == Data.State.Loading;
			}
		}

		public virtual bool IsLoaded
		{
			get
			{
				return this.state == Data.State.Loaded;
			}
		}

		public virtual bool IsAsset
		{
			get
			{
				return this.type == Define.EntryType.Asset || this.type == Define.EntryType.Pack;
			}
		}

		public virtual string FullPath
		{
			get
			{
				return this.path + this.name;
			}
		}

		public virtual string DownloadPath
		{
			get
			{
				return this.FullPath;
			}
		}

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

		public virtual void SetType(string str)
		{
			this.type = (Define.EntryType)Enum.Parse(typeof(Define.EntryType), str);
		}

		public virtual void SetSave(string str)
		{
			int num;
			if (int.TryParse(str, out num))
			{
				this.save = num != 0;
			}
		}

		public Action<Data> onCompleted;

		public Define.EntryType type;

		public string path = "assets/";

		public string name;

		public bool save;

		public string category;

		public string[] tags;

		public uint hash;

		public bool isHashCheck;

		public int[] dependencies;

		public AssetBundle asset;

		public byte[] bytes;

		public Data.State state = Data.State.Unknown;

		public int refcount;

		public int size;

		public bool isDownloadOnly;

		public bool retry;

		public bool async;

		public float downloadProgress;

		public enum State
		{
			Unknown = -1,
			NotExists,
			Downloading,
			Exists,
			Loading,
			Loaded
		}
	}
}
