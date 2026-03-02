using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using DMMHelper;
using Ionic.Zlib;
using SGNFW.Common;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Thread;
using UnityEngine;

namespace SGNFW.Mst
{
	// Token: 0x0200031F RID: 799
	public class MstManager : Singleton<MstManager>
	{
		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x06002844 RID: 10308 RVA: 0x001A7877 File Offset: 0x001A5A77
		public static List<MstVersion> MstVersionList
		{
			get
			{
				return MstManager._mst_ver;
			}
		}

		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x06002845 RID: 10309 RVA: 0x001A787E File Offset: 0x001A5A7E
		// (set) Token: 0x06002846 RID: 10310 RVA: 0x001A7885 File Offset: 0x001A5A85
		public static bool IsMstDataCheck { get; set; }

		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x06002847 RID: 10311 RVA: 0x001A788D File Offset: 0x001A5A8D
		private string CacheBasePath
		{
			get
			{
				return Application.temporaryCachePath;
			}
		}

		// Token: 0x06002848 RID: 10312 RVA: 0x001A7894 File Offset: 0x001A5A94
		private void MstVersion()
		{
			MstVersionCmd mstVersionCmd = MstVersionCmd.Create(Singleton<DMMHelpManager>.Instance.VewerID);
			Singleton<DataManager>.Instance.ServerRequest(mstVersionCmd, new Action<Command>(this.MstVersionCb));
		}

		// Token: 0x06002849 RID: 10313 RVA: 0x001A78C8 File Offset: 0x001A5AC8
		public void MstVersionCb(Command cmd)
		{
			MstVersionResponse mstVersionResponse = cmd.response as MstVersionResponse;
			this.saveMstVerType(mstVersionResponse.mst_ver);
			this._state = MstManager.State.LOAD;
		}

		// Token: 0x0600284A RID: 10314 RVA: 0x001A78F4 File Offset: 0x001A5AF4
		private IEnumerator MstDataStart(MstVersion mstVer)
		{
			string text = "";
			string text2 = this.CacheBasePath + "/" + mstVer.type;
			string mst_file_path = text2 + ".d";
			if (PlayerPrefs.HasKey("parade_mst_version_" + mstVer.type))
			{
				text = PlayerPrefs.GetString("parade_mst_version_" + mstVer.type, "");
			}
			try
			{
				MstType mstType = (MstType)Enum.Parse(typeof(MstType), mstVer.type, true);
			}
			catch
			{
				this.UpdateMstVersionCheck(mstVer.type, mstVer.version);
				yield break;
			}
			if (string.IsNullOrEmpty(text) || text != mstVer.version || !File.Exists(mst_file_path))
			{
				MstDataCmd mstDataCmd = MstDataCmd.Create(mstVer.type, Singleton<DMMHelpManager>.Instance.VewerID);
				mstDataCmd.optionMap = new Dictionary<string, object>();
				mstDataCmd.optionMap.Add("MSTDATA", mstVer);
				Singleton<DataManager>.Instance.ServerRequest(mstDataCmd, new Action<Command>(this.MstDataCb));
			}
			else
			{
				AsyncResult<string> ar_mst_load = ThreadPool.Invoke<string>(delegate
				{
					string text4;
					try
					{
						byte[] array = null;
						using (FileStream fileStream = new FileStream(mst_file_path, FileMode.Open, FileAccess.Read))
						{
							array = new byte[fileStream.Length];
							fileStream.Read(array, 0, array.Length);
							fileStream.Close();
						}
						string text3 = BitConverter.ToString(MD5.Create().ComputeHash(array)).ToLower().Replace("-", "");
						if (text3 != mstVer.version)
						{
							Verbose<PrjLog>.LogError(string.Concat(new string[] { "[BaseData] local data error -> <color=#3080ff>", mstVer.type, "</color> : ", text3, " : ", mstVer.version }), null);
							text4 = "localfail";
						}
						else
						{
							text4 = this.decompressJsonBinary(array);
						}
					}
					catch (Exception)
					{
						text4 = "";
					}
					return text4;
				});
				while (!ar_mst_load.isCompleted)
				{
					yield return null;
				}
				if (!string.IsNullOrEmpty(ar_mst_load.result))
				{
					if (ar_mst_load.result.Equals("localfail"))
					{
						this.RemoveMstDataUnit(this.CacheBasePath, mstVer.type);
					}
					else
					{
						this.UpdateMstVersionCheck(mstVer.type, mstVer.version);
						this.cacheMst(mstVer.type, ar_mst_load.result);
					}
				}
				ar_mst_load = null;
			}
			yield break;
		}

