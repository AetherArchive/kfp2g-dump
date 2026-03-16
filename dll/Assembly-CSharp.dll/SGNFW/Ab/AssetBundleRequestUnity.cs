using System;
using UnityEngine;

namespace SGNFW.Ab
{
	public class AssetBundleRequestUnity : AssetBundleRequest
	{
		public AssetBundleRequestUnity(AssetBundleRequest request)
		{
			this.request_ = request;
		}

		public override Object[] allAssets
		{
			get
			{
				return this.request_.allAssets;
			}
		}

		public override Object asset
		{
			get
			{
				return this.request_.asset;
			}
		}

		public override bool isDone
		{
			get
			{
				return this.request_.isDone;
			}
		}

		public override int priority
		{
			get
			{
				return this.request_.priority;
			}
			set
			{
				this.request_.priority = value;
			}
		}

		public override float progress
		{
			get
			{
				return this.request_.progress;
			}
		}

		public override bool keepWaiting
		{
			get
			{
				return !this.request_.isDone;
			}
		}

		private AssetBundleRequest request_;
	}
}
