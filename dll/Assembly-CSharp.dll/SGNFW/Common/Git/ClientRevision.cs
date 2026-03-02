using System;
using System.IO;
using UnityEngine;

namespace SGNFW.Common.Git
{
	// Token: 0x0200027A RID: 634
	public class ClientRevision
	{
		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x060026C2 RID: 9922 RVA: 0x001A3994 File Offset: 0x001A1B94
		public static int No
		{
			get
			{
				if (ClientRevision.no == -1)
				{
					string text = Application.dataPath.Replace("/Assets", "/Temp/gitinfo.txt");
					if (File.Exists(text))
					{
						int.TryParse(File.ReadAllText(text).Replace("\n", "").Replace("\r\n", "")
							.Replace(" ", ""), out ClientRevision.no);
					}
				}
				return ClientRevision.no;
			}
		}

		// Token: 0x04001C70 RID: 7280
		private static int no = -1;
	}
}
