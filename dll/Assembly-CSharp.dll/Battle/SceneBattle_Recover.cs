using System;
using System.Collections.Generic;

namespace Battle
{
	// Token: 0x02000221 RID: 545
	public class SceneBattle_Recover
	{
		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x06002302 RID: 8962 RVA: 0x001955D6 File Offset: 0x001937D6
		public long Mask
		{
			get
			{
				return this._mask;
			}
		}

		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x06002303 RID: 8963 RVA: 0x001955DE File Offset: 0x001937DE
		public List<CharaDef.ActionBuffType> Type
		{
			get
			{
				return this._typ;
			}
		}

		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x06002304 RID: 8964 RVA: 0x001955E6 File Offset: 0x001937E6
		public string Nam
		{
			get
			{
				return this._nam;
			}
		}

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x06002305 RID: 8965 RVA: 0x001955EE File Offset: 0x001937EE
		public int Prm
		{
			get
			{
				return this._prm;
			}
		}

		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x06002306 RID: 8966 RVA: 0x001955F6 File Offset: 0x001937F6
		public int Icn
		{
			get
			{
				return this._icn;
			}
		}

		// Token: 0x06002307 RID: 8967 RVA: 0x001955FE File Offset: 0x001937FE
		public SceneBattle_Recover(CharaDef.ActionAbnormalMask m, CharaDef.ActionBuffType t, string n, int p, int i)
		{
			this._mask = CharaDef.AbnormalMask(m);
			this._typ = new List<CharaDef.ActionBuffType> { t };
			this._nam = n;
			this._prm = p;
			this._icn = i;
		}

		// Token: 0x06002308 RID: 8968 RVA: 0x0019563B File Offset: 0x0019383B
		public SceneBattle_Recover(CharaDef.ActionAbnormalMask m, List<CharaDef.ActionBuffType> t, string n, int p, int i)
		{
			this._mask = CharaDef.AbnormalMask(m);
			this._typ = t;
			this._nam = n;
			this._prm = p;
			this._icn = i;
		}

		// Token: 0x06002309 RID: 8969 RVA: 0x0019566D File Offset: 0x0019386D
		public SceneBattle_Recover(CharaDef.ActionAbnormalMask2 m, CharaDef.ActionBuffType t, string n, int p, int i)
		{
			this._mask = CharaDef.AbnormalMask(m);
			this._typ = new List<CharaDef.ActionBuffType> { t };
			this._nam = n;
			this._prm = p;
			this._icn = i;
		}

		// Token: 0x0600230A RID: 8970 RVA: 0x001956AA File Offset: 0x001938AA
		public SceneBattle_Recover(CharaDef.ActionAbnormalMask2 m, List<CharaDef.ActionBuffType> t, string n, int p, int i)
		{
			this._mask = CharaDef.AbnormalMask(m);
			this._typ = t;
			this._nam = n;
			this._prm = p;
			this._icn = i;
		}

		// Token: 0x0600230B RID: 8971 RVA: 0x001956DC File Offset: 0x001938DC
		public SceneBattle_Recover(CharaDef.ActionBuffType t, string n, int i)
		{
			this._mask = 0L;
			this._typ = new List<CharaDef.ActionBuffType> { t };
			this._nam = n;
			this._prm = 0;
			this._icn = i;
		}

		// Token: 0x04001A30 RID: 6704
		private long _mask;

		// Token: 0x04001A31 RID: 6705
		private List<CharaDef.ActionBuffType> _typ;

		// Token: 0x04001A32 RID: 6706
		private string _nam;

		// Token: 0x04001A33 RID: 6707
		private int _prm;

		// Token: 0x04001A34 RID: 6708
		private int _icn;
	}
}
