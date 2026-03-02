using System;
using System.Collections.Generic;

namespace AppsFlyerSDK
{
	// Token: 0x02000583 RID: 1411
	public class DeepLinkEventsArgs : EventArgs
	{
		// Token: 0x17000640 RID: 1600
		// (get) Token: 0x06002EC2 RID: 11970 RVA: 0x001B2B87 File Offset: 0x001B0D87
		public DeepLinkStatus status { get; }

		// Token: 0x17000641 RID: 1601
		// (get) Token: 0x06002EC3 RID: 11971 RVA: 0x001B2B8F File Offset: 0x001B0D8F
		public DeepLinkError error { get; }

		// Token: 0x06002EC4 RID: 11972 RVA: 0x001B2B97 File Offset: 0x001B0D97
		public string getMatchType()
		{
			return this.getDeepLinkParameter("match_type");
		}

		// Token: 0x06002EC5 RID: 11973 RVA: 0x001B2BA4 File Offset: 0x001B0DA4
		public string getDeepLinkValue()
		{
			return this.getDeepLinkParameter("deep_link_value");
		}

		// Token: 0x06002EC6 RID: 11974 RVA: 0x001B2BB1 File Offset: 0x001B0DB1
		public string getClickHttpReferrer()
		{
			return this.getDeepLinkParameter("click_http_referrer");
		}

		// Token: 0x06002EC7 RID: 11975 RVA: 0x001B2BBE File Offset: 0x001B0DBE
		public string getMediaSource()
		{
			return this.getDeepLinkParameter("media_source");
		}

		// Token: 0x06002EC8 RID: 11976 RVA: 0x001B2BCB File Offset: 0x001B0DCB
		public string getCampaign()
		{
			return this.getDeepLinkParameter("campaign");
		}

		// Token: 0x06002EC9 RID: 11977 RVA: 0x001B2BD8 File Offset: 0x001B0DD8
		public string getCampaignId()
		{
			return this.getDeepLinkParameter("campaign_id");
		}

		// Token: 0x06002ECA RID: 11978 RVA: 0x001B2BE5 File Offset: 0x001B0DE5
		public string getAfSub1()
		{
			return this.getDeepLinkParameter("af_sub1");
		}

		// Token: 0x06002ECB RID: 11979 RVA: 0x001B2BF2 File Offset: 0x001B0DF2
		public string getAfSub2()
		{
			return this.getDeepLinkParameter("af_sub2");
		}

		// Token: 0x06002ECC RID: 11980 RVA: 0x001B2BFF File Offset: 0x001B0DFF
		public string getAfSub3()
		{
			return this.getDeepLinkParameter("af_sub3");
		}

		// Token: 0x06002ECD RID: 11981 RVA: 0x001B2C0C File Offset: 0x001B0E0C
		public string getAfSub4()
		{
			return this.getDeepLinkParameter("af_sub4");
		}

		// Token: 0x06002ECE RID: 11982 RVA: 0x001B2C19 File Offset: 0x001B0E19
		public string getAfSub5()
		{
			return this.getDeepLinkParameter("af_sub5");
		}

		// Token: 0x06002ECF RID: 11983 RVA: 0x001B2C28 File Offset: 0x001B0E28
		public bool isDeferred()
		{
			if (this.deepLink != null && this.deepLink.ContainsKey("is_deferred"))
			{
				try
				{
					return (bool)this.deepLink["is_deferred"];
				}
				catch (Exception ex)
				{
					AppsFlyer.AFLog("DeepLinkEventsArgs.isDeferred", string.Format("{0} Exception caught.", ex));
				}
				return false;
			}
			return false;
		}

		// Token: 0x06002ED0 RID: 11984 RVA: 0x001B2C94 File Offset: 0x001B0E94
		public Dictionary<string, object> getDeepLinkDictionary()
		{
			return this.deepLink;
		}

		// Token: 0x06002ED1 RID: 11985 RVA: 0x001B2C9C File Offset: 0x001B0E9C
		public DeepLinkEventsArgs(string str)
		{
			try
			{
				Dictionary<string, object> dictionary = AppsFlyer.CallbackStringToDictionary(str);
				string text = "";
				string text2 = "";
				if (dictionary.ContainsKey("status") && dictionary["status"] != null)
				{
					text = dictionary["status"].ToString();
				}
				if (dictionary.ContainsKey("error") && dictionary["error"] != null)
				{
					text2 = dictionary["error"].ToString();
				}
				if (dictionary.ContainsKey("deepLink") && dictionary["deepLink"] != null)
				{
					this.deepLink = AppsFlyer.CallbackStringToDictionary(dictionary["deepLink"].ToString());
				}
				if (!(text == "FOUND"))
				{
					if (!(text == "NOT_FOUND"))
					{
						this.status = DeepLinkStatus.ERROR;
					}
					else
					{
						this.status = DeepLinkStatus.NOT_FOUND;
					}
				}
				else
				{
					this.status = DeepLinkStatus.FOUND;
				}
				if (!(text2 == "TIMEOUT"))
				{
					if (!(text2 == "NETWORK"))
					{
						if (!(text2 == "HTTP_STATUS_CODE"))
						{
							this.error = DeepLinkError.UNEXPECTED;
						}
						else
						{
							this.error = DeepLinkError.HTTP_STATUS_CODE;
						}
					}
					else
					{
						this.error = DeepLinkError.NETWORK;
					}
				}
				else
				{
					this.error = DeepLinkError.TIMEOUT;
				}
			}
			catch (Exception ex)
			{
				AppsFlyer.AFLog("DeepLinkEventsArgs.parseDeepLink", string.Format("{0} Exception caught.", ex));
			}
		}

		// Token: 0x06002ED2 RID: 11986 RVA: 0x001B2E04 File Offset: 0x001B1004
		private string getDeepLinkParameter(string name)
		{
			if (this.deepLink != null && this.deepLink.ContainsKey(name) && this.deepLink[name] != null)
			{
				return this.deepLink[name].ToString();
			}
			return null;
		}

		// Token: 0x040028F2 RID: 10482
		public Dictionary<string, object> deepLink;
	}
}
