using System;
using System.Collections.Generic;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;

public class StickerUtil : MonoBehaviour
{
	public class SizeChangeBtnGUI
	{
		public SizeChangeBtnGUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Btn_SizeChange = baseTr.GetComponent<PguiButtonCtrl>();
			this.Txt = baseTr.Find("BaseImage/On/Txt").GetComponent<PguiTextCtrl>();
		}

		public void Setup(StickerUtil.SizeChangeBtnGUI.SetupParam param)
		{
			this.setupParam = param;
			this.scrollSize = this.setupParam.refScrollView.Size;
			this.Btn_SizeChange.AddOnClickListener(delegate(PguiButtonCtrl btn)
			{
				if (btn == this.Btn_SizeChange)
				{
					this.setupParam.sizeIndex--;
					this.setupParam.sizeIndex = (this.setupParam.sizeIndex + this.setupParam.iconStickerParamList.Count) % this.setupParam.iconStickerParamList.Count;
					this.ResetScrollView();
					this.setupParam.funcResult(new StickerUtil.SizeChangeBtnGUI.ResultParam
					{
						sizeIndex = this.setupParam.sizeIndex
					});
				}
			}, PguiButtonCtrl.SoundType.DEFAULT);
		}

		private void UpdateText(int index)
		{
			switch (index)
			{
			case 0:
				this.Txt.text = PrjUtil.MakeMessage("小");
				return;
			case 1:
				this.Txt.text = PrjUtil.MakeMessage("中");
				return;
			case 2:
				this.Txt.text = PrjUtil.MakeMessage("大");
				return;
			case 3:
				this.Txt.text = PrjUtil.MakeMessage("特大");
				return;
			default:
				this.Txt.text = PrjUtil.MakeMessage("エラー");
				return;
			}
		}

		public void ResetScrollView()
		{
			this.UpdateText(this.setupParam.sizeIndex);
			this.setupParam.refScrollView.Clear();
			this.setupParam.refScrollView.SetPrefab(this.setupParam.iconStickerParamList[this.setupParam.sizeIndex].prefab);
			UnityAction resetCallback = this.setupParam.resetCallback;
			if (resetCallback != null)
			{
				resetCallback();
			}
			if (this.setupParam.refScrollView.onStartItem != null)
			{
				ReuseScroll refScrollView = this.setupParam.refScrollView;
				refScrollView.onStartItem = (Action<int, GameObject>)Delegate.Remove(refScrollView.onStartItem, this.setupParam.onStartItem);
			}
			if (this.setupParam.refScrollView.onUpdateItem != null)
			{
				ReuseScroll refScrollView2 = this.setupParam.refScrollView;
				refScrollView2.onUpdateItem = (Action<int, GameObject>)Delegate.Remove(refScrollView2.onUpdateItem, this.setupParam.onUpdateItem);
			}
			ReuseScroll refScrollView3 = this.setupParam.refScrollView;
			refScrollView3.onStartItem = (Action<int, GameObject>)Delegate.Combine(refScrollView3.onStartItem, this.setupParam.onStartItem);
			ReuseScroll refScrollView4 = this.setupParam.refScrollView;
			refScrollView4.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(refScrollView4.onUpdateItem, this.setupParam.onUpdateItem);
			this.setupParam.refScrollView.Size = this.scrollSize * this.setupParam.iconStickerParamList[this.setupParam.sizeIndex].scale.y;
			int num = ((this.setupParam.dispIconStickerCountCallback == null) ? 10 : this.setupParam.dispIconStickerCountCallback());
			this.setupParam.refScrollView.Setup(num / this.setupParam.iconStickerParamList[this.setupParam.sizeIndex].num + 1, 0);
			this.setupParam.refScrollView.Refresh();
		}

		public List<StickerUtil.SizeChangeBtnGUI.IconStickerParam> IconStickerParamList
		{
			get
			{
				return this.setupParam.iconStickerParamList;
			}
		}

		public int SizeIndex
		{
			get
			{
				return this.setupParam.sizeIndex;
			}
		}

		public GameObject baseObj;

		public PguiButtonCtrl Btn_SizeChange;

		public PguiTextCtrl Txt;

		private float scrollSize;

		private StickerUtil.SizeChangeBtnGUI.SetupParam setupParam = new StickerUtil.SizeChangeBtnGUI.SetupParam();

		public delegate void FuncResult(StickerUtil.SizeChangeBtnGUI.ResultParam result);

		public class IconStickerParam
		{
			public Vector3 scale;

			public Vector3 scaleCurrent;

			public Vector3 scaleCount;

			public int num;

			public GameObject prefab;
		}

		public class SetupParam
		{
			public int sizeIndex;

			public List<StickerUtil.SizeChangeBtnGUI.IconStickerParam> iconStickerParamList;

			public ReuseScroll refScrollView;

			public StickerUtil.SizeChangeBtnGUI.FuncResult funcResult;

			public Action<int, GameObject> onStartItem;

			public Action<int, GameObject> onUpdateItem;

			public UnityAction resetCallback;

			public Func<int> dispIconStickerCountCallback;
		}

		public class ResultParam
		{
			public int sizeIndex;
		}
	}
}
