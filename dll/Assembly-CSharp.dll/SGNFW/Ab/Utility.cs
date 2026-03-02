using System;
using System.Collections.Generic;
using System.Linq;

namespace SGNFW.Ab
{
	// Token: 0x02000289 RID: 649
	public static class Utility
	{
		// Token: 0x060027AB RID: 10155 RVA: 0x001A68E9 File Offset: 0x001A4AE9
		public static Utility.DownloadInfo GetDownloadInfo(IEnumerable<string> flies)
		{
			return Utility.GetDownloadInfo(flies.Select<string, Data>((string file) => Manager.GetData(file)));
		}

		// Token: 0x060027AC RID: 10156 RVA: 0x001A6918 File Offset: 0x001A4B18
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

		// Token: 0x060027AD RID: 10157 RVA: 0x001A6A54 File Offset: 0x001A4C54
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

		// Token: 0x020010C7 RID: 4295
		public class DownloadInfo
		{
			// Token: 0x04005CFB RID: 23803
			public long totalSize;

			// Token: 0x04005CFC RID: 23804
			public List<Data> datas = new List<Data>();

			// Token: 0x04005CFD RID: 23805
			public List<Data> totalDatas = new List<Data>();
		}
	}
}
