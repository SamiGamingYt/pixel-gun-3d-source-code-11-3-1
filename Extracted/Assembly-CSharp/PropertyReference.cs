using System;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;

// Token: 0x0200036D RID: 877
[Serializable]
public class PropertyReference
{
	// Token: 0x06001E9D RID: 7837 RVA: 0x0008A0F4 File Offset: 0x000882F4
	public PropertyReference()
	{
	}

	// Token: 0x06001E9E RID: 7838 RVA: 0x0008A0FC File Offset: 0x000882FC
	public PropertyReference(Component target, string fieldName)
	{
		this.mTarget = target;
		this.mName = fieldName;
	}

	// Token: 0x1700051C RID: 1308
	// (get) Token: 0x06001EA0 RID: 7840 RVA: 0x0008A128 File Offset: 0x00088328
	// (set) Token: 0x06001EA1 RID: 7841 RVA: 0x0008A130 File Offset: 0x00088330
	public Component target
	{
		get
		{
			return this.mTarget;
		}
		set
		{
			this.mTarget = value;
			this.mProperty = null;
			this.mField = null;
		}
	}

	// Token: 0x1700051D RID: 1309
	// (get) Token: 0x06001EA2 RID: 7842 RVA: 0x0008A148 File Offset: 0x00088348
	// (set) Token: 0x06001EA3 RID: 7843 RVA: 0x0008A150 File Offset: 0x00088350
	public string name
	{
		get
		{
			return this.mName;
		}
		set
		{
			this.mName = value;
			this.mProperty = null;
			this.mField = null;
		}
	}

	// Token: 0x1700051E RID: 1310
	// (get) Token: 0x06001EA4 RID: 7844 RVA: 0x0008A168 File Offset: 0x00088368
	public bool isValid
	{
		get
		{
			return this.mTarget != null && !string.IsNullOrEmpty(this.mName);
		}
	}

	// Token: 0x1700051F RID: 1311
	// (get) Token: 0x06001EA5 RID: 7845 RVA: 0x0008A198 File Offset: 0x00088398
	public bool isEnabled
	{
		get
		{
			if (this.mTarget == null)
			{
				return false;
			}
			MonoBehaviour monoBehaviour = this.mTarget as MonoBehaviour;
			return monoBehaviour == null || monoBehaviour.enabled;
		}
	}

	// Token: 0x06001EA6 RID: 7846 RVA: 0x0008A1DC File Offset: 0x000883DC
	public Type GetPropertyType()
	{
		if (this.mProperty == null && this.mField == null && this.isValid)
		{
			this.Cache();
		}
		if (this.mProperty != null)
		{
			return this.mProperty.PropertyType;
		}
		if (this.mField != null)
		{
			return this.mField.FieldType;
		}
		return typeof(void);
	}

