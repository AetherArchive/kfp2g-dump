using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000EB RID: 235
public class StagePutObjManager : MonoBehaviour
{
	// Token: 0x06000AC5 RID: 2757 RVA: 0x0003EC08 File Offset: 0x0003CE08
	public void Data2Scene()
	{
		this.DestoryPutObj();
		for (int i = 0; i < this.putInfoList.Count; i++)
		{
			if (!(this.putInfoList[i].putPrefab == null))
			{
				GameObject gameObject = Object.Instantiate(this.putInfoList[i].putPrefab, base.transform) as GameObject;
				gameObject.transform.localPosition = this.putInfoList[i].localPosition;
				gameObject.transform.localScale = this.putInfoList[i].localScale;
				gameObject.transform.localRotation = this.putInfoList[i].localRotation;
				gameObject.SetLayerRecursively(LayerMask.NameToLayer("FieldStage"));
				gameObject.name = this.putInfoList[i].objName;
				this.putObjList.Add(gameObject);
			}
		}
	}

	// Token: 0x06000AC6 RID: 2758 RVA: 0x0003ECFC File Offset: 0x0003CEFC
	public void DestoryPutObj()
	{
		for (int i = 0; i < this.putObjList.Count; i++)
		{
			Object.Destroy(this.putObjList[i]);
		}
		this.putObjList.Clear();
	}

	// Token: 0x04000866 RID: 2150
	public List<StagePutObjManager.PutInfo> putInfoList = new List<StagePutObjManager.PutInfo>();

	// Token: 0x04000867 RID: 2151
	private List<GameObject> putObjList = new List<GameObject>();

	// Token: 0x020007F4 RID: 2036
	[Serializable]
	public class PutInfo
	{
		// Token: 0x04003590 RID: 13712
		public Vector3 localPosition;

		// Token: 0x04003591 RID: 13713
		public Vector3 localScale;

		// Token: 0x04003592 RID: 13714
		public Quaternion localRotation;

		// Token: 0x04003593 RID: 13715
		public Object putPrefab;

		// Token: 0x04003594 RID: 13716
		public string objName = "";
	}
}
