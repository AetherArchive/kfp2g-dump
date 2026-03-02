using System;
using System.Collections;

// Token: 0x020001C0 RID: 448
public class SelReviewGuide
{
	// Token: 0x06001EE3 RID: 7907 RVA: 0x0017FFA2 File Offset: 0x0017E1A2
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
