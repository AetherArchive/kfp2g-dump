using System;

namespace SGNFW.Http
{
	[Serializable]
	public class ErrorCode
	{
		public bool checkType(ActionTypeMask mask)
		{
			return (this.typ & (int)mask) != 0;
		}

		public string msg;

		public string tit;

		public int typ;

		public int id;
	}
}
