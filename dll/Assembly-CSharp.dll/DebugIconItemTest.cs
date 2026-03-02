using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x0200002E RID: 46
public class DebugIconItemTest : MonoBehaviour
{
	// Token: 0x060000A2 RID: 162 RVA: 0x00005FBA File Offset: 0x000041BA
	private void Start()
	{
	}

	// Token: 0x060000A3 RID: 163 RVA: 0x00005FBC File Offset: 0x000041BC
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

	// Token: 0x060000A4 RID: 164 RVA: 0x000060B4 File Offset: 0x000042B4
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

	// Token: 0x0400011D RID: 285
	public List<IconItemCtrl> iconList;

	// Token: 0x0400011E RID: 286
	private bool isSetup;

	// Token: 0x0400011F RID: 287
	private int step;

	// Token: 0x04000120 RID: 288
	private IconItemCtrl icon;

	// Token: 0x04000121 RID: 289
	private int item;

	// Token: 0x04000122 RID: 290
	private Rect rect = Rect.zero;

	// Token: 0x04000123 RID: 291
	private List<ItemStaticBase> list;

	// Token: 0x04000124 RID: 292
	private ImageCapture capture;
}
