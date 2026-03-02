using System;
using System.Collections.Generic;
using UnityEngine;

namespace SGNFW.Common
{
	// Token: 0x02000258 RID: 600
	public static class GameObjectExtensions
	{
		// Token: 0x06002594 RID: 9620 RVA: 0x0019FE0B File Offset: 0x0019E00B
		public static void ResetLocalTransform(this GameObject self)
		{
			self.transform.ResetLocalTransform();
		}

		// Token: 0x06002595 RID: 9621 RVA: 0x0019FE18 File Offset: 0x0019E018
		public static bool HasChild(this GameObject self)
		{
			return self.transform.HasChild();
		}

		// Token: 0x06002596 RID: 9622 RVA: 0x0019FE25 File Offset: 0x0019E025
		public static bool HasParent(this GameObject self)
		{
			return self.transform.parent != null;
		}

		// Token: 0x06002597 RID: 9623 RVA: 0x0019FE38 File Offset: 0x0019E038
		public static void SetParent(this GameObject self, GameObject parent)
		{
			self.SetParent((parent != null) ? parent.transform : null);
		}

		// Token: 0x06002598 RID: 9624 RVA: 0x0019FE52 File Offset: 0x0019E052
		public static void SetParent(this GameObject self, Transform parent)
		{
			self.transform.SetParent(parent, true);
		}

		// Token: 0x06002599 RID: 9625 RVA: 0x0019FE61 File Offset: 0x0019E061
		public static string GetHierarchyPath(this GameObject self)
		{
			if (!self.HasParent())
			{
				return self.name;
			}
			return self.transform.parent.gameObject.GetHierarchyPath() + "/" + self.name;
		}

		// Token: 0x0600259A RID: 9626 RVA: 0x0019FE98 File Offset: 0x0019E098
		public static string GetHierarchyPath(this GameObject self, GameObject rootPath)
		{
			if (!self.HasParent() || !(rootPath != self.transform.parent.gameObject))
			{
				return self.name;
			}
			return self.transform.parent.gameObject.GetHierarchyPath(rootPath) + "/" + self.name;
		}

		// Token: 0x0600259B RID: 9627 RVA: 0x0019FEF2 File Offset: 0x0019E0F2
		public static int GetChildCount(this GameObject self)
		{
			return self.transform.childCount;
		}

		// Token: 0x0600259C RID: 9628 RVA: 0x0019FEFF File Offset: 0x0019E0FF
		public static GameObject GetChildGameObject(this GameObject self, int index)
		{
			if (index < 0 || index >= self.GetChildCount())
			{
				return null;
			}
			return self.transform.GetChild(index).gameObject;
		}

		// Token: 0x0600259D RID: 9629 RVA: 0x0019FF21 File Offset: 0x0019E121
		public static GameObject[] GetChildren(this GameObject self)
		{
			return self.GetChildList().ToArray();
		}

		// Token: 0x0600259E RID: 9630 RVA: 0x0019FF30 File Offset: 0x0019E130
		public static List<GameObject> GetChildList(this GameObject self)
		{
			List<GameObject> list = new List<GameObject>();
			for (int i = 0; i < self.GetChildCount(); i++)
			{
				list.Add(self.GetChildGameObject(i));
			}
			return list;
		}

		// Token: 0x0600259F RID: 9631 RVA: 0x0019FF64 File Offset: 0x0019E164
		public static List<string> GetNamesInChildren(this GameObject self, bool recursively)
		{
			List<string> list = new List<string>();
			GameObjectExtensions._GetNamesInChildren(self, list, recursively, false, null);
			return list;
		}

		// Token: 0x060025A0 RID: 9632 RVA: 0x0019FF84 File Offset: 0x0019E184
		public static List<GameObject> GetChildListRecursively(this GameObject self)
		{
			List<GameObject> list = new List<GameObject>();
			GameObjectExtensions._GetChildListRecrusively(self, list);
			return list;
		}

		// Token: 0x060025A1 RID: 9633 RVA: 0x0019FF9F File Offset: 0x0019E19F
		public static bool HasComponent<Type>(this GameObject self) where Type : Component
		{
			return self.GetComponent<Type>() != null;
		}

		// Token: 0x060025A2 RID: 9634 RVA: 0x0019FFB4 File Offset: 0x0019E1B4
		public static Type AddComponentOnce<Type>(this GameObject self) where Type : Component
		{
			Type type = self.GetComponent<Type>();
			if (type == null)
			{
				type = self.AddComponent<Type>();
			}
			return type;
		}

		// Token: 0x060025A3 RID: 9635 RVA: 0x0019FFE0 File Offset: 0x0019E1E0
		public static Component AddcomponentOnce(this GameObject self, Type type)
		{
			Component component = self.GetComponent(type);
			if (component == null)
			{
				component = self.AddComponent(type);
			}
			return component;
		}

		// Token: 0x060025A4 RID: 9636 RVA: 0x001A0007 File Offset: 0x0019E207
		public static bool IsRoot(this GameObject self)
		{
			return self.transform.IsRoot();
		}

