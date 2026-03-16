using System;
using SGNFW.Common.NativePlugin;

namespace SGNFW.Common.ResidentProcess
{
	public class Manager : Singleton<Manager>
	{
		private void Init_Init()
		{
			this.mode_.set(Manager.State.CustomSchemeAnalyze);
		}

		private void Resume_Init()
		{
			this.mode_.set(Manager.State.CustomSchemeAnalyze);
		}

		private void Wait_Proc()
		{
		}

		private void CustomSchemeAnalyze_Proc()
		{
			if (CustomScheme.AnalyzeCustomScheme())
			{
				this.mode_.set(Manager.State.Wait);
			}
		}

		protected override void OnSingletonAwake()
		{
			Verbose<Manager.Verbose>.Enabled = true;
			this.mode_ = Mode.create<Manager.State>(this);
			this.mode_.enabledChangeStateLog = true;
			this.mode_.set(Manager.State.Init);
		}

		protected override void OnSingletonDestroy()
		{
		}

		protected void Start()
		{
		}

		protected void Update()
		{
			this.mode_.proc();
		}

		protected void OnApplicationPause(bool pauseStatus)
		{
			if (!pauseStatus)
			{
				this.mode_.set(Manager.State.Resume);
			}
		}

		private Mode mode_;

		private enum State
		{
			None,
			Init,
			Wait,
			Resume,
			CustomSchemeAnalyze
		}

		public class Verbose : Verbose<Manager.Verbose>
		{
		}
	}
}
