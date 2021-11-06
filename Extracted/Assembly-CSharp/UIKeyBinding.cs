using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000335 RID: 821
[AddComponentMenu("NGUI/Interaction/Key Binding")]
public class UIKeyBinding : MonoBehaviour
{
	// Token: 0x06001C52 RID: 7250 RVA: 0x00075BEC File Offset: 0x00073DEC
	public static bool IsBound(KeyCode key)
	{
		int i = 0;
		int count = UIKeyBinding.mList.Count;
		while (i < count)
		{
			UIKeyBinding uikeyBinding = UIKeyBinding.mList[i];
			if (uikeyBinding != null && uikeyBinding.keyCode == key)
			{
				return true;
			}
			i++;
		}
		return false;
	}

	// Token: 0x06001C53 RID: 7251 RVA: 0x00075C40 File Offset: 0x00073E40
	protected virtual void OnEnable()
	{
		UIKeyBinding.mList.Add(this);
	}

	// Token: 0x06001C54 RID: 7252 RVA: 0x00075C50 File Offset: 0x00073E50
	protected virtual void OnDisable()
	{
		UIKeyBinding.mList.Remove(this);
	}

	// Token: 0x06001C55 RID: 7253 RVA: 0x00075C60 File Offset: 0x00073E60
	protected virtual void Start()
	{
		UIInput component = base.GetComponent<UIInput>();
		this.mIsInput = (component != null);
		if (component != null)
		{
			EventDelegate.Add(component.onSubmit, new EventDelegate.Callback(this.OnSubmit));
		}
	}

	// Token: 0x06001C56 RID: 7254 RVA: 0x00075CA8 File Offset: 0x00073EA8
	protected virtual void OnSubmit()
	{
		if (UICamera.currentKey == this.keyCode && this.IsModifierActive())
		{
			this.mIgnoreUp = true;
		}
	}

	// Token: 0x06001C57 RID: 7255 RVA: 0x00075CD8 File Offset: 0x00073ED8
	protected virtual bool IsModifierActive()
	{
		if (this.modifier == UIKeyBinding.Modifier.Any)
		{
			return true;
		}
		if (this.modifier == UIKeyBinding.Modifier.Alt)
		{
			if (UICamera.GetKey(KeyCode.LeftAlt) || UICamera.GetKey(KeyCode.RightAlt))
			{
				return true;
			}
		}
		else if (this.modifier == UIKeyBinding.Modifier.Control)
		{
			if (UICamera.GetKey(KeyCode.LeftControl) || UICamera.GetKey(KeyCode.RightControl))
			{
				return true;
			}
		}
		else if (this.modifier == UIKeyBinding.Modifier.Shift)
		{
			if (UICamera.GetKey(KeyCode.LeftShift) || UICamera.GetKey(KeyCode.RightShift))
			{
				return true;
			}
		}
		else if (this.modifier == UIKeyBinding.Modifier.None)
		{
			return !UICamera.GetKey(KeyCode.LeftAlt) && !UICamera.GetKey(KeyCode.RightAlt) && !UICamera.GetKey(KeyCode.LeftControl) && !UICamera.GetKey(KeyCode.RightControl) && !UICamera.GetKey(KeyCode.LeftShift) && !UICamera.GetKey(KeyCode.RightShift);
		}
		return false;
	}

	// Token: 0x06001C58 RID: 7256 RVA: 0x00075E2C File Offset: 0x0007402C
	protected virtual void Update()
	{
		if (UICamera.inputHasFocus)
		{
			return;
		}
		if (this.keyCode == KeyCode.None || !this.IsModifierActive())
		{
			return;
		}
		bool flag = UICamera.GetKeyDown(this.keyCode);
		bool flag2 = UICamera.GetKeyUp(this.keyCode);
		if (flag)
		{
			this.mPress = true;
		}
		if (this.action == UIKeyBinding.Action.PressAndClick || this.action == UIKeyBinding.Action.All)
		{
			if (flag)
			{
				UICamera.currentKey = this.keyCode;
				this.OnBindingPress(true);
			}
			if (this.mPress && flag2)
			{
				UICamera.currentKey = this.keyCode;
				this.OnBindingPress(false);
				this.OnBindingClick();
			}
		}
		if ((this.action == UIKeyBinding.Action.Select || this.action == UIKeyBinding.Action.All) && flag2)
		{
			if (this.mIsInput)
			{
				if (!this.mIgnoreUp && !UICamera.inputHasFocus && this.mPress)
				{
					UICamera.selectedObject = base.gameObject;
				}
				this.mIgnoreUp = false;
			}
			else if (this.mPress)
			{
				UICamera.hoveredObject = base.gameObject;
			}
		}
		if (flag2)
		{
			this.mPress = false;
		}
	}

	// Token: 0x06001C59 RID: 7257 RVA: 0x00075F68 File Offset: 0x00074168
	protected virtual void OnBindingPress(bool pressed)
	{
		UICamera.Notify(base.gameObject, "OnPress", pressed);
	}

	// Token: 0x06001C5A RID: 7258 RVA: 0x00075F80 File Offset: 0x00074180
	protected virtual void OnBindingClick()
	{
		UICamera.Notify(base.gameObject, "OnClick", null);
	}

	// Token: 0x04001178 RID: 4472
	private static List<UIKeyBinding> mList = new List<UIKeyBinding>();

	// Token: 0x04001179 RID: 4473
	public KeyCode keyCode;

	// Token: 0x0400117A RID: 4474
	public UIKeyBinding.Modifier modifier;

	// Token: 0x0400117B RID: 4475
	public UIKeyBinding.Action action;

	// Token: 0x0400117C RID: 4476
	[NonSerialized]
	private bool mIgnoreUp;

	// Token: 0x0400117D RID: 4477
	[NonSerialized]
	private bool mIsInput;

	// Token: 0x0400117E RID: 4478
	[NonSerialized]
	private bool mPress;

	// Token: 0x02000336 RID: 822
	public enum Action
	{
		// Token: 0x04001180 RID: 4480
		PressAndClick,
		// Token: 0x04001181 RID: 4481
		Select,
		// Token: 0x04001182 RID: 4482
		All
	}

	// Token: 0x02000337 RID: 823
	public enum Modifier
	{
		// Token: 0x04001184 RID: 4484
		Any,
		// Token: 0x04001185 RID: 4485
		Shift,
		// Token: 0x04001186 RID: 4486
		Control,
		// Token: 0x04001187 RID: 4487
		Alt,
		// Token: 0x04001188 RID: 4488
		None
	}
}
