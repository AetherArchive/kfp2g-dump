using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using UnityEngine;

namespace SGNFW.uGUI
{
	public class Manager
	{
		public static Camera CameraUI
		{
			get
			{
				return Manager.cam;
			}
		}

		public static Transform Root
		{
			get
			{
				return Manager.trs;
			}
		}

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

		public static void Initialize(Transform trs, Camera cam)
		{
			if (Manager.instance != null)
			{
				return;
			}
			Manager.instance = new Manager(trs, cam);
		}

		public static void Terminate()
		{
			Manager.instance = null;
		}

		public static void BeginGroup(string group)
		{
			Manager.groupName = group;
		}

		public static void EndGroup()
		{
			Manager.groupName = null;
		}

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

		public static void Clear()
		{
			foreach (object obj in Manager.trs)
			{
				Object.Destroy(((Transform)obj).gameObject);
			}
		}

		public static GameObject Create(string path, Transform root = null, string group = null, string name = null, Layer layer = Layer.UI)
		{
			return Manager.Create(Resources.Load(path), root, group, name, layer);
		}

		public static GameObject Create(Object obj, Transform root = null, string group = null, string name = null, Layer layer = Layer.UI)
		{
			if (root == null)
			{
				root = Manager.trs;
			}
			return Manager._Create(obj, root, group, name, layer);
		}

		public static T Create<T>(string path, Transform root = null, string group = null, string name = null, Layer layer = Layer.UI) where T : MonoBehaviour
		{
			GameObject gameObject = Manager.Create(path, root, group, name, layer);
			if (gameObject == null)
			{
				return default(T);
			}
			return gameObject.GetComponent<T>();
		}

		public static T Create<T>(Object obj, Transform root = null, string group = null, string name = null, Layer layer = Layer.UI) where T : MonoBehaviour
		{
			GameObject gameObject = Manager.Create(obj, root, group, name, layer);
			if (gameObject == null)
			{
				return default(T);
			}
			return gameObject.GetComponent<T>();
		}

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

		private static Manager instance;

		private static string groupName;

		private static List<Group> groupList = new List<Group>();

		private static Transform trs;

		private static Camera cam;

		private static Dictionary<Layer, Manager.Range> currentLayerDict = new Dictionary<Layer, Manager.Range>();

		private class Range
		{
			public Range(int _min, int _max)
			{
				this.min = _min;
				this.max = _max;
				this.cur = _min;
			}

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

			public void Reset()
			{
				this.cur = this.min;
			}

			private readonly int min;

			private readonly int max;

			private int cur;
		}
	}
}
