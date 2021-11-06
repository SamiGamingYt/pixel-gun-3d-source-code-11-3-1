using System;
using System.Reflection;
using UnityEngine;

// Token: 0x02000007 RID: 7
internal sealed class AddCamFX : MonoBehaviour
{
	// Token: 0x0600001F RID: 31 RVA: 0x00002D24 File Offset: 0x00000F24
	private void Start()
	{
	}

	// Token: 0x06000020 RID: 32 RVA: 0x00002D34 File Offset: 0x00000F34
	private Component CopyComponent(Component original, GameObject destination)
	{
		Type type = original.GetType();
		Component component = destination.AddComponent(type);
		FieldInfo[] fields = type.GetFields();
		foreach (FieldInfo fieldInfo in fields)
		{
			fieldInfo.SetValue(component, fieldInfo.GetValue(original));
		}
		return component;
	}
}
