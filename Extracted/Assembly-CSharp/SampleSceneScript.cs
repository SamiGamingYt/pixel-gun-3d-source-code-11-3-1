using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Linq;
using UnityEngine;

// Token: 0x020002E4 RID: 740
public class SampleSceneScript : MonoBehaviour
{
	// Token: 0x060019D2 RID: 6610 RVA: 0x00068130 File Offset: 0x00066330
	private void OnGUI()
	{
		GameObject gameObject = GameObject.Find("Origin");
		if (GUILayout.Button("Parent", new GUILayoutOption[0]))
		{
			Debug.Log("------Parent");
			GameObject gameObject2 = gameObject.Parent();
			Debug.Log(gameObject2.name);
		}
		if (GUILayout.Button("Child", new GUILayoutOption[0]))
		{
			Debug.Log("------Child");
			GameObject gameObject3 = gameObject.Child("Sphere_B");
			Debug.Log(gameObject3.name);
		}
		if (GUILayout.Button("Descendants", new GUILayoutOption[0]))
		{
			Debug.Log("------Descendants");
			IEnumerable<GameObject> enumerable = gameObject.Descendants();
			foreach (GameObject gameObject4 in enumerable)
			{
				Debug.Log(gameObject4.name);
			}
		}
		if (GUILayout.Button("name filter overload", new GUILayoutOption[0]))
		{
			Debug.Log("name filter overload");
			IEnumerable<GameObject> enumerable2 = gameObject.Descendants("Group");
			foreach (GameObject gameObject5 in enumerable2)
			{
				Debug.Log(gameObject5.name);
			}
		}
		if (GUILayout.Button("OfComponent", new GUILayoutOption[0]))
		{
			Debug.Log("------OfComponent");
			IEnumerable<SphereCollider> enumerable3 = gameObject.Descendants().OfComponent<SphereCollider>();
			foreach (SphereCollider sphereCollider in enumerable3)
			{
				Debug.Log(string.Concat(new object[]
				{
					"Sphere:",
					sphereCollider.name,
					" Radius:",
					sphereCollider.radius
				}));
			}
			(from x in gameObject.Descendants()
			where x.CompareTag("foobar")
			select x).OfComponent<BoxCollider2D>();
		}
		if (GUILayout.Button("LINQ", new GUILayoutOption[0]))
		{
			Debug.Log("------LINQ");
			IEnumerable<GameObject> enumerable4 = from x in gameObject.Children()
			where x.name.EndsWith("B")
			select x;
			foreach (GameObject gameObject6 in enumerable4)
			{
				Debug.Log(gameObject6.name);
			}
		}
		if (GUILayout.Button("Add", new GUILayoutOption[0]))
		{
			gameObject.Add(new GameObject[]
			{
				new GameObject("lastChild1"),
				new GameObject("lastChild2"),
				new GameObject("lastChild3")
			}, TransformCloneType.KeepOriginal, null, null);
			gameObject.AddFirst(new GameObject[]
			{
				new GameObject("firstChild1"),
				new GameObject("firstChild2"),
				new GameObject("firstChild3")
			}, TransformCloneType.KeepOriginal, null, null);
			gameObject.AddBeforeSelf(new GameObject[]
			{
				new GameObject("beforeSelf1"),
				new GameObject("beforeSelf2"),
				new GameObject("beforeSelf3")
			}, TransformCloneType.KeepOriginal, null, null);
			gameObject.AddAfterSelf(new GameObject[]
			{
				new GameObject("afterSelf1"),
				new GameObject("afterSelf2"),
				new GameObject("afterSelf3")
			}, TransformCloneType.KeepOriginal, null, null);
			(from GameObject x in Resources.FindObjectsOfTypeAll<GameObject>()
			where x.Parent() == null && !x.name.Contains("Camera") && x.name != "Root" && x.name != string.Empty && x.name != "HandlesGO" && !x.name.StartsWith("Scene") && !x.name.Contains("Light") && !x.name.Contains("Materials")
			select x).Select(delegate(GameObject x)
			{
				Debug.Log(x.name);
				return x;
			}).Destroy(false);
		}
		if (GUILayout.Button("MoveTo", new GUILayoutOption[0]))
		{
			gameObject.MoveToLast(new GameObject[]
			{
				new GameObject("lastChild1(Original)"),
				new GameObject("lastChild2(Original)"),
				new GameObject("lastChild3(Original)")
			}, TransformMoveType.DoNothing, null);
			gameObject.MoveToFirst(new GameObject[]
			{
				new GameObject("firstChild1(Original)"),
				new GameObject("firstChild2(Original)"),
				new GameObject("firstChild3(Original)")
			}, TransformMoveType.DoNothing, null);
			gameObject.MoveToBeforeSelf(new GameObject[]
			{
				new GameObject("beforeSelf1(Original)"),
				new GameObject("beforeSelf2(Original)"),
				new GameObject("beforeSelf3(Original)")
			}, TransformMoveType.DoNothing, null);
			gameObject.MoveToAfterSelf(new GameObject[]
			{
				new GameObject("afterSelf1(Original)"),
				new GameObject("afterSelf2(Original)"),
				new GameObject("afterSelf3(Original)")
			}, TransformMoveType.DoNothing, null);
		}
		if (GUILayout.Button("Destroy", new GUILayoutOption[0]))
		{
			(from x in gameObject.transform.root.gameObject.Descendants()
			where x.name.EndsWith("(Clone)") || x.name.EndsWith("(Original)")
			select x).Select(delegate(GameObject x)
			{
				Debug.Log(x.name);
				return x;
			}).Destroy(false);
		}
	}
}
