using System;

public class MayaClip
{
	public MayaClip()
	{
		this.Name = "";
		this.StartFrame = 0.0;
		this.SourceStart = 0.0;
		this.SourceEnd = 0.0;
		this.Scale = 1.0;
		this.PreCycle = 0.0;
		this.PostCycle = 0.0;
		this.Hold = 0.0;
	}

	public override string ToString()
	{
		return string.Format("CLIP{{ name={0} frame={1} sstart={2} send={3} scale={4} pre={5} post={6} hold={7} }}", new object[] { this.Name, this.StartFrame, this.SourceStart, this.SourceEnd, this.Scale, this.PreCycle, this.PostCycle, this.Hold });
	}

	public string Name;

	public double StartFrame;

	public double SourceStart;

	public double SourceEnd;

	public double Scale;

	public double PreCycle;

	public double PostCycle;

	public double Hold;
}
