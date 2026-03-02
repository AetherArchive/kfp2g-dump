using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000111 RID: 273
public class ScenarioGUISelect : MonoBehaviour
{
	// Token: 0x06000D12 RID: 3346 RVA: 0x0005226F File Offset: 0x0005046F
	private void Start()
	{
	}

	// Token: 0x06000D13 RID: 3347 RVA: 0x00052271 File Offset: 0x00050471
	private void Update()
	{
	}

	// Token: 0x06000D14 RID: 3348 RVA: 0x00052274 File Offset: 0x00050474
	public void InitialiseSelect()
	{
		this.mSelectObj01 = base.transform.Find("Btn_Choice01").gameObject;
		this.mSelectLabel01 = this.mSelectObj01.transform.Find("BaseImage/Txt").GetComponent<Text>();
		this.mSelectObj02 = base.transform.Find("Btn_Choice02").gameObject;
		this.mSelectLabel02 = this.mSelectObj02.transform.Find("BaseImage/Txt").GetComponent<Text>();
		this.mSelectObj01.GetComponent<PguiButtonCtrl>().AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickSelect01), PguiButtonCtrl.SoundType.DEFAULT);
		this.mSelectObj02.GetComponent<PguiButtonCtrl>().AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickSelect02), PguiButtonCtrl.SoundType.DEFAULT);
		this.doublePosOne = this.mSelectObj01.transform.localPosition;
		this.doublePosTwo = this.mSelectObj02.transform.localPosition;
		this.singlePos = (this.doublePosOne + this.doublePosTwo) / 2f;
	}

	// Token: 0x06000D15 RID: 3349 RVA: 0x00052380 File Offset: 0x00050580
	public void SetSelectLabel(string label1, string label2 = "", bool isSingle = false)
	{
		this.label01 = label1;
		this.label02 = label2;
		this.mSelectedRow = -1;
		this.mSelectLabel01.text = this.label01;
		this.mSelectLabel02.text = this.label02;
		this.mSelectObj02.SetActive(!isSingle);
		if (isSingle)
		{
			this.mSelectObj01.transform.localPosition = this.singlePos;
			return;
		}
		this.mSelectObj01.transform.localPosition = this.doublePosOne;
		this.mSelectObj02.transform.localPosition = this.doublePosTwo;
	}

	// Token: 0x06000D16 RID: 3350 RVA: 0x00052419 File Offset: 0x00050619
	private void OnClickSelect01(PguiButtonCtrl button)
	{
		this.mSelectedRow = 1;
	}

	// Token: 0x06000D17 RID: 3351 RVA: 0x00052422 File Offset: 0x00050622
	private void OnClickSelect02(PguiButtonCtrl button)
	{
		this.mSelectedRow = 2;
	}

	// Token: 0x04000A80 RID: 2688
	public Text mSelectLabel01;

	// Token: 0x04000A81 RID: 2689
	private GameObject mSelectObj01;

	// Token: 0x04000A82 RID: 2690
	public Text mSelectLabel02;

	// Token: 0x04000A83 RID: 2691
	private GameObject mSelectObj02;

	// Token: 0x04000A84 RID: 2692
	public int mSelectedRow = -1;

	// Token: 0x04000A85 RID: 2693
	private string label01;

	// Token: 0x04000A86 RID: 2694
	private string label02;

	// Token: 0x04000A87 RID: 2695
	private Vector3 singlePos;

	// Token: 0x04000A88 RID: 2696
	private Vector3 doublePosOne;

	// Token: 0x04000A89 RID: 2697
	private Vector3 doublePosTwo;
}
