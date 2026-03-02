using System;

// Token: 0x020000BE RID: 190
public class StaminaIntvlChgInfo
{
	// Token: 0x06000870 RID: 2160 RVA: 0x00036FDB File Offset: 0x000351DB
	public StaminaIntvlChgInfo(DateTime changeDate, long nextIntervalTick)
	{
		this.changeDate = changeDate;
		this.afterChgIntervalTick = nextIntervalTick;
	}

	// Token: 0x04000719 RID: 1817
	public readonly DateTime changeDate;

	// Token: 0x0400071A RID: 1818
	public readonly long afterChgIntervalTick;
}
