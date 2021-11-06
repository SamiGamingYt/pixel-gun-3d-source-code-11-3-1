using System;
using UnityEngine;

// Token: 0x02000853 RID: 2131
public static class TWTools
{
	// Token: 0x06004D30 RID: 19760 RVA: 0x001BDBAC File Offset: 0x001BBDAC
	public static void SetActiveSelf(GameObject go, bool state)
	{
		go.SetActive(state);
	}

	// Token: 0x06004D31 RID: 19761 RVA: 0x001BDBB8 File Offset: 0x001BBDB8
	private static void Activate(Transform t)
	{
		TWTools.SetActiveSelf(t.gameObject, true);
		int i = 0;
		int childCount = t.GetChildCount();
		while (i < childCount)
		{
			Transform child = t.GetChild(i);
			if (child.gameObject.activeSelf)
			{
				return;
			}
			i++;
		}
		int j = 0;
		int childCount2 = t.GetChildCount();
		while (j < childCount2)
		{
			Transform child2 = t.GetChild(j);
			TWTools.Activate(child2);
			j++;
		}
	}

	// Token: 0x06004D32 RID: 19762 RVA: 0x001BDC30 File Offset: 0x001BBE30
	private static void Deactivate(Transform t)
	{
		TWTools.SetActiveSelf(t.gameObject, false);
	}

	// Token: 0x06004D33 RID: 19763 RVA: 0x001BDC40 File Offset: 0x001BBE40
	public static void SetActive(GameObject go, bool state)
	{
		if (state)
		{
			TWTools.Activate(go.transform);
		}
		else
		{
			TWTools.Deactivate(go.transform);
		}
	}

	// Token: 0x06004D34 RID: 19764 RVA: 0x001BDC64 File Offset: 0x001BBE64
	public static GameObject AddChild(GameObject parent)
	{
		GameObject gameObject = new GameObject();
		if (parent != null)
		{
			Transform transform = gameObject.transform;
			transform.parent = parent.transform;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
			gameObject.layer = parent.layer;
		}
		return gameObject;
	}

	// Token: 0x06004D35 RID: 19765 RVA: 0x001BDCC4 File Offset: 0x001BBEC4
	public static GameObject AddChild(GameObject parent, GameObject prefab)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
		if (gameObject != null && parent != null)
		{
			Transform transform = gameObject.transform;
			transform.parent = parent.transform;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
			gameObject.layer = parent.layer;
		}
		return gameObject;
	}

	// Token: 0x06004D36 RID: 19766 RVA: 0x001BDD34 File Offset: 0x001BBF34
	public static void Destroy(UnityEngine.Object obj)
	{
		if (obj != null)
		{
			if (Application.isPlaying)
			{
				UnityEngine.Object.Destroy(obj);
			}
			else
			{
				UnityEngine.Object.DestroyImmediate(obj);
			}
		}
	}

	// Token: 0x06004D37 RID: 19767 RVA: 0x001BDD60 File Offset: 0x001BBF60
	public static void DestroyImmediate(UnityEngine.Object obj)
	{
		if (obj != null)
		{
			if (Application.isEditor)
			{
				UnityEngine.Object.DestroyImmediate(obj);
			}
			else
			{
				UnityEngine.Object.Destroy(obj);
			}
		}
	}
}
