using System;

namespace SGNFW.Common
{
	public class Mode
	{
		private Mode(object owner, Type enumType)
		{
			this.owner_ = owner;
			this.setFuncs(enumType);
			this.current_ = 0;
		}

		public static Mode create<ID>(object owner)
		{
			return new Mode(owner, typeof(ID));
		}

		public void init(Enum id)
		{
			this.set(id);
			while (this.previous_ != this.current_)
			{
				this.term_internal();
				this.init_internal();
			}
		}

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

		public void term()
		{
			this.term_internal();
		}

		public void set(Enum id)
		{
			this.current_ = (int)id;
		}

		public int get()
		{
			return this.current_;
		}

		public string getName(int id)
		{
			return Enum.GetName(this.enumType_, id);
		}

		public bool isState(Enum enumVal)
		{
			return this.current_ == Convert.ToInt32(enumVal);
		}

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

		private void proc_internal()
		{
			if (this.procFuncs_[this.current_] != null)
			{
				this.procFuncs_[this.current_]();
			}
		}

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

		public bool enabledChangeStateLog;

		private object owner_;

		private int previous_ = -1;

		private int current_ = -1;

		private Mode.ModeFuncType[] initFuncs_;

		private Mode.ModeFuncType[] procFuncs_;

		private Mode.ModeFuncType[] termFuncs_;

		private Type enumType_;

		public class Verbose : Verbose<Mode.Verbose>
		{
		}

		private delegate void ModeFuncType();
	}
}
