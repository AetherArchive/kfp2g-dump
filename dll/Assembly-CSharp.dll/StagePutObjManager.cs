using System;
using System.Collections.Generic;
using UnityEngine;

public class StagePutObjManager : MonoBehaviour
{
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

	public void DestoryPutObj()
	{
		for (int i = 0; i < this.putObjList.Count; i++)
		{
			Object.Destroy(this.putObjList[i]);
		}
		this.putObjList.Clear();
	}

	public List<StagePutObjManager.PutInfo> putInfoList = new List<StagePutObjManager.PutInfo>();

	private List<GameObject> putObjList = new List<GameObject>();

	[Serializable]
	public class PutInfo
	{
		public Vector3 localPosition;

		public Vector3 localScale;

		public Quaternion localRotation;

		public Object putPrefab;

		public string objName = "";
	}
}
