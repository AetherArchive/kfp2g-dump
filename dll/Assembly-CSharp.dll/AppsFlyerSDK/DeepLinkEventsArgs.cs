using System;
using System.Collections.Generic;

namespace AppsFlyerSDK
{
	public class DeepLinkEventsArgs : EventArgs
	{
		public DeepLinkStatus status { get; }

		public DeepLinkError error { get; }

		public string getMatchType()
		{
			return this.getDeepLinkParameter("match_type");
		}

		public string getDeepLinkValue()
		{
			return this.getDeepLinkParameter("deep_link_value");
		}

		public string getClickHttpReferrer()
		{
			return this.getDeepLinkParameter("click_http_referrer");
		}

		public string getMediaSource()
		{
			return this.getDeepLinkParameter("media_source");
		}

		public string getCampaign()
		{
			return this.getDeepLinkParameter("campaign");
		}

		public string getCampaignId()
		{
			return this.getDeepLinkParameter("campaign_id");
		}

		public string getAfSub1()
		{
			return this.getDeepLinkParameter("af_sub1");
		}

		public string getAfSub2()
		{
			return this.getDeepLinkParameter("af_sub2");
		}

		public string getAfSub3()
		{
			return this.getDeepLinkParameter("af_sub3");
		}

		public string getAfSub4()
		{
			return this.getDeepLinkParameter("af_sub4");
		}

		public string getAfSub5()
		{
			return this.getDeepLinkParameter("af_sub5");
		}

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

		public Dictionary<string, object> getDeepLinkDictionary()
		{
			return this.deepLink;
		}

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

		private string getDeepLinkParameter(string name)
		{
			if (this.deepLink != null && this.deepLink.ContainsKey(name) && this.deepLink[name] != null)
			{
				return this.deepLink[name].ToString();
			}
			return null;
		}

		public Dictionary<string, object> deepLink;
	}
}
