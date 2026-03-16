using System;

public class BundleVersion
{
	public static string GetBuildVersion()
	{
		return "no version";
	}

	public static string GetBundleIdentifier()
	{
		return "no bundle id";
	}

	public static string GetBundleName()
	{
		string bundleIdentifier = BundleVersion.GetBundleIdentifier();
		return bundleIdentifier.Substring(bundleIdentifier.LastIndexOf('.') + 1);
	}
}
