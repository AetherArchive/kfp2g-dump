using System;

namespace SGNFW.Common
{
	// Token: 0x0200025A RID: 602
	public class Mode
	{
		// Token: 0x060025B2 RID: 9650 RVA: 0x001A02AC File Offset: 0x0019E4AC
		private Mode(object owner, Type enumType)
		{
			this.owner_ = owner;
			this.setFuncs(enumType);
			this.current_ = 0;
		}

		// Token: 0x060025B3 RID: 9651 RVA: 0x001A02D7 File Offset: 0x0019E4D7
		public static Mode create<ID>(object owner)
		{
			return new Mode(owner, typeof(ID));
		}

		// Token: 0x060025B4 RID: 9652 RVA: 0x001A02E9 File Offset: 0x0019E4E9
		public void init(Enum id)
		{
			this.set(id);
			while (this.previous_ != this.current_)
			{
				this.term_internal();
				this.init_internal();
			}
		}

		// Token: 0x060025B5 RID: 9653 RVA: 0x001A030E File Offset: 0x0019E50E
		public void proc()
		{
			while (this.previous_ != this.current_)
			{
				bool flag = this.enabledChangeStateLog;
				this.term_internal();
				this.init_internal();
			}
			this.proc_internal();
		}

		// Token: 0x060025B6 RID: 9654 RVA: 0x001A0339 File Offset: 0x0019E539
		public void term()
		{
			this.term_internal();
		}

		// Token: 0x060025B7 RID: 9655 RVA: 0x001A0341 File Offset: 0x0019E541
		public void set(Enum id)
		{
			this.current_ = (int)id;
		}

		// Token: 0x060025B8 RID: 9656 RVA: 0x001A034F File Offset: 0x0019E54F
		public int get()
		{
			return this.current_;
		}

		// Token: 0x060025B9 RID: 9657 RVA: 0x001A0357 File Offset: 0x0019E557
		public string getName(int id)
		{
			return Enum.GetName(this.enumType_, id);
		}

		// Token: 0x060025BA RID: 9658 RVA: 0x001A036A File Offset: 0x0019E56A
		public bool isState(Enum enumVal)
		{
			return this.current_ == Convert.ToInt32(enumVal);
		}

		// Token: 0x060025BB RID: 9659 RVA: 0x001A037C File Offset: 0x0019E57C
		private void setFuncs(Type enumType)
		{
			this.enumType_ = enumType;
			Array values = Enum.GetValues(enumType);
			this.initFuncs_ = new Mode.ModeFuncType[values.Length];
			this.procFuncs_ = new Mode.ModeFuncType[values.Length];
			this.termFuncs_ = new Mode.ModeFuncType[values.Length];
			foreach (object obj in values)
			{
				int num = (int)obj;
				string name = Enum.GetName(enumType, num);
				this.initFuncs_[num] = (Mode.ModeFuncType)Delegate.CreateDelegate(typeof(Mode.ModeFuncType), this.owner_, name + "_Init", false, false);
				this.procFuncs_[num] = (Mode.ModeFuncType)Delegate.CreateDelegate(typeof(Mode.ModeFuncType), this.owner_, name + "_Proc", false, false);
				this.termFuncs_[num] = (Mode.ModeFuncType)Delegate.CreateDelegate(typeof(Mode.ModeFuncType), this.owner_, name + "_Term", false, false);
			}
		}

		// Token: 0x060025BC RID: 9660 RVA: 0x001A04B0 File Offset: 0x0019E6B0
		private void init_internal()
		{
			if (-1 != this.current_)
			{
				this.previous_ = this.current_;
				if (this.initFuncs_[this.current_] != null)
				{
					this.initFuncs_[this.current_]();
				}
			}
		}

		// Token: 0x060025BD RID: 9661 RVA: 0x001A04E8 File Offset: 0x0019E6E8
		private void proc_internal()
		{
			if (this.procFuncs_[this.current_] != null)
			{
				this.procFuncs_[this.current_]();
			}
		}

		// Token: 0x060025BE RID: 9662 RVA: 0x001A050B File Offset: 0x0019E70B
		private void term_internal()
		{
			if (-1 != this.previous_)
			{
				if (this.termFuncs_[this.previous_] != null)
				{
					this.termFuncs_[this.previous_]();
				}
				this.previous_ = -1;
			}
		}

		// Token: 0x04001BC8 RID: 7112
		public bool enabledChangeStateLog;

		// Token: 0x04001BC9 RID: 7113
		private object owner_;

		// Token: 0x04001BCA RID: 7114
		private int previous_ = -1;

		// Token: 0x04001BCB RID: 7115
		private int current_ = -1;

		// Token: 0x04001BCC RID: 7116
		private Mode.ModeFuncType[] initFuncs_;

		// Token: 0x04001BCD RID: 7117
		private Mode.ModeFuncType[] procFuncs_;

		// Token: 0x04001BCE RID: 7118
		private Mode.ModeFuncType[] termFuncs_;

		// Token: 0x04001BCF RID: 7119
		private Type enumType_;

		// Token: 0x0200109C RID: 4252
		public class Verbose : Verbose<Mode.Verbose>
		{
		}

		// Token: 0x0200109D RID: 4253
		// (Invoke) Token: 0x06005364 RID: 21348
		private delegate void ModeFuncType();
	}
}
