using System;
using System.Collections.Generic;

namespace Battle
{
	// Token: 0x0200021E RID: 542
	public class SceneBattle_PlayerSkillRecover
	{
		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x060022F2 RID: 8946 RVA: 0x00194F23 File Offset: 0x00193123
		public SceneBattle_Friends Ply
		{
			get
			{
				return this._ply;
			}
		}

		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x060022F3 RID: 8947 RVA: 0x00194F2B File Offset: 0x0019312B
		public List<SceneBattle_Buff> NewBuff
		{
			get
			{
				return this._newBuff;
			}
		}

		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x060022F4 RID: 8948 RVA: 0x00194F33 File Offset: 0x00193133
		public List<SceneBattle_Buff> Buff
		{
			get
			{
				return this._buff;
			}
		}

		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x060022F5 RID: 8949 RVA: 0x00194F3B File Offset: 0x0019313B
		public List<int> Recover
		{
			get
			{
				return this._recover;
			}
		}

		// Token: 0x060022F6 RID: 8950 RVA: 0x00194F43 File Offset: 0x00193143
		public void SetFriends(SceneBattle_Friends ply)
		{
			this._ply = ply;
		}

		// Token: 0x04001A26 RID: 6694
		private SceneBattle_Friends _ply;

		// Token: 0x04001A27 RID: 6695
		private List<SceneBattle_Buff> _newBuff = new List<SceneBattle_Buff>();

		// Token: 0x04001A28 RID: 6696
		private List<SceneBattle_Buff> _buff = new List<SceneBattle_Buff>();

		// Token: 0x04001A29 RID: 6697
		private List<int> _recover = new List<int>();
	}
}
