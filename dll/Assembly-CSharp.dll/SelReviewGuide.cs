using System;
using System.Collections;

public class SelReviewGuide
{
	public static IEnumerator ReviewGuide()
	{
		bool review = false;
		CanvasManager.HdlOpenWindowBasic.Setup("", "レビューにご協力をお願いします", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.REVIEW), true, delegate(int index)
		{
			review = index == 1;
			return true;
		}, null, false);
		CanvasManager.HdlOpenWindowBasic.Open();
		do
		{
			yield return null;
		}
		while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
		bool review2 = review;
		yield return null;
		yield break;
	}
}
