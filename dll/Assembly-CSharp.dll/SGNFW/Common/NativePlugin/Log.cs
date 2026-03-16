using System;
using System.IO;
using System.Text;

namespace SGNFW.Common.NativePlugin
{
	public class Log
	{
		public void Dispose()
		{
			if (Log.streamWriter_ != null)
			{
				Log.streamWriter_.Close();
				Log.streamWriter_ = null;
			}
		}

		public static void Write(string log)
		{
			if (Log.streamWriter_ == null)
			{
				return;
			}
			Log.streamWriter_.WriteLine(log);
			Log.streamWriter_.Flush();
		}

		public static void CreateFile(string filename)
		{
			if (Log.streamWriter_ != null)
			{
				return;
			}
			Log.streamWriter_ = new StreamWriter(filename, false, Encoding.UTF8);
		}

		private static StreamWriter streamWriter_;
	}
}