		// Token: 0x0600284B RID: 10315 RVA: 0x001A790A File Offset: 0x001A5B0A
		private void MstDataCb(Command cmd)
		{
			Request request = cmd.request;
			base.StartCoroutine(this.saveMstStart(cmd.optionMap["MSTDATA"] as MstVersion, cmd.response));
		}

		// Token: 0x0600284C RID: 10316 RVA: 0x001A793B File Offset: 0x001A5B3B
		private IEnumerator saveMstStart(MstVersion mstVer, Response res)
		{
			MstDataResponse result = res as MstDataResponse;
			string text = this.CacheBasePath + "/" + mstVer.type;
			string mst_file_path = text + ".d";
			AsyncResult<string> ar_mst_save = ThreadPool.Invoke<string>(delegate
			{
				byte[] array = Convert.FromBase64String(result.data);
				try
				{
					using (FileStream fileStream = File.Create(mst_file_path))
					{
						fileStream.Write(array, 0, array.Length);
						fileStream.Close();
					}
				}
				catch (Exception)
				{
					return "";
				}
				return this.decompressJsonBinary(array);
			});
			while (!ar_mst_save.isCompleted)
			{
				yield return null;
			}
			if (!string.IsNullOrEmpty(ar_mst_save.result))
			{
				PlayerPrefs.SetString("parade_mst_version_" + mstVer.type, mstVer.version);
				this.UpdateMstVersionCheck(mstVer.type, mstVer.version);
				this.cacheMst(mstVer.type, ar_mst_save.result);
			}
			yield break;
		}

		// Token: 0x0600284D RID: 10317 RVA: 0x001A7958 File Offset: 0x001A5B58
		private string decompressJsonBinary(byte[] compressJsonBinary)
		{
			MemoryStream memoryStream = new MemoryStream();
			byte[] array = new byte[131072];
			string text;
			try
			{
				MemoryStream memoryStream2 = new MemoryStream(compressJsonBinary);
				using (GZipStream gzipStream = new GZipStream(memoryStream2, CompressionMode.Decompress, true))
				{
					for (;;)
					{
						int num = gzipStream.Read(array, 0, array.Length);
						if (num == 0)
						{
							break;
						}
						memoryStream.Write(array, 0, num);
					}
					gzipStream.Close();
				}
				memoryStream2.Close();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				byte[] array2 = new byte[memoryStream.Length];
				memoryStream.Read(array2, 0, array2.Length);
				memoryStream.Close();
				text = Encoding.UTF8.GetString(array2);
			}
			catch (Exception)
			{
				text = "";
			}
			return text;
		}

		// Token: 0x0600284E RID: 10318 RVA: 0x001A7A24 File Offset: 0x001A5C24
		private void saveMstVerType(List<MstVersion> mstVerList)
		{
			string text = "";
			string[] array = null;
			if (PlayerPrefs.HasKey("parade_mst_type_list"))
			{
				text = PlayerPrefs.GetString("parade_mst_type_list", "");
				array = text.Split(',', StringSplitOptions.None);
			}
			MstManager._mst_ver.Clear();
			foreach (MstVersion mstVersion in mstVerList)
			{
				MstVersion mstVersion2 = new MstVersion();
				mstVersion2.type = mstVersion.type;
				mstVersion2.version = mstVersion.version;
				MstManager._mst_ver.Add(mstVersion2);
				bool flag = true;
				if (array != null)
				{
					foreach (string text2 in array)
					{
						if (mstVersion.type == text2)
						{
							flag = false;
							break;
						}
					}
				}
				if (flag)
				{
					if (!string.IsNullOrEmpty(text))
					{
						text += ",";
					}
					text += mstVersion.type;
				}
			}
			PlayerPrefs.SetString("parade_mst_type_list", text);
			this.InitMstDataVersionCheck();
		}

