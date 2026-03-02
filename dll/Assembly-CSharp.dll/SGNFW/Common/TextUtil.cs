using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SGNFW.Common
{
	// Token: 0x0200025D RID: 605
	public static class TextUtil
	{
		// Token: 0x060025D2 RID: 9682 RVA: 0x001A08E4 File Offset: 0x0019EAE4
		public static bool WriteText(string path, string text)
		{
			return TextUtil.WriteText(path, text, Encoding.UTF8);
		}

		// Token: 0x060025D3 RID: 9683 RVA: 0x001A08F4 File Offset: 0x0019EAF4
		public static bool WriteText(string path, string text, Encoding encoding)
		{
			try
			{
				using (StreamWriter streamWriter = new StreamWriter(path, false, encoding))
				{
					streamWriter.Write(text);
				}
			}
			catch
			{
				return false;
			}
			return true;
		}

		// Token: 0x060025D4 RID: 9684 RVA: 0x001A0944 File Offset: 0x0019EB44
		public static bool AppendText(string path, string text, Encoding encoding)
		{
			try
			{
				using (StreamWriter streamWriter = new StreamWriter(path, true, encoding))
				{
					streamWriter.Write(text);
				}
			}
			catch
			{
				return false;
			}
			return true;
		}

		// Token: 0x060025D5 RID: 9685 RVA: 0x001A0994 File Offset: 0x0019EB94
		public static string ReadText(string path, Encoding encoding)
		{
			string text = "";
			try
			{
				using (StreamReader streamReader = new StreamReader(path, encoding))
				{
					text = streamReader.ReadToEnd();
				}
			}
			catch
			{
				text = "";
			}
			return text;
		}

		// Token: 0x060025D6 RID: 9686 RVA: 0x001A09EC File Offset: 0x0019EBEC
		public static string[] ReadLines(string path, Encoding encoding)
		{
			List<string> list = new List<string>();
			try
			{
				using (StreamReader streamReader = new StreamReader(path, encoding))
				{
					while (streamReader.Peek() > 0)
					{
						list.Add(streamReader.ReadLine());
					}
				}
			}
			catch
			{
				list.Clear();
			}
			return list.ToArray();
		}
	}
}
