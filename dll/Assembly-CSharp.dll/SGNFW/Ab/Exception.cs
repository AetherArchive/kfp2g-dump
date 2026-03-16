using System;

namespace SGNFW.Ab
{
	public class Exception : Exception
	{
		public Exception(string msg, Exception.Code code)
			: base(msg)
		{
			this.code = code;
		}

		public override string ToString()
		{
			string text = base.GetType().ToString();
			string text2 = "[code:" + this.code.ToString() + "]";
			string message = this.Message;
			return string.Concat(new string[] { text, " ", text2, " ", message });
		}

		public Exception.Code code;

		public enum Code
		{
			Unknown,
			FileIO,
			FileEOF,
			Timeout,
			Hash,
			Parse,
			Version
		}
	}
}