		// Token: 0x0600284F RID: 10319 RVA: 0x001A7B44 File Offset: 0x001A5D44
		public void RemoveMstDataCaches()
		{
			if (PlayerPrefs.HasKey("parade_mst_type_list"))
			{
				foreach (string text in PlayerPrefs.GetString("parade_mst_type_list", "").Split(',', StringSplitOptions.None))
				{
					this.RemoveMstDataUnit(this.CacheBasePath, text);
				}
				PlayerPrefs.DeleteKey("parade_mst_type_list");
			}
		}

		// Token: 0x06002850 RID: 10320 RVA: 0x001A7BA0 File Offset: 0x001A5DA0
		private void RemoveMstDataUnit(string cachePath, string basedata_type)
		{
			string text = cachePath + "/" + basedata_type + ".d";
			if (PlayerPrefs.HasKey("parade_mst_version_" + basedata_type))
			{
				PlayerPrefs.DeleteKey("parade_mst_version_" + basedata_type);
			}
			if (File.Exists(text))
			{
				File.Delete(text);
			}
		}

		// Token: 0x06002851 RID: 10321 RVA: 0x001A7BF4 File Offset: 0x001A5DF4
		private void InitMstDataVersionCheck()
		{
			MstManager._mst_ver.ForEach(delegate(MstVersion b)
			{
				if (!MstManager._mst_version_chek.ContainsKey(b.type))
				{
					MstManager._mst_version_chek.Add(b.type, null);
					return;
				}
				MstManager._mst_version_chek[b.type] = null;
			});
			MstManager.IsMstDataCheck = false;
		}

		// Token: 0x06002852 RID: 10322 RVA: 0x001A7C25 File Offset: 0x001A5E25
		private void UpdateMstVersionCheck(string type, string version)
		{
			if (!MstManager._mst_version_chek.ContainsKey(type))
			{
				MstManager._mst_version_chek.Add(type, version);
			}
			else
			{
				MstManager._mst_version_chek[type] = version;
			}
			MstManager.CheckMstVersion();
		}

		// Token: 0x06002853 RID: 10323 RVA: 0x001A7C54 File Offset: 0x001A5E54
		private static bool CheckMstVersion()
		{
			bool flag = true;
			foreach (MstVersion mstVersion in MstManager._mst_ver)
			{
				if (!MstManager._mst_version_chek.ContainsKey(mstVersion.type))
				{
					flag = false;
					break;
				}
				if (MstManager._mst_version_chek[mstVersion.type] == null)
				{
					flag = false;
					break;
				}
				if (!MstManager._mst_version_chek[mstVersion.type].Equals(mstVersion.version))
				{
					flag = false;
					break;
				}
			}
			MstManager.IsMstDataCheck = flag;
			return flag;
		}

		// Token: 0x06002854 RID: 10324 RVA: 0x001A7CF8 File Offset: 0x001A5EF8
		private void cacheMst(string strType, string json)
		{
			if (json == null)
			{
				return;
			}
			try
			{
				MstType mstType = (MstType)Enum.Parse(typeof(MstType), strType, true);
				object obj = MstClass.cnv(mstType, json);
				this._mst_map[mstType] = obj;
			}
			catch
			{
			}
		}

		// Token: 0x06002855 RID: 10325 RVA: 0x001A7D4C File Offset: 0x001A5F4C
		public bool Refresh()
		{
			if (this._state == MstManager.State.INIT || this._state == MstManager.State.FINISH)
			{
				this._state = MstManager.State.VERSION;
				return true;
			}
			base.StopAllCoroutines();
			this._state = MstManager.State.VERSION;
			return true;
		}

		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x06002856 RID: 10326 RVA: 0x001A7D76 File Offset: 0x001A5F76
		public bool IsLoadFinish
		{
			get
			{
				return this._state == MstManager.State.FINISH;
			}
		}

