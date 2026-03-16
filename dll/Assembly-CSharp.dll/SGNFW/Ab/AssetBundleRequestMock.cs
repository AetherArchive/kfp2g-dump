using System;
using UnityEngine;

namespace SGNFW.Ab
{
	public class AssetBundleRequestMock : AssetBundleRequest
	{
		public AssetBundleRequestMock(Object asset)
		{
			this.asset_ = asset;
			this.startTime_ = Time.realtimeSinceStartup;
		}

		public AssetBundleRequestMock(Object[] allAssets)
		{
			this.allAssets_ = allAssets;
			this.startTime_ = Time.realtimeSinceStartup;
		}

		public override Object[] allAssets
		{
			get
			{
				return this.allAssets_;
			}
		}

		public override Object asset
		{
			get
			{
				return this.asset_;
			}
		}

		public override bool isDone
		{
			get
			{
				return this.startTime_ + 0.1f < Time.realtimeSinceStartup;
			}
		}

		public override int priority { get; set; }

		public override float progress
		{
			get
			{
				return Mathf.Clamp01(Time.realtimeSinceStartup - this.startTime_ / 0.1f);
			}
		}

		public override bool keepWaiting
		{
			get
			{
				return !this.isDone;
			}
		}

		private Object asset_;

		private Object[] allAssets_;

		private float startTime_;

		private const float LOAD_TIME = 0.1f;
	}
}
