using System;
using System.IO;
using UnityEngine;

namespace SGNFW.Common.SVNRevision
{
	public class ClientRevision
	{
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

		private static int no = -1;
	}
}
