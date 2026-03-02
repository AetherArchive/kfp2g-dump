using System;
using System.IO;
using System.Text;

namespace SGNFW.Common.NativePlugin
{
	// Token: 0x0200026E RID: 622
	public class Log
	{
		// Token: 0x06002636 RID: 9782 RVA: 0x001A140A File Offset: 0x0019F60A
		public void Dispose()
		{
			if (Log.streamWriter_ != null)
			{
				Log.streamWriter_.Close();
				Log.streamWriter_ = null;
			}
		}

		// Token: 0x06002637 RID: 9783 RVA: 0x001A1423 File Offset: 0x0019F623
		public static void Write(string log)
		{
			if (Log.streamWriter_ == null)
			{
				return;
			}
			Log.streamWriter_.WriteLine(log);
			Log.streamWriter_.Flush();
		}

		// Token: 0x06002638 RID: 9784 RVA: 0x001A1442 File Offset: 0x0019F642
		public static void CreateFile(string filename)
		{
			if (Log.streamWriter_ != null)
			{
				return;
			}
			Log.streamWriter_ = new StreamWriter(filename, false, Encoding.UTF8);
		}

		// Token: 0x04001C46 RID: 7238
		private static StreamWriter streamWriter_;
	}
}
