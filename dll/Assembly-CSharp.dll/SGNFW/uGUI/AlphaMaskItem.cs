using System;
using UnityEngine;

namespace SGNFW.uGUI
{
	public class AlphaMaskItem : MonoBehaviour
	{
		public void SetAlphaMask(AlphaMask alphaMask)
		{
			this.alphaMask = alphaMask;
		}

		public void Apply()
		{
			if (this.alphaMask == null)
			{
				return;
			}
			this.alphaMask.Apply(base.gameObject);
		}

		private void Start()
		{
			if (this.alphaMask == null)
			{
				this.alphaMask = base.transform.GetComponentInParent<AlphaMask>();
			}
			this.Apply();
		}

		[SerializeField]
		private AlphaMask alphaMask;
	}
}
