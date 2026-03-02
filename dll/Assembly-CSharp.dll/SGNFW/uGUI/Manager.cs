using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using UnityEngine;

namespace SGNFW.uGUI
{
	// Token: 0x0200022F RID: 559
	public class Manager
	{
		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x06002339 RID: 9017 RVA: 0x001969F7 File Offset: 0x00194BF7
		public static Camera CameraUI
		{
			get
			{
				return Manager.cam;
			}
		}

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x0600233A RID: 9018 RVA: 0x001969FE File Offset: 0x00194BFE
		public static Transform Root
		{
			get
			{
				return Manager.trs;
			}
		}

		// Token: 0x0600233B RID: 9019 RVA: 0x00196A08 File Offset: 0x00194C08
		private Manager(Transform trs, Camera cam)
		{
			Manager.trs = trs;
			Manager.cam = cam;
			Manager.currentLayerDict[Layer.None] = new Manager.Range(-32768, 32767);
			Manager.currentLayerDict[Layer.BG] = new Manager.Range(-1000, -1);
			Manager.currentLayerDict[Layer.UI] = new Manager.Range(0, 999);
			Manager.currentLayerDict[Layer.Overlay] = new Manager.Range(1000, 1999);
			Manager.currentLayerDict[Layer.System] = new Manager.Range(10000, 10999);
			Manager.currentLayerDict[Layer.Debug] = new Manager.Range(20000, 20999);
		}

		// Token: 0x0600233C RID: 9020 RVA: 0x00196ABB File Offset: 0x00194CBB
		public static void Initialize(Transform trs, Camera cam)
		{
			if (Manager.instance != null)
			{
				return;
			}
			Manager.instance = new Manager(trs, cam);
		}

		// Token: 0x0600233D RID: 9021 RVA: 0x00196AD1 File Offset: 0x00194CD1
		public static void Terminate()
		{
			Manager.instance = null;
		}

		// Token: 0x0600233E RID: 9022 RVA: 0x00196AD9 File Offset: 0x00194CD9
		public static void BeginGroup(string group)
		{
			Manager.groupName = group;
		}

		// Token: 0x0600233F RID: 9023 RVA: 0x00196AE1 File Offset: 0x00194CE1
		public static void EndGroup()
		{
			Manager.groupName = null;
		}

		// Token: 0x06002340 RID: 9024 RVA: 0x00196AEC File Offset: 0x00194CEC
		public static void ClearGroup(string group)
		{
			List<Group> list = Manager.groupList.FindAll((Group t) => t.label == group);
			for (int i = list.Count - 1; i >= 0; i--)
			{
				if (list[i] == null || list[i].gameObject == null)
				{
					Manager.groupList.Remove(list[i]);
				}
				else
				{
					Object.Destroy(list[i].gameObject);
				}
			}
		}

		// Token: 0x06002341 RID: 9025 RVA: 0x00196B78 File Offset: 0x00194D78
		public static void RemoveGroupUI(Group group)
		{
			if (!Manager.groupList.Contains(group))
			{
				return;
			}
			Manager.groupList.Remove(group);
			if (group.layer != Layer.None)
			{
				int num = int.MinValue;
				for (int i = 0; i < Manager.groupList.Count; i++)
				{
					Group group2 = Manager.groupList[i];
					if (group2.layer == group.layer && num <= group2.order)
					{
						num = group2.order;
					}
				}
				if (num == -2147483648)
				{
					Manager.currentLayerDict[group.layer].Reset();
					return;
				}
				Manager.currentLayerDict[group.layer].Current = num + 1;
			}
		}

		// Token: 0x06002342 RID: 9026 RVA: 0x00196C24 File Offset: 0x00194E24
		public static void AddGroupUI(Group group, bool addOrder)
		{
			if (Manager.groupList.Contains(group))
			{
				return;
			}
			if (group.layer > Layer.None && addOrder)
			{
				Manager.currentLayerDict[group.layer].Current = group.order + 1;
			}
			Manager.groupList.Add(group);
		}

		// Token: 0x06002343 RID: 9027 RVA: 0x00196C74 File Offset: 0x00194E74
		public static void Clear()
		{
			foreach (object obj in Manager.trs)
			{
				Object.Destroy(((Transform)obj).gameObject);
			}
		}

		// Token: 0x06002344 RID: 9028 RVA: 0x00196CD0 File Offset: 0x00194ED0
		public static GameObject Create(string path, Transform root = null, string group = null, string name = null, Layer layer = Layer.UI)
		{
			return Manager.Create(Resources.Load(path), root, group, name, layer);
		}

		// Token: 0x06002345 RID: 9029 RVA: 0x00196CE2 File Offset: 0x00194EE2
		public static GameObject Create(Object obj, Transform root = null, string group = null, string name = null, Layer layer = Layer.UI)
		{
			if (root == null)
			{
				root = Manager.trs;
			}
			return Manager._Create(obj, root, group, name, layer);
		}

		// Token: 0x06002346 RID: 9030 RVA: 0x00196D00 File Offset: 0x00194F00
		public static T Create<T>(string path, Transform root = null, string group = null, string name = null, Layer layer = Layer.UI) where T : MonoBehaviour
		{
			GameObject gameObject = Manager.Create(path, root, group, name, layer);
			if (gameObject == null)
			{
				return default(T);
			}
			return gameObject.GetComponent<T>();
		}

