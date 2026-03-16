using System;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioGUISelect : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

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

	private void OnClickSelect01(PguiButtonCtrl button)
	{
		this.mSelectedRow = 1;
	}

	private void OnClickSelect02(PguiButtonCtrl button)
	{
		this.mSelectedRow = 2;
	}

	public Text mSelectLabel01;

	private GameObject mSelectObj01;

	public Text mSelectLabel02;

	private GameObject mSelectObj02;

	public int mSelectedRow = -1;

	private string label01;

	private string label02;

	private Vector3 singlePos;

	private Vector3 doublePosOne;

	private Vector3 doublePosTwo;
}
