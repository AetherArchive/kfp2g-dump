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
	public class MstManager : Singleton<MstManager>
	{
		public static List<MstVersion> MstVersionList
		{
			get
			{
				return MstManager._mst_ver;
			}
		}

		public static bool IsMstDataCheck { get; set; }

		private string CacheBasePath
		{
			get
			{
				return Application.temporaryCachePath;
			}
		}

		private void MstVersion()
		{
			MstVersionCmd mstVersionCmd = MstVersionCmd.Create(Singleton<DMMHelpManager>.Instance.VewerID);
			Singleton<DataManager>.Instance.ServerRequest(mstVersionCmd, new Action<Command>(this.MstVersionCb));
		}

		public void MstVersionCb(Command cmd)
		{
			MstVersionResponse mstVersionResponse = cmd.response as MstVersionResponse;
			this.saveMstVerType(mstVersionResponse.mst_ver);
			this._state = MstManager.State.LOAD;
		}

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

		private void MstDataCb(Command cmd)
		{
			Request request = cmd.request;
			base.StartCoroutine(this.saveMstStart(cmd.optionMap["MSTDATA"] as MstVersion, cmd.response));
		}

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

		public bool IsLoadFinish
		{
			get
			{
				return this._state == MstManager.State.FINISH;
			}
		}

		public T GetMst<T>(MstType type)
		{
			if (!this._mst_map.ContainsKey(type))
			{
				Verbose<PrjLog>.LogError("Error : MstManager.GetMst() Undefined error:" + type.ToString(), null);
				return default(T);
			}
			return (T)((object)this._mst_map[type]);
		}

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

		protected override void OnSingletonAwake()
		{
		}

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

		private void Start()
		{
			this._state = MstManager.State.INIT;
		}

		private const int MSTDATA_DECOMPRESS_READ_BUFFER_SIZE = 131072;

		private const string MST_PREFS_PREFIX = "parade_mst_version_";

		private const string MST_PREFS_TYPE_LIST = "parade_mst_type_list";

		private MstManager.State _state;

		private int _loadCount;

		private static List<MstVersion> _mst_ver = new List<MstVersion>();

		private static Dictionary<string, string> _mst_version_chek = new Dictionary<string, string>();

		private Dictionary<MstType, object> _mst_map = new Dictionary<MstType, object>();

		private enum State
		{
			INIT,
			VERSION,
			VERSION_WAIT,
			LOAD,
			LOAD_WAIT,
			FINISH
		}
	}
}