		// Token: 0x06002857 RID: 10327 RVA: 0x001A7D84 File Offset: 0x001A5F84
		public T GetMst<T>(MstType type)
		{
			if (!this._mst_map.ContainsKey(type))
			{
				Verbose<PrjLog>.LogError("Error : MstManager.GetMst() Undefined error:" + type.ToString(), null);
				return default(T);
			}
			return (T)((object)this._mst_map[type]);
		}

		// Token: 0x06002858 RID: 10328 RVA: 0x001A7DD8 File Offset: 0x001A5FD8
		public void GetMstCount(out int num, out int cnt)
		{
			num = MstManager._mst_version_chek.Count;
			int num2;
			if (this._state >= MstManager.State.LOAD)
			{
				num2 = new List<string>(MstManager._mst_version_chek.Values).FindAll((string itm) => !string.IsNullOrEmpty(itm)).Count;
			}
			else
			{
				num2 = 0;
			}
			cnt = num2;
		}

		// Token: 0x06002859 RID: 10329 RVA: 0x001A7E37 File Offset: 0x001A6037
		protected override void OnSingletonAwake()
		{
		}

		// Token: 0x0600285A RID: 10330 RVA: 0x001A7E3C File Offset: 0x001A603C
		private void Update()
		{
			switch (this._state)
			{
			case MstManager.State.INIT:
			case MstManager.State.VERSION_WAIT:
			case MstManager.State.FINISH:
				break;
			case MstManager.State.VERSION:
				this.MstVersion();
				this._state = MstManager.State.VERSION_WAIT;
				this._loadCount = 0;
				return;
			case MstManager.State.LOAD:
			{
				for (int i = 0; i < 5; i++)
				{
					if (this._loadCount >= MstManager._mst_ver.Count)
					{
						this._state = MstManager.State.LOAD_WAIT;
						return;
					}
					base.StartCoroutine(this.MstDataStart(MstManager._mst_ver[this._loadCount]));
					this._loadCount++;
				}
				return;
			}
			case MstManager.State.LOAD_WAIT:
				if (MstManager.IsMstDataCheck)
				{
					string[] names = Enum.GetNames(typeof(MstType));
					for (int j = 0; j < names.Length; j++)
					{
						string mt = names[j];
						MstManager._mst_ver.Find((MstVersion itm) => itm.type == mt);
					}
					this._state = MstManager.State.FINISH;
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x0600285B RID: 10331 RVA: 0x001A7F2D File Offset: 0x001A612D
		private void Start()
		{
			this._state = MstManager.State.INIT;
		}

		// Token: 0x04002321 RID: 8993
		private const int MSTDATA_DECOMPRESS_READ_BUFFER_SIZE = 131072;

		// Token: 0x04002322 RID: 8994
		private const string MST_PREFS_PREFIX = "parade_mst_version_";

		// Token: 0x04002323 RID: 8995
		private const string MST_PREFS_TYPE_LIST = "parade_mst_type_list";

		// Token: 0x04002324 RID: 8996
		private MstManager.State _state;

		// Token: 0x04002325 RID: 8997
		private int _loadCount;

		// Token: 0x04002326 RID: 8998
		private static List<MstVersion> _mst_ver = new List<MstVersion>();

		// Token: 0x04002327 RID: 8999
		private static Dictionary<string, string> _mst_version_chek = new Dictionary<string, string>();

		// Token: 0x04002328 RID: 9000
		private Dictionary<MstType, object> _mst_map = new Dictionary<MstType, object>();

		// Token: 0x020010CA RID: 4298
		private enum State
		{
			// Token: 0x04005D07 RID: 23815
			INIT,
			// Token: 0x04005D08 RID: 23816
			VERSION,
			// Token: 0x04005D09 RID: 23817
			VERSION_WAIT,
			// Token: 0x04005D0A RID: 23818
			LOAD,
			// Token: 0x04005D0B RID: 23819
			LOAD_WAIT,
			// Token: 0x04005D0C RID: 23820
			FINISH
		}
	}
}
