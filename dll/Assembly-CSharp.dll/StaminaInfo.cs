using System;

public class StaminaInfo
{
	public StaminaInfo(int num, long tick, long interval, int max, StaminaIntvlChgInfo staminaIntvlChgInfo = null)
	{
		this.baseNum = num;
		this.baseTick = tick;
		this.intervalTick = interval;
		this.maxStock = max;
		this.limitBonus = 0;
		this.staminaIntvlChgInfo = staminaIntvlChgInfo;
	}

	public void update(int num, long tick)
	{
		this.baseNum = num;
		this.baseTick = tick;
	}

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

	private int baseNum;

	private long baseTick;

	private readonly long intervalTick;

	private readonly int maxStock;

	public int limitBonus;

	private StaminaIntvlChgInfo staminaIntvlChgInfo;

	public class NowInfo
	{
		public NowInfo(int stackNum, int stackMaxNum, DateTime nextRecoveryTime, DateTime allRecoveryTime)
		{
			this.stackNum = stackNum;
			this.stackMaxNum = stackMaxNum;
			this.nextRecoveryTime = nextRecoveryTime;
			this.allRecoveryTime = allRecoveryTime;
		}

		public int stackNum;

		public int stackMaxNum;

		public DateTime nextRecoveryTime;

		public DateTime allRecoveryTime;
	}
}
