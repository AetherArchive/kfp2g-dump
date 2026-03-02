using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000CB RID: 203
public class Grayscale : MonoBehaviour
{
	// Token: 0x06000912 RID: 2322 RVA: 0x000397D4 File Offset: 0x000379D4
	private void Setup()
	{
		if (this.img == null)
		{
			this.img = new List<Grayscale.IMG>();
			Image[] componentsInChildren = base.GetComponentsInChildren<Image>(true);
			if (componentsInChildren != null)
			{
				foreach (Image image in componentsInChildren)
				{
					this.img.Add(new Grayscale.IMG
					{
						img = image,
						mat = image.material,
						gry = new Material(Shader.Find("UI/Grayscale"))
					});
				}
			}
		}
		if (this.raw == null)
		{
			this.raw = new List<Grayscale.RAW>();
			RawImage[] componentsInChildren2 = base.GetComponentsInChildren<RawImage>(true);
			if (componentsInChildren2 != null)
			{
				foreach (RawImage rawImage in componentsInChildren2)
				{
					this.raw.Add(new Grayscale.RAW
					{
						raw = rawImage,
						mat = rawImage.material,
						gry = new Material(Shader.Find("UI/Grayscale"))
					});
				}
			}
		}
		if (this.txt == null)
		{
			this.txt = new List<Grayscale.TXT>();
			Text[] componentsInChildren3 = base.GetComponentsInChildren<Text>(true);
			if (componentsInChildren3 != null)
			{
				foreach (Text text in componentsInChildren3)
				{
					this.txt.Add(new Grayscale.TXT
					{
						txt = text,
						mat = text.material,
						gry = new Material(Shader.Find("UI/Grayscale"))
					});
				}
			}
			foreach (Grayscale.TXT txt in this.txt)
			{
				txt.gry.color = new Color(1f, 1f, 1f, 0.5f);
			}
		}
		if (this.lin == null)
		{
			PguiOutline[] componentsInChildren4 = base.GetComponentsInChildren<PguiOutline>();
			this.lin = ((componentsInChildren4 == null) ? new List<PguiOutline>() : new List<PguiOutline>(componentsInChildren4));
		}
	}

	// Token: 0x06000913 RID: 2323 RVA: 0x000399C0 File Offset: 0x00037BC0
	public void OnEnable()
	{
		this.Setup();
		foreach (Grayscale.IMG img in this.img)
		{
			img.img.material = img.gry;
		}
		foreach (Grayscale.RAW raw in this.raw)
		{
			raw.raw.material = raw.gry;
		}
		foreach (Grayscale.TXT txt in this.txt)
		{
			txt.txt.material = txt.gry;
		}
		foreach (PguiOutline pguiOutline in this.lin)
		{
			pguiOutline.enabled = false;
		}
	}

	// Token: 0x06000914 RID: 2324 RVA: 0x00039B00 File Offset: 0x00037D00
	public void OnDisable()
	{
		this.Setup();
		foreach (Grayscale.IMG img in this.img)
		{
			img.img.material = img.mat;
		}
		foreach (Grayscale.RAW raw in this.raw)
		{
			raw.raw.material = raw.mat;
		}
		foreach (Grayscale.TXT txt in this.txt)
		{
			txt.txt.material = txt.mat;
		}
		foreach (PguiOutline pguiOutline in this.lin)
		{
			pguiOutline.enabled = true;
		}
	}

	// Token: 0x06000915 RID: 2325 RVA: 0x00039C40 File Offset: 0x00037E40
	public void Awake()
	{
		this.img = null;
		this.raw = null;
		this.txt = null;
		this.lin = null;
	}

	// Token: 0x06000916 RID: 2326 RVA: 0x00039C5E File Offset: 0x00037E5E
	private void OnDestroy()
	{
		this.img = null;
		this.raw = null;
		this.txt = null;
		this.lin = null;
	}

	// Token: 0x06000917 RID: 2327 RVA: 0x00039C7C File Offset: 0x00037E7C
	private void Start()
	{
	}

	// Token: 0x06000918 RID: 2328 RVA: 0x00039C7E File Offset: 0x00037E7E
	private void Update()
	{
	}

	// Token: 0x04000771 RID: 1905
	private List<Grayscale.IMG> img;

	// Token: 0x04000772 RID: 1906
	private List<Grayscale.RAW> raw;

	// Token: 0x04000773 RID: 1907
	private List<Grayscale.TXT> txt;

	// Token: 0x04000774 RID: 1908
	private List<PguiOutline> lin;

	// Token: 0x020007C2 RID: 1986
	public class IMG
	{
		// Token: 0x04003495 RID: 13461
		public Image img;

		// Token: 0x04003496 RID: 13462
		public Material mat;

		// Token: 0x04003497 RID: 13463
		public Material gry;
	}

	// Token: 0x020007C3 RID: 1987
	public class RAW
	{
		// Token: 0x04003498 RID: 13464
		public RawImage raw;

		// Token: 0x04003499 RID: 13465
		public Material mat;

		// Token: 0x0400349A RID: 13466
		public Material gry;
	}

	// Token: 0x020007C4 RID: 1988
	public class TXT
	{
		// Token: 0x0400349B RID: 13467
		public Text txt;

		// Token: 0x0400349C RID: 13468
		public Material mat;

		// Token: 0x0400349D RID: 13469
		public Material gry;
	}
}
