using System;
using UnityEngine;

// Token: 0x0200006A RID: 106
public class Osage : MonoBehaviour
{
	// Token: 0x060002B8 RID: 696 RVA: 0x0001604C File Offset: 0x0001424C
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

	// Token: 0x060002B9 RID: 697 RVA: 0x000160E4 File Offset: 0x000142E4
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

	// Token: 0x060002BA RID: 698 RVA: 0x000161A4 File Offset: 0x000143A4
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

	// Token: 0x060002BB RID: 699 RVA: 0x00016258 File Offset: 0x00014458
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

	// Token: 0x060002BC RID: 700 RVA: 0x000165B0 File Offset: 0x000147B0
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

	// Token: 0x060002BD RID: 701 RVA: 0x00016600 File Offset: 0x00014800
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

	// Token: 0x060002BE RID: 702 RVA: 0x000166DC File Offset: 0x000148DC
	public void Reset()
	{
		if (this.reset < 3)
		{
			this.reset = 3;
		}
	}

	// Token: 0x1700007E RID: 126
	// (get) Token: 0x060002BF RID: 703 RVA: 0x000166EE File Offset: 0x000148EE
	public Vector3 InitialLocalPos
	{
		get
		{
			return this.initialLocalPos;
		}
	}

	// Token: 0x1700007F RID: 127
	// (get) Token: 0x060002C0 RID: 704 RVA: 0x000166F6 File Offset: 0x000148F6
	public Quaternion LocalRotation
	{
		get
		{
			return this.localRotation;
		}
	}

	// Token: 0x060002C1 RID: 705 RVA: 0x00016700 File Offset: 0x00014900
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

	// Token: 0x04000453 RID: 1107
	public Vector3 boneAxis = new Vector3(0f, 0f, -1f);

	// Token: 0x04000454 RID: 1108
	public float stiffnessForce = 0.1f;

	// Token: 0x04000455 RID: 1109
	public float dragForce = 0.01f;

	// Token: 0x04000456 RID: 1110
	public Vector3 springForce = new Vector3(0f, -0.001f, 0f);

	// Token: 0x04000457 RID: 1111
	private Transform childBone;

	// Token: 0x04000458 RID: 1112
	private Transform parentBone;

	// Token: 0x04000459 RID: 1113
	private float springLength;

	// Token: 0x0400045A RID: 1114
	private Quaternion localRotation;

	// Token: 0x0400045B RID: 1115
	private Transform trs;

	// Token: 0x0400045C RID: 1116
	private Vector3 crntPos;

	// Token: 0x0400045D RID: 1117
	private Vector3 prevPos;

	// Token: 0x0400045E RID: 1118
	private Vector3 oldPos;

	// Token: 0x0400045F RID: 1119
	private int reset;

	// Token: 0x04000460 RID: 1120
	private Vector3 initialLocalPos;

	// Token: 0x04000461 RID: 1121
	private float scale;
}
