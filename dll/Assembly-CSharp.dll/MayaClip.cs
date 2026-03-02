using System;

// Token: 0x020001F4 RID: 500
public class MayaClip
{
	// Token: 0x0600214B RID: 8523 RVA: 0x0018E734 File Offset: 0x0018C934
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

	// Token: 0x0600214C RID: 8524 RVA: 0x0018E7BC File Offset: 0x0018C9BC
	public override string ToString()
	{
		return string.Format("CLIP{{ name={0} frame={1} sstart={2} send={3} scale={4} pre={5} post={6} hold={7} }}", new object[] { this.Name, this.StartFrame, this.SourceStart, this.SourceEnd, this.Scale, this.PreCycle, this.PostCycle, this.Hold });
	}

	// Token: 0x040017EB RID: 6123
	public string Name;

	// Token: 0x040017EC RID: 6124
	public double StartFrame;

	// Token: 0x040017ED RID: 6125
	public double SourceStart;

	// Token: 0x040017EE RID: 6126
	public double SourceEnd;

	// Token: 0x040017EF RID: 6127
	public double Scale;

	// Token: 0x040017F0 RID: 6128
	public double PreCycle;

	// Token: 0x040017F1 RID: 6129
	public double PostCycle;

	// Token: 0x040017F2 RID: 6130
	public double Hold;
}
