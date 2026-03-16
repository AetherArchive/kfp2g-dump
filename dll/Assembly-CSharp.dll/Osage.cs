using System;
using UnityEngine;

public class Osage : MonoBehaviour
{
	private void Initialize()
	{
		if (this.trs != null)
		{
			return;
		}
		this.trs = base.transform;
		this.localRotation = this.trs.localRotation;
		this.initialLocalPos = this.trs.localPosition;
		this.scale = this.trs.localScale.x;
		Transform transform = this.trs.parent;
		while (transform != null)
		{
			this.scale *= transform.localScale.x;
			transform = transform.parent;
		}
	}

	private void Awake()
	{
		this.Initialize();
		this.childBone = null;
		foreach (object obj in base.transform)
		{
			Transform transform = (Transform)obj;
			if (transform.name.IndexOf("j_osg") == 0)
			{
				this.childBone = transform;
				break;
			}
		}
		this.parentBone = ((base.transform.parent == null || base.transform.parent.GetComponent<Osage>() == null) ? null : base.transform.parent);
		this.reset = 0;
	}

	private void Start()
	{
		if (this.trs == null)
		{
			return;
		}
		if (this.childBone == null)
		{
			this.springLength = this.boneAxis.magnitude * this.scale;
			this.crntPos = this.trs.TransformPoint(this.boneAxis);
		}
		else
		{
			this.springLength = Vector3.Distance(this.trs.position, this.childBone.position);
			this.crntPos = this.childBone.position;
		}
		this.prevPos = this.crntPos;
		this.oldPos = new Vector3(0f, -9999f, 0f);
	}

	private void UpdateSpring(bool rst, Transform thighL, Transform thighR, bool mov)
	{
		if (mov)
		{
			this.trs.localRotation = Quaternion.identity * this.localRotation;
			Vector3 vector = this.trs.TransformDirection(this.boneAxis);
			if (rst)
			{
				this.crntPos = vector.normalized * this.springLength + this.trs.position;
				this.prevPos = this.crntPos;
			}
			this.oldPos = this.trs.position;
			Vector3 vector2 = this.trs.rotation * (this.boneAxis * this.stiffnessForce);
			vector2 += (this.prevPos - this.crntPos) / this.scale * this.dragForce;
			vector2 += this.springForce;
			Vector3 vector3 = this.crntPos;
			this.crntPos = this.crntPos - this.prevPos + this.crntPos + vector2 * this.scale;
			this.crntPos = (this.crntPos - this.trs.position).normalized * this.springLength + this.trs.position;
			this.prevPos = vector3;
			Quaternion quaternion = Quaternion.FromToRotation(vector, this.crntPos - this.trs.position);
			this.trs.rotation = quaternion * this.trs.rotation;
			if (thighL != null && thighR != null && base.name == "j_osg_apron_a")
			{
				float num = 90f - thighL.eulerAngles.z;
				if (num < 0f)
				{
					num = 0f;
				}
				float num2 = 90f - thighR.eulerAngles.z;
				if (num2 < 0f)
				{
					num2 = 0f;
				}
				float num3 = this.localRotation.eulerAngles.x - (num + num2) * 0.5f;
				Vector3 eulerAngles = this.trs.eulerAngles;
				if (eulerAngles.x > num3)
				{
					eulerAngles.x = num3;
				}
				if ((num3 -= eulerAngles.x) < 0f)
				{
					num3 = 0f;
				}
				eulerAngles.y -= eulerAngles.z;
				eulerAngles.z = (num2 - num) * 0.5f;
				if (eulerAngles.z < 0f)
				{
					eulerAngles.z += num3;
				}
				else
				{
					eulerAngles.z -= num3;
				}
				this.trs.eulerAngles = eulerAngles;
			}
		}
		else
		{
			this.prevPos = this.crntPos;
			this.crntPos = this.trs.TransformDirection(this.boneAxis).normalized * this.springLength + this.trs.position;
			this.oldPos = this.trs.position;
		}
		Osage osage = ((this.childBone == null) ? null : this.childBone.GetComponent<Osage>());
		if (osage != null)
		{
			osage.UpdateSpring(rst, null, null, mov);
		}
	}

	public void UpdateOsage()
	{
		if (this.trs == null)
		{
			return;
		}
		if (this.parentBone != null)
		{
			return;
		}
		if (TimeManager.Pause)
		{
			return;
		}
		this.trs.localRotation = Quaternion.identity * this.localRotation;
	}

	public void LateUpdateOsage(Transform thighL, Transform thighR)
	{
		if (this.trs == null)
		{
			return;
		}
		if (this.parentBone != null)
		{
			return;
		}
		if (TimeManager.Pause)
		{
			return;
		}
		if (Vector3.Distance(this.oldPos, this.trs.position) / this.scale > 0.1f && this.reset < 2)
		{
			this.reset = 2;
		}
		bool flag = false;
		Quaternion quaternion = this.trs.localRotation;
		this.trs.localRotation = Quaternion.identity * this.localRotation;
		if (quaternion == this.trs.localRotation)
		{
			flag = true;
		}
		else
		{
			this.trs.localRotation = quaternion;
		}
		this.UpdateSpring(this.reset > 0, thighL, thighR, flag);
		if (this.reset > 0)
		{
			this.reset--;
		}
	}

	public void Reset()
	{
		if (this.reset < 3)
		{
			this.reset = 3;
		}
	}

	public Vector3 InitialLocalPos
	{
		get
		{
			return this.initialLocalPos;
		}
	}

	public Quaternion LocalRotation
	{
		get
		{
			return this.localRotation;
		}
	}

	public void ResetForce(Osage inSrc = null)
	{
		this.Initialize();
		if (inSrc != null)
		{
			inSrc.Initialize();
			this.initialLocalPos = inSrc.InitialLocalPos;
			this.localRotation = inSrc.LocalRotation;
		}
		this.trs.localPosition = this.initialLocalPos;
		this.trs.localRotation = this.localRotation;
	}

	public Vector3 boneAxis = new Vector3(0f, 0f, -1f);

	public float stiffnessForce = 0.1f;

	public float dragForce = 0.01f;

	public Vector3 springForce = new Vector3(0f, -0.001f, 0f);

	private Transform childBone;

	private Transform parentBone;

	private float springLength;

	private Quaternion localRotation;

	private Transform trs;

	private Vector3 crntPos;

	private Vector3 prevPos;

	private Vector3 oldPos;

	private int reset;

	private Vector3 initialLocalPos;

	private float scale;
}
