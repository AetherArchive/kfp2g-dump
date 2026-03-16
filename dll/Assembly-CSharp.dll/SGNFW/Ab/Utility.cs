using System;
using System.Collections.Generic;
using System.Linq;

namespace SGNFW.Ab
{
	public static class Utility
	{
		public static Utility.DownloadInfo GetDownloadInfo(IEnumerable<string> flies)
		{
			return Utility.GetDownloadInfo(flies.Select<string, Data>((string file) => Manager.GetData(file)));
		}

		public static Utility.DownloadInfo GetDownloadInfo(IEnumerable<Data> datas)
		{
			Utility.DownloadInfo downloadInfo = new Utility.DownloadInfo();
			HashSet<Data> hashSet = new HashSet<Data>();
			foreach (Data data in datas)
			{
				bool flag = false;
				if (data != null)
				{
					if (!hashSet.Contains(data))
					{
						hashSet.Add(data);
						Manager.CheckState(data);
						if (data.IsNeedDownload)
						{
							downloadInfo.totalDatas.Add(data);
							downloadInfo.totalSize += (long)data.size;
							flag = true;
						}
					}
					IEnumerable<Data> dependencyData = Manager.GetDependencyData(data);
					if (dependencyData != null)
					{
						foreach (Data data2 in dependencyData)
						{
							if (!hashSet.Contains(data2))
							{
								hashSet.Add(data2);
								Manager.CheckState(data2);
								if (data2.IsNeedDownload)
								{
									downloadInfo.totalDatas.Add(data2);
									downloadInfo.totalSize += (long)data2.size;
									flag = true;
								}
							}
						}
					}
					if (flag)
					{
						downloadInfo.datas.Add(data);
					}
				}
			}
			return downloadInfo;
		}

		public static long GetDownloadedFileSize(IEnumerable<Data> totalDatas)
		{
			if (totalDatas == null)
			{
				return 0L;
			}
			long num = 0L;
			foreach (Data data in totalDatas)
			{
				num += (long)data.DownloadedSize;
			}
			return num;
		}

		public class DownloadInfo
		{
			public long totalSize;

			public List<Data> datas = new List<Data>();

			public List<Data> totalDatas = new List<Data>();
		}
	}
}
