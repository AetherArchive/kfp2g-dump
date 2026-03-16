using System;
using UnityEngine;

public class DebugModelTestCtrl : MonoBehaviour
{
	private void Start()
	{
		if (this.charaAnimeCtrl != null)
		{
			this.charaAnimeCtrl.ExPlayAnimation("IDLING_LP_DHS_M_NOM", null);
		}
	}

	private void Update()
	{
	}

	public SimpleAnimation charaAnimeCtrl;
}
