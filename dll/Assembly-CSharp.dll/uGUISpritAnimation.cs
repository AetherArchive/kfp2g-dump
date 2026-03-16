using System;
using UnityEngine;
using UnityEngine.UI;

public class uGUISpritAnimation : MonoBehaviour
{
	private void Start()
	{
		if (this.LTime <= 0f)
		{
			base.GetComponent<uGUISpritAnimation>().enabled = false;
		}
		if (base.GetComponent<Image>() == null)
		{
			base.gameObject.AddComponent<Image>();
		}
		this.WTime -= this.delay / this.LTime;
		this.m_image = base.GetComponent<Image>();
	}

	private void Update()
	{
		this.WTime += Time.deltaTime / this.LTime;
		if (1f <= this.WTime && this.Loop)
		{
			this.WTime = 0f;
		}
		this.ACurvTime = Mathf.Clamp(this.AA.Evaluate(this.WTime), 0f, 1f);
		this.EVCount = Mathf.FloorToInt(this.ACurvTime * (float)this._image.Length);
		if (this._image.Length <= this.EVCount)
		{
			this.EVCount = 0;
		}
		this.m_image.sprite = this._image[this.EVCount];
	}

	[SerializeField]
	public AnimationCurve AA;

	public float delay = 0.5f;

	public float LTime = 2f;

	public bool Loop = true;

	private float ACurvTime;

	private float WTime;

	public Sprite[] _image;

	private Image m_image;

	private int EVCount;

	private Transform _tran;
}
