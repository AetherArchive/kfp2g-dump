using System;

namespace SGNFW.Http
{
	// Token: 0x02000251 RID: 593
	[Serializable]
	public class ErrorCode
	{
		// Token: 0x0600255A RID: 9562 RVA: 0x0019F35D File Offset: 0x0019D55D
		public bool checkType(ActionTypeMask mask)
		{
			return (this.typ & (int)mask) != 0;
		}

		// Token: 0x04001BAB RID: 7083
		public string msg;

		// Token: 0x04001BAC RID: 7084
		public string tit;

		// Token: 0x04001BAD RID: 7085
		public int typ;

		// Token: 0x04001BAE RID: 7086
		public int id;
	}
}
