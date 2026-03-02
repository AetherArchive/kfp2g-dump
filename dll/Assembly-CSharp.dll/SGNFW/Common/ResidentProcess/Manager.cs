using System;
using SGNFW.Common.NativePlugin;

namespace SGNFW.Common.ResidentProcess
{
	// Token: 0x02000269 RID: 617
	public class Manager : Singleton<Manager>
	{
		// Token: 0x06002617 RID: 9751 RVA: 0x001A11CD File Offset: 0x0019F3CD
		private void Init_Init()
		{
			this.mode_.set(Manager.State.CustomSchemeAnalyze);
		}

		// Token: 0x06002618 RID: 9752 RVA: 0x001A11E0 File Offset: 0x0019F3E0
		private void Resume_Init()
		{
			this.mode_.set(Manager.State.CustomSchemeAnalyze);
		}

		// Token: 0x06002619 RID: 9753 RVA: 0x001A11F3 File Offset: 0x0019F3F3
		private void Wait_Proc()
		{
		}

		// Token: 0x0600261A RID: 9754 RVA: 0x001A11F5 File Offset: 0x0019F3F5
		private void CustomSchemeAnalyze_Proc()
		{
			if (CustomScheme.AnalyzeCustomScheme())
			{
				this.mode_.set(Manager.State.Wait);
			}
		}

		// Token: 0x0600261B RID: 9755 RVA: 0x001A120F File Offset: 0x0019F40F
		protected override void OnSingletonAwake()
		{
			Verbose<Manager.Verbose>.Enabled = true;
			this.mode_ = Mode.create<Manager.State>(this);
			this.mode_.enabledChangeStateLog = true;
			this.mode_.set(Manager.State.Init);
		}

		// Token: 0x0600261C RID: 9756 RVA: 0x001A1240 File Offset: 0x0019F440
		protected override void OnSingletonDestroy()
		{
		}

		// Token: 0x0600261D RID: 9757 RVA: 0x001A1242 File Offset: 0x0019F442
		protected void Start()
		{
		}

		// Token: 0x0600261E RID: 9758 RVA: 0x001A1244 File Offset: 0x0019F444
		protected void Update()
		{
			this.mode_.proc();
		}

		// Token: 0x0600261F RID: 9759 RVA: 0x001A1251 File Offset: 0x0019F451
		protected void OnApplicationPause(bool pauseStatus)
		{
			if (!pauseStatus)
			{
				this.mode_.set(Manager.State.Resume);
			}
		}

		// Token: 0x04001C43 RID: 7235
		private Mode mode_;

		// Token: 0x020010A1 RID: 4257
		private enum State
		{
			// Token: 0x04005C45 RID: 23621
			None,
			// Token: 0x04005C46 RID: 23622
			Init,
			// Token: 0x04005C47 RID: 23623
			Wait,
			// Token: 0x04005C48 RID: 23624
			Resume,
			// Token: 0x04005C49 RID: 23625
			CustomSchemeAnalyze
		}

		// Token: 0x020010A2 RID: 4258
		public class Verbose : Verbose<Manager.Verbose>
		{
		}
	}
}