		// Token: 0x060025A5 RID: 9637 RVA: 0x001A0014 File Offset: 0x0019E214
		public static GameObject GetRootObject(this GameObject self)
		{
			if (self.IsRoot())
			{
				return self;
			}
			return self.transform.GetRootTransform().gameObject;
		}

		// Token: 0x060025A6 RID: 9638 RVA: 0x001A0030 File Offset: 0x0019E230
		public static GameObject GetParent(this GameObject self)
		{
			if (self.HasParent())
			{
				return self.transform.parent.gameObject;
			}
			return null;
		}

		// Token: 0x060025A7 RID: 9639 RVA: 0x001A004C File Offset: 0x0019E24C
		public static Type GetComponentInParent<Type>(this GameObject self, bool includeInactive) where Type : Component
		{
			Type type = default(Type);
			if (includeInactive)
			{
				type = self.GetComponent<Type>();
			}
			else if (self.activeSelf)
			{
				type = self.GetComponent<Type>();
			}
			if (type == null && self.HasParent())
			{
				type = self.GetParent().GetComponentInParent<Type>(includeInactive);
			}
			return type;
		}

		// Token: 0x060025A8 RID: 9640 RVA: 0x001A00A0 File Offset: 0x0019E2A0
		public static void SetLayer(this GameObject self, int layer)
		{
			self.layer = layer;
		}

		// Token: 0x060025A9 RID: 9641 RVA: 0x001A00A9 File Offset: 0x0019E2A9
		public static void SetLayer(this GameObject self, string layerName)
		{
			self.SetLayer(LayerMask.NameToLayer(layerName));
		}

		// Token: 0x060025AA RID: 9642 RVA: 0x001A00B8 File Offset: 0x0019E2B8
		public static void SetLayerRecrusively(this GameObject self, int layer)
		{
			GameObjectExtensions.DoActionRecrusively(self, delegate(GameObject go)
			{
				go.layer = layer;
			});
		}

		// Token: 0x060025AB RID: 9643 RVA: 0x001A00E4 File Offset: 0x0019E2E4
		public static GameObject AddEmptyChild(this GameObject self, string childName = "empty")
		{
			GameObject gameObject = new GameObject(childName);
			gameObject.SetParent(self);
			gameObject.ResetLocalTransform();
			return gameObject;
		}

		// Token: 0x060025AC RID: 9644 RVA: 0x001A00FC File Offset: 0x0019E2FC
		private static void _GetComponentsInParent<Type>(GameObject go, List<Type> components) where Type : Component
		{
			Type component = go.GetComponent<Type>();
			if (component != null)
			{
				components.Add(component);
			}
			if (go.HasParent())
			{
				GameObjectExtensions._GetComponentsInParent<Type>(go.transform.parent.gameObject, components);
			}
		}

		// Token: 0x060025AD RID: 9645 RVA: 0x001A0144 File Offset: 0x0019E344
		private static void _GetNamesInChildren(GameObject go, List<string> list, bool recursively, bool perfectMatch, string[] searchPattern = null)
		{
			int childCount = go.GetChildCount();
			for (int i = 0; i < childCount; i++)
			{
				GameObject childGameObject = go.GetChildGameObject(i);
				if (!(childGameObject == null))
				{
					if (searchPattern != null && searchPattern.Length != 0)
					{
						if (perfectMatch && go.name.IsMatchAny(searchPattern))
						{
							list.Add(childGameObject.name);
						}
						else if (go.name.ContainsAny(searchPattern))
						{
							list.Add(childGameObject.name);
						}
					}
					else
					{
						list.Add(childGameObject.name);
					}
					if (recursively)
					{
						GameObjectExtensions._GetNamesInChildren(childGameObject, list, recursively, perfectMatch, searchPattern);
					}
				}
			}
		}

		// Token: 0x060025AE RID: 9646 RVA: 0x001A01D8 File Offset: 0x0019E3D8
		private static void _GetChildListRecrusively(GameObject node, List<GameObject> list)
		{
			list.Add(node);
			for (int i = 0; i < node.GetChildCount(); i++)
			{
				GameObjectExtensions._GetChildListRecrusively(node.GetChildGameObject(i), list);
			}
		}

		// Token: 0x060025AF RID: 9647 RVA: 0x001A020C File Offset: 0x0019E40C
		public static void DoActionRecrusively(GameObject node, GameObjectExtensions.GameObjectDelegate action)
		{
			if (action != null)
			{
				action(node);
			}
			int childCount = node.GetChildCount();
			for (int i = 0; i < childCount; i++)
			{
				GameObjectExtensions.DoActionRecrusively(node.GetChildGameObject(i), action);
			}
		}

		// Token: 0x0200109A RID: 4250
		// (Invoke) Token: 0x0600535D RID: 21341
		public delegate void GameObjectDelegate(GameObject go);
	}
}
