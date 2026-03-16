using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/Vintage")]
public class CC_Vintage : CC_LookupFilter
{
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

	public CC_Vintage.Filter filter;

	public enum Filter
	{
		None,
		F1977,
		Aden,
		Amaro,
		Brannan,
		Crema,
		Earlybird,
		Hefe,
		Hudson,
		Inkwell,
		Kelvin,
		LoFi,
		Ludwig,
		Mayfair,
		Nashville,
		Perpetua,
		Rise,
		Sierra,
		Slumber,
		Sutro,
		Toaster,
		Valencia,
		Walden,
		Willow,
		XProII
	}
}