		// Token: 0x06002347 RID: 9031 RVA: 0x00196D34 File Offset: 0x00194F34
		public static T Create<T>(Object obj, Transform root = null, string group = null, string name = null, Layer layer = Layer.UI) where T : MonoBehaviour
		{
			GameObject gameObject = Manager.Create(obj, root, group, name, layer);
			if (gameObject == null)
			{
				return default(T);
			}
			return gameObject.GetComponent<T>();
		}

		// Token: 0x06002348 RID: 9032 RVA: 0x00196D66 File Offset: 0x00194F66
		public static IEnumerator CreateAsync(Action<GameObject> callback, string path, Transform root = null, string group = null, string name = null, Layer layer = Layer.UI)
		{
			ResourceRequest req = Resources.LoadAsync(path);
			while (!req.isDone)
			{
				yield return null;
			}
			Object @object = req.asset as GameObject;
			if (root == null)
			{
				root = Manager.trs;
			}
			GameObject gameObject = Manager._Create(@object, root, group, name, layer);
			callback(gameObject);
			yield break;
		}

		// Token: 0x06002349 RID: 9033 RVA: 0x00196D9C File Offset: 0x00194F9C
		private static GameObject _Create(Object obj, Transform root, string group, string name, Layer layer)
		{
			if (obj == null)
			{
				Verbose<Verbose>.LogError("[uGUI.Manager] Create obj is null", null);
				return null;
			}
			GameObject gameObject = Object.Instantiate(obj, root) as GameObject;
			if (string.IsNullOrEmpty(name))
			{
				gameObject.name = gameObject.name.Replace("(Clone)", "");
			}
			else
			{
				gameObject.name = name;
			}
			Canvas canvas = null;
			if (root != null)
			{
				canvas = root.GetComponentInParent<Canvas>();
			}
			bool flag = false;
			int num = Manager.currentLayerDict[layer].Current;
			if (layer != Layer.None)
			{
				Canvas[] componentsInChildren = gameObject.transform.GetComponentsInChildren<Canvas>(true);
				if (componentsInChildren != null && componentsInChildren.Length != 0)
				{
					foreach (Canvas canvas2 in componentsInChildren)
					{
						if (canvas2.worldCamera == null)
						{
							canvas2.worldCamera = Manager.cam;
						}
						if (canvas2.sortingOrder == 0)
						{
							if (canvas != null)
							{
								canvas2.sortingOrder = canvas.sortingOrder;
							}
							else
							{
								canvas2.sortingOrder = num;
							}
							flag = true;
						}
					}
				}
				ParticleSystem[] componentsInChildren2 = gameObject.transform.GetComponentsInChildren<ParticleSystem>(true);
				if (componentsInChildren2 != null && componentsInChildren2.Length != 0)
				{
					if (canvas != null)
					{
						ParticleSystem[] array2 = componentsInChildren2;
						for (int i = 0; i < array2.Length; i++)
						{
							array2[i].GetComponent<Renderer>().sortingOrder = canvas.sortingOrder;
						}
					}
					else
					{
						ParticleSystem[] array2 = componentsInChildren2;
						for (int i = 0; i < array2.Length; i++)
						{
							array2[i].GetComponent<Renderer>().sortingOrder = num;
							flag = true;
						}
					}
				}
			}
			if (string.IsNullOrEmpty(group))
			{
				group = Manager.groupName;
			}
			Group group2 = gameObject.AddComponent<Group>();
			group2.label = group;
			if (flag)
			{
				group2.layer = layer;
				group2.order = num;
			}
			Manager.AddGroupUI(group2, canvas == null);
			return gameObject;
		}

		// Token: 0x04001A9B RID: 6811
		private static Manager instance;

		// Token: 0x04001A9C RID: 6812
		private static string groupName;

		// Token: 0x04001A9D RID: 6813
		private static List<Group> groupList = new List<Group>();

		// Token: 0x04001A9E RID: 6814
		private static Transform trs;

		// Token: 0x04001A9F RID: 6815
		private static Camera cam;

		// Token: 0x04001AA0 RID: 6816
		private static Dictionary<Layer, Manager.Range> currentLayerDict = new Dictionary<Layer, Manager.Range>();

		// Token: 0x02001063 RID: 4195
		private class Range
		{
			// Token: 0x060052D9 RID: 21209 RVA: 0x002495A4 File Offset: 0x002477A4
			public Range(int _min, int _max)
			{
				this.min = _min;
				this.max = _max;
				this.cur = _min;
			}

			// Token: 0x17000BEA RID: 3050
			// (get) Token: 0x060052DA RID: 21210 RVA: 0x002495C1 File Offset: 0x002477C1
			// (set) Token: 0x060052DB RID: 21211 RVA: 0x002495C9 File Offset: 0x002477C9
			public int Current
			{
				get
				{
					return this.cur;
				}
				set
				{
					this.Set(value);
				}
			}

			// Token: 0x060052DC RID: 21212 RVA: 0x002495D2 File Offset: 0x002477D2
			public void Set(int value)
			{
				if (value < this.min)
				{
					value = this.min;
				}
				if (value > this.max)
				{
					value = this.max;
				}
				this.cur = value;
			}

			// Token: 0x060052DD RID: 21213 RVA: 0x002495FD File Offset: 0x002477FD
			public void Reset()
			{
				this.cur = this.min;
			}

			// Token: 0x04005BA4 RID: 23460
			private readonly int min;

			// Token: 0x04005BA5 RID: 23461
			private readonly int max;

			// Token: 0x04005BA6 RID: 23462
			private int cur;
		}
	}
}
