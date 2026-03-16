using System;

public class StaminaIntvlChgInfo
{
	public StaminaIntvlChgInfo(DateTime changeDate, long nextIntervalTick)
	{
		this.changeDate = changeDate;
		this.afterChgIntervalTick = nextIntervalTick;
	}

	public readonly DateTime changeDate;

	public readonly long afterChgIntervalTick;
}
