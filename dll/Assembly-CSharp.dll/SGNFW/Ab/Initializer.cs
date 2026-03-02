using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace SGNFW.Ab
{
	// Token: 0x02000283 RID: 643
	public class Initializer
	{
		// Token: 0x06002704 RID: 9988 RVA: 0x001A4218 File Offset: 0x001A2418
		public virtual void Load(bool redownload = false)
		{
			Action _finished = null;
			Action<string, Data, Exception> _error = null;
			this.loader = Manager.GetLoader();
			_error = delegate(string path, Data data, Exception e)
			{
				this.loader.Exit();
				if (this.onError != null)
				{
					this.onError(path, e);
				}
			};
			_finished = delegate
			{
				Loader loader3 = this.loader;
				loader3.onFinished = (Action)Delegate.Remove(loader3.onFinished, _finished);
				Loader loader4 = this.loader;
				loader4.onError = (Action<string, Data, Exception>)Delegate.Remove(loader4.onError, _error);
				if (this.onCompleted != null)
				{
					this.onCompleted();
				}
			};
			Loader loader = this.loader;
			loader.onError = (Action<string, Data, Exception>)Delegate.Combine(loader.onError, _error);
			Loader loader2 = this.loader;
			loader2.onFinished = (Action)Delegate.Combine(loader2.onFinished, _finished);
			this.Request(redownload);
		}

		// Token: 0x06002705 RID: 9989 RVA: 0x001A42B8 File Offset: 0x001A24B8
		protected virtual void Request(bool redownload = false)
		{
			Action<Data> action = delegate(Data _data)
			{
				this.Parse(_data, redownload);
				Manager.Unload(_data, false);
			};
			Data data = new Data();
			data.type = Define.EntryType.Raw;
			data.name = "ab_list.txt";
			data.path = "";
			data.size = int.MaxValue;
			data.save = true;
			if (redownload)
			{
				data.state = Data.State.NotExists;
			}
			this.loader.Request(data, action, false, false);
		}

		// Token: 0x06002706 RID: 9990 RVA: 0x001A4338 File Offset: 0x001A2538
		protected virtual void Parse(Data data, bool redownload)
		{
			if (data == null || data.bytes == null)
			{
				return;
			}
			byte[] buffer = null;
			buffer = data.bytes;
			if (!delegate
			{
				byte[] array = new byte[] { 91, 69, 79, 70, 93, 10 };
				for (int i = 0; i < array.Length; i++)
				{
					int num = buffer.Length - array.Length + i;
					if (num < 0 || num >= buffer.Length)
					{
						return false;
					}
					if (buffer[num] != array[i])
					{
						return false;
					}
				}
				return true;
			}())
			{
				Exception ex = new Exception("", Exception.Code.Parse);
				if (this.onError != null)
				{
					this.onError(data.name, ex);
				}
				return;
			}
			using (MemoryStream memoryStream = new MemoryStream(buffer))
			{
				using (StreamReader streamReader = new StreamReader(memoryStream))
				{
					if (string.Compare(streamReader.ReadLine(), Manager.DataVersion) != 0)
					{
						if (!redownload)
						{
							this.Request(true);
						}
						else
						{
							Exception ex2 = new Exception("", Exception.Code.Version);
							if (this.onError != null)
							{
								this.onError(data.name, ex2);
							}
						}
					}
					else
					{
						Manager.Pack packData = Manager.packData;
						string text;
						while ((text = streamReader.ReadLine()) != null)
						{
							if (text.StartsWith("[EOF]"))
							{
								break;
							}
							if (!string.IsNullOrEmpty(text))
							{
								this.ParseLine(text, packData);
							}
						}
					}
				}
			}
		}

		// Token: 0x06002707 RID: 9991 RVA: 0x001A446C File Offset: 0x001A266C
		protected virtual Data ParseLine(string line)
		{
			string[] array = line.Split('\t', StringSplitOptions.None);
			Data data = new Data();
			if (array.Length != 0)
			{
				data.name = array[0];
			}
			if (array.Length > 1)
			{
				data.SetSave(array[1]);
			}
			if (array.Length > 2)
			{
				data.SetType(array[2]);
			}
			if (array.Length > 3)
			{
				data.category = array[3];
			}
			if (array.Length > 4 && array[4].Length > 0)
			{
				data.tags = array[4].Split(',', StringSplitOptions.None);
			}
			if (array.Length > 5)
			{
				data.size = int.Parse(array[5]);
			}
			if (array.Length > 6 && uint.TryParse(array[6], NumberStyles.HexNumber, null, out data.hash))
			{
				data.isHashCheck = true;
			}
			if (array.Length > 7 && array[7].Length > 0)
			{
				string[] array2 = array[7].Split(',', StringSplitOptions.None);
				int[] array3 = new int[array2.Length];
				for (int i = 0; i < array3.Length; i++)
				{
					int.TryParse(array2[i], out array3[i]);
				}
				data.dependencies = array3;
			}
			return data;
		}

		// Token: 0x06002708 RID: 9992 RVA: 0x001A4570 File Offset: 0x001A2770
		protected virtual Data ParseLine(string line, Manager.Pack pack)
		{
			Data data = this.ParseLine(line);
			pack.dataList.Add(data);
			pack.dataDic[data.name] = data;
			if (!string.IsNullOrEmpty(data.category))
			{
				if (!pack.categoryDic.ContainsKey(data.category))
				{
					pack.categoryDic[data.category] = new List<Data>();
				}
				pack.categoryDic[data.category].Add(data);
			}
			if (data.tags != null)
			{
				for (int i = 0; i < data.tags.Length; i++)
				{
					if (!string.IsNullOrEmpty(data.tags[i]))
					{
						if (!pack.tagDic.ContainsKey(data.tags[i]))
						{
							pack.tagDic[data.tags[i]] = new List<Data>();
						}
						pack.tagDic[data.tags[i]].Add(data);
					}
				}
			}
			return data;
		}

		// Token: 0x04001C9E RID: 7326
		public Action onCompleted;

		// Token: 0x04001C9F RID: 7327
		public Action<string, Exception> onError;

		// Token: 0x04001CA0 RID: 7328
		protected Loader loader;
	}
}
