using System;
using UnityEngine;
using UnityEngine.UI;

public class BirthdayWindowCtrl : MonoBehaviour
{
	public class GUI
	{
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.openWindow = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.inputFiledYear = baseTr.Find("Base/Window/Input_Base/InputField_L").GetComponent<InputField>();
			this.inputFiledMonth = baseTr.Find("Base/Window/Input_Base/InputField_R").GetComponent<InputField>();
		}

		public GameObject baseObj;

		public PguiOpenWindowCtrl openWindow;

		public InputField inputFiledYear;

		public InputField inputFiledMonth;
	}
}
