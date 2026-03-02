using System;

// Token: 0x020001E7 RID: 487
public class BundleVersion
{
	// Token: 0x06002085 RID: 8325 RVA: 0x0018B945 File Offset: 0x00189B45
	public static string GetBuildVersion()
	{
		return "no version";
	}

	// Token: 0x06002086 RID: 8326 RVA: 0x0018B94C File Offset: 0x00189B4C
	public static string GetBundleIdentifier()
	{
		return "no bundle id";
	}

	// Token: 0x06002087 RID: 8327 RVA: 0x0018B953 File Offset: 0x00189B53
	public static string GetBundleName()
	{
		string bundleIdentifier = BundleVersion.GetBundleIdentifier();
		return bundleIdentifier.Substring(bundleIdentifier.LastIndexOf('.') + 1);
	}
}
