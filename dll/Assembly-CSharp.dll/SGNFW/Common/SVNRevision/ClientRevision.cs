using System;
using System.IO;
using UnityEngine;

namespace SGNFW.Common.SVNRevision
{
	// Token: 0x02000266 RID: 614
	public class ClientRevision
	{
		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x06002613 RID: 9747 RVA: 0x001A1148 File Offset: 0x0019F348
		public static int No
		{
			get
			{
				string text = Application.dataPath.Replace("/Assets", "/Temp/svninfo.txt");
				if (File.Exists(text))
				{
					int.TryParse(File.ReadAllText(text).Replace("\n", "").Replace("\r\n", "")
						.Replace(" ", ""), out ClientRevision.no);
				}
				return ClientRevision.no;
			}
		}

		// Token: 0x04001BE3 RID: 7139
		private static int no = -1;
	}
}
