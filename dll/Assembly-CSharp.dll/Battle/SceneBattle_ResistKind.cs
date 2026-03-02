using System;

namespace Battle
{
	// Token: 0x02000222 RID: 546
	public class SceneBattle_ResistKind
	{
		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x0600230C RID: 8972 RVA: 0x00195713 File Offset: 0x00193913
		public long typ
		{
			get
			{
				return this._typ;
			}
		}

		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x0600230D RID: 8973 RVA: 0x0019571B File Offset: 0x0019391B
		public int num
		{
			get
			{
				return this._num;
			}
		}

		// Token: 0x0600230E RID: 8974 RVA: 0x00195723 File Offset: 0x00193923
		public SceneBattle_ResistKind(long m, int j)
		{
			this._typ = m;
			this._num = j;
		}

		// Token: 0x04001A35 RID: 6709
		private long _typ;

		// Token: 0x04001A36 RID: 6710
		private int _num;
	}
}
