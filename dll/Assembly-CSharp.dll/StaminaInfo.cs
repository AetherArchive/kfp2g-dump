using System;

// Token: 0x020000BD RID: 189
public class StaminaInfo
{
	// Token: 0x0600086D RID: 2157 RVA: 0x00036E6C File Offset: 0x0003506C
	public StaminaInfo(int num, long tick, long interval, int max, StaminaIntvlChgInfo staminaIntvlChgInfo = null)
	{
		this.baseNum = num;
		this.baseTick = tick;
		this.intervalTick = interval;
		this.maxStock = max;
		this.limitBonus = 0;
		this.staminaIntvlChgInfo = staminaIntvlChgInfo;
	}

	// Token: 0x0600086E RID: 2158 RVA: 0x00036EA0 File Offset: 0x000350A0
	public void update(int num, long tick)
	{
		this.baseNum = num;
		this.baseTick = tick;
	}

	// Token: 0x0600086F RID: 2159 RVA: 0x00036EB0 File Offset: 0x000350B0
	public StaminaInfo.NowInfo GetInfoByNow(DateTime now)
	{
		int num = this.maxStock + this.limitBonus;
		int num2 = this.baseNum;
		long afterChgIntervalTick = this.intervalTick;
		num2 += (int)((now.Ticks - this.baseTick) / this.intervalTick);
		long num3 = (now.Ticks - this.baseTick) % this.intervalTick;
		if (this.staminaIntvlChgInfo != null && this.staminaIntvlChgInfo.changeDate.Ticks < now.Ticks)
		{
			long num4 = this.staminaIntvlChgInfo.changeDate.Ticks - this.baseTick;
			long num5 = now.Ticks - this.staminaIntvlChgInfo.changeDate.Ticks;
			afterChgIntervalTick = this.staminaIntvlChgInfo.afterChgIntervalTick;
			num2 = this.baseNum;
			num2 += (int)((num4 + this.intervalTick - 1L) / this.intervalTick) + (int)(num5 / afterChgIntervalTick);
			num3 = num5 % afterChgIntervalTick;
		}
		num2 = Math.Min(num2, num);
		num2 = Math.Max(num2, this.baseNum);
		bool flag = num2 < num;
		return new StaminaInfo.NowInfo(num2, num, new DateTime(flag ? (afterChgIntervalTick - num3) : 0L), new DateTime(flag ? ((long)(num - num2) * afterChgIntervalTick - num3) : 0L));
	}

	// Token: 0x04000713 RID: 1811
	private int baseNum;

	// Token: 0x04000714 RID: 1812
	private long baseTick;

	// Token: 0x04000715 RID: 1813
	private readonly long intervalTick;

	// Token: 0x04000716 RID: 1814
	private readonly int maxStock;

	// Token: 0x04000717 RID: 1815
	public int limitBonus;

	// Token: 0x04000718 RID: 1816
	private StaminaIntvlChgInfo staminaIntvlChgInfo;

	// Token: 0x020007B4 RID: 1972
	public class NowInfo
	{
		// Token: 0x0600370E RID: 14094 RVA: 0x001C7517 File Offset: 0x001C5717
		public NowInfo(int stackNum, int stackMaxNum, DateTime nextRecoveryTime, DateTime allRecoveryTime)
		{
			this.stackNum = stackNum;
			this.stackMaxNum = stackMaxNum;
			this.nextRecoveryTime = nextRecoveryTime;
			this.allRecoveryTime = allRecoveryTime;
		}

		// Token: 0x0400342E RID: 13358
		public int stackNum;

		// Token: 0x0400342F RID: 13359
		public int stackMaxNum;

		// Token: 0x04003430 RID: 13360
		public DateTime nextRecoveryTime;

		// Token: 0x04003431 RID: 13361
		public DateTime allRecoveryTime;
	}
}
