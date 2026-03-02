using System;

namespace SGNFW.Ab
{
	// Token: 0x02000282 RID: 642
	public class Exception : Exception
	{
		// Token: 0x06002702 RID: 9986 RVA: 0x001A41A0 File Offset: 0x001A23A0
		public Exception(string msg, Exception.Code code)
			: base(msg)
		{
			this.code = code;
		}

		// Token: 0x06002703 RID: 9987 RVA: 0x001A41B0 File Offset: 0x001A23B0
		public override string ToString()
		{
			string text = base.GetType().ToString();
			string text2 = "[code:" + this.code.ToString() + "]";
			string message = this.Message;
			return string.Concat(new string[] { text, " ", text2, " ", message });
		}

		// Token: 0x04001C9D RID: 7325
		public Exception.Code code;

		// Token: 0x020010AB RID: 4267
		public enum Code
		{
			// Token: 0x04005C87 RID: 23687
			Unknown,
			// Token: 0x04005C88 RID: 23688
			FileIO,
			// Token: 0x04005C89 RID: 23689
			FileEOF,
			// Token: 0x04005C8A RID: 23690
			Timeout,
			// Token: 0x04005C8B RID: 23691
			Hash,
			// Token: 0x04005C8C RID: 23692
			Parse,
			// Token: 0x04005C8D RID: 23693
			Version
		}
	}
}