	// Token: 0x06001EA7 RID: 7847 RVA: 0x0008A24C File Offset: 0x0008844C
	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return !this.isValid;
		}
		if (obj is PropertyReference)
		{
			PropertyReference propertyReference = obj as PropertyReference;
			return this.mTarget == propertyReference.mTarget && string.Equals(this.mName, propertyReference.mName);
		}
		return false;
	}

	// Token: 0x06001EA8 RID: 7848 RVA: 0x0008A2A8 File Offset: 0x000884A8
	public override int GetHashCode()
	{
		return PropertyReference.s_Hash;
	}

	// Token: 0x06001EA9 RID: 7849 RVA: 0x0008A2B0 File Offset: 0x000884B0
	public void Set(Component target, string methodName)
	{
		this.mTarget = target;
		this.mName = methodName;
	}

	// Token: 0x06001EAA RID: 7850 RVA: 0x0008A2C0 File Offset: 0x000884C0
	public void Clear()
	{
		this.mTarget = null;
		this.mName = null;
	}

	// Token: 0x06001EAB RID: 7851 RVA: 0x0008A2D0 File Offset: 0x000884D0
	public void Reset()
	{
		this.mField = null;
		this.mProperty = null;
	}

	// Token: 0x06001EAC RID: 7852 RVA: 0x0008A2E0 File Offset: 0x000884E0
	public override string ToString()
	{
		return PropertyReference.ToString(this.mTarget, this.name);
	}

	// Token: 0x06001EAD RID: 7853 RVA: 0x0008A2F4 File Offset: 0x000884F4
	public static string ToString(Component comp, string property)
	{
		if (!(comp != null))
		{
			return null;
		}
		string text = comp.GetType().ToString();
		int num = text.LastIndexOf('.');
		if (num > 0)
		{
			text = text.Substring(num + 1);
		}
		if (!string.IsNullOrEmpty(property))
		{
			return text + "." + property;
		}
		return text + ".[property]";
	}

	// Token: 0x06001EAE RID: 7854 RVA: 0x0008A358 File Offset: 0x00088558
	[DebuggerHidden]
	[DebuggerStepThrough]
	public object Get()
	{
		if (this.mProperty == null && this.mField == null && this.isValid)
		{
			this.Cache();
		}
		if (this.mProperty != null)
		{
			if (this.mProperty.CanRead)
			{
				return this.mProperty.GetValue(this.mTarget, null);
			}
		}
		else if (this.mField != null)
		{
			return this.mField.GetValue(this.mTarget);
		}
		return null;
	}

	// Token: 0x06001EAF RID: 7855 RVA: 0x0008A3E0 File Offset: 0x000885E0
	[DebuggerStepThrough]
	[DebuggerHidden]
	public bool Set(object value)
	{
		if (this.mProperty == null && this.mField == null && this.isValid)
		{
			this.Cache();
		}
		if (this.mProperty == null && this.mField == null)
		{
			return false;
		}
		if (value == null)
		{
			try
			{
				if (this.mProperty == null)
				{
					this.mField.SetValue(this.mTarget, null);
					return true;
				}
				if (this.mProperty.CanWrite)
				{
					this.mProperty.SetValue(this.mTarget, null, null);
					return true;
				}
			}
			catch (Exception)
			{
				return false;
			}
		}
		if (!this.Convert(ref value))
		{
			if (Application.isPlaying)
			{
				UnityEngine.Debug.LogError(string.Concat(new object[]
				{
					"Unable to convert ",
					value.GetType(),
					" to ",
					this.GetPropertyType()
				}));
			}
		}
		else
		{
			if (this.mField != null)
			{
				this.mField.SetValue(this.mTarget, value);
				return true;
			}
			if (this.mProperty.CanWrite)
			{
				this.mProperty.SetValue(this.mTarget, value, null);
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001EB0 RID: 7856 RVA: 0x0008A54C File Offset: 0x0008874C
	[DebuggerHidden]
	[DebuggerStepThrough]
	private bool Cache()
	{
		if (this.mTarget != null && !string.IsNullOrEmpty(this.mName))
		{
			Type type = this.mTarget.GetType();
			this.mField = type.GetField(this.mName);
			this.mProperty = type.GetProperty(this.mName);
		}
		else
		{
			this.mField = null;
			this.mProperty = null;
		}
		return this.mField != null || this.mProperty != null;
	}

	// Token: 0x06001EB1 RID: 7857 RVA: 0x0008A5D8 File Offset: 0x000887D8
	private bool Convert(ref object value)
	{
		if (this.mTarget == null)
		{
			return false;
		}
		Type propertyType = this.GetPropertyType();
		Type from;
		if (value == null)
		{
			if (!propertyType.IsClass)
			{
				return false;
			}
			from = propertyType;
		}
		else
		{
			from = value.GetType();
		}
		return PropertyReference.Convert(ref value, from, propertyType);
	}

	// Token: 0x06001EB2 RID: 7858 RVA: 0x0008A62C File Offset: 0x0008882C
	public static bool Convert(Type from, Type to)
	{
		object obj = null;
		return PropertyReference.Convert(ref obj, from, to);
	}

	// Token: 0x06001EB3 RID: 7859 RVA: 0x0008A644 File Offset: 0x00088844
	public static bool Convert(object value, Type to)
	{
		if (value == null)
		{
			value = null;
			return PropertyReference.Convert(ref value, to, to);
		}
		return PropertyReference.Convert(ref value, value.GetType(), to);
	}

	// Token: 0x06001EB4 RID: 7860 RVA: 0x0008A668 File Offset: 0x00088868
	public static bool Convert(ref object value, Type from, Type to)
	{
		if (to.IsAssignableFrom(from))
		{
			return true;
		}
		if (to == typeof(string))
		{
			value = ((value == null) ? "null" : value.ToString());
			return true;
		}
		if (value == null)
		{
			return false;
		}
		float num2;
		if (to == typeof(int))
		{
			if (from == typeof(string))
			{
				int num;
				if (int.TryParse((string)value, out num))
				{
					value = num;
					return true;
				}
			}
			else if (from == typeof(float))
			{
				value = Mathf.RoundToInt((float)value);
				return true;
			}
		}
		else if (to == typeof(float) && from == typeof(string) && float.TryParse((string)value, out num2))
		{
			value = num2;
			return true;
		}
		return false;
	}

	// Token: 0x04001347 RID: 4935
	[SerializeField]
	private Component mTarget;

	// Token: 0x04001348 RID: 4936
	[SerializeField]
	private string mName;

	// Token: 0x04001349 RID: 4937
	private FieldInfo mField;

	// Token: 0x0400134A RID: 4938
	private PropertyInfo mProperty;

	// Token: 0x0400134B RID: 4939
	private static int s_Hash = "PropertyBinding".GetHashCode();
}
