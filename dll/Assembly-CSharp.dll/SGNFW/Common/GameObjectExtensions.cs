using System;
using System.Collections.Generic;
using UnityEngine;

namespace SGNFW.Common
{
	public static class GameObjectExtensions
	{
		public static void ResetLocalTransform(this GameObject self)
		{
			self.transform.ResetLocalTransform();
		}

		public static bool HasChild(this GameObject self)
		{
			return self.transform.HasChild();
		}

		public static bool HasParent(this GameObject self)
		{
			return self.transform.parent != null;
		}

		public static void SetParent(this GameObject self, GameObject parent)
		{
			self.SetParent((parent != null) ? parent.transform : null);
		}

		public static void SetParent(this GameObject self, Transform parent)
		{
			self.transform.SetParent(parent, true);
		}

		public static string GetHierarchyPath(this GameObject self)
		{
			if (!self.HasParent())
			{
				return self.name;
			}
			return self.transform.parent.gameObject.GetHierarchyPath() + "/" + self.name;
		}

		public static string GetHierarchyPath(this GameObject self, GameObject rootPath)
		{
			if (!self.HasParent() || !(rootPath != self.transform.parent.gameObject))
			{
				return self.name;
			}
			return self.transform.parent.gameObject.GetHierarchyPath(rootPath) + "/" + self.name;
		}

		public static int GetChildCount(this GameObject self)
		{
			return self.transform.childCount;
		}

		public static GameObject GetChildGameObject(this GameObject self, int index)
		{
			if (index < 0 || index >= self.GetChildCount())
			{
				return null;
			}
			return self.transform.GetChild(index).gameObject;
		}

		public static GameObject[] GetChildren(this GameObject self)
		{
			return self.GetChildList().ToArray();
		}

		public static List<GameObject> GetChildList(this GameObject self)
		{
			List<GameObject> list = new List<GameObject>();
			for (int i = 0; i < self.GetChildCount(); i++)
			{
				list.Add(self.GetChildGameObject(i));
			}
			return list;
		}

		public static List<string> GetNamesInChildren(this GameObject self, bool recursively)
		{
			List<string> list = new List<string>();
			GameObjectExtensions._GetNamesInChildren(self, list, recursively, false, null);
			return list;
		}

		public static List<GameObject> GetChildListRecursively(this GameObject self)
		{
			List<GameObject> list = new List<GameObject>();
			GameObjectExtensions._GetChildListRecrusively(self, list);
			return list;
		}

		public static bool HasComponent<Type>(this GameObject self) where Type : Component
		{
			return self.GetComponent<Type>() != null;
		}

		public static Type AddComponentOnce<Type>(this GameObject self) where Type : Component
		{
			Type type = self.GetComponent<Type>();
			if (type == null)
			{
				type = self.AddComponent<Type>();
			}
			return type;
		}

		public static Component AddcomponentOnce(this GameObject self, Type type)
		{
			Component component = self.GetComponent(type);
			if (component == null)
			{
				component = self.AddComponent(type);
			}
			return component;
		}

		public static bool IsRoot(this GameObject self)
		{
			return self.transform.IsRoot();
		}

		public static GameObject GetRootObject(this GameObject self)
		{
			if (self.IsRoot())
			{
				return self;
			}
			return self.transform.GetRootTransform().gameObject;
		}

		public static GameObject GetParent(this GameObject self)
		{
			if (self.HasParent())
			{
				return self.transform.parent.gameObject;
			}
			return null;
		}

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

		public static void SetLayer(this GameObject self, int layer)
		{
			self.layer = layer;
		}

		public static void SetLayer(this GameObject self, string layerName)
		{
			self.SetLayer(LayerMask.NameToLayer(layerName));
		}

		public static void SetLayerRecrusively(this GameObject self, int layer)
		{
			GameObjectExtensions.DoActionRecrusively(self, delegate(GameObject go)
			{
				go.layer = layer;
			});
		}

		public static GameObject AddEmptyChild(this GameObject self, string childName = "empty")
		{
			GameObject gameObject = new GameObject(childName);
			gameObject.SetParent(self);
			gameObject.ResetLocalTransform();
			return gameObject;
		}

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

		private static void _GetChildListRecrusively(GameObject node, List<GameObject> list)
		{
			list.Add(node);
			for (int i = 0; i < node.GetChildCount(); i++)
			{
				GameObjectExtensions._GetChildListRecrusively(node.GetChildGameObject(i), list);
			}
		}

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

		public delegate void GameObjectDelegate(GameObject go);
	}
}
