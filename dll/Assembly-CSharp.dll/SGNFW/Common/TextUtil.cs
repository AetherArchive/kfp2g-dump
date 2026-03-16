using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SGNFW.Common
{
	public static class TextUtil
	{
		public static bool WriteText(string path, string text)
		{
			return TextUtil.WriteText(path, text, Encoding.UTF8);
		}

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
