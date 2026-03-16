using System;
using System.IO;
using UnityEngine;

namespace SGNFW.Common.Git
{
	public class ClientRevision
	{
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

		private static int no = -1;
	}
}
