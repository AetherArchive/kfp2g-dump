using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grayscale : MonoBehaviour
{
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

	public void Awake()
	{
		this.img = null;
		this.raw = null;
		this.txt = null;
		this.lin = null;
	}

	private void OnDestroy()
	{
		this.img = null;
		this.raw = null;
		this.txt = null;
		this.lin = null;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	private List<Grayscale.IMG> img;

	private List<Grayscale.RAW> raw;

	private List<Grayscale.TXT> txt;

	private List<PguiOutline> lin;

	public class IMG
	{
		public Image img;

		public Material mat;

		public Material gry;
	}

	public class RAW
	{
		public RawImage raw;

		public Material mat;

		public Material gry;
	}

	public class TXT
	{
		public Text txt;

		public Material mat;

		public Material gry;
	}
}
