using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Linq
{
	// Token: 0x020002E6 RID: 742
	public static class GameObjectExtensions
	{
		// Token: 0x060019E0 RID: 6624 RVA: 0x00068960 File Offset: 0x00066B60
		public static IEnumerable<GameObject> Ancestors(this IEnumerable<GameObject> source)
		{
			foreach (GameObject item in source)
			{
				foreach (GameObject item2 in item.Ancestors())
				{
					yield return item2;
				}
			}
			yield break;
		}

		// Token: 0x060019E1 RID: 6625 RVA: 0x0006898C File Offset: 0x00066B8C
		public static IEnumerable<GameObject> Ancestors(this IEnumerable<GameObject> source, string name)
		{
			foreach (GameObject item in source)
			{
				foreach (GameObject item2 in item.Ancestors(name))
				{
					yield return item2;
				}
			}
			yield break;
		}

		// Token: 0x060019E2 RID: 6626 RVA: 0x000689C4 File Offset: 0x00066BC4
		public static IEnumerable<GameObject> AncestorsAndSelf(this IEnumerable<GameObject> source)
		{
			foreach (GameObject item in source)
			{
				foreach (GameObject item2 in item.AncestorsAndSelf())
				{
					yield return item2;
				}
			}
			yield break;
		}

		// Token: 0x060019E3 RID: 6627 RVA: 0x000689F0 File Offset: 0x00066BF0
		public static IEnumerable<GameObject> AncestorsAndSelf(this IEnumerable<GameObject> source, string name)
		{
			foreach (GameObject item in source)
			{
				foreach (GameObject item2 in item.AncestorsAndSelf(name))
				{
					yield return item2;
				}
			}
			yield break;
		}

		// Token: 0x060019E4 RID: 6628 RVA: 0x00068A28 File Offset: 0x00066C28
		public static IEnumerable<GameObject> Descendants(this IEnumerable<GameObject> source)
		{
			foreach (GameObject item in source)
			{
				foreach (GameObject item2 in item.Descendants())
				{
					yield return item2;
				}
			}
			yield break;
		}

		// Token: 0x060019E5 RID: 6629 RVA: 0x00068A54 File Offset: 0x00066C54
		public static IEnumerable<GameObject> Descendants(this IEnumerable<GameObject> source, string name)
		{
			foreach (GameObject item in source)
			{
				foreach (GameObject item2 in item.Descendants(name))
				{
					yield return item2;
				}
			}
			yield break;
		}

		// Token: 0x060019E6 RID: 6630 RVA: 0x00068A8C File Offset: 0x00066C8C
		public static IEnumerable<GameObject> DescendantsAndSelf(this IEnumerable<GameObject> source)
		{
			foreach (GameObject item in source)
			{
				foreach (GameObject item2 in item.DescendantsAndSelf())
				{
					yield return item2;
				}
			}
			yield break;
		}

		// Token: 0x060019E7 RID: 6631 RVA: 0x00068AB8 File Offset: 0x00066CB8
		public static IEnumerable<GameObject> DescendantsAndSelf(this IEnumerable<GameObject> source, string name)
		{
			foreach (GameObject item in source)
			{
				foreach (GameObject item2 in item.DescendantsAndSelf(name))
				{
					yield return item2;
				}
			}
			yield break;
		}

		// Token: 0x060019E8 RID: 6632 RVA: 0x00068AF0 File Offset: 0x00066CF0
		public static IEnumerable<GameObject> Children(this IEnumerable<GameObject> source)
		{
			foreach (GameObject item in source)
			{
				foreach (GameObject item2 in item.Children())
				{
					yield return item2;
				}
			}
			yield break;
		}

		// Token: 0x060019E9 RID: 6633 RVA: 0x00068B1C File Offset: 0x00066D1C
		public static IEnumerable<GameObject> Children(this IEnumerable<GameObject> source, string name)
		{
			foreach (GameObject item in source)
			{
				foreach (GameObject item2 in item.Children(name))
				{
					yield return item2;
				}
			}
			yield break;
		}

		// Token: 0x060019EA RID: 6634 RVA: 0x00068B54 File Offset: 0x00066D54
		public static IEnumerable<GameObject> ChildrenAndSelf(this IEnumerable<GameObject> source)
		{
			foreach (GameObject item in source)
			{
				foreach (GameObject item2 in item.ChildrenAndSelf())
				{
					yield return item2;
				}
			}
			yield break;
		}

		// Token: 0x060019EB RID: 6635 RVA: 0x00068B80 File Offset: 0x00066D80
		public static IEnumerable<GameObject> ChildrenAndSelf(this IEnumerable<GameObject> source, string name)
		{
			foreach (GameObject item in source)
			{
				foreach (GameObject item2 in item.ChildrenAndSelf(name))
				{
					yield return item2;
				}
			}
			yield break;
		}

		// Token: 0x060019EC RID: 6636 RVA: 0x00068BB8 File Offset: 0x00066DB8
		public static void Destroy(this IEnumerable<GameObject> source, bool useDestroyImmediate = false)
		{
			foreach (GameObject self in new List<GameObject>(source))
			{
				self.Destroy(useDestroyImmediate);
			}
		}

		// Token: 0x060019ED RID: 6637 RVA: 0x00068C20 File Offset: 0x00066E20
		public static IEnumerable<T> OfComponent<T>(this IEnumerable<GameObject> source) where T : Component
		{
			foreach (GameObject item in source)
			{
				T component = item.GetComponent<T>();
				if (component != null)
				{
					yield return component;
				}
			}
			yield break;
		}

		// Token: 0x060019EE RID: 6638 RVA: 0x00068C4C File Offset: 0x00066E4C
		public static GameObject Add(this GameObject parent, GameObject childOriginal, TransformCloneType cloneType = TransformCloneType.KeepOriginal, bool? setActive = null, string specifiedName = null)
		{
			if (parent == null)
			{
				throw new ArgumentNullException("parent");
			}
			if (childOriginal == null)
			{
				throw new ArgumentNullException("childOriginal");
			}
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(childOriginal);
			Transform transform = gameObject.transform;
			RectTransform rectTransform = transform as RectTransform;
			if (rectTransform != null)
			{
				rectTransform.SetParent(parent.transform, false);
			}
			else
			{
				Transform transform2 = parent.transform;
				transform.parent = transform2;
				switch (cloneType)
				{
				case TransformCloneType.KeepOriginal:
				{
					Transform transform3 = childOriginal.transform;
					transform.localPosition = transform3.localPosition;
					transform.localScale = transform3.localScale;
					transform.localRotation = transform3.localRotation;
					break;
				}
				case TransformCloneType.FollowParent:
					transform.localPosition = transform2.localPosition;
					transform.localScale = transform2.localScale;
					transform.localRotation = transform2.localRotation;
					break;
				case TransformCloneType.Origin:
					transform.localPosition = Vector3.zero;
					transform.localScale = Vector3.one;
					transform.localRotation = Quaternion.identity;
					break;
				}
			}
			gameObject.layer = parent.layer;
			if (setActive != null)
			{
				gameObject.SetActive(setActive.Value);
			}
			if (specifiedName != null)
			{
				gameObject.name = specifiedName;
			}
			return gameObject;
		}

		// Token: 0x060019EF RID: 6639 RVA: 0x00068DA4 File Offset: 0x00066FA4
		public static List<GameObject> Add(this GameObject parent, IEnumerable<GameObject> childOriginals, TransformCloneType cloneType = TransformCloneType.KeepOriginal, bool? setActive = null, string specifiedName = null)
		{
			if (parent == null)
			{
				throw new ArgumentNullException("parent");
			}
			if (childOriginals == null)
			{
				throw new ArgumentNullException("childOriginals");
			}
			List<GameObject> list = new List<GameObject>();
			foreach (GameObject childOriginal in childOriginals)
			{
				GameObject item = parent.Add(childOriginal, cloneType, setActive, specifiedName);
				list.Add(item);
			}
			return list;
		}

		// Token: 0x060019F0 RID: 6640 RVA: 0x00068E40 File Offset: 0x00067040
		public static GameObject AddFirst(this GameObject parent, GameObject childOriginal, TransformCloneType cloneType = TransformCloneType.KeepOriginal, bool? setActive = null, string specifiedName = null)
		{
			GameObject gameObject = parent.Add(childOriginal, cloneType, setActive, specifiedName);
			gameObject.transform.SetAsFirstSibling();
			return gameObject;
		}

		// Token: 0x060019F1 RID: 6641 RVA: 0x00068E68 File Offset: 0x00067068
		public static List<GameObject> AddFirst(this GameObject parent, IEnumerable<GameObject> childOriginals, TransformCloneType cloneType = TransformCloneType.KeepOriginal, bool? setActive = null, string specifiedName = null)
		{
			List<GameObject> list = parent.Add(childOriginals, cloneType, setActive, specifiedName);
			for (int i = list.Count - 1; i >= 0; i--)
			{
				list[i].transform.SetAsFirstSibling();
			}
			return list;
		}

		// Token: 0x060019F2 RID: 6642 RVA: 0x00068EAC File Offset: 0x000670AC
		public static GameObject AddBeforeSelf(this GameObject parent, GameObject childOriginal, TransformCloneType cloneType = TransformCloneType.KeepOriginal, bool? setActive = null, string specifiedName = null)
		{
			GameObject gameObject = parent.Parent();
			if (gameObject == null)
			{
				throw new InvalidOperationException("The parent root is null");
			}
			int siblingIndex = parent.transform.GetSiblingIndex();
			GameObject gameObject2 = gameObject.Add(childOriginal, cloneType, setActive, specifiedName);
			gameObject2.transform.SetSiblingIndex(siblingIndex);
			return gameObject2;
		}

		// Token: 0x060019F3 RID: 6643 RVA: 0x00068EFC File Offset: 0x000670FC
		public static List<GameObject> AddBeforeSelf(this GameObject parent, IEnumerable<GameObject> childOriginals, TransformCloneType cloneType = TransformCloneType.KeepOriginal, bool? setActive = null, string specifiedName = null)
		{
			GameObject gameObject = parent.Parent();
			if (gameObject == null)
			{
				throw new InvalidOperationException("The parent root is null");
			}
			int siblingIndex = parent.transform.GetSiblingIndex();
			List<GameObject> list = gameObject.Add(childOriginals, cloneType, setActive, specifiedName);
			for (int i = list.Count - 1; i >= 0; i--)
			{
				list[i].transform.SetSiblingIndex(siblingIndex);
			}
			return list;
		}

		// Token: 0x060019F4 RID: 6644 RVA: 0x00068F6C File Offset: 0x0006716C
		public static GameObject AddAfterSelf(this GameObject parent, GameObject childOriginal, TransformCloneType cloneType = TransformCloneType.KeepOriginal, bool? setActive = null, string specifiedName = null)
		{
			GameObject gameObject = parent.Parent();
			if (gameObject == null)
			{
				throw new InvalidOperationException("The parent root is null");
			}
			int siblingIndex = parent.transform.GetSiblingIndex() + 1;
			GameObject gameObject2 = gameObject.Add(childOriginal, cloneType, setActive, specifiedName);
			gameObject2.transform.SetSiblingIndex(siblingIndex);
			return gameObject2;
		}

		// Token: 0x060019F5 RID: 6645 RVA: 0x00068FC0 File Offset: 0x000671C0
		public static List<GameObject> AddAfterSelf(this GameObject parent, IEnumerable<GameObject> childOriginals, TransformCloneType cloneType = TransformCloneType.KeepOriginal, bool? setActive = null, string specifiedName = null)
		{
			GameObject gameObject = parent.Parent();
			if (gameObject == null)
			{
				throw new InvalidOperationException("The parent root is null");
			}
			int siblingIndex = parent.transform.GetSiblingIndex() + 1;
			List<GameObject> list = gameObject.Add(childOriginals, cloneType, setActive, specifiedName);
			for (int i = list.Count - 1; i >= 0; i--)
			{
				list[i].transform.SetSiblingIndex(siblingIndex);
			}
			return list;
		}

		// Token: 0x060019F6 RID: 6646 RVA: 0x00069034 File Offset: 0x00067234
		public static GameObject MoveToLast(this GameObject parent, GameObject child, TransformMoveType moveType = TransformMoveType.DoNothing, bool? setActive = null)
		{
			if (parent == null)
			{
				throw new ArgumentNullException("parent");
			}
			if (child == null)
			{
				throw new ArgumentNullException("child");
			}
			Transform transform = child.transform;
			RectTransform rectTransform = transform as RectTransform;
			if (rectTransform != null)
			{
				rectTransform.SetParent(parent.transform, false);
			}
			else
			{
				Transform transform2 = parent.transform;
				transform.parent = transform2;
				switch (moveType)
				{
				case TransformMoveType.FollowParent:
					transform.localPosition = transform2.localPosition;
					transform.localScale = transform2.localScale;
					transform.localRotation = transform2.localRotation;
					break;
				case TransformMoveType.Origin:
					transform.localPosition = Vector3.zero;
					transform.localScale = Vector3.one;
					transform.localRotation = Quaternion.identity;
					break;
				}
			}
			child.layer = parent.layer;
			if (setActive != null)
			{
				child.SetActive(setActive.Value);
			}
			return child;
		}

		// Token: 0x060019F7 RID: 6647 RVA: 0x0006913C File Offset: 0x0006733C
		public static List<GameObject> MoveToLast(this GameObject parent, IEnumerable<GameObject> childs, TransformMoveType moveType = TransformMoveType.DoNothing, bool? setActive = null)
		{
			if (parent == null)
			{
				throw new ArgumentNullException("parent");
			}
			if (childs == null)
			{
				throw new ArgumentNullException("childs");
			}
			List<GameObject> list = new List<GameObject>();
			foreach (GameObject child in childs)
			{
				GameObject item = parent.MoveToLast(child, moveType, null);
				list.Add(item);
			}
			return list;
		}

		// Token: 0x060019F8 RID: 6648 RVA: 0x000691DC File Offset: 0x000673DC
		public static GameObject MoveToFirst(this GameObject parent, GameObject child, TransformMoveType moveType = TransformMoveType.DoNothing, bool? setActive = null)
		{
			parent.MoveToLast(child, moveType, setActive);
			child.transform.SetAsFirstSibling();
			return child;
		}

		// Token: 0x060019F9 RID: 6649 RVA: 0x000691F4 File Offset: 0x000673F4
		public static List<GameObject> MoveToFirst(this GameObject parent, IEnumerable<GameObject> childs, TransformMoveType moveType = TransformMoveType.DoNothing, bool? setActive = null)
		{
			List<GameObject> list = parent.MoveToLast(childs, moveType, setActive);
			for (int i = list.Count - 1; i >= 0; i--)
			{
				list[i].transform.SetAsFirstSibling();
			}
			return list;
		}

		// Token: 0x060019FA RID: 6650 RVA: 0x00069238 File Offset: 0x00067438
		public static GameObject MoveToBeforeSelf(this GameObject parent, GameObject child, TransformMoveType moveType = TransformMoveType.DoNothing, bool? setActive = null)
		{
			GameObject gameObject = parent.Parent();
			if (gameObject == null)
			{
				throw new InvalidOperationException("The parent root is null");
			}
			int siblingIndex = parent.transform.GetSiblingIndex();
			gameObject.MoveToLast(child, moveType, setActive);
			child.transform.SetSiblingIndex(siblingIndex);
			return child;
		}

		// Token: 0x060019FB RID: 6651 RVA: 0x00069288 File Offset: 0x00067488
		public static List<GameObject> MoveToBeforeSelf(this GameObject parent, IEnumerable<GameObject> childs, TransformMoveType moveType = TransformMoveType.DoNothing, bool? setActive = null)
		{
			GameObject gameObject = parent.Parent();
			if (gameObject == null)
			{
				throw new InvalidOperationException("The parent root is null");
			}
			int siblingIndex = parent.transform.GetSiblingIndex();
			List<GameObject> list = gameObject.MoveToLast(childs, moveType, setActive);
			for (int i = list.Count - 1; i >= 0; i--)
			{
				list[i].transform.SetSiblingIndex(siblingIndex);
			}
			return list;
		}

		// Token: 0x060019FC RID: 6652 RVA: 0x000692F8 File Offset: 0x000674F8
		public static GameObject MoveToAfterSelf(this GameObject parent, GameObject child, TransformMoveType moveType = TransformMoveType.DoNothing, bool? setActive = null)
		{
			GameObject gameObject = parent.Parent();
			if (gameObject == null)
			{
				throw new InvalidOperationException("The parent root is null");
			}
			int siblingIndex = parent.transform.GetSiblingIndex() + 1;
			gameObject.MoveToLast(child, moveType, setActive);
			child.transform.SetSiblingIndex(siblingIndex);
			return child;
		}

		// Token: 0x060019FD RID: 6653 RVA: 0x00069348 File Offset: 0x00067548
		public static List<GameObject> MoveToAfterSelf(this GameObject parent, IEnumerable<GameObject> childs, TransformMoveType moveType = TransformMoveType.DoNothing, bool? setActive = null)
		{
			GameObject gameObject = parent.Parent();
			if (gameObject == null)
			{
				throw new InvalidOperationException("The parent root is null");
			}
			int siblingIndex = parent.transform.GetSiblingIndex() + 1;
			List<GameObject> list = gameObject.MoveToLast(childs, moveType, setActive);
			for (int i = list.Count - 1; i >= 0; i--)
			{
				list[i].transform.SetSiblingIndex(siblingIndex);
			}
			return list;
		}

		// Token: 0x060019FE RID: 6654 RVA: 0x000693B8 File Offset: 0x000675B8
		public static void Destroy(this GameObject self, bool useDestroyImmediate = false)
		{
			if (self == null)
			{
				return;
			}
			self.SetActive(false);
			self.transform.parent = null;
			self.transform.SetParent(null);
			if (useDestroyImmediate)
			{
				UnityEngine.Object.DestroyImmediate(self);
			}
			else
			{
				UnityEngine.Object.Destroy(self);
			}
		}

		// Token: 0x060019FF RID: 6655 RVA: 0x00069408 File Offset: 0x00067608
		public static GameObject Parent(this GameObject origin)
		{
			if (origin == null)
			{
				return null;
			}
			Transform parent = origin.transform.parent;
			if (parent == null)
			{
				return null;
			}
			return parent.gameObject;
		}

		// Token: 0x06001A00 RID: 6656 RVA: 0x00069444 File Offset: 0x00067644
		public static GameObject Child(this GameObject origin, string name)
		{
			if (origin == null)
			{
				return null;
			}
			Transform transform = origin.transform.FindChild(name);
			if (transform == null)
			{
				return null;
			}
			return transform.gameObject;
		}

		// Token: 0x06001A01 RID: 6657 RVA: 0x00069480 File Offset: 0x00067680
		public static IEnumerable<GameObject> Children(this GameObject origin)
		{
			return origin.ChildrenCore(null, false);
		}

		// Token: 0x06001A02 RID: 6658 RVA: 0x0006948C File Offset: 0x0006768C
		public static IEnumerable<GameObject> Children(this GameObject origin, string name)
		{
			return origin.ChildrenCore(name, false);
		}

		// Token: 0x06001A03 RID: 6659 RVA: 0x00069498 File Offset: 0x00067698
		public static IEnumerable<GameObject> ChildrenAndSelf(this GameObject origin)
		{
			return origin.ChildrenCore(null, true);
		}

		// Token: 0x06001A04 RID: 6660 RVA: 0x000694A4 File Offset: 0x000676A4
		public static IEnumerable<GameObject> ChildrenAndSelf(this GameObject origin, string name)
		{
			return origin.ChildrenCore(name, true);
		}

		// Token: 0x06001A05 RID: 6661 RVA: 0x000694B0 File Offset: 0x000676B0
		private static IEnumerable<GameObject> ChildrenCore(this GameObject origin, string nameFilter, bool withSelf)
		{
			if (origin == null)
			{
				yield break;
			}
			if (withSelf && (nameFilter == null || origin.name == nameFilter))
			{
				yield return origin;
			}
			foreach (object obj in origin.transform)
			{
				Transform child = (Transform)obj;
				if (nameFilter == null || child.name == nameFilter)
				{
					yield return child.gameObject;
				}
			}
			yield break;
		}

		// Token: 0x06001A06 RID: 6662 RVA: 0x000694F8 File Offset: 0x000676F8
		public static IEnumerable<GameObject> Ancestors(this GameObject origin)
		{
			return GameObjectExtensions.AncestorsCore(origin, null, false);
		}

		// Token: 0x06001A07 RID: 6663 RVA: 0x00069504 File Offset: 0x00067704
		public static IEnumerable<GameObject> Ancestors(this GameObject origin, string name)
		{
			return GameObjectExtensions.AncestorsCore(origin, null, false);
		}

		// Token: 0x06001A08 RID: 6664 RVA: 0x00069510 File Offset: 0x00067710
		public static IEnumerable<GameObject> AncestorsAndSelf(this GameObject origin)
		{
			return GameObjectExtensions.AncestorsCore(origin, null, true);
		}

		// Token: 0x06001A09 RID: 6665 RVA: 0x0006951C File Offset: 0x0006771C
		public static IEnumerable<GameObject> AncestorsAndSelf(this GameObject origin, string name)
		{
			return GameObjectExtensions.AncestorsCore(origin, name, true);
		}

		// Token: 0x06001A0A RID: 6666 RVA: 0x00069528 File Offset: 0x00067728
		private static IEnumerable<GameObject> AncestorsCore(GameObject origin, string nameFilter, bool withSelf)
		{
			if (origin == null)
			{
				yield break;
			}
			if (withSelf && (nameFilter == null || origin.name == nameFilter))
			{
				yield return origin;
			}
			Transform parentTransform = origin.transform.parent;
			while (parentTransform != null)
			{
				if (nameFilter == null || parentTransform.name == nameFilter)
				{
					yield return parentTransform.gameObject;
				}
				parentTransform = parentTransform.parent;
			}
			yield break;
		}

		// Token: 0x06001A0B RID: 6667 RVA: 0x00069570 File Offset: 0x00067770
		public static IEnumerable<GameObject> Descendants(this GameObject origin)
		{
			return origin.DescendantsCore(null, false);
		}

		// Token: 0x06001A0C RID: 6668 RVA: 0x0006957C File Offset: 0x0006777C
		public static IEnumerable<GameObject> Descendants(this GameObject origin, string name)
		{
			return origin.DescendantsCore(name, false);
		}

		// Token: 0x06001A0D RID: 6669 RVA: 0x00069588 File Offset: 0x00067788
		public static IEnumerable<GameObject> DescendantsAndSelf(this GameObject origin)
		{
			return origin.DescendantsCore(null, true);
		}

		// Token: 0x06001A0E RID: 6670 RVA: 0x00069594 File Offset: 0x00067794
		public static IEnumerable<GameObject> DescendantsAndSelf(this GameObject origin, string name)
		{
			return origin.DescendantsCore(name, true);
		}

		// Token: 0x06001A0F RID: 6671 RVA: 0x000695A0 File Offset: 0x000677A0
		private static IEnumerable<GameObject> DescendantsCore(this GameObject origin, string nameFilter, bool withSelf)
		{
			if (origin == null)
			{
				yield break;
			}
			if (withSelf && (nameFilter == null || origin.name == nameFilter))
			{
				yield return origin;
			}
			foreach (object obj in origin.transform)
			{
				Transform item = (Transform)obj;
				foreach (GameObject child in item.gameObject.DescendantsCore(nameFilter, true))
				{
					if (nameFilter == null || child.name == nameFilter)
					{
						yield return child.gameObject;
					}
				}
			}
			yield break;
		}

		// Token: 0x06001A10 RID: 6672 RVA: 0x000695E8 File Offset: 0x000677E8
		public static IEnumerable<GameObject> BeforeSelf(this GameObject origin)
		{
			return origin.BeforeSelfCore(null, false);
		}

		// Token: 0x06001A11 RID: 6673 RVA: 0x000695F4 File Offset: 0x000677F4
		public static IEnumerable<GameObject> BeforeSelf(this GameObject origin, string name)
		{
			return origin.BeforeSelfCore(name, false);
		}

		// Token: 0x06001A12 RID: 6674 RVA: 0x00069600 File Offset: 0x00067800
		public static IEnumerable<GameObject> BeforeSelfAndSelf(this GameObject origin)
		{
			return origin.BeforeSelfCore(null, true);
		}

		// Token: 0x06001A13 RID: 6675 RVA: 0x0006960C File Offset: 0x0006780C
		public static IEnumerable<GameObject> BeforeSelfAndSelf(this GameObject origin, string name)
		{
			return origin.BeforeSelfCore(name, true);
		}

		// Token: 0x06001A14 RID: 6676 RVA: 0x00069618 File Offset: 0x00067818
		private static IEnumerable<GameObject> BeforeSelfCore(this GameObject origin, string nameFilter, bool withSelf)
		{
			if (origin == null)
			{
				yield break;
			}
			Transform parent = origin.transform.parent;
			if (!(parent == null))
			{
				foreach (object obj in parent.transform)
				{
					Transform item = (Transform)obj;
					GameObject go = item.gameObject;
					if (go == origin)
					{
						break;
					}
					if (nameFilter == null || item.name == nameFilter)
					{
						yield return go;
					}
				}
			}
			if (withSelf && (nameFilter == null || origin.name == nameFilter))
			{
				yield return origin;
			}
			yield break;
		}

		// Token: 0x06001A15 RID: 6677 RVA: 0x00069660 File Offset: 0x00067860
		public static IEnumerable<GameObject> AfterSelf(this GameObject origin)
		{
			return origin.AfterSelfCore(null, false);
		}

		// Token: 0x06001A16 RID: 6678 RVA: 0x0006966C File Offset: 0x0006786C
		public static IEnumerable<GameObject> AfterSelf(this GameObject origin, string name)
		{
			return origin.AfterSelfCore(name, false);
		}

		// Token: 0x06001A17 RID: 6679 RVA: 0x00069678 File Offset: 0x00067878
		public static IEnumerable<GameObject> AfterSelfAndSelf(this GameObject origin)
		{
			return origin.AfterSelfCore(null, true);
		}

		// Token: 0x06001A18 RID: 6680 RVA: 0x00069684 File Offset: 0x00067884
		public static IEnumerable<GameObject> AfterSelfAndSelf(this GameObject origin, string name)
		{
			return origin.AfterSelfCore(name, true);
		}

		// Token: 0x06001A19 RID: 6681 RVA: 0x00069690 File Offset: 0x00067890
		private static IEnumerable<GameObject> AfterSelfCore(this GameObject origin, string nameFilter, bool withSelf)
		{
			if (origin == null)
			{
				yield break;
			}
			if (withSelf && (nameFilter == null || origin.name == nameFilter))
			{
				yield return origin;
			}
			Transform parent = origin.transform.parent;
			if (parent == null)
			{
				yield break;
			}
			int index = origin.transform.GetSiblingIndex() + 1;
			Transform parentTransform = parent.transform;
			int count = parentTransform.childCount;
			while (index < count)
			{
				GameObject target = parentTransform.GetChild(index).gameObject;
				if (nameFilter == null || target.name == nameFilter)
				{
					yield return target;
				}
				index++;
			}
			yield break;
		}
	}
}
