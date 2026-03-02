using System;
using UnityEngine;

// Token: 0x02000026 RID: 38
[ExecuteInEditMode]
[AddComponentMenu("Colorful/Vintage")]
public class CC_Vintage : CC_LookupFilter
{
	// Token: 0x06000063 RID: 99 RVA: 0x00003D0C File Offset: 0x00001F0C
	protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.filter == CC_Vintage.Filter.None)
		{
			this.lookupTexture = null;
		}
		else
		{
			this.lookupTexture = Resources.Load<Texture2D>("Instagram/" + this.filter.ToString());
		}
		base.OnRenderImage(source, destination);
	}

	// Token: 0x040000EE RID: 238
	public CC_Vintage.Filter filter;

	// Token: 0x0200058E RID: 1422
	public enum Filter
	{
		// Token: 0x04002926 RID: 10534
		None,
		// Token: 0x04002927 RID: 10535
		F1977,
		// Token: 0x04002928 RID: 10536
		Aden,
		// Token: 0x04002929 RID: 10537
		Amaro,
		// Token: 0x0400292A RID: 10538
		Brannan,
		// Token: 0x0400292B RID: 10539
		Crema,
		// Token: 0x0400292C RID: 10540
		Earlybird,
		// Token: 0x0400292D RID: 10541
		Hefe,
		// Token: 0x0400292E RID: 10542
		Hudson,
		// Token: 0x0400292F RID: 10543
		Inkwell,
		// Token: 0x04002930 RID: 10544
		Kelvin,
		// Token: 0x04002931 RID: 10545
		LoFi,
		// Token: 0x04002932 RID: 10546
		Ludwig,
		// Token: 0x04002933 RID: 10547
		Mayfair,
		// Token: 0x04002934 RID: 10548
		Nashville,
		// Token: 0x04002935 RID: 10549
		Perpetua,
		// Token: 0x04002936 RID: 10550
		Rise,
		// Token: 0x04002937 RID: 10551
		Sierra,
		// Token: 0x04002938 RID: 10552
		Slumber,
		// Token: 0x04002939 RID: 10553
		Sutro,
		// Token: 0x0400293A RID: 10554
		Toaster,
		// Token: 0x0400293B RID: 10555
		Valencia,
		// Token: 0x0400293C RID: 10556
		Walden,
		// Token: 0x0400293D RID: 10557
		Willow,
		// Token: 0x0400293E RID: 10558
		XProII
	}
}
