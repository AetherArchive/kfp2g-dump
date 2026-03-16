using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DebugIconItemTest : MonoBehaviour
{
	private void Start()
	{
	}

	public void UpdateIcon()
	{
		if (!this.isSetup)
		{
			this.isSetup = true;
			List<int> list = new List<int>
			{
				2, 4, 2001, 2002, 10002, 11002, 12001, 13001, 14001, 30001,
				30101, 30102, 31001, 54001, 70020
			};
			for (int i = 0; i < list.Count; i++)
			{
				this.iconList[i].Setup(DataManager.DmItem.GetItemStaticBase(list[i]));
			}
		}
	}

	private void Update()
	{
		if (!this.isSetup)
		{
			return;
		}
		if (this.icon == null)
		{
			this.icon = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item).GetComponent<IconItemCtrl>();
			this.icon.transform.SetParent(base.transform, false);
			this.rect = this.icon.GetComponent<RectTransform>().rect;
			this.rect.position = this.rect.position + new Vector2((float)Screen.width * 0.5f, (float)Screen.height * 0.5f);
		}
		if (this.list == null)
		{
			this.list = new List<ItemStaticBase>(DataManager.DmItem.GetItemStaticMap().Values);
		}
		if (this.capture == null)
		{
			Camera componentInChildren = base.transform.GetComponentInChildren<Camera>();
			this.capture = componentInChildren.gameObject.AddComponent<ImageCapture>();
		}
		if (this.step == 0)
		{
			if (this.list.Count > 0)
			{
				this.item = this.list[0].GetId();
				this.icon.Setup(this.list[0]);
				this.list.RemoveAt(0);
				this.step++;
				return;
			}
			this.icon.Clear();
			return;
		}
		else
		{
			if (this.step == 1)
			{
				this.capture.capture(this.rect, Screen.width, Screen.height);
				this.step++;
				return;
			}
			if (this.step == 2)
			{
				if (this.capture.data != null)
				{
					string text = "../ItemIconImage";
					if (!Directory.Exists(text))
					{
						Directory.CreateDirectory(text);
					}
					text = text + "/itemicon_" + this.item.ToString() + ".png";
					File.WriteAllBytes(text, this.capture.data);
					this.capture.data = null;
					this.step++;
					return;
				}
			}
			else
			{
				this.step = 0;
			}
			return;
		}
	}

	public List<IconItemCtrl> iconList;

	private bool isSetup;

	private int step;

	private IconItemCtrl icon;

	private int item;

	private Rect rect = Rect.zero;

	private List<ItemStaticBase> list;

	private ImageCapture capture;
}
